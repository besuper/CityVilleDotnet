using System.Text.Json.Serialization;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using FluorineFx;

namespace CityVilleDotnet.Domain.GameEntities;

public class PlayerDto
{
    [JsonPropertyName("uid")] public string Uid { get; set; } = "333";

    [JsonPropertyName("lastTrackingTimestamp")]
    public long LastTrackingTimestamp { get; set; } = 0;

    [JsonPropertyName("playerNews")] public List<object> PlayerNews { get; set; } = [];

    [JsonPropertyName("neighbors")] public List<NeighborDto> Neighbors { get; set; } = [];

    [JsonPropertyName("wishlist")] public List<object> Wishlist { get; set; } = [];

    [JsonPropertyName("options")] public required OptionsDto Options { get; set; }

    [JsonPropertyName("commodities")] public required CommoditiesDto Commodities { get; set; }

    [JsonPropertyName("inventory")] public required InventoryDto Inventory { get; set; }

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

    [JsonPropertyName("storageComponent")] public Dictionary<string, object> StorageComponent { get; set; } = new();

    [JsonPropertyName("additionalWareHouseSlots")]
    public int AdditionalWareHouseSlots { get; set; } = 0;

    [JsonPropertyName("quests")] public List<QuestDto> ActiveQuests { get; set; } = [];
}

public static class PlayerDtoMapper
{
    public static Dictionary<string, object> ToStorageComponentDto(this Player model)
    {
        var mStorage = new Dictionary<string, object>();

        foreach (var obj in model.GetWorld().Objects)
        {
            if (obj.ClassName != BuildingClassType.ItemStorage) continue;

            var gameItem = GameSettingsManager.Instance.GetItem(obj.ItemName);

            if (gameItem?.StorageUnit is null) continue;

            var storageType = gameItem.StorageUnit.StorageType;
            var storageKey = gameItem.StorageUnit.StorageKey;

            if (storageType is null || storageKey is null) continue;

            if (!mStorage.ContainsKey(storageType))
                mStorage[storageType] = new Dictionary<string, object>();

            var byType = (Dictionary<string, object>)mStorage[storageType];

            if (!byType.ContainsKey(storageKey))
            {
                var innerStorage = new Dictionary<string, object>();

                foreach (var item in model.InventoryItems.Where(x => x.StorageType == storageKey))
                {
                    if (item.StoredObject is null)
                    {
                        innerStorage[item.Name] = item.Amount;
                    }
                    else
                    {
                        var worldObjects = new List<WorldObjectDto>();

                        for (var i = 0; i < item.Amount; i++)
                        {
                            worldObjects.Add(item.StoredObject.ToDto());
                        }

                        innerStorage[item.Name] = worldObjects;
                    }
                }

                byType[storageKey] = new Dictionary<string, object>
                {
                    ["m_storage"] = innerStorage,
                    ["m_capacity"] = gameItem.StorageUnit.InitialCapacity,
                    ["m_maxCapacity"] = gameItem.StorageUnit.MaxCapacity
                };
            }
        }

        return new Dictionary<string, object>
        {
            ["m_storage"] = mStorage
        };
    }
}