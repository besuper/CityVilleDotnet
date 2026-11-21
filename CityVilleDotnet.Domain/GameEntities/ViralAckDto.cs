using System.Text.Json.Serialization;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Entities;

namespace CityVilleDotnet.Domain.GameEntities;

public class ViralAckDto
{
    [JsonPropertyName("helpers")] public List<ViralAckHelperDto> Helpers { get; set; } = [];
}

public static class ViralAckDtoMapper
{
    // ViralAckData: one entry per building of the played world, keyed "va<worldObjectId>"
    public static Dictionary<string, ViralAckDto> ToViralAckDtos(this Player model)
    {
        return model.GetViralAcks(model.LastPlayedWorldType, ServerUtils.TimeProvider.GetUtcNow().UtcDateTime)
            .GroupBy(x => x.TargetObjectId)
            .ToDictionary(g => $"va{g.Key}", g => new ViralAckDto
            {
                Helpers = g.Select(x => new ViralAckHelperDto
                {
                    Zid = x.HelperSnuid.ToString(),
                    ItemName = x.ItemName,
                    // the client only tests that crewPos exists
                    CrewPos = x.IsCrew ? 0 : null,
                    Ts = UserDtoMapper.ToUnixSeconds(x.HelpedAt),
                    TargetObjectName = x.TargetObjectName
                }).ToList()
            });
    }
}
