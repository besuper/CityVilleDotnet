using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Settings.GameSettings;
using CityVilleDotnet.Domain.Enums;

namespace CityVilleDotnet.Domain.Entities;

public partial class Player
{
    public static readonly TimeSpan ViralAckLifetime = TimeSpan.FromDays(7);

    public List<SocialPost> SocialPosts { get; private set; } = [];
    public DateTime SocialNotificationsSeenAt { get; private set; }

    public void Publish(SocialPostType type, string key, string data, string? userMessage, Player? targetPlayer, int targetObjectId, string? targetObjectName, string? targetItemName, DateTime now)
    {
        SocialPosts.Add(new SocialPost(this, type, key, data, userMessage, targetPlayer, targetObjectId, targetObjectName, targetItemName, now));
    }

    public List<Player> SendRequest(SocialPostType type, string itemName, string data, IReadOnlyCollection<int> recipientSnuids, DateTime now)
    {
        var recipients = Friends
            .Where(x => x.Status == FriendshipStatus.Accepted && !x.GetFriend().IsSamantha() && recipientSnuids.Contains(x.GetFriend().Snuid))
            .Select(x => x.GetFriend())
            .ToList();

        foreach (var recipient in recipients)
        {
            SocialPosts.Add(new SocialPost(this, type, itemName, data, null, recipient, 0, null, null, now));
        }

        return recipients;
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
            .Where(x => x.Type == SocialPostType.Feed)
            .GroupBy(x => x.Key)
            .Select(g => (Key: g.Key, Reset: TimeSpan.FromHours(GameSettingsManager.Instance.GetViral(g.Key)?.TimeTillReset ?? 0), Posts: g.ToList()))
            .Select(x => (x.Key, Posts: x.Posts.Where(p => p.CreatedAt > now - x.Reset).ToList()))
            .Where(x => x.Posts.Count > 0)
            .Select(x => (x.Key, x.Posts.Count, x.Posts.Min(p => p.CreatedAt)))
            .ToList();
    }

    // Player::canSendGift
    public List<(int RecipientSnuid, DateTime SentAt)> GetSentGifts(DateTime now)
    {
        var since = now.AddSeconds(-GameSettingsManager.Instance.GetSettings().GiftingPerUserTimeLimit);

        return SocialPosts
            .Where(x => x.Type == SocialPostType.Gift && x.CreatedAt > since)
            .GroupBy(x => x.TargetPlayer!.Snuid)
            .Select(g => (g.Key, g.Max(x => x.CreatedAt)))
            .ToList();
    }

    // Player::addActiveRequests
    public (DateTime Ts, List<int> RecipientSnuids)? GetActiveItemRequests(DateTime now)
    {
        var since = now.AddHours(-GameSettingsManager.Instance.GetSettings().RequestGiftRequestResetTime);
        var requests = SocialPosts.Where(x => x.Type == SocialPostType.Request && x.CreatedAt > since).ToList();

        if (requests.Count == 0)
        {
            return null;
        }

        return (requests.Min(x => x.CreatedAt), requests.Select(x => x.TargetPlayer!.Snuid).Distinct().ToList());
    }

    // ViralAckData::loadFeatureData
    public List<(int TargetObjectId, string? TargetObjectName, string? ItemName, int HelperSnuid, DateTime HelpedAt)> GetViralAcks(WorldType world, DateTime now)
    {
        var since = now.Add(-ViralAckLifetime);

        return SocialPosts
            .Where(x => x.TargetObjectId is not null && x.TargetWorld == world)
            .SelectMany(x => x.Helpers
                .Where(h => !h.AckSeen && h.HelpedAt > since)
                .Select(h => (x.TargetObjectId!.Value, x.TargetObjectName, x.TargetItemName, h.Helper!.Snuid, h.HelpedAt)))
            .ToList();
    }

    // ViralAckPickStack::onPickClosed
    public void MarkViralAckSeen(WorldType world, int targetObjectId, int? helperSnuid)
    {
        var helpers = SocialPosts
            .Where(x => x.TargetObjectId == targetObjectId && x.TargetWorld == world)
            .SelectMany(x => x.Helpers)
            .Where(h => helperSnuid is null || h.Helper!.Snuid == helperSnuid);

        foreach (var helper in helpers)
        {
            helper.MarkAckSeen();
        }
    }

    public void MarkSocialNotificationsSeen(DateTime now)
    {
        SocialNotificationsSeenAt = now;
    }
}
