using CityVilleDotnet.Common.Global;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.GameEntities;

namespace CityVilleDotnet.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public ApplicationUser? AppUser { get; private set; }
    public List<Quest> Quests { get; private set; } = new List<Quest>();
    public Player? Player { get; private set; }
    public World? World { get; set; }
    public List<object> Franchises { get; set; } = new List<object>();
    public List<Friend> Friends { get; private set; } = new List<Friend>();

    public static User CreateNewPlayer(UserDto defaultValue, ApplicationUser user)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            UserId = Guid.Parse(user.Id),
            AppUser = user,
            Player = new Player
            {
                Id = Guid.NewGuid(),
                Cash = 900,
                Gold = 50000,
                Energy = 12,
                EnergyMax = 12,
                Goods = defaultValue.UserInfo.Player.Commodities.Storage.Goods,
                Username = user.UserName
            },
            World = new World()
            {
                Id = Guid.NewGuid(),
                Population = defaultValue.UserInfo.World.CitySim.Population,
                PopulationCap = defaultValue.UserInfo.World.CitySim.PopulationCap,
                PotentialPopulation = defaultValue.UserInfo.World.CitySim.PotentialPopulation,
                SizeX = defaultValue.UserInfo.World.SizeX,
                SizeY = defaultValue.UserInfo.World.SizeY,
                WorldName = "",
                MapRects = defaultValue.UserInfo.World.MapRects.Select(x => new MapRect()
                {
                    Id = Guid.NewGuid(),
                    Height = x.Height,
                    Width = x.Width,
                    X = x.X,
                    Y = x.Y,
                }).ToList(),
                Objects = defaultValue.UserInfo.World.Objects.Select(x => new WorldObject()
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

    public void ComputeLevel()
    {
        var level = 1;
        var energyMax = 12;
        var currentXp = Player?.Xp;

        if (currentXp is null) return;

        foreach (var item in GameSettingsManager.Instance.GetLevels())
        {
            if (currentXp >= int.Parse(item.RequiredXp))
            {
                level = int.Parse(item.Num);
                energyMax = int.Parse(item.EnergyMax);
            }
            else
            {
                break;
            }
        }

        Player.Level = level;
        Player.EnergyMax = energyMax;
    }

    public void ComputeSocialLevel()
    {
        // FIXME: Give the reward
        var level = 1;
        var currentXp = Player?.SocialXp;

        if (currentXp is null) return;

        foreach (var item in GameSettingsManager.Instance.GetSocialLevels())
        {
            if (currentXp >= int.Parse(item.RequiredXp))
            {
                level = int.Parse(item.Num);
            }
            else
            {
                break;
            }
        }

        Player.SocialLevel = level;
    }

    public int GetGold()
    {
        return Player.Gold;
    }

    public int GetExperience()
    {
        return Player.Xp;
    }

    public int GetGoods()
    {
        return Player.Goods;
    }

    public int GetCash()
    {
        return Player.Cash;
    }

    public int GetLevel()
    {
        return Player.Level;
    }

    public World GetWorld()
    {
        return World;
    }

    public void AddXp(int xp)
    {
        Player.Xp += xp;

        // FIXME: Check user level after each xp
        ComputeLevel();
    }

    public int GetMaxEnergy()
    {
        return Player.EnergyMax;
    }

    public void AddEnergy(int energy)
    {
        if (Player.Energy >= GetMaxEnergy())
        {
            return;
        }

        Player.Energy += energy;
    }

    public void AddGold(int amount)
    {
        Player.Gold += amount;
    }

    public void CompleteTutorial()
    {
        Player.IsNew = false;
        Player.FirstDay = false;

        Player.Uid = $"{Player.Snuid}";
    }

    public string SetWorldName(string name)
    {
        var newName = name.Trim();

        World.WorldName = newName;

        return newName;
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

    public void RemoveCoin(int amount)
    {
        Player.Gold += amount;
    }

    public void AddGoods(int amount)
    {
        Player.Goods += amount;
    }

    public void RemoveGoods(int amount)
    {
        Player.Goods -= amount;
    }

    public void AddSocialXp(int amount)
    {
        Player.SocialXp += amount;

        ComputeSocialLevel();
    }

    // From Player::processRandomModifiersFromConfig
    public List<int> CollectDoobersRewards(string itemName, List<string>? allowedDooberTypes = null)
    {
        if (Player is null) return [];

        var gameItem = GameSettingsManager.Instance.GetItem(itemName);

        if (gameItem?.RandomModifiers?.Modifiers is null) return [];

        var secureRands = new List<int>();

        foreach (var itemModifier in gameItem.RandomModifiers.Modifiers)
        {
            Player.RollCounter += 1;

            var debugName = gameItem.Name;
            var secureRand = SecureRand.GenerateRand(0, 99, Player.RollCounter, Player.Uid);
            
            Console.WriteLine($"SecureRand for {debugName}: rollCounter={Player.RollCounter} => {secureRand}");

            secureRands.Add(secureRand);

            var modifierTable = GameSettingsManager.Instance.GetRandomModifier(itemModifier.TableName);

            if (modifierTable is null) continue;

            Console.WriteLine($"Checking random table named {modifierTable.Name} type {modifierTable.Type} with rand {secureRand}");

            var previousRollPercent = 0;
            var found = false;

            foreach (var roll in modifierTable.Rolls)
            {
                if (roll.Percent > 0)
                {
                    var currentRollPercent = roll.Percent + previousRollPercent;

                    Console.WriteLine($"Percent {currentRollPercent}");

                    if (secureRand < currentRollPercent && !found)
                    {
                        Console.WriteLine("FOUND WITH PERCENT : " + currentRollPercent);
                        Console.WriteLine("SECURE RAND : " + secureRand);

                        foreach (var (key, value) in roll.Rewards)
                        {
                            // FIXME: Implement a better skip
                            if (allowedDooberTypes is not null && !allowedDooberTypes.Contains(key))
                            {
                                Console.WriteLine($"Skipping doober type {key} as it is not allowed");
                                continue;
                            }
                            
                            Console.WriteLine("TYPE : " + key);

                            switch (key)
                            {
                                case "coin":
                                    Console.WriteLine(value.Sum(x => x.Amount));
                                    AddGold((int)value.Sum(x => x.Amount));
                                    break;
                                case "xp":
                                    AddXp((int)value.Sum(x => x.Amount));
                                    Console.WriteLine(value.Sum(x => x.Amount));
                                    break;
                                case "energy":
                                    AddEnergy((int)value.Sum(x => x.Amount));
                                    break;
                                case "collectable":
                                    Console.WriteLine($"Found collectable {string.Join(", ", value.Select(x => x.Name).ToList())}");
                                    foreach (var element in value)
                                    {
                                        var collectionName = GameSettingsManager.Instance.GetCollectionByItemName(element.Name);

                                        if (collectionName is not null)
                                        {
                                            Player.AddItemToCollection(collectionName, element.Name);
                                            Console.WriteLine($"Added {element.Name} to collection {collectionName}");
                                        }
                                        else
                                        {
                                            Console.WriteLine($"Collection for item {element.Name} not found");
                                        }
                                    }

                                    break;
                                case "food":
                                    AddGoods((int)value.Sum(x => x.Amount));
                                    Console.WriteLine((int)value.Sum(x => x.Amount));
                                    break;
                            }
                        }

                        found = true;
                    }

                    previousRollPercent = currentRollPercent;
                }
            }
        }

        return secureRands;
    }

    public List<GameFriendData> GetFriendsData()
    {
        return Friends.Select(friend => friend.ToFriendData()).ToList();
    }

    public void SetSeenFlag(string flag)
    {
        if (!Player.SeenFlags.Any(x => x.Key == flag))
        {
            Player.SeenFlags.Add(new SeenFlag(flag));
        }
    }
}