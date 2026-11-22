using System.Text.Json.Serialization;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;

namespace CityVilleDotnet.Domain.GameEntities;

public class SimSummaryDto
{
    [JsonPropertyName("id")] public string? Id { get; set; }

    [JsonPropertyName("minimum")] public int Minimum { get; set; }

    [JsonPropertyName("yield")] public int Yield { get; set; }

    [JsonPropertyName("maximum")] public int Maximum { get; set; }

    [JsonPropertyName("capacity")] public int Capacity { get; set; }

    [JsonPropertyName("potential")] public int Potential { get; set; }
}

public static class SimSummaryDtoMapper
{
    public static SimSummaryDto ToAppraisalSummaryDto(this World model)
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

        return new SimSummaryDto
        {
            Id = appraisalId,
            Minimum = minimum,
            Yield = yield,
            Maximum = maximum,
            Capacity = capacity,
            Potential = potential
        };
    }
}
