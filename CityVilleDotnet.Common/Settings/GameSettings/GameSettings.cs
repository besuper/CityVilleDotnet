using System.Xml.Serialization;

namespace CityVilleDotnet.Common.Settings.GameSettings;

[Serializable]
[XmlRoot("settings")]
public class GameSettings
{
    [XmlElement("items")] public required ItemsContainer Items { get; set; }

    [XmlElement("mechanicPacks")] public MechanicPacksContainer? MechanicPacks { get; set; }

    [XmlElement("levels_cv_level_regrade_var_0")] public required LevelsContainer Levels { get; set; }
    [XmlElement("reputation")] public required ReputationContainer Reputation { get; set; }

    [XmlElement("farming")] public required FarmingSettings Farming { get; set; }

    [XmlElement("randomModifierTables")] public required RandomModifierTables Modifiers { get; set; }
    [XmlElement("randomModifierPacks")] public RandomModifierPacksContainer? ModifierPacks { get; set; }
    [XmlElement("lootTables")] public LootTablesContainer? LootTables { get; set; }
    [XmlElement("collections")] public required CollectionContainer Collections { get; set; }
    [XmlElement("expansionRequirements")] public required ExpansionsGate Expansions { get; set; }
    [XmlElement("worldRects")] public required WorldRectsContainer WorldRects { get; set; }
    [XmlElement("worldConfigs")] public WorldConfigsContainer? WorldConfigs { get; set; }
    [XmlElement("dynamicExpansions")] public DynamicExpansionsContainer? DynamicExpansions { get; set; }
    [XmlElement("citysim")] public required CitySim CitySim { get; set; }
}