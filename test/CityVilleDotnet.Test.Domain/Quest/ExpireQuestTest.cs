using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.Quest;
using CityVilleDotnet.Test.Domain.Fixtures;

namespace CityVilleDotnet.Test.Domain.Quest;

[Collection("Domain")]
public class ExpireQuestTest(DomainFixture fixture)
{
    [Fact]
    public void Player_ExpireQuest_ActiveQuest_SetsExpired()
    {
        var faker = new Faker();
        var player = faker.Player();
        var quest = faker.Quest();

        player.Quests.Add(quest);

        player.ExpireQuest(quest.Name);

        quest.QuestType.Should().Be(QuestType.Expired);
    }

    [Fact]
    public void Player_ExpireQuest_CompletedQuest_StaysCompleted()
    {
        var faker = new Faker();
        var player = faker.Player();
        var quest = faker.Quest(questType: QuestType.Completed);

        player.Quests.Add(quest);

        player.ExpireQuest(quest.Name);

        quest.QuestType.Should().Be(QuestType.Completed);
    }

    [Fact]
    public void Player_ExpireQuest_UnknownQuest_DoesNothing()
    {
        var faker = new Faker();
        var player = faker.Player();
        var quest = faker.Quest();

        player.Quests.Add(quest);

        player.ExpireQuest(faker.Random.String2(64));

        quest.QuestType.Should().Be(QuestType.Active);
    }
}
