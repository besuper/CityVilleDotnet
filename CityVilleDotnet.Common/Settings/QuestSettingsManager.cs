using Microsoft.Extensions.Logging;
using System.Xml.Serialization;

namespace CityVilleDotnet.Common.Settings;

public class QuestSettingsManager
{
    public static List<string> TASK_ACTIONS = new List<string>() { "seenQuest" };

    private static QuestSettingsManager _instance;
    private static readonly object _lock = new object();
    private Dictionary<string, QuestItem> _items = new Dictionary<string, QuestItem>();
    private bool _isInitialized = false;

    public static QuestSettingsManager Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new QuestSettingsManager();
                    }
                }
            }
            return _instance;
        }
    }

    public void Initialize(ILogger<QuestSettingsManager> logger)
    {
        if (_isInitialized)
            return;

        XmlSerializer serializer = new XmlSerializer(typeof(GameQuests));

        using (FileStream fileStream = new FileStream("wwwroot/questSettings.xml", FileMode.Open))
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
                            if (!TASK_ACTIONS.Contains(task.Action))
                            {
                                TASK_ACTIONS.Add(task.Action);
                            }
                        }
                    }
                }
            }
        }

        logger.LogInformation($"Loaded questSettings.xml with {_items.Count} items and {TASK_ACTIONS.Count} task actions");

        _isInitialized = true;
    }

    public QuestItem GetItem(string itemName)
    {
        if (!_isInitialized)
        {
            throw new InvalidOperationException("QuestSettingsManager not initialized");
        }

        if (_items.TryGetValue(itemName, out QuestItem item))
        {
            return item;
        }

        return null;
    }


    [Serializable]
    [XmlRoot("quests")]
    public class GameQuests
    {
        [XmlElement("quest")]
        public List<QuestItem> Quests { get; set; }
    }

    [Serializable]
    public class QuestItem
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("tasks")]
        public TasksContainer Tasks { get; set; }

        [XmlElement("sequels")]
        public SequelsContainer Sequels { get; set; }
    }

    [Serializable]
    public class TasksContainer
    {
        [XmlElement("task")]
        public List<QuestTask> Tasks { get; set; }
    }

    [Serializable]
    public class QuestTask
    {
        [XmlAttribute("action")]
        public string Action { get; set; }

        [XmlAttribute("type")]
        public string Type { get; set; }
    }

    [Serializable]
    public class SequelsContainer
    {
        [XmlElement("sequel")]
        public List<Sequel> Sequels { get; set; }
    }

    [Serializable]
    public class Sequel
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
    }
}