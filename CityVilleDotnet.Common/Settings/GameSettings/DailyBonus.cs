using System.Xml.Serialization;

namespace CityVilleDotnet.Common.Settings.GameSettings;

[Serializable]
public class DailyBonusContainer
{
    [XmlElement("consecutiveDay")] public List<DailyBonusDay> Days { get; set; } = [];
}

[Serializable]
public class DailyBonusRewards
{
    [XmlElement("coin")] public DailyBonusAmount? Coin { get; set; }
    [XmlElement("cash")] public DailyBonusAmount? Cash { get; set; }
    [XmlElement("energy")] public DailyBonusAmount? Energy { get; set; }
    [XmlElement("goods")] public DailyBonusAmount? Goods { get; set; }
    [XmlElement("xp")] public DailyBonusAmount? Xp { get; set; }
    [XmlElement("item")] public List<DailyBonusItem> Items { get; set; } = [];
    
    public string? RollItemName()
    {
        var cumulative = new[] { Coin, Cash, Energy, Goods, Xp }.Sum(x => x?.Weight ?? 0);
        var roll = Random.Shared.Next(cumulative + Items.Sum(x => x.Weight));

        if (roll < cumulative)
        {
            return null; // Returns null when the roll lands on a currency reward
        }

        foreach (var item in Items)
        {
            cumulative += item.Weight;

            if (roll < cumulative)
            {
                return item.Name;
            }
        }

        return null;
    }
}

[Serializable]
public class DailyBonusDay : DailyBonusRewards
{
    [XmlAttribute("number")] public int Number { get; set; }
    [XmlAttribute("_zexp")] public string? ExperimentName { get; set; }
    [XmlAttribute("_zvar")] public string? Variants { get; set; }
    [XmlElement("randomchance")] public DailyBonusRewards? RandomChance { get; set; }

    // ExperimentManager::filterXmlByExperiment
    public bool IsInExperiment(IReadOnlyDictionary<string, int> experiments)
    {
        if (ExperimentName is null || Variants is null)
        {
            return true;
        }

        var variant = experiments.GetValueOrDefault(ExperimentName).ToString();

        return Variants.Split(',').Contains(variant);
    }
}

[Serializable]
public class DailyBonusAmount
{
    [XmlAttribute("amount")] public int Amount { get; set; }
    [XmlAttribute("weight")] public int Weight { get; set; }
}

[Serializable]
public class DailyBonusItem
{
    [XmlAttribute("name")] public required string Name { get; set; }
    [XmlAttribute("weight")] public int Weight { get; set; }
}
