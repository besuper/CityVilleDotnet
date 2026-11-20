using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Settings.GameSettings;
using CityVilleDotnet.Domain.Enums;

namespace CityVilleDotnet.Domain.Entities;

public partial class Player
{
    public List<SocialPost> SocialPosts { get; private set; } = [];
    public DateTime SocialNotificationsSeenAt { get; private set; }

    public void Publish(SocialPostType type, string key, string data, string? userMessage, Player? targetPlayer, DateTime now)
    {
        SocialPosts.Add(new SocialPost(this, type, key, data, userMessage, targetPlayer, now));
    }

    // TODO: implement loot/collectable/event
    public List<ViralReward> GrantViralRewards(IEnumerable<ViralReward> rewards)
    {
        var granted = new List<ViralReward>();

        foreach (var reward in rewards)
        {
            switch (reward.Type)
            {
                case "reward_coins":
                    AddCoins(reward.Amount);
                    break;
                case "reward_goods":
                    AddGoods(reward.Amount);
                    break;
                case "reward_xp":
                    AddXp(reward.Amount);
                    break;
                case "reward_energy":
                    AddEnergy(reward.Amount);
                    break;
                case "reward_cash":
                    AddCash(reward.Amount);
                    break;
                case "reward_item" when reward.ItemName is not null:
                    AddGift(reward.ItemName, reward.Amount);
                    break;
                default:
                    continue;
            }

            granted.Add(reward);
        }

        return granted;
    }

    // ViralManager::loadActiveVirals, shortcut: the viral "tuning" cooldowns and limitOnePerRecipient are ignored
    public List<(string Key, int TimesPosted, DateTime OriginTime)> GetActiveVirals(DateTime now)
    {
        return SocialPosts
            .GroupBy(x => x.Key)
            .Select(g => (Key: g.Key, Reset: TimeSpan.FromHours(GameSettingsManager.Instance.GetViral(g.Key)?.TimeTillReset ?? 0), Posts: g.ToList()))
            .Select(x => (x.Key, Posts: x.Posts.Where(p => p.CreatedAt > now - x.Reset).ToList()))
            .Where(x => x.Posts.Count > 0)
            .Select(x => (x.Key, x.Posts.Count, x.Posts.Min(p => p.CreatedAt)))
            .ToList();
    }

    public void MarkSocialNotificationsSeen(DateTime now)
    {
        SocialNotificationsSeenAt = now;
    }
}
