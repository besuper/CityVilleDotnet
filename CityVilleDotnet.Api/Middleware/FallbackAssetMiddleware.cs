namespace CityVilleDotnet.Api.Middleware;

public class FallbackAssetMiddleware(
    RequestDelegate next,
    IWebHostEnvironment env,
    IConfiguration configuration,
    ILogger<FallbackAssetMiddleware> logger)
{
    private readonly Dictionary<string, string> _fallbacks = new(configuration.GetSection("assetFallbacks").Get<Dictionary<string, string>>() ?? [], StringComparer.OrdinalIgnoreCase);

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
                    logger.LogDebug("Asset not found: {RequestPath}, serving configured fallback: {FallbackFile}", context.Request.Path, fallbackFile);

                    await SendFallbackAsync(context, fallbackFile, contentType);
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
        if (!_fallbacks.TryGetValue(requestPath.Value!, out var fallbackPath)) return null;

        var fallbackFile = Path.GetFullPath(Path.Combine(env.WebRootPath, fallbackPath.TrimStart('/')));

        if (File.Exists(fallbackFile)) return fallbackFile;

        logger.LogError("Configured fallback for {RequestPath} doesn't exist: {FallbackFile}", requestPath, fallbackFile);

        return null;
    }

    private static async Task SendFallbackAsync(HttpContext context, string file, string contentType)
    {
        context.Response.StatusCode = 200;
        context.Response.ContentType = contentType;
        context.Response.Headers.CacheControl = "public, max-age=2592000"; // 1 month
        context.Response.Headers.Expires = DateTime.UtcNow.AddMonths(1).ToString("R");
        await context.Response.SendFileAsync(file);
    }
}