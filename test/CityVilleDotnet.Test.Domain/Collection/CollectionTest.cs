using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Factory.Collection;

namespace CityVilleDotnet.Test.Domain.Collection;

public class CollectionTest
{
    [Fact]
    public void Collection_AddItem_Success()
    {
        var faker = new Faker();
        var collection = faker.Collection();

        collection.Items.Count.Should().Be(0);

        collection.AddItem(faker.Lorem.Word());

        collection.Items.Count.Should().Be(1);
    }

    [Fact]
    public void Collection_AddItem_ExistingItem_IncrementsAmount()
    {
        var faker = new Faker();
        var collection = faker.Collection();
        var itemName = faker.Lorem.Word();

        collection.AddItem(itemName);
        collection.AddItem(itemName);

        collection.Items.Count.Should().Be(1);
        collection.Items.First().Amount.Should().Be(2);
    }

    [Fact]
    public void Collection_AddItem_WithAmount_SetsCorrectAmount()
    {
        var faker = new Faker();
        var collection = faker.Collection();
        var itemName = faker.Lorem.Word();

        collection.AddItem(itemName, 5);

        collection.Items.Count.Should().Be(1);
        collection.Items.First().Amount.Should().Be(5);
    }

    [Fact]
    public void Collection_AddItem_MultipleDistinctItems()
    {
        var faker = new Faker();
        var collection = faker.Collection();

        collection.AddItem(faker.Random.String2(64));
        collection.AddItem(faker.Random.String2(64));
        collection.AddItem(faker.Random.String2(64));

        collection.Items.Count.Should().Be(3);
    }

    [Fact]
    public void Collection_RemoveItem_Success()
    {
        var faker = new Faker();
        var collection = faker.Collection();
        var itemName = faker.Lorem.Word();

        collection.AddItem(itemName);

        collection.Items.Count.Should().Be(1);

        collection.RemoveItem(itemName);

        collection.Items.Count.Should().Be(0);
    }

    [Fact]
    public void Collection_RemoveItem_PartialAmount_KeepsItem()
    {
        var faker = new Faker();
        var collection = faker.Collection();
        var itemName = faker.Lorem.Word();

        collection.AddItem(itemName, 3);

        var result = collection.RemoveItem(itemName);

        result.Should().BeNull();
        collection.Items.Count.Should().Be(1);
        collection.Items.First().Amount.Should().Be(2);
    }

    [Fact]
    public void Collection_RemoveItem_AllAmount_ReturnsRemovedItem()
    {
        var faker = new Faker();
        var collection = faker.Collection();
        var itemName = faker.Lorem.Word();

        collection.AddItem(itemName, 2);

        var result = collection.RemoveItem(itemName, 2);

        result.Should().NotBeNull();
        result!.Name.Should().Be(itemName);
        collection.Items.Count.Should().Be(0);
    }

    [Fact]
    public void Collection_RemoveItem_NotFound_ThrowsException()
    {
        var faker = new Faker();
        var collection = faker.Collection();

        var act = () => collection.RemoveItem(faker.Lorem.Word());

        act.Should().Throw<Exception>().WithMessage("Item not found in collection*");
    }

    [Fact]
    public void Collection_RemoveItem_NotEnoughItems_ThrowsException()
    {
        var faker = new Faker();
        var collection = faker.Collection();
        var itemName = faker.Lorem.Word();

        collection.AddItem(itemName, 1);

        var act = () => collection.RemoveItem(itemName, 5);

        act.Should().Throw<Exception>().WithMessage("Not enough items");
    }

    [Fact]
    public void Collection_Complete_IncrementsCompleted()
    {
        var faker = new Faker();
        var collection = faker.Collection();

        collection.Completed.Should().Be(0);

        collection.Complete();

        collection.Completed.Should().Be(1);
    }

    [Fact]
    public void Collection_Complete_MultipleTimes_IncrementsEachTime()
    {
        var faker = new Faker();
        var collection = faker.Collection();

        collection.Complete();
        collection.Complete();
        collection.Complete();

        collection.Completed.Should().Be(3);
    }
}