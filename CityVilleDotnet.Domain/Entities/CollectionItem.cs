namespace CityVilleDotnet.Domain.Entities;

public class CollectionItem(string name, int amount)
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; } = name;
    public int Amount { get; private set; } = amount;
    
    public void AddAmount(int amount)
    {
        Amount += amount;
    }
    
    public void RemoveAmount(int amount)
    {
        if (Amount < amount)
            throw new Exception("Not enough items to remove");
        
        Amount -= amount;
    }
}