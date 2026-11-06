using CityVilleDotnet.Common.Settings;

namespace CityVilleDotnet.Domain.Entities;

public enum FriendshipStatus
{
    Pending = 0,
    Accepted = 1
}

public class Friend
{
    public Guid Id { get; private set; }

    public Player? Player { get; private set; }

    public Player? FriendPlayer { get; private set; }

    public FriendshipStatus Status { get; set; }

    public bool Requested { get; set; }

    public int EnergyLeft { get; set; }

    public long LastEnergyLeftReset { get; set; } = 0;

    public Friend(Player user, Player friend, bool requested)
    {
        Id = Guid.NewGuid();
        Player = user;
        FriendPlayer = friend;

        Status = FriendshipStatus.Pending;
        Requested = requested;
        EnergyLeft = GameSettingsManager.Instance.GetSettings().MaxVisitingEnergy;
    }

    public Friend()
    {
    }

    public Player GetPlayer()
    {
        if (Player is null) throw new Exception("Player is not loaded");

        return Player;
    }

    public Player GetFriend()
    {
        if (FriendPlayer is null) throw new Exception("Friend player is not loaded");

        return FriendPlayer;
    }

    public bool CanResetEnergy(long now) => now - LastEnergyLeftReset >= GameSettingsManager.Instance.GetSettings().InGameDaySeconds * 1000L;

    public int GetAvailableEnergy(long now) => CanResetEnergy(now) ? GameSettingsManager.Instance.GetSettings().MaxVisitingEnergy : EnergyLeft;

    public void ResetEnergy(long now)
    {
        EnergyLeft = GameSettingsManager.Instance.GetSettings().MaxVisitingEnergy;
        LastEnergyLeftReset = now;
    }
}