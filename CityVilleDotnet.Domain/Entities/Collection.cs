namespace CityVilleDotnet.Domain.Entities;

public class Collection
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public int Completed { get; private set; } = 0;

    public List<CollectionItem> Items { get; private set; } = [];

    public Collection()
    {
    }

    public Collection(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }

    public void AddItem(string itemName, int amount = 1)
    {
        var item = Items.FirstOrDefault(x => x.Name == itemName);

        if (item is null)
            Items.Add(new CollectionItem(itemName, amount));
        else
            item.AddAmount(amount);
    }

    public CollectionItem? RemoveItem(string itemName, int amount = 1)
    {
        var item = Items.FirstOrDefault(x => x.Name == itemName);

        if (item is null)
            throw new Exception($"Item not found in collection {itemName}");

        if (item.Amount < amount)
            throw new Exception("Not enough items");

        item.RemoveAmount(amount);

        if (item.Amount <= 0)
        {
            Items.Remove(item);
            return item;
        }

        return null;
    }

    public void Complete()
    {
        Completed += 1;
    }
}