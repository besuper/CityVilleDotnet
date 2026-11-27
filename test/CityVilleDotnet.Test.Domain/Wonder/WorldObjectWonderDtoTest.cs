using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Domain.GameEntities;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Domain.Fixtures;

namespace CityVilleDotnet.Test.Domain.Wonder;

// test_wonder: timer 180, sleep 1000
// test_wonder_terminal: coupons test_vip_coupon_a/b (b: 60s)
[Collection("Domain")]
public class WorldObjectWonderDtoTest
{
    private static readonly DateTime Now = new(2026, 1, 15, 12, 0, 0, DateTimeKind.Utc);
    private static readonly long NowTs = ServerUtils.ToUnixTimeSeconds(Now);

    [Fact]
    public void WorldObject_ToDto_StartedWonder_SendsHarvestAndSleepState()
    {
        var faker = new Faker();
        var wonder = faker.WorldObject(itemName: "test_wonder", className: BuildingClassType.Wonder);
        wonder.StartHarvestLoop(GameSettingsManager.Instance.GetItem("test_wonder")!.HarvestLoopConfig!, Now, null);

        var dto = wonder.ToDto();

        var harvestState = dto.MechanicData["harvestState"].Should().BeOfType<HarvestLoopStateDto>().Subject;
        harvestState.OpenTs.Should().Be(NowTs);
        harvestState.CloseTs.Should().Be(NowTs + 180);
        var sleepState = dto.MechanicData["sleepState"].Should().BeOfType<SleepStateDto>().Subject;
        sleepState.SleepTs.Should().Be(NowTs + 180);
        sleepState.WakeTs.Should().Be(NowTs + 1180);
        dto.MechanicData.Should().NotContainKey("harvestLoop");
    }

    [Fact]
    public void WorldObject_ToDto_NeverStartedWonder_SendsNoHarvestLoopData()
    {
        var faker = new Faker();
        var wonder = faker.WorldObject(itemName: "test_wonder", className: BuildingClassType.Wonder);

        var dto = wonder.ToDto();

        dto.MechanicData.Should().NotContainKey("harvestState");
        dto.MechanicData.Should().NotContainKey("sleepState");
    }

    [Fact]
    public void WorldObject_ToDto_StartedTerminal_SendsHarvestLoopAndAppliedCoupon()
    {
        var faker = new Faker();
        var terminal = faker.WorldObject(itemName: "test_wonder_terminal", className: BuildingClassType.TimedHarvestableResource);
        terminal.StartHarvestLoop(GameSettingsManager.Instance.GetItem("test_wonder_terminal")!.HarvestLoopConfig!, Now, "test_vip_coupon_b");

        var dto = terminal.ToDto();

        dto.MechanicData["harvestLoop"].Should().BeOfType<HarvestLoopDataDto>().Which.StartTs.Should().Be(NowTs);
        dto.MechanicData["appliedCoupon"].Should().Be("test_vip_coupon_b");
        dto.MechanicData.Should().NotContainKey("harvestState");
        dto.MechanicData.Should().NotContainKey("sleepState");
    }

    [Fact]
    public void WorldObject_ToDto_NeverStartedTerminal_SendsNoHarvestLoop()
    {
        var faker = new Faker();
        var terminal = faker.WorldObject(itemName: "test_wonder_terminal", className: BuildingClassType.TimedHarvestableResource);

        var dto = terminal.ToDto();

        dto.MechanicData.Should().NotContainKey("harvestLoop");
        dto.MechanicData.Should().NotContainKey("appliedCoupon");
    }

    [Fact]
    public void WorldObject_ToDto_TerminalWithHeldVipCoupons_SendsOnePeepPerCoupon()
    {
        var faker = new Faker();
        var terminal = faker.WorldObject(itemName: "test_wonder_terminal", className: BuildingClassType.TimedHarvestableResource);

        var dto = terminal.ToDto(coupons: ["test_vip_coupon_b", "test_wonder_coupon", "test_vip_coupon_a"]);

        dto.MechanicData["peeps"].Should().BeAssignableTo<List<WonderPeepDto>>()
            .Which.Select(x => x.Coupon).Should().BeEquivalentTo("test_vip_coupon_a", "test_vip_coupon_b");
    }

    [Fact]
    public void WorldObject_ToDto_TerminalWithoutVipCoupon_SendsNoPeeps()
    {
        var faker = new Faker();
        var terminal = faker.WorldObject(itemName: "test_wonder_terminal", className: BuildingClassType.TimedHarvestableResource);

        var dto = terminal.ToDto(coupons: ["test_wonder_coupon"]);

        dto.MechanicData.Should().NotContainKey("peeps");
    }

    [Fact]
    public void WorldObject_ToDto_WonderWithVipCoupons_SendsNoPeeps()
    {
        var faker = new Faker();
        var wonder = faker.WorldObject(itemName: "test_wonder", className: BuildingClassType.Wonder);

        var dto = wonder.ToDto(coupons: ["test_vip_coupon_a"]);

        dto.MechanicData.Should().NotContainKey("peeps");
    }
}
