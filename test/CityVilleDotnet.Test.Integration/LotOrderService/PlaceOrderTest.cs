using AwesomeAssertions;
using CityVilleDotnet.Api.Services.LotOrderService;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Test.Integration.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Test.Integration.LotOrderService;

[Collection("Database")]
public class PlaceOrderTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    private const int LotId = 42;

    private async Task<(Player Sender, Player Recipient)> SeedPlayers(int gold)
    {
        var sender = Faker.Player();
        var recipient = Faker.Player();
        sender.SetGold(gold);

        await Context.AddRangeAsync([sender, recipient], TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        return (sender, recipient);
    }

    private static PlaceOrderRequest CreateRequest(Player sender, Player recipient)
    {
        return new PlaceOrderRequest
        {
            Order = new PlaceOrderParams
            {
                SenderId = sender.Snuid.ToString(),
                RecipientId = recipient.Snuid.ToString(),
                LotId = LotId,
                ResourceType = "test_lot_coins",
                OrderResourceName = "test_franchise"
            }
        };
    }

    [Fact]
    public async Task PlaceOrder_EnoughCoins_CreatesOrdersAndRemovesCoins()
    {
        var (sender, recipient) = await SeedPlayers(1000);

        var response = await new PlaceOrder(Context).HandlePacket(CreateRequest(sender, recipient), sender.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        sender.Gold.Should().Be(800);
        var orders = await Context.Set<LotOrder>().AsNoTracking().ToListAsync(TestContext.Current.CancellationToken);
        orders.Should().HaveCount(2).And.OnlyContain(x => x.LotId == LotId && x.OrderState == OrderState.Pending);
        orders.Select(x => x.TransmissionStatus).Should().BeEquivalentTo([TransmissionStatus.Sent, TransmissionStatus.Received]);
    }

    [Fact]
    public async Task PlaceOrder_NotEnoughCoins_ThrowsNotEnoughMoney()
    {
        var (sender, recipient) = await SeedPlayers(100);

        var act = () => new PlaceOrder(Context).HandlePacket(CreateRequest(sender, recipient), sender.Id, TestContext.Current.CancellationToken);

        (await act.Should().ThrowAsync<DomainException>()).Which.Reason.Should().Be(GameErrorType.NotEnoughMoney);
        (await Context.Set<LotOrder>().AnyAsync(TestContext.Current.CancellationToken)).Should().BeFalse();
    }
}
