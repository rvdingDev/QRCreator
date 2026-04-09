using QRCreator.Models;
using SkiaSharp;

namespace QRCreator.Rendering;

public sealed class DefaultCellRenderer : ICellRenderer
{
    public void DrawCell(SKCanvas canvas, SKRect rect, SKPaint paint, CellShape shape, int cellSize)
    {
        float cornerRadius = cellSize * 0.3f;

        switch (shape)
        {
            case CellShape.Square:
                canvas.DrawRect(rect, paint);
                break;

            case CellShape.Circle:
                canvas.DrawOval(rect, paint);
                break;

            case CellShape.RoundedSquare:
                canvas.DrawRoundRect(rect, cornerRadius, cornerRadius, paint);
                break;

            case CellShape.Diamond:
                DrawDiamond(canvas, rect, paint);
                break;
        }
    }

    public void DrawFinderModule(SKCanvas canvas, SKRect rect, SKPaint paint, FinderShape shape, int cellSize)
    {
        float cornerRadius = cellSize * 0.3f;

        switch (shape)
        {
            case FinderShape.Square:
                canvas.DrawRect(rect, paint);
                break;

            case FinderShape.Circle:
                canvas.DrawOval(rect, paint);
                break;

            case FinderShape.RoundedSquare:
                canvas.DrawRoundRect(rect, cornerRadius, cornerRadius, paint);
                break;
        }
    }

    private static void DrawDiamond(SKCanvas canvas, SKRect rect, SKPaint paint)
    {
        float cx = rect.MidX;
        float cy = rect.MidY;

        using var path = new SKPath();
        path.MoveTo(cx, rect.Top);
        path.LineTo(rect.Right, cy);
        path.LineTo(cx, rect.Bottom);
        path.LineTo(rect.Left, cy);
        path.Close();

        canvas.DrawPath(path, paint);
    }
}
