using Bogus;

namespace CityVilleDotnet.Factory.PreyGroup;

public static class PreyGroupFactory
{
    public static Domain.Entities.PreyGroup PreyGroup(
        this Faker faker,
        string? groupId = null,
        List<int>? preys = null,
        List<Domain.Entities.PreyHunter>? hunters = null)
    {
        var group = new Domain.Entities.PreyGroup(groupId ?? faker.Random.String2(32));

        group.Preys.AddRange(preys ?? []);
        group.Hunters.AddRange(hunters ?? []);

        return group;
    }
}
