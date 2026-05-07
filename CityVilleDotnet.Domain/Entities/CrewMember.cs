namespace CityVilleDotnet.Domain.Entities;

public class CrewMember
{
    public int Id { get; set; }
    public Player? Player { get; set; } // If player is null means fake player => -1

    private CrewMember()
    {
    }

    public CrewMember(Player? player)
    {
        Player = player;
    }
}