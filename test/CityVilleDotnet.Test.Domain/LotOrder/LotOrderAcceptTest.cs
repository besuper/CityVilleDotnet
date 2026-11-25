using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.LotOrder;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Test.Domain.Fixtures;

namespace CityVilleDotnet.Test.Domain.LotOrder;

[Collection("Domain")]
public class LotOrderAcceptTest
{
    [Fact]
    public void LotOrder_AcceptOnLot_SetsLotIdAndAccepts()
    {
        var faker = new Faker();
        var order = faker.LotOrder(lotId: -1);

        order.AcceptOnLot(42);

        order.LotId.Should().Be(42);
        order.OrderState.Should().Be(OrderState.Accepted);
    }

    [Fact]
    public void Player_FindLotOrder_MatchingLot_ReturnsMatchingOrder()
    {
        var faker = new Faker();
        var player = faker.Player();
        var other = faker.LotOrder(lotId: 1);
        var expected = faker.LotOrder(lotId: 2);
        player.AddLotOrder(other);
        player.AddLotOrder(expected);

        player.FindLotOrder(2).Should().BeSameAs(expected);
    }

    [Fact]
    public void Player_FindLotOrder_NoMatchingLot_ReturnsFirstOrder()
    {
        var faker = new Faker();
        var player = faker.Player();
        var order = faker.LotOrder(lotId: -1);
        player.AddLotOrder(order);

        player.FindLotOrder(42).Should().BeSameAs(order);
    }

    [Fact]
    public void Player_FindLotOrder_NoOrder_ReturnsNull()
    {
        var faker = new Faker();
        var player = faker.Player();

        player.FindLotOrder(42).Should().BeNull();
    }
}
