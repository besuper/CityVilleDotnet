using System.Text.RegularExpressions;

namespace CityVilleDotnet.Common.Utils;

public static partial class StringSanitizer
{
    public static string Sanitize(this string value)
    {
        var singleLine = value.Replace("\r", "").Replace("\n", " ");

        return UnsafeCharsRegex().Replace(singleLine, "");
    }

    [GeneratedRegex(@"[\p{Cc}\p{Zl}\p{Zp}]")]
    private static partial Regex UnsafeCharsRegex();
}
