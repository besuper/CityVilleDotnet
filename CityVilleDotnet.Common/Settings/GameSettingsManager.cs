using Microsoft.Extensions.Logging;
using System.Xml.Serialization;
using CityVilleDotnet.Common.Settings.GameSettings;

namespace CityVilleDotnet.Common.Settings;

public class GameSettingsManager
{
    private static GameSettingsManager? _instance;
    private static readonly object Lock = new();
    private readonly Dictionary<string, GameItem?> _items;
    private readonly Dictionary<string, RandomModifierTable> _randomModifiers;
    private readonly Dictionary<string, RandomModifierPack> _randomModifierPacks;
    private readonly Dictionary<string, WorldRectItem> _worldRects;
    private readonly Dictionary<string, DynamicExpansionItem> _dynamicExpansions;
    private readonly Dictionary<string, WorldConfigItem> _worldConfigs;
    private FarmingSettings _farmSettings;
    private List<LevelItem> _levels = [];
    private List<ReputationItem> _reputationLevels = [];
    private List<CollectionSetting> _collections = [];
    private List<ExpansionSetting> _expansions = [];
    private bool _isInitialized;

    private GameSettingsManager()
    {
        _items = new Dictionary<string, GameItem?>();
        _randomModifiers = new Dictionary<string, RandomModifierTable>();
        _randomModifierPacks = new Dictionary<string, RandomModifierPack>();
        _worldRects = new Dictionary<string, WorldRectItem>();
        _dynamicExpansions = new Dictionary<string, DynamicExpansionItem>();
        _worldConfigs = new Dictionary<string, WorldConfigItem>();
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

        using (var fileStream = new FileStream(path, FileMode.Open))
        {
            var content = serializer.Deserialize(fileStream);

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

            if (gameSettings.ModifierPacks?.Packs is not null)
            {
                foreach (var pack in gameSettings.ModifierPacks.Packs)
                {
                    _randomModifierPacks[pack.Id] = pack;
                }
            }

            _levels = gameSettings.Levels.Levels;
            _reputationLevels = gameSettings.Reputation.Levels;
            _farmSettings = gameSettings.Farming;

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

            if (gameSettings.DynamicExpansions is not null)
            {
                foreach (var dynamicExpansion in gameSettings.DynamicExpansions.Expansions)
                {
                    _dynamicExpansions[dynamicExpansion.Id] = dynamicExpansion;
                }
            }

            if (gameSettings.WorldConfigs is not null)
            {
                foreach (var worldConfig in gameSettings.WorldConfigs.WorldConfigs)
                {
                    _worldConfigs[worldConfig.Name] = worldConfig;
                }
            }
        }

        logger.LogInformation("Loaded gameSettings.xml with {ItemsCount} items", _items.Count);
        logger.LogInformation("Loaded {LevelsCount} levels", _levels.Count);
        logger.LogInformation("Loaded {ReputationLevelsCount} social levels", _reputationLevels.Count);
        logger.LogInformation("Loaded {RandomModifiersCount} random modifiers", _randomModifiers.Count);
        logger.LogInformation("Loaded {RandomModifierPacksCount} random modifier packs", _randomModifierPacks.Count);
        logger.LogInformation("Loaded {CollectionsCount} collections", _collections.Count);
        logger.LogInformation("Loaded {WorldRectsCount} world rects", _worldRects.Count);
        logger.LogInformation("Loaded {ExpansionsCount} expansions", _expansions.Count);
        logger.LogInformation("Loaded {DynamicExpansionsCount} dynamic expansions", _dynamicExpansions.Count);
        logger.LogInformation("Loaded {WorldConfigsCount} world configs", _worldConfigs.Count);

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

    public List<RandomModifier>? GetRandomModifierPackModifiers(string packId)
    {
        if (!_isInitialized)
            throw new InvalidOperationException("GameSettingsManager not initialized");

        return _randomModifierPacks.TryGetValue(packId, out var pack) ? pack.Modifiers : null;
    }

    public IReadOnlyCollection<LevelItem> GetLevels()
    {
        return _levels.AsReadOnly();
    }

    public IReadOnlyCollection<ReputationItem> GetSocialLevels()
    {
        return _reputationLevels.AsReadOnly();
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

    public DynamicExpansionItem? GetDynamicExpansion(string name)
    {
        if (!_isInitialized)
            throw new InvalidOperationException("GameSettingsManager not initialized");

        return _dynamicExpansions.TryGetValue(name, out var item) ? item : null;
    }

    public WorldRectItem? GetWorldRect(string name)
    {
        if (!_isInitialized)
            throw new InvalidOperationException("GameSettingsManager not initialized");

        return _worldRects.TryGetValue(name, out var rect) ? rect : null;
    }

    public WorldConfigItem? GetWorldConfig(string name)
    {
        if (!_isInitialized)
            throw new InvalidOperationException("GameSettingsManager not initialized");

        return _worldConfigs.TryGetValue(name, out var config) ? config : null;
    }

    public IReadOnlyCollection<ExpansionSetting> GetExpansions()
    {
        return _expansions.AsReadOnly();
    }

    public FarmingSettings GetSettings()
    {
        return _farmSettings;
    }
}