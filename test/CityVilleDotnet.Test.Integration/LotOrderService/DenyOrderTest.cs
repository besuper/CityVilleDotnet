using AwesomeAssertions;
using CityVilleDotnet.Api.Services.LotOrderService;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.LotOrder;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Test.Integration.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Test.Integration.LotOrderService;

[Collection("Database")]
public class DenyOrderTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    private const int LotId = 42;

    private async Task<(Player Receiver, Player Sender)> SeedPlayers()
    {
        var receiver = Faker.Player();
        var sender = Faker.Player();

        await Context.AddRangeAsync([receiver, sender], TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        return (receiver, sender);
    }

    private async Task AddOrders(Player receiver, Player sender, int lotId, bool withSentOrder = true)
    {
        var senderId = sender.Snuid.ToString();
        var recipientId = receiver.Snuid.ToString();

        receiver.AddLotOrder(Faker.LotOrder(senderId: senderId, recipientId: recipientId, lotId: lotId, transmissionStatus: TransmissionStatus.Received));

        if (withSentOrder)
        {
            sender.AddLotOrder(Faker.LotOrder(senderId: senderId, recipientId: recipientId, lotId: lotId, transmissionStatus: TransmissionStatus.Sent));
        }

        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
    }

    private static DenyOrderRequest CreateRequest(Player sender, int lotId)
    {
        return new DenyOrderRequest { Order = new PlaceOrderParams { SenderId = sender.Snuid.ToString(), LotId = lotId } };
    }

    [Fact]
    public async Task DenyOrder_PendingOrder_RemovesReceivedAndSentOrders()
    {
        var (receiver, sender) = await SeedPlayers();
        await AddOrders(receiver, sender, LotId);
        await AddOrders(receiver, sender, LotId + 1);

        var response = await new DenyOrder(Context).HandlePacket(CreateRequest(sender, LotId), receiver.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        var remaining = await Context.Set<LotOrder>().AsNoTracking().ToListAsync(TestContext.Current.CancellationToken);
        remaining.Should().HaveCount(2).And.OnlyContain(x => x.LotId == LotId + 1);
    }

    [Fact]
    public async Task DenyOrder_SentOrderMissing_RemovesReceivedOrder()
    {
        var (receiver, sender) = await SeedPlayers();
        await AddOrders(receiver, sender, LotId, withSentOrder: false);

        var response = await new DenyOrder(Context).HandlePacket(CreateRequest(sender, LotId), receiver.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        (await Context.Set<LotOrder>().AnyAsync(TestContext.Current.CancellationToken)).Should().BeFalse();
    }

    [Fact]
    public async Task DenyOrder_NoPendingReceivedOrder_ThrowsInvalidState()
    {
        var (receiver, sender) = await SeedPlayers();

        var act = () => new DenyOrder(Context).HandlePacket(CreateRequest(sender, LotId), receiver.Id, TestContext.Current.CancellationToken);

        (await act.Should().ThrowAsync<DomainException>()).Which.Reason.Should().Be(GameErrorType.InvalidState);
    }
}
