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
    [XmlAttribute("height")] public string? Height { get; set; }
    [XmlAttribute("width")] public string? Width { get; set; }

    [XmlElement("requiredLevel")] public int? RequiredLevel { get; set; }

    [XmlElement("requiredPopulation")] public int? RequiredPopulation { get; set; }

    [XmlElement("populationYield")] public int? PopulationYield { get; set; }

    [XmlElement("populationCapYield")] public int? PopulationCapYield { get; set; }

    [XmlElement("cost")] public int? Cost { get; set; }
    [XmlElement("growTime")] public double? GrowTime { get; set; }

    [XmlElement("coinYield")] public int? CoinYield { get; set; }

    [XmlElement("xpYield")] public int? XpYield { get; set; }

    [XmlElement("goodsYield")] public int? GoodsYield { get; set; }

    [XmlElement("construction")] public required string Construction { get; set; }

    [XmlElement("commodityReq")] public int? CommodityRequired { get; set; }

    [XmlElement("randomModifiers")] public RandomModifiers? RandomModifiers { get; set; }
}