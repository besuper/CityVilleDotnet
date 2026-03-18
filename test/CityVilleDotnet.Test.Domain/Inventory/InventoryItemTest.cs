using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Factory.InventoryItem;

namespace CityVilleDotnet.Test.Domain.Inventory;

public class InventoryItemTest
{
    [Fact]
    public void InventoryItem_Constructor_Success()
    {
        var faker = new Faker();
        var itemName = faker.Lorem.Word();

        var item = faker.InventoryItem(itemName: itemName);

        item.Name.Should().Be(itemName);
        item.Amount.Should().Be(1);
    }

    [Fact]
    public void InventoryItem_Constructor_CustomAmount()
    {
        var faker = new Faker();
        var itemName = faker.Lorem.Word();
        var amount = faker.Random.Int(2, 100);

        var item = faker.InventoryItem(itemName: itemName, amount: amount);

        item.Name.Should().Be(itemName);
        item.Amount.Should().Be(amount);
    }

    [Fact]
    public void InventoryItem_AddAmount_IncreasesAmount()
    {
        var faker = new Faker();
        var item = faker.InventoryItem(amount: 5);

        item.AddAmount(3);

        item.Amount.Should().Be(8);
    }

    [Fact]
    public void InventoryItem_RemoveAmount_DecreasesAmount()
    {
        var faker = new Faker();
        var item = faker.InventoryItem(amount: 10);

        item.RemoveAmount(4);

        item.Amount.Should().Be(6);
    }

    [Fact]
    public void InventoryItem_RemoveAmount_CanGoToZero()
    {
        var faker = new Faker();
        var item = faker.InventoryItem(amount: 5);

        item.RemoveAmount(5);

        item.Amount.Should().Be(0);
    }

    [Fact]
    public void InventoryItem_RemoveAmount_CanGoNegative()
    {
        var faker = new Faker();
        var item = faker.InventoryItem(amount: 2);

        item.RemoveAmount(5);

        item.Amount.Should().Be(-3);
    }
}
