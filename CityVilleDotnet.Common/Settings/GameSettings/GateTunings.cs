using System.Globalization;
using System.Xml.Serialization;

namespace CityVilleDotnet.Common.Settings.GameSettings;

[Serializable]
public class DynamicTuningsContainer
{
    [XmlElement("gateTunings")] public List<GateTuningItem> GateTunings { get; set; } = [];
}

[Serializable]
public class GateTuningItem
{
    [XmlAttribute("name")] public required string Name { get; set; }
    [XmlAttribute("hook")] public required string Hook { get; set; }
    [XmlAttribute("_zexp")] public string? Experiment { get; set; }
    [XmlElement("tier")] public List<GateTuningTier> Tiers { get; set; } = [];
}

[Serializable]
public class GateTuningTier
{
    [XmlAttribute("id")] public int Id { get; set; }
    [XmlAttribute("action")] public required string Action { get; set; }

    [XmlIgnore] public double Amount { get; set; }

    [XmlAttribute("amount")]
    public string? AmountString
    {
        get => Amount.ToString(CultureInfo.InvariantCulture);
        set => Amount = string.IsNullOrEmpty(value) ? 0 : double.Parse(value, CultureInfo.InvariantCulture);
    }

    [XmlAttribute("round")] public string? Round { get; set; }
    [XmlAttribute("min")] public int Min { get; set; }

    // DynamicTuningManager::getModifiedAmount
    public int Apply(int amount)
    {
        var value = Action switch
        {
            "add" => amount + Amount,
            "mul" => amount * Amount,
            "set" => Amount,
            _ => amount
        };

        var rounded = (int)(Round == "up" ? Math.Ceiling(value) : Math.Floor(value));

        return Math.Max(rounded, Min);
    }
}
