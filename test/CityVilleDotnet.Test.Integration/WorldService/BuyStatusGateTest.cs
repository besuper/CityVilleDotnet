using AwesomeAssertions;
using CityVilleDotnet.Api.Services.WorldService;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Integration.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Test.Integration.WorldService;

[Collection("Database")]
public class BuyStatusGateTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    private static BuyStatusGateRequest CreateRequest(int id, string gateName)
    {
        return new BuyStatusGateRequest
        {
            Building = new BuyStatusGateBuildingRequest { Id = id },
            Params = [gateName]
        };
    }

    [Fact]
    public async Task BuyStatusGate_ByWorldFlatId_ChargesCashAndCompletesGate()
    {
        var building = Faker.WorldObject(itemName: "test_status_gate", className: BuildingClassType.Business, worldFlatId: 5);
        building.SetUpgradeAction(3);
        var player = Faker.Player(world: Faker.World(objects: [building]));
        player.SetCash(100);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        Context.ChangeTracker.Clear();

        var handler = new BuyStatusGate(Context);

        var response = await handler.HandlePacket(CreateRequest(5, "upgrade_status"), player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var storedPlayer = await Context.Set<Player>().AsNoTracking().FirstAsync(x => x.Id == player.Id, TestContext.Current.CancellationToken);
        storedPlayer.Cash.Should().Be(65);

        var storedBuilding = await Context.Set<WorldObject>().AsNoTracking().FirstAsync(x => x.Id == building.Id, TestContext.Current.CancellationToken);
        storedBuilding.UpgradeActionCount.Should().Be(10);
    }

    [Fact]
    public async Task BuyStatusGate_ByTempId_CompletesGate()
    {
        var building = Faker.WorldObject(itemName: "test_status_gate", className: BuildingClassType.Business, tempId: 16777220, worldFlatId: 5);
        var player = Faker.Player(world: Faker.World(objects: [building]));
        player.SetCash(100);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        Context.ChangeTracker.Clear();

        var handler = new BuyStatusGate(Context);

        var response = await handler.HandlePacket(CreateRequest(16777220, "harvest_status"), player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var storedPlayer = await Context.Set<Player>().AsNoTracking().FirstAsync(x => x.Id == player.Id, TestContext.Current.CancellationToken);
        storedPlayer.Cash.Should().Be(75);

        var storedBuilding = await Context.Set<WorldObject>().AsNoTracking().FirstAsync(x => x.Id == building.Id, TestContext.Current.CancellationToken);
        storedBuilding.UpgradeActionCount.Should().Be(4);
    }

    [Fact]
    public async Task BuyStatusGate_UpgradeStatus2_PersistsUpgradeActionCount2Only()
    {
        var building = Faker.WorldObject(itemName: "test_status_gate", className: BuildingClassType.Business, worldFlatId: 5);
        building.SetUpgradeAction(3);
        var player = Faker.Player(world: Faker.World(objects: [building]));
        player.SetCash(100);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        Context.ChangeTracker.Clear();

        var handler = new BuyStatusGate(Context);

        var response = await handler.HandlePacket(CreateRequest(5, "upgrade_status2"), player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var storedPlayer = await Context.Set<Player>().AsNoTracking().FirstAsync(x => x.Id == player.Id, TestContext.Current.CancellationToken);
        storedPlayer.Cash.Should().Be(50);

        var storedBuilding = await Context.Set<WorldObject>().AsNoTracking().FirstAsync(x => x.Id == building.Id, TestContext.Current.CancellationToken);
        storedBuilding.UpgradeActionCount2.Should().Be(10);
        storedBuilding.UpgradeActionCount.Should().Be(3);
    }

    [Fact]
    public async Task BuyStatusGate_NotEnoughCash_ThrowsNotEnoughMoney()
    {
        var building = Faker.WorldObject(itemName: "test_status_gate", className: BuildingClassType.Business, worldFlatId: 5);
        var player = Faker.Player(world: Faker.World(objects: [building]));
        player.SetCash(10);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        Context.ChangeTracker.Clear();

        var handler = new BuyStatusGate(Context);

        var act = () => handler.HandlePacket(CreateRequest(5, "upgrade_status"), player.Id, TestContext.Current.CancellationToken);

        (await act.Should().ThrowAsync<DomainException>()).Which.Reason.Should().Be(GameErrorType.NotEnoughMoney);
    }

    [Fact]
    public async Task BuyStatusGate_UnknownGate_ThrowsException()
    {
        var building = Faker.WorldObject(itemName: "test_status_gate", className: BuildingClassType.Business, worldFlatId: 5);
        var player = Faker.Player(world: Faker.World(objects: [building]));
        player.SetCash(100);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        Context.ChangeTracker.Clear();

        var handler = new BuyStatusGate(Context);

        var act = () => handler.HandlePacket(CreateRequest(5, "inventory_gate"), player.Id, TestContext.Current.CancellationToken);

        await act.Should().ThrowAsync<Exception>().WithMessage("*inventory_gate*");
    }

    [Fact]
    public async Task BuyStatusGate_UnknownBuilding_ThrowsForceReload()
    {
        var player = Faker.Player(world: Faker.World(objects: []));
        player.SetCash(100);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        Context.ChangeTracker.Clear();

        var handler = new BuyStatusGate(Context);

        var act = () => handler.HandlePacket(CreateRequest(5, "upgrade_status"), player.Id, TestContext.Current.CancellationToken);

        (await act.Should().ThrowAsync<DomainException>()).Which.Reason.Should().Be(GameErrorType.ForceReload);
    }
}
