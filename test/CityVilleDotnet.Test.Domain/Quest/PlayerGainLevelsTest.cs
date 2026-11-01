using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.Quest;
using CityVilleDotnet.Test.Domain.Fixtures;

namespace CityVilleDotnet.Test.Domain.Quest;

// See qm_test_gain_levels in TestData/questSettings.xml, highestLevel is 5 in TestData/gameSettings.xml
[Collection("Domain")]
public class PlayerGainLevelsTest
{
    [Fact]
    public void Player_AddXp_GainsOneLevel_ProgressesGainLevels()
    {
        var faker = new Faker();
        var player = faker.Player();
        var quest = faker.Quest(name: "qm_test_gain_levels", length: 1);
        player.Quests.Add(quest);

        player.AddXp(4);

        quest.Progress.Should().Equal(1);
    }

    [Fact]
    public void Player_AddXp_SkipsSeveralLevels_ProgressesByLevelsGained()
    {
        var faker = new Faker();
        var player = faker.Player();
        var quest = faker.Quest(name: "qm_test_gain_levels", length: 1);
        player.Quests.Add(quest);

        player.AddXp(16);

        quest.Progress.Should().Equal(2);
    }

    [Fact]
    public void Player_AddXp_WithoutLevelUp_DoesNotProgressGainLevels()
    {
        var faker = new Faker();
        var player = faker.Player();
        var quest = faker.Quest(name: "qm_test_gain_levels", length: 1);
        player.Quests.Add(quest);

        player.AddXp(3);

        quest.Progress.Should().Equal(0);
    }

    [Fact]
    public void Player_HandleQuestsProgress_AtHighestLevel_CompletesGainLevels()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.SetLevel(5);
        var quest = faker.Quest(name: "qm_test_gain_levels", length: 1);
        player.Quests.Add(quest);

        player.HandleQuestsProgress("");

        quest.Progress.Should().Equal(2);
    }

    [Fact]
    public void Player_HandleQuestsProgress_BelowHighestLevel_DoesNotCompleteGainLevels()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.SetLevel(4);
        var quest = faker.Quest(name: "qm_test_gain_levels", length: 1);
        player.Quests.Add(quest);

        player.HandleQuestsProgress("");

        quest.Progress.Should().Equal(0);
    }
}
