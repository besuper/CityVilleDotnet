using CityVilleDotnet.Domain.Entities;
using System.Text.Json.Serialization;
using CityVilleDotnet.Domain.EnumExtensions;

namespace CityVilleDotnet.Domain.GameEntities;

public class WorldObjectDto
{
    [JsonPropertyName("itemName")] public required string ItemName { get; set; }

    [JsonPropertyName("className")] public required string ClassName { get; set; }

    [JsonPropertyName("contractName")] public string? ContractName { get; set; }

    /*[JsonPropertyName("components")]
    public object? Components { get; set; }*/

    [JsonPropertyName("deleted")] public bool Deleted { get; set; }

    [JsonPropertyName("tempId")] public int TempId { get; set; }

    [JsonPropertyName("buildTime")] public double? BuildTime { get; set; }

    [JsonPropertyName("plantTime")] public double? PlantTime { get; set; }

    [JsonPropertyName("state")] public string State { get; set; }

    [JsonPropertyName("direction")] public int Direction { get; set; }

    [JsonPropertyName("position")] public required WorldObjectPositionDto Position { get; set; }

    [JsonPropertyName("id")] public int WorldFlatId { get; set; }

    [JsonPropertyName("targetBuildingClass")]
    public string? TargetBuildingClass { get; set; }

    [JsonPropertyName("targetBuildingName")]
    public string? TargetBuildingName { get; set; }

    [JsonPropertyName("stage")] public int? Stage { get; set; }

    [JsonPropertyName("finishedBuilds")] public int? FinishedBuilds { get; set; }

    [JsonPropertyName("builds")] public int? Builds { get; set; }
    [JsonPropertyName("visits")] public int? Visits { get; set; }
    
    // TODO: Add franchise_info from Business->loadObject
}

public static class WorldObjectDtoMapper
{
    public static WorldObjectDto ToDto(this WorldObject model)
    {
        return new WorldObjectDto()
        {
            ItemName = model.ItemName,
            ClassName = model.ClassName,
            ContractName = model.ContractName,
            Deleted = model.Deleted,
            TempId = model.TempId,
            BuildTime = model.BuildTime,
            PlantTime = model.PlantTime,
            Stage = model.Stage,
            State = model.State.ToDescriptionString(),
            Direction = model.Direction,
            Position = new WorldObjectPositionDto
            {
                X = model.X,
                Y = model.Y,
                Z = model.Z ?? 0
            },
            WorldFlatId = model.WorldFlatId,
            TargetBuildingClass = model.TargetBuildingClass,
            TargetBuildingName = model.TargetBuildingName,
            FinishedBuilds = model.FinishedBuilds,
            Builds = model.Builds,
            Visits = model.Visits
        };
    }
}