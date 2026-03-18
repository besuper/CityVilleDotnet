using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Domain.GameEntities;

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
    public bool Deleted { get; private set; }
    public int TempId { get; private set; }
    public double? BuildTime { get; private set; }
    public double? PlantTime { get; private set; }
    public WorldObjectState State { get; private set; }
    public int Direction { get; private set; }
    public int X { get; private set; }
    public int Y { get; private set; }
    public int? Z { get; private set; }
    public int WorldFlatId { get; private set; }
    public BuildingClassType? TargetBuildingClass { get; private set; }
    public string? TargetBuildingName { get; private set; }
    public int? Stage { get; private set; }
    public int? FinishedBuilds { get; private set; }
    public int? Builds { get; private set; }
    public int? RequiredStages { get; private set; }
    public ConstructionState? CurrentState { get; private set; }
    public FranchiseLocation? FranchiseLocation { get; private set; }
    public string? ItemOwner { get; private set; }
    public int? Visits { get; private set; }
    public bool NeverOpened { get; private set; }
    public int? UpgradeActionCount { get; private set; }

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

    public List<WorldObject> FinishConstruction()
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

        var newObjects = new List<WorldObject>();

        // We need to construct the all bridge parts by ourselves
        if (ItemName == "bridge_standard")
        {
            var item = GameSettingsManager.Instance.GetItem(ItemName);

            if (item?.BridgeParts is null || item.BridgeCenterPart is null || item.BridgeRightPart is null) return [];

            foreach (var part in item.BridgeParts.Parts)
            {
                if (part.X is null || part.Y is null) continue;

                var partItemName = part.Type switch
                {
                    "center" => item.BridgeCenterPart,
                    "right" => item.BridgeRightPart,
                    _ => "bridge_left"
                };

                var newObject = new WorldObject(partItemName, BuildingClassType.BridgePart, null, false, 0, WorldObjectState.Open, 0, null, null, part.X.Value, part.Y.Value, 0, 1);

                newObject.State = WorldObjectState.Static;
                newObjects.Add(newObject);
            }
        }

        return newObjects;
    }

    public bool HasGrown()
    {
        return State == WorldObjectState.Planted && PlantTime <= ServerUtils.GetCurrentTime();
    }

    public void SetReadyToHarvest()
    {
        State = WorldObjectState.Grown;
    }

    public (int CoinYield, int CashYield) Harvest()
    {
        var coinYield = 0;
        var cashYield = 0;

        if (ContractName is not null)
        {
            var gameItem = GameSettingsManager.Instance.GetItem(ContractName);

            if (gameItem is not null)
            {
                coinYield = gameItem.CoinYield ?? 0;
                cashYield = gameItem.CashYield ?? 0;
            }

            State = WorldObjectState.Plowed;
            UpgradeActionCount = (UpgradeActionCount ?? 0) + 1;
        }
        else
        {
            var gameItem = GameSettingsManager.Instance.GetItem(ItemName);

            if (gameItem is not null)
            {
                coinYield = gameItem.CoinYield ?? 0;
                cashYield = gameItem.CashYield ?? 0;
            }
        }

        // Update state to planted if it was grown
        if (HasGrown()) SetReadyToHarvest();

        // This is harvesting Residence
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

        return (coinYield, cashYield);
    }

    public void OpenBusiness()
    {
        if (ClassName != BuildingClassType.Business && ClassName != BuildingClassType.SocialBusiness) throw new Exception("Can't open other than business building, class name is: " + ClassName + "");
        if (State == WorldObjectState.Open || State == WorldObjectState.ClosedHarvestable) throw new Exception("Building is already open");

        Visits = 0;
        PlantTime = ServerUtils.GetCurrentTime();
        State = WorldObjectState.Open;
        NeverOpened = false;
        UpgradeActionCount = (UpgradeActionCount ?? 0) + 1;

        if (FranchiseLocation is not null)
        {
            FranchiseLocation.TimeLastOperated = ServerUtils.GetCurrentTimeSeconds();
        }
    }

    public void SetFranchiseLocation(FranchiseLocation franchiseLocation, string itemOwner)
    {
        FranchiseLocation = franchiseLocation;
        ItemOwner = itemOwner;
    }

    public void UpdateVisits(int visits)
    {
        if (ClassName != BuildingClassType.Business && ClassName != BuildingClassType.SocialBusiness) throw new Exception($"Can't update visits on non business building {Id} {ClassName} {State}");
        if (State != WorldObjectState.Open)
        {
            if (State == WorldObjectState.ClosedHarvestable) return; // If client get out of sync and try to send processVisit while visits are completed, just ignore

            throw new Exception($"Can't update visits on non open business building {Id} {ClassName} {State}");
        }

        var gameItem = GameSettingsManager.Instance.GetItem(ItemName);

        if (gameItem is null)
            throw new Exception("Can't find game item for business building");

        var maxVisits = gameItem.CommodityRequired;

        if (maxVisits is null)
            throw new Exception("Can't find max visits for business building");

        Visits += visits;

        if (Visits >= maxVisits)
        {
            State = WorldObjectState.ClosedHarvestable;
        }
    }

    public void UpgradeBuilding(string newItemName)
    {
        ItemName = newItemName;
    }

    public void MoveTo(int x, int y, int z, int direction)
    {
        // TODO: Check if position is free
        X = x;
        Y = y;
        Z = z;
        Direction = direction;
    }

    public void StartContract(string contractName, WorldObjectState state)
    {
        ContractName = contractName;
        PlantTime = ServerUtils.GetCurrentTime();
        State = state;

        // Why don't ships work like plots?
        // plowed means ready
        // planted, planted
        // grown, grown
        // but receive plowed in start contract transaction
        if (ClassName == BuildingClassType.HarvestableShip)
        {
            State = WorldObjectState.Planted;
        }
    }

    public void UpdateWorldFlatId(int id)
    {
        if (WorldFlatId != 1) throw new Exception($"Can't modify WorldFlatId {WorldFlatId} to {id}");

        WorldFlatId = id;
    }

    public WorldObject LoadObject(WorldObjectDto worldObjectDto)
    {
        Builds = worldObjectDto.Builds;
        BuildTime = worldObjectDto.BuildTime;
        ClassName = Enum.Parse<BuildingClassType>(worldObjectDto.ClassName);
        ContractName = worldObjectDto.ContractName;
        Deleted = worldObjectDto.Deleted;
        Direction = worldObjectDto.Direction;
        FinishedBuilds = worldObjectDto.FinishedBuilds;
        ItemName = worldObjectDto.ItemName;
        PlantTime = worldObjectDto.PlantTime;
        X = worldObjectDto.Position.X;
        Y = worldObjectDto.Position.Y;
        Z = worldObjectDto.Position.Z;
        Stage = worldObjectDto.Stage;
        State = EnumExtensions.EnumExtensions.ParseFromDescription<WorldObjectState>(worldObjectDto.State);
        TargetBuildingClass = worldObjectDto.TargetBuildingClass is null ? null : Enum.Parse<BuildingClassType>(worldObjectDto.TargetBuildingClass);
        TargetBuildingName = worldObjectDto.TargetBuildingName;
        TempId = worldObjectDto.TempId;
        WorldFlatId = worldObjectDto.WorldFlatId;

        return this;
    }

    public void SetTempId(int id)
    {
        if (TempId != -1) throw new Exception("Can't define TempId twice");

        TempId = id;
    }

    public void CleanTempId()
    {
        TempId = -1;
    }

    public void Close()
    {
        if (ClassName != BuildingClassType.Business && ClassName != BuildingClassType.SocialBusiness) throw new Exception($"Can't close non business building {ClassName}");

        State = WorldObjectState.Closed;
    }

    public string GetItemName()
    {
        var itemName = ItemName;

        switch (ClassName)
        {
            case BuildingClassType.ConstructionSite:
            {
                itemName = TargetBuildingName;

                if (itemName is null)
                    throw new Exception("TargetBuildingName can't be null with ConstructionSite");
                break;
            }
            case BuildingClassType.Plot:
            {
                itemName = ContractName;

                if (itemName is null)
                    throw new Exception("ContractName can't be null with Plot");
                break;
            }
        }

        return itemName;
    }

    public BuildingClassType GetClassName()
    {
        var className = ClassName;

        if (ClassName == BuildingClassType.ConstructionSite)
        {
            if (TargetBuildingClass is null)
                throw new Exception("TargetBuildingClass can't be null with ConstructionSite");

            className = TargetBuildingClass.Value;
        }

        return className;
    }
}