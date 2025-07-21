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
    [XmlElement("collectables")] public required CollectableContainer Collectables { get; set; }
    [XmlElement("tradeInReward")] public required TradeInRewards TradeInRewards { get; set; }
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
public class TradeInReward
{
    [XmlAttribute("amount")] public string? Amount { get; set; }
    [XmlAttribute("name")] public string? Name { get; set; }
}