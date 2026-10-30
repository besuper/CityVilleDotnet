namespace CityVilleDotnet.Domain.Entities;

public class WishlistItem
{
    public int Id { get; }
    public string ItemName { get; private set; } = string.Empty;

    public WishlistItem() {}

    public WishlistItem(string itemName)
    {
        ItemName = itemName;
    }
}
