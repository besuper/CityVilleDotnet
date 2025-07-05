using Microsoft.Extensions.Logging;
using System.Xml;
using System.Xml.Serialization;
using CityVilleDotnet.Common.Utils;

namespace CityVilleDotnet.Common.Settings;

[Serializable]
[XmlRoot("settings")]
public class GameSettings
{
    [XmlElement("items")] public ItemsContainer Items { get; set; }

    [XmlElement("levels")] public LevelsContainer Levels { get; set; }
    [XmlElement("reputation")] public ReputationContainer Reputation { get; set; }

    [XmlElement("farming")] public FarmingSettings Farming { get; set; }

    [XmlElement("randomModifierTables")] public RandomModifierTables Modifiers { get; set; }
}

[Serializable]
[XmlRoot("farming")]
public class FarmingSettings
{
    [XmlAttribute("inGameDaySeconds")] public string InGameDaySeconds { get; set; }

    [XmlAttribute("friendVisitShipRepGain")]
    public string FriendVisitShipRepGain { get; set; }

    [XmlAttribute("friendVisitConstructionRepGain")]
    public string FriendVisitConstructionRepGain { get; set; }

    [XmlAttribute("friendVisitPlotRepGain")]
    public string FriendVisitPlotRepGain { get; set; }

    [XmlAttribute("friendHelpDefaultGoodsReward")]
    public string FriendHelpDefaultGoodsReward { get; set; }

    [XmlAttribute("friendVisitBusinessRepGain")]
    public string FriendVisitBusinessRepGain { get; set; }

    [XmlAttribute("friendHelpDefaultCoinReward")]
    public string FriendHelpDefaultCoinReward { get; set; }

    [XmlAttribute("friendVisitWildernessRepGain")]
    public string FriendVisitWildernessRepGain { get; set; }

    [XmlAttribute("friendVisitResidenceRepGain")]
    public string FriendVisitResidenceRepGain { get; set; }
}

[Serializable]
public class ItemsContainer
{
    [XmlElement("item")] public List<GameItem?> Items { get; set; }
}

[Serializable]
public class GameItem
{
    [XmlAttribute("name")] public string Name { get; set; }
    [XmlAttribute("height")] public string? Height { get; set; }
    [XmlAttribute("width")] public string? Width { get; set; }

    [XmlElement("requiredLevel")] public int? RequiredLevel { get; set; }

    [XmlElement("requiredPopulation")] public int? RequiredPopulation { get; set; }

    [XmlElement("populationYield")] public int? PopulationYield { get; set; }

    [XmlElement("populationCapYield")] public int? PopulationCapYield { get; set; }

    [XmlElement("cost")] public int? Cost { get; set; }
    [XmlElement("growTime")] public double? GrowTime { get; set; }

    [XmlElement("coinYield")] public int? CoinYield { get; set; }

    [XmlElement("xpYield")] public int? XpYield { get; set; }

    [XmlElement("goodsYield")] public int? GoodsYield { get; set; }

    [XmlElement("construction")] public string Construction { get; set; }

    [XmlElement("commodityReq")] public int? CommodityRequired { get; set; }

    [XmlElement("randomModifiers")] public RandomModifiers? RandomModifiers { get; set; }
}

[Serializable]
public class LevelsContainer
{
    [XmlElement("level")] public List<LevelItem> Levels { get; set; }
}

[Serializable]
public class ReputationContainer
{
    [XmlElement("level")] public List<ReputationItem> Levels { get; set; }
}

[Serializable]
public class LevelItem
{
    [XmlAttribute("num")] public string Num { get; set; }

    [XmlAttribute("requiredXP")] public string RequiredXp { get; set; }

    [XmlAttribute("energyMax")] public string EnergyMax { get; set; }
}

[Serializable]
public class ReputationItem
{
    [XmlAttribute("num")] public string Num { get; set; }

    [XmlAttribute("requiredXP")] public string RequiredXp { get; set; }

    [XmlAttribute("reward")] public string Reward { get; set; }
}

[Serializable]
public class RandomModifiers
{
    [XmlElement("modifier")] public List<RandomModifier>? Modifiers { get; set; }
}

[Serializable]
public class RandomModifier
{
    [XmlAttribute("type")] public string Type { get; set; }

    [XmlAttribute("tableName")] public string TableName { get; set; }
}

[Serializable]
public class RandomModifierTables
{
    [XmlElement("randomModifierTable")] public List<RandomModifierTable> Table { get; set; }
}

[Serializable]
public class RandomModifierTable
{
    [XmlAttribute("type")] public string Type { get; set; }

    [XmlAttribute("name")] public string Name { get; set; }

    [XmlElement("roll")] public List<Roll> Rolls { get; set; }
}

[Serializable]
public class Roll
{
    [XmlAttribute("percent")] public int Percent { get; set; }

    [XmlIgnore] public Dictionary<string, List<AmountElement>> Rewards { get; set; } = new();

    [XmlAnyElement] public XmlElement[] RewardElements { get; set; }

    public void OnDeserialized()
    {
        if (RewardElements is not null)
        {
            foreach (var element in RewardElements)
            {
                var rewardType = element.Name;
                var amountAttribute = element.GetAttribute("amount");

                var amount = amountAttribute == "" ? 0 : double.Parse(amountAttribute);
                var name = element.GetAttribute("name");

                if (!Rewards.ContainsKey(rewardType))
                {
                    Rewards[rewardType] = [];
                }

                Rewards[rewardType].Add(new AmountElement { Amount = amount, Name = name });
            }
        }
    }
}

[Serializable]
public class AmountElement
{
    [XmlAttribute("amount")] public double Amount { get; set; }

    [XmlAttribute("name")] public string Name { get; set; }
}

public class GameSettingsManager
{
    private static GameSettingsManager? _instance;
    private static readonly object Lock = new();
    private readonly Dictionary<string, GameItem?> _items;
    private readonly Dictionary<string, RandomModifierTable> _randomModifiers;
    private Dictionary<string, object> _settings;
    private List<LevelItem> _levels = [];
    private List<ReputationItem> _reputationLevels = [];
    private bool _isInitialized;

    private GameSettingsManager()
    {
        _items = new Dictionary<string, GameItem?>();
        _randomModifiers = new Dictionary<string, RandomModifierTable>();
        _settings = new Dictionary<string, object>();
        _isInitialized = false;
    }

    public static GameSettingsManager Instance
    {
        get
        {
            if (_instance is null)
            {
                lock (Lock)
                {
                    _instance ??= new GameSettingsManager();
                }
            }

            return _instance;
        }
    }

    public void Initialize(ILogger<GameSettingsManager> logger)
    {
        if (_isInitialized)
            return;

        var serializer = new XmlSerializer(typeof(GameSettings));

        using (var fileStream = new FileStream("wwwroot/gameSettings.xml", FileMode.Open))
        {
            var gameSettings = (GameSettings)serializer.Deserialize(fileStream);

            if (gameSettings?.Items?.Items is not null)
            {
                foreach (var item in gameSettings.Items.Items)
                {
                    if (item.Name is not null)
                    {
                        _items[item.Name] = item;
                    }
                }
            }

            foreach (var item in gameSettings.Modifiers.Table)
            {
                _randomModifiers[item.Name] = item;

                foreach (var roll in _randomModifiers[item.Name].Rolls)
                {
                    roll.OnDeserialized();
                }
            }

            _levels = gameSettings.Levels.Levels;
            _reputationLevels = gameSettings.Reputation.Levels;

            _settings = gameSettings.Farming.ToDictionary();
        }

        logger.LogInformation($"Loaded gameSettings.xml with {_items.Count} items");
        logger.LogInformation($"Loaded {_levels.Count} levels");
        logger.LogInformation($"Loaded {_reputationLevels.Count} social levels");
        logger.LogInformation($"Loaded {_randomModifiers.Count} random modifiers");

        _isInitialized = true;
    }

    public GameItem? GetItem(string itemName)
    {
        if (!_isInitialized)
            throw new InvalidOperationException("GameSettingsManager not initialized");

        return _items.TryGetValue(itemName, out var item) ? item : null;
    }

    public RandomModifierTable? GetRandomModifier(string name)
    {
        if (!_isInitialized)
            throw new InvalidOperationException("GameSettingsManager not initialized");

        return _randomModifiers.TryGetValue(name, out var item) ? item : null;
    }

    public IReadOnlyCollection<LevelItem> GetLevels()
    {
        return _levels.AsReadOnly();
    }

    public IReadOnlyCollection<ReputationItem> GetSocialLevels()
    {
        return _reputationLevels.AsReadOnly();
    }

    public int GetInt(string name)
    {
        return (int)_settings[name];
    }
}