using CityVilleDotnet.Domain.Entities;
using System.Text.Json.Serialization;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.EnumExtensions;
using CityVilleDotnet.Domain.Enums;
using FluorineFx;

namespace CityVilleDotnet.Domain.GameEntities;

public class WorldDto
{
    [JsonPropertyName("sizeX")] public int SizeX { get; set; } = 36;

    [JsonPropertyName("sizeY")] public int SizeY { get; set; } = 36;

    [JsonPropertyName("mapRects")] public required List<MapRectDto> MapRects { get; set; }

    [JsonPropertyName("citySim")] public CitySimDto? CitySim { get; set; }

    [JsonPropertyName("objects")] public required List<WorldObjectDto> Objects { get; set; }

    [JsonPropertyName("lastExpansionTier")]
    public int LastExpansionTier { get; set; }

    [JsonPropertyName("world_id")] public required string WorldId { get; set; }

    [JsonPropertyName("mostFrequentHelpers")]
    public ASObject MostFrequentHelpers { get; set; } = new();

    [JsonPropertyName("currentThemeCollections")]
    public List<string> ThemeCollections { get; set; } = [];
}

public static class WorldDtoMapper
{
    public static WorldDto ToDto(this World model)
    {
        return new WorldDto()
        {
            SizeX = model.SizeX,
            SizeY = model.SizeY,
            MapRects = model.MapRects.Select(x => x.ToDto()).ToList(),
            CitySim = new CitySimDto()
            {
                PopulationSummary = model.ToPopulationSummaryDto()
            },
            Objects = model.Objects.Select(x => x.ToDto()).ToList(),
            ThemeCollections = model.ThemeCollections,
            LastExpansionTier = 0,
            WorldId = model.Type.ToDescriptionString()
        };
    }

    public static PopulationSummaryDto ToPopulationSummaryDto(this World model)
    {
        return new PopulationSummaryDto()
        {
            Segments = new ASObject(new Dictionary<string, object>()
            {
                // TODO: Support multiple population types
                {
                    "citizen", new Dictionary<string, object>()
                    {
                        { "id", "citizen" },
                        { "minimum", model.PopulationMin }, // Calculte from <population min
                        { "yield", model.Population }, // Current population min or max (base on the level)
                        { "maximum", model.PopulationMax }, // Calculte from <population max
                        { "capacity", model.PopulationCap }, // Calculate from <population cap
                        { "potential", model.PotentialPopulation } // idk
                    }
                }
            })
        };
    }

    public static ASObject ToCommoditySummaryDto(this World model)
    {
        var capacities = new Dictionary<string, int>();

        foreach (var obj in model.Objects)
        {
            var gameItem = GameSettingsManager.Instance.GetItem(obj.ItemName);

            if (gameItem is null) continue;

            foreach (var commodityItem in gameItem.Commodity)
            {
                /*if (commodityItem.Capacity <= 0)
                    continue;*/

                var name = commodityItem.Name;

                if (capacities.TryGetValue(name, out var existing))
                    capacities[name] = existing + commodityItem.Capacity;
                else
                    capacities[name] = commodityItem.Capacity;
            }
        }

        var commodityObj = new ASObject();

        foreach (var (name, capacity) in capacities)
        {
            commodityObj[name] = new ASObject
            {
                { "id", name },
                { "capacity", capacity }
            };
        }

        return new ASObject { { "commodity", commodityObj } };
    }
    
    public static ASObject BuildIncentivizedExpansions(this World world)
    {
        var expansions = new ASObject();
        var cellToId = new ASObject();
        var failureCount = new ASObject();

        foreach (var expansion in world.IncentivizedExpansions)
        {
            if (expansion.IsCompleted)
            {
                expansions[expansion.ExpansionId] = null;
            }
            else if (expansion.IsActive())
            {
                expansions[expansion.ExpansionId] = new ASObject
                {
                    { "x", expansion.X!.Value },
                    { "y", expansion.Y!.Value },
                    { "start", expansion.StartTimestamp ?? 0 },
                };

                cellToId[$"{expansion.X}_{expansion.Y}"] = expansion.ExpansionId;
            }

            if (expansion.FailureCount > 0)
                failureCount[expansion.ExpansionId] = expansion.FailureCount;
        }

        return new ASObject
        {
            { "expansions", expansions },
            { "cellToId", cellToId },
            { "parentExpansions", new ASObject() },
            { "failureCount", failureCount },
        };
    }
}