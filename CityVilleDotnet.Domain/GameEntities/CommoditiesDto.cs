using CityVilleDotnet.Domain.Entities;
using System.Text.Json.Serialization;

namespace CityVilleDotnet.Domain.GameEntities;

public class CommoditiesDto
{
    [JsonPropertyName("storage")]
    public StorageDto? Storage { get; set; }
}

public static class CommoditiesDtoMapper
{
    public static CommoditiesDto ToDto(this Commodities model)
    {
        return new CommoditiesDto()
        {
            Storage = model.Storage?.ToDto(),
        };
    }
}