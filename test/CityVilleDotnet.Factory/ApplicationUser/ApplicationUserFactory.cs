using Bogus;

namespace CityVilleDotnet.Factory.ApplicationUser;

public static class ApplicationUserFactory
{
    public static Domain.Entities.ApplicationUser ApplicationUser(this Faker faker, string? id = null, string? userName = null)
    {
        return new Domain.Entities.ApplicationUser
        {
            Id = id ?? Guid.NewGuid().ToString(),
            UserName = userName ?? faker.Internet.UserName()
        };
    }
}
