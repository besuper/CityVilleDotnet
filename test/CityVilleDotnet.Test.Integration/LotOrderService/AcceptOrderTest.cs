using AwesomeAssertions;
using CityVilleDotnet.Api.Services.LotOrderService;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Franchise;
using CityVilleDotnet.Factory.LotOrder;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Integration.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Test.Integration.LotOrderService;

[Collection("Database")]
public class AcceptOrderTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    private const int LotSiteId = 42;
    private const string ResourceType = "test_bus";
    private const string FranchiseName = "test_franchise";

    private async Task<(Player Receiver, Player Sender)> SeedPlayers()
    {
        var lotSite = Faker.WorldObject(className: BuildingClassType.LotSite, worldFlatId: LotSiteId);
        var receiver = Faker.Player(world: Faker.World(objects: [lotSite]));
        var sender = Faker.Player();
        sender.Franchises.Add(Faker.Franchise(franchiseType: ResourceType, franchiseName: FranchiseName));

        await Context.AddRangeAsync([receiver, sender], TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        return (receiver, sender);
    }

    private async Task AddOrders(Player receiver, Player sender, int lotId)
    {
        var senderId = sender.Snuid.ToString();
        var recipientId = receiver.Snuid.ToString();

        receiver.AddLotOrder(Faker.LotOrder(senderId: senderId, recipientId: recipientId, lotId: lotId, transmissionStatus: TransmissionStatus.Received, resourceType: ResourceType, orderResourceName: FranchiseName));
        sender.AddLotOrder(Faker.LotOrder(senderId: senderId, recipientId: recipientId, lotId: lotId, transmissionStatus: TransmissionStatus.Sent, resourceType: ResourceType, orderResourceName: FranchiseName));

        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
    }

    private async Task AssertAcceptedOnLotSite(Player receiver)
    {
        var orders = await Context.Set<LotOrder>().AsNoTracking().ToListAsync(TestContext.Current.CancellationToken);
        orders.Should().HaveCount(2).And.OnlyContain(x => x.OrderState == OrderState.Accepted && x.LotId == LotSiteId);

        var building = receiver.GetWorld().Objects.Single(x => x.WorldFlatId == LotSiteId);
        building.ItemName.Should().Be(ResourceType);
        building.ClassName.Should().Be(BuildingClassType.Business);
        building.FranchiseLocation!.ObjectId.Should().Be($"{LotSiteId}");
    }

    [Fact]
    public async Task AcceptOrder_MatchingLot_ReplacesLotSiteAndAcceptsBothOrders()
    {
        var (receiver, sender) = await SeedPlayers();
        await AddOrders(receiver, sender, LotSiteId);

        var response = await new AcceptOrder(Context).HandlePacket(new AcceptOrderRequest { SenderId = sender.Snuid, LotId = LotSiteId }, receiver.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        await AssertAcceptedOnLotSite(receiver);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(28)]
    public async Task AcceptOrder_LotReassignedByClient_AcceptsOnRequestedLot(int storedLotId)
    {
        var (receiver, sender) = await SeedPlayers();
        await AddOrders(receiver, sender, storedLotId);

        var response = await new AcceptOrder(Context).HandlePacket(new AcceptOrderRequest { SenderId = sender.Snuid, LotId = LotSiteId }, receiver.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        await AssertAcceptedOnLotSite(receiver);
    }

    [Fact]
    public async Task AcceptOrder_LotReassignedByClient_SenderCanCompleteOrder()
    {
        var (receiver, sender) = await SeedPlayers();
        await AddOrders(receiver, sender, -1);
        await new AcceptOrder(Context).HandlePacket(new AcceptOrderRequest { SenderId = sender.Snuid, LotId = LotSiteId }, receiver.Id, TestContext.Current.CancellationToken);

        var request = new CompleteOrderRequest { Order = new PlaceOrderParams { RecipientId = receiver.Snuid.ToString(), LotId = LotSiteId } };
        var response = await new CompleteOrder(Context).HandlePacket(request, sender.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        (await Context.Set<LotOrder>().AnyAsync(TestContext.Current.CancellationToken)).Should().BeFalse();
    }

    [Fact]
    public async Task AcceptOrder_NoPendingReceivedOrder_Throws()
    {
        var (receiver, sender) = await SeedPlayers();

        var act = () => new AcceptOrder(Context).HandlePacket(new AcceptOrderRequest { SenderId = sender.Snuid, LotId = LotSiteId }, receiver.Id, TestContext.Current.CancellationToken);

        await act.Should().ThrowAsync<Exception>();
    }
}
