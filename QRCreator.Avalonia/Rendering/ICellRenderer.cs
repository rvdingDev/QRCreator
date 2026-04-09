using QRCreator.Avalonia.Models;
using SkiaSharp;

namespace QRCreator.Avalonia.Rendering;

public interface ICellRenderer
{
    void DrawCell(SKCanvas canvas, SKRect rect, SKPaint paint, CellShape shape, int cellSize);
    void DrawFinderModule(SKCanvas canvas, SKRect rect, SKPaint paint, FinderShape shape, int cellSize);
}
