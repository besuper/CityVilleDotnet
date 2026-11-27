using AwesomeAssertions;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Test.Domain.Fixtures;

namespace CityVilleDotnet.Test.Domain.Wonder;

// test_wonder: timer 180, no coupon
// test_wonder_terminal: timer 180, coupons test_vip_coupon_a/b, b overrides the timer to 60
[Collection("Domain")]
public class HarvestLoopConfigTest
{
    [Fact]
    public void HarvestLoopConfig_GetTimerDuration_NoAppliedCoupon_ReturnsTimerDuration()
    {
        var config = GameSettingsManager.Instance.GetItem("test_wonder_terminal")!.HarvestLoopConfig!;

        config.GetTimerDuration(null).Should().Be(180);
    }

    [Fact]
    public void HarvestLoopConfig_GetTimerDuration_OverriddenCoupon_ReturnsOverride()
    {
        var config = GameSettingsManager.Instance.GetItem("test_wonder_terminal")!.HarvestLoopConfig!;

        config.GetTimerDuration("test_vip_coupon_b").Should().Be(60);
    }

    [Fact]
    public void HarvestLoopConfig_GetTimerDuration_CouponWithoutOverride_ReturnsTimerDuration()
    {
        var config = GameSettingsManager.Instance.GetItem("test_wonder_terminal")!.HarvestLoopConfig!;

        config.GetTimerDuration("test_vip_coupon_a").Should().Be(180);
    }

    [Fact]
    public void HarvestLoopConfig_GetTimerDuration_NoOverrideDuration_ReturnsTimerDuration()
    {
        var config = GameSettingsManager.Instance.GetItem("test_wonder")!.HarvestLoopConfig!;

        config.GetTimerDuration("test_vip_coupon_b").Should().Be(180);
    }

    [Fact]
    public void HarvestLoopConfig_GetRequiredCoupons_CommaList_ReturnsEachCoupon()
    {
        var config = GameSettingsManager.Instance.GetItem("test_wonder_terminal")!.HarvestLoopConfig!;

        config.GetRequiredCoupons().Should().Equal("test_vip_coupon_a", "test_vip_coupon_b");
    }

    [Fact]
    public void HarvestLoopConfig_GetRequiredCoupons_NoCoupon_ReturnsEmpty()
    {
        var config = GameSettingsManager.Instance.GetItem("test_wonder")!.HarvestLoopConfig!;

        config.GetRequiredCoupons().Should().BeEmpty();
    }

    [Fact]
    public void HarvestLoopConfig_Parse_ReadsAllFields()
    {
        var config = GameSettingsManager.Instance.GetItem("test_wonder")!.HarvestLoopConfig!;

        config.TimerDuration.Should().Be(180);
        config.SleepDuration.Should().Be(1000);
        config.LockOnKeyword.Should().Be("test_wonder_lock");
        config.Cash.Should().Be(10);
        config.EnergyGain.Should().Be(10);
        config.BoostSeconds.Should().Be(30);
    }
}
