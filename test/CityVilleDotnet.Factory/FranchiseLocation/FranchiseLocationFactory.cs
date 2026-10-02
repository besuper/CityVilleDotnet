using Bogus;

namespace CityVilleDotnet.Factory.FranchiseLocation;

public static class FranchiseLocationFactory
{
    public static Domain.Entities.FranchiseLocation FranchiseLocation(
        this Faker faker,
        int commodityLeft = 0,
        int commodityMax = 10,
        int customersServed = 0,
        int starRating = 1)
    {
        return new Domain.Entities.FranchiseLocation
        {
            Uid = faker.Random.String2(64),
            ObjectId = faker.Random.String2(64),
            FranchiseName = faker.Random.String2(64),
            StarRating = starRating,
            CommodityLeft = commodityLeft,
            CommodityMax = commodityMax,
            CustomersServed = customersServed
        };
    }
}
