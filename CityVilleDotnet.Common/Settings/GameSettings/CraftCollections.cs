using System.Xml.Serialization;

namespace CityVilleDotnet.Common.Settings.GameSettings;

[Serializable]
[XmlRoot("craftingCollections")]
public class CraftCollectionsContainer
{
    [XmlElement("collection")] public List<CraftCollection> Collections { get; set; } = [];
}

[Serializable]
public class CraftCollection
{
    [XmlAttribute("name")] public required string Name { get; set; }
    [XmlElement("droptable")] public CraftDropTable? DropTable { get; set; }
    [XmlElement("collectable")] public List<CraftCollectable> Collectables { get; set; } = [];
    [XmlElement("tradeInReward")] public required CraftTradeInReward TradeInReward { get; set; }
}

[Serializable]
public class CraftDropTable
{
    [XmlAttribute("table")] public required string Table { get; set; }
}

[Serializable]
public class CraftTradeInReward
{
    [XmlElement("item")] public required CraftCollectable Item { get; set; }
}

[Serializable]
public class CraftCollectable
{
    [XmlAttribute("name")] public required string Name { get; set; }
    [XmlAttribute("quantity")] public int Quantity { get; set; }
    [XmlAttribute("amount")] public int Amount { get; set; }
}
