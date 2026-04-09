using QRCreator.Models;
using SkiaSharp;

namespace QRCreator.Rendering;

public static class QrRenderer
{
    private static readonly ICellRenderer Renderer = new DefaultCellRenderer();

    public static SKBitmap Render(bool[,] matrix, QrRenderOptions options)
    {
        int moduleCount = matrix.GetLength(0);
        int step = options.CellSize + options.CellGap;
        int margin = options.CellSize * 2;
        int totalSize = moduleCount * step - options.CellGap + margin * 2;

        var bitmap = new SKBitmap(totalSize, totalSize);
        using var canvas = new SKCanvas(bitmap);

        // Background
        canvas.Clear(options.BackgroundColor);

        // Determine logo mask area
        SKRect? logoMaskRect = null;
        if (options.LogoImage is not null && options.LogoSizeRatio > 0f)
        {
            float logoAreaSize = moduleCount * step * options.LogoSizeRatio;
            float center = totalSize / 2f;
            logoMaskRect = new SKRect(
                center - logoAreaSize / 2f,
                center - logoAreaSize / 2f,
                center + logoAreaSize / 2f,
                center + logoAreaSize / 2f);
        }

        var finderRegions = GetFinderRegions(moduleCount);

        using var fgPaint = new SKPaint { Color = options.ForegroundColor, IsAntialias = true, Style = SKPaintStyle.Fill };

        // Draw cells
        for (int row = 0; row < moduleCount; row++)
        {
            for (int col = 0; col < moduleCount; col++)
            {
                if (!matrix[row, col])
                    continue;

                float x = margin + col * step;
                float y = margin + row * step;
                var rect = new SKRect(x, y, x + options.CellSize, y + options.CellSize);

                // Skip cells under logo mask
                if (logoMaskRect.HasValue && logoMaskRect.Value.IntersectsWith(rect))
                    continue;

                if (IsInFinderRegion(row, col, finderRegions))
                {
                    Renderer.DrawFinderModule(canvas, rect, fgPaint, options.FinderShape, options.CellSize);
                }
                else
                {
                    Renderer.DrawCell(canvas, rect, fgPaint, options.CellShape, options.CellSize);
                }
            }
        }

        // Draw logo
        if (options.LogoImage is not null && logoMaskRect.HasValue)
        {
            using var logoPaint = new SKPaint { IsAntialias = true };
            canvas.DrawBitmap(options.LogoImage, logoMaskRect.Value, logoPaint);
        }

        return bitmap;
    }

    private static (int Row, int Col, int Size)[] GetFinderRegions(int moduleCount)
    {
        const int finderSize = 7;
        return
        [
            (0, 0, finderSize),                                    // Top-left
            (0, moduleCount - finderSize, finderSize),             // Top-right
            (moduleCount - finderSize, 0, finderSize),             // Bottom-left
        ];
    }

    private static bool IsInFinderRegion(int row, int col, (int Row, int Col, int Size)[] regions)
    {
        foreach (var (r, c, size) in regions)
        {
            if (row >= r && row < r + size && col >= c && col < c + size)
                return true;
        }
        return false;
    }
}
