using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace QRCreator.Converters;

public sealed class ColorToHexConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Color color)
            return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
        return "#000000";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string hex && TryParseHex(hex, out var color))
            return color;
        return Colors.Black;
    }

    private static bool TryParseHex(string hex, out Color color)
    {
        color = Colors.Black;
        hex = hex.TrimStart('#');
        if (hex.Length != 6)
            return false;
        if (!byte.TryParse(hex[0..2], NumberStyles.HexNumber, null, out byte r))
            return false;
        if (!byte.TryParse(hex[2..4], NumberStyles.HexNumber, null, out byte g))
            return false;
        if (!byte.TryParse(hex[4..6], NumberStyles.HexNumber, null, out byte b))
            return false;
        color = Color.FromRgb(r, g, b);
        return true;
    }
}
