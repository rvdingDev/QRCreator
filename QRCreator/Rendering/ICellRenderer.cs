using QRCreator.Models;
using SkiaSharp;

namespace QRCreator.Rendering;

public interface ICellRenderer
{
    void DrawCell(SKCanvas canvas, SKRect rect, SKPaint paint, CellShape shape, int cellSize);
    void DrawFinderModule(SKCanvas canvas, SKRect rect, SKPaint paint, FinderShape shape, int cellSize);
}
