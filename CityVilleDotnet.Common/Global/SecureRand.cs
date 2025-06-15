using System.Security.Cryptography;
using System.Text;

namespace CityVilleDotnet.Common.Global;

public static class SecureRand
{
    private const string HardCodedSecret = "YOUR_LIKE_AN_8";
    private static string _handshake = "";

    public static void SetHandshake(string handshake)
    {
        _handshake = handshake;
    }

    public static string GetMd5Hash(string input)
    {
        return Convert.ToHexString(MD5.HashData(Encoding.UTF8.GetBytes(input)));
    }

    // From SecureRand::rand
    public static int GenerateRand(int min, int max, int rollCounter)
    {
        // FIXME: Use current user id
        var stringToHash = HardCodedSecret + "::" + _handshake + "::" + 333 + "::" + rollCounter;

        var range = max - min + 1;

        var md5Hash = "0x" + GetMd5Hash(stringToHash).Substring(0, 8);
        var hashNumber = Convert.ToUInt64(md5Hash, 16);

        var moduloResult = (int)(hashNumber % (ulong)range);

        return moduloResult + min;
    }
}