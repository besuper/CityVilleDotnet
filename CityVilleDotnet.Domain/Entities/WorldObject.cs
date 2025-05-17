namespace CityVilleDotnet.Domain.Entities;

public class WorldObject
{
    public WorldObject(string itemName, string className, string? contractName, bool deleted, int tempId, string state, int direction, double buildTime, double plantTime, WorldObjectPosition position, int worldFlatId)
    {
        Id = Guid.NewGuid();
        ItemName = itemName;
        ClassName = className;
        ContractName = contractName;
        Deleted = deleted;
        TempId = tempId;
        State = state;
        Direction = direction;
        Position = position;
        WorldFlatId = worldFlatId;
        BuildTime = buildTime;
        PlantTime = plantTime;
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
    public WorldObjectPosition? Position { get; set; }
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
        ClassName = "ConstructionSite";
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
        {
            throw new Exception("Can't finish build");
        }

        ItemName = TargetBuildingName;
        ClassName = TargetBuildingClass;

        Stage = null;
        FinishedBuilds = null;
        Builds = null;
        TargetBuildingName = null;
        TargetBuildingClass = null;
    }
}