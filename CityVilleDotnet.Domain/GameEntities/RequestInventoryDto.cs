using System.Text.Json.Serialization;
using CityVilleDotnet.Domain.Entities;

namespace CityVilleDotnet.Domain.GameEntities;

public class RequestInventoryDto
{
    [JsonPropertyName("m_inventories")] public required Dictionary<string, RequestInventoryStorageDto> Inventories { get; set; }
}

public class RequestInventoryStorageDto
{
    [JsonPropertyName("storage")] public required Dictionary<string, int> Storage { get; set; }
}

public static class RequestInventoryDtoMapper
{
    public static RequestInventoryDto ToRequestInventoryDto(this Player model)
    {
        return new RequestInventoryDto
        {
            Inventories = model.RequestInventoryItems
                .GroupBy(x => x.InventoryName)
                .ToDictionary(g => g.Key, g => new RequestInventoryStorageDto { Storage = g.ToDictionary(x => x.ItemName, x => x.Amount) })
        };
    }
}
