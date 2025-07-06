using System.Text.Json.Serialization;

namespace CityVilleDotnet.Domain.GameEntities;

public class CommoditiesDto
{
    [JsonPropertyName("storage")]
    public StorageDto? Storage { get; set; }
}