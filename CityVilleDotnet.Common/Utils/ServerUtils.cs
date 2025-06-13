namespace CityVilleDotnet.Common.Utils;

public static class ServerUtils
{
    public static long GetCurrentTime()
    {
        return DateTimeOffset.Now.ToUnixTimeMilliseconds();
    }
}