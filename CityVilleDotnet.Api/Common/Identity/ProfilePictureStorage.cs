using System.Text.RegularExpressions;
using CityVilleDotnet.Common.Utils;
using SkiaSharp;

namespace CityVilleDotnet.Api.Common.Identity;

public class ProfilePictureStorage(IWebHostEnvironment webHost)
{
    private const int MaxDimension = 50;

    public async Task<string?> SaveAsync(Stream stream, string userName, string ext)
    {
        using var original = SKBitmap.Decode(stream);

        if (original is null) return null;

        var picturesDir = Path.Combine(webHost.WebRootPath, "pictures");
        Directory.CreateDirectory(picturesDir);

        var baseName = Regex.Replace(userName, @"[^a-zA-Z0-9_-]", "_");
        foreach (var oldFile in Directory.EnumerateFiles(picturesDir, $"{baseName}.*"))
        {
            File.Delete(oldFile);
        }

        using var resized = new SKBitmap(MaxDimension, MaxDimension);
        using var canvas = new SKCanvas(resized);

        canvas.DrawBitmap(original, new SKRect(0, 0, MaxDimension, MaxDimension), new SKSamplingOptions(SKFilterMode.Linear, SKMipmapMode.Linear));

        using var image = SKImage.FromBitmap(resized);
        using var data = ext is ".png" ? image.Encode(SKEncodedImageFormat.Png, 100) : image.Encode(SKEncodedImageFormat.Jpeg, 90);

        await using var fs = new FileStream(Path.Combine(picturesDir, $"{baseName}{ext}"), FileMode.Create);

        data.SaveTo(fs);

        return $"/pictures/{baseName}{ext}?version={ServerUtils.GetCurrentTime()}";
    }
}
