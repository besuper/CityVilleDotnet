using Microsoft.Extensions.Logging;
using System.Xml.Serialization;
using CityVilleDotnet.Common.Settings.GameSettings;
using CityVilleDotnet.Common.Utils;

namespace CityVilleDotnet.Common.Settings;

public class GameSettingsManager
{
    private static GameSettingsManager? _instance;
    private static readonly object Lock = new();
    private readonly Dictionary<string, GameItem?> _items;
    private readonly Dictionary<string, RandomModifierTable> _randomModifiers;
    private readonly Dictionary<string, WorldRectItem> _worldRects;
    private Dictionary<string, object> _settings;
    private List<LevelItem> _levels = [];
    private List<ReputationItem> _reputationLevels = [];
    private List<CollectionSetting> _collections = [];
    private List<ExpansionSetting> _expansions = [];
    private bool _isInitialized;

    private GameSettingsManager()
    {
        _items = new Dictionary<string, GameItem?>();
        _randomModifiers = new Dictionary<string, RandomModifierTable>();
        _worldRects = new Dictionary<string, WorldRectItem>();
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

    public void Initialize(ILogger<GameSettingsManager> logger, string path = "wwwroot/gameSettings.xml")
    {
        if (_isInitialized)
            return;

        if (!File.Exists(path))
        {
            logger.LogError("Missing file assets ({Path})", path);
            return;
        }

        var serializer = new XmlSerializer(typeof(GameSettings.GameSettings));

        var xmlContent = File.ReadAllText(path);
        xmlContent = xmlContent.Replace("&gt;", "");

        using (var stringReader = new StringReader(xmlContent))
        {
            var content = serializer.Deserialize(stringReader);

            if (content is null)
            {
                logger.LogError("Can't deserialize gameSettings.xml file");
                return;
            }

            var gameSettings = (GameSettings.GameSettings)content;

            foreach (var item in gameSettings.Items.Items)
            {
                if (item?.Name is not null)
                {
                    _items[item.Name] = item;
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

            foreach (var collection in gameSettings.Collections.Collections)
            {
                collection.TradeInRewards.OnDeserialized();
            }

            _collections = gameSettings.Collections.Collections;
            // TODO: Support other expansions gates
            _expansions = gameSettings.Expansions.ExpansionGates.FirstOrDefault(x => x.Name == "population")!.Expansions.Expansions;

            foreach (var worldRect in gameSettings.WorldRects.WorldRects)
            {
                _worldRects[worldRect.Name] = worldRect;
            }
        }

        logger.LogInformation("Loaded gameSettings.xml with {ItemsCount} items", _items.Count);
        logger.LogInformation("Loaded {LevelsCount} levels", _levels.Count);
        logger.LogInformation("Loaded {ReputationLevelsCount} social levels", _reputationLevels.Count);
        logger.LogInformation("Loaded {RandomModifiersCount} random modifiers", _randomModifiers.Count);
        logger.LogInformation("Loaded {CollectionsCount} collections", _collections.Count);
        logger.LogInformation("Loaded {WorldRectsCount} world rects", _worldRects.Count);
        logger.LogInformation("Loaded {ExpansionsCount} expansions", _expansions.Count);

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

    public double GetDouble(string name)
    {
        return (double)_settings[name];
    }

    public string? GetCollectionByItemName(string itemName)
    {
        if (!_isInitialized)
            throw new InvalidOperationException("GameSettingsManager not initialized");

        foreach (var collection in _collections)
        {
            if (collection.Collectables.Collectables.Any(c => c.Name == itemName))
            {
                return collection.Name;
            }
        }

        return null;
    }

    public CollectionSetting? GetCollectionByName(string collectionName)
    {
        if (!_isInitialized)
            throw new InvalidOperationException("GameSettingsManager not initialized");

        return _collections.FirstOrDefault(x => x.Name == collectionName);
    }

    public WorldRectItem? GetWorldRect(string name)
    {
        if (!_isInitialized)
            throw new InvalidOperationException("GameSettingsManager not initialized");

        return _worldRects.TryGetValue(name, out var rect) ? rect : null;
    }

    public IReadOnlyCollection<ExpansionSetting> GetExpansions()
    {
        return _expansions.AsReadOnly();
    }
}