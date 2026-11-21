namespace CityVilleDotnet.Domain.Entities;

public class SocialPostHelper
{
    public int Id { get; }
    public Player? Helper { get; private set; }
    public DateTime HelpedAt { get; private set; }
    public bool AckSeen { get; private set; }

    public SocialPostHelper() {}

    public SocialPostHelper(Player helper, DateTime helpedAt)
    {
        Helper = helper;
        HelpedAt = helpedAt;
    }

    public void MarkAckSeen()
    {
        AckSeen = true;
    }
}
