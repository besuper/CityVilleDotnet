using Microsoft.Extensions.Logging;
using System.Xml.Serialization;
using CityVilleDotnet.Common.Settings.QuestSettings;

namespace CityVilleDotnet.Common.Settings;

public class QuestSettingsManager
{
    public static readonly List<string> TaskActions = ["seenQuest"];

    private static QuestSettingsManager? _instance;
    private static readonly object Lock = new();
    private readonly Dictionary<string, QuestItem> _items = new();
    private bool _isInitialized = false;

    public static QuestSettingsManager Instance
    {
        get
        {
            if (_instance is null)
            {
                lock (Lock)
                {
                    _instance ??= new QuestSettingsManager();
                }
            }

            return _instance;
        }
    }

    public void Initialize(ILogger<QuestSettingsManager> logger)
    {
        if (_isInitialized)
            return;

        var serializer = new XmlSerializer(typeof(GameQuests));

        using (var fileStream = new FileStream("wwwroot/questSettings.xml", FileMode.Open))
        {
            var gameSettings = (GameQuests)serializer.Deserialize(fileStream);

            if (gameSettings?.Quests is not null)
            {
                foreach (var item in gameSettings.Quests)
                {
                    _items[item.Name] = item;

                    foreach (var task in item.Tasks.Tasks)
                    {
                        if (!TaskActions.Contains(task.Action))
                        {
                            TaskActions.Add(task.Action);
                        }
                    }
                }
            }
        }

        logger.LogInformation("Loaded questSettings.xml with {ItemsCount} items and {TaskActionsCount} task actions", _items.Count, TaskActions.Count);

        _isInitialized = true;
    }

    public QuestItem? GetItem(string itemName)
    {
        if (!_isInitialized)
            throw new InvalidOperationException("QuestSettingsManager not initialized");

        return _items.TryGetValue(itemName, out var item) ? item : null;
    }
}