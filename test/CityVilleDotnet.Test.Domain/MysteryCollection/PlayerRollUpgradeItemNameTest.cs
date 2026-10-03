using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Domain.Fixtures;

namespace CityVilleDotnet.Test.Domain.MysteryCollection;

// MapResource::getUpgradeItemName rolls SecureRand.randPerFeature(0, 1000, "lootTables", ...) once per building
// test_mystery_upgrade_1: test_mystery_a=10, test_mystery_b=90 / test_mystery_upgrade_2: test_mystery_c=100
// With uid 333, the client rolls 102 then 116 so a weighted roll of 10 (a) then 11 (b)
[Collection("Domain")]
public class PlayerRollUpgradeItemNameTest
{
    [Fact]
    public void Player_RollUpgradeItemName_MatchesClientRollsAndIncrementsFeatureCounter()
    {
        var faker = new Faker();
        var firstCrate = faker.WorldObject(itemName: "test_mystery_crate", className: BuildingClassType.Decoration);
        var secondCrate = faker.WorldObject(itemName: "test_mystery_crate", className: BuildingClassType.Decoration);
        var player = faker.Player(world: faker.World(objects: [firstCrate, secondCrate]));
        player.Snuid = 333;

        player.RollUpgradeItemName(firstCrate).Should().Be("test_mystery_a");
        player.RollUpgradeItemName(secondCrate).Should().Be("test_mystery_b");

        firstCrate.UpgradeItemName.Should().Be("test_mystery_a");
        secondCrate.UpgradeItemName.Should().Be("test_mystery_b");
        player.FeatureRollCounters.Should().ContainSingle(x => x.Feature == "lootTables" && x.Count == 2);
    }

    [Fact]
    public void Player_RollUpgradeItemName_AlreadyRolled_KeepsNameWithoutRolling()
    {
        var faker = new Faker();
        var crate = faker.WorldObject(itemName: "test_mystery_crate", className: BuildingClassType.Decoration);
        var player = faker.Player(world: faker.World(objects: [crate]));
        player.Snuid = 333;

        var firstRoll = player.RollUpgradeItemName(crate);
        var secondRoll = player.RollUpgradeItemName(crate);

        secondRoll.Should().Be(firstRoll);
        player.FeatureRollCounters.Should().ContainSingle(x => x.Feature == "lootTables" && x.Count == 1);
    }

    [Fact]
    public void Player_RollUpgradeItemName_UsesHeadquartersGroupTable()
    {
        var faker = new Faker();
        var crate = faker.WorldObject(itemName: "test_mystery_crate", className: BuildingClassType.Decoration);
        var headquarters = faker.WorldObject(itemName: "mun_national_park_hq_2", className: BuildingClassType.Municipal);
        var player = faker.Player(world: faker.World(objects: [crate, headquarters]));

        player.RollUpgradeItemName(crate).Should().Be("test_mystery_c");
    }

    [Fact]
    public void Player_RollUpgradeItemName_MissingGroupTable_ThrowsInvalidData()
    {
        var faker = new Faker();
        var crate = faker.WorldObject(itemName: "test_mystery_crate", className: BuildingClassType.Decoration);
        var headquarters = faker.WorldObject(itemName: "mun_national_park_hq_3", className: BuildingClassType.Municipal);
        var player = faker.Player(world: faker.World(objects: [crate, headquarters]));

        var act = () => player.RollUpgradeItemName(crate);

        act.Should().Throw<DomainException>().Which.Reason.Should().Be(GameErrorType.InvalidData);
        crate.UpgradeItemName.Should().BeNull();
    }

    [Fact]
    public void Player_RollUpgradeItemName_NoMysteryCollectionUpgrade_ThrowsInvalidState()
    {
        var faker = new Faker();
        var business = faker.WorldObject(itemName: "test_bus_upgradable", className: BuildingClassType.Business);
        var player = faker.Player(world: faker.World(objects: [business]));

        var act = () => player.RollUpgradeItemName(business);

        act.Should().Throw<DomainException>().Which.Reason.Should().Be(GameErrorType.InvalidState);
        player.FeatureRollCounters.Should().BeEmpty();
    }

    [Fact]
    public void WorldObject_UpgradeBuilding_ClearsUpgradeItemName()
    {
        var faker = new Faker();
        var crate = faker.WorldObject(itemName: "test_mystery_crate", className: BuildingClassType.Decoration);
        crate.SetUpgradeItemName("test_mystery_a");

        crate.UpgradeBuilding(GameSettingsManager.Instance.GetItem("test_mystery_crate")!, "test_mystery_a");

        crate.ItemName.Should().Be("test_mystery_a");
        crate.UpgradeItemName.Should().BeNull();
    }
}
