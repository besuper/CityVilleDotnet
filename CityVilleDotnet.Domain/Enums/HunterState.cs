using System.ComponentModel;

namespace CityVilleDotnet.Domain.Enums;

public enum HunterState
{
    [Description("state_patrolling")]
    Patrolling = 0,
    [Description("state_sleeping")]
    Sleeping = 1,
}
