using System.Xml.Serialization;

namespace CityVilleDotnet.Common.Settings.QuestSettings;

[Serializable]
[XmlRoot("quests")]
public class GameQuests
{
    [XmlElement("quest")] public required List<QuestItem> Quests { get; set; }
}

[Serializable]
public class QuestItem
{
    [XmlAttribute("name")] public required string Name { get; set; }

    [XmlElement("tasks")] public required TasksContainer Tasks { get; set; }

    [XmlElement("sequels")] public SequelsContainer? Sequels { get; set; }

    [XmlElement("resourceModifiers")] public ResourceModifiers? ResourceModifiers { get; set; }
}

[Serializable]
public class TasksContainer
{
    [XmlElement("task")] public required List<QuestTask> Tasks { get; set; }
}

[Serializable]
public class QuestTask
{
    [XmlAttribute("action")] public required string Action { get; set; }

    [XmlAttribute("type")] public required string Type { get; set; }

    [XmlAttribute("total")] public required string Total { get; set; }
}

[Serializable]
public class SequelsContainer
{
    [XmlElement("sequel")] public List<Sequel>? Sequels { get; set; }
}

[Serializable]
public class Sequel
{
    [XmlAttribute("name")] public required string Name { get; set; }
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