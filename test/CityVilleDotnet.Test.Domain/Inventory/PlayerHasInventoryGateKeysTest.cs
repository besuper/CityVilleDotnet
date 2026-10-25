using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Test.Domain.Fixtures;

namespace CityVilleDotnet.Test.Domain.Inventory;

// See test_tuned_gate_building in TestData/gameSettings.xml, level 1 tunes its 10 materials down to 6
[Collection("Domain")]
public class PlayerHasInventoryGateKeysTest
{
    private const string BuildingName = "test_tuned_gate_building";
    private const string MaterialName = "test_gate_material";

    [Fact]
    public void Player_HasInventoryGateKeys_TunedAmountOwned_ReturnsTrue()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.AddItem(MaterialName, 6);

        player.HasInventoryGateKeys(BuildingName, "build").Should().BeTrue();
    }

    [Fact]
    public void Player_HasInventoryGateKeys_LessThanTunedAmount_ReturnsFalse()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.AddItem(MaterialName, 5);

        player.HasInventoryGateKeys(BuildingName, "build").Should().BeFalse();
    }

    [Fact]
    public void Player_HasInventoryGateKeys_EveryKeyOwned_ReturnsTrue()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.AddItem(MaterialName, 2);
        player.AddItem("test_flyer");

        player.HasInventoryGateKeys(BuildingName, "test_two_keys").Should().BeTrue();
    }

    [Fact]
    public void Player_HasInventoryGateKeys_OneKeyMissing_ReturnsFalse()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.AddItem(MaterialName, 2);

        player.HasInventoryGateKeys(BuildingName, "test_two_keys").Should().BeFalse();
    }

    [Fact]
    public void Player_HasInventoryGateKeys_GateNotFound_ReturnsFalse()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.AddItem(MaterialName, 10);

        player.HasInventoryGateKeys(BuildingName, "nonexistent").Should().BeFalse();
    }

    [Fact]
    public void Player_HasInventoryGateKeys_ItemNotFound_ReturnsFalse()
    {
        var faker = new Faker();
        var player = faker.Player();

        player.HasInventoryGateKeys("test_unknown_item", "build").Should().BeFalse();
    }
}
