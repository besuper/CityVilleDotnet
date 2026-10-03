using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Test.Domain.Fixtures;

namespace CityVilleDotnet.Test.Domain.MysteryCollection;

// IMysteryCollectionManager::grantCollectibleToken adds "{item}_token" to the inventory unless it is owned or its collection was traded in,
// test_mystery_collection: test_mystery_a_token + test_mystery_c_token, rewards 500 coins and test_mystery_reward
[Collection("Domain")]
public class PlayerGrantMysteryCollectibleTokenTest
{
    [Fact]
    public void Player_GrantMysteryCollectibleToken_AddsTokenToInventory()
    {
        var faker = new Faker();
        var player = faker.Player();

        var removed = player.GrantMysteryCollectibleToken("test_mystery_a");

        removed.Should().BeEmpty();
        player.CountInventoryItem("test_mystery_a_token").Should().Be(1);
        player.Collections.Should().BeEmpty();
    }

    [Fact]
    public void Player_GrantMysteryCollectibleToken_TokenAlreadyOwned_DoesNothing()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.AddItem("test_mystery_a_token");

        player.GrantMysteryCollectibleToken("test_mystery_a");

        player.CountInventoryItem("test_mystery_a_token").Should().Be(1);
    }

    [Fact]
    public void Player_GrantMysteryCollectibleToken_TokenWithoutCollection_OnlyAddsToken()
    {
        var faker = new Faker();
        var player = faker.Player();
        var goldBefore = player.Gold;

        var removed = player.GrantMysteryCollectibleToken("test_mystery_b");

        removed.Should().BeEmpty();
        player.CountInventoryItem("test_mystery_b_token").Should().Be(1);
        player.Gold.Should().Be(goldBefore);
        player.Collections.Should().BeEmpty();
    }

    [Fact]
    public void Player_GrantMysteryCollectibleToken_LastToken_TradesCollectionIn()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.AddItem("test_mystery_c_token");
        var goldBefore = player.Gold;

        var removed = player.GrantMysteryCollectibleToken("test_mystery_a");

        removed.Should().ContainSingle(x => x.Name == "test_mystery_c_token");
        player.HasItem("test_mystery_a_token").Should().BeFalse();
        player.HasItem("test_mystery_c_token").Should().BeFalse();
        player.CountInventoryItem("test_mystery_reward").Should().Be(1);
        player.Gold.Should().Be(goldBefore + 500);

        var collection = player.Collections.Should().ContainSingle().Subject;
        collection.Name.Should().Be("test_mystery_collection");
        collection.TradeIns.Should().Be(1);
        collection.Completed.Should().Be(0);
    }

    [Fact]
    public void Player_GrantMysteryCollectibleToken_CollectionAlreadyTradedIn_DoesNothing()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.AddItem("test_mystery_c_token");
        player.GrantMysteryCollectibleToken("test_mystery_a");
        var goldBefore = player.Gold;

        var removed = player.GrantMysteryCollectibleToken("test_mystery_a");

        removed.Should().BeEmpty();
        player.HasItem("test_mystery_a_token").Should().BeFalse();
        player.Gold.Should().Be(goldBefore);
        player.Collections.Should().ContainSingle(x => x.TradeIns == 1);
    }
}
