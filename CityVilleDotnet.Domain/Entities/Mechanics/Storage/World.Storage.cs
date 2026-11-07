using CityVilleDotnet.Common.Settings.GameSettings;
using CityVilleDotnet.Domain.Enums;

namespace CityVilleDotnet.Domain.Entities;

public partial class World
{
    public List<WorldObject> GetStoredObjects(WorldObject container)
    {
        return Objects.Where(x => x.StoredInId == container.WorldFlatId).OrderBy(x => x.StorageSlotIndex).ToList();
    }

    // GameQuestUtility::countSlottedContainerObjects, getObjectsByClass also returns subclasses (SuperNeighborhood extends Neighborhood)
    public int GetMaxStoredObjectsByClass(string className)
    {
        return Objects
            .Where(x => x.StoredInId is null && (x.ClassName.ToString() == className || (className == nameof(BuildingClassType.Neighborhood) && x.ClassName == BuildingClassType.SuperNeighborhood)))
            .Select(x => Objects.Count(o => o.StoredInId == x.WorldFlatId))
            .DefaultIfEmpty()
            .Max();
    }

    // GameQuestUtility::checkThemeparkSlots
    public int GetMaxFilledSlots(IReadOnlyCollection<string> itemNames, IReadOnlyCollection<int> slotIndexes)
    {
        return Objects
            .Where(x => x.StoredInId is null && itemNames.Contains(x.ItemName))
            .Select(x => x.Slots.Count(s => slotIndexes.Contains(s.SlotIndex)))
            .DefaultIfEmpty()
            .Max();
    }

    public void StoreObject(WorldObject container, GameItem containerItem, WorldObject obj)
    {
        var storageMechanic = containerItem.GetObjectStorageMechanic() ?? throw new Exception($"No object storage on {container.ItemName}");

        if (obj == container || obj.StoredInId is not null || !container.CanStore(obj, containerItem, storageMechanic))
        {
            throw new Exception($"Object {obj.ItemName} can't be stored in {container.ItemName}");
        }

        var storedCount = GetStoredObjects(container).Count;
        var capacity = container.GetStorageCapacity(containerItem.StorageUnit!);

        if (storedCount >= capacity)
        {
            throw new Exception($"Storage of {container.WorldFlatId} is full ({capacity})");
        }

        obj.StoreIn(container.WorldFlatId, storedCount);
    }

    public void PlaceFromStorage(WorldObject container, int slotIndex, int x, int y, int direction)
    {
        var stored = GetStoredObjects(container);

        if (slotIndex < 0 || slotIndex >= stored.Count)
        {
            throw new Exception($"Invalid storage slot {slotIndex} on object {container.WorldFlatId}");
        }

        var removed = stored[slotIndex];
        removed.TakeOutOfStorage(x, y, direction);

        foreach (var next in stored.Skip(slotIndex + 1))
        {
            next.ShiftStorageSlot();
        }

        if (container.IsStorageSupplied())
        {
            container.RemoveSupplyResource(removed.ItemName, GetStoredObjects(container));
        }
    }
}
