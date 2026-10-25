using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Factory.MapRect;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.Quest;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Domain.Fixtures;

namespace CityVilleDotnet.Test.Domain.Quest;

// See qm_test_lakefront_* in TestData/questSettings.xml, quests only check global tasks once world objects are loaded
[Collection("Domain")]
public class PlayerLakefrontQuestTasksTest
{
    [Fact]
    public void Player_HandleQuestsProgress_ItemGateComplete_KeysOwned_CompletesTask()
    {
        var faker = new Faker();
        var player = faker.Player(world: faker.World(objects: [faker.WorldObject()]));
        var quest = faker.Quest(name: "qm_test_lakefront_canal", length: 1);
        player.Quests.Add(quest);
        player.AddItem("test_gate_material", 6);

        player.HandleQuestsProgress("");

        quest.Progress.Should().Equal(1);
    }

    [Fact]
    public void Player_HandleQuestsProgress_ItemGateComplete_KeysMissing_KeepsTaskOpen()
    {
        var faker = new Faker();
        var player = faker.Player(world: faker.World(objects: [faker.WorldObject()]));
        var quest = faker.Quest(name: "qm_test_lakefront_canal", length: 1);
        player.Quests.Add(quest);
        player.AddItem("test_gate_material", 5);

        player.HandleQuestsProgress("");

        quest.Progress.Should().Equal(0);
    }

    [Fact]
    public void Player_HandleQuestsProgress_ItemGateComplete_NoGate_UsesBuildGate()
    {
        var faker = new Faker();
        var player = faker.Player(world: faker.World(objects: [faker.WorldObject()]));
        var quest = faker.Quest(name: "qm_test_lakefront_default_gate", length: 1);
        player.Quests.Add(quest);
        player.AddItem("test_gate_material", 2);

        player.HandleQuestsProgress("");

        quest.Progress.Should().Equal(1);
    }

    [Fact]
    public void Player_HandleQuestsProgress_CouponObtained_CouponOwned_CompletesTask()
    {
        var faker = new Faker();
        var player = faker.Player(world: faker.World(objects: [faker.WorldObject()]));
        var quest = faker.Quest(name: "qm_test_lakefront_bridge", length: 1);
        player.Quests.Add(quest);
        player.GiveCoupon("test_bridge_coupon");

        player.HandleQuestsProgress("");

        quest.Progress.Should().Equal(1);
    }

    [Fact]
    public void Player_HandleQuestsProgress_CouponObtained_NoCoupon_KeepsTaskOpen()
    {
        var faker = new Faker();
        var player = faker.Player(world: faker.World(objects: [faker.WorldObject()]));
        var quest = faker.Quest(name: "qm_test_lakefront_bridge", length: 1);
        player.Quests.Add(quest);

        player.HandleQuestsProgress("");

        quest.Progress.Should().Equal(0);
    }

    [Fact]
    public void Player_HandleQuestsProgress_GetExpansionByCoord_ExpansionOwned_CompletesTaskOfThisWorldOnly()
    {
        var faker = new Faker();
        var world = faker.World(mapRects: [faker.MapRect(x: 48, y: -84, width: 12, height: 12)], objects: [faker.WorldObject()]);
        var player = faker.Player(world: world);
        var quest = faker.Quest(name: "qm_test_lakefront_expansion", length: 2);
        player.Quests.Add(quest);

        player.HandleQuestsProgress("");

        quest.Progress.Should().Equal(1, 0);
    }

    [Fact]
    public void Player_HandleQuestsProgress_GetExpansionByCoord_ExpansionMissing_KeepsTasksOpen()
    {
        var faker = new Faker();
        var world = faker.World(mapRects: [faker.MapRect(x: 0, y: 0, width: 12, height: 12)], objects: [faker.WorldObject()]);
        var player = faker.Player(world: world);
        var quest = faker.Quest(name: "qm_test_lakefront_expansion", length: 2);
        player.Quests.Add(quest);

        player.HandleQuestsProgress("");

        quest.Progress.Should().Equal(0, 0);
    }
}
