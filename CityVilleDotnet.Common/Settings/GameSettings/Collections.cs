using System.Xml.Serialization;

namespace CityVilleDotnet.Common.Settings.GameSettings;

[Serializable]
public class CollectionContainer
{
    [XmlElement("collection")] public required List<CollectionSetting> Collections { get; set; }
}

[Serializable]
[XmlRoot("collection")]
public class CollectionSetting
{
    [XmlAttribute("name")] public required string Name { get; set; }
    [XmlAttribute("autoTradeIn")] public string? AutoTradeInString { get; set; }
    [XmlAttribute("tradeInRemove")] public string? TradeInRemoveString { get; set; }
    [XmlElement("collectables")] public required CollectableContainer Collectables { get; set; }
    [XmlElement("tradeInReward")] public required TradeInRewards TradeInRewards { get; set; }

    [XmlArray("oneTimeRewards")]
    [XmlArrayItem("oneTimeReward")]
    public List<OneTimeReward> OneTimeRewards { get; set; } = [];

    public bool IsAutoTradeIn() => AutoTradeInString == "true";
    public bool IsTradeInRemove() => TradeInRemoveString != "false";

    // Collection::getRewardXml
    public TradeInRewards GetTradeInRewards(int tradeIns) => OneTimeRewards.FirstOrDefault(x => x.Count == tradeIns) ?? TradeInRewards;
}

[Serializable]
public class CollectableContainer
{
    [XmlElement("collectable")] public required List<Collectable> Collectables { get; set; }
}

[Serializable]
[XmlRoot("collectable")]
public class Collectable
{
    [XmlAttribute("name")] public required string Name { get; set; }
    [XmlIgnore] public int TradeInCount { get; set; } = 1;

    [XmlAttribute("tradeInCount")]
    public string? TradeInCountString
    {
        get => TradeInCount.ToString();
        set => TradeInCount = int.TryParse(value, out var count) && count > 0 ? count : 1;
    }

    [XmlElement("pickUpReward")] public PickUpRewards? PickUpRewards { get; set; }
}

[Serializable]
public class PickUpRewards
{
    [XmlElement("xp")] public List<PickUpReward> XpRewards { get; set; } = [];
    [XmlElement("coin")] public List<PickUpReward> CoinRewards { get; set; } = [];
    [XmlElement("goods")] public List<PickUpReward> GoodsRewards { get; set; } = [];
    [XmlElement("energy")] public List<PickUpReward> EnergyRewards { get; set; } = [];
    [XmlElement("permits")] public List<PickUpReward> PermitsRewards { get; set; } = [];
    [XmlElement("item")] public List<PickUpReward> ItemRewards { get; set; } = [];

    public IEnumerable<(string Type, PickUpReward Reward)> GetReached(int ownedBefore, int ownedAfter)
    {
        return XpRewards.Select(x => ("xp", x))
            .Concat(CoinRewards.Select(x => ("coin", x)))
            .Concat(GoodsRewards.Select(x => ("goods", x)))
            .Concat(EnergyRewards.Select(x => ("energy", x)))
            .Concat(PermitsRewards.Select(x => ("permits", x)))
            .Concat(ItemRewards.Select(x => ("item", x)))
            .Where(x => x.Item2.IsReached(ownedBefore, ownedAfter));
    }
}

[Serializable]
public class PickUpReward
{
    [XmlAttribute("name")] public string? Name { get; set; }
    [XmlAttribute("amount")] public int Amount { get; set; } = 1;
    [XmlIgnore] public int? Count { get; set; }

    [XmlAttribute("count")]
    public string? CountString
    {
        get => Count?.ToString();
        set => Count = string.IsNullOrEmpty(value) ? null : int.Parse(value);
    }

    public bool IsReached(int ownedBefore, int ownedAfter) => Count is null || (Count > ownedBefore && Count <= ownedAfter);
}

[Serializable]
public class TradeInRewards
{
    [XmlElement("xp")] public required List<TradeInReward> XpRewards { get; set; }
    [XmlElement("coin")] public required List<TradeInReward> CoinRewards { get; set; }
    [XmlElement("goods")] public required List<TradeInReward> GoodsRewards { get; set; }
    [XmlElement("energy")] public required List<TradeInReward> EnergyRewards { get; set; }
    [XmlElement("item")] public required List<TradeInReward> ItemRewards { get; set; }
    [XmlIgnore] public Dictionary<string, List<TradeInReward>> Rewards { get; set; } = [];
    
    public void OnDeserialized()
    {
        Rewards = new Dictionary<string, List<TradeInReward>>();
        
        if(XpRewards.Count > 0)
            Rewards["xp"] = XpRewards;
        
        if(CoinRewards.Count > 0)
            Rewards["coin"] = CoinRewards;
        
        if(GoodsRewards.Count > 0)
            Rewards["goods"] = GoodsRewards;
        
        if(EnergyRewards.Count > 0)
            Rewards["energy"] = EnergyRewards;
        
        if(ItemRewards.Count > 0)
            Rewards["item"] = ItemRewards;
    }
}

[Serializable]
public class OneTimeReward : TradeInRewards
{
    [XmlAttribute("count")] public int Count { get; set; }
}

[Serializable]
public class TradeInReward
{
    [XmlAttribute("amount")] public string? Amount { get; set; }
    [XmlAttribute("name")] public string? Name { get; set; }
}