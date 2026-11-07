namespace CityVilleDotnet.Domain.Entities;

public class WorldObjectSupplyResource
{
    public int Id { get; set; }
    public int Position { get; private set; }
    public string ItemName { get; private set; } = string.Empty;

    private WorldObjectSupplyResource()
    {
    }

    public WorldObjectSupplyResource(int position, string itemName)
    {
        Position = position;
        ItemName = itemName;
    }
}
