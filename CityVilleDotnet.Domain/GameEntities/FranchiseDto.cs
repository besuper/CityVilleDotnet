using System.Text.Json.Serialization;
using CityVilleDotnet.Domain.Entities;
using FluorineFx;

namespace CityVilleDotnet.Domain.GameEntities;

public class FranchiseDto
{
    [JsonPropertyName("name")] public required string FranchiseType { get; set; }

    [JsonPropertyName("franchise_name")] public required string FranchiseName { get; set; }

    [JsonPropertyName("locations")] public required Dictionary<string, FranchiseLocationDto> Locations { get; set; }

    [JsonPropertyName("time_last_collected")]
    public int TimeLastCollected { get; set; }
}

public static class FranchiseDtoMapper
{
    public static FranchiseDto ToDto(this Franchise model)
    {
        return new FranchiseDto
        {
            FranchiseType = model.FranchiseType,
            FranchiseName = model.FranchiseName,
            TimeLastCollected = model.TimeLastCollected,
            Locations = model.Locations.ToDictionary(x => x.Uid, x => x.ToDto())
        };
    }
}