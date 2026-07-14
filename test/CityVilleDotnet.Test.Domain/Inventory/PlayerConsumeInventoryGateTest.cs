using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Test.Domain.Fixtures;

namespace CityVilleDotnet.Test.Domain.Inventory;

[Collection("Domain")]
public class PlayerConsumeInventoryGateTest(DomainFixture fixture)
{
    private const string BuildingName = "test_gated_building";
    private const string MaterialName = "test_gate_material";

    [Fact]
    public void Player_ConsumeInventoryGate_TypelessGate_RemovesRequiredAmount()
    {
        var faker = new Faker();
        var player = faker.Player();
        var buildingItem = GameSettingsManager.Instance.GetItem(BuildingName)!;

        player.AddItem(MaterialName, 5);

        var removed = player.ConsumeInventoryGate(buildingItem, "build");

        removed.Should().BeEmpty();
        player.CountInventoryItem(MaterialName).Should().Be(3);
    }

    [Fact]
    public void Player_ConsumeInventoryGate_ExactAmount_RemovesItemAndReturnsIt()
    {
        var faker = new Faker();
        var player = faker.Player();
        var buildingItem = GameSettingsManager.Instance.GetItem(BuildingName)!;

        player.AddItem(MaterialName, 2);

        var removed = player.ConsumeInventoryGate(buildingItem, "build");

        removed.Should().ContainSingle(x => x.Name == MaterialName);
        player.HasItem(MaterialName).Should().BeFalse();
    }

    [Fact]
    public void Player_ConsumeInventoryGate_PlayerHasLessThanRequired_ThrowsException()
    {
        var faker = new Faker();
        var player = faker.Player();
        var buildingItem = GameSettingsManager.Instance.GetItem(BuildingName)!;

        player.AddItem(MaterialName, 1);

        var act = () => player.ConsumeInventoryGate(buildingItem, "build");

        act.Should().Throw<Exception>().WithMessage("Not enough items*");
    }

    [Fact]
    public void Player_ConsumeInventoryGate_CrewGate_DoesNothing()
    {
        var faker = new Faker();
        var player = faker.Player();
        var buildingItem = GameSettingsManager.Instance.GetItem(BuildingName)!;

        player.AddItem(MaterialName, 5);

        var removed = player.ConsumeInventoryGate(buildingItem, "pre_upgrade");

        removed.Should().BeEmpty();
        player.CountInventoryItem(MaterialName).Should().Be(5);
    }

    [Fact]
    public void Player_ConsumeInventoryGate_GateNotFound_ReturnsEmpty()
    {
        var faker = new Faker();
        var player = faker.Player();
        var buildingItem = GameSettingsManager.Instance.GetItem(BuildingName)!;

        player.AddItem(MaterialName, 5);

        var removed = player.ConsumeInventoryGate(buildingItem, "nonexistent");

        removed.Should().BeEmpty();
        player.CountInventoryItem(MaterialName).Should().Be(5);
    }
}
