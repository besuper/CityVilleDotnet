namespace CityVilleDotnet.Domain.Entities;

public class Mastery(string itemName)
{
    public int Id { get; }
    public string ItemName { get; private set; } = itemName;
    public int Level { get; private set; } = 0;
    public int Count { get; private set; } = 0;

    public void AddCount()
    {
        Count++;
    }

    public void LevelUp(int level)
    {
        Level = level;
    }
}