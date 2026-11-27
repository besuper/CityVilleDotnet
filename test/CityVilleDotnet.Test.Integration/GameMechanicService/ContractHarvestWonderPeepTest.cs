using AwesomeAssertions;
using CityVilleDotnet.Api.Services.GameMechanicService;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Integration.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Test.Integration.GameMechanicService;

// test_vip_plane (HarvestWonderPeepContractMechanic) + test_vip_plane_contract: cost 100, default table 100 coins, firstTime table gives test_vip_coupon_a
// test_wonder_terminal: keyword wonderTerminal
[Collection("Database")]
public class ContractHarvestWonderPeepTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    private const int PlaneId = 5;

    private async Task<Player> SaveGrownPlane(params WorldObject[] others)
    {
        var plane = Faker.WorldObject(itemName: "test_vip_plane", className: BuildingClassType.Airplane, state: WorldObjectState.Plowed, worldFlatId: PlaneId);
        plane.AttachToMacroObject("test_airport_1");
        var player = Faker.Player(world: Faker.World(objects: [plane, ..others]));
        player.SetGold(100);
        player.PlantMechanicContract(plane, GameSettingsManager.Instance.GetItem("test_vip_plane")!, GameSettingsManager.Instance.GetItem("test_vip_plane_contract")!);
        plane.SetFullGrown();

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        Context.ChangeTracker.Clear();

        return player;
    }

    private WorldObject Terminal(string macroName, int worldFlatId = 6)
    {
        var terminal = Faker.WorldObject(itemName: "test_wonder_terminal", className: BuildingClassType.TimedHarvestableResource, worldFlatId: worldFlatId);
        terminal.AttachToMacroObject(macroName);

        return terminal;
    }

    private Task<Player> LoadPlayer(Guid id) => Context.Set<Player>().AsNoTracking()
        .Include(x => x.Coupons)
        .FirstAsync(x => x.Id == id, TestContext.Current.CancellationToken);

    private Task<WorldObject> LoadObject(int id) => Context.Set<WorldObject>().AsNoTracking()
        .Include(x => x.MechanicCounters)
        .FirstAsync(x => x.Id == id, TestContext.Current.CancellationToken);

    private Task HarvestPlane(Guid playerId) => new ContractHarvest(Context)
        .HandlePacket(new ContractHarvestRequest { ObjectId = PlaneId, GameMode = "GMPlay" }, playerId, TestContext.Current.CancellationToken);

    [Fact]
    public async Task ContractHarvest_VipPlaneFirstHarvest_GivesVipCouponAndMarksTerminal()
    {
        var terminal = Terminal("test_airport_1");
        var player = await SaveGrownPlane(terminal);

        await HarvestPlane(player.Id);

        var storedPlayer = await LoadPlayer(player.Id);
        storedPlayer.Coupons.Should().ContainSingle(x => x.Name == "test_vip_coupon_a" && x.WorldFlatId == null);
        storedPlayer.Gold.Should().Be(0);
        (await LoadObject(terminal.Id)).MechanicCounters.Should().ContainSingle(x => x.MechanicType == "firstTime");
    }

    [Fact]
    public async Task ContractHarvest_VipPlaneSecondHarvest_RollsDefaultTable()
    {
        var terminal = Terminal("test_airport_1");
        terminal.IncrementMechanicCounter("firstTime");
        var player = await SaveGrownPlane(terminal);

        await HarvestPlane(player.Id);

        var storedPlayer = await LoadPlayer(player.Id);
        storedPlayer.Coupons.Should().BeEmpty();
        storedPlayer.Gold.Should().Be(100);
        (await LoadObject(terminal.Id)).MechanicCounters.Should().ContainSingle(x => x.MechanicType == "firstTime").Which.Count.Should().Be(1);
    }

    [Fact]
    public async Task ContractHarvest_VipPlaneWithoutTerminal_RollsDefaultTable()
    {
        var player = await SaveGrownPlane();

        await HarvestPlane(player.Id);

        var storedPlayer = await LoadPlayer(player.Id);
        storedPlayer.Coupons.Should().BeEmpty();
        storedPlayer.Gold.Should().Be(100);
    }

    [Fact]
    public async Task ContractHarvest_VipPlaneTerminalOfAnotherAirport_RollsDefaultTable()
    {
        var terminal = Terminal("test_airport_2");
        var player = await SaveGrownPlane(terminal);

        await HarvestPlane(player.Id);

        (await LoadPlayer(player.Id)).Gold.Should().Be(100);
        (await LoadObject(terminal.Id)).MechanicCounters.Should().BeEmpty();
    }

    [Fact]
    public async Task ContractHarvest_VipPlane_PlowsPlane()
    {
        var player = await SaveGrownPlane(Terminal("test_airport_1"));

        await HarvestPlane(player.Id);

        var storedPlane = await Context.Set<WorldObject>().AsNoTracking().FirstAsync(x => x.WorldFlatId == PlaneId && x.ItemName == "test_vip_plane", TestContext.Current.CancellationToken);
        storedPlane.State.Should().Be(WorldObjectState.Plowed);
        storedPlane.ContractName.Should().BeNull();
    }

    [Fact]
    public async Task ContractHarvest_VipPlaneCouponAlreadyHeld_DoesNotDuplicateCoupon()
    {
        var terminal = Terminal("test_airport_1");
        var plane = Faker.WorldObject(itemName: "test_vip_plane", className: BuildingClassType.Airplane, state: WorldObjectState.Plowed, worldFlatId: PlaneId);
        plane.AttachToMacroObject("test_airport_1");
        var player = Faker.Player(world: Faker.World(objects: [plane, terminal]));
        player.SetGold(100);
        player.GiveCoupon("test_vip_coupon_a");
        player.PlantMechanicContract(plane, GameSettingsManager.Instance.GetItem("test_vip_plane")!, GameSettingsManager.Instance.GetItem("test_vip_plane_contract")!);
        plane.SetFullGrown();
        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        Context.ChangeTracker.Clear();

        await HarvestPlane(player.Id);

        (await LoadPlayer(player.Id)).Coupons.Should().ContainSingle(x => x.Name == "test_vip_coupon_a");
    }
}
