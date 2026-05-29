using CityVilleDotnet.Domain.Entities;
using System.Text.Json.Serialization;
using CityVilleDotnet.Domain.EnumExtensions;
using CityVilleDotnet.Domain.Enums;
using FluorineFx;

namespace CityVilleDotnet.Domain.GameEntities;

public class UserDto
{
    [JsonPropertyName("userInfo")] public required UserInfoDto UserInfo { get; set; }
    [JsonPropertyName("franchises")] public List<FranchiseDto> Franchises { get; set; } = new();
    [JsonPropertyName("featureData")] public required ASObject FeatureData { get; set; }
    [JsonPropertyName("crews")] public Dictionary<string, List<string>> Crews { get; set; } = [];
}

public static class UserDtoMapper
{
    public static UserDto ToDto(this Player model)
    {
        var activeQuests = model.Quests
            .Where(q => q.QuestType == QuestType.Active)
            .ToList();

        var energyModifiers = new Dictionary<string, int>();

        foreach (var obj in model.GetWorld().Objects.Where(o => o.StreakLength > 0))
        {
            energyModifiers[$"{obj.WorldFlatId}"] = obj.StreakLength;
        }

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
                LastPlayedWorldId = model.LastPlayedWorldType.ToDescriptionString(),
                Player = new PlayerDto
                {
                    Uid = model.Snuid.ToString(),
                    Cash = model.Cash,
                    Collections = new ASObject(model.Collections
                        .GroupBy(item => item.Name)
                        .ToDictionary(
                            group => group.Key, object (group) => new ASObject(
                                group.SelectMany(x => x.Items).ToDictionary(x => x.Name, x => (object)x.Amount))
                        )),
                    Commodities = new CommoditiesDto
                    {
                        Storage = new StorageDto
                        {
                            Goods = model.Goods,
                            PremiumGoods = model.PremiumGoods
                        }
                    },
                    CompletedCollections = new ASObject(model.Collections.Where(x => x.Completed > 0).ToDictionary(x => x.Name, x => (object)x.Completed)),
                    Energy = model.Energy,
                    EnergyMax = model.EnergyMax,
                    LastEnergyCheck = model.GetLastCheckEnergyTimestamp(),
                    ExpansionsPurchased = model.ExpansionsPurchased,
                    Gold = model.Gold,
                    Inventory = new InventoryDto
                    {
                        Count = model.CountInventoryItems(),
                        Items = new ASObject(model.InventoryItems.ToDictionary(x => x.Name, x => (object)x.Amount))
                    },
                    LastTrackingTimestamp = model.LastTrackingTimestamp.ToUnixTimeMilliseconds(),
                    Level = model.Level,
                    Licenses = new ASObject(model.Licenses.ToDictionary(x => x.Name, x => (object)x.Amount)),
                    Neighbors = model.Friends
                        .Where(f => !f.FriendPlayer.IsSamantha() && f.Status == FriendshipStatus.Accepted)
                        .Select(friend => friend.ToNeighborDto()).ToList(), // TODO: Change this after moving friends to player
                    Options = new OptionsDto
                    {
                        MusicDisabled = model.MusicDisabled,
                        SfxDisabled = model.SfxDisabled,
                    },
                    PlayerNews = [], // TODO: Implement news
                    RollCounter = model.RollCounter,
                    SeenFlags = new ASObject(model.SeenFlags.ToDictionary(x => x.Key, x => (object)true)),
                    // FIXME: Handle that better
                    FlagContainer =
                    [
                        new ASObject
                        {
                            ["name"] = "completed_bridge",
                            ["m_value"] = model.GetWorld().Objects.Count(x => x.ClassName == BuildingClassType.Bridge),
                            ["lastModifiedGlobalEngineTime"] = 0
                        }
                    ],
                    Wishlist = [], // TODO: Implement wishlist
                    Xp = model.Xp,
                    SocialLevel = model.SocialLevel,
                    SocialXp = model.SocialXp,
                    Orders = BuildOrdersAsObject(model),
                    LightLevel = 0, // TODO
                    PaidEnergy = 0, // TODO
                    EnergyModifiers = energyModifiers,
                    FeatureData = new ASObject(new Dictionary<string, object>()),
                    ShowNpcCloud = true,
                    StorageComponent = model.ToStorageComponentDto(),
                    AdditionalWareHouseSlots = 0,
                    ActiveQuests = activeQuests.Select(q => q.ToDto()).ToList()
                },
                World = model.GetWorld().ToDto(),
                Username = model.Username,
                WorldName = model.GetWorld().WorldName,
                // This fix null in setFinishedWorldFTUE after tutorial
                WorldSummary = new ASObject(new Dictionary<string, object>()
                {
                    {
                        model.GetWorld().Type.ToDescriptionString(), new ASObject(new Dictionary<string, object>()
                        {
                            { "world_id", model.GetWorld().Type.ToDescriptionString() },
                            { "ftueCompleted", !model.IsNew },
                            {
                                "items_by_name", model.GetWorld().Objects
                                    .Where(x => x.ClassName != BuildingClassType.ConstructionSite)
                                    .GroupBy(x => x.ItemName)
                                    .ToDictionary(g => g.Key, g => g.Count())
                            },
                            {
                                "construction_items", model.GetWorld().Objects
                                    .Where(x => x.ClassName == BuildingClassType.ConstructionSite && x.TargetBuildingName != null)
                                    .GroupBy(x => x.TargetBuildingName)
                                    .ToDictionary(g => g.Key, g => g.Count())
                            },
                            { "malls_items", new ASObject() }, // TODO: Implement containers
                            { "incentivized_expansion", new ASObject() }, // TODO: Implement specials expansions
                            { "numberOfExpansions", model.ExpansionsPurchased },
                            { "number_of_business", model.GetWorld().Objects.Count(x => x.ClassName == BuildingClassType.Business) },
                            { "populationSummary", model.GetWorld().ToPopulationSummaryDto() },
                            {
                                "appraisalSummary", new ASObject() // TODO: Implement appraisal (not used in world_main)
                                {
                                    { "id", null },
                                    { "yield", 0 },
                                    { "capacity", 0 },
                                    { "potential", 0 },
                                }
                            },
                            { "commoditySummary", model.GetWorld().ToCommoditySummaryDto() },
                            {
                                "savedQuestSequence", activeQuests
                                    .Where(q => q.Location == QuestLocation.Sidebar)
                                    .OrderBy(q => q.Order)
                                    .Select(q => q.Name)
                                    .ToList()
                            },
                            {
                                "questManagerQuests", activeQuests
                                    .Where(q => q.Location == QuestLocation.Menu)
                                    .OrderBy(q => q.Order)
                                    .Select(q => q.Name)
                                    .ToList()
                            },
                            {
                                "hiddenQuests", activeQuests
                                    .Where(q => q.Location == QuestLocation.Hidden)
                                    .OrderBy(q => q.Order)
                                    .Select(q => q.Name)
                                    .ToList()
                            },
                        })
                    }
                })
            },
            Franchises = model.Franchises.Select(x => x.ToDto()).ToList(),
            FeatureData = new ASObject(new Dictionary<string, object>()
            {
                {
                    "rewardForSpend", new ASObject()
                    {
                        { "version", 0 },
                        {
                            "versionedData", new ASObject()
                            {
                                { "initialAmount", 0 },
                                { "progress", 0 },
                                { "targetAmount", 0 }
                            }
                        }
                    }
                },
                {
                    "cityAtNight", new ASObject()
                    {
                        { "nightModeAvailable", true },
                        { "earlyUnlocked", true },
                        { "cityLightCount", 0 },
                        { "active", false },
                        { "numExpansionsPurchased", model.ExpansionsPurchased },
                    }
                },
                { "remodel", new ASObject() { { "enabled", false } } },
                { "gardens", new ASObject() },
                {
                    "incentivizedExpansions", new ASObject()
                    {
                        { "expansions", new ASObject() },
                        { "cellToId", new ASObject() },
                        { "parentExpansions", new ASObject() },
                        { "failureCount", new ASObject() },
                    }
                },
                { "helperClicks", new ASObject() },
                { "viralAck", new ASObject() },
                { "matchup", new ASObject() },
                { "trickOrTreat", new ASObject() },
                { "poll", new ASObject() },
                { "itemCounts", new ASObject() },
                { "rollCall", false },
                {
                    "goal", new ASObject
                    {
                        {
                            "mastery", model.Masteries.ToDictionary(x => x.ItemName, x => new ASObject
                            {
                                { "count", x.Count },
                                { "level", x.Level },
                            })
                        }
                    }
                },
                {
                    "weather", new ASObject()
                    {
                        {
                            "supportedWeather", new ASObject()
                            {
                                { "0", 0 },
                                { "1", 1 },
                            }
                        }
                    }
                },
                {
                    "leaderboards", new ASObject
                    {
                        { "summaries", new ASObject() }
                    }
                },

                // Prey groups
                // FIXME: load them from gameSettings
                { "copsNBandits", new ASObject { { "workers", new ASObject() } } },
                { "animalRescue", new ASObject { { "workers", new ASObject() } } },
                { "downtownpolice", new ASObject { { "workers", new ASObject() } } },
                { "fishing", new ASObject { { "workers", new ASObject() } } },
                { "area51", new ASObject { { "workers", new ASObject() } } },
                { "trains", new ASObject { { "workers", new ASObject() } } },
                { "factories", new ASObject { { "workers", new ASObject() } } }, // TODO: Implement hiring workers
                { "detectiveGameWorkerManager", new ASObject { { "workers", new ASObject() } } },
                { "socialInventory", new ASObject { { "samObjectIds", new ASObject() } } }
            }), // Enable or disable some features for the user
        };
    }

    private static ASObject BuildOrdersAsObject(Player model)
    {
        var root = new ASObject();

        // TODO: Add VisitorHelp and TrainOrder
        foreach (var order in model.LotOrders.Where(x => x.OrderState == OrderState.Pending))
        {
            var orderTypeKey = order.OrderType.ToDescriptionString(); // "order_lot"
            var transmissionKey = order.TransmissionStatus.ToDescriptionString(); // "sent"/"received"
            var stateKey = order.OrderState.ToDescriptionString(); // "pending"/"accepted"/"denied"

            var isReceived = transmissionKey == "received";
            var otherUserId = isReceived ? $"{order.SenderId}" : $"{order.RecipientId}";

            if (!root.ContainsKey(orderTypeKey))
                root[orderTypeKey] = new ASObject();

            var byTransmission = (ASObject)root[orderTypeKey]!;

            if (!byTransmission.ContainsKey(transmissionKey))
                byTransmission[transmissionKey] = new ASObject();

            var byState = (ASObject)byTransmission[transmissionKey]!;

            if (!byState.ContainsKey(stateKey))
                byState[stateKey] = new ASObject();

            var byOtherUser = (ASObject)byState[stateKey]!;

            if (!byOtherUser.ContainsKey(otherUserId))
                byOtherUser[otherUserId] = new ASObject();

            var orderParams = new ASObject
            {
                ["senderID"] = order.SenderId,
                ["recipientID"] = order.RecipientId,
                ["timeSent"] = order.TimeSent,
                ["lastTimeReminded"] = order.LastTimeReminded,
                ["orderType"] = orderTypeKey,
                ["orderState"] = stateKey,
                ["transmissionStatus"] = transmissionKey,

                ["lotId"] = order.LotId,
                ["resourceType"] = order.ResourceType,
                ["orderResourceName"] = order.OrderResourceName,
                ["constructionCount"] = order.ConstructionCount,
                ["offsetX"] = order.OffsetX,
                ["offsetY"] = order.OffsetY
            };

            byOtherUser[otherUserId] = orderParams;
        }

        foreach (var order in model.VisitorHelpOrders)
        {
            var orderTypeKey = order.OrderType.ToDescriptionString(); // "order_lot"
            var transmissionKey = order.TransmissionStatus.ToDescriptionString(); // "sent"/"received"
            var stateKey = order.OrderState.ToDescriptionString(); // "pending"/"accepted"/"denied"

            var isReceived = transmissionKey == "received";
            var otherUserId = isReceived ? $"{order.SenderId}" : $"{order.RecipientId}";

            if (!root.ContainsKey(orderTypeKey))
                root[orderTypeKey] = new ASObject();

            var byTransmission = (ASObject)root[orderTypeKey]!;

            if (!byTransmission.ContainsKey(transmissionKey))
                byTransmission[transmissionKey] = new ASObject();

            var byState = (ASObject)byTransmission[transmissionKey]!;

            if (!byState.ContainsKey(stateKey))
                byState[stateKey] = new ASObject();

            var byOtherUser = (ASObject)byState[stateKey]!;

            if (!byOtherUser.ContainsKey(otherUserId))
                byOtherUser[otherUserId] = new ASObject();

            var orderParams = new ASObject
            {
                ["senderID"] = order.SenderId,
                ["recipientID"] = order.RecipientId,
                ["timeSent"] = order.TimeSent,
                ["lastTimeReminded"] = order.LastTimeReminded,
                ["orderType"] = orderTypeKey,
                ["orderState"] = stateKey,
                ["transmissionStatus"] = transmissionKey,

                ["helpTargets"] = order.HelpTargets,
                ["status"] = order.Status.ToDescriptionString()
            };

            byOtherUser[otherUserId] = orderParams;
        }

        return root;
    }
}