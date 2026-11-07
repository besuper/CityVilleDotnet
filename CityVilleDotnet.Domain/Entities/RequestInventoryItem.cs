namespace CityVilleDotnet.Domain.Entities;

public class RequestInventoryItem
{
    public int Id { get; set; }
    public string InventoryName { get; private set; } = string.Empty;
    public string ItemName { get; private set; } = string.Empty;
    public int Amount { get; private set; }

    private RequestInventoryItem()
    {
    }

    public RequestInventoryItem(string inventoryName, string itemName)
    {
        InventoryName = inventoryName;
        ItemName = itemName;
    }

    public void AddAmount(int amount)
    {
        Amount += amount;
    }
}
