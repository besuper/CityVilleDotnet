namespace CityVilleDotnet.Domain.Entities;

using CityVilleDotnet.Common.Settings;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;

public class User
{
    [JsonIgnore]
    public Guid Id { get; private set; }

    [JsonIgnore]
    public Guid UserId { get; private set; }

    [JsonIgnore]
    public ApplicationUser? AppUser { get; private set; }

    [JsonIgnore]
    public List<Quest> Quests { get; private set; } = new List<Quest>();

    [JsonPropertyName("userInfo")]
    public UserInfo? UserInfo { get; set; }

    [JsonPropertyName("Franchises")]
    public List<object> Franchises { get; set; } = new List<object>();

    public void SetupNewPlayer(ApplicationUser user)
    {
        Id = Guid.NewGuid();
        UserId = Guid.Parse(user.Id);
        AppUser = user;

        UserInfo.Id = Guid.NewGuid();
        UserInfo.Username = user.UserName;

        UserInfo.Player.Id = Guid.NewGuid();
        UserInfo.Player.Inventory.Id = Guid.NewGuid();
        UserInfo.Player.Commodities.Id = Guid.NewGuid();

        UserInfo.World.Id = Guid.NewGuid();

        foreach (var item in UserInfo.World.MapRects)
        {
            item.Id = Guid.NewGuid();
        }

        foreach (var item in UserInfo.World.Objects)
        {
            item.Id = Guid.NewGuid();
        }

        // Setup first quest
        Quests.Add(Quest.Create("q_rename_city", 0, 1, QuestType.Active));
    }

    public void ComputeLevel()
    {
        var level = 1;
        var energyMax = 12;
        var currentXP = UserInfo.Player.Xp;

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

        UserInfo.Player.Level = level;
        UserInfo.Player.EnergyMax = energyMax;
    }

    public int GetGold()
    {
        return UserInfo.Player.Gold;
    }

    public int GetExperience()
    {
        return UserInfo.Player.Xp;
    }

    public int GetGoods()
    {
        return UserInfo.Player.Commodities.Storage.Goods;
    }

    public int GetCash()
    {
        return UserInfo.Player.Cash;
    }

    public int GetLevel()
    {
        return UserInfo.Player.Level;
    }

    public World GetWorld()
    {
        return UserInfo.World;
    }

    public void AddXp(int xp)
    {
        UserInfo.Player.Xp += xp;
    }

    public int GetMaxEnergy()
    {
        return UserInfo.Player.EnergyMax;
    }

    public void AddEnergy(int energy)
    {
        if (UserInfo.Player.Energy >= GetMaxEnergy())
        {
            return;
        }

        UserInfo.Player.Energy += energy;
    }

    public void AddGold(int amount)
    {
        UserInfo.Player.Gold += amount;
    }

    public void CompleteTutorial()
    {
        UserInfo.IsNew = false;
    }

    public string SetWorldName(string name)
    {
        var newName = name.Trim();

        UserInfo.WorldName = newName;

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
        UserInfo.Player.Gold += amount;
    }

    public void AddGoods(int amount)
    {
        UserInfo.Player.Commodities.Storage.Goods += amount;
    }

    public void RemoveGoods(int amount)
    {
        UserInfo.Player.Commodities.Storage.Goods -= amount;
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
        UserInfo.Player.RollCounter += 1;
        int rollCounter = UserInfo.Player.RollCounter;

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
}