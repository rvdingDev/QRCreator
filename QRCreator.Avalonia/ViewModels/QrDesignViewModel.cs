using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Input.Platform;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QRCoder;
using QRCreator.Avalonia.Models;
using QRCreator.Avalonia.Rendering;
using SkiaSharp;

namespace QRCreator.Avalonia.ViewModels;

public partial class QrDesignViewModel : ObservableObject
{
    private System.Timers.Timer? _debounceTimer;

    public QrDesignViewModel()
    {
        _debounceTimer = new System.Timers.Timer(300) { AutoReset = false };
        _debounceTimer.Elapsed += (_, _) =>
        {
            Dispatcher.UIThread.Invoke(RegenerateQr);
        };

        // Generate initial preview
        Dispatcher.UIThread.InvokeAsync(RegenerateQr, DispatcherPriority.Loaded);
    }

    // --- Properties ---

    [ObservableProperty]
    private string _targetUrl = "https://example.com";

    [ObservableProperty]
    private CellShape _cellShape = CellShape.Square;

    [ObservableProperty]
    private FinderShape _finderShape = FinderShape.Square;

    [ObservableProperty]
    private Color _foregroundColor = Colors.Black;

    [ObservableProperty]
    private Color _backgroundColor = Colors.White;

    [ObservableProperty]
    private int _cellSize = 30;

    [ObservableProperty]
    private int _cellGap = 0;

    [ObservableProperty]
    private string? _logoPath;

    [ObservableProperty]
    private double _logoSizeRatio = 0.2;

    [ObservableProperty]
    private int _exportScale = 1;

    [ObservableProperty]
    private Bitmap? _previewBitmap;

    [ObservableProperty]
    private string? _errorMessage;

    [ObservableProperty]
    private bool _hasError;

    private SKBitmap? _logoSkBitmap;

    // --- Property change hooks ---

    partial void OnTargetUrlChanged(string value)
    {
        // URL input uses 300ms debounce
        _debounceTimer?.Stop();
        _debounceTimer?.Start();
    }

    partial void OnCellShapeChanged(CellShape value) => RegenerateQr();
    partial void OnFinderShapeChanged(FinderShape value) => RegenerateQr();
    partial void OnForegroundColorChanged(Color value) => RegenerateQr();
    partial void OnBackgroundColorChanged(Color value) => RegenerateQr();
    partial void OnCellSizeChanged(int value) => RegenerateQr();
    partial void OnCellGapChanged(int value) => RegenerateQr();
    partial void OnLogoSizeRatioChanged(double value) => RegenerateQr();

    // --- Commands ---

    [RelayCommand]
    private async Task SelectLogoAsync()
    {
        var topLevel = TopLevel.GetTopLevel(App.MainWindow);
        if (topLevel is null) return;

        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Select Logo Image",
            AllowMultiple = false,
            FileTypeFilter = [new FilePickerFileType("Images") { Patterns = ["*.png", "*.jpg", "*.jpeg", "*.bmp"] }]
        });

        if (files.Count > 0)
        {
            var path = files[0].Path.LocalPath;
            LogoPath = path;
            _logoSkBitmap?.Dispose();
            _logoSkBitmap = SKBitmap.Decode(path);
            RegenerateQr();
        }
    }

    [RelayCommand]
    private void RemoveLogo()
    {
        LogoPath = null;
        _logoSkBitmap?.Dispose();
        _logoSkBitmap = null;
        RegenerateQr();
    }

    [RelayCommand]
    private async Task SavePngAsync()
    {
        if (string.IsNullOrWhiteSpace(TargetUrl))
            return;

        var topLevel = TopLevel.GetTopLevel(App.MainWindow);
        if (topLevel is null) return;

        var file = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Save QR Code",
            SuggestedFileName = "qrcode.png",
            FileTypeChoices = [new FilePickerFileType("PNG Image") { Patterns = ["*.png"] }]
        });

        if (file is not null)
        {
            try
            {
                var options = BuildOptions(CellSize * ExportScale);
                var matrix = GenerateMatrix();
                if (matrix is null) return;

                using var bitmap = QrRenderer.Render(matrix, options);
                SkiaInterop.SavePng(bitmap, file.Path.LocalPath);
            }
            catch (Exception ex)
            {
                ShowError($"Save failed: {ex.Message}");
            }
        }
    }

    [RelayCommand]
    private async Task CopyToClipboardAsync()
    {
        if (PreviewBitmap is null) return;

        // Avalonia clipboard doesn't directly support images like WPF
        // Save to temp file and copy file reference
        var tempPath = Path.Combine(Path.GetTempPath(), "qrcode_clipboard.png");
        var matrix = GenerateMatrix();
        if (matrix is null) return;
        var options = BuildOptions(CellSize);
        using var bitmap = QrRenderer.Render(matrix, options);
        SkiaInterop.SavePng(bitmap, tempPath);

        var topLevel = TopLevel.GetTopLevel(App.MainWindow);
        if (topLevel?.Clipboard is not null)
        {
            // TODO: Avalonia 12 clipboard doesn't easily support image/file data.
            // For now, copy the temp file path as text so user knows where the file is.
            await topLevel.Clipboard.SetValueAsync(DataFormat.Text, tempPath);
        }
    }

    [RelayCommand]
    private void SetForegroundColor(string hex)
    {
        if (Color.TryParse(hex, out var color))
            ForegroundColor = color;
    }

    [RelayCommand]
    private void SetBackgroundColor(string hex)
    {
        if (Color.TryParse(hex, out var color))
            BackgroundColor = color;
    }

    // --- Core Logic ---

    private void RegenerateQr()
    {
        if (string.IsNullOrWhiteSpace(TargetUrl))
        {
            PreviewBitmap = null;
            ClearError();
            return;
        }

        try
        {
            var matrix = GenerateMatrix();
            if (matrix is null) return;

            var options = BuildOptions(CellSize);
            using var skBitmap = QrRenderer.Render(matrix, options);
            PreviewBitmap = SkiaInterop.ToBitmap(skBitmap);
            ClearError();
        }
        catch (Exception ex)
        {
            ShowError($"QR generation failed: {ex.Message}");
        }
    }

    private bool[,]? GenerateMatrix()
    {
        try
        {
            var generator = new QRCodeGenerator();
            var data = generator.CreateQrCode(TargetUrl, QRCodeGenerator.ECCLevel.H);
            var moduleCount = data.ModuleMatrix.Count;
            var matrix = new bool[moduleCount, moduleCount];

            for (int row = 0; row < moduleCount; row++)
            {
                for (int col = 0; col < moduleCount; col++)
                {
                    matrix[row, col] = data.ModuleMatrix[row][col];
                }
            }

            return matrix;
        }
        catch (Exception ex)
        {
            ShowError($"Invalid input: {ex.Message}");
            return null;
        }
    }

    private QrRenderOptions BuildOptions(int cellSize)
    {
        return new QrRenderOptions
        {
            CellShape = CellShape,
            FinderShape = FinderShape,
            ForegroundColor = SkiaInterop.ToSkColor(ForegroundColor),
            BackgroundColor = SkiaInterop.ToSkColor(BackgroundColor),
            CellSize = cellSize,
            CellGap = CellGap,
            LogoImage = _logoSkBitmap,
            LogoSizeRatio = (float)LogoSizeRatio,
        };
    }

    private void ShowError(string message)
    {
        ErrorMessage = message;
        HasError = true;
    }

    private void ClearError()
    {
        ErrorMessage = null;
        HasError = false;
    }
}
