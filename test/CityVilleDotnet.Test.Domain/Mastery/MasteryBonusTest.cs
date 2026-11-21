using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Test.Domain.Fixtures;

namespace CityVilleDotnet.Test.Domain.Mastery;

[Collection("Domain")]
public class MasteryBonusTest
{
    [Fact]
    public void Player_GetMasteryBonusMultiplier_Crop_ReturnsCurrentLevelBonus()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.SetLevel(15);
        player.IncrementMastery("test_mastery_bonus_crop", 5);

        var multiplier = player.GetMasteryBonusMultiplier(GameSettingsManager.Instance.GetItem("test_mastery_bonus_crop")!, "goods");

        multiplier.Should().Be(1.2);
    }

    [Fact]
    public void Player_GetMasteryBonusMultiplier_NoMastery_ReturnsOne()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.SetLevel(15);

        var multiplier = player.GetMasteryBonusMultiplier(GameSettingsManager.Instance.GetItem("test_mastery_bonus_crop")!, "goods");

        multiplier.Should().Be(1);
    }

    [Fact]
    public void Player_GetMasteryBonusMultiplier_CropBelowRequiredLevel_ReturnsOne()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.SetLevel(14);
        player.IncrementMastery("test_mastery_bonus_crop", 5);

        var multiplier = player.GetMasteryBonusMultiplier(GameSettingsManager.Instance.GetItem("test_mastery_bonus_crop")!, "goods");

        multiplier.Should().Be(1);
    }

    [Fact]
    public void Player_GetMasteryBonusMultiplier_FieldWithoutBonus_ReturnsOne()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.SetLevel(15);
        player.IncrementMastery("test_mastery_bonus_crop", 5);

        var multiplier = player.GetMasteryBonusMultiplier(GameSettingsManager.Instance.GetItem("test_mastery_bonus_crop")!, "coinYield");

        multiplier.Should().Be(1);
    }

    [Fact]
    public void Player_GetMasteryBonusMultiplier_AirplaneContract_IgnoresPlayerLevel()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.IncrementMastery("test_mastery_bonus_plane", 2);

        var multiplier = player.GetMasteryBonusMultiplier(GameSettingsManager.Instance.GetItem("test_mastery_bonus_plane")!, "coinYield");

        multiplier.Should().Be(1.1);
    }
}
