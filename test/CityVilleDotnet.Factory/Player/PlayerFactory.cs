using Bogus;
using CityVilleDotnet.Factory.ApplicationUser;
using CityVilleDotnet.Factory.World;

namespace CityVilleDotnet.Factory.Player;

public static class PlayerFactory
{
    public static Domain.Entities.Player Player(this Faker faker, Domain.Entities.ApplicationUser? appUser = null, Domain.Entities.World? world = null)
    {
        return new Domain.Entities.Player(appUser ?? faker.ApplicationUser(), world ?? faker.World());
    }
}