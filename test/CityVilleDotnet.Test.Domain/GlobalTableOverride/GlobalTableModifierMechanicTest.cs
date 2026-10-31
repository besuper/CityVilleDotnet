using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Common.Global;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Domain.Fixtures;

namespace CityVilleDotnet.Test.Domain.GlobalTableOverride;

// test_custom_table_provider: custom mechanic type, adds test_global_a_table on Business (load, onAddToWorld), removes it on MacroObjectSell, sellObject and onPreUpgrade
// test_toggled_table_provider: load adds test_toggled_load_table, test_toggled_add_table and test_postexplode_table on Business
// test_toggled_table_remover removes test_toggled_load_table on load and test_toggled_add_table on onAddToWorld, test_toggled_table_construction adds test_postexplode_table on postExplode
[Collection("Domain")]
public class GlobalTableModifierMechanicTest
{
    private const string ItemName = "test_global_table_bus";
    private const int Snuid = 333;

    [Theory]
    [InlineData("globalTableModifiers")]
    [InlineData("test_custom_table")]
    [InlineData("test_toggled_load_table")]
    [InlineData("test_postexplode_table")]
    public void GameSettingsManager_GlobalTableModifierTypes_ContainsEveryMechanicType(string mechanicType)
    {
        GameSettingsManager.GlobalTableModifierTypes.Should().Contain(mechanicType);
    }

    [Theory]
    [InlineData("tutorial")]
    [InlineData("test_global_a_table")]
    public void GameSettingsManager_GlobalTableModifierTypes_IgnoresOtherMechanicsAndTableNames(string name)
    {
        GameSettingsManager.GlobalTableModifierTypes.Should().NotContain(name);
    }

    [Theory]
    [InlineData("test_toggled_load_table")] // removed on load
    [InlineData("test_toggled_add_table")] // removed on onAddToWorld
    [InlineData("test_postexplode_table")] // added on postExplode
    public void GameSettingsManager_UnsupportedGlobalTables_ContainsTablesNotDerivableFromWorld(string table)
    {
        GameSettingsManager.UnsupportedGlobalTables.Should().Contain(table);
    }

    [Fact]
    public void GameSettingsManager_UnsupportedGlobalTables_RemovedOnSellOrUpgrade_IsSupported()
    {
        GameSettingsManager.UnsupportedGlobalTables.Should().NotContain("test_global_a_table");
    }

    [Fact]
    public void GameItem_GetGlobalTableModifiers_CustomMechanicType_ReturnsLoadAddTable()
    {
        var modifiers = GameSettingsManager.Instance.GetItem("test_custom_table_provider")!.GetGlobalTableModifiers();

        modifiers.Select(x => (x.TargetKeyword, x.Table)).Should().Equal(("Business", "test_global_a_table"));
    }

    [Fact]
    public void GameItem_GetGlobalTableModifiers_UnsupportedTables_ReturnsEmpty()
    {
        var modifiers = GameSettingsManager.Instance.GetItem("test_toggled_table_provider")!.GetGlobalTableModifiers();

        modifiers.Should().BeEmpty();
    }

    [Fact]
    public void GameSettingsManager_GetGlobalTableProviders_OnlyItemsWithSupportedTables()
    {
        var providers = GameSettingsManager.Instance.GetGlobalTableProviders();

        providers.Should().Contain(["test_global_table_provider", "test_custom_table_provider"]);
        providers.Should().NotContain(["test_toggled_table_provider", "test_toggled_table_remover", "test_toggled_table_construction"]);
    }

    [Fact]
    public void Player_CollectDoobersRewards_CustomMechanicTypeProvider_RollsItsTable()
    {
        var faker = new Faker();
        var provider = faker.WorldObject(itemName: "test_custom_table_provider", className: BuildingClassType.Decoration);
        var player = faker.Player(world: faker.World(objects: [provider]));
        player.Snuid = Snuid;

        var secureRands = player.CollectDoobersRewards(ItemName);

        secureRands.Should().Equal(Roll(99, 1), Roll(1000000, 2));
    }

    [Fact]
    public void Player_CollectDoobersRewards_SameTableFromTwoProviders_RolledOnce()
    {
        var faker = new Faker();
        var custom = faker.WorldObject(itemName: "test_custom_table_provider", className: BuildingClassType.Decoration);
        var legacy = faker.WorldObject(itemName: "test_global_table_provider", className: BuildingClassType.Decoration);
        var player = faker.Player(world: faker.World(objects: [custom, legacy]));
        player.Snuid = Snuid;

        var secureRands = player.CollectDoobersRewards(ItemName);

        secureRands.Should().Equal(Roll(99, 1), Roll(1000000, 2));
    }

    [Fact]
    public void Player_CollectDoobersRewards_UnsupportedTablesProvider_RollsOnlyItemTable()
    {
        var faker = new Faker();
        var provider = faker.WorldObject(itemName: "test_toggled_table_provider", className: BuildingClassType.Decoration);
        var player = faker.Player(world: faker.World(objects: [provider]));
        player.Snuid = Snuid;

        var secureRands = player.CollectDoobersRewards(ItemName);

        secureRands.Should().Equal(Roll(99, 1));
    }

    private static int Roll(int rollRange, int rollCounter)
    {
        return SecureRand.GenerateRand(0, rollRange, rollCounter, Snuid.ToString());
    }
}
