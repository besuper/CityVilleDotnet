using AwesomeAssertions;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Test.Domain.Fixtures;

namespace CityVilleDotnet.Test.Domain.GateTuning;

// See test_tuning in TestData/gameSettings.xml
[Collection("Domain")]
public class GateTuningTest
{
    [Theory]
    [InlineData(1, 6)] // tier 1: mul 0.6 round up
    [InlineData(25, 6)]
    [InlineData(26, 15)] // tier 2: add 5
    [InlineData(45, 15)]
    [InlineData(46, 7)] // tier 3: set 7
    [InlineData(70, 7)]
    [InlineData(71, 3)] // tier 4: mul 0.1 round down, min 3
    [InlineData(90, 3)]
    [InlineData(91, 11)] // tier 5: mul 1.15 round down
    public void GameSettingsManager_GetGateTunedAmount_AppliesPlayerLevelTier(int playerLevel, int expected)
    {
        GameSettingsManager.Instance.GetGateTunedAmount("test_tuning", "inventory", playerLevel, 10).Should().Be(expected);
    }

    [Fact]
    public void GameSettingsManager_GetGateTunedAmount_NoTuning_ReturnsAmount()
    {
        GameSettingsManager.Instance.GetGateTunedAmount(null, "inventory", 1, 10).Should().Be(10);
    }

    [Fact]
    public void GameSettingsManager_GetGateTunedAmount_UnknownTuning_ReturnsAmount()
    {
        GameSettingsManager.Instance.GetGateTunedAmount("test_unknown_tuning", "inventory", 1, 10).Should().Be(10);
    }

    [Fact]
    public void GameSettingsManager_GetGateTunedAmount_OtherHook_UsesItsOwnTiers()
    {
        GameSettingsManager.Instance.GetGateTunedAmount("test_tuning", "other", 1, 10).Should().Be(0);
    }

    [Fact]
    public void GameSettingsManager_GetGateTunedAmount_UnknownHook_ReturnsAmount()
    {
        GameSettingsManager.Instance.GetGateTunedAmount("test_tuning", "crew", 1, 10).Should().Be(10);
    }
}
