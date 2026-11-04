using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.Quest;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;

namespace CityVilleDotnet.Test.Domain.Quest;

// See qm_test_completed_building in TestData/questSettings.xml
[Collection("Domain")]
public class PlayerCompletedBuildingTaskTest
{
    private static CityVilleDotnet.Domain.Entities.WorldObject ConstructionSite(Faker faker, string targetName)
    {
        var site = faker.WorldObject(itemName: targetName);
        site.SetAsConstructionSite("test_completed_site", 0);

        return site;
    }

    [Fact]
    public void Player_HandleQuestsProgress_CountCompletedBuildingByName_CountsEveryListedBuilding()
    {
        var faker = new Faker();
        var world = faker.World(objects: [faker.WorldObject(itemName: "test_completed_a"), faker.WorldObject(itemName: "test_completed_b"), faker.WorldObject(itemName: "test_kw_other")]);
        var player = faker.Player(world: world);
        var quest = faker.Quest(name: "qm_test_completed_building", length: 1);
        player.Quests.Add(quest);

        player.HandleQuestsProgress("");

        quest.Progress.Should().Equal(2);
    }

    [Fact]
    public void Player_HandleQuestsProgress_CountCompletedBuildingByName_SubtractsConstructionSites()
    {
        var faker = new Faker();
        var world = faker.World(objects: [faker.WorldObject(itemName: "test_completed_a"), ConstructionSite(faker, "test_completed_a")]);
        var player = faker.Player(world: world);
        var quest = faker.Quest(name: "qm_test_completed_building", length: 1);
        player.Quests.Add(quest);

        player.HandleQuestsProgress("");

        quest.Progress.Should().Equal(0);
    }

    [Fact]
    public void Player_HandleQuestsProgress_CountCompletedBuildingByName_OnlyConstructionSites_StaysAtZero()
    {
        var faker = new Faker();
        var world = faker.World(objects: [ConstructionSite(faker, "test_completed_a"), ConstructionSite(faker, "test_completed_b")]);
        var player = faker.Player(world: world);
        var quest = faker.Quest(name: "qm_test_completed_building", length: 1);
        player.Quests.Add(quest);

        player.HandleQuestsProgress("");

        quest.Progress.Should().Equal(0);
    }

    [Fact]
    public void Player_HandleQuestsProgress_CountCompletedBuildingByName_SiteOfOtherBuilding_IsIgnored()
    {
        var faker = new Faker();
        var world = faker.World(objects: [faker.WorldObject(itemName: "test_completed_a"), ConstructionSite(faker, "test_kw_other")]);
        var player = faker.Player(world: world);
        var quest = faker.Quest(name: "qm_test_completed_building", length: 1);
        player.Quests.Add(quest);

        player.HandleQuestsProgress("");

        quest.Progress.Should().Equal(1);
    }
}
