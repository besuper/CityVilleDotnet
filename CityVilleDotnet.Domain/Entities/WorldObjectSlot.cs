namespace CityVilleDotnet.Domain.Entities;

public class WorldObjectSlot
{
    public int Id { get; set; }
    public int SlotIndex { get; private set; }
    public string ItemName { get; private set; } = string.Empty;

    private WorldObjectSlot()
    {
    }

    public WorldObjectSlot(int slotIndex, string itemName)
    {
        SlotIndex = slotIndex;
        ItemName = itemName;
    }
}
