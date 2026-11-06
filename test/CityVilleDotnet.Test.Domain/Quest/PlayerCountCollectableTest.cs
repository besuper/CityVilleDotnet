using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Factory.Player;

namespace CityVilleDotnet.Test.Domain.Quest;

[Collection("Domain")]
public class PlayerCountCollectableTest
{
    [Fact]
    public void Player_CountCollectableByName_SumsAmounts()
    {
        var faker = new Faker();
        var player = faker.Player();
        var itemName = faker.Random.String2(64);
        player.AddItemToCollection(faker.Random.String2(64), itemName, 2);

        player.CountCollectableByName(itemName).Should().Be(2);
    }

    [Fact]
    public void Player_CountCollectableByName_SumsAcrossCollections()
    {
        var faker = new Faker();
        var player = faker.Player();
        var itemName = faker.Random.String2(64);
        player.AddItemToCollection(faker.Random.String2(64), itemName, 2);
        player.AddItemToCollection(faker.Random.String2(64), itemName, 3);

        player.CountCollectableByName(itemName).Should().Be(5);
    }

    [Fact]
    public void Player_CountCollectableByName_UnknownItem_ReturnsZero()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.AddItemToCollection(faker.Random.String2(64), faker.Random.String2(64), 2);

        player.CountCollectableByName(faker.Random.String2(64)).Should().Be(0);
    }
}
