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
    private Dictionary<string, object> _settings;
    private List<LevelItem> _levels = [];
    private List<ReputationItem> _reputationLevels = [];
    private List<CollectionSetting> _collections = [];
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

        var serializer = new XmlSerializer(typeof(GameSettings.GameSettings));

        using (var fileStream = new FileStream("wwwroot/gameSettings.xml", FileMode.Open))
        {
            var gameSettings = (GameSettings.GameSettings)serializer.Deserialize(fileStream);

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
            _collections = gameSettings.Collections.Collections;
        }

        logger.LogInformation("Loaded gameSettings.xml with {ItemsCount} items", _items.Count);
        logger.LogInformation("Loaded {LevelsCount} levels", _levels.Count);
        logger.LogInformation("Loaded {ReputationLevelsCount} social levels", _reputationLevels.Count);
        logger.LogInformation("Loaded {RandomModifiersCount} random modifiers", _randomModifiers.Count);
        logger.LogInformation("Loaded {CollectionsCount} collections", _collections.Count);

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
}