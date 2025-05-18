namespace CityVilleDotnet.Domain.Entities;

public class Inventory
{
    public Guid Id { get; set; }
    public List<InventoryItem> Items { get; set; } = [];

    public void AddItem(string itemName, int amount = 1)
    {
        var item = Items.FirstOrDefault(x => x.Name == itemName);

        if (item is null)
            Items.Add(new InventoryItem(itemName, amount));
        else
            item.AddAmount(amount);
    }

    public void RemoveItem(string itemName, int amount = 1)
    {
        var item = Items.FirstOrDefault(x => x.Name == itemName);

        if (item is null)
            throw new Exception($"Item not found in player inventory {itemName}");

        if (item.Amount < amount)
            throw new Exception("Not enough items");

        item.RemoveAmount(amount);

        if (item.Amount <= 0)
            Items.Remove(item);
    }

    public int Count()
    {
        return Items.Sum(x => x.Amount);
    }
}
