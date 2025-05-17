using CityVilleDotnet.Domain.Entities;
using System.Text.Json.Serialization;

namespace CityVilleDotnet.Domain.GameEntities;

public class StorageDto
{
    [JsonPropertyName("goods")]
    public int Goods { get; set; }
}

public static class StorageDtoMapper
{
    public static StorageDto ToDto(this Storage model)
    {
        return new StorageDto()
        {
            Goods = model.Goods,
        };
    }
}