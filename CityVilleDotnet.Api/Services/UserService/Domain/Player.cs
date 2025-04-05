using System.Text.Json.Serialization;

namespace CityVilleDotnet.Api.Services.UserService.Domain;

public class Player
{
    [JsonIgnore]
    public Guid Id { get; set; }

    [JsonPropertyName("uid")]
    public int Uid { get; set; } = 333;

    [JsonPropertyName("lastTrackingTimestamp")]
    public int LastTrackingTimestamp { get; set; } = 0;

    [JsonPropertyName("playerNews")]
    public List<object> PlayerNews { get; set; } = [];

    [JsonPropertyName("neighbors")]
    public List<object> Neighbors { get; set; } = [];

    [JsonPropertyName("wishlist")]
    public List<object> Wishlist { get; set; } = [];

    [JsonPropertyName("options")]
    public Options? Options { get; set; }

    [JsonPropertyName("commodities")]
    public Commodities? Commodities { get; set; }

    [JsonPropertyName("inventory")]
    public Inventory? Inventory { get; set; }

    [JsonPropertyName("gold")]
    public int Gold { get; set; } = 500;

    [JsonPropertyName("cash")]
    public int Cash { get; set; } = 0;

    [JsonPropertyName("level")]
    public int Level { get; set; } = 1;

    [JsonPropertyName("xp")]
    public int Xp { get; set; } = 0;

    [JsonPropertyName("energy")]
    public int Energy { get; set; } = 12;

    [JsonPropertyName("energyMax")]
    public int EnergyMax { get; set; } = 12;

    [JsonPropertyName("seenFlags")]
    public Dictionary<object, object> SeenFlags { get; set; } = new Dictionary<object, object>();

    [JsonPropertyName("expansionsPurchased")]
    public int ExpansionsPurchased { get; set; } = 0;

    [JsonPropertyName("collections")]
    public Dictionary<object, object> Collections { get; set; } = new Dictionary<object, object>();

    [JsonPropertyName("completedCollections")]
    public Dictionary<object, object> CompletedCollections { get; set; } = new Dictionary<object, object>();

    [JsonPropertyName("licenses")]
    public Dictionary<object, object> Licenses { get; set; } = new Dictionary<object, object>();

    [JsonPropertyName("rollCounter")]
    public int RollCounter { get; set; } = 0;
}
