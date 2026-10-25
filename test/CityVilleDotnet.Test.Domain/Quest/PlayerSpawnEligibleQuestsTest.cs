using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Test.Domain.Fixtures;

namespace CityVilleDotnet.Test.Domain.Quest;

// See qh_lakefront_tracker and qh_test_hidden in TestData/questSettings.xml
[Collection("Domain")]
public class PlayerSpawnEligibleQuestsTest
{
    [Fact]
    public void Player_SpawnEligibleQuests_WhitelistedHiddenQuest_IsStarted()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.CompleteTutorial();

        player.SpawnEligibleQuests();

        player.Quests.Should().Contain(x => x.Name == "qh_lakefront_tracker");
    }

    [Fact]
    public void Player_SpawnEligibleQuests_OtherHiddenQuest_IsNotStarted()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.CompleteTutorial();

        player.SpawnEligibleQuests();

        player.Quests.Should().NotContain(x => x.Name == "qh_test_hidden");
    }
}
