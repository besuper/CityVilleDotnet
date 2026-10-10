using Bogus;

namespace CityVilleDotnet.Factory.GlobalTableOverride;

public static class GlobalTableOverrideFactory
{
    public static Domain.Entities.GlobalTableOverride GlobalTableOverride(this Faker faker, string? keyword = null, string? table = null, string? source = null)
    {
        return new Domain.Entities.GlobalTableOverride(
            keyword ?? faker.Random.String2(64),
            table ?? faker.Random.String2(128),
            source ?? faker.Random.String2(64));
    }
}
