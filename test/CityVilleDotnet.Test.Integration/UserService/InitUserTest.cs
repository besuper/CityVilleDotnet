using AwesomeAssertions;
using CityVilleDotnet.Api.Services.UserService;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Integration.Fixtures;
using Microsoft.Extensions.Configuration;

namespace CityVilleDotnet.Test.Integration.UserService;

// See <grantItems> and test_grant_pad (upgrades to test_grant_pad_2) in TestData/gameSettings.xml
[Collection("Database")]
public class InitUserTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    [Fact]
    public async Task InitUser_EligibleGrant_AddsItemToInventory()
    {
        var player = Faker.Player();
        player.SetLevel(10);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new InitUser(Context, new ConfigurationBuilder().Build());

        var response = await handler.HandlePacket([], player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        player.CountInventoryItem("test_grant_pad").Should().Be(1);
        player.Flags.Should().Contain(x => x.Name == "grant_test_grant_level");
    }

    [Fact]
    public async Task InitUser_UpgradeOwnedInOtherWorld_SkipsGrant()
    {
        var player = Faker.Player();
        player.SetLevel(10);
        player.AddWorld(Faker.World(type: WorldType.Downtown, objects: [Faker.WorldObject(itemName: "test_grant_pad_2", className: BuildingClassType.Decoration)]));

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new InitUser(Context, new ConfigurationBuilder().Build());

        await handler.HandlePacket([], player.Id, TestContext.Current.CancellationToken);

        player.CountInventoryItem("test_grant_pad").Should().Be(0);
        player.Flags.Should().Contain(x => x.Name == "grant_test_grant_level");
    }
}
