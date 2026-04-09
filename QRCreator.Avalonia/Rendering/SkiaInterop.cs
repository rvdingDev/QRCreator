using Avalonia.Media.Imaging;
using SkiaSharp;

namespace QRCreator.Avalonia.Rendering;

public static class SkiaInterop
{
    public static Bitmap ToBitmap(SKBitmap skBitmap)
    {
        using var image = SKImage.FromBitmap(skBitmap);
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        using var stream = new MemoryStream(data.ToArray());
        return new Bitmap(stream);
    }

    public static SKColor ToSkColor(global::Avalonia.Media.Color avColor)
    {
        return new SKColor(avColor.R, avColor.G, avColor.B);
    }

    public static void SavePng(SKBitmap bitmap, string filePath)
    {
        using var image = SKImage.FromBitmap(bitmap);
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        using var stream = File.OpenWrite(filePath);
        data.SaveTo(stream);
    }
}
