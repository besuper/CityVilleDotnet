using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Settings.GameSettings;
using CityVilleDotnet.Factory.Player;

namespace CityVilleDotnet.Test.Domain.MasteryGate;

[Collection("Domain")]
public class PlayerBuyMasteryGateTest
{
    private static GatesItem GetGate(string gateName)
    {
        return GameSettingsManager.Instance.GetItem("test_mastery_gate")!.GetGates().First(x => x.Name == gateName);
    }

    private static CityVilleDotnet.Domain.Entities.Player CreatePlayer(int cash, int cropACount = 0, int cropBCount = 0)
    {
        var player = new Faker().Player();
        player.SetCash(cash);

        if (cropACount > 0) player.IncrementMastery("test_mastery_crop_a", cropACount);
        if (cropBCount > 0) player.IncrementMastery("test_mastery_crop_b", cropBCount);

        return player;
    }

    [Fact]
    public void Player_BuyMasteryGate_Crop_NoProgress_ChargesFullCostAndSetsFlag()
    {
        var player = CreatePlayer(150);

        player.BuyMasteryGate("test_mastery_gate", GetGate("mastery_crop"));

        player.Cash.Should().Be(50);
        player.HasSeenFlag("buyout_test_mastery_gate_mastery_crop").Should().BeTrue();
    }

    [Fact]
    public void Player_BuyMasteryGate_Crop_PartialProgress_ChargesLowestProratedCost()
    {
        // a: floor(10 / 40 * 100) = 25, b: floor(300 / 300 * 100) = 100
        var player = CreatePlayer(150, cropACount: 30);

        player.BuyMasteryGate("test_mastery_gate", GetGate("mastery_crop"));

        player.Cash.Should().Be(125);
    }

    [Fact]
    public void Player_BuyMasteryGate_Crop_FractionalCost_FloorsCost()
    {
        // b: floor(200 / 300 * 100) = 66
        var player = CreatePlayer(150, cropBCount: 100);

        player.BuyMasteryGate("test_mastery_gate", GetGate("mastery_crop"));

        player.Cash.Should().Be(84);
    }

    [Fact]
    public void Player_BuyMasteryGate_Crop_AlmostMastered_ChargesAtLeastOne()
    {
        // b: floor(1 / 300 * 100) = 0 => 1
        var player = CreatePlayer(150, cropBCount: 299);

        player.BuyMasteryGate("test_mastery_gate", GetGate("mastery_crop"));

        player.Cash.Should().Be(149);
    }

    [Fact]
    public void Player_BuyMasteryGate_Crop_OneKeyMastered_IsFree()
    {
        var player = CreatePlayer(150, cropACount: 40);

        player.BuyMasteryGate("test_mastery_gate", GetGate("mastery_crop"));

        player.Cash.Should().Be(150);
        player.HasSeenFlag("buyout_test_mastery_gate_mastery_crop").Should().BeTrue();
    }

    [Fact]
    public void Player_BuyMasteryGate_Business_ChargesCashCost()
    {
        var player = CreatePlayer(150);

        player.BuyMasteryGate("test_mastery_gate", GetGate("mastery_business"));

        player.Cash.Should().Be(70);
        player.HasSeenFlag("buyout_test_mastery_gate_mastery_business").Should().BeTrue();
    }

    [Fact]
    public void Player_BuyMasteryGate_AlreadyBought_DoesNotChargeAgain()
    {
        var player = CreatePlayer(150);
        player.SetSeenFlag("buyout_test_mastery_gate_mastery_crop");

        player.BuyMasteryGate("test_mastery_gate", GetGate("mastery_crop"));

        player.Cash.Should().Be(150);
    }

    [Fact]
    public void Player_BuyMasteryGate_NotEnoughCash_ThrowsAndDoesNotSetFlag()
    {
        var player = CreatePlayer(99);

        var act = () => player.BuyMasteryGate("test_mastery_gate", GetGate("mastery_crop"));

        act.Should().Throw<DomainException>().Which.Reason.Should().Be(GameErrorType.NotEnoughMoney);
        player.Cash.Should().Be(99);
        player.HasSeenFlag("buyout_test_mastery_gate_mastery_crop").Should().BeFalse();
    }
}
