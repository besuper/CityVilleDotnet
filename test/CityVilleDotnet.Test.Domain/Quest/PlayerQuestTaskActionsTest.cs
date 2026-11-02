using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.Quest;
using CityVilleDotnet.Factory.World;

namespace CityVilleDotnet.Test.Domain.Quest;

// See qm_test_keywords, qm_test_incentivized_expansion, qm_test_public_visitor_help and qm_test_cross_promo in TestData/questSettings.xml
[Collection("Domain")]
public class PlayerQuestTaskActionsTest
{
    [Fact]
    public void Player_HandleQuestsProgress_ByKeyword_CountsEachMatchingKeyword()
    {
        var faker = new Faker();
        var player = faker.Player();
        var quest = faker.Quest(name: "qm_test_keywords", length: 3);
        player.Quests.Add(quest);

        player.HandleQuestsProgress("harvestMechanicByKeywords", itemName: "test_kw_item");

        quest.Progress.Should().Equal(2, 0, 0);
    }

    [Fact]
    public void Player_HandleQuestsProgress_ByKeyword_SingleKeyword_ProgressesByOne()
    {
        var faker = new Faker();
        var player = faker.Player();
        var quest = faker.Quest(name: "qm_test_keywords", length: 3);
        player.Quests.Add(quest);

        player.HandleQuestsProgress("harvestByKeyword", itemName: "test_kw_item");

        quest.Progress.Should().Equal(0, 1, 0);
    }

    [Fact]
    public void Player_HandleQuestsProgress_ByKeyword_NoMatchingKeyword_KeepsProgress()
    {
        var faker = new Faker();
        var player = faker.Player();
        var quest = faker.Quest(name: "qm_test_keywords", length: 3);
        player.Quests.Add(quest);

        player.HandleQuestsProgress("harvestMechanicByKeywords", itemName: "test_kw_other");
        player.HandleQuestsProgress("harvestByKeyword", itemName: "test_kw_other");

        quest.Progress.Should().Equal(0, 0, 0);
    }

    [Fact]
    public void Player_HandleQuestsProgress_ByKeyword_UnknownItem_Throws()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.Quests.Add(faker.Quest(name: "qm_test_keywords", length: 3));

        var act = () => player.HandleQuestsProgress("harvestMechanicByKeywords", itemName: "test_unknown_item");

        act.Should().Throw<Exception>();
    }

    [Fact]
    public void Player_HandleQuestsProgress_UnlockIncentivizedExpansion_ListedExpansionCompleted_CompletesTaskWithoutWorldObjects()
    {
        var faker = new Faker();
        var world = faker.World();
        world.GetOrCreateIncentivizedExpansion("test_expansion_b").Complete();
        var player = faker.Player(world: world);
        var quest = faker.Quest(name: "qm_test_incentivized_expansion", length: 1);
        player.Quests.Add(quest);

        player.HandleQuestsProgress("");

        quest.Progress.Should().Equal(1);
    }

    [Fact]
    public void Player_HandleQuestsProgress_UnlockIncentivizedExpansion_ExpansionNotCompleted_KeepsTaskOpen()
    {
        var faker = new Faker();
        var world = faker.World();
        world.GetOrCreateIncentivizedExpansion("test_expansion_a").Store(10, 10, 0);
        var player = faker.Player(world: world);
        var quest = faker.Quest(name: "qm_test_incentivized_expansion", length: 1);
        player.Quests.Add(quest);

        player.HandleQuestsProgress("");

        quest.Progress.Should().Equal(0);
    }

    [Fact]
    public void Player_HandleQuestsProgress_UnlockIncentivizedExpansion_OtherExpansionCompleted_KeepsTaskOpen()
    {
        var faker = new Faker();
        var world = faker.World();
        world.GetOrCreateIncentivizedExpansion("test_expansion_c").Complete();
        var player = faker.Player(world: world);
        var quest = faker.Quest(name: "qm_test_incentivized_expansion", length: 1);
        player.Quests.Add(quest);

        player.HandleQuestsProgress("");

        quest.Progress.Should().Equal(0);
    }

    [Theory]
    [InlineData("plotWater")]
    [InlineData("plotRevive")]
    public void Player_HandleQuestsProgress_PublicVisitorHelpTypeGlobal_ListedHelpType_ProgressesTask(string helpType)
    {
        var faker = new Faker();
        var player = faker.Player();
        var quest = faker.Quest(name: "qm_test_public_visitor_help", length: 1);
        player.Quests.Add(quest);

        player.HandleQuestsProgress("publicVisitorHelpTypeGlobal", helpType);

        quest.Progress.Should().Equal(1);
    }

    [Fact]
    public void Player_HandleQuestsProgress_PublicVisitorHelpTypeGlobal_OtherHelpType_KeepsProgress()
    {
        var faker = new Faker();
        var player = faker.Player();
        var quest = faker.Quest(name: "qm_test_public_visitor_help", length: 1);
        player.Quests.Add(quest);

        player.HandleQuestsProgress("publicVisitorHelpTypeGlobal", "plotHarvest");

        quest.Progress.Should().Equal(0);
    }

    [Fact]
    public void Player_HandleQuestsProgress_CrossPromoGames_CompletesEveryTask()
    {
        var faker = new Faker();
        var player = faker.Player();
        var quest = faker.Quest(name: "qm_test_cross_promo", length: 4);
        player.Quests.Add(quest);

        player.HandleQuestsProgress("");

        quest.Progress.Should().Equal(10, 20, 1, 1);
        quest.IsCompleted().Should().BeTrue();
    }
}
