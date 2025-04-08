using System.Text.Json.Serialization;

namespace CityVilleDotnet.Domain.Entities;

public class WorldObject
{
    public WorldObject(string itemName, string className, string? contractName, bool deleted, int tempId, string state, int direction, WorldObjectPosition position, int worldFlatId)
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
    }

    public WorldObject()
    {

    }

    [JsonIgnore]
    public Guid Id { get; set; }

    [JsonPropertyName("itemName")]
    public string ItemName { get; set; }

    [JsonPropertyName("className")]
    public string ClassName { get; set; }

    [JsonPropertyName("contractName")]
    public string? ContractName { get; set; }

    /*[JsonPropertyName("components")]
    public object? Components { get; set; }*/

    [JsonPropertyName("deleted")]
    public bool Deleted { get; set; }

    [JsonPropertyName("tempId")]
    public int TempId { get; set; }

    [JsonPropertyName("state")]
    public string State { get; set; }

    [JsonPropertyName("direction")]
    public int Direction { get; set; }

    [JsonPropertyName("position")]
    public WorldObjectPosition? Position { get; set; }

    [JsonPropertyName("id")]
    public int WorldFlatId { get; set; }

    [JsonPropertyName("targetBuildingClass")]
    public string? TargetBuildingClass { get; set; }

    [JsonPropertyName("targetBuildingName")]
    public string? TargetBuildingName { get; set; }

    [JsonPropertyName("stage")]
    public int? Stage { get; set; }

    [JsonPropertyName("finishedBuilds")]
    public int? FinishedBuilds { get; set; }

    [JsonPropertyName("builds")]
    public int? Builds { get; set; }

    public void SetAsConstructionSite()
    {
        Stage = 0;
        FinishedBuilds = 0;
        Builds = 0;
        TargetBuildingName = ItemName;
        TargetBuildingClass = ClassName;
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