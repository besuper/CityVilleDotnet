using Microsoft.Extensions.Logging;

namespace CityVilleDotnet.Common.Utils;

public static class ServerUtils
{
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

    public static double GetCurrentTimeSeconds()
    {
        return DateTimeOffset.Now.ToUnixTimeSeconds();
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