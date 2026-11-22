using System.Text.Json.Serialization;

namespace CityVilleDotnet.Domain.GameEntities;

public class PlayerDto
{
    [JsonPropertyName("uid")] public string Uid { get; set; } = "333";

    [JsonPropertyName("firstPlayDate")] public long FirstPlayDate { get; set; }

    [JsonPropertyName("lastTrackingTimestamp")]
    public long LastTrackingTimestamp { get; set; } = 0;

    [JsonPropertyName("playerNews")] public List<object> PlayerNews { get; set; } = [];

    [JsonPropertyName("lastSuperFeedPostError")] public double LastSuperFeedPostError { get; set; }

    [JsonPropertyName("activeVirals")] public Dictionary<string, ActiveViralDto> ActiveVirals { get; set; } = [];

    [JsonPropertyName("sentGifts")] public Dictionary<string, long> SentGifts { get; set; } = [];

    [JsonPropertyName("activeRequests")] public Dictionary<string, ActiveRequestDto> ActiveRequests { get; set; } = [];

    [JsonPropertyName("neighbors")] public List<NeighborDto> Neighbors { get; set; } = [];

    [JsonPropertyName("wishlist")] public List<string> Wishlist { get; set; } = [];

    [JsonPropertyName("options")] public required OptionsDto Options { get; set; }

    [JsonPropertyName("commodities")] public required CommoditiesDto Commodities { get; set; }

    [JsonPropertyName("inventory")] public required InventoryDto Inventory { get; set; }

    [JsonPropertyName("requestInventory")] public required RequestInventoryDto RequestInventory { get; set; }

    [JsonPropertyName("gold")] public int Gold { get; set; } = 500;

    [JsonPropertyName("cash")] public int Cash { get; set; } = 0;

    [JsonPropertyName("level")] public int Level { get; set; } = 1;

    [JsonPropertyName("xp")] public int Xp { get; set; } = 0;
    [JsonPropertyName("lightLevel")] public int LightLevel { get; set; } = 0;
    [JsonPropertyName("paidEnergy")] public int PaidEnergy { get; set; } = 0;

    [JsonPropertyName("m_energyModifiers")]
    public Dictionary<string, int> EnergyModifiers { get; set; } = new();

    [JsonPropertyName("socialLevel")] public int SocialLevel { get; set; } = 1;

    [JsonPropertyName("socialXp")] public int SocialXp { get; set; } = 0;

    [JsonPropertyName("currentBonusDay")] public int CurrentBonusDay { get; set; }
    [JsonPropertyName("previousBonusDay")] public int PreviousBonusDay { get; set; }

    [JsonPropertyName("energy")] public int Energy { get; set; } = 12;

    [JsonPropertyName("energyMax")] public int EnergyMax { get; set; } = 12;
    [JsonPropertyName("lastEnergyCheck")] public int LastEnergyCheck { get; set; } = 0;
    [JsonPropertyName("regenerableResources")] public Dictionary<string, int> RegenerableResources { get; set; } = [];
    [JsonPropertyName("regenerableLastUpdates")] public Dictionary<string, long> RegenerableLastUpdates { get; set; } = [];

    [JsonPropertyName("seenFlags")] public Dictionary<string, object> SeenFlags { get; set; } = [];
    [JsonPropertyName("coupons")] public List<string> Coupons { get; set; } = [];
    [JsonPropertyName("featureThemes")] public List<string> FeatureThemes { get; set; } = [];
    [JsonPropertyName("associatedCoupons")] public Dictionary<string, List<string>> AssociatedCoupons { get; set; } = [];
    [JsonPropertyName("flagContainer")] public required List<FlagDto> FlagContainer { get; set; }

    [JsonPropertyName("expansionsPurchased")]
    public int ExpansionsPurchased { get; set; } = 0;

    [JsonPropertyName("collections")] public Dictionary<string, Dictionary<string, int>> Collections { get; set; } = [];

    [JsonPropertyName("completedCollections")]
    public Dictionary<string, int> CompletedCollections { get; set; } = [];

    [JsonPropertyName("collectionTradeIns")]
    public Dictionary<string, int> CollectionTradeIns { get; set; } = [];

    [JsonPropertyName("licenses")] public Dictionary<string, int> Licenses { get; set; } = [];

    [JsonPropertyName("sublicenses")] public Dictionary<string, string> SubLicenses { get; set; } = [];

    [JsonPropertyName("Orders")] public Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, OrderDto>>>> Orders { get; set; } = [];

    [JsonPropertyName("rollCounter")] public int RollCounter { get; set; } = 0;
    [JsonPropertyName("rollCounterMap")] public Dictionary<string, int> RollCounterMap { get; set; } = [];
    [JsonPropertyName("featureData")] public EmptyDto FeatureData { get; set; } = new();

    [JsonPropertyName("npc_cloud_visible")]
    public bool ShowNpcCloud { get; set; }

    [JsonPropertyName("fastbuild")] public bool FastBuild { get; set; } = true;

    [JsonPropertyName("storageComponent")] public required StorageComponentDto StorageComponent { get; set; }

    [JsonPropertyName("additionalWareHouseSlots")]
    public int AdditionalWareHouseSlots { get; set; } = 0;

    [JsonPropertyName("quests")] public List<QuestDto> ActiveQuests { get; set; } = [];
}
