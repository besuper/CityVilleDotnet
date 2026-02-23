namespace CityVilleDotnet.Domain.Entities;

public class Mastery
{
    public int Id { get; }
    public string ItemName { get; private set; }
    public int Level { get; private set; }
    public int Count { get; private set; }

    public Mastery(string itemName)
    {
        ItemName = itemName;
        Level = 0;
        Count = 0;
    }

    public void AddCount()
    {
        Count++;
    }
}