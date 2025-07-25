using System.Text.Json.Serialization;
using CityVilleDotnet.Domain.Entities;

namespace CityVilleDotnet.Domain.GameEntities;

public class QuestDto
{
    [JsonPropertyName("name")] public required string Name { get; set; }

    [JsonPropertyName("complete")] public int Complete { get; set; }

    [JsonPropertyName("progress")] public required int[] Progress { get; set; }

    [JsonPropertyName("purchased")] public required int[] Purchased { get; set; }
}

public static class QuestDtoMapper
{
    public static QuestDto ToDto(this Quest model)
    {
        return new QuestDto()
        {
            Name = model.Name,
            Complete = model.QuestType == QuestType.Completed ? 1 : 0,
            Progress = model.Progress,
            Purchased = model.Purchased
        };
    }
}