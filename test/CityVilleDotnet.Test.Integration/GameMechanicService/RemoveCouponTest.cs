using AwesomeAssertions;
using CityVilleDotnet.Api.Services.GameMechanicService;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Integration.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Test.Integration.GameMechanicService;

// test_wonder_terminal: VIP coupons test_vip_coupon_a/b
[Collection("Database")]
public class RemoveCouponTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    [Fact]
    public async Task RemoveCoupon_Terminal_SpendsHeldVipCouponsOnly()
    {
        var terminal = Faker.WorldObject(itemName: "test_wonder_terminal", className: BuildingClassType.TimedHarvestableResource, worldFlatId: 42);
        var player = Faker.Player(world: Faker.World(objects: [terminal]));
        player.GiveCoupon("test_vip_coupon_a");
        player.GiveCoupon("test_vip_coupon_b");
        player.GiveCoupon("test_wonder_coupon");
        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        Context.ChangeTracker.Clear();

        var response = await new RemoveCoupon(Context).HandlePacket(new RemoveCouponRequest { ObjectId = 42 }, player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        var storedPlayer = await Context.Set<Player>().AsNoTracking().Include(x => x.Coupons).FirstAsync(x => x.Id == player.Id, TestContext.Current.CancellationToken);
        storedPlayer.Coupons.Should().ContainSingle().Which.Name.Should().Be("test_wonder_coupon");
    }

    [Fact]
    public async Task RemoveCoupon_NoVipCoupon_DoesNothing()
    {
        var terminal = Faker.WorldObject(itemName: "test_wonder_terminal", className: BuildingClassType.TimedHarvestableResource, worldFlatId: 42);
        var player = Faker.Player(world: Faker.World(objects: [terminal]));
        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        Context.ChangeTracker.Clear();

        var response = await new RemoveCoupon(Context).HandlePacket(new RemoveCouponRequest { ObjectId = 42 }, player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
    }

    [Fact]
    public async Task RemoveCoupon_UnknownObject_Throws()
    {
        var player = Faker.Player();
        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        Context.ChangeTracker.Clear();

        var act = () => new RemoveCoupon(Context).HandlePacket(new RemoveCouponRequest { ObjectId = 42 }, player.Id, TestContext.Current.CancellationToken);

        await act.Should().ThrowAsync<Exception>();
    }
}
