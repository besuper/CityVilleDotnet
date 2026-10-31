using AwesomeAssertions;
using CityVilleDotnet.Api.Services.UserService;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Integration.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace CityVilleDotnet.Test.Integration.UserService;

[Collection("Database")]
public class PurchaseRemainingCrewTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    private async Task<(Player Player, WorldObject Building)> SeedAsync(string itemName, int existingCrew, int cash)
    {
        var building = Faker.WorldObject(itemName: itemName, className: BuildingClassType.Municipal, worldFlatId: 5);

        for (var i = 0; i < existingCrew; i++)
        {
            building.AddCrewMember(null);
        }

        var world = Faker.World(objects: [building]);
        var player = Faker.Player(world: world);
        player.SetCash(cash);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        return (player, building);
    }

    private async Task<int> CountCrewAsync(WorldObject building)
    {
        var updated = await Context.Set<WorldObject>()
            .Include(x => x.CrewMembers)
            .FirstAsync(x => x.Id == building.Id, TestContext.Current.CancellationToken);

        return updated.CrewMembers.Count;
    }

    [Fact]
    public async Task PurchaseRemainingCrew_NullGateName_FirstGateIsNotCrew_UsesCrewGate()
    {
        // 4 slots * 2 cash = 8, all slots open => 25% discount => 6
        var (player, building) = await SeedAsync("test_crew_hood", existingCrew: 0, cash: 50);

        var handler = new PurchaseRemainingCrew(Context, NullLogger<PurchaseRemainingCrew>.Instance);

        var response = await handler.HandlePacket(new PurchaseRemainingCrewRequest { ObjectId = 5, GateName = null }, player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        player.Cash.Should().Be(44);
        (await CountCrewAsync(building)).Should().Be(4);
    }

    [Fact]
    public async Task PurchaseRemainingCrew_NullGateName_SeveralCrewGates_UsesFirstCrewGate()
    {
        var (player, building) = await SeedAsync("test_crew_upgrade", existingCrew: 0, cash: 50);

        var handler = new PurchaseRemainingCrew(Context, NullLogger<PurchaseRemainingCrew>.Instance);

        var response = await handler.HandlePacket(new PurchaseRemainingCrewRequest { ObjectId = 5, GateName = null }, player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        player.Cash.Should().Be(44);
        (await CountCrewAsync(building)).Should().Be(4);
    }

    [Fact]
    public async Task PurchaseRemainingCrew_ExplicitGateName_UsesThatGate()
    {
        // 10 slots * 3 cash = 30, all slots open => 25% discount => 22
        var (player, building) = await SeedAsync("test_crew_upgrade", existingCrew: 0, cash: 50);

        var handler = new PurchaseRemainingCrew(Context, NullLogger<PurchaseRemainingCrew>.Instance);

        var response = await handler.HandlePacket(new PurchaseRemainingCrewRequest { ObjectId = 5, GateName = "pre_upgrade" }, player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        player.Cash.Should().Be(28);
        (await CountCrewAsync(building)).Should().Be(10);
    }

    [Fact]
    public async Task PurchaseRemainingCrew_FewSlotsOpen_NoDiscount()
    {
        // 2 of 10 slots open (20% < 25%) => 2 * 3 = 6
        var (player, building) = await SeedAsync("test_crew_upgrade", existingCrew: 8, cash: 50);

        var handler = new PurchaseRemainingCrew(Context, NullLogger<PurchaseRemainingCrew>.Instance);

        var response = await handler.HandlePacket(new PurchaseRemainingCrewRequest { ObjectId = 5, GateName = "pre_upgrade" }, player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        player.Cash.Should().Be(44);
        (await CountCrewAsync(building)).Should().Be(10);
    }

    [Fact]
    public async Task PurchaseRemainingCrew_CrewAlreadyFull_ReturnsForceReloadWithoutCharging()
    {
        var (player, building) = await SeedAsync("test_crew_hood", existingCrew: 4, cash: 50);

        var handler = new PurchaseRemainingCrew(Context, NullLogger<PurchaseRemainingCrew>.Instance);

        var response = await handler.HandlePacket(new PurchaseRemainingCrewRequest { ObjectId = 5, GateName = null }, player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be((int)GameErrorType.ForceReload);
        player.Cash.Should().Be(50);
        (await CountCrewAsync(building)).Should().Be(4);
    }

    [Fact]
    public async Task PurchaseRemainingCrew_NotEnoughCash_ReturnsForceReloadWithoutAddingCrew()
    {
        var (player, building) = await SeedAsync("test_crew_hood", existingCrew: 0, cash: 5);

        var handler = new PurchaseRemainingCrew(Context, NullLogger<PurchaseRemainingCrew>.Instance);

        var response = await handler.HandlePacket(new PurchaseRemainingCrewRequest { ObjectId = 5, GateName = null }, player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be((int)GameErrorType.ForceReload);
        player.Cash.Should().Be(5);
        (await CountCrewAsync(building)).Should().Be(0);
    }
}
