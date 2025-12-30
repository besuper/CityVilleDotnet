namespace CityVilleDotnet.Domain.Enums;

public enum ConstructionState
{
    Idle = 0,
    Building = 1,
    AtGate = 2,
    CanBeFinished = 3,
    Finished = 4,
}