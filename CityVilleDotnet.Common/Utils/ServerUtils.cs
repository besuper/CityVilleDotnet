using Microsoft.Extensions.Logging;

namespace CityVilleDotnet.Common.Utils;

public static class ServerUtils
{
    private const long MaxClientEnqueueDriftMs = 60_000;

    private static readonly List<string> RequiredFiles =
    [
        "wwwroot/Preloader.swf",
        "wwwroot/gameSettings.xml",
        "wwwroot/questSettings.xml",
        "wwwroot/effectsConfig.xml",
        "wwwroot/FontMapper.swf",
        "wwwroot/lang/locale_en_US.swf",
        "wwwroot/bootstrap.xml",
        "wwwroot/settings.amf.z",
        "wwwroot/EmbeddedArt.swf",
        "wwwroot/Game.2012.swf"
    ];

    public static long GetCurrentTime()
    {
        return DateTimeOffset.Now.ToUnixTimeMilliseconds();
    }

    public static long GetCurrentTimeSeconds()
    {
        return DateTimeOffset.Now.ToUnixTimeSeconds();
    }
    
    public static long GetActionTime(long? clientEnqueueTimeSeconds)
    {
        var now = GetCurrentTime();

        if (clientEnqueueTimeSeconds is null or <= 0) return now;

        return Math.Clamp(clientEnqueueTimeSeconds.Value * 1000, now - MaxClientEnqueueDriftMs, now);
    }

    public static void CheckRequiredFiles(ILogger logger)
    {
        foreach (var filePath in RequiredFiles)
        {
            if (File.Exists(filePath)) continue;

            logger.LogError("Missing required file ({FilePath})", filePath);
        }
    }
}