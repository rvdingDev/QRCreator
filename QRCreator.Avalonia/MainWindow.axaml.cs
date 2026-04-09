using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using QRCreator.Avalonia.ViewModels;

namespace QRCreator.Avalonia;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new QrDesignViewModel();
    }

    private void About_Click(object? sender, global::Avalonia.Interactivity.RoutedEventArgs e)
    {
        var about = new AboutWindow();
        about.ShowDialog(this);
    }

    private void ThemeToggle_Click(object? sender, global::Avalonia.Interactivity.RoutedEventArgs e)
    {
        var app = Application.Current;
        if (app is not null)
        {
            app.RequestedThemeVariant = app.RequestedThemeVariant == ThemeVariant.Dark
                ? ThemeVariant.Light
                : ThemeVariant.Dark;
        }
    }
}
