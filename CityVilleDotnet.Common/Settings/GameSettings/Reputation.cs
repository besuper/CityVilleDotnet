using System.Xml.Serialization;

namespace CityVilleDotnet.Common.Settings.GameSettings;

[Serializable]
public class ReputationContainer
{
    [XmlElement("level")] public required List<ReputationItem> Levels { get; set; }
}

[Serializable]
public class ReputationItem
{
    [XmlAttribute("num")] public required string Num { get; set; }

    [XmlAttribute("requiredXP")] public required string RequiredXp { get; set; }

    [XmlAttribute("reward")] public required string Reward { get; set; }
}