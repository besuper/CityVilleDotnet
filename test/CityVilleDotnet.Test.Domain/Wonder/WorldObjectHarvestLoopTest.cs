using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Settings.GameSettings;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Domain.Fixtures;

namespace CityVilleDotnet.Test.Domain.Wonder;

// test_wonder: timer 180, sleep 1000, boost 30
// test_wonder_terminal: timer 180, test_vip_coupon_b overrides the timer to 60, no sleep
[Collection("Domain")]
public class WorldObjectHarvestLoopTest
{
    private static readonly DateTime Now = new(2026, 1, 15, 12, 0, 0, DateTimeKind.Utc);

    private static HarvestLoopConfigItem WonderConfig => GameSettingsManager.Instance.GetItem("test_wonder")!.HarvestLoopConfig!;
    private static HarvestLoopConfigItem TerminalConfig => GameSettingsManager.Instance.GetItem("test_wonder_terminal")!.HarvestLoopConfig!;

    private static CityVilleDotnet.Domain.Entities.WorldObject Wonder(Faker faker) => faker.WorldObject(itemName: "test_wonder", className: BuildingClassType.Wonder);

    [Fact]
    public void WorldObject_IsHarvestLoopOpen_NeverStarted_ReturnsFalse()
    {
        var faker = new Faker();
        var wonder = Wonder(faker);

        wonder.IsHarvestLoopOpen(WonderConfig, Now).Should().BeFalse();
    }

    [Fact]
    public void WorldObject_IsHarvestLoopOpen_BeforeTimerEnd_ReturnsTrue()
    {
        var faker = new Faker();
        var wonder = Wonder(faker);
        wonder.StartHarvestLoop(WonderConfig, Now, null);

        wonder.IsHarvestLoopOpen(WonderConfig, Now.AddSeconds(179)).Should().BeTrue();
    }

    [Fact]
    public void WorldObject_IsHarvestLoopOpen_AtTimerEnd_ReturnsFalse()
    {
        var faker = new Faker();
        var wonder = Wonder(faker);
        wonder.StartHarvestLoop(WonderConfig, Now, null);

        wonder.IsHarvestLoopOpen(WonderConfig, Now.AddSeconds(180)).Should().BeFalse();
    }

    [Fact]
    public void WorldObject_IsHarvestLoopOpen_OverriddenCoupon_UsesOverrideDuration()
    {
        var faker = new Faker();
        var terminal = faker.WorldObject(itemName: "test_wonder_terminal", className: BuildingClassType.TimedHarvestableResource);
        terminal.StartHarvestLoop(TerminalConfig, Now, "test_vip_coupon_b");

        terminal.IsHarvestLoopOpen(TerminalConfig, Now.AddSeconds(59)).Should().BeTrue();
        terminal.IsHarvestLoopOpen(TerminalConfig, Now.AddSeconds(60)).Should().BeFalse();
    }

    [Fact]
    public void WorldObject_StartHarvestLoop_NeverStarted_SetsStartTimeWithoutCoupon()
    {
        var faker = new Faker();
        var wonder = Wonder(faker);

        wonder.StartHarvestLoop(WonderConfig, Now, null);

        wonder.HarvestLoopStartTime.Should().Be(Now);
        wonder.AppliedCoupon.Should().BeNull();
    }

    [Fact]
    public void WorldObject_StartHarvestLoop_Sleeping_Throws()
    {
        var faker = new Faker();
        var wonder = Wonder(faker);
        wonder.StartHarvestLoop(WonderConfig, Now, null);

        var act = () => wonder.StartHarvestLoop(WonderConfig, Now.AddSeconds(500), null);

        act.Should().Throw<Exception>();
        wonder.HarvestLoopStartTime.Should().Be(Now);
    }

    [Fact]
    public void WorldObject_StartHarvestLoop_AfterTimerAndSleep_Restarts()
    {
        var faker = new Faker();
        var wonder = Wonder(faker);
        wonder.StartHarvestLoop(WonderConfig, Now, null);

        wonder.StartHarvestLoop(WonderConfig, Now.AddSeconds(1180), null);

        wonder.HarvestLoopStartTime.Should().Be(Now.AddSeconds(1180));
    }

    [Fact]
    public void WorldObject_StartHarvestLoop_ReadyWithinTolerance_Restarts()
    {
        var faker = new Faker();
        var wonder = Wonder(faker);
        wonder.StartHarvestLoop(WonderConfig, Now, null);

        wonder.StartHarvestLoop(WonderConfig, Now.AddSeconds(1150), null);

        wonder.HarvestLoopStartTime.Should().Be(Now.AddSeconds(1150));
    }

    [Fact]
    public void WorldObject_StartHarvestLoop_Coupon_SkipsSleepAndStoresAppliedCoupon()
    {
        var faker = new Faker();
        var terminal = faker.WorldObject(itemName: "test_wonder_terminal", className: BuildingClassType.TimedHarvestableResource);
        terminal.StartHarvestLoop(TerminalConfig, Now, "test_vip_coupon_a");

        terminal.StartHarvestLoop(TerminalConfig, Now.AddSeconds(10), "test_vip_coupon_b");

        terminal.HarvestLoopStartTime.Should().Be(Now.AddSeconds(10));
        terminal.AppliedCoupon.Should().Be("test_vip_coupon_b");
    }

    [Fact]
    public void WorldObject_StartHarvestLoop_WithoutCoupon_KeepsPreviousAppliedCoupon()
    {
        var faker = new Faker();
        var terminal = faker.WorldObject(itemName: "test_wonder_terminal", className: BuildingClassType.TimedHarvestableResource);
        terminal.StartHarvestLoop(TerminalConfig, Now, "test_vip_coupon_b");

        terminal.StartHarvestLoop(TerminalConfig, Now.AddSeconds(60), null);

        terminal.AppliedCoupon.Should().Be("test_vip_coupon_b");
    }

    [Fact]
    public void WorldObject_BoostHarvestLoop_Open_PushesStartTime()
    {
        var faker = new Faker();
        var wonder = Wonder(faker);
        wonder.StartHarvestLoop(WonderConfig, Now, null);

        wonder.BoostHarvestLoop(WonderConfig, Now.AddSeconds(100));

        wonder.HarvestLoopStartTime.Should().Be(Now.AddSeconds(30));
    }

    [Fact]
    public void WorldObject_BoostHarvestLoop_ClosedWithinTolerance_PushesStartTime()
    {
        var faker = new Faker();
        var wonder = Wonder(faker);
        wonder.StartHarvestLoop(WonderConfig, Now, null);

        wonder.BoostHarvestLoop(WonderConfig, Now.AddSeconds(200));

        wonder.HarvestLoopStartTime.Should().Be(Now.AddSeconds(30));
    }

    [Fact]
    public void WorldObject_BoostHarvestLoop_Closed_Throws()
    {
        var faker = new Faker();
        var wonder = Wonder(faker);
        wonder.StartHarvestLoop(WonderConfig, Now, null);

        var act = () => wonder.BoostHarvestLoop(WonderConfig, Now.AddSeconds(500));

        act.Should().Throw<Exception>();
        wonder.HarvestLoopStartTime.Should().Be(Now);
    }

    [Fact]
    public void WorldObject_BoostHarvestLoop_NeverStarted_Throws()
    {
        var faker = new Faker();
        var wonder = Wonder(faker);

        var act = () => wonder.BoostHarvestLoop(WonderConfig, Now);

        act.Should().Throw<Exception>();
    }

    [Fact]
    public void WorldObject_FinishHarvestLoopMaintenance_Sleeping_ResetsStartTime()
    {
        var faker = new Faker();
        var wonder = Wonder(faker);
        wonder.StartHarvestLoop(WonderConfig, Now, null);

        wonder.FinishHarvestLoopMaintenance(WonderConfig, Now.AddSeconds(500));

        wonder.HarvestLoopStartTime.Should().BeNull();
    }

    [Fact]
    public void WorldObject_FinishHarvestLoopMaintenance_Open_Throws()
    {
        var faker = new Faker();
        var wonder = Wonder(faker);
        wonder.StartHarvestLoop(WonderConfig, Now, null);

        var act = () => wonder.FinishHarvestLoopMaintenance(WonderConfig, Now.AddSeconds(100));

        act.Should().Throw<Exception>();
        wonder.HarvestLoopStartTime.Should().Be(Now);
    }

    [Fact]
    public void WorldObject_FinishHarvestLoopMaintenance_NeverStarted_Throws()
    {
        var faker = new Faker();
        var wonder = Wonder(faker);

        var act = () => wonder.FinishHarvestLoopMaintenance(WonderConfig, Now);

        act.Should().Throw<Exception>();
    }

    [Fact]
    public void WorldObject_FinishHarvestLoopMaintenance_ThenStart_IsReadyImmediately()
    {
        var faker = new Faker();
        var wonder = Wonder(faker);
        wonder.StartHarvestLoop(WonderConfig, Now, null);
        wonder.FinishHarvestLoopMaintenance(WonderConfig, Now.AddSeconds(500));

        wonder.StartHarvestLoop(WonderConfig, Now.AddSeconds(501), null);

        wonder.HarvestLoopStartTime.Should().Be(Now.AddSeconds(501));
    }
}
