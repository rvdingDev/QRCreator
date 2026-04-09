using System.Globalization;
using System.Windows.Data;

namespace QRCreator.Converters;

public sealed class EnumToBooleanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is null || parameter is null)
            return false;

        string enumValue = value.ToString()!;
        string targetValue = parameter.ToString()!;
        return enumValue.Equals(targetValue, StringComparison.OrdinalIgnoreCase);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is true && parameter is string enumString)
            return Enum.Parse(targetType, enumString);
        return Binding.DoNothing;
    }
}
