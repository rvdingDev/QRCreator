using QRCreator.Avalonia.Models;
using SkiaSharp;

namespace QRCreator.Avalonia.Rendering;

public sealed class DefaultCellRenderer : ICellRenderer
{
    private const float ViewBox = 24f;

    // Indexed by enum value: null means draw manually
    private static readonly string?[] SvgData =
    [
        FluentIconPaths.Square,        // 0
        FluentIconPaths.Circle,        // 1
        FluentIconPaths.RoundedSquare, // 2
        FluentIconPaths.Diamond,       // 3
        FluentIconPaths.Heart,         // 4
        FluentIconPaths.Star,          // 5
        FluentIconPaths.Hexagon,       // 6
        FluentIconPaths.Raindrop,      // 7
        FluentIconPaths.Clover,        // 8
        FluentIconPaths.Octagon,       // 9  → null
    ];

    private static readonly SKPath?[] CachedPaths = new SKPath?[SvgData.Length];
    private static readonly bool[] ParseAttempted = new bool[SvgData.Length];

    public void DrawCell(SKCanvas canvas, SKRect rect, SKPaint paint, CellShape shape, int cellSize)
    {
        DrawShape(canvas, rect, paint, (int)shape);
    }

    public void DrawFinderModule(SKCanvas canvas, SKRect rect, SKPaint paint, FinderShape shape, int cellSize)
    {
        DrawShape(canvas, rect, paint, (int)shape);
    }

    private static void DrawShape(SKCanvas canvas, SKRect rect, SKPaint paint, int shapeIndex)
    {
        // Manual draw for shapes without SVG path
        if (shapeIndex == 9) // Octagon
        {
            DrawOctagon(canvas, rect, paint);
            return;
        }

        // SVG path rendering
        var path = GetCachedPath(shapeIndex);
        if (path is null)
        {
            canvas.DrawRect(rect, paint); // fallback
            return;
        }

        float scale = rect.Width / ViewBox;
        canvas.Save();
        canvas.Translate(rect.Left, rect.Top);
        canvas.Scale(scale, scale);
        canvas.DrawPath(path, paint);
        canvas.Restore();
    }

    private static SKPath? GetCachedPath(int index)
    {
        if (ParseAttempted[index])
            return CachedPaths[index];

        ParseAttempted[index] = true;
        var svg = SvgData[index];
        if (svg is null)
            return null;

        var path = SKPath.ParseSvgPathData(svg);
        CachedPaths[index] = path;
        return path;
    }

    private static void DrawOctagon(SKCanvas canvas, SKRect rect, SKPaint paint)
    {
        float cx = rect.MidX;
        float cy = rect.MidY;
        float r = rect.Width / 2f;

        using var path = new SKPath();
        for (int i = 0; i < 8; i++)
        {
            float angle = (float)(-Math.PI / 8.0 + i * Math.PI / 4.0);
            float x = cx + r * (float)Math.Cos(angle);
            float y = cy + r * (float)Math.Sin(angle);
            if (i == 0) path.MoveTo(x, y);
            else path.LineTo(x, y);
        }
        path.Close();

        canvas.DrawPath(path, paint);
    }
}
