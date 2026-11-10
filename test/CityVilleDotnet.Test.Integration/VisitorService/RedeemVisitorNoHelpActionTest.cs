using AwesomeAssertions;
using CityVilleDotnet.Api.Services.VisitorService;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.VisitorHelpOrder;
using CityVilleDotnet.Test.Integration.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Test.Integration.VisitorService;

[Collection("Database")]
public class RedeemVisitorNoHelpActionTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    private const string VisitorId = "11835";
    private const string OtherVisitorId = "22946";

    [Fact]
    public async Task RedeemVisitorNoHelpAction_UnclaimedOrder_RemovesOnlyThisVisitorOrder()
    {
        var player = Faker.Player();
        player.AddVisitorHelpOrder(Faker.VisitorHelpOrder(senderId: VisitorId));
        player.AddVisitorHelpOrder(Faker.VisitorHelpOrder(senderId: OtherVisitorId));

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var response = await new RedeemVisitorNoHelpAction(Context).HandlePacket(new RedeemVisitorNoHelpActionRequest { VisitorId = VisitorId }, player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        var remaining = await Context.Set<VisitorHelpOrder>().AsNoTracking().ToListAsync(TestContext.Current.CancellationToken);
        remaining.Should().ContainSingle().Which.SenderId.Should().Be(OtherVisitorId);
    }

    [Fact]
    public async Task RedeemVisitorNoHelpAction_NoOrder_Succeeds()
    {
        var player = Faker.Player();

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var response = await new RedeemVisitorNoHelpAction(Context).HandlePacket(new RedeemVisitorNoHelpActionRequest { VisitorId = VisitorId }, player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
    }
}
