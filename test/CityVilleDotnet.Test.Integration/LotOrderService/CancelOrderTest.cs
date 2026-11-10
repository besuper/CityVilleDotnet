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
public class CancelOrderTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    private const int LotId = 42;
    private const string ResourceType = "test_lot_coins";

    private async Task<(Player Sender, Player Recipient)> SeedPlayers()
    {
        var sender = Faker.Player();
        var recipient = Faker.Player();
        sender.SetGold(1000);

        await Context.AddRangeAsync([sender, recipient], TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        return (sender, recipient);
    }

    private async Task AddOrders(Player sender, Player recipient, int lotId, bool withReceivedOrder = true)
    {
        var senderId = sender.Snuid.ToString();
        var recipientId = recipient.Snuid.ToString();

        sender.AddLotOrder(Faker.LotOrder(senderId: senderId, recipientId: recipientId, lotId: lotId, transmissionStatus: TransmissionStatus.Sent, resourceType: ResourceType));

        if (withReceivedOrder)
        {
            recipient.AddLotOrder(Faker.LotOrder(senderId: senderId, recipientId: recipientId, lotId: lotId, transmissionStatus: TransmissionStatus.Received, resourceType: ResourceType));
        }

        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
    }

    private static CancelOrderRequest CreateRequest(Player recipient, int lotId)
    {
        return new CancelOrderRequest { Order = new PlaceOrderParams { RecipientId = recipient.Snuid.ToString(), LotId = lotId } };
    }

    [Fact]
    public async Task CancelOrder_PendingOrder_RemovesBothOrdersAndRefundsCoins()
    {
        var (sender, recipient) = await SeedPlayers();
        await AddOrders(sender, recipient, LotId);
        await AddOrders(sender, recipient, LotId + 1);

        var response = await new CancelOrder(Context).HandlePacket(CreateRequest(recipient, LotId), sender.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        sender.Gold.Should().Be(1200);
        var remaining = await Context.Set<LotOrder>().AsNoTracking().ToListAsync(TestContext.Current.CancellationToken);
        remaining.Should().HaveCount(2).And.OnlyContain(x => x.LotId == LotId + 1);
    }

    [Fact]
    public async Task CancelOrder_ReceivedOrderMissing_RemovesSentOrder()
    {
        var (sender, recipient) = await SeedPlayers();
        await AddOrders(sender, recipient, LotId, withReceivedOrder: false);

        var response = await new CancelOrder(Context).HandlePacket(CreateRequest(recipient, LotId), sender.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        (await Context.Set<LotOrder>().AnyAsync(TestContext.Current.CancellationToken)).Should().BeFalse();
    }

    [Fact]
    public async Task CancelOrder_NoPendingSentOrder_ThrowsInvalidState()
    {
        var (sender, recipient) = await SeedPlayers();

        var act = () => new CancelOrder(Context).HandlePacket(CreateRequest(recipient, LotId), sender.Id, TestContext.Current.CancellationToken);

        (await act.Should().ThrowAsync<DomainException>()).Which.Reason.Should().Be(GameErrorType.InvalidState);
        sender.Gold.Should().Be(1000);
    }
}
