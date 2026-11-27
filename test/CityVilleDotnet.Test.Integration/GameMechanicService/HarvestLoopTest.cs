using AwesomeAssertions;
using CityVilleDotnet.Api.Services.GameMechanicService;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.Quest;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Integration.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Time.Testing;

namespace CityVilleDotnet.Test.Integration.GameMechanicService;

// test_wonder: lock test_wonder_lock, timer 180, sleep 1000, energy 3, 50 coins, boost 10 cash / +10 energy / +30s
// test_coupon_wonder: requires test_wonder_coupon
// test_wonder_terminal: coupons test_vip_coupon_a/b (b: 60s)
[Collection("Database")]
public class HarvestLoopTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    private static readonly DateTimeOffset StartTime = new(2026, 1, 15, 12, 0, 0, TimeSpan.Zero);

    private HarvestLoop CreateHandler() => new(Context, new ConfigurationBuilder().Build());

    private static HarvestLoopRequest Request(int objectId, string operation = "harvest", string? couponName = null) => new()
    {
        ObjectId = objectId,
        Params = new HarvestLoopParamsRequest { Operation = operation, CouponName = couponName }
    };

    private async Task<Player> SavePlayer(params WorldObject[] objects)
    {
        var player = Faker.Player(world: Faker.World(objects: [..objects]));
        player.SetGold(0);
        player.SetEnergy(10);
        player.SetCash(25);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        Context.ChangeTracker.Clear();

        return player;
    }

    private Task<Player> LoadPlayer(Guid id) => Context.Set<Player>().AsNoTracking()
        .Include(x => x.Coupons)
        .Include(x => x.Quests)
        .FirstAsync(x => x.Id == id, TestContext.Current.CancellationToken);

    private Task<WorldObject> LoadObject(int id) => Context.Set<WorldObject>().AsNoTracking()
        .FirstAsync(x => x.Id == id, TestContext.Current.CancellationToken);

    [Fact]
    public async Task HarvestLoop_Harvest_Ready_StartsLoopPaysCoinsAndProgressesQuest()
    {
        ServerUtils.TimeProvider = new FakeTimeProvider(StartTime);
        var wonder = Faker.WorldObject(itemName: "test_wonder", className: BuildingClassType.Wonder, worldFlatId: 42);
        var player = Faker.Player(world: Faker.World(objects: [wonder]));
        player.SetGold(0);
        player.SetEnergy(10);
        player.Quests.Add(Faker.Quest(name: "qm_test_wonder", length: 3));
        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        Context.ChangeTracker.Clear();

        var response = await CreateHandler().HandlePacket(Request(42), player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        var storedPlayer = await LoadPlayer(player.Id);
        storedPlayer.Gold.Should().Be(50);
        storedPlayer.Energy.Should().Be(7);
        storedPlayer.Quests.First(x => x.Name == "qm_test_wonder").Progress.Should().Equal(1, 1, 1);
        (await LoadObject(wonder.Id)).HarvestLoopStartTime.Should().Be(StartTime.UtcDateTime);
    }

    [Fact]
    public async Task HarvestLoop_Harvest_Sleeping_ThrowsAndKeepsStartTime()
    {
        ServerUtils.TimeProvider = new FakeTimeProvider(StartTime);
        var wonder = Faker.WorldObject(itemName: "test_wonder", className: BuildingClassType.Wonder, worldFlatId: 42);
        wonder.StartHarvestLoop(GameSettingsManager.Instance.GetItem("test_wonder")!.HarvestLoopConfig!, StartTime.UtcDateTime.AddSeconds(-500), null);
        var player = await SavePlayer(wonder);

        var act = () => CreateHandler().HandlePacket(Request(42), player.Id, TestContext.Current.CancellationToken);

        await act.Should().ThrowAsync<Exception>();
        Context.ChangeTracker.Clear();
        (await LoadObject(wonder.Id)).HarvestLoopStartTime.Should().Be(StartTime.UtcDateTime.AddSeconds(-500));
        (await LoadPlayer(player.Id)).Gold.Should().Be(0);
    }

    [Fact]
    public async Task HarvestLoop_Harvest_AfterSleep_Restarts()
    {
        ServerUtils.TimeProvider = new FakeTimeProvider(StartTime);
        var wonder = Faker.WorldObject(itemName: "test_wonder", className: BuildingClassType.Wonder, worldFlatId: 42);
        wonder.StartHarvestLoop(GameSettingsManager.Instance.GetItem("test_wonder")!.HarvestLoopConfig!, StartTime.UtcDateTime.AddSeconds(-1200), null);
        var player = await SavePlayer(wonder);

        await CreateHandler().HandlePacket(Request(42), player.Id, TestContext.Current.CancellationToken);

        (await LoadObject(wonder.Id)).HarvestLoopStartTime.Should().Be(StartTime.UtcDateTime);
    }

    [Fact]
    public async Task HarvestLoop_Harvest_OtherLockedWonderOpen_Throws()
    {
        ServerUtils.TimeProvider = new FakeTimeProvider(StartTime);
        var wonder = Faker.WorldObject(itemName: "test_wonder", className: BuildingClassType.Wonder, worldFlatId: 42);
        var other = Faker.WorldObject(itemName: "test_wonder", className: BuildingClassType.Wonder, worldFlatId: 43);
        other.StartHarvestLoop(GameSettingsManager.Instance.GetItem("test_wonder")!.HarvestLoopConfig!, StartTime.UtcDateTime.AddSeconds(-60), null);
        var player = await SavePlayer(wonder, other);

        var act = () => CreateHandler().HandlePacket(Request(42), player.Id, TestContext.Current.CancellationToken);

        await act.Should().ThrowAsync<Exception>();
        Context.ChangeTracker.Clear();
        (await LoadObject(wonder.Id)).HarvestLoopStartTime.Should().BeNull();
    }

    [Fact]
    public async Task HarvestLoop_Harvest_RequiredCoupon_ConsumesCouponAndStoresAppliedCoupon()
    {
        ServerUtils.TimeProvider = new FakeTimeProvider(StartTime);
        var wonder = Faker.WorldObject(itemName: "test_coupon_wonder", className: BuildingClassType.Wonder, worldFlatId: 42);
        var player = Faker.Player(world: Faker.World(objects: [wonder]));
        player.GiveCoupon("test_wonder_coupon");
        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        Context.ChangeTracker.Clear();

        await CreateHandler().HandlePacket(Request(42), player.Id, TestContext.Current.CancellationToken);

        (await LoadPlayer(player.Id)).Coupons.Should().BeEmpty();
        var storedWonder = await LoadObject(wonder.Id);
        storedWonder.HarvestLoopStartTime.Should().Be(StartTime.UtcDateTime);
        storedWonder.AppliedCoupon.Should().Be("test_wonder_coupon");
    }

    [Fact]
    public async Task HarvestLoop_Harvest_RequiredCouponMissing_Throws()
    {
        ServerUtils.TimeProvider = new FakeTimeProvider(StartTime);
        var wonder = Faker.WorldObject(itemName: "test_coupon_wonder", className: BuildingClassType.Wonder, worldFlatId: 42);
        var player = await SavePlayer(wonder);

        var act = () => CreateHandler().HandlePacket(Request(42), player.Id, TestContext.Current.CancellationToken);

        await act.Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async Task HarvestLoop_UseCouponOnTerminal_ConsumesVipCouponAndStoresAppliedCoupon()
    {
        ServerUtils.TimeProvider = new FakeTimeProvider(StartTime);
        var terminal = Faker.WorldObject(itemName: "test_wonder_terminal", className: BuildingClassType.TimedHarvestableResource, worldFlatId: 42);
        var player = Faker.Player(world: Faker.World(objects: [terminal]));
        player.GiveCoupon("test_vip_coupon_a");
        player.GiveCoupon("test_vip_coupon_b");
        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        Context.ChangeTracker.Clear();

        await CreateHandler().HandlePacket(Request(42, couponName: "test_vip_coupon_b"), player.Id, TestContext.Current.CancellationToken);

        (await LoadPlayer(player.Id)).Coupons.Should().ContainSingle().Which.Name.Should().Be("test_vip_coupon_a");
        var storedTerminal = await LoadObject(terminal.Id);
        storedTerminal.HarvestLoopStartTime.Should().Be(StartTime.UtcDateTime);
        storedTerminal.AppliedCoupon.Should().Be("test_vip_coupon_b");
    }

    [Fact]
    public async Task HarvestLoop_HudTimerClicked_Open_ChargesCashGivesEnergyAndExtendsTimer()
    {
        ServerUtils.TimeProvider = new FakeTimeProvider(StartTime);
        var wonder = Faker.WorldObject(itemName: "test_wonder", className: BuildingClassType.Wonder, worldFlatId: 42);
        wonder.StartHarvestLoop(GameSettingsManager.Instance.GetItem("test_wonder")!.HarvestLoopConfig!, StartTime.UtcDateTime.AddSeconds(-100), null);
        var player = await SavePlayer(wonder);

        var response = await CreateHandler().HandlePacket(Request(42, "hudTimerClicked"), player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        var storedPlayer = await LoadPlayer(player.Id);
        storedPlayer.Cash.Should().Be(15);
        storedPlayer.Energy.Should().Be(20);
        (await LoadObject(wonder.Id)).HarvestLoopStartTime.Should().Be(StartTime.UtcDateTime.AddSeconds(-70));
    }

    [Fact]
    public async Task HarvestLoop_HudTimerClicked_Closed_ThrowsWithoutCharging()
    {
        ServerUtils.TimeProvider = new FakeTimeProvider(StartTime);
        var wonder = Faker.WorldObject(itemName: "test_wonder", className: BuildingClassType.Wonder, worldFlatId: 42);
        wonder.StartHarvestLoop(GameSettingsManager.Instance.GetItem("test_wonder")!.HarvestLoopConfig!, StartTime.UtcDateTime.AddSeconds(-500), null);
        var player = await SavePlayer(wonder);

        var act = () => CreateHandler().HandlePacket(Request(42, "hudTimerClicked"), player.Id, TestContext.Current.CancellationToken);

        await act.Should().ThrowAsync<Exception>();
        Context.ChangeTracker.Clear();
        (await LoadPlayer(player.Id)).Cash.Should().Be(25);
    }

    [Theory]
    [InlineData("harvest", true)]
    [InlineData("hudTimerClicked", true)]
    [InlineData("useConsumables", false)]
    [InlineData("", false)]
    public void HarvestLoopRequestValidator_Operation_OnlyAcceptsHarvestAndHudTimerClicked(string operation, bool isValid)
    {
        var result = new HarvestLoopRequestValidator().Validate(Request(42, operation));

        result.IsValid.Should().Be(isValid);
    }

    [Fact]
    public void HarvestLoopRequestValidator_CouponNameTooLong_IsInvalid()
    {
        var result = new HarvestLoopRequestValidator().Validate(Request(42, couponName: new string('a', 65)));

        result.IsValid.Should().BeFalse();
    }
}
