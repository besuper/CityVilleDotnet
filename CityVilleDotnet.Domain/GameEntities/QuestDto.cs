using System.Text.Json.Serialization;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;

namespace CityVilleDotnet.Domain.GameEntities;

public class QuestDto
{
    [JsonPropertyName("name")] public required string Name { get; set; }

    [JsonPropertyName("complete")] public bool Complete { get; set; }

    [JsonPropertyName("progress")] public required int[] Progress { get; set; }

    [JsonPropertyName("purchased")] public required int[] Purchased { get; set; }
    [JsonPropertyName("expired")] public bool Expired { get; set; }
    [JsonPropertyName("activatedTime")] public long ActivatedTime { get; set; }
    [JsonPropertyName("isNew")] public bool IsNew { get; set; }
}

public static class QuestDtoMapper
{
    public static QuestDto ToDto(this Quest model)
    {
        return new QuestDto
        {
            Name = model.Name,
            Complete = model.QuestType == QuestType.Completed,
            Progress = model.Progress,
            Purchased = model.Purchased,
            Expired = false,
            ActivatedTime = new DateTimeOffset(model.CreatedAt).ToUnixTimeSeconds(),
            IsNew = (DateTime.Now - model.CreatedAt) > TimeSpan.FromSeconds(10)
        };
    }
}