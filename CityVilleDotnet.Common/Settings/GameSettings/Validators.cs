using System.Xml.Serialization;
using CityVilleDotnet.Common.Global;
using Microsoft.Extensions.Logging;

namespace CityVilleDotnet.Common.Settings.GameSettings;

[Serializable]
public class ValidatorsContainer
{
    [XmlElement("validate")] public List<ValidateItem> Validators { get; set; } = [];
}

[Serializable]
public class ValidateItem
{
    [XmlAttribute("name")] public required string Name { get; set; }

    [XmlElement("allOf")] public List<ValidateGroup> AllOf { get; set; } = [];
    [XmlElement("anyOf")] public List<ValidateGroup> AnyOf { get; set; } = [];

    public bool IsValid(int playerLevel)
    {
        return AllOf.All(x => x.IsAllValid(playerLevel)) && (AnyOf.Count == 0 || AnyOf.Any(x => x.IsAnyValid(playerLevel)));
    }
}

[Serializable]
public class ValidateGroup
{
    [XmlElement("func")] public List<ValidateFunc> Functions { get; set; } = [];

    [XmlElement("allOf")] public List<ValidateGroup> AllOf { get; set; } = [];
    [XmlElement("anyOf")] public List<ValidateGroup> AnyOf { get; set; } = [];

    public bool IsAllValid(int playerLevel)
    {
        return Functions.All(x => x.IsValid(playerLevel))
               && AllOf.All(x => x.IsAllValid(playerLevel))
               && (AnyOf.Count == 0 || AnyOf.Any(x => x.IsAnyValid(playerLevel)));
    }

    public bool IsAnyValid(int playerLevel)
    {
        return Functions.Any(x => x.IsValid(playerLevel))
               || AllOf.Any(x => x.IsAllValid(playerLevel))
               || AnyOf.Any(x => x.IsAnyValid(playerLevel));
    }
}

[Serializable]
public class ValidateFunc
{
    [XmlAttribute("_class")] public string? Class { get; set; }
    [XmlAttribute("_name")] public string? Name { get; set; }
    [XmlAttribute("level")] public int Level { get; set; }

    public bool IsValid(int playerLevel)
    {
        if (Name is "isLevelAtLeast" or "playerRequiredLevel")
            return playerLevel >= Level;

        StaticLogger.Current.LogWarning("Unsupported validation function {Class}.{Name}", Class, Name);

        return false;
    }
}
