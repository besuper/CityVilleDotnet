using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Settings.GameSettings;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.Quest;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Domain.Fixtures;

namespace CityVilleDotnet.Test.Domain.Wonder;

// test_wonder: lock test_wonder_lock, timer 180, sleep 1000, energy 3, 50 coins, boost 10 cash / +10 energy / +30s, maintenance 36 cash
// test_coupon_wonder: requires test_wonder_coupon
// test_wonder_terminal: keyword wonderTerminal, coupons test_vip_coupon_a/b (b: 60s)
// test_vip_plane + test_vip_plane_contract: default table 100 coins, firstTime table gives test_vip_coupon_a
[Collection("Domain")]
public class PlayerWonderTest
{
    private static readonly DateTime Now = new(2026, 1, 15, 12, 0, 0, DateTimeKind.Utc);
    private static readonly Dictionary<string, int> NoExperiments = [];

    private static GameItem Item(string name) => GameSettingsManager.Instance.GetItem(name)!;

    private static CityVilleDotnet.Domain.Entities.WorldObject Wonder(Faker faker, int worldFlatId = 1) =>
        faker.WorldObject(itemName: "test_wonder", className: BuildingClassType.Wonder, worldFlatId: worldFlatId);

    private static CityVilleDotnet.Domain.Entities.WorldObject Terminal(Faker faker, int worldFlatId = 2) =>
        faker.WorldObject(itemName: "test_wonder_terminal", className: BuildingClassType.TimedHarvestableResource, worldFlatId: worldFlatId);

    [Fact]
    public void Player_HarvestWonder_Ready_StartsLoopAndPaysCoinsForEnergy()
    {
        var faker = new Faker();
        var wonder = Wonder(faker);
        var world = faker.World(objects: [wonder]);
        var player = faker.Player(world: world);
        player.SetGold(0);
        player.SetEnergy(10);

        player.HarvestWonder(world, wonder, Item("test_wonder"), Now, null, NoExperiments);

        wonder.HarvestLoopStartTime.Should().Be(Now);
        player.Gold.Should().Be(50);
        player.Energy.Should().Be(7);
    }

    [Fact]
    public void Player_HarvestWonder_NotEnoughEnergy_Throws()
    {
        var faker = new Faker();
        var wonder = Wonder(faker);
        var world = faker.World(objects: [wonder]);
        var player = faker.Player(world: world);
        player.SetEnergy(0);

        var act = () => player.HarvestWonder(world, wonder, Item("test_wonder"), Now, null, NoExperiments);

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Player_HarvestWonder_ProgressesHarvestQuests()
    {
        var faker = new Faker();
        var wonder = Wonder(faker);
        var world = faker.World(objects: [wonder]);
        var player = faker.Player(world: world);
        var quest = faker.Quest(name: "qm_test_wonder", length: 3);
        player.Quests.Add(quest);

        player.HarvestWonder(world, wonder, Item("test_wonder"), Now, null, NoExperiments);

        quest.Progress.Should().Equal(1, 1, 1);
    }

    [Fact]
    public void Player_HarvestWonder_OtherLockedWonderOpen_Throws()
    {
        var faker = new Faker();
        var wonder = Wonder(faker, 1);
        var other = Wonder(faker, 2);
        var world = faker.World(objects: [wonder, other]);
        var player = faker.Player(world: world);
        other.StartHarvestLoop(Item("test_wonder").HarvestLoopConfig!, Now.AddSeconds(-60), null);

        var act = () => player.HarvestWonder(world, wonder, Item("test_wonder"), Now, null, NoExperiments);

        act.Should().Throw<Exception>();
        wonder.HarvestLoopStartTime.Should().BeNull();
    }

    [Fact]
    public void Player_HarvestWonder_OtherLockedWonderClosed_Starts()
    {
        var faker = new Faker();
        var wonder = Wonder(faker, 1);
        var other = Wonder(faker, 2);
        var world = faker.World(objects: [wonder, other]);
        var player = faker.Player(world: world);
        other.StartHarvestLoop(Item("test_wonder").HarvestLoopConfig!, Now.AddSeconds(-500), null);

        player.HarvestWonder(world, wonder, Item("test_wonder"), Now, null, NoExperiments);

        wonder.HarvestLoopStartTime.Should().Be(Now);
    }

    [Fact]
    public void Player_HarvestWonder_LockUsesOtherObjectOverrideDuration()
    {
        // the terminal opened with test_vip_coupon_b closes after 60s, not 180s
        var faker = new Faker();
        var wonder = Wonder(faker, 1);
        var terminal = Terminal(faker);
        var world = faker.World(objects: [wonder, terminal]);
        var player = faker.Player(world: world);
        terminal.StartHarvestLoop(Item("test_wonder_terminal").HarvestLoopConfig!, Now.AddSeconds(-90), "test_vip_coupon_b");

        player.HarvestWonder(world, wonder, Item("test_wonder"), Now, null, NoExperiments);

        wonder.HarvestLoopStartTime.Should().Be(Now);
    }

    [Fact]
    public void Player_HarvestWonder_RequiredCouponMissing_Throws()
    {
        var faker = new Faker();
        var wonder = faker.WorldObject(itemName: "test_coupon_wonder", className: BuildingClassType.Wonder, worldFlatId: 1);
        var world = faker.World(objects: [wonder]);
        var player = faker.Player(world: world);

        var act = () => player.HarvestWonder(world, wonder, Item("test_coupon_wonder"), Now, null, NoExperiments);

        act.Should().Throw<Exception>();
        wonder.HarvestLoopStartTime.Should().BeNull();
    }

    [Fact]
    public void Player_HarvestWonder_RequiredGlobalCoupon_ConsumesCouponAndStarts()
    {
        var faker = new Faker();
        var wonder = faker.WorldObject(itemName: "test_coupon_wonder", className: BuildingClassType.Wonder, worldFlatId: 1);
        var world = faker.World(objects: [wonder]);
        var player = faker.Player(world: world);
        player.GiveCoupon("test_wonder_coupon");

        player.HarvestWonder(world, wonder, Item("test_coupon_wonder"), Now, null, NoExperiments);

        player.HasCoupon("test_wonder_coupon").Should().BeFalse();
        wonder.HarvestLoopStartTime.Should().Be(Now);
        wonder.AppliedCoupon.Should().Be("test_wonder_coupon");
    }

    [Fact]
    public void Player_HarvestWonder_AssociatedCoupon_IsConsumedBeforeGlobalOne()
    {
        var faker = new Faker();
        var wonder = faker.WorldObject(itemName: "test_coupon_wonder", className: BuildingClassType.Wonder, worldFlatId: 7);
        var world = faker.World(objects: [wonder]);
        var player = faker.Player(world: world);
        player.GiveCoupon("test_wonder_coupon");
        player.GiveCoupon("test_wonder_coupon", 7);

        player.HarvestWonder(world, wonder, Item("test_coupon_wonder"), Now, null, NoExperiments);

        player.HasCoupon("test_wonder_coupon").Should().BeTrue();
        player.Coupons.Should().NotContain(x => x.WorldFlatId == 7);
    }

    [Fact]
    public void Player_HarvestWonder_CouponSkipsSleep()
    {
        var faker = new Faker();
        var terminal = Terminal(faker);
        var world = faker.World(objects: [terminal]);
        var player = faker.Player(world: world);
        var config = Item("test_wonder_terminal").HarvestLoopConfig!;
        terminal.StartHarvestLoop(config, Now.AddSeconds(-500), "test_vip_coupon_a");
        player.GiveCoupon("test_vip_coupon_b");

        player.HarvestWonder(world, terminal, Item("test_wonder_terminal"), Now, "test_vip_coupon_b", NoExperiments);

        terminal.HarvestLoopStartTime.Should().Be(Now);
        terminal.AppliedCoupon.Should().Be("test_vip_coupon_b");
        player.HasCoupon("test_vip_coupon_b").Should().BeFalse();
    }

    [Fact]
    public void Player_HarvestWonder_CouponNameNotInList_StartsWithoutCoupon()
    {
        var faker = new Faker();
        var terminal = Terminal(faker);
        var world = faker.World(objects: [terminal]);
        var player = faker.Player(world: world);
        player.GiveCoupon("test_wonder_coupon");

        player.HarvestWonder(world, terminal, Item("test_wonder_terminal"), Now, "test_wonder_coupon", NoExperiments);

        terminal.HarvestLoopStartTime.Should().Be(Now);
        terminal.AppliedCoupon.Should().BeNull();
        player.HasCoupon("test_wonder_coupon").Should().BeTrue();
    }

    [Fact]
    public void Player_BoostWonder_Open_ChargesCashGivesEnergyAndExtendsTimer()
    {
        var faker = new Faker();
        var wonder = Wonder(faker);
        var player = faker.Player(world: faker.World(objects: [wonder]));
        wonder.StartHarvestLoop(Item("test_wonder").HarvestLoopConfig!, Now, null);
        player.SetCash(25);
        var energyBefore = player.Energy;

        player.BoostWonder(wonder, Item("test_wonder"), Now.AddSeconds(100));

        player.Cash.Should().Be(15);
        player.Energy.Should().Be(energyBefore + 10);
        wonder.HarvestLoopStartTime.Should().Be(Now.AddSeconds(30));
    }

    [Fact]
    public void Player_BoostWonder_NotEnoughCash_ThrowsNotEnoughMoney()
    {
        var faker = new Faker();
        var wonder = Wonder(faker);
        var player = faker.Player(world: faker.World(objects: [wonder]));
        wonder.StartHarvestLoop(Item("test_wonder").HarvestLoopConfig!, Now, null);
        player.SetCash(5);

        var act = () => player.BoostWonder(wonder, Item("test_wonder"), Now.AddSeconds(100));

        act.Should().Throw<DomainException>().Which.Reason.Should().Be(GameErrorType.NotEnoughMoney);
        player.Cash.Should().Be(5);
    }

    [Fact]
    public void Player_BoostWonder_Closed_ThrowsWithoutCharging()
    {
        var faker = new Faker();
        var wonder = Wonder(faker);
        var player = faker.Player(world: faker.World(objects: [wonder]));
        wonder.StartHarvestLoop(Item("test_wonder").HarvestLoopConfig!, Now, null);
        player.SetCash(25);

        var act = () => player.BoostWonder(wonder, Item("test_wonder"), Now.AddSeconds(500));

        act.Should().Throw<Exception>();
        player.Cash.Should().Be(25);
    }

    [Fact]
    public void Player_FinishWonderMaintenanceWithCash_Sleeping_ChargesFullPriceAndWakesUp()
    {
        var faker = new Faker();
        var wonder = Wonder(faker);
        var player = faker.Player(world: faker.World(objects: [wonder]));
        wonder.StartHarvestLoop(Item("test_wonder").HarvestLoopConfig!, Now, null);
        player.SetCash(40);

        player.FinishWonderMaintenanceWithCash(wonder, Item("test_wonder"), Now.AddSeconds(500));

        player.Cash.Should().Be(4);
        wonder.HarvestLoopStartTime.Should().BeNull();
    }

    [Fact]
    public void Player_FinishWonderMaintenanceWithCash_NotEnoughCash_Throws()
    {
        var faker = new Faker();
        var wonder = Wonder(faker);
        var player = faker.Player(world: faker.World(objects: [wonder]));
        wonder.StartHarvestLoop(Item("test_wonder").HarvestLoopConfig!, Now, null);
        player.SetCash(10);

        var act = () => player.FinishWonderMaintenanceWithCash(wonder, Item("test_wonder"), Now.AddSeconds(500));

        act.Should().Throw<DomainException>().Which.Reason.Should().Be(GameErrorType.NotEnoughMoney);
    }

    [Fact]
    public void Player_FinishWonderMaintenanceWithCash_NoMaintenanceMechanic_Throws()
    {
        var faker = new Faker();
        var wonder = faker.WorldObject(itemName: "test_coupon_wonder", className: BuildingClassType.Wonder);
        var player = faker.Player(world: faker.World(objects: [wonder]));
        player.SetCash(40);

        var act = () => player.FinishWonderMaintenanceWithCash(wonder, Item("test_coupon_wonder"), Now);

        act.Should().Throw<Exception>();
        player.Cash.Should().Be(40);
    }

    [Fact]
    public void Player_UseAddToWorldHarvestLoopCoupon_ItemCoupon_IsConsumed()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.GiveCoupon("test_wonder");
        player.GiveCoupon("test_wonder", 3);

        player.UseAddToWorldHarvestLoopCoupon(Item("test_wonder"));

        player.HasCoupon("test_wonder").Should().BeFalse();
        player.Coupons.Should().ContainSingle(x => x.WorldFlatId == 3);
    }

    [Fact]
    public void Player_UseAddToWorldHarvestLoopCoupon_NoCoupon_DoesNothing()
    {
        var faker = new Faker();
        var player = faker.Player();

        player.UseAddToWorldHarvestLoopCoupon(Item("test_wonder"));

        player.Coupons.Should().BeEmpty();
    }

    [Fact]
    public void Player_UseAddToWorldHarvestLoopCoupon_NoTimerCouponOnAddToWorld_KeepsCoupon()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.GiveCoupon("test_coupon_wonder");

        player.UseAddToWorldHarvestLoopCoupon(Item("test_coupon_wonder"));

        player.HasCoupon("test_coupon_wonder").Should().BeTrue();
    }

    [Fact]
    public void Player_SpendWonderPeepCoupons_RemovesOnlyGlobalTerminalCoupons()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.GiveCoupon("test_vip_coupon_a");
        player.GiveCoupon("test_vip_coupon_b");
        player.GiveCoupon("test_vip_coupon_a", 3);
        player.GiveCoupon("test_wonder_coupon");

        player.SpendWonderPeepCoupons(Item("test_wonder_terminal"));

        player.HasCoupon("test_vip_coupon_a").Should().BeFalse();
        player.HasCoupon("test_vip_coupon_b").Should().BeFalse();
        player.HasCoupon("test_wonder_coupon").Should().BeTrue();
        player.Coupons.Should().Contain(x => x.Name == "test_vip_coupon_a" && x.WorldFlatId == 3);
    }

    [Fact]
    public void Player_SpendWonderPeepCoupons_NoHarvestLoopConfig_Throws()
    {
        var faker = new Faker();
        var player = faker.Player();

        var act = () => player.SpendWonderPeepCoupons(Item("test_vip_plane"));

        act.Should().Throw<Exception>();
    }

    private static (CityVilleDotnet.Domain.Entities.Player Player, CityVilleDotnet.Domain.Entities.World World, CityVilleDotnet.Domain.Entities.WorldObject Plane, MechanicItem Mechanic) PlantedVipPlane(Faker faker, params CityVilleDotnet.Domain.Entities.WorldObject[] others)
    {
        var plane = faker.WorldObject(itemName: "test_vip_plane", className: BuildingClassType.Airplane, state: WorldObjectState.Plowed, worldFlatId: 1);
        plane.AttachToMacroObject("test_airport_1");
        var world = faker.World(objects: [plane, ..others]);
        var player = faker.Player(world: world);
        player.SetGold(100);
        player.PlantMechanicContract(plane, Item("test_vip_plane"), Item("test_vip_plane_contract"));
        plane.SetFullGrown();
        var mechanic = Item("test_vip_plane").Mechanics!.GetMechanicByGameMode("GMPlay")!.GetMechanicItemByType("contractHarvest")!;

        return (player, world, plane, mechanic);
    }

    [Fact]
    public void Player_HarvestWonderPeepContract_FirstHarvestWithTerminal_GivesVipCouponAndMarksTerminal()
    {
        var faker = new Faker();
        var terminal = Terminal(faker);
        terminal.AttachToMacroObject("test_airport_1");
        var (player, world, plane, mechanic) = PlantedVipPlane(faker, terminal);

        var contractName = player.HarvestWonderPeepContract(world, plane, Item("test_vip_plane"), mechanic);

        contractName.Should().Be("test_vip_plane_contract");
        player.HasCoupon("test_vip_coupon_a").Should().BeTrue();
        player.Gold.Should().Be(0);
        terminal.HasMechanicCounter("firstTime").Should().BeTrue();
        plane.ContractName.Should().BeNull();
    }

    [Fact]
    public void Player_HarvestWonderPeepContract_TerminalAlreadyMarked_RollsDefaultTable()
    {
        var faker = new Faker();
        var terminal = Terminal(faker);
        terminal.AttachToMacroObject("test_airport_1");
        terminal.IncrementMechanicCounter("firstTime");
        var (player, world, plane, mechanic) = PlantedVipPlane(faker, terminal);

        player.HarvestWonderPeepContract(world, plane, Item("test_vip_plane"), mechanic);

        player.Gold.Should().Be(100);
        player.HasCoupon("test_vip_coupon_a").Should().BeFalse();
        terminal.MechanicCounters.Should().ContainSingle(x => x.MechanicType == "firstTime").Which.Count.Should().Be(1);
    }

    [Fact]
    public void Player_HarvestWonderPeepContract_NoTerminal_RollsDefaultTable()
    {
        var faker = new Faker();
        var (player, world, plane, mechanic) = PlantedVipPlane(faker);

        player.HarvestWonderPeepContract(world, plane, Item("test_vip_plane"), mechanic);

        player.Gold.Should().Be(100);
        player.HasCoupon("test_vip_coupon_a").Should().BeFalse();
    }

    [Fact]
    public void Player_HarvestWonderPeepContract_TerminalOfAnotherAirport_RollsDefaultTable()
    {
        var faker = new Faker();
        var terminal = Terminal(faker);
        terminal.AttachToMacroObject("test_airport_2");
        var (player, world, plane, mechanic) = PlantedVipPlane(faker, terminal);

        player.HarvestWonderPeepContract(world, plane, Item("test_vip_plane"), mechanic);

        player.Gold.Should().Be(100);
        terminal.HasMechanicCounter("firstTime").Should().BeFalse();
    }

    [Fact]
    public void Player_HarvestWonderPeepContract_NotGrown_Throws()
    {
        var faker = new Faker();
        var plane = faker.WorldObject(itemName: "test_vip_plane", className: BuildingClassType.Airplane, state: WorldObjectState.Plowed, worldFlatId: 1);
        var world = faker.World(objects: [plane]);
        var player = faker.Player(world: world);
        var mechanic = Item("test_vip_plane").Mechanics!.GetMechanicByGameMode("GMPlay")!.GetMechanicItemByType("contractHarvest")!;

        var act = () => player.HarvestWonderPeepContract(world, plane, Item("test_vip_plane"), mechanic);

        act.Should().Throw<Exception>();
    }
}
