using System.Xml.Serialization;

namespace CityVilleDotnet.Common.Settings.GameSettings;

[Serializable]
[XmlRoot("settings")]
public class GameSettings
{
    [XmlElement("items")] public required ItemsContainer Items { get; set; }

    [XmlElement("levels")] public required LevelsContainer Levels { get; set; }
    [XmlElement("reputation")] public required ReputationContainer Reputation { get; set; }

    [XmlElement("farming")] public required FarmingSettings Farming { get; set; }

    [XmlElement("randomModifierTables")] public required RandomModifierTables Modifiers { get; set; }
    [XmlElement("collections")] public required CollectionContainer Collections { get; set; }
    [XmlElement("expansions")] public required ExpansionsContainer Expansions { get; set; }
}