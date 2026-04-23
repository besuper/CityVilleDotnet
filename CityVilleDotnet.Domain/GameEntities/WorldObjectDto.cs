using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using System.Text.Json.Serialization;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.EnumExtensions;
using CityVilleDotnet.Domain.Enums;
using FluorineFx;

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

    [JsonPropertyName("state")] public required string State { get; set; }

    [JsonPropertyName("direction")] public int Direction { get; set; }

    [JsonPropertyName("position")] public required WorldObjectPositionDto Position { get; set; }

    [JsonPropertyName("id")] public int WorldFlatId { get; set; }

    [JsonPropertyName("targetBuildingClass")]
    public string? TargetBuildingClass { get; set; }

    [JsonPropertyName("targetBuildingName")]
    public string? TargetBuildingName { get; set; }

    [JsonPropertyName("stage")] public int? Stage { get; set; }
    [JsonPropertyName("currentState")] public int? CurrentState { get; set; }

    [JsonPropertyName("finishedBuilds")] public int? FinishedBuilds { get; set; }

    [JsonPropertyName("builds")] public int? Builds { get; set; }
    [JsonPropertyName("visits")] public int? Visits { get; set; }
    [JsonPropertyName("harvestCounter")] public int HarvestCounter { get; set; }

    [JsonPropertyName("upgradeActionCount")]
    public int UpgradeActionCount { get; set; }

    [JsonPropertyName("neverOpened")] public bool NeverOpened { get; set; }

    [JsonPropertyName("endPosition")] public WorldObjectPositionDto? EndPosition { get; set; }

    [JsonPropertyName("mechanicData")] public ASObject? MechanicData { get; set; }
    // TODO: Implement Gates
    //[JsonPropertyName("gates")] public List<object>? Gates { get; set; }

    [JsonPropertyName("itemOwner")] public string? ItemOwner { get; set; }

    [JsonPropertyName("franchise_info")] public FranchiseInfoDto? FranchiseInfo { get; set; }
    [JsonPropertyName("builtFloorCount")] public int? BuiltFloorCount { get; set; }
}

public class FranchiseInfoDto
{
    [JsonPropertyName("star_rating")] public int StarRating { get; set; }
    [JsonPropertyName("commodity_left")] public int CommodityLeft { get; set; }
    [JsonPropertyName("franchise_name")] public string FranchiseName { get; set; } = string.Empty;
}

public static class WorldObjectDtoMapper
{
    public static WorldObjectDto ToDto(this WorldObject model)
    {
        if (model.HasGrown())
            model.SetReadyToHarvest();

        var dto = new WorldObjectDto()
        {
            ItemName = model.ItemName,
            ClassName = model.ClassName.ToString(),
            ContractName = model.ContractName,
            Deleted = model.Deleted,
            TempId = model.TempId,
            BuildTime = model.BuildTime,
            PlantTime = model.PlantTime,
            CurrentState = model.CurrentState is null ? (int)ConstructionState.Idle : (int)model.CurrentState,
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
            TargetBuildingClass = model.TargetBuildingClass?.ToString(),
            TargetBuildingName = model.TargetBuildingName,
            FinishedBuilds = model.FinishedBuilds,
            Builds = model.Builds,
            Visits = model.Visits,
            NeverOpened = model.NeverOpened,
            HarvestCounter = model.UpgradeActionCount ?? 0, // This is for Plot
            UpgradeActionCount = model.UpgradeActionCount ?? 0, // This is for Business
            ItemOwner = model.ItemOwner,
            FranchiseInfo = model.FranchiseLocation is not null
                ? new FranchiseInfoDto
                {
                    StarRating = model.FranchiseLocation.StarRating,
                    CommodityLeft = model.FranchiseLocation.CommodityLeft,
                    FranchiseName = model.FranchiseLocation.FranchiseName
                }
                : null,
            BuiltFloorCount = model.BuiltFloorCount
        };

        // FIXME: Not enough (not sure if it is the right way to implement this)
        if (model.ClassName == BuildingClassType.Bridge)
        {
            var item = GameSettingsManager.Instance.GetItem(model.ItemName);

            if (item?.DerivesFrom is not null)
                item = GameSettingsManager.Instance.GetItem(item.DerivesFrom);

            var rightPart = item?.BridgeParts?.Parts.FirstOrDefault(p => p.Type == "right");

            if (rightPart?.X != null && rightPart.Y != null)
            {
                dto.EndPosition = new WorldObjectPositionDto
                {
                    X = rightPart.X.Value,
                    Y = rightPart.Y.Value
                };
            }
        }

        if (model.ClassName == BuildingClassType.SocialBusiness || model.ClassName == BuildingClassType.Hotel)
        {
            var item = GameSettingsManager.Instance.GetItem(model.ItemName);

            dto.MechanicData = new ASObject
            {
                {
                    "harvestState", model.State == WorldObjectState.Open || model.State == WorldObjectState.ClosedHarvestable
                        ? new ASObject
                        {
                            { "customers", model.Visits },
                            { "customersReq", item?.CustomerCapacity ?? 0 }
                        }
                        : null
                }
            };
        }

        var gameItem = GameSettingsManager.Instance.GetItem(model.ItemName);

        if (gameItem?.Mechanics is not null)
        {
            var loadGameMode = gameItem.Mechanics.GetMechanicByGameMode("load");

            foreach (var mechanic in loadGameMode?.Mechanics ?? [])
            {
                if (mechanic.Type == "streakData")
                {
                    var activeDuration = mechanic.ActiveDuration;
                    var inactiveDuration = mechanic.InactiveDuration;
                    var maxStreakLength = mechanic.MaxStreakLength;

                    model.UpdateStreakData(activeDuration, inactiveDuration, maxStreakLength);

                    dto.MechanicData ??= new ASObject();

                    dto.MechanicData["streakData"] = new ASObject
                    {
                        { "activationTime", model.ActivationTime },
                        { "inactiveTime", model.InactiveTime },
                        { "streakLength", model.StreakLength }
                    };
                }
            }
        }

        return dto;
    }
}