namespace CityVilleDotnet.Api.Middleware;

public class FallbackAssetMiddleware(
    RequestDelegate next,
    IWebHostEnvironment env,
    ILogger<FallbackAssetMiddleware> logger)
{
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
                var defaultFile = Path.Combine(
                    env.WebRootPath,
                    "assets",
                    $"default{extension}"
                );

                if (File.Exists(defaultFile))
                {
                    logger.LogWarning(
                        "Asset not found: {RequestPath}, serving default fallback: {DefaultFile}",
                        context.Request.Path,
                        defaultFile);

                    context.Response.StatusCode = 200;
                    context.Response.ContentType = contentType;
                    context.Response.Headers.CacheControl = "public, max-age=2592000"; // 1 month
                    context.Response.Headers.Expires = DateTime.UtcNow.AddMonths(1).ToString("R");
                    await context.Response.SendFileAsync(defaultFile);
                }
                else
                {
                    logger.LogWarning(
                        "Asset not found: {RequestPath}, but no default fallback exists at: {DefaultFile}",
                        context.Request.Path,
                        defaultFile);
                }
            }
        }
    }
}