using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Settings.GameSettings;
using CityVilleDotnet.Domain.Enums;

namespace CityVilleDotnet.Domain.Entities;

public class SocialPost
{
    public int Id { get; }
    public Player? Player { get; private set; }
    public Player? TargetPlayer { get; private set; }
    public SocialPostType Type { get; private set; }
    public string Key { get; private set; } = string.Empty;
    public string Data { get; private set; } = string.Empty;
    public string? UserMessage { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public int MaxHelpers { get; private set; }
    public List<SocialPostHelper> Helpers { get; private set; } = [];

    public SocialPost() {}

    public SocialPost(Player author, SocialPostType type, string key, string data, string? userMessage, Player? targetPlayer, DateTime now)
    {
        Player = author;
        Type = type;
        Key = key;
        Data = data;
        UserMessage = userMessage?[..Math.Min(userMessage.Length, 256)];
        TargetPlayer = targetPlayer;
        CreatedAt = now;

        if (type != SocialPostType.Feed)
        {
            ExpiresAt = now.Add(TimeSpan.FromDays(7));
            MaxHelpers = 1;
            return;
        }

        var reward = GameSettingsManager.Instance.GetViral(key)?.Reward;

        ExpiresAt = reward?.ExpirationTime is { } hours ? now.AddHours(hours) : now.Add(TimeSpan.FromHours(48));
        MaxHelpers = reward?.MaxHelpers ?? 0;
    }

    public bool CanHelp(Guid helperId, DateTime now)
    {
        return helperId != Player!.Id
               && (Type == SocialPostType.Feed || TargetPlayer!.Id == helperId)
               && now < ExpiresAt
               && Helpers.Count < MaxHelpers
               && Helpers.All(x => x.Helper!.Id != helperId);
    }

    public List<ViralReward> Help(Player helper, DateTime now)
    {
        if (Player is null)
        {
            throw new Exception("Social post author is not loaded");
        }

        if (!CanHelp(helper.Id, now))
        {
            throw new DomainException(GameErrorType.InvalidState);
        }

        Helpers.Add(new SocialPostHelper(helper, now));

        if (Type == SocialPostType.Gift)
        {
            helper.AddGift(Key);
            return [new ViralReward { Type = "reward_item", Data = new ViralRewardData { ItemName = Key, Amount = 1 } }];
        }

        if (Type == SocialPostType.Request)
        {
            Player.AddGift(Key);
            return [];
        }

        var viral = GameSettingsManager.Instance.GetViral(Key);

        var granted = helper.GrantViralRewards(viral?.Reward?.HelperRewards ?? []);

        if (Helpers.Count <= (viral?.MaxHostRewards ?? MaxHelpers))
        {
            Player.GrantViralRewards(viral?.Reward?.HostRewards ?? []);
        }

        return granted;
    }
}
