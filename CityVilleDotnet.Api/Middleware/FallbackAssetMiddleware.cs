namespace CityVilleDotnet.Api.Middleware;

public class FallbackAssetMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<FallbackAssetMiddleware> _logger;

    public FallbackAssetMiddleware(
        RequestDelegate next,
        IWebHostEnvironment env,
        ILogger<FallbackAssetMiddleware> logger)
    {
        _next = next;
        _env = env;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);

        if (context.Response.StatusCode == 404 && context.Request.Path.StartsWithSegments("/assets"))
        {
            var extension = Path.GetExtension(context.Request.Path).ToLowerInvariant();
            var contentType = extension switch
            {
                ".png" => "image/png",
                ".jpg" or ".jpeg" => "image/jpeg",
                ".gif" => "image/gif",
                ".swf" => "application/x-shockwave-flash",
                _ => null
            };

            if (contentType != null)
            {
                var defaultFile = Path.Combine(
                    _env.WebRootPath,
                    "assets",
                    $"default{extension}"
                );

                if (File.Exists(defaultFile))
                {
                    _logger.LogWarning(
                        "Asset not found: {RequestPath}, serving default fallback: {DefaultFile}",
                        context.Request.Path,
                        defaultFile);

                    context.Response.StatusCode = 200;
                    context.Response.ContentType = contentType;
                    await context.Response.SendFileAsync(defaultFile);
                }
                else
                {
                    _logger.LogWarning(
                        "Asset not found: {RequestPath}, but no default fallback exists at: {DefaultFile}",
                        context.Request.Path,
                        defaultFile);
                }
            }
        }
    }
}