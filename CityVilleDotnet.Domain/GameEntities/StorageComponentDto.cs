using System.Text.Json.Serialization;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;

namespace CityVilleDotnet.Domain.GameEntities;

public class StorageComponentDto
{
    [JsonPropertyName("m_storage")] public required Dictionary<string, Dictionary<string, StorageUnitDto>> Storage { get; set; }
}

public class StorageUnitDto
{
    // An item amount, or the list of stored objects
    [JsonPropertyName("m_storage")] public required Dictionary<string, object> Storage { get; set; }

    [JsonPropertyName("m_capacity")] public int Capacity { get; set; }

    [JsonPropertyName("m_maxCapacity")] public int MaxCapacity { get; set; }
}

public static class StorageComponentDtoMapper
{
    // 2013 now only use deco_2 instead of building name (decoration, warehouse_deluxe)
    public static StorageComponentDto ToStorageComponentDto(this Player model)
    {
        var storage = new Dictionary<string, Dictionary<string, StorageUnitDto>>
        {
            // capacity upgraded to 80/115/150 by capacity_set items, every item is merged inside the same storage
            ["deco_2"] = new() { ["deco_2"] = model.ToStorageUnitDto(model.GetDecoStorageCapacity()) }
        };

        // send empty decoration and warehouse_deluxe storages to avoid the client crash
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

                if (!storage.TryGetValue(storageUnit.StorageType, out var units))
                {
                    units = [];
                    storage[storageUnit.StorageType] = units;
                }

                units[storageUnit.StorageKey] = new StorageUnitDto
                {
                    Storage = [],
                    Capacity = storageUnit.InitialCapacity,
                    MaxCapacity = storageUnit.MaxCapacity
                };
            }
        }

        return new StorageComponentDto { Storage = storage };
    }

    private static StorageUnitDto ToStorageUnitDto(this Player model, int capacity)
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

        return new StorageUnitDto
        {
            Storage = innerStorage,
            Capacity = capacity,
            MaxCapacity = capacity
        };
    }
}
