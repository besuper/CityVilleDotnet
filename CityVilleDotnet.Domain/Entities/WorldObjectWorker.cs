namespace CityVilleDotnet.Domain.Entities;

public class WorldObjectWorker
{
    public int Id { get; set; }
    public int Zid { get; private set; }

    private WorldObjectWorker()
    {
    }

    public WorldObjectWorker(int zid)
    {
        Zid = zid;
    }

    public bool IsPurchased()
    {
        return Zid < 0;
    }
}
