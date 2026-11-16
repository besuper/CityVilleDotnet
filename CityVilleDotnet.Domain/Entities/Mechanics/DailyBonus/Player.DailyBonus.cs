using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Common.Settings.GameSettings;
using CityVilleDotnet.Common.Utils;

namespace CityVilleDotnet.Domain.Entities;

public partial class Player
{
    public int DailyBonusDay { get; private set; }
    public DateOnly? LastDailyBonusDate { get; private set; }

    public (int Current, int Previous) GetDailyBonusStreak()
    {
        var today = GetDailyBonusToday();

        if (LastDailyBonusDate == today)
        {
            return (DailyBonusDay, DailyBonusDay);
        }

        if (LastDailyBonusDate == today.AddDays(-1))
        {
            return (DailyBonusDay + 1, DailyBonusDay);
        }

        return (1, 0);
    }

    public string? CollectDailyBonus(List<DailyBonusDay> days)
    {
        var (current, previous) = GetDailyBonusStreak();

        if (current == previous)
        {
            throw new DomainException(GameErrorType.InvalidState, "Daily bonus already collected today");
        }

        var day = days.FirstOrDefault(x => x.Number == current) ?? throw new DomainException(GameErrorType.InvalidData, $"Daily bonus day {current} not found");

        // Day 0 after the last day so tomorrow restarts the streak at 1
        DailyBonusDay = current == days.Count ? 0 : current;
        LastDailyBonusDate = GetDailyBonusToday();

        if (day.RandomChance is null)
        {
            GrantDailyBonusRewards(day);

            if (day.Items.Count > 0)
            {
                AddItem(day.Items[0].Name);
            }

            return null;
        }

        var randomReward = day.RandomChance.RollItemName();

        if (randomReward is null)
        {
            GrantDailyBonusRewards(day.RandomChance);
        }
        else
        {
            AddItem(randomReward);
        }

        return randomReward;
    }

    private static DateOnly GetDailyBonusToday() => DateOnly.FromDateTime(ServerUtils.TimeProvider.GetUtcNow().UtcDateTime);

    // DailyBonusManager::grantBonus
    private void GrantDailyBonusRewards(DailyBonusRewards rewards)
    {
        AddCash(rewards.Cash?.Amount ?? 0);
        AddCoins(rewards.Coin?.Amount ?? 0);
        AddGoods(rewards.Goods?.Amount ?? 0);
        AddXp(rewards.Xp?.Amount ?? 0);

        if (rewards.Energy is not null)
        {
            AddEnergy(rewards.Energy.Amount);
        }
    }
}
