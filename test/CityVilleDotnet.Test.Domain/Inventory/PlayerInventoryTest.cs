using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Test.Domain.Fixtures;

namespace CityVilleDotnet.Test.Domain.Inventory;

[Collection("Domain")]
public class PlayerInventoryTest(DomainFixture fixture)
{
    [Fact]
    public void Player_AddItem_Success()
    {
        var faker = new Faker();
        var player = faker.Player();
        var itemName = faker.Lorem.Word();

        player.AddItem(itemName);

        player.InventoryItems.Count.Should().Be(1);
        player.InventoryItems.First().Name.Should().Be(itemName);
        player.InventoryItems.First().Amount.Should().Be(1);
    }

    [Fact]
    public void Player_AddItem_ExistingItem_IncrementsAmount()
    {
        var faker = new Faker();
        var player = faker.Player();
        var itemName = faker.Lorem.Word();

        player.AddItem(itemName);
        player.AddItem(itemName);

        player.InventoryItems.Count.Should().Be(1);
        player.InventoryItems.First().Amount.Should().Be(2);
    }

    [Fact]
    public void Player_AddItem_WithAmount_SetsCorrectAmount()
    {
        var faker = new Faker();
        var player = faker.Player();
        var itemName = faker.Lorem.Word();

        player.AddItem(itemName, 5);

        player.InventoryItems.Count.Should().Be(1);
        player.InventoryItems.First().Amount.Should().Be(5);
    }

    [Fact]
    public void Player_AddItem_MultipleDistinctItems()
    {
        var faker = new Faker();
        var player = faker.Player();

        player.AddItem(faker.Random.String2(10));
        player.AddItem(faker.Random.String2(10));
        player.AddItem(faker.Random.String2(10));

        player.InventoryItems.Count.Should().Be(3);
    }

    [Fact]
    public void Player_RemoveItem_Success()
    {
        var faker = new Faker();
        var player = faker.Player();
        var itemName = faker.Lorem.Word();

        player.AddItem(itemName);

        var result = player.RemoveItem(itemName);

        result.Should().NotBeNull();
        player.InventoryItems.Count.Should().Be(0);
    }

    [Fact]
    public void Player_RemoveItem_PartialAmount_KeepsItem()
    {
        var faker = new Faker();
        var player = faker.Player();
        var itemName = faker.Lorem.Word();

        player.AddItem(itemName, 3);

        var result = player.RemoveItem(itemName);

        result.Should().BeNull();
        player.InventoryItems.Count.Should().Be(1);
        player.InventoryItems.First().Amount.Should().Be(2);
    }

    [Fact]
    public void Player_RemoveItem_AllAmount_ReturnsRemovedItem()
    {
        var faker = new Faker();
        var player = faker.Player();
        var itemName = faker.Lorem.Word();

        player.AddItem(itemName, 2);

        var result = player.RemoveItem(itemName, 2);

        result.Should().NotBeNull();
        result!.Name.Should().Be(itemName);
        player.InventoryItems.Count.Should().Be(0);
    }

    [Fact]
    public void Player_RemoveItem_NotFound_ThrowsException()
    {
        var faker = new Faker();
        var player = faker.Player();

        var act = () => player.RemoveItem(faker.Lorem.Word());

        act.Should().Throw<Exception>().WithMessage("Item not found in player inventory*");
    }

    [Fact]
    public void Player_RemoveItem_NotEnoughItems_ThrowsException()
    {
        var faker = new Faker();
        var player = faker.Player();
        var itemName = faker.Lorem.Word();

        player.AddItem(itemName, 1);

        var act = () => player.RemoveItem(itemName, 5);

        act.Should().Throw<Exception>().WithMessage("Not enough items " + itemName);
    }

    [Fact]
    public void Player_CountInventoryItems_ReturnsTotal()
    {
        var faker = new Faker();
        var player = faker.Player();

        player.AddItem(faker.Random.String2(10), 3);
        player.AddItem(faker.Random.String2(10), 2);

        player.CountInventoryItems().Should().Be(5);
    }

    [Fact]
    public void Player_CountInventoryItem_ReturnsAmountForSpecificItem()
    {
        var faker = new Faker();
        var player = faker.Player();
        var itemName = faker.Random.String2(10);

        player.AddItem(itemName, 7);
        player.AddItem(faker.Random.String2(10), 3);

        player.CountInventoryItem(itemName).Should().Be(7);
    }

    [Fact]
    public void Player_HasItem_ReturnsTrue_WhenItemExists()
    {
        var faker = new Faker();
        var player = faker.Player();
        var itemName = faker.Lorem.Word();

        player.AddItem(itemName);

        player.HasItem(itemName).Should().BeTrue();
    }

    [Fact]
    public void Player_HasItem_ReturnsFalse_WhenItemNotExists()
    {
        var faker = new Faker();
        var player = faker.Player();

        player.HasItem(faker.Lorem.Word()).Should().BeFalse();
    }
}
