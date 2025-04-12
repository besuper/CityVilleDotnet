namespace CityVilleDotnet.Domain.Entities;

using CityVilleDotnet.Common.Settings;
using System.Collections.Generic;
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

    public void HandleQuestProgress(string actionType = "")
    {
        foreach (var quest in Quests.Where(x => x.QuestType == QuestType.Active))
        {
            var questItem = QuestSettingsManager.Instance.GetItem(quest.Name);

            if (questItem is null) continue;

            var index = 0;

            foreach (var task in questItem.Tasks.Tasks)
            {
                if (task.Action.Equals(actionType))
                {
                    quest.Progress[index] = 1;
                }

                if (task.Action.Equals("countPlayerResourceByType"))
                {
                    var completed = false;
                    var ressourceType = task.Type;
                    var amount = int.Parse(task.Total);

                    switch (ressourceType)
                    {
                        case "population":
                            completed = GetWorld().GetCurrentPopulation() >= amount;
                            break;
                        default:
                            break;
                    }

                    if (completed)
                    {
                        quest.Progress[index] = 1;
                    }
                }

                if (task.Action.Equals("countConstructionOrBuildingByName"))
                {
                    var buildingName = task.Type;
                    var amount = int.Parse(task.Total);

                    if (GetWorld().CountBuildingByName(buildingName) >= amount)
                    {
                        quest.Progress[index] = 1;
                    }
                }

                if (task.Action.Equals("openBusinessByName"))
                {
                    var buildingName = task.Type;
                    var amount = int.Parse(task.Total);

                    if (GetWorld().CountOpenedBuildingByName(buildingName) >= amount)
                    {
                        quest.Progress[index] = 1;
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

            var completed = true;

            for (var i = 0; i < item.Progress.Length; i++)
            {
                if (item.Progress[i] == 0)
                {
                    completed = false;
                    break;
                }
            }

            if (completed)
            {
                item.QuestType = QuestType.Completed;

                var questItem = QuestSettingsManager.Instance.GetItem(item.Name);

                if (questItem is null) continue;

                foreach (var sequel in questItem.Sequels.Sequels)
                {
                    var sequelItem = QuestSettingsManager.Instance.GetItem(sequel.Name);

                    if (sequelItem is null) continue;

                    // TODO: Add support for pending tasks
                    var newQuest = Quest.Create(sequelItem.Name, 0, sequelItem.Tasks.Tasks.Count, QuestType.Active);

                    newQuests.Add(newQuest);
                }
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
}