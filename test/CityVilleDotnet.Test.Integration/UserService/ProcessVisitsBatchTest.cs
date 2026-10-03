using AwesomeAssertions;
using CityVilleDotnet.Api.Services.UserService;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Integration.Fixtures;
using FluorineFx;

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

        var handler = new ProcessVisitsBatch(Context);
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

        var handler = new ProcessVisitsBatch(Context);
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

        var handler = new ProcessVisitsBatch(Context);
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

        var handler = new ProcessVisitsBatch(Context);
        var request = new ProcessVisitsBatchRequest { Content = new ASObject { ["99"] = new ASObject { ["customers"] = CreateVisit(3) } } };

        var response = await handler.HandlePacket(request, player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        business.Visits.Should().Be(0);
    }
}
