namespace CityVilleDotnet.Common.Global;

using Microsoft.Extensions.Logging;

public static class StaticLogger
{
    private static ILogger? _logger;

    public static void Configure(ILoggerFactory loggerFactory
    )
    {
        _logger = loggerFactory.CreateLogger("Global");
    }

    public static ILogger Current => _logger ?? throw new InvalidOperationException("Logger is not ready. Call Configure() first.");
}