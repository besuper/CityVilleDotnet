using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Factory.Player;
using Microsoft.Extensions.Time.Testing;

namespace CityVilleDotnet.Test.Domain.Grants;

// See <grantItems>, test_grant_* validators, the test_grant_sunset sunset and test_grant_* items in TestData/gameSettings.xml
[Collection("Domain")]
public class PlayerGrantEligibleItemsTest
{
    private static readonly Dictionary<string, int> NoExperiments = [];

    private static void UseFakeTime(DateTimeOffset now)
    {
        ServerUtils.TimeProvider = new FakeTimeProvider(now);
    }

    [Fact]
    public void Player_GrantEligibleItems_LevelReached_GrantsItemAndSetsFlag()
    {
        var player = new Faker().Player();
        player.SetLevel(10);

        player.GrantEligibleItems(NoExperiments, []);

        player.CountInventoryItem("test_grant_pad").Should().Be(1);
        player.Flags.Should().ContainSingle(x => x.Name == "grant_test_grant_level").Which.Value.Should().Be(1);
    }

    [Fact]
    public void Player_GrantEligibleItems_LevelTooLow_GrantsNothingAndRetriesLater()
    {
        var player = new Faker().Player();
        player.SetLevel(9);

        player.GrantEligibleItems(NoExperiments, []);

        player.CountInventoryItem("test_grant_pad").Should().Be(0);
        player.Flags.Should().NotContain(x => x.Name == "grant_test_grant_level");

        player.SetLevel(10);
        player.GrantEligibleItems(NoExperiments, []);

        player.CountInventoryItem("test_grant_pad").Should().Be(1);
    }

    [Fact]
    public void Player_GrantEligibleItems_AlreadyGranted_DoesNotGrantAgain()
    {
        var player = new Faker().Player();
        player.SetLevel(10);

        player.GrantEligibleItems(NoExperiments, []);
        player.RemoveItem("test_grant_pad");
        player.GrantEligibleItems(NoExperiments, []);

        player.CountInventoryItem("test_grant_pad").Should().Be(0);
    }

    [Fact]
    public void Player_GrantEligibleItems_UpgradeOwnedInWorld_SkipsItemButSetsFlag()
    {
        var player = new Faker().Player();
        player.SetLevel(10);

        player.GrantEligibleItems(NoExperiments, ["test_grant_pad_2"]);

        player.CountInventoryItem("test_grant_pad").Should().Be(0);
        player.Flags.Should().Contain(x => x.Name == "grant_test_grant_level");
    }

    [Fact]
    public void Player_GrantEligibleItems_ItemInInventory_SkipsItem()
    {
        var player = new Faker().Player();
        player.SetLevel(10);
        player.AddItem("test_grant_pad");

        player.GrantEligibleItems(NoExperiments, []);

        player.CountInventoryItem("test_grant_pad").Should().Be(1);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 0)]
    [InlineData(2, 1)]
    public void Player_GrantEligibleItems_ExperimentVariant_GrantsWhenGreaterThanVariant(int variant, int expected)
    {
        var player = new Faker().Player();

        player.GrantEligibleItems(new Dictionary<string, int> { ["test_grant_exp"] = variant }, []);

        player.CountInventoryItem("test_grant_exp_item").Should().Be(expected);
    }

    [Fact]
    public void Player_GrantEligibleItems_BeforeSunsetEnd_GrantsItem()
    {
        UseFakeTime(new DateTimeOffset(2026, 6, 15, 18, 59, 0, TimeSpan.Zero));
        var player = new Faker().Player();

        player.GrantEligibleItems(NoExperiments, []);

        player.CountInventoryItem("test_grant_sunset_item").Should().Be(1);
    }

    [Fact]
    public void Player_GrantEligibleItems_AfterSunsetEnd_GrantsNothing()
    {
        // endDate is 06/15/2026 12:00:00 PDT, so 19:00 UTC
        UseFakeTime(new DateTimeOffset(2026, 6, 15, 19, 1, 0, TimeSpan.Zero));
        var player = new Faker().Player();

        player.GrantEligibleItems(NoExperiments, []);

        player.CountInventoryItem("test_grant_sunset_item").Should().Be(0);
        player.Flags.Should().NotContain(x => x.Name == "grant_test_grant_sunset");
    }

    [Fact]
    public void Player_GrantEligibleItems_UnsupportedValidator_GrantsNothing()
    {
        var player = new Faker().Player();
        player.SetLevel(10);

        player.GrantEligibleItems(NoExperiments, []);

        player.CountInventoryItem("test_grant_unsupported_item").Should().Be(0);
    }

    [Fact]
    public void Player_GrantEligibleItems_NoValidatorWithRepeatedNames_GrantsEachName()
    {
        var player = new Faker().Player();

        player.GrantEligibleItems(NoExperiments, []);

        player.CountInventoryItem("test_grant_key").Should().Be(2);
    }
}
