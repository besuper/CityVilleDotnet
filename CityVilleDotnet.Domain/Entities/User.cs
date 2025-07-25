﻿using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.GameEntities;
using FluorineFx;

namespace CityVilleDotnet.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public ApplicationUser? AppUser { get; private set; }
    public List<Quest> Quests { get; } = [];
    public Player? Player { get; private set; }
    public World? World { get; set; }
    public List<object> Franchises { get; set; } = [];
    public List<Friend> Friends { get; } = [];

    public static User CreateNewPlayer(WorldDto defaultValue, ApplicationUser user)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            UserId = Guid.Parse(user.Id),
            AppUser = user,
            Player = new Player(user.UserName!),
            World = new World
            {
                Id = Guid.NewGuid(),
                Population = 2,
                PopulationCap = 0,
                PotentialPopulation = 0,
                SizeX = 36,
                SizeY = 36,
                WorldName = "",
                MapRects = defaultValue.MapRects.Select(x => new MapRect()
                {
                    Id = Guid.NewGuid(),
                    Height = x.Height,
                    Width = x.Width,
                    X = x.X,
                    Y = x.Y,
                }).ToList(),
                Objects = defaultValue.Objects.Select(x => new WorldObject()
                {
                    Id = Guid.NewGuid(),
                    Builds = x.Builds,
                    BuildTime = x.BuildTime,
                    ClassName = x.ClassName,
                    ContractName = x.ContractName,
                    Deleted = x.Deleted,
                    Direction = x.Direction,
                    FinishedBuilds = x.FinishedBuilds,
                    ItemName = x.ItemName,
                    PlantTime = x.PlantTime,
                    X = x.Position.X,
                    Y = x.Position.Y,
                    Z = x.Position.Z,
                    Stage = x.Stage,
                    State = x.State,
                    TargetBuildingClass = x.TargetBuildingClass,
                    TargetBuildingName = x.TargetBuildingName,
                    TempId = x.TempId,
                    WorldFlatId = x.WorldFlatId,
                }).ToList()
            }
        };
    }

    public void SetupNewPlayer(ApplicationUser user)
    {
        // Setup first quest
        Quests.Add(Quest.Create("q_rename_city", 1, QuestType.Active));
    }

    public World GetWorld()
    {
        return World;
    }

    public void HandleQuestsProgress(string actionType, string? className = null, string? itemName = null)
    {
        foreach (var quest in Quests.Where(x => x.QuestType == QuestType.Active))
        {
            // FIXME: Optimize this, it is called too often
            var questItem = QuestSettingsManager.Instance.GetItem(quest.Name);

            if (questItem is null) continue;

            var index = -1;

            foreach (var task in questItem.Tasks.Tasks)
            {
                index++;

                var actionTask = task.Action;
                var taskType = task.Type;
                var splitType = taskType.Contains(',') ? taskType.Split(',') : null;

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
                            continue;
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

                            continue;
                        }
                        case "harvestResidenceByName":
                        case "harvestPlotByName":
                        case "openBusinessByName":
                        case "harvestBusinessByName":
                        {
                            if (itemName is null)
                                throw new Exception("Can't validate byName action without itemName");

                            if (task.Type.Equals(itemName) || (splitType is not null && splitType.Contains(itemName)))
                                quest.Progress[index] += 1;

                            continue;
                        }
                    }
                }

                // Here we can check global values like counting population or buildings

                // FIXME: countConstructionOrBuildingByName
                if (actionTask.Equals("countWorldObjectByName") || actionTask.Equals("countConstructionOrBuildingByName"))
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

                if (actionTask.Equals("countPlayerResourceByType"))
                {
                    switch (task.Type)
                    {
                        case "population":
                        {
                            quest.Progress[index] = GetWorld().GetCurrentPopulation();
                            continue;
                        }
                    }
                }

                if (actionTask.Equals("countCollectableByName"))
                {
                    quest.Progress[index] = Player!.CountCollectableByName(task.Type);
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

    public List<GameFriendData> GetFriendsData()
    {
        return Friends.Select(friend => friend.ToFriendData()).ToList();
    }
}