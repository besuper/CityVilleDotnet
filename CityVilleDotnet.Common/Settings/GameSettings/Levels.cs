using System.Xml.Serialization;

namespace CityVilleDotnet.Common.Settings.GameSettings;

[Serializable]
public class LevelsContainer
{
    [XmlElement("level")] public required List<LevelItem> Levels { get; set; }
}

[Serializable]
public class LevelItem
{
    [XmlAttribute("num")] public required string Num { get; set; }

    [XmlAttribute("requiredXP")] public required string RequiredXp { get; set; }

    [XmlAttribute("energyMax")] public required string EnergyMax { get; set; }
}