using System.Text.Json.Serialization;
using CityVilleDotnet.Domain.Entities;

namespace CityVilleDotnet.Domain.GameEntities;

public class MacroObjectDto
{
    [JsonPropertyName("parentMacroItemName")] public required string ParentMacroItemName { get; set; }

    [JsonPropertyName("children")] public required List<string> Children { get; set; }
}

public static class MacroObjectDtoMapper
{
    public static Dictionary<string, MacroObjectDto?> ToMacroObjectsDto(this World model)
    {
        return model.MacroObjects.ToDictionary(x => x.Name, MacroObjectDto? (macroObject) => new MacroObjectDto
        {
            ParentMacroItemName = macroObject.ParentItemName,
            Children = model.Objects
                .Where(x => x.ParentMacroObjectId == macroObject.Name)
                .Select(x => x.WorldFlatId.ToString())
                .ToList()
        });
    }
}
