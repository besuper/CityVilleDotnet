using AwesomeAssertions;
using CityVilleDotnet.Api.Services.TrainService;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Integration.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Test.Integration.TrainService;

[Collection("Database")]
public class SendTrainTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    private static SendTrainRequest CreateRequest(string trainName, TrainOperationType operation)
    {
        return new SendTrainRequest
        {
            Attributes = new SendTrainAttributesRequest
            {
                TrainName = trainName,
                Operation = operation,
                CommodityName = "goods"
            }
        };
    }

    [Fact]
    public async Task SendTrain_Platform1SellTrain_PersistsUpgradeActionCount2()
    {
        var station = Faker.WorldObject(itemName: "train_platform", className: BuildingClassType.TrainStation);
        var player = Faker.Player(world: Faker.World(objects: [station]));
        player.SetGoods(100);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        Context.ChangeTracker.Clear();

        var handler = new SendTrain(Context);

        var response = await handler.HandlePacket(CreateRequest("test_schedule_sell", TrainOperationType.Sell), player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var storedStation = await Context.Set<WorldObject>().AsNoTracking().FirstAsync(x => x.Id == station.Id, TestContext.Current.CancellationToken);
        storedStation.UpgradeActionCount2.Should().Be(1);
        storedStation.UpgradeActionCount.Should().BeNull();
    }

    [Fact]
    public async Task SendTrain_Platform1BuyTrain_PersistsUpgradeActionCount()
    {
        var station = Faker.WorldObject(itemName: "train_platform", className: BuildingClassType.TrainStation);
        station.SetUpgradeAction(2);
        var player = Faker.Player(world: Faker.World(objects: [station]));
        player.SetGold(1000);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        Context.ChangeTracker.Clear();

        var handler = new SendTrain(Context);

        var response = await handler.HandlePacket(CreateRequest("test_schedule_buy", TrainOperationType.Buy), player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var storedStation = await Context.Set<WorldObject>().AsNoTracking().FirstAsync(x => x.Id == station.Id, TestContext.Current.CancellationToken);
        storedStation.UpgradeActionCount.Should().Be(3);
        storedStation.UpgradeActionCount2.Should().BeNull();
    }

    [Fact]
    public async Task SendTrain_Platform2TouristTrain_PersistsUpgradeActionCount2()
    {
        var station = Faker.WorldObject(itemName: "train_platform_2", className: BuildingClassType.TrainStation);
        var player = Faker.Player(world: Faker.World(objects: [station]));
        player.SetGoods(100);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        Context.ChangeTracker.Clear();

        var handler = new SendTrain(Context);

        var response = await handler.HandlePacket(CreateRequest("test_schedule_sell_tourist", TrainOperationType.Sell), player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var storedStation = await Context.Set<WorldObject>().AsNoTracking().FirstAsync(x => x.Id == station.Id, TestContext.Current.CancellationToken);
        storedStation.UpgradeActionCount2.Should().Be(1);
        storedStation.UpgradeActionCount.Should().BeNull();
    }

    [Fact]
    public async Task SendTrain_NoTrainStation_SendsTrainWithoutCounting()
    {
        var business = Faker.WorldObject(itemName: "test_bus_upgradable", className: BuildingClassType.Business);
        var player = Faker.Player(world: Faker.World(objects: [business]));
        player.SetGoods(100);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        Context.ChangeTracker.Clear();

        var handler = new SendTrain(Context);

        var response = await handler.HandlePacket(CreateRequest("test_schedule_sell", TrainOperationType.Sell), player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var storedPlayer = await Context.Set<Player>().AsNoTracking().FirstAsync(x => x.Id == player.Id, TestContext.Current.CancellationToken);
        storedPlayer.Goods.Should().Be(50);

        var storedBusiness = await Context.Set<WorldObject>().AsNoTracking().FirstAsync(x => x.Id == business.Id, TestContext.Current.CancellationToken);
        storedBusiness.UpgradeActionCount.Should().BeNull();
        storedBusiness.UpgradeActionCount2.Should().BeNull();
    }
}
