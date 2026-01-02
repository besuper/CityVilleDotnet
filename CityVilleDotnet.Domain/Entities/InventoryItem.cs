namespace CityVilleDotnet.Domain.Entities;

public class InventoryItem
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int Amount { get; set; }

    public InventoryItem()
    { }

    public InventoryItem(string itemName, int amount = 1)
    {
        Name = itemName;
        Amount = amount;
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
