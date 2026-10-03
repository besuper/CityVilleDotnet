using AwesomeAssertions;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Test.Domain.Fixtures;

namespace CityVilleDotnet.Test.Domain.Rolls;

// LootTablesManager::rollForLoot: weightedRoll = Math.floor(roll / 1000 * (totalWeight || 100)) first item where weightedRoll <= weight + cumulative
[Collection("Domain")]
public class LootTableTest
{
    // test_weighted_table: a=28, b=22, c=50 without totalWeight
    [Theory]
    [InlineData(0, "test_weighted_a")]
    [InlineData(280, "test_weighted_a")]
    [InlineData(290, "test_weighted_a")] // 290 / 1000 * 100 = 28.999999999999996 in floating point, like the client
    [InlineData(300, "test_weighted_b")]
    [InlineData(500, "test_weighted_b")]
    [InlineData(501, "test_weighted_b")]
    [InlineData(510, "test_weighted_c")]
    [InlineData(1000, "test_weighted_c")]
    public void LootTable_GetItemNameForRoll_DefaultTotalWeight_MatchesClientRoll(int roll, string expected)
    {
        var lootTable = GameSettingsManager.Instance.GetLootTable("test_weighted_table")!;

        lootTable.GetItemNameForRoll(roll).Should().Be(expected);
    }

    // test_weighted_total_table: totalWeight=10, x=5, y=5
    [Theory]
    [InlineData(500, "test_weighted_x")]
    [InlineData(600, "test_weighted_y")]
    public void LootTable_GetItemNameForRoll_UsesTotalWeight(int roll, string expected)
    {
        var lootTable = GameSettingsManager.Instance.GetLootTable("test_weighted_total_table")!;

        lootTable.GetItemNameForRoll(roll).Should().Be(expected);
    }
}
