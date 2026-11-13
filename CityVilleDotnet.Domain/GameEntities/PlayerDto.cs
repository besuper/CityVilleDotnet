using System.Text.Json.Serialization;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using FluorineFx;

namespace CityVilleDotnet.Domain.GameEntities;

public class PlayerDto
{
    [JsonPropertyName("uid")] public string Uid { get; set; } = "333";

    [JsonPropertyName("firstPlayDate")] public long FirstPlayDate { get; set; }

    [JsonPropertyName("lastTrackingTimestamp")]
    public long LastTrackingTimestamp { get; set; } = 0;

    [JsonPropertyName("playerNews")] public List<object> PlayerNews { get; set; } = [];

    [JsonPropertyName("neighbors")] public List<NeighborDto> Neighbors { get; set; } = [];

    [JsonPropertyName("wishlist")] public List<object> Wishlist { get; set; } = [];

    [JsonPropertyName("options")] public required OptionsDto Options { get; set; }

    [JsonPropertyName("commodities")] public required CommoditiesDto Commodities { get; set; }

    [JsonPropertyName("inventory")] public required InventoryDto Inventory { get; set; }

    [JsonPropertyName("requestInventory")] public ASObject RequestInventory { get; set; } = new();

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

    [JsonPropertyName("energy")] public int Energy { get; set; } = 12;

    [JsonPropertyName("energyMax")] public int EnergyMax { get; set; } = 12;
    [JsonPropertyName("lastEnergyCheck")] public int LastEnergyCheck { get; set; } = 0;

    [JsonPropertyName("seenFlags")] public ASObject SeenFlags { get; set; } = new ASObject();
    [JsonPropertyName("coupons")] public List<string> Coupons { get; set; } = [];
    [JsonPropertyName("featureThemes")] public List<string> FeatureThemes { get; set; } = [];
    [JsonPropertyName("associatedCoupons")] public ASObject AssociatedCoupons { get; set; } = new();
    [JsonPropertyName("flagContainer")] public required List<ASObject> FlagContainer { get; set; }

    [JsonPropertyName("expansionsPurchased")]
    public int ExpansionsPurchased { get; set; } = 0;

    [JsonPropertyName("collections")] public ASObject Collections { get; set; } = new();

    [JsonPropertyName("completedCollections")]
    public ASObject CompletedCollections { get; set; } = new();

    [JsonPropertyName("collectionTradeIns")]
    public ASObject CollectionTradeIns { get; set; } = new();

    [JsonPropertyName("licenses")] public ASObject Licenses { get; set; } = new();

    [JsonPropertyName("Orders")] public ASObject Orders { get; set; } = new();

    [JsonPropertyName("rollCounter")] public int RollCounter { get; set; } = 0;
    [JsonPropertyName("rollCounterMap")] public ASObject RollCounterMap { get; set; } = new();
    [JsonPropertyName("featureData")] public required ASObject FeatureData { get; set; }

    [JsonPropertyName("npc_cloud_visible")]
    public bool ShowNpcCloud { get; set; }

    [JsonPropertyName("fastbuild")] public bool FastBuild { get; set; } = true;

    [JsonPropertyName("storageComponent")] public Dictionary<string, object> StorageComponent { get; set; } = new();

    [JsonPropertyName("additionalWareHouseSlots")]
    public int AdditionalWareHouseSlots { get; set; } = 0;

    [JsonPropertyName("quests")] public List<QuestDto> ActiveQuests { get; set; } = [];
}

public static class PlayerDtoMapper
{
    // 2013 now only use deco_2 instead of building name (decoration, warehouse_deluxe) 
    public static Dictionary<string, object> ToStorageComponentDto(this Player model)
    {
        var mStorage = new Dictionary<string, object>
        {
            ["deco_2"] = new Dictionary<string, object>
            {
                // capacity upgraded to 80/115/150 by capacity_set items
                ["deco_2"] = model.ToStorageUnitDto(model.GetDecoStorageCapacity()) // Merge every items inside the same mStorage
            }
        };
        
        // send empty decoration and warehouse_deluxe mStorage to avoid the client crash 
        foreach (var obj in model.GetWorld().Objects.Where(x => x.ClassName == BuildingClassType.ItemStorage))
        {
            var gameItem = GameSettingsManager.Instance.GetItem(obj.ItemName);
            var upgradeItem = gameItem?.Upgrade is null ? null : GameSettingsManager.Instance.GetItem(gameItem.Upgrade.Name);

            foreach (var storageUnit in new[] { gameItem?.StorageUnit, upgradeItem?.StorageUnit })
            {
                if (storageUnit?.StorageType is null || storageUnit.StorageKey is null)
                {
                    continue;
                }

                if (!mStorage.ContainsKey(storageUnit.StorageType))
                {
                    mStorage[storageUnit.StorageType] = new Dictionary<string, object>();
                }

                ((Dictionary<string, object>)mStorage[storageUnit.StorageType])[storageUnit.StorageKey] = new Dictionary<string, object>
                {
                    ["m_storage"] = new Dictionary<string, object>(),
                    ["m_capacity"] = storageUnit.InitialCapacity,
                    ["m_maxCapacity"] = storageUnit.MaxCapacity
                };
            }
        }

        return new Dictionary<string, object>
        {
            ["m_storage"] = mStorage
        };
    }

    private static Dictionary<string, object> ToStorageUnitDto(this Player model, int capacity)
    {
        var innerStorage = new Dictionary<string, object>();

        foreach (var group in model.InventoryItems.Where(x => x.StorageType is not null).GroupBy(x => x.Name))
        {
            if (group.Any(x => x.StoredObject is null))
            {
                innerStorage[group.Key] = group.Sum(x => x.Amount);
            }
            else
            {
                innerStorage[group.Key] = group
                    .SelectMany(x => Enumerable.Repeat(x.StoredObject!, x.Amount))
                    .Select(x => x.ToDto())
                    .ToList();
            }
        }

        return new Dictionary<string, object>
        {
            ["m_storage"] = innerStorage,
            ["m_capacity"] = capacity,
            ["m_maxCapacity"] = capacity
        };
    }
}