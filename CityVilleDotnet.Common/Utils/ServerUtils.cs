namespace CityVilleDotnet.Common.Utils;

public static class ServerUtils
{
    public static long GetCurrentTime()
    {
        return DateTimeOffset.Now.ToUnixTimeMilliseconds();
    }
    
    public static double GetCurrentTimeSeconds()
    {
        return DateTime.Now.ToUniversalTime().Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
    }
}