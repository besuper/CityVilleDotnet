using CityVilleDotnet.Common.Global;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Domain.GameEntities;
using Microsoft.Extensions.Logging;

namespace CityVilleDotnet.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public ApplicationUser? AppUser { get; private set; }
    public List<Quest> Quests { get; } = [];
    public Player? Player { get; private set; }
    public World? World { get; private set; }
    public List<Friend> Friends { get; } = [];

    public User(Guid userId, ApplicationUser appUser, string username, World world)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        AppUser = appUser;
        Player = new Player(username);
        World = world;
    }

    private User()
    {
    }

    public static User CreateNewPlayer(WorldDto defaultValue, ApplicationUser user)
    {
        var mapRects = defaultValue.MapRects.Select(x => new MapRect()
        {
            Height = x.Height,
            Width = x.Width,
            X = x.X,
            Y = x.Y,
        }).ToList();

        var objects = defaultValue.Objects.Select(x => new WorldObject().LoadObject(x)).ToList();

        var world = new World("", 36, 36, 30, 0, 50, 0, 0, mapRects, objects);

        return new User(Guid.Parse(user.Id), user, user.UserName!, world);
    }

    public void SetupNewPlayer(ApplicationUser user)
    {
        // Setup first quest
        Quests.Add(Quest.Create("q_rename_city", 1, QuestType.Active));
    }

    public World GetWorld()
    {
        if (World is null) throw new Exception("GetWorld called on not loaded world");

        return World;
    }

    public bool IsWorldLoaded()
    {
        return World != null && World.Objects.Count != 0;
    }

    public void HandleQuestsProgress(string actionType, string? className = null, string? itemName = null)
    {
        StaticLogger.Current.LogDebug("Handle quest actionType = {ActionType}, className = {ClassName}, itemName = {ItemName}", actionType, className, itemName);

        foreach (var quest in Quests.Where(x => x.QuestType == QuestType.Active))
        {
            var questItem = QuestSettingsManager.Instance.GetItem(quest.Name);

            if (questItem is null) continue;

            var index = -1;

            foreach (var task in questItem.Tasks.Tasks)
            {
                index++;

                if (quest.Progress[index] + quest.Purchased[index] >= int.Parse(task.Total)) continue;

                var actionTask = task.Action;
                var taskType = task.Type ?? "";
                var splitType = taskType.Contains(',') ? taskType.Split(',') : null;

                var gameItem = itemName is not null ? GameSettingsManager.Instance.GetItem(itemName) : null;

                // When user performs an action
                if (!string.IsNullOrEmpty(actionType) && actionTask.Equals(actionType))
                {
                    switch (actionType)
                    {
                        case "seenQuest":
                        case "popNews":
                        case "sendTrain":
                        case "welcomeTrain":
                        case "neighborVisit":
                        case "onValidCityName":
                            quest.Progress[index] += 1;
                            break;
                        case "harvestByClass":
                        case "startContractByClass":
                        case "placeByClass":
                        case "harvestBusinessByClass":
                        case "clearByClass":
                        {
                            if (className is null)
                                throw new Exception("Can't validate byClass action without className");

                            if (task.Type.Equals(className))
                                quest.Progress[index] += 1;

                            break;
                        }
                        case "harvestResidenceByName":
                        case "harvestPlotByName":
                        case "openBusinessByName":
                        case "harvestBusinessByName":
                        case "placeBuildingByName":
                        case "sendTourNeighborBusinessByName":
                        {
                            if (itemName is null)
                                throw new Exception("Can't validate byName action without itemName");

                            if (task.Type.Equals(itemName) || (splitType is not null && splitType.Contains(itemName)))
                                quest.Progress[index] += 1;

                            break;
                        }
                        case "placeByKeyword":
                            if (itemName is null)
                                throw new Exception("Can't validate placeByKeyword action without itemName");

                            if (gameItem is null)
                                throw new Exception("Can't validate placeByKeyword action without gameItem");

                            if (gameItem.HasKeyword(task.Type))
                                quest.Progress[index] += 1;

                            break;
                        case "visitorHelp":
                            // plotHarvest, businessSendTour, ...
                            if (task.Type == className)
                                quest.Progress[index] += 1;

                            break;
                    }
                }

                // Here we can check global values like counting population or buildings

                if (!IsWorldLoaded()) continue;

                switch (actionTask)
                {
                    // FIXME: countConstructionOrBuildingByName
                    case "countWorldObjectByName":
                    case "countConstructionOrBuildingByName":
                    {
                        if (splitType is null)
                        {
                            quest.Progress[index] = GetWorld().CountBuildingByName(task.Type);
                        }
                        else
                        {
                            //bus_toyota1_zyngage,bus_toyota1_zyngage_2,bus_toyota1_zyngage_3
                            quest.Progress[index] = splitType.Sum(x => GetWorld().CountBuildingByName(x));
                        }

                        continue;
                    }
                    case "countWorldObjectByRegEx":
                    {
                        quest.Progress[index] = GetWorld().CountBuildingByRegex(task.Type);
                        continue;
                    }
                    case "countPlayerResourceByType":
                        quest.Progress[index] = task.Type switch
                        {
                            // population,ghost
                            "population" => GetWorld().GetCurrentPopulation(),
                            "coin" => Player!.Gold,
                            "goods" => Player!.Goods,
                            _ => 0
                        };

                        break;
                    case "countCollectableByName":
                        quest.Progress[index] = Player!.CountCollectableByName(task.Type);
                        break;
                }
            }
        }
    }

    public void CheckCompletedQuests()
    {
        var newQuests = new List<Quest>();

        foreach (var item in Quests.Where(x => x.QuestType == QuestType.Active))
        {
            if (item.IsCompleted())
            {
                item.QuestType = QuestType.Completed;
                item.ClaimRewards(Player!);

                newQuests = item.StartSequels();
            }
        }

        Quests.AddRange(newQuests);
    }

    public List<SocialNetworkUserDto> GetSocialNetworkUserFriendsList(string baseUrl)
    {
        return Friends.Select(friend => friend.ToSocialNetworkUserDto(baseUrl)).ToList();
    }
}