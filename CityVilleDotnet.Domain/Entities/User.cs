namespace CityVilleDotnet.Domain.Entities;

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
}