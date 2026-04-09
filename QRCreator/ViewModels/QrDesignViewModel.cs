using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using QRCoder;
using QRCreator.Models;
using QRCreator.Rendering;
using SkiaSharp;

namespace QRCreator.ViewModels;

public partial class QrDesignViewModel : ObservableObject
{
    private System.Timers.Timer? _debounceTimer;

    public QrDesignViewModel()
    {
        _debounceTimer = new System.Timers.Timer(300) { AutoReset = false };
        _debounceTimer.Elapsed += (_, _) =>
        {
            Application.Current.Dispatcher.Invoke(RegenerateQr);
        };

        // Generate initial preview
        Application.Current.Dispatcher.InvokeAsync(RegenerateQr, System.Windows.Threading.DispatcherPriority.Loaded);
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
    private int _cellSize = 10;

    [ObservableProperty]
    private int _cellGap = 0;

    [ObservableProperty]
    private string? _logoPath;

    [ObservableProperty]
    private double _logoSizeRatio = 0.2;

    [ObservableProperty]
    private int _exportScale = 1;

    [ObservableProperty]
    private BitmapSource? _previewBitmap;

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
    private void SelectLogo()
    {
        var dialog = new OpenFileDialog
        {
            Filter = "Image files (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp",
            Title = "Select Logo Image",
        };

        if (dialog.ShowDialog() == true)
        {
            LogoPath = dialog.FileName;
            _logoSkBitmap?.Dispose();
            _logoSkBitmap = SKBitmap.Decode(dialog.FileName);
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
    private void SavePng()
    {
        if (string.IsNullOrWhiteSpace(TargetUrl))
            return;

        var dialog = new SaveFileDialog
        {
            Filter = "PNG Image (*.png)|*.png",
            FileName = "qrcode.png",
            Title = "Save QR Code",
        };

        if (dialog.ShowDialog() == true)
        {
            try
            {
                var options = BuildOptions(CellSize * ExportScale);
                var matrix = GenerateMatrix();
                if (matrix is null) return;

                using var bitmap = QrRenderer.Render(matrix, options);
                SkiaInterop.SavePng(bitmap, dialog.FileName);
            }
            catch (Exception ex)
            {
                ShowError($"Save failed: {ex.Message}");
            }
        }
    }

    [RelayCommand]
    private void CopyToClipboard()
    {
        if (PreviewBitmap is not null)
        {
            Clipboard.SetImage((BitmapSource)PreviewBitmap);
        }
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
            using var bitmap = QrRenderer.Render(matrix, options);
            PreviewBitmap = SkiaInterop.ToBitmapSource(bitmap);
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
