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
        return IsValid(x => x.IsValid(playerLevel));
    }

    public bool IsValid(Func<ValidateFunc, bool> isFuncValid)
    {
        return AllOf.All(x => x.IsAllValid(isFuncValid)) && (AnyOf.Count == 0 || AnyOf.Any(x => x.IsAnyValid(isFuncValid)));
    }
}

[Serializable]
public class ValidateGroup
{
    [XmlElement("func")] public List<ValidateFunc> Functions { get; set; } = [];

    [XmlElement("allOf")] public List<ValidateGroup> AllOf { get; set; } = [];
    [XmlElement("anyOf")] public List<ValidateGroup> AnyOf { get; set; } = [];

    public bool IsAllValid(Func<ValidateFunc, bool> isFuncValid)
    {
        return Functions.All(isFuncValid)
               && AllOf.All(x => x.IsAllValid(isFuncValid))
               && (AnyOf.Count == 0 || AnyOf.Any(x => x.IsAnyValid(isFuncValid)));
    }

    public bool IsAnyValid(Func<ValidateFunc, bool> isFuncValid)
    {
        return Functions.Any(isFuncValid)
               || AllOf.Any(x => x.IsAllValid(isFuncValid))
               || AnyOf.Any(x => x.IsAnyValid(isFuncValid));
    }
}

[Serializable]
public class ValidateFunc
{
    [XmlAttribute("_class")] public string? Class { get; set; }
    [XmlAttribute("_name")] public string? Name { get; set; }
    [XmlAttribute("level")] public int Level { get; set; }
    [XmlAttribute("type")] public string? Type { get; set; }
    [XmlAttribute("keyword")] public string? Keyword { get; set; }
    [XmlAttribute("itemName")] public string? ItemName { get; set; }
    [XmlAttribute("check")] public string? Check { get; set; }

    public bool IsValid(int playerLevel)
    {
        if (Name is "isLevelAtLeast" or "playerRequiredLevel")
            return playerLevel >= Level;

        StaticLogger.Current.LogWarning("Unsupported validation function {Class}.{Name}", Class, Name);

        return false;
    }
}
