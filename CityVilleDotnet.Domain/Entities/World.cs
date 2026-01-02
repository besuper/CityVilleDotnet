using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.EnumExtensions;
using CityVilleDotnet.Domain.Enums;

namespace CityVilleDotnet.Domain.Entities;

public class World
{
    public int Id { get; set; }
    public string WorldName { get; private set; }
    public int SizeX { get; set; }
    public int SizeY { get; set; }
    public int Population { get; set; }
    public int PopulationCap { get; set; }
    public int PotentialPopulation { get; set; }
    public List<MapRect> MapRects { get; set; } = [];
    public List<WorldObject> Objects { get; set; } = [];
    
    public World() { }

    public World(string worldName, int sizeX, int sizeY, int population, int populationCap, int potentialPopulation, List<MapRect> mapRects, List<WorldObject> objects)
    {
        WorldName = worldName;
        SizeX = sizeX;
        SizeY = sizeY;
        Population = population;
        PopulationCap = populationCap;
        PotentialPopulation = potentialPopulation;
        MapRects = mapRects;
        Objects = objects;
    }

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

    public void ReplaceBuildingFromLotOrder(LotOrder lotOrder)
    {
        var building = Objects.FirstOrDefault(x => x.WorldFlatId == lotOrder.LotId);

        if (building is null) return;
        if (building.ClassName != BuildingClassType.LotSite) throw new Exception("Building is not a LotSite");
        
        var gameItem = GameSettingsManager.Instance.GetItem(lotOrder.ResourceType);

        if(gameItem is null) throw new Exception("Item not found");
        
        building.ItemName = lotOrder.ResourceType;
        building.ClassName = Enum.Parse<BuildingClassType>(gameItem.Type);
        building.State = WorldObjectState.Closed;
    }
}