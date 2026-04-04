using System.Text.Json.Serialization;
using FluorineFx;

namespace CityVilleDotnet.Domain.GameEntities;

public class PlayerDto
{
    [JsonPropertyName("uid")] public string Uid { get; set; } = "333";

    [JsonPropertyName("lastTrackingTimestamp")]
    public int LastTrackingTimestamp { get; set; } = 0;

    [JsonPropertyName("playerNews")] public List<object> PlayerNews { get; set; } = [];

    [JsonPropertyName("neighbors")] public List<NeighborDto> Neighbors { get; set; } = [];

    [JsonPropertyName("wishlist")] public List<object> Wishlist { get; set; } = [];

    [JsonPropertyName("options")] public OptionsDto? Options { get; set; }

    [JsonPropertyName("commodities")] public CommoditiesDto? Commodities { get; set; }

    [JsonPropertyName("inventory")] public InventoryDto? Inventory { get; set; }

    [JsonPropertyName("gold")] public int Gold { get; set; } = 500;

    [JsonPropertyName("cash")] public int Cash { get; set; } = 0;

    [JsonPropertyName("level")] public int Level { get; set; } = 1;

    [JsonPropertyName("xp")] public int Xp { get; set; } = 0;
    [JsonPropertyName("lightLevel")] public int LightLevel { get; set; } = 0;
    [JsonPropertyName("paidEnergy")] public int PaidEnergy { get; set; } = 0;

    [JsonPropertyName("m_energyModifiers")]
    public List<object> EnergyModifiers { get; set; }

    [JsonPropertyName("socialLevel")] public int SocialLevel { get; set; } = 1;

    [JsonPropertyName("socialXp")] public int SocialXp { get; set; } = 0;

    [JsonPropertyName("energy")] public int Energy { get; set; } = 12;

    [JsonPropertyName("energyMax")] public int EnergyMax { get; set; } = 12;
    [JsonPropertyName("lastEnergyCheck")] public int LastEnergyCheck { get; set; } = 0;

    [JsonPropertyName("seenFlags")] public ASObject SeenFlags { get; set; } = new ASObject();
    [JsonPropertyName("flagContainer")] public required List<ASObject> FlagContainer { get; set; }

    [JsonPropertyName("expansionsPurchased")]
    public int ExpansionsPurchased { get; set; } = 0;

    [JsonPropertyName("collections")] public ASObject Collections { get; set; } = new();

    [JsonPropertyName("completedCollections")]
    public ASObject CompletedCollections { get; set; } = new();

    [JsonPropertyName("licenses")] public ASObject Licenses { get; set; } = new();

    [JsonPropertyName("Orders")] public ASObject Orders { get; set; } = new();

    [JsonPropertyName("rollCounter")] public int RollCounter { get; set; } = 0;
    [JsonPropertyName("featureData")] public required ASObject FeatureData { get; set; }

    [JsonPropertyName("npc_cloud_visible")]
    public bool ShowNpcCloud { get; set; }

    [JsonPropertyName("fastbuild")] public bool FastBuild { get; set; } = true;

    [JsonPropertyName("storageComponent")] public ASObject StorageComponent { get; set; } = new();

    [JsonPropertyName("additionalWareHouseSlots")]
    public int AdditionalWareHouseSlots { get; set; } = 0;
}