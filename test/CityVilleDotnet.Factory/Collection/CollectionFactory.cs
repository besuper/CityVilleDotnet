using Bogus;

namespace CityVilleDotnet.Factory.Collection;

public static class CollectionFactory
{
    public static Domain.Entities.Collection Collection(this Faker faker, string? itemName = null)
    {
        return new Domain.Entities.Collection(itemName ?? faker.Random.String2(64));
    }
}