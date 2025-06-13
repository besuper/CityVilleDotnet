using Microsoft.Extensions.Logging;
using System.Xml.Serialization;

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
                    if (item.Name is not null)
                    {
                        _items[item.Name] = item;

                        if (item.Tasks is null) continue;

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
        }

        logger.LogInformation($"Loaded questSettings.xml with {_items.Count} items and {TaskActions.Count} task actions");

        _isInitialized = true;
    }

    public QuestItem? GetItem(string itemName)
    {
        if (!_isInitialized)
            throw new InvalidOperationException("QuestSettingsManager not initialized");

        return _items.TryGetValue(itemName, out var item) ? item : null;
    }


    [Serializable]
    [XmlRoot("quests")]
    public class GameQuests
    {
        [XmlElement("quest")] public List<QuestItem> Quests { get; set; }
    }

    [Serializable]
    public class QuestItem
    {
        [XmlAttribute("name")] public string Name { get; set; }

        [XmlElement("tasks")] public TasksContainer Tasks { get; set; }

        [XmlElement("sequels")] public SequelsContainer? Sequels { get; set; }

        [XmlElement("resourceModifiers")] public ResourceModifiers? ResourceModifiers { get; set; }
    }

    [Serializable]
    public class TasksContainer
    {
        [XmlElement("task")] public List<QuestTask> Tasks { get; set; }
    }

    [Serializable]
    public class QuestTask
    {
        [XmlAttribute("action")] public string Action { get; set; }

        [XmlAttribute("type")] public string Type { get; set; }

        [XmlAttribute("total")] public string Total { get; set; }
    }

    [Serializable]
    public class SequelsContainer
    {
        [XmlElement("sequel")] public List<Sequel>? Sequels { get; set; }
    }

    [Serializable]
    public class Sequel
    {
        [XmlAttribute("name")] public string Name { get; set; }
    }

    [Serializable]
    public class ResourceModifiers
    {
        [XmlElement("questRewards")] public List<QuestRewards>? Rewards { get; set; }
    }

    [Serializable]
    public class QuestRewards
    {
        [XmlAttribute("gold")] public string? Gold { get; set; }

        [XmlAttribute("xp")] public string? Xp { get; set; }

        [XmlAttribute("goods")] public string? Goods { get; set; }

        [XmlAttribute("energy")] public string? Energy { get; set; }

        [XmlAttribute("itemUnlock")] public string? ItemUnlock { get; set; }

        [XmlAttribute("item")] public string? Item { get; set; }
    }
}