using AwesomeAssertions;
using CityVilleDotnet.Api.Services.UserService;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Integration.Fixtures;
using FluorineFx;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace CityVilleDotnet.Test.Integration.UserService;

// TNPCVisitBatch.as sends { objectId: { mechanicType: { count, operation, npcBreakDown, type } } }
// test_bus_goods needs 15 visits (commodityReq) to become harvestable, opening the business resets the visits to 0
[Collection("Database")]
public class ProcessVisitsBatchTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    private static ASObject CreateVisit(int count)
    {
        return new ASObject { ["count"] = count, ["operation"] = "visit", ["type"] = "business" };
    }

    [Fact]
    public async Task ProcessVisitsBatch_OpenBusiness_AddsVisits()
    {
        var business = Faker.WorldObject(itemName: "test_bus_goods", className: BuildingClassType.Business, state: WorldObjectState.Closed, worldFlatId: 5);
        business.OpenBusiness(playerLevel: 1);
        var world = Faker.World(objects: [business]);
        var player = Faker.Player(world: world);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new ProcessVisitsBatch(Context, NullLogger<ProcessVisitsBatch>.Instance);
        var request = new ProcessVisitsBatchRequest { Content = new ASObject { ["5"] = new ASObject { ["customers"] = CreateVisit(4) } } };

        var response = await handler.HandlePacket(request, player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        business.Visits.Should().Be(4);
        business.State.Should().Be(WorldObjectState.Open);
    }

    [Fact]
    public async Task ProcessVisitsBatch_SeveralMechanics_SumsCountsUntilHarvestable()
    {
        var business = Faker.WorldObject(itemName: "test_bus_goods", className: BuildingClassType.Business, state: WorldObjectState.Closed, worldFlatId: 5);
        business.OpenBusiness(playerLevel: 1);
        var world = Faker.World(objects: [business]);
        var player = Faker.Player(world: world);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new ProcessVisitsBatch(Context, NullLogger<ProcessVisitsBatch>.Instance);
        var request = new ProcessVisitsBatchRequest
        {
            Content = new ASObject { ["5"] = new ASObject { ["customers"] = CreateVisit(10), ["tourists"] = CreateVisit(5) } }
        };

        var response = await handler.HandlePacket(request, player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        business.Visits.Should().Be(15);
        business.State.Should().Be(WorldObjectState.ClosedHarvestable);
    }

    [Fact]
    public async Task ProcessVisitsBatch_BusinessReferencedByTempId_AddsVisits()
    {
        var business = Faker.WorldObject(itemName: "test_bus_goods", className: BuildingClassType.Business, state: WorldObjectState.Closed, tempId: 63000, worldFlatId: 5);
        business.OpenBusiness(playerLevel: 1);
        var world = Faker.World(objects: [business]);
        var player = Faker.Player(world: world);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new ProcessVisitsBatch(Context, NullLogger<ProcessVisitsBatch>.Instance);
        var request = new ProcessVisitsBatchRequest { Content = new ASObject { ["63000"] = new ASObject { ["customers"] = CreateVisit(3) } } };

        var response = await handler.HandlePacket(request, player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        business.Visits.Should().Be(3);
    }

    [Fact]
    public async Task ProcessVisitsBatch_UnknownObject_IsIgnored()
    {
        var business = Faker.WorldObject(itemName: "test_bus_goods", className: BuildingClassType.Business, state: WorldObjectState.Closed, worldFlatId: 5);
        business.OpenBusiness(playerLevel: 1);
        var world = Faker.World(objects: [business]);
        var player = Faker.Player(world: world);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new ProcessVisitsBatch(Context, NullLogger<ProcessVisitsBatch>.Instance);
        var request = new ProcessVisitsBatchRequest { Content = new ASObject { ["99"] = new ASObject { ["customers"] = CreateVisit(3) } } };

        var response = await handler.HandlePacket(request, player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        business.Visits.Should().Be(0);
    }

    // test_supply_mall supplied with test_mall_shop_a (10) and test_mall_shop_b (20) needs 30 customers
    private async Task<(Player Player, WorldObject Mall)> SeedMall(bool supplied)
    {
        var mall = Faker.WorldObject(itemName: "test_supply_mall", className: BuildingClassType.Mall, state: WorldObjectState.Open, worldFlatId: 5);
        var shopA = Faker.WorldObject(itemName: "test_mall_shop_a", className: BuildingClassType.Business, worldFlatId: 10);
        var shopB = Faker.WorldObject(itemName: "test_mall_shop_b", className: BuildingClassType.Business, worldFlatId: 11);
        shopA.StoreIn(5, 0);
        shopB.StoreIn(5, 1);

        if (supplied)
        {
            mall.OpenStorageSupply([shopA, shopB]);
        }

        var player = Faker.Player(world: Faker.World(objects: [mall, shopA, shopB]));

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        Context.ChangeTracker.Clear();

        return (player, mall);
    }

    private async Task<WorldObject> LoadMall(WorldObject mall)
    {
        Context.ChangeTracker.Clear();

        return await Context.Set<WorldObject>().AsNoTracking().SingleAsync(x => x.Id == mall.Id, TestContext.Current.CancellationToken);
    }

    [Fact]
    public async Task ProcessVisitsBatch_SuppliedMall_HarvestableAtSuppliedCustomersReq()
    {
        var (player, mall) = await SeedMall(supplied: true);
        var handler = new ProcessVisitsBatch(Context, NullLogger<ProcessVisitsBatch>.Instance);

        await handler.HandlePacket(new ProcessVisitsBatchRequest { Content = new ASObject { ["5"] = new ASObject { ["harvestState"] = CreateVisit(29) } } }, player.Id, TestContext.Current.CancellationToken);
        (await LoadMall(mall)).State.Should().Be(WorldObjectState.Open);

        await handler.HandlePacket(new ProcessVisitsBatchRequest { Content = new ASObject { ["5"] = new ASObject { ["harvestState"] = CreateVisit(1) } } }, player.Id, TestContext.Current.CancellationToken);

        var storedMall = await LoadMall(mall);
        storedMall.Visits.Should().Be(30);
        storedMall.State.Should().Be(WorldObjectState.ClosedHarvestable);
    }

    [Fact]
    public async Task ProcessVisitsBatch_OpenMallWithoutSuppliedShops_RefusesVisits()
    {
        var (player, mall) = await SeedMall(supplied: false);
        var handler = new ProcessVisitsBatch(Context, NullLogger<ProcessVisitsBatch>.Instance);

        var response = await handler.HandlePacket(new ProcessVisitsBatchRequest { Content = new ASObject { ["5"] = new ASObject { ["harvestState"] = CreateVisit(30) } } }, player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        var storedMall = await LoadMall(mall);
        storedMall.Visits.Should().BeNull();
        storedMall.State.Should().Be(WorldObjectState.Open);
    }
}
