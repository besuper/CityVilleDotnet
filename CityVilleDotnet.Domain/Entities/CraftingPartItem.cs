namespace CityVilleDotnet.Domain.Entities;

public class CraftingPartItem
{
    public int Id { get; private set; }

    public string Name { get; private set; }

    public int Amount { get; private set; }

    public CraftingPartItem(string name, int amount)
    {
        Name = name;
        Amount = amount;
    }

    public void SetAmount(int amount)
    {
        Amount = amount;
    }
}
