using System.Text.Json.Serialization;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.EnumExtensions;
using CityVilleDotnet.Domain.Enums;

namespace CityVilleDotnet.Domain.GameEntities;

public class WorldSummaryDto
{
    [JsonPropertyName("world_id")] public required string WorldId { get; set; }

    [JsonPropertyName("ftueCompleted")] public bool FtueCompleted { get; set; }

    [JsonPropertyName("items_by_name")] public required Dictionary<string, int> ItemsByName { get; set; }

    [JsonPropertyName("construction_items")] public required Dictionary<string, int> ConstructionItems { get; set; }

    // TODO: Implement containers
    [JsonPropertyName("malls_items")] public EmptyDto MallsItems { get; set; } = new();

    // TODO: Implement specials expansions
    [JsonPropertyName("incentivized_expansion")] public EmptyDto IncentivizedExpansion { get; set; } = new();

    [JsonPropertyName("numberOfExpansions")] public int NumberOfExpansions { get; set; }

    [JsonPropertyName("number_of_business")] public int NumberOfBusiness { get; set; }

    [JsonPropertyName("populationSummary")] public required PopulationSummaryDto PopulationSummary { get; set; }

    [JsonPropertyName("appraisalSummary")] public required SimSummaryDto AppraisalSummary { get; set; }

    [JsonPropertyName("commoditySummary")] public required CommoditySummaryDto CommoditySummary { get; set; }

    [JsonPropertyName("savedQuestSequence")] public required List<string> SavedQuestSequence { get; set; }

    [JsonPropertyName("questManagerQuests")] public required List<string> QuestManagerQuests { get; set; }

    [JsonPropertyName("hiddenQuests")] public required List<string> HiddenQuests { get; set; }
}

public static class WorldSummaryDtoMapper
{
    public static WorldSummaryDto ToWorldSummaryDto(this World world, Player model, List<Quest> activeQuests)
    {
        return new WorldSummaryDto
        {
            WorldId = world.Type.ToDescriptionString(),
            FtueCompleted = world.Type == WorldType.Main ? !model.IsNew : world.IsFtueCompleted(),
            ItemsByName = world.Objects
                .Where(x => x.ClassName != BuildingClassType.ConstructionSite)
                .GroupBy(x => x.ItemName)
                .ToDictionary(g => g.Key, g => g.Count()),
            ConstructionItems = world.Objects
                .Where(x => x.ClassName == BuildingClassType.ConstructionSite && x.TargetBuildingName != null)
                .GroupBy(x => x.TargetBuildingName!)
                .ToDictionary(g => g.Key, g => g.Count()),
            NumberOfExpansions = model.ExpansionsPurchased,
            NumberOfBusiness = world.Objects.Count(x => x.ClassName == BuildingClassType.Business),
            PopulationSummary = world.ToPopulationSummaryDto(),
            AppraisalSummary = world.ToAppraisalSummaryDto(),
            CommoditySummary = world.ToCommoditySummaryDto(),
            SavedQuestSequence = GetQuestNames(activeQuests, QuestLocation.Sidebar),
            QuestManagerQuests = GetQuestNames(activeQuests, QuestLocation.Menu),
            HiddenQuests = GetQuestNames(activeQuests, QuestLocation.Hidden)
        };
    }

    private static List<string> GetQuestNames(List<Quest> quests, QuestLocation location)
    {
        return quests.Where(q => q.Location == location).OrderBy(q => q.Order).Select(q => q.Name).ToList();
    }
}
