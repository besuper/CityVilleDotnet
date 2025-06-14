using CityVilleDotnet.Common.Settings;
using System.Text.Json.Serialization;

namespace CityVilleDotnet.Domain.Entities;

public class Quest
{
    public Quest(string name, int complete, int[] progress, int[] purchased, QuestType questType)
    {
        Id = Guid.NewGuid();
        Name = name;
        Complete = complete;
        Progress = progress;
        Purchased = purchased;
        QuestType = questType;
    }

    public Quest()
    {

    }

    [JsonIgnore]
    public Guid Id { get; private set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("complete")]
    public int Complete { get; set; }

    [JsonPropertyName("progress")]
    public int[] Progress { get; set; }

    [JsonPropertyName("purchased")]
    public int[] Purchased { get; set; }

    [JsonIgnore]
    public QuestType QuestType { get; set; }

    public static Quest Create(string name, int complete, int length, QuestType questType)
    {
        return new Quest(name, complete, new int[length], new int[length], questType);
    }

    public bool IsCompleted()
    {
        var questItem = QuestSettingsManager.Instance.GetItem(Name);

        if (questItem is null) return false;

        var completed = true;

        for (var i = 0; i < Progress.Length; i++)
        {
            if (Progress[i] < int.Parse(questItem.Tasks.Tasks[i].Total))
            {
                completed = false;
                break;
            }
        }

        return completed;
    }

    public void ClaimRewards(User user)
    {
        var questItem = QuestSettingsManager.Instance.GetItem(Name);

        if (questItem is null) return;
        if (questItem.ResourceModifiers is null) return;
        if (questItem.ResourceModifiers.Rewards is null) return;

        foreach (var reward in questItem.ResourceModifiers.Rewards)
        {
            if (reward.Gold is not null)
            {
                user.AddGold(int.Parse(reward.Gold));
            }

            if (reward.Xp is not null)
            {
                user.AddXp(int.Parse(reward.Xp));
            }

            if (reward.Goods is not null)
            {
                user.AddGoods(int.Parse(reward.Goods));
            }

            if (reward.Item is not null)
            {
                var inventory = user.GetInventory();

                inventory?.AddItem(reward.Item);
            }

            if (reward.ItemUnlock is not null)
            {
                user.SetSeenFlag(reward.ItemUnlock);
            }

            // TODO: Support energy (add energy engine)
        }
    }

    public List<Quest> StartSequels()
    {
        var sequels = new List<Quest>();
        var questItem = QuestSettingsManager.Instance.GetItem(Name);

        if (questItem is null) return sequels;

        if (questItem.Sequels is null) return sequels;
        if (questItem.Sequels.Sequels is null) return sequels;

        foreach (var sequel in questItem.Sequels.Sequels)
        {
            var sequelItem = QuestSettingsManager.Instance.GetItem(sequel.Name);

            if (sequelItem is null) continue;

            // TODO: Add support for pending tasks
            var newQuest = Quest.Create(sequelItem.Name, 0, sequelItem.Tasks.Tasks.Count, QuestType.Active);

            sequels.Add(newQuest);
        }

        return sequels;
    }
}
