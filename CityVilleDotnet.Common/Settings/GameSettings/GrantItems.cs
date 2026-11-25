using System.Globalization;
using System.Xml.Serialization;

namespace CityVilleDotnet.Common.Settings.GameSettings;

[Serializable]
public class GrantItemSetting
{
    [XmlAttribute("key")] public required string Key { get; set; }
    [XmlAttribute("name")] public string? Name { get; set; }
    [XmlAttribute("names")] public string? NamesList { get; set; }
    [XmlAttribute("validator")] public string? Validator { get; set; }

    public string[] GetNames()
    {
        return (NamesList ?? Name ?? string.Empty).Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    }
}

[Serializable]
public class SunsetSetting
{
    [XmlAttribute("theme")] public required string Theme { get; set; }
    [XmlElement("endDate")] public string? EndDate { get; set; }
    
    public bool IsSunset(DateTimeOffset now)
    {
        return ParseDate(EndDate) is not { } endDate || endDate < now;
    }

    private static DateTimeOffset? ParseDate(string? value)
    {
        var parts = value?.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (parts is not { Length: 3 } || !DateTime.TryParseExact($"{parts[0]} {parts[1]}", "M/d/yyyy H:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
        {
            return null;
        }

        return new DateTimeOffset(date, TimeSpan.FromHours(parts[2] == "PST" ? -8 : -7));
    }
}
