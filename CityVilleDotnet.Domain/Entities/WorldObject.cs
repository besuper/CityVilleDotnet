using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Enums;

namespace CityVilleDotnet.Domain.Entities;

public class WorldObject
{
    public WorldObject(string itemName, BuildingClassType className, string? contractName, bool deleted, int tempId, WorldObjectState state, int direction, double? buildTime, double? plantTime, int x, int y, int z, int worldFlatId)
    {
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
        NeverOpened = true;
    }

    public WorldObject()
    {
    }

    public int Id { get; set; }
    public string ItemName { get; set; }
    public BuildingClassType ClassName { get; set; }
    public string? ContractName { get; set; }

    /*[JsonPropertyName("components")]
    public object? Components { get; set; }*/
    public bool Deleted { get; set; }
    public int TempId { get; set; }
    public double? BuildTime { get; set; }
    public double? PlantTime { get; set; }
    public WorldObjectState State { get; set; }
    public int Direction { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public int? Z { get; set; }
    public int WorldFlatId { get; set; }
    public BuildingClassType? TargetBuildingClass { get; set; }
    public string? TargetBuildingName { get; set; }
    public int? Stage { get; set; }
    public int? FinishedBuilds { get; set; }
    public int? Builds { get; set; }
    public int? RequiredStages { get; set; }
    public ConstructionState? CurrentState { get; set; }
    public FranchiseLocation? FranchiseLocation { get; private set; }
    public int? Visits { get; private set; }
    public bool NeverOpened { get; private set; }

    public void SetAsConstructionSite(string itemName, int maxStages)
    {
        Stage = 0;
        FinishedBuilds = 0;
        Builds = 0;
        RequiredStages = maxStages;
        TargetBuildingName = ItemName;
        TargetBuildingClass = ClassName;
        CurrentState = ConstructionState.Idle;

        ItemName = itemName;
        ClassName = BuildingClassType.ConstructionSite;
    }

    public void AddConstructionStage()
    {
        Builds += 1;
        Stage += 1;
        FinishedBuilds = Builds;

        if (FinishedBuilds >= RequiredStages)
        {
            CurrentState = ConstructionState.AtGate;
        }
    }

    public void FinishConstruction()
    {
        if (TargetBuildingName is null || TargetBuildingClass is null)
            throw new Exception("Can't finish build");

        ItemName = TargetBuildingName;
        ClassName = TargetBuildingClass.Value;

        Stage = null;
        FinishedBuilds = null;
        Builds = null;
        TargetBuildingName = null;
        TargetBuildingClass = null;
        CurrentState = null;
        RequiredStages = null;
    }

    public bool HasGrown()
    {
        return State == WorldObjectState.Planted && PlantTime <= ServerUtils.GetCurrentTime();
    }

    public void SetReadyToHarvest()
    {
        State = WorldObjectState.Grown;
    }

    public int Harvest()
    {
        var coinYield = 0;

        if (ClassName == BuildingClassType.Plot)
        {
            if (ContractName is null)
                throw new Exception("Contract name is null, can't harvest");

            var gameItem = GameSettingsManager.Instance.GetItem(ContractName);

            if (gameItem is not null)
                coinYield = gameItem.CoinYield ?? 0;

            State = WorldObjectState.Plowed;
        }
        else
        {
            var gameItem = GameSettingsManager.Instance.GetItem(ItemName);

            if (gameItem is not null)
                coinYield = gameItem.CoinYield ?? 0;
        }

        // Update state to planted if it was grown
        if (HasGrown()) SetReadyToHarvest();

        // If ready to harvest, update state to planted
        if (State == WorldObjectState.Grown)
        {
            State = WorldObjectState.Planted;
            PlantTime = ServerUtils.GetCurrentTime();
        }

        if (ClassName == BuildingClassType.Business)
        {
            if (State != WorldObjectState.ClosedHarvestable)
            {
                throw new Exception("Can't harvest business building that is not harvestable");
            }

            State = WorldObjectState.Closed;
            Visits = 0;
        }

        return coinYield;
    }

    public void OpenBusiness(double buildTime, double plantTime)
    {
        if (ClassName != BuildingClassType.Business) throw new Exception("Can't open other than business building, class name is: " + ClassName + "");
        if (State == WorldObjectState.Open || State == WorldObjectState.ClosedHarvestable) throw new Exception("Building is already open");

        // TODO: Manage these from server not client
        Visits = 0;
        BuildTime = buildTime;
        PlantTime = plantTime;
        State = WorldObjectState.Open;
        NeverOpened = false;

        if (FranchiseLocation is not null)
        {
            FranchiseLocation.TimeLastSupplied = ServerUtils.GetCurrentTime();
        }
    }

    public void SetFranchiseLocation(FranchiseLocation franchiseLocation)
    {
        FranchiseLocation = franchiseLocation;
    }

    public void UpdateVisits(int visits)
    {
        if (ClassName != BuildingClassType.Business) throw new Exception("Can't update visits on non business building");
        if (State != WorldObjectState.Open) throw new Exception("Can't update visits on non open business building");

        var gameItem = GameSettingsManager.Instance.GetItem(ItemName);

        if (gameItem is null)
        {
            throw new Exception("Can't find game item for business building");
        }

        var maxVisits = gameItem.CommodityRequired;

        if (maxVisits is null)
        {
            throw new Exception("Can't find max visits for business building");
        }

        Visits += visits;

        if (Visits >= maxVisits)
        {
            State = WorldObjectState.ClosedHarvestable;
        }
    }
}