using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.GameEntities;

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
                    Position = new WorldObjectPosition
                    {
                        X = x.Position.X,
                        Y = x.Position.Y,
                        Z = x.Position.Z,
                    },
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
        Quests.Add(Quest.Create("q_rename_city", 0, 1, QuestType.Active));
    }

    public World GetWorld()
    {
        return World;
    }

    public void HandleQuestProgress(string actionType = "", string itemName = "")
    {
        // TODO: Make actionType required, update how quests are checked

        foreach (var quest in Quests.Where(x => x.QuestType == QuestType.Active))
        {
            var questItem = QuestSettingsManager.Instance.GetItem(quest.Name);

            if (questItem is null) continue;

            var index = 0;

            foreach (var task in questItem.Tasks.Tasks)
            {
                if (task.Action.Equals(actionType))
                {
                    quest.Progress[index] += 1;
                }

                if (task.Action.Equals("countPlayerResourceByType"))
                {
                    var count = 0;
                    var ressourceType = task.Type;

                    switch (ressourceType)
                    {
                        case "population":
                            count = GetWorld().GetCurrentPopulation();
                            break;
                        default:
                            break;
                    }

                    quest.Progress[index] = count;
                }

                if (task.Action.Equals("countConstructionOrBuildingByName"))
                {
                    var buildingName = task.Type;

                    quest.Progress[index] = GetWorld().CountBuildingByName(buildingName);
                }

                if (task.Action.Equals("openBusinessByName"))
                {
                    var buildingName = task.Type;

                    quest.Progress[index] = GetWorld().CountOpenedBuildingByName(buildingName);
                }

                if (task.Action.Equals("harvestBusinessByName"))
                {
                    var buildingName = task.Type;

                    // TODO: Check amount

                    if (buildingName.Equals(itemName))
                    {
                        quest.Progress[index] += 1;
                    }
                }

                if (task.Action.Equals("countWorldObjectByName"))
                {
                    var buildingName = task.Type;

                    quest.Progress[index] = GetWorld().CountBuildingByName(buildingName);
                }

                if (task.Action.Equals("harvestByClass")
                    || task.Action.Equals("harvestResidenceByName")
                    || task.Action.Equals("startContractByClass")
                    || task.Action.Equals("clearByClass")
                    || task.Action.Equals("harvestPlotByName")
                    || task.Action.Equals("placeByClass")
                   )
                {
                    var buildingName = task.Type;
                    var amount = int.Parse(task.Total);

                    if (buildingName.Equals(itemName))
                    {
                        quest.Progress[index] += 1;
                    }
                }

                index++;
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
                item.ClaimRewards(this);

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