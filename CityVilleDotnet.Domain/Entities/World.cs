using CityVilleDotnet.Common.Settings;
using System.Text.Json.Serialization;

namespace CityVilleDotnet.Domain.Entities;

public class World
{
    [JsonIgnore]
    public Guid Id { get; set; }

    [JsonPropertyName("sizeX")]
    public int SizeX { get; set; } = 36;

    [JsonPropertyName("sizeY")]
    public int SizeY { get; set; } = 36;

    [JsonPropertyName("mapRects")]
    public List<MapRect> MapRects { get; set; }

    [JsonPropertyName("citySim")]
    public CitySim? CitySim { get; set; }

    [JsonPropertyName("objects")]
    public List<WorldObject> Objects { get; set; }

    public void AddBuilding(WorldObject obj)
    {
        Objects.Add(obj);
    }

    public int GetCurrentPopulation()
    {
        return CitySim.Population * 10;
    }

    public void calculateCurrentPopulation()
    {
        var population = 0;

        foreach (var item in Objects)
        {
            var gameItem = GameSettingsManager.Instance.GetItem(item.ItemName);

            if (gameItem is null) continue;

            population += 10 * gameItem.PopulationYield ?? 0;
        }

        CitySim.Population = population / 10;
    }

    public void calculatePopulationCap()
    {
        // TODO: Load default initPopulationCap from farming in game settings
        var population = 120;

        foreach (var item in Objects)
        {
            var gameItem = GameSettingsManager.Instance.GetItem(item.ItemName);

            if (gameItem is null) continue;

            population += 10 * gameItem.PopulationCapYield ?? 0;
        }

        CitySim.PopulationCap = population / 10;
    }

    public WorldObject? GetBuilding(int id, int x, int y, int z)
    {
        return Objects.FirstOrDefault(w => w.WorldFlatId == id && w.Position.X == x && w.Position.Y == y && w.Position.Z == z);
    }

    public WorldObject? GetBuildingByCoord(int x, int y, int z)
    {
        return Objects.FirstOrDefault(w => w.Position.X == x && w.Position.Y == y && w.Position.Z == z);
    }

    public int CountBuildingByName(string name)
    {
        return Objects.Count(x => x.ItemName.Equals(name));
    }

    public int CountOpenedBuildingByName(string name)
    {
        return Objects.Count(x => x.ItemName.Equals(name) && x.State.Equals("open"));
    }

    public int GetAvailableBuildingId()
    {
        for (var i = Objects.Count; i < Objects.Count + 1000; i++)
        {
            var building = Objects.FirstOrDefault(x => x.WorldFlatId == i);

            if (building is null)
            {
                return i;
            }
        }

        return -1;
    }

    public void RemoveObject(WorldObject obj)
    {
        Objects.Remove(obj);
    }
}
