using System.Collections.Concurrent;
using CityVilleDotnet.Common.Settings;
using SkiaSharp;

namespace CityVilleDotnet.Api.Middleware;

public class FallbackAssetMiddleware(
    RequestDelegate next,
    IWebHostEnvironment env,
    IConfiguration configuration,
    ILogger<FallbackAssetMiddleware> logger)
{
    private readonly Dictionary<string, string> _fallbacks = new(configuration.GetSection("assetFallbacks").Get<Dictionary<string, string>>() ?? [], StringComparer.OrdinalIgnoreCase);
    private readonly ConcurrentDictionary<(string File, int Height), byte[]> _resizedPlaceholders = new();

    public async Task InvokeAsync(HttpContext context)
    {
        await next(context);

        if (context.Response.StatusCode == 404 && context.Request.Path.StartsWithSegments("/assets"))
        {
            var extension = Path.GetExtension(context.Request.Path).ToLowerInvariant();
            var contentType = extension switch
            {
                ".png" => "image/png",
                ".jpg" or ".jpeg" => "image/jpeg",
                ".gif" => "image/gif",
                ".mp3" => "audio/mpeg",
                ".swf" => "application/x-shockwave-flash",
                ".css" => "text/css",
                _ => null
            };

            if (contentType != null)
            {
                var fallbackFile = GetConfiguredFallback(context.Request.Path);

                if (fallbackFile is not null)
                {
                    logger.LogWarning("Asset not found: {RequestPath}, serving configured fallback: {FallbackFile}", context.Request.Path, fallbackFile);

                    await SendFallbackAsync(context, fallbackFile, contentType);
                    return;
                }

                var placeholder = GameSettingsManager.Instance.GetAssetPlaceholder(context.Request.Path.Value!);
                var placeholderFile = placeholder is not null ? ResolveFallbackFile(context.Request.Path, placeholder.Path) : null;

                if (placeholder is not null && placeholderFile is not null)
                {
                    logger.LogWarning("Asset not found: {RequestPath}, serving construction placeholder: {PlaceholderFile}", context.Request.Path, placeholderFile);

                    if (placeholder.Height is null)
                    {
                        await SendFallbackAsync(context, placeholderFile, contentType);
                    }
                    else
                    {
                        var image = _resizedPlaceholders.GetOrAdd((placeholderFile, placeholder.Height.Value), x => RenderPlaceholder(x.File, x.Height));

                        SetFallbackHeaders(context, "image/png");
                        await context.Response.Body.WriteAsync(image);
                    }

                    return;
                }

                var defaultFile = Path.Combine(env.WebRootPath, "assets", $"default{extension}");

                if (File.Exists(defaultFile))
                {
                    logger.LogWarning("Asset not found: {RequestPath}, serving default fallback: {DefaultFile}", context.Request.Path, defaultFile);

                    await SendFallbackAsync(context, defaultFile, contentType);
                }
                else
                {
                    logger.LogWarning("Asset not found: {RequestPath}, but no default fallback exists at: {DefaultFile}", context.Request.Path, defaultFile);
                }
            }
        }
    }

    private string? GetConfiguredFallback(PathString requestPath)
    {
        return _fallbacks.TryGetValue(requestPath.Value!, out var fallbackPath) ? ResolveFallbackFile(requestPath, fallbackPath) : null;
    }

    private string? ResolveFallbackFile(PathString requestPath, string fallbackPath)
    {
        var fallbackFile = Path.GetFullPath(Path.Combine(env.WebRootPath, fallbackPath.TrimStart('/')));

        if (File.Exists(fallbackFile)) return fallbackFile;

        logger.LogError("Configured fallback for {RequestPath} doesn't exist: {FallbackFile}", requestPath, fallbackFile);

        return null;
    }

    private static byte[] RenderPlaceholder(string file, int height)
    {
        using var source = SKBitmap.Decode(file);
        using var resized = new SKBitmap(source.Width, height);
        using var canvas = new SKCanvas(resized);

        canvas.Clear(SKColors.Transparent);
        canvas.DrawBitmap(source, SKRect.Create(0, height - source.Height, source.Width, source.Height), SKSamplingOptions.Default);

        using var image = SKImage.FromBitmap(resized);
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);

        return data.ToArray();
    }

    private static async Task SendFallbackAsync(HttpContext context, string file, string contentType)
    {
        SetFallbackHeaders(context, contentType);
        await context.Response.SendFileAsync(file);
    }

    private static void SetFallbackHeaders(HttpContext context, string contentType)
    {
        context.Response.StatusCode = 200;
        context.Response.ContentType = contentType;
        context.Response.Headers.CacheControl = "public, max-age=2592000"; // 1 month
        context.Response.Headers.Expires = DateTime.UtcNow.AddMonths(1).ToString("R");
    }
}