using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.Quest;
using CityVilleDotnet.Test.Domain.Fixtures;

namespace CityVilleDotnet.Test.Domain.Quest;

// See qm_test_lakefront_canal in TestData/questSettings.xml
[Collection("Domain")]
public class PlayerQuestConsumptionTest
{
    private const string QuestName = "qm_test_lakefront_canal";

    [Fact]
    public void Player_CheckCompletedQuests_Consumption_RemovesInventoryItems()
    {
        var faker = new Faker();
        var player = faker.Player();
        var quest = faker.Quest(name: QuestName, length: 1);
        quest.Progress[0] = 1;
        player.Quests.Add(quest);
        player.AddItem("test_gate_material", 7);
        player.AddItem("test_flyer", 5);
        var gold = player.Gold;

        var removed = player.CheckCompletedQuests();

        quest.QuestType.Should().Be(QuestType.Completed);
        player.CountInventoryItem("test_gate_material").Should().Be(0);
        player.CountInventoryItem("test_flyer").Should().Be(3);
        player.Gold.Should().Be(gold);
        removed.Should().ContainSingle().Which.Name.Should().Be("test_gate_material");
    }

    [Fact]
    public void Player_CheckCompletedQuests_ConsumptionAboveOwned_RemovesWhatIsOwned()
    {
        var faker = new Faker();
        var player = faker.Player();
        var quest = faker.Quest(name: QuestName, length: 1);
        quest.Progress[0] = 1;
        player.Quests.Add(quest);
        player.AddItem("test_flyer");

        var removed = player.CheckCompletedQuests();

        player.InventoryItems.Should().BeEmpty();
        removed.Should().ContainSingle().Which.Name.Should().Be("test_flyer");
    }

    [Fact]
    public void Player_CheckCompletedQuests_CouponReward_GivesCoupon()
    {
        var faker = new Faker();
        var player = faker.Player();
        var quest = faker.Quest(name: QuestName, length: 1);
        quest.Progress[0] = 1;
        player.Quests.Add(quest);

        player.CheckCompletedQuests();

        player.HasCoupon("test_canal_coupon").Should().BeTrue();
    }

    [Fact]
    public void Player_CheckCompletedQuests_QuestNotCompleted_KeepsItems()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.Quests.Add(faker.Quest(name: QuestName, length: 1));
        player.AddItem("test_gate_material", 7);

        var removed = player.CheckCompletedQuests();

        player.CountInventoryItem("test_gate_material").Should().Be(7);
        removed.Should().BeEmpty();
    }
}
