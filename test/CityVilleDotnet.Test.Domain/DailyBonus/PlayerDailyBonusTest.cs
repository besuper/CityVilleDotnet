using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Settings.GameSettings;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Factory.Player;
using Microsoft.Extensions.Time.Testing;

namespace CityVilleDotnet.Test.Domain.DailyBonus;

// See dailyBonus and test_daily_* in TestData/gameSettings.xml
[Collection("Domain")]
public class PlayerDailyBonusTest
{
    private static readonly DateTimeOffset StartTime = new(2026, 1, 15, 12, 0, 0, TimeSpan.Zero);

    private static FakeTimeProvider UseFakeTime()
    {
        var time = new FakeTimeProvider(StartTime);
        ServerUtils.TimeProvider = time;
        return time;
    }

    private static List<DailyBonusDay> Days(int variant = 0)
    {
        return GameSettingsManager.Instance.GetDailyBonusDays(new Dictionary<string, int> { ["test_dailybonus"] = variant });
    }

    [Fact]
    public void Player_GetDailyBonusDays_FiltersByExperimentVariant()
    {
        Days(0).Select(x => x.Number).Should().Equal(1, 2, 3);
        Days(0).Should().OnlyContain(x => x.Variants == "0,2");
        Days(2).Should().OnlyContain(x => x.Variants == "0,2");
        Days(1).Should().OnlyContain(x => x.Variants == "1");
        Days(5).Should().BeEmpty();
    }

    [Fact]
    public void Player_GetDailyBonusStreak_NeverCollected_ReturnsDayOne()
    {
        UseFakeTime();
        var player = new Faker().Player();

        player.GetDailyBonusStreak().Should().Be((1, 0));
    }

    [Fact]
    public void Player_CollectDailyBonus_DayOne_GrantsEnergyAndMarksCollected()
    {
        UseFakeTime();
        var player = new Faker().Player();
        var energyBefore = player.Energy;

        var randomReward = player.CollectDailyBonus(Days());

        randomReward.Should().BeNull();
        player.Energy.Should().Be(energyBefore + 3);
        player.DailyBonusDay.Should().Be(1);
        player.LastDailyBonusDate.Should().Be(new DateOnly(2026, 1, 15));
        player.GetDailyBonusStreak().Should().Be((1, 1));
    }

    [Fact]
    public void Player_CollectDailyBonus_AlreadyCollectedToday_ThrowsInvalidState()
    {
        var time = UseFakeTime();
        var player = new Faker().Player();
        player.CollectDailyBonus(Days());
        time.Advance(TimeSpan.FromHours(11));

        var act = () => player.CollectDailyBonus(Days());

        act.Should().Throw<DomainException>().Which.Reason.Should().Be(GameErrorType.InvalidState);
    }

    [Fact]
    public void Player_CollectDailyBonus_NextCalendarDay_GrantsDayTwoRewards()
    {
        var time = UseFakeTime();
        var player = new Faker().Player();
        player.CollectDailyBonus(Days());
        time.Advance(TimeSpan.FromHours(12));

        var goldBefore = player.Gold;
        var cashBefore = player.Cash;
        var goodsBefore = player.Goods;

        player.GetDailyBonusStreak().Should().Be((2, 1));

        var randomReward = player.CollectDailyBonus(Days());

        randomReward.Should().BeNull();
        player.Gold.Should().Be(goldBefore + 100);
        player.Cash.Should().Be(cashBefore + 2);
        player.Goods.Should().Be(goodsBefore + 50);
        player.CountInventoryItem("test_daily_item").Should().Be(1);
        player.DailyBonusDay.Should().Be(2);
    }

    [Fact]
    public void Player_CollectDailyBonus_XpReward_AddsXpAndLevelsUp()
    {
        var time = UseFakeTime();
        var player = new Faker().Player();
        player.CollectDailyBonus(Days(1));
        time.Advance(TimeSpan.FromDays(1));

        player.CollectDailyBonus(Days(1));

        player.Xp.Should().Be(10);
        player.Level.Should().Be(2);
    }

    [Fact]
    public void Player_CollectDailyBonus_MissedDay_RestartsAtDayOne()
    {
        var time = UseFakeTime();
        var player = new Faker().Player();
        player.CollectDailyBonus(Days());
        time.Advance(TimeSpan.FromDays(2));

        player.GetDailyBonusStreak().Should().Be((1, 0));

        player.CollectDailyBonus(Days());

        player.DailyBonusDay.Should().Be(1);
        player.CountInventoryItem("test_daily_item").Should().Be(0);
    }

    [Fact]
    public void Player_CollectDailyBonus_LastDayItem_ReturnsRandomRewardAndRestartsStreak()
    {
        var time = UseFakeTime();
        var player = new Faker().Player();
        player.CollectDailyBonus(Days());
        time.Advance(TimeSpan.FromDays(1));
        player.CollectDailyBonus(Days());
        time.Advance(TimeSpan.FromDays(1));

        var randomReward = player.CollectDailyBonus(Days());

        randomReward.Should().Be("test_daily_surprise");
        player.CountInventoryItem("test_daily_surprise").Should().Be(1);
        player.DailyBonusDay.Should().Be(0);
        player.GetDailyBonusStreak().Should().Be((0, 0));

        time.Advance(TimeSpan.FromDays(1));

        player.GetDailyBonusStreak().Should().Be((1, 0));
    }

    [Fact]
    public void Player_CollectDailyBonus_LastDayCurrency_GrantsEveryAmount()
    {
        var time = UseFakeTime();
        var player = new Faker().Player();
        player.CollectDailyBonus(Days(1));
        time.Advance(TimeSpan.FromDays(1));
        player.CollectDailyBonus(Days(1));
        time.Advance(TimeSpan.FromDays(1));

        var goldBefore = player.Gold;
        var cashBefore = player.Cash;

        var randomReward = player.CollectDailyBonus(Days(1));

        randomReward.Should().BeNull();
        player.Gold.Should().Be(goldBefore + 500);
        player.Cash.Should().Be(cashBefore + 1);
    }

    [Fact]
    public void Player_CollectDailyBonus_NoDaysForVariant_ThrowsInvalidData()
    {
        UseFakeTime();
        var player = new Faker().Player();

        var act = () => player.CollectDailyBonus(Days(5));

        act.Should().Throw<DomainException>().Which.Reason.Should().Be(GameErrorType.InvalidData);
    }
}
