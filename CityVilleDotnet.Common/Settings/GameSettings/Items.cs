using System.Xml.Serialization;

namespace CityVilleDotnet.Common.Settings.GameSettings;

[Serializable]
public class ItemsContainer
{
    [XmlElement("item")] public required List<GameItem?> Items { get; set; }
}

[Serializable]
public class GameItem
{
    [XmlAttribute("name")] public required string Name { get; set; }
    [XmlAttribute("type")] public required string Type { get; set; }
    [XmlAttribute("sellSendsToInventory")] public string? SellSendsToInventory { get; set; }
    [XmlAttribute("height")] public string? Height { get; set; }
    [XmlAttribute("width")] public string? Width { get; set; }

    [XmlElement("requiredLevel")]
    public string? RequiredLevelString
    {
        get => RequiredLevel?.ToString();
        set => RequiredLevel = string.IsNullOrEmpty(value) ? 0 : int.Parse(value);
    }

    [XmlIgnore] public int? RequiredLevel { get; set; }

    [XmlElement("requiredPopulation")] public int? RequiredPopulation { get; set; }
    [XmlElement("headquarters")] public string? HeadquartersName { get; set; }

    [XmlElement("population")] public PopulationItem? Population { get; set; }
    [XmlElement("upgrade")] public UpgradeItem? Upgrade { get; set; }

    [XmlElement("cost")] public int? Cost { get; set; }
    [XmlElement("unlock")] public string? Unlock { get; set; }

    // Support empty tag <unlockCost/>
    [XmlElement("unlockCost")]
    public string? UnlockCostString
    {
        get => UnlockCost?.ToString();
        set => UnlockCost = string.IsNullOrEmpty(value) ? 0 : int.Parse(value);
    }

    [XmlIgnore] public int? UnlockCost { get; set; }

    [XmlElement("cash")]
    public string? CashString
    {
        get => Cash?.ToString();
        set => Cash = string.IsNullOrEmpty(value) ? 0 : int.Parse(value);
    }

    [XmlIgnore] public int? Cash { get; set; }

    [XmlElement("growTime")] public double? GrowTime { get; set; }

    [XmlElement("coinYield")] public int? CoinYield { get; set; }

    [XmlElement("xpYield")] public int? XpYield { get; set; }

    [XmlElement("goodsYield")] public int? GoodsYield { get; set; }

    [XmlElement("construction")] public string? Construction { get; set; }

    [XmlElement("commodityReq")] public int? CommodityRequired { get; set; }
    [XmlElement("numberOfStages")] public int? NumberOfStages { get; set; }
    [XmlElement("energyCostPerBuild")] public int? EnergyCostPerBuild { get; set; }
    [XmlElement("energyRewards")] public int? EnergyRewards { get; set; }

    [XmlElement("randomModifiers")] public RandomModifiers? RandomModifiers { get; set; }
    [XmlElement("energyCost")] public EnergyCost? EnergyCost { get; set; }
    [XmlElement("gates")] public required GatesContainer Gates { get; set; }
    [XmlElement("keyword")] public List<string> Keywords { get; set; } = [];
    [XmlElement("mastery")] public required List<MasteryItem> MasteryItems { get; set; }

    public bool HasKeyword(string keyword)
    {
        return Keywords.Contains(keyword);
    }

    public List<GatesItem> GetGates()
    {
        return Gates.Gates ?? [];
    }

    public bool HasMasteries()
    {
        return MasteryItems.Count > 0;
    }
}

[Serializable]
public class EnergyCost
{
    [XmlAttribute("build")] public string? Build { get; set; }
    [XmlAttribute("instantFinish")] public string? InstantFinish { get; set; }
    [XmlAttribute("harvest")] public string? Harvest { get; set; }
    [XmlAttribute("open")] public string? Open { get; set; }
    [XmlAttribute("clean")] public string? Clean { get; set; }
    [XmlAttribute("clear")] public string? Clear { get; set; }
}

[Serializable]
public class GatesContainer
{
    [XmlElement("gate")] public required List<GatesItem>? Gates { get; set; }
}

[Serializable]
public class GatesItem
{
    [XmlAttribute("name")] public string? Name { get; set; }
    [XmlAttribute("type")] public string? Type { get; set; }
    [XmlAttribute("instructions")] public string? Instructions { get; set; }
    [XmlElement("key")] public required List<GateKey?> Keys { get; set; }
}

[Serializable]
public class GateKey
{
    [XmlAttribute("name")] public required string Name { get; set; }
    [XmlAttribute("viral")] public string? Viral { get; set; }
    [XmlAttribute("amount")] public int Amount { get; set; }
    [XmlAttribute("cashCost")] public string? CashCost { get; set; }
    [XmlElement("member")] public List<MemberKey>? Members { get; set; }
}

[Serializable]
public class MemberKey
{
    [XmlAttribute("name")] public required string Name { get; set; }
    [XmlAttribute("amount")] public int Amount { get; set; }
}

[Serializable]
public class PopulationItem
{
    [XmlAttribute("min")]
    public string? MinString
    {
        get => Min?.ToString();
        set => Min = string.IsNullOrEmpty(value) ? 0 : int.Parse(value);
    }

    [XmlIgnore] public int? Min { get; set; }

    [XmlAttribute("max")]
    public string? MaxString
    {
        get => Max?.ToString();
        set => Max = string.IsNullOrEmpty(value) ? 0 : int.Parse(value);
    }

    [XmlIgnore] public int? Max { get; set; }

    [XmlAttribute("cap")]
    public string? CapString
    {
        get => Cap?.ToString();
        set => Cap = string.IsNullOrEmpty(value) ? 0 : int.Parse(value);
    }

    [XmlIgnore] public int? Cap { get; set; }
}

[Serializable]
public class UpgradeItem
{
    [XmlAttribute("item")] public required string Name { get; set; }
    [XmlAttribute("cashcost")] public string? CashCost { get; set; }
}

[Serializable]
public class MasteryItem
{
    [XmlIgnore] public int? Level { get; set; }

    [XmlAttribute("level")]
    private string? LevelString
    {
        get => Level?.ToString();
        set => Level = string.IsNullOrEmpty(value) ? null : int.Parse(value);
    }

    [XmlIgnore] public int? RequiredCount { get; set; }

    [XmlAttribute("req")]
    private string? Req
    {
        get => RequiredCount?.ToString();
        set => RequiredCount = string.IsNullOrEmpty(value) ? null : int.Parse(value);
    }
}