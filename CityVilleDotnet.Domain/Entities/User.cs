namespace CityVilleDotnet.Domain.Entities;

using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.GameEntities;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

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
                Cash = 0,
                Energy = 12,
                EnergyMax = 12,
                Commodities = new Commodities()
                {
                    Id = Guid.NewGuid(),
                    Storage = new Storage()
                    {
                        Goods = defaultValue.UserInfo.Player.Commodities.Storage.Goods
                    }
                },
                Inventory = new Inventory()
                {
                    Id = Guid.NewGuid()
                },
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
                    Position = new WorldObjectPosition()
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
        var currentXP = Player.Xp;

        foreach (var item in GameSettingsManager.Instance.GetLevels())
        {
            if (currentXP >= int.Parse(item.RequiredXp))
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
        return Player.Commodities.Storage.Goods;
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
            // TODO: Support purchased tasks
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
        Player.Commodities.Storage.Goods += amount;
    }

    public void RemoveGoods(int amount)
    {
        Player.Commodities.Storage.Goods -= amount;
    }

    private static string GetMD5Hash(string input)
    {
        using (MD5 md5 = MD5.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("x2"));
            }
            return sb.ToString();
        }
    }

    // From SecureRand::rand
    // FIXME: Looks like secure rand from client and server is not the same
    public int GenerateRand(int min, int max)
    {
        Player.RollCounter += 1;
        int rollCounter = Player.RollCounter;

        string stringToHash = "YOUR_LIKE_AN_8" + "::" + "" + "::" + 333 + "::" + rollCounter;

        int range = max - min + 1;

        string md5Hash = "0x" + GetMD5Hash(stringToHash).Substring(0, 8);
        ulong hashNumber = Convert.ToUInt64(md5Hash, 16);

        int moduloResult = (int)(hashNumber % (ulong)range);
        int result = moduloResult + min;

        return result;
    }

    // From Player::processRandomModifiersFromConfig
    public List<int> CollectDoobersRewards(string itemName)
    {
        var gameItem = GameSettingsManager.Instance.GetItem(itemName);

        if (gameItem is null) return [];
        if (gameItem.RandomModifiers is null) return [];
        if (gameItem.RandomModifiers.Modifiers is null) return [];

        var secureRands = new List<int>();

        foreach (var itemModifier in gameItem.RandomModifiers.Modifiers)
        {
            var secureRand = GenerateRand(0, 99);

            secureRands.Add(secureRand);

            var modifierTable = GameSettingsManager.Instance.GetRandomModifier(itemModifier.TableName);

            if (modifierTable is null) continue;

            var previousRollPercent = 0;
            var found = false;

            foreach (var roll in modifierTable.Rolls)
            {
                var percent = roll.Percent + previousRollPercent;

                previousRollPercent = roll.Percent;

                if (secureRand < percent && !found)
                {
                    Console.WriteLine("FOUND WITH PERCENT : " + percent);
                    Console.WriteLine("SECURE RAND : " + secureRand);

                    foreach (var (key, value) in roll.Rewards)
                    {
                        Console.WriteLine("TYPE : " + key);

                        switch (key)
                        {
                            case "coin":
                                // FIXME: Change coins to double ? There are doubles in amount settings
                                Console.WriteLine(value.Sum(x => x.Amount));
                                AddGold((int)value.Sum(x => x.Amount));
                                break;
                            case "xp":
                                // TODO: XP can be double
                                AddXp((int)value.Sum(x => x.Amount));
                                Console.WriteLine(value.Sum(x => x.Amount));
                                break;
                            case "energy":
                                AddEnergy((int)value.Sum(x => x.Amount));
                                break;
                        }

                        found = true;
                    }
                }
            }
        }

        return secureRands;
    }

    public List<GameFriendData> GetFriendsData()
    {
        var items = new List<GameFriendData>();

        foreach (var friend in Friends)
        {
            items.Add(friend.ToFriendData());
        }

        return items;
    }

    public Inventory? GetInventory()
    {
        return Player?.Inventory;
    }
}