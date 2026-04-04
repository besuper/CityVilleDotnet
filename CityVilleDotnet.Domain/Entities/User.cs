using CityVilleDotnet.Common.Global;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Settings.GameSettings;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Domain.GameEntities;
using Microsoft.Extensions.Logging;

namespace CityVilleDotnet.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Player? Player { get; private set; }

    public User(Guid userId, ApplicationUser appUser, string username, Player player)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Player = player;
    }

    private User()
    {
    }

    public static User CreateNewPlayer(WorldDto defaultValue, ApplicationUser user)
    {
        var mapRects = defaultValue.MapRects.Select(x => new MapRect()
        {
            Height = x.Height,
            Width = x.Width,
            X = x.X,
            Y = x.Y,
        }).ToList();

        var objects = defaultValue.Objects.Select(x => new WorldObject().LoadObject(x)).ToList();

        var world = new World("", 36, 36, 30, 0, 50, 0, 0, mapRects, objects);

        var newPlayer = new Player(user, world);
        newPlayer.SetupNewPlayer(user);

        return new User(Guid.Parse(user.Id), user, user.UserName!, newPlayer);
    }

    public Player GetPlayer()
    {
        if (Player is null) throw new Exception("Player is not loaded");

        return Player;
    }
}