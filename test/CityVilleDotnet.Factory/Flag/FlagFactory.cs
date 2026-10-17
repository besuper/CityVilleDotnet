using Bogus;

namespace CityVilleDotnet.Factory.Flag;

public static class FlagFactory
{
    public static Domain.Entities.Flag Flag(this Faker faker, string? name = null, int? value = null)
    {
        return new Domain.Entities.Flag(name ?? faker.Random.String2(64), value ?? faker.Random.Int());
    }
}
