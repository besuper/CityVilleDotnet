using CityVilleDotnet.Common.Global;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Settings.GameSettings;
using CityVilleDotnet.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace CityVilleDotnet.Domain.Entities;

public partial class WorldObject
{
    public int? StoredInId { get; private set; }
    public int? StorageSlotIndex { get; private set; }
    public List<WorldObjectSupplyResource> SupplyResources { get; private set; } = [];

    // GatedStorageMechanic::initialize
    public int GetStorageCapacity(StorageUnitItem storageUnit)
    {
        return Math.Min(storageUnit.InitialCapacity + CrewMembers.Count, storageUnit.MaxCapacity);
    }

    // BaseStorageMechanic::canStore and its overrides
    public bool CanStore(WorldObject obj, GameItem containerItem, MechanicItem storageMechanic)
    {
        var item = GameSettingsManager.Instance.GetItem(obj.ItemName);

        if (item is null || !item.Storable || containerItem.GetValidation("storage")?.IsValid(x => obj.IsStorageFuncValid(x, item)) != true)
        {
            return false;
        }

        var globalValidatorName = storageMechanic.ClassName switch
        {
            "MunicipalCenterStorageMechanic" => "globalMunicipalCenterStorageValidate",
            "NeighborhoodStorageMechanic" or "SuperNeighborhoodStorageMechanic" => "globalNeighborhoodStorageValidate",
            "MallStorageMechanic" => "globalMallStorageValidate",
            _ => null
        };

        return globalValidatorName is null || GameSettingsManager.Instance.GetValidator(globalValidatorName)?.IsValid(x => obj.IsStorageFuncValid(x, item)) == true;
    }

    private bool IsStorageFuncValid(ValidateFunc func, GameItem item)
    {
        switch (func.Name)
        {
            case "isType":
                return func.Type?.Split(',').Contains(item.Type) == true;
            case "isNotType":
                return func.Type?.Split(',').Contains(item.Type) != true;
            case "hasKeyword":
                return func.Keyword?.Split(',').Any(item.HasKeyword) == true;
            case "doesntHaveKeyword":
                return func.Keyword?.Split(',').Any(item.HasKeyword) != true;
            case "isNotItem":
                return ItemName != func.ItemName;
            case "checkFranchise":
                return (FranchiseLocation is not null) == (func.Check != "false");
            case "isNotActiveObject":
                // todo: rollCall mechanic
                return true;
            default:
                StaticLogger.Current.LogWarning("Unsupported storage validation function {Class}.{Name}", func.Class, func.Name);
                return false;
        }
    }

    public void StoreIn(int containerId, int slotIndex)
    {
        StoredInId = containerId;
        StorageSlotIndex = slotIndex;
    }

    public void TakeOutOfStorage(int x, int y, int direction)
    {
        StoredInId = null;
        StorageSlotIndex = null;
        MoveTo(x, y, Z ?? 0, direction);
    }

    public void ShiftStorageSlot()
    {
        StorageSlotIndex--;
    }

    // StorageSupplyMechanic::getRequiredCommodities
    public static List<(string Type, int Amount)> GetStorageSupplyCommodities(IEnumerable<WorldObject> storedObjects)
    {
        return storedObjects
            .Select(x => GameSettingsManager.Instance.GetItem(x.ItemName))
            .OfType<GameItem>()
            .Where(x => x.CommodityRequired > 0)
            .GroupBy(x => x.GetDefaultCommodityName() is { Length: > 0 } name ? name : "goods", x => x.CommodityRequired!.Value)
            .Select(g => (g.Key, g.Sum()))
            .ToList();
    }

    public bool IsStorageSupplied()
    {
        return SupplyResources.Count > 0;
    }

    public List<string> GetSupplyResourceNames()
    {
        return SupplyResources.OrderBy(x => x.Position).Select(x => x.ItemName).ToList();
    }

    // BaseSupplyMechanic::getCustomerCapacity (no storage supply item has a customerCapacity)
    public int GetSupplyCustomersReq()
    {
        return SupplyResources.Sum(x => GameSettingsManager.Instance.GetItem(x.ItemName)?.CommodityRequired ?? 0);
    }

    // EnergyCostHelper::getFinalCost(owner, "harvestState")
    public int GetStorageSupplyHarvestEnergyCost(GameItem containerItem)
    {
        var cost = GetHarvestEnergyCost(containerItem) + SupplyResources.Sum(x => GetHarvestEnergyCost(GameSettingsManager.Instance.GetItem(x.ItemName)));
        var discount = containerItem.EnergyModifier.FirstOrDefault(x => x.Amount >= SupplyResources.Count)?.Discount ?? 0;

        return discount > 0 ? Math.Max(1, cost - discount) : cost;
    }

    private static int GetHarvestEnergyCost(GameItem? item)
    {
        return int.TryParse(item?.EnergyCost?.Harvest, out var cost) ? cost : 0;
    }

    // StorageSupplyMechanic::generateHarvestState
    public void OpenStorageSupply(IEnumerable<WorldObject> storedObjects)
    {
        if (IsStorageSupplied()) throw new Exception("Building is already supplied");

        SupplyResources.AddRange(storedObjects
            .Where(x => GameSettingsManager.Instance.GetItem(x.ItemName) is not null)
            .Select((x, i) => new WorldObjectSupplyResource(i, x.ItemName)));

        Visits = 0;
        State = WorldObjectState.Open;
    }

    // MallStorageMechanic::handlePlaceFromStorage
    public void RemoveSupplyResource(string itemName, IReadOnlyCollection<WorldObject> remainingStored)
    {
        var resources = SupplyResources.Where(x => x.ItemName == itemName).ToList();

        if (resources.Count <= remainingStored.Count(x => x.ItemName == itemName))
        {
            return;
        }

        SupplyResources.Remove(resources.MaxBy(x => x.Position)!);

        var customersReq = GetSupplyCustomersReq();

        if (customersReq <= 0)
        {
            CloseStorageSupply();
        }
        else if (Visits >= customersReq)
        {
            State = WorldObjectState.ClosedHarvestable;
        }
    }

    public void CloseStorageSupply()
    {
        SupplyResources.Clear();
        State = WorldObjectState.Static;
        Visits = 0;
    }
}
