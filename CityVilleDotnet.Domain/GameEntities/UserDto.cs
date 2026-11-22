using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Entities;
using System.Text.Json.Serialization;
using CityVilleDotnet.Domain.EnumExtensions;
using CityVilleDotnet.Domain.Enums;

namespace CityVilleDotnet.Domain.GameEntities;

public class UserDto
{
    [JsonPropertyName("userInfo")] public required UserInfoDto UserInfo { get; set; }
    [JsonPropertyName("franchises")] public List<FranchiseDto> Franchises { get; set; } = new();
    [JsonPropertyName("featureData")] public required Dictionary<string, object> FeatureData { get; set; }
    [JsonPropertyName("preyGroups")] public List<PreyGroupDto> PreyGroups { get; set; } = [];
    [JsonPropertyName("crews")] public Dictionary<string, List<string>> Crews { get; set; } = [];
}

public static class UserDtoMapper
{
    public static UserDto ToDto(this Player model, IEnumerable<World>? additionalWorlds = null)
    {
        var activeQuests = model.Quests
            .Where(q => q.QuestType == QuestType.Active)
            .ToList();

        // The client replaces its whole summaries map on each response, so every world must always be present
        var summaryWorlds = model.Worlds
            .Concat(additionalWorlds ?? [])
            .GroupBy(w => w.Type)
            .Select(g => g.First())
            .ToList();

        var energyModifiers = new Dictionary<string, int>();

        foreach (var obj in model.GetWorld().Objects.Where(o => o.EnergyModifier > 0))
        {
            energyModifiers[$"{obj.WorldFlatId}"] = obj.EnergyModifier;
        }

        var (gas, gasLastCheck) = model.GetCurrentGas(ServerUtils.GetCurrentTimeSeconds());
        var (currentBonusDay, previousBonusDay) = model.GetDailyBonusStreak();

        return new UserDto()
        {
            Crews = model.GetWorld().Objects.Where(o => o.CrewMembers.Count > 0).ToDictionary(
                o => o.WorldFlatId.ToString(),
                o => o.CrewMembers.Select(m => m.Player?.Snuid.ToString() ?? "-1").ToList()
            ),
            UserInfo = new UserInfoDto
            {
                CreationTimestamp = model.CreationTimestamp.ToUnixTimeMilliseconds(),
                FirstDay = model.FirstDay,
                IsNew = model.IsNew,
                CompletedQuests = model.Quests.Where(q => q.QuestType == QuestType.Completed).Select(q => q.Name).ToList(),
                QuestsExpired = model.Quests.Where(q => q.QuestType == QuestType.Expired).Select(q => q.Name).ToList(),
                LastPlayedWorldId = model.LastPlayedWorldType.ToDescriptionString(),
                Player = new PlayerDto
                {
                    Uid = model.Snuid.ToString(),
                    FirstPlayDate = model.CreationTimestamp.ToUnixTimeSeconds(),
                    Cash = model.Cash,
                    Collections = model.Collections
                        .GroupBy(item => item.Name)
                        .ToDictionary(group => group.Key, group => group.SelectMany(x => x.Items).ToDictionary(x => x.Name, x => x.Amount)),
                    Commodities = new CommoditiesDto
                    {
                        Storage = new StorageDto
                        {
                            Goods = model.Goods,
                            PremiumGoods = model.PremiumGoods
                        }
                    },
                    CompletedCollections = model.Collections.Where(x => x.Completed > 0).ToDictionary(x => x.Name, x => x.Completed),
                    CollectionTradeIns = model.Collections.Where(x => x.TradeIns > 0).ToDictionary(x => x.Name, x => x.TradeIns),
                    Energy = model.Energy,
                    EnergyMax = model.EnergyMax,
                    LastEnergyCheck = model.GetLastCheckEnergyTimestamp(),
                    RegenerableResources = new() { ["gas"] = gas },
                    RegenerableLastUpdates = new() { ["gas"] = gasLastCheck },
                    ExpansionsPurchased = model.ExpansionsPurchased,
                    Gold = model.Gold,
                    Inventory = new InventoryDto
                    {
                        Count = model.CountInventoryItems(),
                        Items = model.InventoryItems.Where(x => x.StorageType is null).ToDictionary(x => x.Name, x => x.Amount)
                    },
                    RequestInventory = model.ToRequestInventoryDto(),
                    LastTrackingTimestamp = model.LastTrackingTimestamp.ToUnixTimeMilliseconds(),
                    Level = model.Level,
                    Licenses = model.Licenses.ToDictionary(x => x.Name, x => x.Amount),
                    SubLicenses = model.SubLicenses.ToDictionary(x => x.Name, x => x.Mask.ToString("x")), // => as hexadecimal
                    Neighbors = model.Friends
                        .Where(f => !f.GetFriend().IsSamantha() && f.Status == FriendshipStatus.Accepted)
                        .Select(friend => friend.ToNeighborDto()).ToList(), // TODO: Change this after moving friends to player
                    Options = new OptionsDto
                    {
                        MusicDisabled = model.MusicDisabled,
                        SfxDisabled = model.SfxDisabled,
                    },
                    PlayerNews = [], // TODO: Implement news
                    ActiveVirals = model.ToActiveViralDtos(),
                    SentGifts = model.GetSentGifts(ServerUtils.TimeProvider.GetUtcNow().UtcDateTime).ToDictionary(x => $"i{x.RecipientSnuid}", x => ToUnixSeconds(x.SentAt)),
                    ActiveRequests = model.ToActiveRequestDtos(),
                    RollCounter = model.RollCounter,
                    RollCounterMap = model.FeatureRollCounters.ToDictionary(x => x.Feature, x => x.Count),
                    SeenFlags = model.SeenFlags.DistinctBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value is { } value ? (object)value : true),
                    Coupons = model.Coupons.Where(x => x.WorldFlatId is null).Select(x => x.Name).ToList(),
                    FeatureThemes = model.NightModeActive ? ["night_theme"] : [],
                    AssociatedCoupons = model.Coupons
                        .Where(x => x.WorldFlatId is not null)
                        .GroupBy(x => x.WorldFlatId!.Value)
                        .ToDictionary(g => g.Key.ToString(), g => g.Select(x => x.Name).ToList()),
                    FlagContainer = model.Flags.Select(x => x.ToDto()).ToList(),
                    Wishlist = model.Wishlist.OrderBy(x => x.Id).Select(x => x.ItemName).ToList(),
                    Xp = model.Xp,
                    SocialLevel = model.SocialLevel,
                    SocialXp = model.SocialXp,
                    CurrentBonusDay = currentBonusDay,
                    PreviousBonusDay = previousBonusDay,
                    Orders = model.ToOrdersDto(),
                    LightLevel = model.LightLevel,
                    PaidEnergy = 0, // TODO
                    EnergyModifiers = energyModifiers,
                    ShowNpcCloud = true,
                    StorageComponent = model.ToStorageComponentDto(),
                    AdditionalWareHouseSlots = 0,
                    ActiveQuests = activeQuests.Select(q => q.ToDto(model)).ToList()
                },
                World = model.GetWorld().ToDto(),
                Username = model.Username,
                WorldName = model.GetWorld().WorldName,
                PendingWorldName = model.GetWorld().PendingWorldName,
                WorldSummary = summaryWorlds.ToDictionary(w => w.Type.ToDescriptionString(), w => w.ToWorldSummaryDto(model, activeQuests))
            },
            Franchises = model.Franchises.Select(x => x.ToDto()).ToList(),
            FeatureData = model.ToFeatureDataDto(summaryWorlds),
            PreyGroups = GameSettingsManager.Instance.GetPreyGroups()
                .Select(setting => model.PreyGroups.FirstOrDefault(x => x.GroupId == setting.Id).ToPreyGroupDto(setting.Id))
                .ToList()
        };
    }

    // TODO: switch to DateTimeOffset later
    internal static long ToUnixSeconds(DateTime utc)
    {
        return new DateTimeOffset(DateTime.SpecifyKind(utc, DateTimeKind.Utc)).ToUnixTimeSeconds();
    }
}