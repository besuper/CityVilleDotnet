using System.Text.RegularExpressions;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Enums;
using Humanizer;

namespace CityVilleDotnet.Domain.Entities;

public class World
{
    public int Id { get; set; }
    public string WorldName { get; private set; }
    public int SizeX { get; private set; }
    public int SizeY { get; private set; }
    public int Population { get; private set; }
    public int PopulationCap { get; private set; }
    public int PopulationMin { get; private set; }
    public int PopulationMax { get; private set; }
    public int PotentialPopulation { get; private set; }
    public int NextBuildingId { get; private set; }
    public List<string> ThemeCollections { get; set; } = [];
    public List<MapRect> MapRects { get; set; } = [];
    public List<WorldObject> Objects { get; set; } = [];
    public WorldType Type { get; set; } = WorldType.Main;

    public World()
    {
    }

    public World(string worldName, int sizeX, int sizeY, int population, int populationMin, int populationMax, int populationCap, int potentialPopulation, List<MapRect> mapRects, List<WorldObject> objects)
    {
        WorldName = worldName;
        SizeX = sizeX;
        SizeY = sizeY;
        Population = population;
        PopulationCap = populationCap;
        PopulationMin = populationMin;
        PopulationMax = populationMax;
        PotentialPopulation = potentialPopulation;
        MapRects = mapRects;
        Objects = objects;
        NextBuildingId = objects.Count > 0 ? objects.Max(o => o.WorldFlatId) + 1 : 1;
    }

    public void AddBuilding(WorldObject obj)
    {
        Objects.Add(obj);
    }

    public int GetCurrentPopulation()
    {
        return Population;
    }

    public void CalculatePopulation()
    {
        var currentPopulation = 0;
        var potentialPopulation = 0;
        var minPopulation = 0;
        var maxPopulation = 0;
        var populationCap = 0;

        foreach (var item in Objects)
        {
            var gameItem = GameSettingsManager.Instance.GetItem(item.ItemName);

            if (gameItem?.Population is null)
            {
                var deepItem = gameItem?.GetFirstDeriveItem(gameItem);
                
                if(deepItem?.Population is null) continue;

                gameItem = deepItem;
            }

            var itemMin = gameItem.Population.Min ?? 0;
            var itemMax = gameItem.Population.Max ?? 0;
            var itemCap = gameItem.Population.Cap ?? 0;

            populationCap += itemCap;
            minPopulation += itemMin;
            maxPopulation += itemMax;
            currentPopulation += itemMin;
        }

        PopulationCap = populationCap;
        Population = currentPopulation;
        PopulationMin = minPopulation;
        PopulationMax = maxPopulation;
        PotentialPopulation = potentialPopulation;
    }

    public WorldObject? GetBuildingByCoord(int x, int y, int z)
    {
        return Objects.FirstOrDefault(w => w.X == x && w.Y == y && (w.Z ?? 0) == z);
    }

    public WorldObject? GetBuildingById(int id)
    {
        return Objects.FirstOrDefault(w => w.WorldFlatId == id);
    }

    public int CountBuildingByName(string name)
    {
        return Objects.Count(x => x.ItemName.Equals(name));
    }

    public int CountBuildingByRegex(string pattern)
    {
        var regex = new Regex(pattern);
        return Objects.Count(x => regex.IsMatch(x.ItemName));
    }

    public int CountWorldObjectByKeyword(string keyword)
    {
        var count = 0;

        foreach (var obj in Objects)
        {
            var item = GameSettingsManager.Instance.GetItem(obj.GetItemName());

            if (item is null) continue;

            if (item.HasKeyword(keyword)) count++;
        }

        return count;
    }

    public int GetAvailableBuildingId()
    {
        return NextBuildingId++;
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

        if (gameItem is null) throw new Exception("Item not found");

        building.ItemName = lotOrder.ResourceType;
        building.ClassName = Enum.Parse<BuildingClassType>(gameItem.Type.Pascalize());
        building.Close();
    }

    public void CleanTempIDs()
    {
        // Avoid IDs conflict after refresh
        foreach (var obj in Objects)
        {
            obj.CleanTempId();
        }
    }

    public void UpdateTheme(string theme, bool enable)
    {
        if (enable)
        {
            ThemeCollections.Add(theme);
        }
        else
        {
            ThemeCollections.Remove(theme);
        }
    }

    public int CountStreakByItemName(string itemName)
    {
        var obj = Objects.FirstOrDefault(o => o.StreakLength > 0 && o.GetDeepItemName() == itemName);

        if (obj is null) return 0;

        return obj.StreakLength;
    }
}