using Microsoft.Extensions.Logging;
using System.Xml.Serialization;
using CityVilleDotnet.Common.Global;
using CityVilleDotnet.Common.Settings.GameSettings;

namespace CityVilleDotnet.Common.Settings;

public class GameSettingsManager
{
    private static GameSettingsManager? _instance;
    private static readonly object Lock = new();
    private readonly Dictionary<string, GameItem?> _items;
    private readonly Dictionary<string, RandomModifierTable> _randomModifiers;
    private readonly Dictionary<string, RandomModifierPack> _randomModifierPacks;
    private readonly Dictionary<string, LootTable> _lootTables;
    private readonly Dictionary<string, WorldRectItem> _worldRects;
    private readonly Dictionary<string, DynamicExpansionItem> _dynamicExpansions;
    private readonly Dictionary<string, WorldConfigItem> _worldConfigs;
    private readonly Dictionary<string, ValidateItem> _validators;
    private readonly Dictionary<string, TieredValueItem> _tieredValues;
    private List<string> _globalTableProviders = [];
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
        _lootTables = new Dictionary<string, LootTable>();
        _worldRects = new Dictionary<string, WorldRectItem>();
        _dynamicExpansions = new Dictionary<string, DynamicExpansionItem>();
        _worldConfigs = new Dictionary<string, WorldConfigItem>();
        _validators = new Dictionary<string, ValidateItem>();
        _tieredValues = new Dictionary<string, TieredValueItem>();
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
        var derivedItemsCount = 0;

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
            
            var mechanicPacks = gameSettings.MechanicPacks?.Packs
                .Where(x => x.Name is not null)
                .ToDictionary(x => x.Name!, x => x.Mechanics);

            if (mechanicPacks is not null)
            {
                foreach (var item in _items.Values)
                {
                    if (item?.MechanicPack is null || item.Mechanics is not null) continue;

                    if (mechanicPacks.TryGetValue(item.MechanicPack, out var mechanics))
                        item.Mechanics = mechanics;
                }
            }

            derivedItemsCount = GameItemInheritance.Resolve(_items);

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

            if (gameSettings.LootTables?.Tables is not null)
            {
                foreach (var lootTable in gameSettings.LootTables.Tables)
                {
                    _lootTables[lootTable.Name] = lootTable;
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

            if (gameSettings.TieredValues is not null)
            {
                foreach (var tieredValue in gameSettings.TieredValues.Values)
                {
                    _tieredValues[tieredValue.Name] = tieredValue;
                }
            }

            if (gameSettings.Validators is not null)
            {
                foreach (var validator in gameSettings.Validators.Validators)
                {
                    _validators[validator.Name] = validator;
                }
            }

            _globalTableProviders = _items.Values
                .Where(x => x is not null && x.GetGlobalTableModifiers().Count > 0)
                .Select(x => x!.Name)
                .ToList();
        }

        logger.LogInformation("Loaded gameSettings.xml with {ItemsCount} items ({DerivedItemsCount} derived from a parent item)", _items.Count, derivedItemsCount);
        logger.LogInformation("Loaded {LevelsCount} levels", _levels.Count);
        logger.LogInformation("Loaded {ReputationLevelsCount} social levels", _reputationLevels.Count);
        logger.LogInformation("Loaded {RandomModifiersCount} random modifiers", _randomModifiers.Count);
        logger.LogInformation("Loaded {RandomModifierPacksCount} random modifier packs", _randomModifierPacks.Count);
        logger.LogInformation("Loaded {LootTablesCount} loot tables", _lootTables.Count);
        logger.LogInformation("Loaded {CollectionsCount} collections", _collections.Count);
        logger.LogInformation("Loaded {WorldRectsCount} world rects", _worldRects.Count);
        logger.LogInformation("Loaded {ExpansionsCount} expansions", _expansions.Count);
        logger.LogInformation("Loaded {DynamicExpansionsCount} dynamic expansions", _dynamicExpansions.Count);
        logger.LogInformation("Loaded {WorldConfigsCount} world configs", _worldConfigs.Count);
        logger.LogInformation("Loaded {ValidatorsCount} validators", _validators.Count);
        logger.LogInformation("Loaded {GlobalTableProvidersCount} global table providers", _globalTableProviders.Count);

        _isInitialized = true;
    }

    public GameItem? GetItem(string itemName)
    {
        if (!_isInitialized)
            throw new InvalidOperationException("GameSettingsManager not initialized");

        return _items.TryGetValue(itemName, out var item) ? item : null;
    }

    public List<string> GetOrderedUpgradeChainByRoot(string root)
    {
        if (!_isInitialized)
            throw new InvalidOperationException("GameSettingsManager not initialized");

        var chain = new List<string>();
        var item = GetItem(root);

        while (item is not null)
        {
            chain.Add(item.Name);
            item = item.Upgrade?.Name is not null ? GetItem(item.Upgrade.Name) : null;
        }

        return chain;
    }

    public IReadOnlyList<GameItem> GetItemsByKeyword(string keyword)
    {
        if (!_isInitialized)
            throw new InvalidOperationException("GameSettingsManager not initialized");

        return _items.Select(x => x.Value).Where(x => x.HasKeyword(keyword)).ToList();
    }

    public RandomModifierTable? GetRandomModifier(string name)
    {
        if (!_isInitialized)
            throw new InvalidOperationException("GameSettingsManager not initialized");

        return _randomModifiers.TryGetValue(name, out var item) ? item : null;
    }

    public LootTable? GetLootTable(string name)
    {
        if (!_isInitialized)
            throw new InvalidOperationException("GameSettingsManager not initialized");

        return _lootTables.TryGetValue(name, out var table) ? table : null;
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

    public bool IsValidatorSatisfied(string? validatorName, int playerLevel)
    {
        if (!_isInitialized)
            throw new InvalidOperationException("GameSettingsManager not initialized");

        if (string.IsNullOrEmpty(validatorName)) return true;

        if (!_validators.TryGetValue(validatorName, out var validator))
        {
            StaticLogger.Current.LogWarning("Can't find validator {ValidatorName}", validatorName);
            return false;
        }

        return validator.IsValid(playerLevel);
    }

    public int GetTieredValue(string? tableName, int tier)
    {
        if (!_isInitialized)
            throw new InvalidOperationException("GameSettingsManager not initialized");

        if (string.IsNullOrEmpty(tableName)) return 0;

        if (!_tieredValues.TryGetValue(tableName, out var tieredValue)) return 0;

        return int.TryParse(tieredValue.GetAmount(tier), out var amount) ? amount : 0;
    }

    public List<string> GetGlobalTableProviders()
    {
        if (!_isInitialized)
            throw new InvalidOperationException("GameSettingsManager not initialized");

        return _globalTableProviders;
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