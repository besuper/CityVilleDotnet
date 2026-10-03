using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Domain.GameEntities;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Domain.Fixtures;

namespace CityVilleDotnet.Test.Domain.MysteryCollection;

[Collection("Domain")]
public class MysteryCollectionDtoTest
{
    [Fact]
    public void Player_ToDto_NoRolls_SendsEmptyRollCounterMap()
    {
        var faker = new Faker();
        var player = faker.Player();

        var dto = player.ToDto().UserInfo.Player;

        dto.RollCounterMap.Should().NotBeNull().And.BeEmpty();
        dto.CollectionTradeIns.Should().NotBeNull().And.BeEmpty();
    }

    [Fact]
    public void Player_ToDto_SendsRollCounterMapAndCollectionTradeIns()
    {
        var faker = new Faker();
        var crate = faker.WorldObject(itemName: "test_mystery_crate", className: BuildingClassType.Decoration);
        var player = faker.Player(world: faker.World(objects: [crate]));
        player.AddItem("test_mystery_c_token");

        player.RollUpgradeItemName(crate);
        player.GrantMysteryCollectibleToken("test_mystery_a");

        var dto = player.ToDto().UserInfo.Player;

        dto.RollCounterMap["lootTables"].Should().Be(1);
        dto.CollectionTradeIns["test_mystery_collection"].Should().Be(1);
        dto.CompletedCollections.Should().NotContainKey("test_mystery_collection");
    }

    [Fact]
    public void WorldObject_ToDto_SendsUpgradeItemName()
    {
        var faker = new Faker();
        var crate = faker.WorldObject(itemName: "test_mystery_crate", className: BuildingClassType.Decoration);
        crate.SetUpgradeItemName("test_mystery_b");

        crate.ToDto().UpgradeItemName.Should().Be("test_mystery_b");
    }
}
