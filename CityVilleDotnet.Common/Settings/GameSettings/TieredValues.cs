using System.Xml.Serialization;

namespace CityVilleDotnet.Common.Settings.GameSettings;

[Serializable]
public class TieredValuesContainer
{
    [XmlElement("tieredValue")] public List<TieredValueItem> Values { get; set; } = [];
}

[Serializable]
public class TieredValueItem
{
    [XmlAttribute("name")] public required string Name { get; set; }
    [XmlElement("tier")] public List<TieredValueTier> Tiers { get; set; } = [];

    public string? GetAmount(int num)
    {
        if (Tiers.Count == 0) return null;

        var result = Tiers[0];

        foreach (var tier in Tiers)
        {
            if (num < tier.Num) break;

            result = tier;
        }

        return result.Amount;
    }
}

[Serializable]
public class TieredValueTier
{
    [XmlAttribute("num")] public int Num { get; set; }
    [XmlAttribute("amount")] public string? Amount { get; set; }
}

[Serializable]
public class TieredValueReference
{
    [XmlAttribute("table")] public string? Table { get; set; }
}
