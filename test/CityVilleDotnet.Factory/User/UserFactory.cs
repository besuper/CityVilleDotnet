using Bogus;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Factory.ApplicationUser;
using CityVilleDotnet.Factory.World;

namespace CityVilleDotnet.Factory.User;

public static class UserFactory
{
    public static Domain.Entities.User User(
        this Faker faker,
        Domain.Entities.ApplicationUser? appUser = null,
        Domain.Entities.World? world = null)
    {
        appUser ??= faker.ApplicationUser();
        world ??= faker.World();

        return new Domain.Entities.User(
            Guid.Parse(appUser.Id),
            appUser,
            appUser.UserName!,
            world);
    }
}
