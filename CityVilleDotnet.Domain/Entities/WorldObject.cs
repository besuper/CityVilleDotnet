using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Utils;

namespace CityVilleDotnet.Domain.Entities;

public class WorldObject
{
    public WorldObject(string itemName, string className, string? contractName, bool deleted, int tempId, string state, int direction, double? buildTime, double? plantTime, int x, int y, int z, int worldFlatId)
    {
        Id = Guid.NewGuid();
        ItemName = itemName;
        ClassName = className;
        ContractName = contractName;
        Deleted = deleted;
        TempId = tempId;
        State = state;
        Direction = direction;
        WorldFlatId = worldFlatId;
        BuildTime = buildTime;
        PlantTime = plantTime;
        X = x;
        Y = y;
        Z = z;
    }

    public WorldObject()
    {
    }

    public Guid Id { get; set; }
    public string ItemName { get; set; }
    public string ClassName { get; set; }
    public string? ContractName { get; set; }

    /*[JsonPropertyName("components")]
    public object? Components { get; set; }*/
    public bool Deleted { get; set; }
    public int TempId { get; set; }
    public double? BuildTime { get; set; }
    public double? PlantTime { get; set; }
    public string State { get; set; }
    public int Direction { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public int? Z { get; set; }
    public int WorldFlatId { get; set; }
    public string? TargetBuildingClass { get; set; }
    public string? TargetBuildingName { get; set; }
    public int? Stage { get; set; }
    public int? FinishedBuilds { get; set; }
    public int? Builds { get; set; }

    public void SetAsConstructionSite(string itemName)
    {
        Stage = 0;
        FinishedBuilds = 0;
        Builds = 0;
        TargetBuildingName = ItemName;
        TargetBuildingClass = ClassName;

        ItemName = itemName;
        ClassName = nameof(BuildingClassType.ConstructionSite);
    }

    public void AddConstructionStage()
    {
        Builds += 1;
        Stage += 1;
        FinishedBuilds = Builds;
    }

    public void FinishConstruction()
    {
        if (TargetBuildingName is null || TargetBuildingClass is null)
            throw new Exception("Can't finish build");

        ItemName = TargetBuildingName;
        ClassName = TargetBuildingClass;

        Stage = null;
        FinishedBuilds = null;
        Builds = null;
        TargetBuildingName = null;
        TargetBuildingClass = null;
    }

    public bool HasGrown()
    {
        return State == "planted" && PlantTime <= ServerUtils.GetCurrentTime();
    }

    public void SetReadyToHarvest()
    {
        State = "grown";
    }

    public int Harvest()
    {
        var coinYield = 0;

        if (ClassName == nameof(BuildingClassType.Plot))
        {
            if (ContractName is null)
                throw new Exception("Contract name is null, can't harvest");
            
            var gameItem = GameSettingsManager.Instance.GetItem(ContractName);

            if (gameItem is not null)
                coinYield = gameItem.CoinYield ?? 0;

            State = "plowed";
        }
        else
        {
            var gameItem = GameSettingsManager.Instance.GetItem(ItemName);

            if (gameItem is not null)
                coinYield = gameItem.CoinYield ?? 0;
        }

        // Update state to planted if it was grown
        if (HasGrown()) SetReadyToHarvest();

        // Close business
        if (State == "open") State = "closed";

        // If ready to harvest, update state to planted
        if (State == "grown")
        {
            State = "planted";
            PlantTime = ServerUtils.GetCurrentTime();
        }

        return coinYield;
    }
}