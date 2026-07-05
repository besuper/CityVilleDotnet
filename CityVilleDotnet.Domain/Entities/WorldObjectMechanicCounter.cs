namespace CityVilleDotnet.Domain.Entities;

public class WorldObjectMechanicCounter
{
    public int Id { get; set; }
    public string MechanicType { get; private set; }
    public int Count { get; private set; }

    private WorldObjectMechanicCounter()
    {
    }

    public WorldObjectMechanicCounter(string mechanicType)
    {
        MechanicType = mechanicType;
    }

    public void Increment()
    {
        Count++;
    }

    public void Add(int amount)
    {
        Count += amount;
    }
}
