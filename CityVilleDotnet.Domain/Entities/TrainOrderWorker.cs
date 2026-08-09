namespace CityVilleDotnet.Domain.Entities;

public class TrainOrderWorker
{
    public int Id { get; set; }
    public int Zid { get; private set; }

    private TrainOrderWorker()
    {
    }

    public TrainOrderWorker(int zid)
    {
        Zid = zid;
    }

    public bool IsPurchased()
    {
        return Zid < 0;
    }
}
