namespace CityVilleDotnet.Domain.Entities;

public class IncentivizedExpansion
{
    public int Id { get; set; }
    public string ExpansionId { get; private set; }
    public int? X { get; private set; }
    public int? Y { get; private set; }
    public long? StartTimestamp { get; private set; }
    public bool IsCompleted { get; private set; }
    public int FailureCount { get; private set; }

    private IncentivizedExpansion()
    {
    }

    public IncentivizedExpansion(string expansionId)
    {
        ExpansionId = expansionId;
    }

    public void Store(int x, int y, long startTimestamp)
    {
        X = x;
        Y = y;
        StartTimestamp = startTimestamp;
    }

    public void Complete()
    {
        IsCompleted = true;
    }

    public void IncrementFailureCount()
    {
        FailureCount++;
    }

    public bool IsActive()
    {
        return !IsCompleted && X is not null && Y is not null;
    }
}
