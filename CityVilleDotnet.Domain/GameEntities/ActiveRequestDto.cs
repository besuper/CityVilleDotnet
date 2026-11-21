using System.Text.Json.Serialization;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Entities;

namespace CityVilleDotnet.Domain.GameEntities;

public class ActiveRequestDto
{
    [JsonPropertyName("ts")] public long Ts { get; set; }

    [JsonPropertyName("zids")] public List<string> Zids { get; set; } = [];
}

public static class ActiveRequestDtoMapper
{
    // Player::addActiveRequests reads only the item request type
    public static Dictionary<string, ActiveRequestDto> ToActiveRequestDtos(this Player model)
    {
        if (model.GetActiveItemRequests(ServerUtils.TimeProvider.GetUtcNow().UtcDateTime) is not { } activeRequests)
        {
            return [];
        }

        return new Dictionary<string, ActiveRequestDto>
        {
            ["request_gift_request"] = new() { Ts = UserDtoMapper.ToUnixSeconds(activeRequests.Ts), Zids = activeRequests.RecipientSnuids.Select(x => $"i{x}").ToList() }
        };
    }
}
