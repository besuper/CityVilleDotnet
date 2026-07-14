namespace CityVilleDotnet.Domain.Entities;

public class InventoryItem
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int Amount { get; set; }
    
    public string? StorageType { get; set; }
    public WorldObject? StoredObject { get; set; }

    public bool IsMainInventory => StorageType is null;

    private InventoryItem() {}
    
    public InventoryItem(string itemName, int amount = 1, string? storageType = null, WorldObject? storedObject = null)
    {
        Name = itemName;
        Amount = amount;
        StorageType = storageType;
        StoredObject = storedObject;
    }

    public void AddAmount(int amount)
    {
        Amount += amount;
    }

    public void RemoveAmount(int amount)
    {
        Amount -= amount;
    }
}
