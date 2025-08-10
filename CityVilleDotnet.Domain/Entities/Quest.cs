using CityVilleDotnet.Common.Settings;
using System.Text.Json.Serialization;

namespace CityVilleDotnet.Domain.Entities;

public class Quest
{
    public Quest(string name, int[] progress, int[] purchased, QuestType questType)
    {
        Id = Guid.NewGuid();
        Name = name;
        Progress = progress;
        Purchased = purchased;
        QuestType = questType;
    }

    public Quest()
    {
    }

    public Guid Id { get; private set; }

    public string Name { get; set; }

    public int[] Progress { get; set; }

    public int[] Purchased { get; set; }

    public QuestType QuestType { get; set; }

    public static Quest Create(string name, int length, QuestType questType)
    {
        return new Quest(name, new int[length], new int[length], questType);
    }

    public bool IsCompleted()
    {
        var questItem = QuestSettingsManager.Instance.GetItem(Name);

        if (questItem is null) return false;

        var completed = true;

        for (var i = 0; i < Progress.Length; i++)
        {
            if ((Progress[i] + Purchased[i]) < int.Parse(questItem.Tasks.Tasks[i].Total))
            {
                completed = false;
                break;
            }
        }

        return completed;
    }

    public void ClaimRewards(Player player)
    {
        var questItem = QuestSettingsManager.Instance.GetItem(Name);

        if (questItem?.ResourceModifiers?.Rewards is null) return;

        foreach (var reward in questItem.ResourceModifiers.Rewards)
        {
            if (reward.Gold is not null)
                player.AddCoins(int.Parse(reward.Gold));

            if (reward.Xp is not null)
                player.AddXp(int.Parse(reward.Xp));

            if (reward.Goods is not null)
                player.AddGoods(int.Parse(reward.Goods));

            if (reward.Item is not null)
                player.AddItem(reward.Item);

            if (reward.ItemUnlock is not null)
                player.SetSeenFlag(reward.ItemUnlock);

            if (reward.Energy is not null)
                player.AddEnergy(int.Parse(reward.Energy));
        }
    }

    public List<Quest> StartSequels()
    {
        var sequels = new List<Quest>();
        var questItem = QuestSettingsManager.Instance.GetItem(Name);

        if (questItem?.Sequels?.Sequels is null) return sequels;

        foreach (var sequel in questItem.Sequels.Sequels)
        {
            var sequelItem = QuestSettingsManager.Instance.GetItem(sequel.Name);

            if (sequelItem is null) continue;

            // TODO: Add support for pending tasks
            var newQuest = Create(sequelItem.Name, sequelItem.Tasks.Tasks.Count, QuestType.Active);

            sequels.Add(newQuest);
        }

        return sequels;
    }

    public void PurchaseProgression(int index)
    {
        var questItem = QuestSettingsManager.Instance.GetItem(Name);

        if (questItem is null) return;

        // TODO: Check if it's ok
        var requiredAmount = int.Parse(questItem.Tasks.Tasks[index].Total);

        Purchased[index] = requiredAmount;
    }
}