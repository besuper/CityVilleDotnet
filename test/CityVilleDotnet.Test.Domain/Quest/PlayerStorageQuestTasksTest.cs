using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.Quest;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;

namespace CityVilleDotnet.Test.Domain.Quest;

// See qm_test_store_in_mechanic and qm_test_storage_counts in TestData/questSettings.xml
[Collection("Domain")]
public class PlayerStorageQuestTasksTest
{
    [Fact]
    public void Player_HandleQuestsProgress_StoreItemInMechanicUser_MatchingContainerType_Progresses()
    {
        var faker = new Faker();
        var player = faker.Player();
        var quest = faker.Quest(name: "qm_test_store_in_mechanic", length: 2);
        player.Quests.Add(quest);

        player.HandleQuestsProgress("storeItemInMechanicUser", itemName: "test_storage_hood");

        quest.Progress.Should().Equal(1, 0);
    }

    [Fact]
    public void Player_HandleQuestsProgress_StoreItemInMechanicUser_OtherContainerType_KeepsProgress()
    {
        var faker = new Faker();
        var player = faker.Player();
        var quest = faker.Quest(name: "qm_test_store_in_mechanic", length: 2);
        player.Quests.Add(quest);

        player.HandleQuestsProgress("storeItemInMechanicUser", itemName: "test_storage_mall");

        quest.Progress.Should().Equal(0, 0);
    }

    [Fact]
    public void Player_HandleQuestsProgress_StoreItemInMechanicUserByKeyword_MatchingContainerKeyword_Progresses()
    {
        var faker = new Faker();
        var player = faker.Player();
        var quest = faker.Quest(name: "qm_test_store_in_mechanic", length: 2);
        player.Quests.Add(quest);

        player.HandleQuestsProgress("storeItemInMechanicUserByKeyword", itemName: "test_storage_hood");
        player.HandleQuestsProgress("storeItemInMechanicUserByKeyword", itemName: "test_storage_mall");

        quest.Progress.Should().Equal(0, 1);
    }

    [Fact]
    public void Player_HandleQuestsProgress_StorageCounts_CountStoredObjectsAndSlots()
    {
        var faker = new Faker();
        var hood = faker.WorldObject(itemName: "test_storage_hood", className: BuildingClassType.Neighborhood, worldFlatId: 1);
        var mall = faker.WorldObject(itemName: "test_storage_mall", className: BuildingClassType.Mall, worldFlatId: 2);
        var themePark = faker.WorldObject(itemName: "test_theme_park", className: BuildingClassType.ThemePark, worldFlatId: 3);
        var houses = new[] { faker.WorldObject(itemName: "test_family_house", worldFlatId: 10), faker.WorldObject(itemName: "test_family_house", worldFlatId: 11) };
        var storedShop = faker.WorldObject(itemName: "test_shop", worldFlatId: 12);
        var placedShop = faker.WorldObject(itemName: "test_shop", worldFlatId: 13);
        var world = faker.World(objects: [hood, mall, themePark, .. houses, storedShop, placedShop]);
        var player = faker.Player(world: world);
        var quest = faker.Quest(name: "qm_test_storage_counts", length: 3);
        player.Quests.Add(quest);

        foreach (var house in houses)
        {
            world.StoreObject(hood, GameSettingsManager.Instance.GetItem("test_storage_hood")!, house);
        }

        world.StoreObject(mall, GameSettingsManager.Instance.GetItem("test_storage_mall")!, storedShop);
        themePark.FillSlot(0, "test_attraction", 2);

        player.HandleQuestsProgress("");

        quest.Progress.Should().Equal(2, 2, 1);
    }
}
