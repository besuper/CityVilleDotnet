using CityVilleDotnet.Domain.GameEntities;

namespace CityVilleDotnet.Domain.Entities;

public enum FriendshipStatus
{
    Pending = 0,
    Accepted = 1
}

public class Friend
{
    public Guid Id { get; private set; }

    public User User { get; private set; }

    public User FriendUser { get; private set; }

    public FriendshipStatus Status { get; set; }

    public bool Requested { get; set; }

    public Friend(User user, User friend, bool requested)
    {
        Id = Guid.NewGuid();
        User = user;
        FriendUser = friend;

        Status = FriendshipStatus.Pending;
        Requested = requested;
    }

    public Friend() { }

    public GameFriendData ToFriendData()
    {
        return new GameFriendData()
        {
            Zid = FriendUser.Player.Uid,
            Snuid = FriendUser.Player.Uid,
            Snid = FriendUser.Player.Uid,
            FirstName = FriendUser.Player.Username,
            Name = FriendUser.Player.Username,
            Picture = "blank.png",
            Gender = "M"
        };
    }
}
