using System.Globalization;
using System.Windows.Data;

namespace QRCreator.Converters;

public sealed class IntToBooleanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int intValue && parameter is string paramStr && int.TryParse(paramStr, out int target))
            return intValue == target;
        return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is true && parameter is string paramStr && int.TryParse(paramStr, out int target))
            return target;
        return Binding.DoNothing;
    }
}
