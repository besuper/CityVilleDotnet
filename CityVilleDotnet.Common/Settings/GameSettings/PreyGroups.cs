using System.Xml.Serialization;

namespace CityVilleDotnet.Common.Settings.GameSettings;

[Serializable]
public class PreyGroupsContainer
{
    [XmlElement("preyGroup")] public List<PreyGroupSetting> Groups { get; set; } = [];
}

[Serializable]
public class PreyGroupSetting
{
    private const string DefaultModeId = "default";

    [XmlAttribute("id")] public required string Id { get; set; }
    [XmlElement("queueInfo")] public required PreyQueueInfo QueueInfo { get; set; }
    [XmlArray("modes"), XmlArrayItem("mode")] public List<PreyGroupMode> Modes { get; set; } = [];
    [XmlArray("preys"), XmlArrayItem("prey")] public List<PreySetting> Preys { get; set; } = [];
    [XmlArray("hubs"), XmlArrayItem("hub")] public List<PreyHub> Hubs { get; set; } = [];
    [XmlElement("spawnsFrom")] public List<PreySpawnSource> SpawnsFrom { get; set; } = [];

    public PreyGroupMode GetActiveMode(int hubLevel)
    {
        return Modes.Where(x => x.LevelRequired <= hubLevel).MaxBy(x => x.LevelRequired) ?? Modes.First(x => x.Id == DefaultModeId);
    }

    public List<string> GetHubNames() => Hubs.Select(x => x.Name).ToList();
    public PreyHub? GetHubByName(string itemName) => Hubs.FirstOrDefault(x => x.Name == itemName);
    public PreyHub? GetHubByLevel(int level) => Hubs.FirstOrDefault(x => x.Level == level);
    public PreySetting? GetPrey(int id) => Preys.FirstOrDefault(x => x.Id == id);
}

[Serializable]
public class PreyQueueInfo
{
    [XmlAttribute("perBlock")] public int PerBlock { get; set; }
    [XmlAttribute("noPreyQueue")] public bool NoPreyQueue { get; set; }
    [XmlAttribute("hunterSleepTime")] public int HunterSleepTime { get; set; }
    [XmlAttribute("resourceCashCost")] public int ResourceCashCost { get; set; }
    [XmlAttribute("catchPreyNowCashCost")] public int CatchPreyNowCashCost { get; set; }
}

[Serializable]
public class PreyGroupMode
{
    [XmlAttribute("id")] public required string Id { get; set; }
    [XmlAttribute("blockLength")] public int BlockLength { get; set; }
    [XmlAttribute("levelRequired")] public int LevelRequired { get; set; }
    [XmlAttribute("spawnPreyPlayerLevelRequired")] public int SpawnPreyPlayerLevelRequired { get; set; }
    [XmlAttribute("spawnPreyExperimentName")] public string? SpawnPreyExperimentName { get; set; }
    [XmlAttribute("spawnPreyExperimentVariant")] public string? SpawnPreyExperimentVariant { get; set; }
    [XmlAttribute("spawnPreyWorldId")] public string? SpawnPreyWorldId { get; set; }
}

[Serializable]
public class PreySpawnSource
{
    [XmlAttribute("name")] public required string Name { get; set; }
    [XmlAttribute("spawnChance")] public int SpawnChance { get; set; }
    [XmlAttribute("world")] public string? World { get; set; }
    [XmlAttribute("expName")] public string? ExperimentName { get; set; }
    [XmlAttribute("expVariant")] public string? ExperimentVariant { get; set; }
}

[Serializable]
public class PreySetting
{
    [XmlAttribute("id")] public int Id { get; set; }
    [XmlAttribute("levelRequired")] public int LevelRequired { get; set; }
    [XmlArray("modes"), XmlArrayItem("mode")] public List<PreyModeSetting> Modes { get; set; } = [];

    public PreyModeSetting? GetMode(string modeId) => Modes.FirstOrDefault(x => x.Id == modeId);
}

[Serializable]
public class PreyModeSetting
{
    [XmlAttribute("id")] public required string Id { get; set; }
    [XmlAttribute("huntersRequired")] public int HuntersRequired { get; set; }
    [XmlAttribute("chance")] public int Chance { get; set; }
    [XmlAttribute("capturesToMaster")] public int CapturesToMaster { get; set; }
    [XmlArray("rewards"), XmlArrayItem("reward")] public List<PreyReward> Rewards { get; set; } = [];
    [XmlArray("masteryrewards"), XmlArrayItem("masteryreward")] public List<PreyReward> MasteryRewards { get; set; } = [];

    public List<PreyReward> GetRewards(int captureCount)
    {
        return CapturesToMaster > 0 && captureCount % CapturesToMaster == 0 ? MasteryRewards : Rewards;
    }
}

[Serializable]
public class PreyReward
{
    [XmlAttribute("type")] public required string Type { get; set; }
    [XmlAttribute("value")] public required string Value { get; set; }
    [XmlAttribute("quantity")] public int Quantity { get; set; } = 1;

    [XmlIgnore]
    public int IntValue => int.Parse(Value);
}

[Serializable]
public class PreyHub
{
    [XmlAttribute("level")] public int Level { get; set; }
    [XmlAttribute("name")] public required string Name { get; set; }
    [XmlAttribute("maxHunters")] public int MaxHunters { get; set; }
}
