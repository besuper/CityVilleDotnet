using System.Xml.Serialization;

namespace CityVilleDotnet.Common.Settings.GameSettings;

[Serializable]
public class DynamicExpansionsContainer
{
    [XmlElement("dynamicExpansion")] public List<DynamicExpansionItem> Expansions { get; set; } = [];
}

[Serializable]
public class DynamicExpansionItem
{
    [XmlAttribute("id")] public required string Id { get; set; }
    [XmlAttribute("grantFreeExpansionType")] public string? GrantFreeExpansionType { get; set; }
    [XmlAttribute("initializeAsFallbackFor")] public string? InitializeAsFallbackFor { get; set; }
    [XmlElement("teasers")] public DynamicExpansionTeasersContainer? Teasers { get; set; }
    [XmlElement("rewards")] public DynamicExpansionRewardsContainer? Rewards { get; set; }

    public List<DynamicExpansionObjectItem> GetTeasers() => Teasers?.Teasers ?? [];
    public List<DynamicExpansionObjectItem> GetRewards() => Rewards?.Rewards ?? [];

    public DynamicExpansionItem GetFallbackChainRoot()
    {
        if (InitializeAsFallbackFor is not null)
        {
            var parent = GameSettingsManager.Instance.GetDynamicExpansion(InitializeAsFallbackFor);

            if (parent is not null) return parent.GetFallbackChainRoot();
        }

        return this;
    }
}

[Serializable]
public class DynamicExpansionTeasersContainer
{
    [XmlElement("teaser")] public List<DynamicExpansionObjectItem> Teasers { get; set; } = [];
}

[Serializable]
public class DynamicExpansionRewardsContainer
{
    [XmlElement("reward")] public List<DynamicExpansionObjectItem> Rewards { get; set; } = [];
}

[Serializable]
public class DynamicExpansionObjectItem
{
    [XmlAttribute("itemName")] public required string ItemName { get; set; }
    [XmlAttribute("construction")] public string? ConstructionString { get; set; }
    [XmlAttribute("callback")] public string? Callback { get; set; }
    [XmlAttribute("inventoryOnly")] public string? InventoryOnlyString { get; set; }
    [XmlIgnore] public int XOffset { get; set; }

    [XmlAttribute("xOffset")]
    public string? XOffsetString
    {
        get => XOffset.ToString();
        set => XOffset = string.IsNullOrEmpty(value) ? 0 : int.Parse(value);
    }

    [XmlIgnore] public int YOffset { get; set; }

    [XmlAttribute("yOffset")]
    public string? YOffsetString
    {
        get => YOffset.ToString();
        set => YOffset = string.IsNullOrEmpty(value) ? 0 : int.Parse(value);
    }

    [XmlIgnore] public int Direction { get; set; }

    [XmlAttribute("direction")]
    public string? DirectionString
    {
        get => Direction.ToString();
        set => Direction = string.IsNullOrEmpty(value) ? 0 : int.Parse(value);
    }

    public bool IsConstruction() => ConstructionString == "true";
    public bool IsInventoryOnly() => InventoryOnlyString == "true";
}
