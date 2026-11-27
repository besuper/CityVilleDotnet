using CityVilleDotnet.Domain.Entities;
using System.Text.Json.Serialization;
using CityVilleDotnet.Domain.EnumExtensions;
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

    [JsonPropertyName("macroObjects")] public Dictionary<string, MacroObjectDto?> MacroObjects { get; set; } = [];

    [JsonPropertyName("macroObjectIdCounter")]
    public int MacroObjectIdCounter { get; set; }

    // The client reads it from the cached world blob when switching worlds (OpenWorld.setupWorld)
    [JsonPropertyName("featureData")] public WorldFeatureDataDto? FeatureData { get; set; }
}

public static class WorldDtoMapper
{
    public static WorldDto ToDto(this World model)
    {
        var coupons = model.Player?.Coupons.Where(x => x.WorldFlatId is null).Select(x => x.Name).ToList();

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
            Objects = model.Objects.Where(x => x.StoredInId is null).Select(x => x.ToDto(model.Objects, coupons)).ToList(),
            ThemeCollections = model.ThemeCollections,
            LastExpansionTier = model.LastExpansionTier,
            WorldId = model.Type.ToDescriptionString(),
            WorldCreated = model.WorldCreated,
            MacroObjects = model.ToMacroObjectsDto(),
            MacroObjectIdCounter = model.MacroObjectIdCounter,
            FeatureData = new WorldFeatureDataDto { IncentivizedExpansions = model.ToIncentivizedExpansionsDto() }
        };
    }
}