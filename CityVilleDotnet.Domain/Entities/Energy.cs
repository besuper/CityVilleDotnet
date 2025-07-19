namespace CityVilleDotnet.Domain.Entities;

public record Energy(int CurrentNewEnergy, double TimeToRegen, double TimeUntilNextRegen, double TimeSinceLastRegen);