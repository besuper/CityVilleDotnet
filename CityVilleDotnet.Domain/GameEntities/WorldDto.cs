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

    [JsonPropertyName("worldCreated")] public string? WorldCreated { get; set; }

    // The client reads it from the cached world blob when switching worlds (OpenWorld.setupWorld)
    [JsonPropertyName("featureData")] public ASObject? FeatureData { get; set; }
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
                PopulationSummary = model.ToPopulationSummaryDto(),
                AppraisalSummary = model.ToAppraisalSummaryDto()
            },
            Objects = model.Objects.Select(x => x.ToDto()).ToList(),
            ThemeCollections = model.ThemeCollections,
            LastExpansionTier = 0,
            WorldId = model.Type.ToDescriptionString(),
            WorldCreated = model.WorldCreated,
            FeatureData = new ASObject
            {
                { "incentivizedExpansions", model.BuildIncentivizedExpansions() }
            }
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

    // TODO: Implement everything in DTO (avoid raw AsObject)
    public static ASObject ToPopulationSummaryAsObject(this World model)
    {
        return new ASObject
        {
            {
                "segments", new ASObject
                {
                    {
                        "citizen", new ASObject
                        {
                            { "id", "citizen" },
                            { "minimum", model.PopulationMin },
                            { "yield", model.Population },
                            { "maximum", model.PopulationMax },
                            { "capacity", model.PopulationCap },
                            { "potential", model.PotentialPopulation }
                        }
                    }
                }
            }
        };
    }

    public static ASObject ToAppraisalSummaryDto(this World model)
    {
        var appraisalId = model.GetAppraisalId();

        var minimum = 0;
        var yield = 0;
        var maximum = 0;
        var capacity = 0;
        var potential = 0;

        if (appraisalId is not null)
        {
            foreach (var obj in model.Objects)
            {
                var appraisal = GameSettingsManager.Instance.GetItem(obj.GetItemName())?.GetAppraisal(appraisalId);

                if (appraisal is null) continue;

                if (obj.ClassName == BuildingClassType.ConstructionSite)
                {
                    potential += appraisal.EffectiveMin;
                    continue;
                }

                minimum += appraisal.EffectiveMin;
                yield += appraisal.EffectiveMin + Math.Clamp(obj.GetBonusAppraisal(), 0, Math.Max(0, appraisal.EffectiveMax - appraisal.EffectiveMin));
                maximum += appraisal.EffectiveMax;
                capacity += appraisal.Cap ?? 0;
            }
        }

        return new ASObject
        {
            { "id", appraisalId },
            { "minimum", minimum },
            { "yield", yield },
            { "maximum", maximum },
            { "capacity", capacity },
            { "potential", potential },
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
    
    // load Global.factoryWorkerManager
    public static ASObject ToFactoryWorkersAsObject(this World world)
    {
        var workers = new ASObject();

        foreach (var obj in world.Objects.Where(x => x.ClassName == BuildingClassType.Factory && x.ContractName is not null))
        {
            workers[$"w{obj.WorldFlatId}"] = new ASObject
            {
                {
                    "attributes", new ASObject
                    {
                        { "numPurchasedWorkers", obj.CountPurchasedWorkers() },
                        { "contractName", obj.ContractName! }
                    }
                },
                {
                    "members", obj.Workers.Select(object (x) => new ASObject
                    {
                        { "zid", x.Zid.ToString() },
                        { "data", new ASObject() }
                    }).ToList()
                }
            };
        }

        return workers;
    }

    // load Global.trainWorkerManager
    public static ASObject ToTrainWorkersAsObject(this World world)
    {
        var workers = new ASObject();

        if (world.TrainOrder is null) return workers;

        workers["w0"] = new ASObject
        {
            {
                "attributes", new ASObject
                {
                    { "trainName", world.TrainOrder.ItemName },
                    { "operation", world.TrainOrder.Operation.ToDescriptionString() },
                    { "commodityName", world.TrainOrder.CommodityName },
                    { "timeSent", world.TrainOrder.TimeSent },
                    { "numPurchasedWorkers", world.TrainOrder.CountPurchasedStops() }
                }
            },
            {
                "members", world.TrainOrder.Workers.Select(object (x) => new ASObject
                {
                    { "zid", x.Zid.ToString() },
                    { "data", new ASObject() }
                }).ToList()
            }
        };

        return workers;
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