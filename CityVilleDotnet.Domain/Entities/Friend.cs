namespace CityVilleDotnet.Domain.Entities;

public enum FriendshipStatus
{
    Pending = 0,
    Accepted = 1
}

public class Friend
{
    public Guid Id { get; private set; }

    public Player Player { get; private set; }

    public Player FriendPlayer { get; private set; }

    public FriendshipStatus Status { get; set; }

    public bool Requested { get; set; }

    public int EnergyLeft { get; set; } = 5;
    
    public long LastEnergyLeftReset { get; set; } = 0;

    public Friend(Player user, Player friend, bool requested)
    {
        Id = Guid.NewGuid();
        Player = user;
        FriendPlayer = friend;

        Status = FriendshipStatus.Pending;
        Requested = requested;
    }

    public Friend() { }
}
