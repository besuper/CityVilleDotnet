using Bogus;
using CityVilleDotnet.Domain.Enums;

namespace CityVilleDotnet.Factory.Quest;

public static class QuestFactory
{
    public static Domain.Entities.Quest Quest(
        this Faker faker,
        string? name = null,
        int length = 3,
        QuestType questType = QuestType.Active)
    {
        return Domain.Entities.Quest.Create(
            name ?? faker.Random.String2(64),
            length,
            questType);
    }
}
