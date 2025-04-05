using System.Text.Json.Serialization;

namespace CityVilleDotnet.Api.Services.UserService.Domain;

public class WorldObject
{
    [JsonIgnore]
    public Guid Id { get; set; }

    [JsonPropertyName("itemName")]
    public string ItemName { get; set; }

    [JsonPropertyName("className")]
    public string ClassName { get; set; }

    [JsonPropertyName("contractName")]
    public string? ContractName { get; set; }

    /*[JsonPropertyName("components")]
    public object? Components { get; set; }*/

    [JsonPropertyName("deleted")]
    public bool Deleted { get; set; }

    [JsonPropertyName("tempId")]
    public int TempId { get; set; }

    [JsonPropertyName("state")]
    public string State { get; set; }

    [JsonPropertyName("direction")]
    public int Direction { get; set; }

    [JsonPropertyName("position")]
    public WorldObjectPosition? Position { get; set; }

    [JsonPropertyName("id")]
    public int WorldFlatId { get; set; }
}