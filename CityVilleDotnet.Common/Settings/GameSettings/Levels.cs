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
    [XmlAttribute("num")] public required int Num { get; set; }

    [XmlAttribute("requiredXP")] public required int RequiredXp { get; set; }

    [XmlAttribute("energyMax")] public required int EnergyMax { get; set; }
}

[Serializable]
public class LightLevelsContainer
{
    [XmlElement("level")] public required List<LightLevelItem> Levels { get; set; }
}

[Serializable]
public class LightLevelItem
{
    [XmlAttribute("num")] public required int Num { get; set; }

    [XmlAttribute("requiredLights")] public required int RequiredLights { get; set; }

    [XmlElement("rewards")] public LightLevelReward? Rewards { get; set; }
}

[Serializable]
public class LightLevelReward
{
    [XmlAttribute("coins")] public int Coins { get; set; }

    [XmlAttribute("goods")] public int Goods { get; set; }

    [XmlAttribute("energy")] public int Energy { get; set; }

    [XmlAttribute("item")] public string? Item { get; set; }

    [XmlAttribute("itemCount")] public int ItemCount { get; set; } = 1;
}