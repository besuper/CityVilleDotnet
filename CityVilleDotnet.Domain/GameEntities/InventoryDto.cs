using CityVilleDotnet.Domain.Entities;
using FluorineFx;
using System.Text.Json.Serialization;

namespace CityVilleDotnet.Domain.GameEntities;

public class InventoryDto
{
    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("items")]
    public required ASObject Items { get; set; }
}

public static class InventoryDtoMapper
{
    public static InventoryDto ToDto(this Inventory model)
    {
        return new InventoryDto()
        {
            Count = model.Count(),
            Items = new ASObject(model.Items.ToDictionary(x => x.Name, x => (object)x.Amount))
        };
    }
}