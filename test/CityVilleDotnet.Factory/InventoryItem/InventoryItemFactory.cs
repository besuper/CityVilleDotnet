using Bogus;

namespace CityVilleDotnet.Factory.InventoryItem;

public static class InventoryItemFactory
{
    public static Domain.Entities.InventoryItem InventoryItem(this Faker faker, string? itemName = null, int? amount = null)
    {
        return new Domain.Entities.InventoryItem(itemName ?? faker.Random.String2(64), amount ?? 1);
    }
}
