using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace QRCreator.Avalonia.Converters;

public sealed class EnumToBooleanConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null || parameter is null) return false;
        return value.ToString()!.Equals(parameter.ToString()!, StringComparison.OrdinalIgnoreCase);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is true && parameter is string enumString)
            return Enum.Parse(targetType, enumString);
        return BindingOperations.DoNothing;
    }
}
