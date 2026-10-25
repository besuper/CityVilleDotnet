using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Test.Domain.Fixtures;

namespace CityVilleDotnet.Test.Domain.Coupon;

[Collection("Domain")]
public class PlayerCouponTest
{
    [Fact]
    public void Player_HasCoupon_GlobalCoupon_ReturnsTrue()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.GiveCoupon("test_coupon");

        player.HasCoupon("test_coupon").Should().BeTrue();
    }

    [Fact]
    public void Player_HasCoupon_BuildingCoupon_ReturnsFalse()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.GiveCoupon("test_coupon", 3);

        player.HasCoupon("test_coupon").Should().BeFalse();
    }

    [Fact]
    public void Player_HasCoupon_NoCoupon_ReturnsFalse()
    {
        var faker = new Faker();
        var player = faker.Player();

        player.HasCoupon("test_coupon").Should().BeFalse();
    }

    // test_drawbridge_construction rolls test_bridge_coupon_table: a not_spawnable coupon at 100%
    [Fact]
    public void Player_CollectDoobersRewards_NotSpawnableCoupon_GivesCoupon()
    {
        var faker = new Faker();
        var player = faker.Player();

        player.CollectDoobersRewards("test_drawbridge_construction");

        player.HasCoupon("test_bridge_coupon").Should().BeTrue();
        player.InventoryItems.Should().BeEmpty();
    }
}
