using System.Text.Json.Serialization;

namespace CityVilleDotnet.Domain.GameEntities;

public class InventoryDto
{
    [JsonPropertyName("count")] public int Count { get; set; }

    [JsonPropertyName("items")] public required Dictionary<string, int> Items { get; set; }
}
