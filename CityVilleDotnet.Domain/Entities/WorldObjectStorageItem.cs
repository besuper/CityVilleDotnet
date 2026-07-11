namespace CityVilleDotnet.Domain.Entities;

public class WorldObjectStorageItem
{
    public int Id { get; set; }
    public string Name { get; private set; }
    public int Amount { get; private set; }

    private WorldObjectStorageItem()
    {
    }

    public WorldObjectStorageItem(string name, int amount)
    {
        Name = name;
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
