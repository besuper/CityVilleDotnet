﻿namespace CityVilleDotnet.Api.Services.UserService.Domain;

using CityVilleDotnet.Api.Common.Domain;
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

    [JsonPropertyName("userInfo")]
    public UserInfo? UserInfo { get; set; }

    [JsonPropertyName("Franchises")]
    public List<object> Franchises { get; set; } = new List<object>();

    internal void SetupNewPlayer(ApplicationUser user)
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
    }

    internal int GetGold()
    {
        return UserInfo.Player.Gold;
    }

    internal int GetExperience()
    {
        return UserInfo.Player.Xp;
    }

    internal int GetGoods()
    {
        return UserInfo.Player.Commodities.Storage.Goods;
    }

    internal int GetCash()
    {
        return UserInfo.Player.Cash;
    }

    internal int GetLevel()
    {
        return UserInfo.Player.Level;
    }

    internal World GetWorld()
    {
        return UserInfo.World;
    }

    internal void AddGold(int amount)
    {
        UserInfo.Player.Gold += amount;
    }

    internal void CompleteTutorial()
    {
        UserInfo.IsNew = false;
    }
}