using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;
using QRCreator.ViewModels;

namespace QRCreator;

public partial class MainWindow : FluentWindow
{
    private readonly QrDesignViewModel _viewModel;

    public MainWindow()
    {
        _viewModel = new QrDesignViewModel();
        DataContext = _viewModel;
        InitializeComponent();
    }

    private void ThemeToggle_Click(object sender, RoutedEventArgs e)
    {
        var currentTheme = ApplicationThemeManager.GetAppTheme();
        var newTheme = currentTheme == ApplicationTheme.Dark
            ? ApplicationTheme.Light
            : ApplicationTheme.Dark;
        ApplicationThemeManager.Apply(newTheme);
    }

    private void FgColorPreset_Click(object sender, RoutedEventArgs e)
    {
        if (sender is System.Windows.Controls.Button btn && btn.Tag is string hex)
        {
            if (TryParseHex(hex, out var color))
                _viewModel.ForegroundColor = color;
        }
    }

    private void BgColorPreset_Click(object sender, RoutedEventArgs e)
    {
        if (sender is System.Windows.Controls.Button btn && btn.Tag is string hex)
        {
            if (TryParseHex(hex, out var color))
                _viewModel.BackgroundColor = color;
        }
    }

    private static bool TryParseHex(string hex, out Color color)
    {
        color = Colors.Black;
        hex = hex.TrimStart('#');
        if (hex.Length != 6) return false;
        if (!byte.TryParse(hex[0..2], NumberStyles.HexNumber, null, out byte r)) return false;
        if (!byte.TryParse(hex[2..4], NumberStyles.HexNumber, null, out byte g)) return false;
        if (!byte.TryParse(hex[4..6], NumberStyles.HexNumber, null, out byte b)) return false;
        color = Color.FromRgb(r, g, b);
        return true;
    }
}
