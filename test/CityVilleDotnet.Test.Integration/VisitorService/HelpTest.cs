using AwesomeAssertions;
using CityVilleDotnet.Api.Services.VisitorService;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Integration.Fixtures;
using Microsoft.Extensions.Logging.Abstractions;

namespace CityVilleDotnet.Test.Integration.VisitorService;

// HarvestableResource::onVisitorHelp: harvest gives rep + goods, water and revive give rep + coins
// TestData: friendVisitPlotRepGain=1, friendHelpDefaultGoodsReward=15, friendHelpDefaultCoinReward=25
[Collection("Database")]
public class HelpTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    private const int TargetId = 160;

    private async Task<(Player Visitor, Player Owner)> SeedFriends()
    {
        var plot = Faker.WorldObject(itemName: "plot_crop", className: BuildingClassType.Plot, contractName: "plot_strawberries", state: WorldObjectState.Planted, worldFlatId: TargetId);
        var owner = Faker.Player(world: Faker.World(objects: [plot]));
        var visitor = Faker.Player();
        visitor.SendFriendRequest(owner);

        await Context.AddRangeAsync(new[] { visitor, owner },TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        return (visitor, owner);
    }

    private static HelpRequest CreateRequest(string type, Player visitor, Player owner)
    {
        return new HelpRequest
        {
            Name = "visitorHelp",
            Type = type,
            HelpParams = new HelpParamsRequest
            {
                SenderId = visitor.Snuid.ToString(),
                RecipientId = owner.Snuid.ToString(),
                HelpTargets = [TargetId],
                TimeSent = 1792928706
            }
        };
    }

    [Theory]
    [InlineData("plotWater")]
    [InlineData("plotRevive")]
    public async Task Help_PlotWaterOrRevive_GivesReputationAndCoins(string type)
    {
        var (visitor, owner) = await SeedFriends();
        var goldBefore = visitor.Gold;
        var goodsBefore = visitor.Goods;
        var socialXpBefore = visitor.SocialXp;

        var response = await new Help(Context, NullLogger<Help>.Instance).HandlePacket(CreateRequest(type, visitor, owner), visitor.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        visitor.Gold.Should().Be(goldBefore + 25);
        visitor.Goods.Should().Be(goodsBefore);
        visitor.SocialXp.Should().Be(socialXpBefore + 1);
    }

    [Fact]
    public async Task Help_PlotHarvest_GivesReputationAndGoods()
    {
        var (visitor, owner) = await SeedFriends();
        var goldBefore = visitor.Gold;
        var goodsBefore = visitor.Goods;
        var socialXpBefore = visitor.SocialXp;

        await new Help(Context, NullLogger<Help>.Instance).HandlePacket(CreateRequest("plotHarvest", visitor, owner), visitor.Id, TestContext.Current.CancellationToken);

        visitor.Gold.Should().Be(goldBefore);
        visitor.Goods.Should().Be(goodsBefore + 15);
        visitor.SocialXp.Should().Be(socialXpBefore + 1);
    }

    [Fact]
    public async Task Help_PlotRevive_ConsumesFriendEnergyAndCreatesHelpOrders()
    {
        var (visitor, owner) = await SeedFriends();
        var friend = visitor.Friends.Single();
        var energyBefore = friend.EnergyLeft;

        await new Help(Context, NullLogger<Help>.Instance).HandlePacket(CreateRequest("plotRevive", visitor, owner), visitor.Id, TestContext.Current.CancellationToken);

        friend.EnergyLeft.Should().Be(energyBefore - 1);
        visitor.VisitorHelpOrders.Should().ContainSingle(x => x.TransmissionStatus == TransmissionStatus.Sent && x.HelpTargets.SequenceEqual(new[] { TargetId }));
        owner.VisitorHelpOrders.Should().ContainSingle(x => x.TransmissionStatus == TransmissionStatus.Received && x.HelpTargets.SequenceEqual(new[] { TargetId }));
    }

    [Fact]
    public async Task Help_NoFriendEnergyLeft_GivesNothing()
    {
        var (visitor, owner) = await SeedFriends();
        visitor.Friends.Single().EnergyLeft = 0;
        var goldBefore = visitor.Gold;

        await new Help(Context, NullLogger<Help>.Instance).HandlePacket(CreateRequest("plotRevive", visitor, owner), visitor.Id, TestContext.Current.CancellationToken);

        visitor.Gold.Should().Be(goldBefore);
        owner.VisitorHelpOrders.Should().BeEmpty();
    }
}
