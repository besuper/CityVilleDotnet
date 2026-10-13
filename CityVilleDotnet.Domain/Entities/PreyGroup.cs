using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Common.Settings.GameSettings;
using CityVilleDotnet.Domain.Enums;

namespace CityVilleDotnet.Domain.Entities;

public class PreyGroup
{
    public int Id { get; set; }
    public string GroupId { get; private set; } = string.Empty;
    public int NumPreysCaptured { get; private set; }
    public int GateUnlockLevel { get; private set; } = 1;
    public long LastQueueRefill { get; private set; }
    public List<int> Preys { get; private set; } = [];
    public List<PreyCapture> Captures { get; private set; } = [];
    public List<PreyHunter> Hunters { get; private set; } = [];

    private PreyGroup()
    {
    }

    public PreyGroup(string groupId)
    {
        GroupId = groupId;
    }

    public void SyncHunters(PreyGroupSetting setting, int hubLevel, long now)
    {
        var maxHunters = setting.GetHubByLevel(hubLevel)?.MaxHunters ?? 0;

        while (Hunters.Count < maxHunters)
        {
            Hunters.Add(new PreyHunter(Hunters.Count, now));
        }
    }

    public void RefillQueue(PreyGroupSetting setting, int hubLevel, long now)
    {
        var mode = setting.GetActiveMode(hubLevel);

        if (setting.QueueInfo.NoPreyQueue || mode.BlockLength <= 0 || hubLevel < mode.LevelRequired)
            return;

        if (LastQueueRefill == 0)
            LastQueueRefill = now - mode.BlockLength;

        var blocks = (now - LastQueueRefill) / mode.BlockLength;

        if (blocks <= 0)
            return;

        LastQueueRefill += blocks * mode.BlockLength;

        var candidates = setting.Preys
            .Where(x => x.LevelRequired <= hubLevel)
            .Select(x => (x.Id, Chance: x.GetMode(mode.Id)?.Chance ?? 0))
            .Where(x => x.Chance > 0)
            .ToList();

        if (candidates.Count == 0)
            return;

        var totalChance = candidates.Sum(x => x.Chance);

        for (var i = Preys.Count; i < setting.QueueInfo.PerBlock; i++)
        {
            var roll = Random.Shared.Next(totalChance);

            foreach (var (preyId, chance) in candidates)
            {
                if (roll < chance)
                {
                    Preys.Add(preyId);
                    break;
                }

                roll -= chance;
            }
        }
    }

    public int Capture(int preyId, int[] crewSlots, int huntersRequired, bool bought, int hunterSleepTime, long now)
    {
        if (!Preys.Remove(preyId))
            throw new DomainException(GameErrorType.InvalidState, $"Prey {preyId} is not in the {GroupId} queue");

        var crew = crewSlots.Distinct().Select(GetHunter).ToList();

        if (crew.Count > huntersRequired || (!bought && crew.Count < huntersRequired))
            throw new DomainException(GameErrorType.InvalidData, $"Prey {preyId} of {GroupId} requires {huntersRequired} hunters, got {crew.Count}");

        foreach (var hunter in crew)
        {
            if (!hunter.IsUsable(hunterSleepTime, now))
                throw new DomainException(GameErrorType.InvalidState, $"Hunter {hunter.Position} of {GroupId} is still sleeping");

            hunter.Sleep(now);
        }

        NumPreysCaptured++;

        var capture = Captures.FirstOrDefault(x => x.PreyId == preyId);

        if (capture is null)
        {
            capture = new PreyCapture(preyId);
            Captures.Add(capture);
        }

        capture.Increment();

        return capture.Count;
    }

    public void WakeHunter(int position, long now)
    {
        var hunter = GetHunter(position);

        if (hunter.State != HunterState.Sleeping)
            throw new DomainException(GameErrorType.InvalidState, $"Hunter {position} of {GroupId} is not sleeping");

        hunter.WakeUp(now);
    }

    public void UnlockGate(int hubLevel)
    {
        GateUnlockLevel = hubLevel;
    }

    private PreyHunter GetHunter(int position)
    {
        return Hunters.FirstOrDefault(x => x.Position == position) ?? throw new DomainException(GameErrorType.InvalidData, $"Hunter {position} not found in {GroupId}");
    }
}
