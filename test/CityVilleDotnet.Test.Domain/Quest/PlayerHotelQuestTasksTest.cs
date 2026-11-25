using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.Quest;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;

namespace CityVilleDotnet.Test.Domain.Quest;

// See qm_test_hotels in TestData/questSettings.xml and test_resort_* in TestData/gameSettings.xml
[Collection("Domain")]
public class PlayerHotelQuestTasksTest
{
    private const string QuestName = "qm_test_hotels";

    [Fact]
    public void Player_HandleQuestsProgress_OpenHotelWithResortBusinesses_ProgressesHotelTasks()
    {
        var faker = new Faker();
        var world = faker.World(objects:
        [
            faker.WorldObject(itemName: "test_resort_hotel", className: BuildingClassType.Hotel, state: WorldObjectState.Open, x: 0, y: 0),
            faker.WorldObject(itemName: "test_resort_shop", className: BuildingClassType.Business, x: 4, y: 1),
            faker.WorldObject(itemName: "test_resort_shop", className: BuildingClassType.Business, x: 2, y: 4)
        ]);
        var player = faker.Player(world: world);
        var quest = faker.Quest(name: QuestName, length: 3);
        player.Quests.Add(quest);

        player.HandleQuestsProgress("");

        quest.Progress.Should().Equal(2, 1, 0);
    }

    [Fact]
    public void Player_HandleQuestsProgress_ClosedHotelWithoutBusinesses_KeepsProgress()
    {
        var faker = new Faker();
        var world = faker.World(objects: [faker.WorldObject(itemName: "test_resort_hotel", className: BuildingClassType.Hotel, state: WorldObjectState.Closed, x: 0, y: 0)]);
        var player = faker.Player(world: world);
        var quest = faker.Quest(name: QuestName, length: 3);
        player.Quests.Add(quest);

        player.HandleQuestsProgress("");

        quest.Progress.Should().Equal(0, 0, 0);
    }

    [Fact]
    public void Player_HandleQuestsProgress_CountStreamPublishByName_MatchingViral_Progresses()
    {
        var faker = new Faker();
        var player = faker.Player();
        var quest = faker.Quest(name: QuestName, length: 3);
        player.Quests.Add(quest);

        player.HandleQuestsProgress("countStreamPublishByName", itemName: "test_viral_checkin_2");

        quest.Progress.Should().Equal(0, 0, 1);
    }

    [Fact]
    public void Player_HandleQuestsProgress_CountStreamPublishByName_OtherViral_KeepsProgress()
    {
        var faker = new Faker();
        var player = faker.Player();
        var quest = faker.Quest(name: QuestName, length: 3);
        player.Quests.Add(quest);

        player.HandleQuestsProgress("countStreamPublishByName", itemName: faker.Random.String2(128));

        quest.Progress.Should().Equal(0, 0, 0);
    }
}
