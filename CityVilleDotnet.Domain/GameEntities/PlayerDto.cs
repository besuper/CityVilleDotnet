using CityVilleDotnet.Domain.Entities;
using System.Text.Json.Serialization;
using FluorineFx;

namespace CityVilleDotnet.Domain.GameEntities;

public class PlayerDto
{
    [JsonPropertyName("uid")] public string Uid { get; set; } = "333";

    [JsonPropertyName("lastTrackingTimestamp")]
    public int LastTrackingTimestamp { get; set; } = 0;

    [JsonPropertyName("playerNews")] public List<object> PlayerNews { get; set; } = [];

    [JsonPropertyName("neighbors")] public List<object> Neighbors { get; set; } = [];

    [JsonPropertyName("wishlist")] public List<object> Wishlist { get; set; } = [];

    [JsonPropertyName("options")] public OptionsDto? Options { get; set; }

    [JsonPropertyName("commodities")] public CommoditiesDto? Commodities { get; set; }

    [JsonPropertyName("inventory")] public InventoryDto? Inventory { get; set; }

    [JsonPropertyName("gold")] public int Gold { get; set; } = 500;

    [JsonPropertyName("cash")] public int Cash { get; set; } = 0;

    [JsonPropertyName("level")] public int Level { get; set; } = 1;

    [JsonPropertyName("xp")] public int Xp { get; set; } = 0;

    [JsonPropertyName("energy")] public int Energy { get; set; } = 12;

    [JsonPropertyName("energyMax")] public int EnergyMax { get; set; } = 12;

    [JsonPropertyName("seenFlags")] public ASObject SeenFlags { get; set; } = new ASObject();

    [JsonPropertyName("expansionsPurchased")]
    public int ExpansionsPurchased { get; set; } = 0;

    [JsonPropertyName("collections")] public Dictionary<object, object> Collections { get; set; } = new Dictionary<object, object>();

    [JsonPropertyName("completedCollections")]
    public Dictionary<object, object> CompletedCollections { get; set; } = new Dictionary<object, object>();

    [JsonPropertyName("licenses")] public Dictionary<object, object> Licenses { get; set; } = new Dictionary<object, object>();

    [JsonPropertyName("rollCounter")] public int RollCounter { get; set; } = 0;
}

public static class PlayerDtoMapper
{
    public static PlayerDto ToDto(this Player model)
    {
        return new PlayerDto()
        {
            Uid = model.Uid,
            Cash = model.Cash,
            Collections = model.Collections,
            Commodities = model.Commodities?.ToDto(),
            CompletedCollections = model.CompletedCollections,
            Energy = model.Energy,
            EnergyMax = model.EnergyMax,
            ExpansionsPurchased = model.ExpansionsPurchased,
            Gold = model.Gold,
            Inventory = model.Inventory?.ToDto(),
            LastTrackingTimestamp = model.LastTrackingTimestamp,
            Level = model.Level,
            Licenses = model.Licenses,
            Neighbors = model.Neighbors,
            Options = new OptionsDto
            {
                MusicDisabled = model.MusicDisabled,
                SfxDisabled = model.SfxDisabled,
            },
            PlayerNews = model.PlayerNews,
            RollCounter = model.RollCounter,
            SeenFlags = new ASObject(model.SeenFlags.ToDictionary(x => x.Key, x => (object)true)),
            Wishlist = model.Wishlist,
            Xp = model.Xp
        };
    }
}