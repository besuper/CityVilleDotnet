using System.Globalization;
using System.Xml;
using System.Xml.Serialization;

namespace CityVilleDotnet.Common.Settings.GameSettings;

[Serializable]
public class RandomModifiers
{
    [XmlElement("modifier")] public List<RandomModifier>? Modifiers { get; set; }
}

[Serializable]
public class RandomModifier
{
    [XmlAttribute("type")] public required string Type { get; set; }

    [XmlAttribute("tableName")] public required string TableName { get; set; }

    [XmlIgnore] public bool AllowOnBuild { get; set; }

    [XmlAttribute("allowOnBuild")]
    private string? AllowOnBuildString
    {
        get => AllowOnBuild.ToString().ToLower();
        set => AllowOnBuild = string.Equals(value, "true", StringComparison.OrdinalIgnoreCase);
    }
}

[Serializable]
public class RandomModifierTables
{
    [XmlElement("randomModifierTable")] public required List<RandomModifierTable> Table { get; set; }
}

[Serializable]
public class RandomModifierTable
{
    [XmlAttribute("type")] public required string Type { get; set; }

    [XmlAttribute("name")] public required string Name { get; set; }

    [XmlIgnore] public int RollRange { get; set; } = 99;

    [XmlAttribute("rollRange")]
    private string? RollRangeString
    {
        get => RollRange.ToString();
        set => RollRange = string.IsNullOrEmpty(value) ? 99 : int.Parse(value);
    }

    [XmlElement("roll")] public required List<Roll> Rolls { get; set; }
}

[Serializable]
public class Roll
{
    [XmlAttribute("percent")] public int Percent { get; set; }

    [XmlIgnore] public Dictionary<string, List<AmountElement>> Rewards { get; set; } = new();

    [XmlAnyElement] public required XmlElement[] RewardElements { get; set; }

    public void OnDeserialized()
    {
        if (RewardElements is not null)
        {
            foreach (var element in RewardElements)
            {
                var rewardType = element.Name;
                var amountAttribute = element.GetAttribute("amount");

                var amount = amountAttribute == "" ? 0 : double.Parse(amountAttribute, CultureInfo.InvariantCulture);
                var name = element.GetAttribute("name");

                if (!Rewards.ContainsKey(rewardType))
                {
                    Rewards[rewardType] = [];
                }

                Rewards[rewardType].Add(new AmountElement { Amount = amount, Name = name });
            }
        }
    }
}

[Serializable]
public class AmountElement
{
    [XmlAttribute("amount")] public double Amount { get; set; }

    [XmlAttribute("name")] public required string Name { get; set; }
}