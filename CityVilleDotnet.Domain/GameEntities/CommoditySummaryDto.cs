using System.Text.Json.Serialization;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;

namespace CityVilleDotnet.Domain.GameEntities;

public class CommoditySummaryDto
{
    [JsonPropertyName("commodity")] public required Dictionary<string, CommodityCapacityDto> Commodity { get; set; }
}

public class CommodityCapacityDto
{
    [JsonPropertyName("id")] public required string Id { get; set; }

    [JsonPropertyName("capacity")] public int Capacity { get; set; }
}

public static class CommoditySummaryDtoMapper
{
    public static CommoditySummaryDto ToCommoditySummaryDto(this World model)
    {
        return new CommoditySummaryDto
        {
            Commodity = model.Objects
                .SelectMany(x => GameSettingsManager.Instance.GetItem(x.ItemName)?.Commodity ?? [])
                .GroupBy(x => x.Name)
                .ToDictionary(g => g.Key, g => new CommodityCapacityDto { Id = g.Key, Capacity = g.Sum(x => x.Capacity) })
        };
    }
}
