namespace CityVilleDotnet.Domain.Entities;

public class PreyCapture(int preyId)
{
    public int Id { get; }
    public int PreyId { get; private set; } = preyId;
    public int Count { get; private set; }

    public void Increment()
    {
        Count++;
    }
}
