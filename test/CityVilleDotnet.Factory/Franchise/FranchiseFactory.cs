using Bogus;

namespace CityVilleDotnet.Factory.Franchise;

public static class FranchiseFactory
{
    public static Domain.Entities.Franchise Franchise(
        this Faker faker,
        string? franchiseType = null,
        List<Domain.Entities.FranchiseLocation>? locations = null)
    {
        return new Domain.Entities.Franchise(franchiseType ?? faker.Random.String2(64), faker.Random.String2(64))
        {
            Locations = locations ?? []
        };
    }
}
