using CityVilleDotnet.Domain.Entities;
using System.Text.Json.Serialization;

namespace CityVilleDotnet.Domain.GameEntities;

public class InventoryDto
{
    [JsonPropertyName("count")]
    public int Count { get; set; }

    /*[JsonPropertyName("Items")]
    public Dictionary<object, object> Items { get; set; }*/
}

public static class InventoryDtoMapper
{
    public static InventoryDto ToDto(this Inventory model)
    {
        return new InventoryDto()
        {
            Count = model.Count,
        };
    }
}