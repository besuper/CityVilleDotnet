using System.Text.Json.Serialization;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using FluorineFx;

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
    // TODO: Persist this, task like incrementalPopulationCount will loose its progress after reload
    [JsonPropertyName("extraData")] public ASObject ExtraData { get; set; } = new();
}

public static class QuestDtoMapper
{
    public static QuestDto ToDto(this Quest model, Player player)
    {
        return new QuestDto
        {
            Name = model.Name,
            Complete = model.QuestType == QuestType.Completed,
            Progress = model.Progress,
            Purchased = model.Purchased,
            Expired = false,
            ActivatedTime = new DateTimeOffset(model.CreatedAt).ToUnixTimeSeconds(),
            IsNew = (ServerUtils.GetCurrentDateTime() - model.CreatedAt) < TimeSpan.FromSeconds(10),
            ExtraData = BuildExtraData(model, player)
        };
    }

    public static List<QuestDto> ToQuestComponent(this Player player)
    {
        return player.Quests.Where(x => x.QuestType == QuestType.Active).Select(x => x.ToDto(player)).ToList();
    }

    private static ASObject BuildExtraData(Quest model, Player player)
    {
        var extraData = new ASObject();
        var tasks = QuestSettingsManager.Instance.GetItem(model.Name)?.Tasks.Tasks ?? [];

        for (var index = 0; index < tasks.Count; index++)
        {
            switch (tasks[index].Action)
            {
                case "countLights":
                    extraData["oldLightCount"] = player.CityLightCount - model.Progress[index];
                    break;
            }
        }

        return extraData;
    }
}
