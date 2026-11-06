using CityVilleDotnet.Common.Global;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Settings.GameSettings;
using Microsoft.Extensions.Logging;

namespace CityVilleDotnet.Domain.Entities;

public partial class WorldObject
{
    public int? StoredInId { get; private set; }
    public int? StorageSlotIndex { get; private set; }

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
}
