using System.Text.Json.Serialization;

namespace CityVilleDotnet.Api.Services.UserService.Domain;

public class Inventory
{
    [JsonIgnore]
    public Guid Id { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }

    /*[JsonPropertyName("Items")]
    public Dictionary<object, object> Items { get; set; }*/
}
