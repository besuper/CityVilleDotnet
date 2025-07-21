using CityVilleDotnet.Common.Settings;

namespace CityVilleDotnet.Domain.Entities;

public class World
{
    public Guid Id { get; set; }
    public required string WorldName { get; set; }
    public int SizeX { get; set; } = 36;
    public int SizeY { get; set; } = 36;
    public int Population { get; set; }
    public int PopulationCap { get; set; }
    public int PotentialPopulation { get; set; }
    public required List<MapRect> MapRects { get; set; }
    public required List<WorldObject> Objects { get; set; }

    public void AddBuilding(WorldObject obj)
    {
        Objects.Add(obj);
    }

    public int GetCurrentPopulation()
    {
        return Population * 10;
    }

    public void CalculateCurrentPopulation()
    {
        var population = 0;

        foreach (var item in Objects)
        {
            var gameItem = GameSettingsManager.Instance.GetItem(item.ItemName);

            if (gameItem is null) continue;

            population += 10 * gameItem.PopulationYield ?? 0;
        }

        Population = population / 10;
    }

    public void CalculatePopulationCap()
    {
        var population = GameSettingsManager.Instance.GetInt("InitPopulationCap") * 10;

        foreach (var item in Objects)
        {
            var gameItem = GameSettingsManager.Instance.GetItem(item.ItemName);

            if (gameItem is null) continue;

            population += 10 * gameItem.PopulationCapYield ?? 0;
        }

        PopulationCap = population / 10;
    }

    public WorldObject? GetBuildingByCoord(int x, int y, int z)
    {
        return Objects.FirstOrDefault(w => w.X == x && w.Y == y && (w.Z ?? 0) == z);
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

    public void RemoveBuilding(WorldObject obj)
    {
        Objects.Remove(obj);
    }

    public void AddMapRect(MapRect mapRect)
    {
        // FIXME: Check if this map already exist
        MapRects.Add(mapRect);
    }

    public string SetWorldName(string name)
    {
        var newName = name.Trim();

        WorldName = newName;

        return newName;
    }
}