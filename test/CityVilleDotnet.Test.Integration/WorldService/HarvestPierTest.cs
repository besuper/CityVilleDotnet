using AwesomeAssertions;
using CityVilleDotnet.Api.Services.WorldService;
using CityVilleDotnet.Api.Services.WorldService.Common;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Integration.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;

namespace CityVilleDotnet.Test.Integration.WorldService;

// test_pier at (10, 10) has berth squares (9, 12) and (9, 9), test_ship at (6, 10) covers x 6..9 and y 10..12
[Collection("Database")]
public class HarvestPierTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    private static HarvestRequest CreateHarvestRequest(int x, int y)
    {
        return new HarvestRequest
        {
            Building = new BuildingHarvestRequest
            {
                Position = new PerformActionPositionRequest { X = x, Y = y, Z = 0 }
            }
        };
    }

    [Fact]
    public async Task Harvest_ShipBerthedAtPier_PersistsPierUpgradeActionCount()
    {
        var pier = Faker.WorldObject(itemName: "test_pier", className: BuildingClassType.Pier, x: 10, y: 10);
        var farPier = Faker.WorldObject(itemName: "test_pier", className: BuildingClassType.Pier, x: 30, y: 30);
        var ship = Faker.WorldObject(itemName: "test_ship", className: BuildingClassType.Ship, contractName: "test_ship_contract", state: WorldObjectState.Grown, x: 6, y: 10);
        var player = Faker.Player(world: Faker.World(objects: [pier, farPier, ship]));

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        Context.ChangeTracker.Clear();

        var handler = new Harvest(Context, NullLogger<Harvest>.Instance, new ConfigurationBuilder().Build());

        var response = await handler.HandlePacket(CreateHarvestRequest(6, 10), player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var storedPier = await Context.Set<WorldObject>().AsNoTracking().FirstAsync(x => x.Id == pier.Id, TestContext.Current.CancellationToken);
        var storedFarPier = await Context.Set<WorldObject>().AsNoTracking().FirstAsync(x => x.Id == farPier.Id, TestContext.Current.CancellationToken);
        storedPier.UpgradeActionCount.Should().Be(1);
        storedFarPier.UpgradeActionCount.Should().BeNull();
    }

    [Fact]
    public async Task Harvest_HarvestableShipBerthedAtPier_DoesNotCountPier()
    {
        var pier = Faker.WorldObject(itemName: "test_pier", className: BuildingClassType.Pier, x: 10, y: 10);
        var ship = Faker.WorldObject(itemName: "test_ship", className: BuildingClassType.HarvestableShip, contractName: "test_ship_contract", state: WorldObjectState.Grown, x: 6, y: 10);
        var player = Faker.Player(world: Faker.World(objects: [pier, ship]));

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        Context.ChangeTracker.Clear();

        var handler = new Harvest(Context, NullLogger<Harvest>.Instance, new ConfigurationBuilder().Build());

        await handler.HandlePacket(CreateHarvestRequest(6, 10), player.Id, TestContext.Current.CancellationToken);

        var storedPier = await Context.Set<WorldObject>().AsNoTracking().FirstAsync(x => x.Id == pier.Id, TestContext.Current.CancellationToken);
        storedPier.UpgradeActionCount.Should().BeNull();
    }
}
