using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.Quest;

namespace CityVilleDotnet.Test.Domain.Quest;

// See qm_test_item_count_reward and qm_test_item_reward in TestData/questSettings.xml
[Collection("Domain")]
public class QuestClaimRewardsTest
{
    [Fact]
    public void Quest_ClaimRewards_ItemCount_AddsThatAmount()
    {
        var faker = new Faker();
        var player = faker.Player();
        var quest = faker.Quest(name: "qm_test_item_count_reward", length: 1);

        quest.ClaimRewards(player);

        player.CountInventoryItem("test_reward_item").Should().Be(5);
    }

    [Fact]
    public void Quest_ClaimRewards_NoItemCount_AddsOne()
    {
        var faker = new Faker();
        var player = faker.Player();
        var quest = faker.Quest(name: "qm_test_item_reward", length: 1);

        quest.ClaimRewards(player);

        player.CountInventoryItem("test_reward_item").Should().Be(1);
    }

    [Fact]
    public void Player_CheckCompletedQuests_ItemCountReward_GrantsThatAmount()
    {
        var faker = new Faker();
        var player = faker.Player();
        var quest = faker.Quest(name: "qm_test_item_count_reward", length: 1);
        quest.Progress[0] = 1;
        player.Quests.Add(quest);

        player.CheckCompletedQuests();

        quest.QuestType.Should().Be(QuestType.Completed);
        player.CountInventoryItem("test_reward_item").Should().Be(5);
    }
}
