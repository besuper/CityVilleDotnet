using Bogus;
using CityVilleDotnet.Domain.Enums;

namespace CityVilleDotnet.Factory.PreyHunter;

public static class PreyHunterFactory
{
    public static Domain.Entities.PreyHunter PreyHunter(
        this Faker faker,
        int? position = null,
        HunterState state = HunterState.Patrolling,
        long? timestamp = null)
    {
        var hunter = new Domain.Entities.PreyHunter(position ?? faker.Random.Int(0, 6), timestamp ?? faker.Random.Long(1, 1_000_000));

        if (state == HunterState.Sleeping)
            hunter.Sleep(hunter.Timestamp);

        return hunter;
    }
}
