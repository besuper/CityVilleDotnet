using CityVilleDotnet.Domain.Enums;

namespace CityVilleDotnet.Domain.Entities;

public class PreyHunter
{
    private const int SleepToleranceSeconds = 60;

    public int Id { get; set; }
    public int Position { get; private set; }
    public HunterState State { get; private set; }
    public long Timestamp { get; private set; }

    private PreyHunter()
    {
    }

    public PreyHunter(int position, long now)
    {
        Position = position;
        State = HunterState.Patrolling;
        Timestamp = now;
    }

    public string GetZid() => $"i-{Position + 1}";

    public bool IsUsable(int sleepTime, long now) => State == HunterState.Patrolling || Timestamp + sleepTime <= now + SleepToleranceSeconds;

    public void Sleep(long now)
    {
        State = HunterState.Sleeping;
        Timestamp = now;
    }

    public void WakeUp(long now)
    {
        State = HunterState.Patrolling;
        Timestamp = now;
    }
}
