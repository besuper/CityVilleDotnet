using Microsoft.Extensions.Logging;
using System.Xml;
using System.Xml.Serialization;

namespace CityVilleDotnet.Common.Settings;

[Serializable]
[XmlRoot("settings")]
public class GameSettings
{
    [XmlElement("items")]
    public ItemsContainer Items { get; set; }

    [XmlElement("levels")]
    public LevelsContainer Levels { get; set; }

    [XmlElement("randomModifierTables")]
    public RandomModifierTables Modifiers { get; set; }
}

[Serializable]
public class ItemsContainer
{
    [XmlElement("item")]
    public List<GameItem> Items { get; set; }
}

[Serializable]
public class GameItem
{
    [XmlAttribute("name")]
    public string Name { get; set; }

    [XmlElement("requiredLevel")]
    public int? RequiredLevel { get; set; }

    [XmlElement("requiredPopulation")]
    public int? RequiredPopulation { get; set; }

    [XmlElement("populationYield")]
    public int? PopulationYield { get; set; }

    [XmlElement("populationCapYield")]
    public int? PopulationCapYield { get; set; }

    [XmlElement("cost")]
    public int? Cost { get; set; }

    [XmlElement("coinYield")]
    public int? CoinYield { get; set; }

    [XmlElement("xpYield")]
    public int? XpYield { get; set; }

    [XmlElement("goodsYield")]
    public int? GoodsYield { get; set; }

    [XmlElement("construction")]
    public string Construction { get; set; }

    [XmlElement("commodityReq")]
    public int? CommodityRequired { get; set; }

    [XmlElement("randomModifiers")]
    public RandomModifiers? RandomModifiers { get; set; }
}

[Serializable]
public class LevelsContainer
{
    [XmlElement("level")]
    public List<LevelItem> Levels { get; set; }
}

[Serializable]
public class LevelItem
{
    [XmlAttribute("num")]
    public string Num { get; set; }

    [XmlAttribute("requiredXP")]
    public string RequiredXp { get; set; }

    [XmlAttribute("energyMax")]
    public string EnergyMax { get; set; }
}

[Serializable]
public class RandomModifiers
{
    [XmlElement("modifier")]
    public List<RandomModifier>? Modifiers { get; set; }
}

[Serializable]
public class RandomModifier
{
    [XmlAttribute("type")]
    public string Type { get; set; }

    [XmlAttribute("tableName")]
    public string TableName { get; set; }
}

[Serializable]
public class RandomModifierTables
{
    [XmlElement("randomModifierTable")]
    public List<RandomModifierTable> Table { get; set; }
}

[Serializable]
public class RandomModifierTable
{
    [XmlAttribute("type")]
    public string Type { get; set; }

    [XmlAttribute("name")]
    public string Name { get; set; }

    [XmlElement("roll")]
    public List<Roll> Rolls { get; set; }
}

[Serializable]
public class Roll
{
    [XmlAttribute("percent")]
    public int Percent { get; set; }

    [XmlIgnore]
    public Dictionary<string, List<AmountElement>> Rewards { get; set; } = new Dictionary<string, List<AmountElement>>();

    [XmlAnyElement]
    public XmlElement[] RewardElements { get; set; }

    public void OnDeserialized()
    {
        if (RewardElements != null)
        {
            foreach (var element in RewardElements)
            {
                var rewardType = element.Name;
                var _amount = element.GetAttribute("amount");

                var amount = _amount == "" ? 0 : double.Parse(_amount);
                var name = element.GetAttribute("name");

                if (!Rewards.ContainsKey(rewardType))
                {
                    Rewards[rewardType] = new List<AmountElement>();
                }

                Rewards[rewardType].Add(new AmountElement { Amount = amount, Name = name });
            }
        }
    }
}

[Serializable]
public class AmountElement
{
    [XmlAttribute("amount")]
    public double Amount { get; set; }

    [XmlAttribute("name")]
    public string Name { get; set; }
}

public class GameSettingsManager
{
    private static GameSettingsManager _instance;
    private static readonly object _lock = new object();
    private Dictionary<string, GameItem> _items;
    private Dictionary<string, RandomModifierTable> _randomModifiers;
    private List<LevelItem> _levels = new List<LevelItem>();
    private bool _isInitialized;

    private GameSettingsManager()
    {
        _items = new Dictionary<string, GameItem>();
        _randomModifiers = new Dictionary<string, RandomModifierTable>();
        _isInitialized = false;
    }

    public static GameSettingsManager Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new GameSettingsManager();
                    }
                }
            }
            return _instance;
        }
    }

    public void Initialize(ILogger<GameSettingsManager> logger)
    {
        if (_isInitialized)
            return;

        XmlSerializer serializer = new XmlSerializer(typeof(GameSettings));

        using (FileStream fileStream = new FileStream("wwwroot/gameSettings.xml", FileMode.Open))
        {
            var gameSettings = (GameSettings)serializer.Deserialize(fileStream);

            if (gameSettings?.Items?.Items != null)
            {
                foreach (var item in gameSettings.Items.Items)
                {
                    if (item.Name != null)
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
        }

        logger.LogInformation($"Loaded gameSettings.xml with {_items.Count} items");
        logger.LogInformation($"Loaded {_levels.Count} levels");
        logger.LogInformation($"Loaded {_randomModifiers.Count} random modifiers");

        _isInitialized = true;
    }

    public GameItem GetItem(string itemName)
    {
        if (!_isInitialized)
        {
            throw new InvalidOperationException("GameSettingsManager not initialized");
        }

        if (_items.TryGetValue(itemName, out GameItem item))
        {
            return item;
        }

        return null;
    }

    public RandomModifierTable GetRandomModifier(string name)
    {
        if (!_isInitialized)
        {
            throw new InvalidOperationException("GameSettingsManager not initialized");
        }

        if (_randomModifiers.TryGetValue(name, out RandomModifierTable item))
        {
            return item;
        }

        return null;
    }

    public IReadOnlyCollection<LevelItem> GetLevels()
    {
        return _levels.AsReadOnly();
    }
}