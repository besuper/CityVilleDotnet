using System.Text.Json.Serialization;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Entities;

namespace CityVilleDotnet.Domain.GameEntities;

public class ActiveViralDto
{
    [JsonPropertyName("timesPosted")] public int TimesPosted { get; set; }

    [JsonPropertyName("originTime")] public long OriginTime { get; set; }

    [JsonPropertyName("recipients")] public Dictionary<string, object> Recipients { get; set; } = [];
}

public static class ActiveViralDtoMapper
{
    public static Dictionary<string, ActiveViralDto> ToActiveViralDtos(this Player model)
    {
        return model.GetActiveVirals(ServerUtils.TimeProvider.GetUtcNow().UtcDateTime).ToDictionary(
            x => x.Key,
            x => new ActiveViralDto { TimesPosted = x.TimesPosted, OriginTime = UserDtoMapper.ToUnixSeconds(x.OriginTime) });
    }
}
