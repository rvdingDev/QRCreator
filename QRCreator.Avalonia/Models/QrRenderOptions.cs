using SkiaSharp;

namespace QRCreator.Avalonia.Models;

public sealed class QrRenderOptions
{
    public CellShape CellShape { get; init; } = CellShape.Square;
    public FinderShape FinderShape { get; init; } = FinderShape.Square;
    public SKColor ForegroundColor { get; init; } = SKColors.Black;
    public SKColor BackgroundColor { get; init; } = SKColors.White;
    public int CellSize { get; init; } = 10;
    public int CellGap { get; init; } = 0;
    public SKBitmap? LogoImage { get; init; }
    public float LogoSizeRatio { get; init; } = 0.2f;
}
