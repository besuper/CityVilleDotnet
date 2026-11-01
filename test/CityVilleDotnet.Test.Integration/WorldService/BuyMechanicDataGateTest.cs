using AwesomeAssertions;
using CityVilleDotnet.Api.Services.WorldService;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Integration.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Test.Integration.WorldService;

[Collection("Database")]
public class BuyMechanicDataGateTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    private static BuyMechanicDataGateRequest CreateRequest(int id, string gateName)
    {
        return new BuyMechanicDataGateRequest
        {
            Building = new BuyMechanicDataGateBuildingRequest { Id = id },
            Params = ["test_stockexchange", gateName]
        };
    }

    [Fact]
    public async Task BuyMechanicDataGate_PartialReturns_ChargesRemainingRatioAndCompletesReturns()
    {
        // floor(0.75 * 48) = 36
        var building = Faker.WorldObject(itemName: "test_stockexchange", className: BuildingClassType.PeepCapacityResource, state: WorldObjectState.Plowed, worldFlatId: 5);
        building.CompleteInvestmentReturns(250);
        var player = Faker.Player(world: Faker.World(objects: [building]));
        player.SetCash(100);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        Context.ChangeTracker.Clear();

        var handler = new BuyMechanicDataGate(Context);

        var response = await handler.HandlePacket(CreateRequest(5, "upgrade_investment"), player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var storedPlayer = await Context.Set<Player>().AsNoTracking().FirstAsync(x => x.Id == player.Id, TestContext.Current.CancellationToken);
        storedPlayer.Cash.Should().Be(64);

        var storedBuilding = await Context.Set<WorldObject>().AsNoTracking().FirstAsync(x => x.Id == building.Id, TestContext.Current.CancellationToken);
        storedBuilding.InvestmentReturns.Should().Be(1000);
    }

    [Fact]
    public async Task BuyMechanicDataGate_UnknownGate_ThrowsAndChargesNothing()
    {
        var building = Faker.WorldObject(itemName: "test_stockexchange", className: BuildingClassType.PeepCapacityResource, state: WorldObjectState.Plowed, worldFlatId: 5);
        var player = Faker.Player(world: Faker.World(objects: [building]));
        player.SetCash(100);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        Context.ChangeTracker.Clear();

        var handler = new BuyMechanicDataGate(Context);

        var act = () => handler.HandlePacket(CreateRequest(5, "unknown_gate"), player.Id, TestContext.Current.CancellationToken);

        await act.Should().ThrowAsync<Exception>();

        var storedPlayer = await Context.Set<Player>().AsNoTracking().FirstAsync(x => x.Id == player.Id, TestContext.Current.CancellationToken);
        storedPlayer.Cash.Should().Be(100);
    }
}
