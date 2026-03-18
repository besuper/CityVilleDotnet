using Bogus;

namespace CityVilleDotnet.Factory.Player;

public static class PlayerFactory
{
    public static Domain.Entities.Player Player(this Faker faker, string? username = null)
    {
        return new Domain.Entities.Player(username ?? faker.Random.String2(32));
    }
}
