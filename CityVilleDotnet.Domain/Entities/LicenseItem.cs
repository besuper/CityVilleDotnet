namespace CityVilleDotnet.Domain.Entities;

public class LicenseItem
{
    public Guid Id { get; private set; }
    
    public string Name { get; private set; }
    
    public int Amount { get; private set; }
    
    public LicenseItem(string name, int amount)
    {
        Id = Guid.NewGuid();
        Name = name;
        Amount = amount;
    }
    
    public void Add(int count)
    {
        Amount += count;
    }
    
    public void Remove(int count)
    {
        Amount -= count;
        if (Amount < 0) Amount = 0;
    }
}