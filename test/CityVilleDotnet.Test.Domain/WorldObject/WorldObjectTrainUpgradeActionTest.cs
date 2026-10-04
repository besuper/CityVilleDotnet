using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.WorldObject;

namespace CityVilleDotnet.Test.Domain.WorldObject;

[Collection("Domain")]
public class WorldObjectTrainUpgradeActionTest
{
    [Fact]
    public void WorldObject_CountTrainUpgradeAction_Platform1BuyTrain_CountsUpgradeAction()
    {
        var faker = new Faker();
        var station = faker.WorldObject(itemName: "train_platform", className: BuildingClassType.TrainStation);

        station.CountTrainUpgradeAction(GameSettingsManager.Instance.GetItem("test_schedule_buy")!, TrainOperationType.Buy);

        station.UpgradeActionCount.Should().Be(1);
        station.UpgradeActionCount2.Should().BeNull();
    }

    [Fact]
    public void WorldObject_CountTrainUpgradeAction_Platform1SellTrain_CountsUpgradeAction2()
    {
        var faker = new Faker();
        var station = faker.WorldObject(itemName: "train_platform", className: BuildingClassType.TrainStation);

        station.CountTrainUpgradeAction(GameSettingsManager.Instance.GetItem("test_schedule_sell")!, TrainOperationType.Sell);

        station.UpgradeActionCount.Should().BeNull();
        station.UpgradeActionCount2.Should().Be(1);
    }

    [Fact]
    public void WorldObject_CountTrainUpgradeAction_Platform2TouristTrain_CountsUpgradeAction2()
    {
        var faker = new Faker();
        var station = faker.WorldObject(itemName: "train_platform_2", className: BuildingClassType.TrainStation);

        station.CountTrainUpgradeAction(GameSettingsManager.Instance.GetItem("test_schedule_sell_tourist")!, TrainOperationType.Sell);

        station.UpgradeActionCount.Should().BeNull();
        station.UpgradeActionCount2.Should().Be(1);
    }

    [Fact]
    public void WorldObject_CountTrainUpgradeAction_Platform2SellTrainWithoutTourist_CountsUpgradeAction()
    {
        var faker = new Faker();
        var station = faker.WorldObject(itemName: "train_platform_2", className: BuildingClassType.TrainStation);

        station.CountTrainUpgradeAction(GameSettingsManager.Instance.GetItem("test_schedule_sell")!, TrainOperationType.Sell);

        station.UpgradeActionCount.Should().Be(1);
        station.UpgradeActionCount2.Should().BeNull();
    }

    [Fact]
    public void WorldObject_CountTrainUpgradeAction_Platform2BuyTrain_CountsUpgradeAction()
    {
        var faker = new Faker();
        var station = faker.WorldObject(itemName: "train_platform_2", className: BuildingClassType.TrainStation);

        station.CountTrainUpgradeAction(GameSettingsManager.Instance.GetItem("test_schedule_buy")!, TrainOperationType.Buy);

        station.UpgradeActionCount.Should().Be(1);
        station.UpgradeActionCount2.Should().BeNull();
    }

    [Fact]
    public void WorldObject_CountTrainUpgradeAction_Platform3_CountsNothing()
    {
        var faker = new Faker();
        var station = faker.WorldObject(itemName: "train_platform_3", className: BuildingClassType.TrainStation);

        station.CountTrainUpgradeAction(GameSettingsManager.Instance.GetItem("test_schedule_sell_tourist")!, TrainOperationType.Sell);

        station.UpgradeActionCount.Should().BeNull();
        station.UpgradeActionCount2.Should().BeNull();
    }

    [Fact]
    public void WorldObject_CountTrainUpgradeAction_ExistingCount_Increments()
    {
        var faker = new Faker();
        var station = faker.WorldObject(itemName: "train_platform", className: BuildingClassType.TrainStation);
        station.SetUpgradeAction(4);

        station.CountTrainUpgradeAction(GameSettingsManager.Instance.GetItem("test_schedule_buy")!, TrainOperationType.Buy);

        station.UpgradeActionCount.Should().Be(5);
    }

    [Fact]
    public void GameItem_TrainTourist_ReadsTrainTouristPayoutTable()
    {
        GameSettingsManager.Instance.GetItem("test_schedule_sell_tourist")!.TrainTourist!.Table.Should().Be("test_schedule_sell_tourist_tourist_values");
        GameSettingsManager.Instance.GetItem("test_schedule_sell")!.TrainTourist.Should().BeNull();
    }
}
