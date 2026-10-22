using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Domain.GameEntities;
using CityVilleDotnet.Factory.InventoryItem;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Test.Domain.Fixtures;

namespace CityVilleDotnet.Test.Domain.Inventory;

[Collection("Domain")]
public class InventoryDtoTest
{
    [Fact]
    public void Player_ToDto_SameItemInInventoryAndStorage_SendsOnlyInventoryItem()
    {
        var faker = new Faker();
        var player = faker.Player();
        var itemName = faker.Random.String2(64);
        player.InventoryItems.Add(faker.InventoryItem(itemName: itemName, amount: 2));
        player.InventoryItems.Add(faker.InventoryItem(itemName: itemName, amount: 5, storageType: faker.Random.String2(64)));

        var items = player.ToDto().UserInfo.Player.Inventory.Items;

        items.Should().ContainSingle();
        items[itemName].Should().Be(2);
    }
}
