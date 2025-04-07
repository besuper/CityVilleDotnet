using System.Text.Json.Serialization;

namespace CityVilleDotnet.Api.Services.QuestService.Domain;

public class Quest
{
    public Quest(string name, int complete, int[] progress, int[] purchased, QuestType questType)
    {
        Id = Guid.NewGuid();
        Name = name;
        Complete = complete;
        Progress = progress;
        Purchased = purchased;
        QuestType = questType;
    }

    public Quest()
    {

    }

    [JsonIgnore]
    public Guid Id { get; private set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("complete")]
    public int Complete { get; set; }

    [JsonPropertyName("progress")]
    public int[] Progress { get; set; }

    [JsonPropertyName("purchased")]
    public int[] Purchased { get; set; }

    [JsonIgnore]
    public QuestType QuestType { get; set; }

    public static Quest Create(string name, int complete, int length, QuestType questType)
    {
        return new Quest(name, complete, new int[length], new int[length], questType);
    }
}
