using System.Xml.Serialization;

namespace CityVilleDotnet.Common.Settings.GameSettings;

[Serializable]
public class LootTablesContainer
{
    [XmlElement("lootTable")] public required List<LootTable> Tables { get; set; }
}

[Serializable]
public class LootTable
{
    [XmlAttribute("name")] public required string Name { get; set; }

    [XmlIgnore] public int TotalWeight { get; set; }

    [XmlAttribute("totalWeight")]
    public string? TotalWeightString
    {
        get => TotalWeight.ToString();
        set => TotalWeight = string.IsNullOrEmpty(value) ? 0 : int.Parse(value);
    }

    [XmlElement("lootItem")] public required List<LootItem> Items { get; set; }

    public string RollItemName()
    {
        var totalWeight = TotalWeight > 0 ? TotalWeight : Items.Sum(x => x.Weight);
        var roll = Random.Shared.Next(totalWeight);
        var cumulative = 0;

        foreach (var item in Items)
        {
            cumulative += item.Weight;

            if (roll < cumulative)
                return item.ItemName;
        }

        return Items[^1].ItemName;
    }
}

[Serializable]
public class LootItem
{
    [XmlAttribute("handler")] public string? Handler { get; set; }

    [XmlAttribute("itemName")] public required string ItemName { get; set; }

    [XmlIgnore] public int Weight { get; set; }

    [XmlAttribute("weight")]
    public string? WeightString
    {
        get => Weight.ToString();
        set => Weight = string.IsNullOrEmpty(value) ? 0 : int.Parse(value);
    }
}
