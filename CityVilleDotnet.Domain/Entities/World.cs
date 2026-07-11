using System.Text.RegularExpressions;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Settings.GameSettings;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.EnumExtensions;
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
    public List<IncentivizedExpansion> IncentivizedExpansions { get; set; } = [];
    public WorldType Type { get; set; } = WorldType.Main;
    public Player? Player { get; set; }
    public string? WorldCreated { get; private set; }

    public World()
    {
    }

    public World(string worldName, int sizeX, int sizeY, int population, int populationMin, int populationMax, int populationCap, int potentialPopulation, List<MapRect> mapRects, List<WorldObject> objects, WorldType type = WorldType.Main)
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
        Type = type;
        NextBuildingId = objects.Count > 0 ? objects.Max(o => o.WorldFlatId) + 1 : 1;
    }

    public void SetWorldCreated(string? stage)
    {
        WorldCreated = stage;
    }

    public bool IsFtueCompleted()
    {
        return WorldCreated is null;
    }

    public void AddBuilding(WorldObject obj)
    {
        Objects.Add(obj);
    }

    public int GetCurrentPopulation()
    {
        return Population;
    }

    public int AddBonusPopulation(int amount)
    {
        var remaining = amount;

        foreach (var obj in Objects.Where(x => x.ClassName == BuildingClassType.Residence))
        {
            if (remaining <= 0) break;

            remaining -= obj.AddBonusPopulation(remaining);
        }

        Population += amount - remaining;

        return amount - remaining;
    }

    public string? GetAppraisalId()
    {
        return GameSettingsManager.Instance.GetWorldConfig(Type.ToDescriptionString())?.AppraisalId;
    }

    public bool AreIncentivizedExpansionsEnabled()
    {
        return GameSettingsManager.Instance.GetWorldConfig(Type.ToDescriptionString())?.EnableIncentivizedExpansions ?? true;
    }

    public int AddBonusAppraisal(int amount)
    {
        var appraisalId = GetAppraisalId();

        if (appraisalId is null) return 0;

        var remaining = amount;

        foreach (var obj in Objects.Where(x => x.ClassName != BuildingClassType.ConstructionSite))
        {
            if (remaining <= 0) break;

            remaining -= obj.AddBonusAppraisal(remaining, appraisalId);
        }

        return amount - remaining;
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
            currentPopulation += itemMin + Math.Clamp(item.GetBonusPopulation(), 0, Math.Max(0, itemMax - itemMin));
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

    public WorldObject? GetBuildingByClientId(int id)
    {
        var objectByTempId = Objects.FirstOrDefault(w => w.TempId == id);

        if (objectByTempId is null)
        {
            return Objects.FirstOrDefault(w => w.WorldFlatId == id);
        }
        
        return objectByTempId;
    }

    public int CountBuildingByName(string name)
    {
        return Objects.Count(x => x.ItemName.Equals(name));
    }

    public int CountConstructionOrBuildingByName(string name)
    {
        return Objects.Count(x => x.ItemName.Equals(name) || name.Equals(x.TargetBuildingName));
    }

    public int CountZooAnimals(string enclosureItemName)
    {
        var enclosure = Objects.FirstOrDefault(x => x.ItemName.Equals(enclosureItemName));

        if (enclosure is null) return 0;

        return enclosure.StorageItems.Sum(x => x.Amount) + enclosure.Slots.Count;
    }

    private static readonly string[] RemodelHeadquartersNames = ["mun_constructioncompany", "mun_constructioncompany_2", "mun_constructioncompany_3"];

    public bool HasRemodelHeadquarters()
    {
        return Objects.Any(x => RemodelHeadquartersNames.Contains(x.ItemName));
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

    public WorldObject EmbedDynamicExpansionObject(DynamicExpansionObjectItem definition, int baseX, int baseY, int tempId)
    {
        var item = GameSettingsManager.Instance.GetItem(definition.ItemName);

        if (item is null) throw new Exception($"Can't find item {definition.ItemName}");

        var obj = new WorldObject(
            definition.ItemName,
            Enum.Parse<BuildingClassType>(item.Type.Pascalize()),
            null,
            false,
            tempId,
            WorldObjectState.Static,
            definition.Direction,
            ServerUtils.GetCurrentTime(),
            ServerUtils.GetCurrentTime(),
            baseX + definition.XOffset,
            baseY + definition.YOffset,
            0,
            GetAvailableBuildingId()
        );

        if (definition.IsConstruction() && item.Construction is not null)
        {
            var constructionItem = GameSettingsManager.Instance.GetItem(item.Construction);

            if (constructionItem?.NumberOfStages is null)
                throw new Exception($"Construction item not found with {item.Construction}");

            obj.SetAsConstructionSite(item.Construction, constructionItem.NumberOfStages.Value);
        }

        AddBuilding(obj);

        return obj;
    }

    public IncentivizedExpansion GetOrCreateIncentivizedExpansion(string expansionId)
    {
        var expansion = IncentivizedExpansions.FirstOrDefault(x => x.ExpansionId == expansionId);

        if (expansion is null)
        {
            expansion = new IncentivizedExpansion(expansionId);
            IncentivizedExpansions.Add(expansion);
        }

        return expansion;
    }

    public void AddMapRect(MapRect mapRect)
    {
        // FIXME: Check if this map already exist
        MapRects.Add(mapRect);
    }

    public void GrantFreeExpansions(string? expansionCoords, string? expansionType)
    {
        if (string.IsNullOrEmpty(expansionCoords) || string.IsNullOrEmpty(expansionType)) return;

        var expansionItem = GameSettingsManager.Instance.GetItem(expansionType);

        if (expansionItem?.Width is null || expansionItem.Height is null) return;

        var width = expansionItem.Width.Value;
        var height = expansionItem.Height.Value;

        // coordinates are a flat list of x|y pairs, "-12|-36" (see client ExpansionManager.onProcessGrantedExpansionsFromMapResource)
        var coords = expansionCoords.Split('|');

        for (var i = 0; i + 1 < coords.Length; i += 2)
        {
            if (!int.TryParse(coords[i], out var x) || !int.TryParse(coords[i + 1], out var y)) continue;

            var intersectsTerritory = MapRects.Any(r => x < r.X + r.Width && r.X < x + width && y < r.Y + r.Height && r.Y < y + height);

            if (intersectsTerritory) continue;

            MapRects.Add(new MapRect
            {
                X = x,
                Y = y,
                Width = width,
                Height = height
            });
        }
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
        foreach (var obj in Objects.Where(o => o.TempId != -1))
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

    public int GetStreakEffectByItemName(string itemName)
    {
        var obj = Objects.FirstOrDefault(o => o.EnergyModifier > 0 && o.GetDeepItemName() == itemName);

        if (obj is null) return 0;

        return obj.EnergyModifier;
    }
}