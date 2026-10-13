using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.EnumExtensions;
using FluorineFx;

namespace CityVilleDotnet.Domain.GameEntities;

public static class PreyGroupDto
{
    public static ASObject ToPreyGroupAsObject(this PreyGroup? group, string groupId)
    {
        return new ASObject
        {
            { "preyId", groupId },
            { "preys", group?.Preys.Select(object (x) => x).ToList() ?? [] },
            { "numPreysCaptured", group?.NumPreysCaptured ?? 0 },
            { "typesPreysCaptured", new ASObject(group?.Captures.ToDictionary(x => x.PreyId.ToString(), object (x) => x.Count) ?? []) },
            { "gateUnlockLevel", group?.GateUnlockLevel ?? 1 }
        };
    }

    // Every prey group reads its hunters from the "cops" bucket
    public static ASObject ToHunterWorkersAsObject(this PreyGroup? group)
    {
        return new ASObject
        {
            {
                "cops", new ASObject
                {
                    { "attributes", new ASObject() },
                    {
                        "members", group?.Hunters.OrderBy(x => x.Position).Select(object (x) => new ASObject
                        {
                            { "zid", x.GetZid() },
                            {
                                "data", new ASObject
                                {
                                    { "state", x.State.ToDescriptionString() },
                                    { "ts", x.Timestamp }
                                }
                            }
                        }).ToList() ?? []
                    }
                }
            }
        };
    }
}
