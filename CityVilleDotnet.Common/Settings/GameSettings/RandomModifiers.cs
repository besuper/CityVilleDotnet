using System.Globalization;
using System.Xml;
using System.Xml.Serialization;

namespace CityVilleDotnet.Common.Settings.GameSettings;

[Serializable]
public class RandomModifiers
{
    [XmlAttribute("name")] public string? Name { get; set; }

    [XmlElement("modifier")] public List<RandomModifier>? Modifiers { get; set; }
}

[Serializable]
public class RandomModifier
{
    [XmlAttribute("type")] public required string Type { get; set; }

    [XmlAttribute("tableName")] public required string TableName { get; set; }

    [XmlIgnore] public bool AllowOnBuild { get; set; }

    [XmlAttribute("allowOnBuild")]
    public string? AllowOnBuildString
    {
        get => AllowOnBuild.ToString().ToLower();
        set => AllowOnBuild = string.Equals(value, "true", StringComparison.OrdinalIgnoreCase);
    }

    [XmlAttribute("validate")] public string? Validate { get; set; }

    [XmlAttribute("experimentName")] public string? ExperimentName { get; set; }

    [XmlAttribute("variants")] public string? Variants { get; set; }

    [XmlIgnore] public double Multiplier { get; set; } = 1;

    [XmlAttribute("multiplier")]
    public string? MultiplierString
    {
        get => Multiplier.ToString(CultureInfo.InvariantCulture);
        set => Multiplier = string.IsNullOrEmpty(value) ? 1 : double.Parse(value, CultureInfo.InvariantCulture);
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
    public string? RollRangeString
    {
        get => RollRange.ToString();
        set => RollRange = string.IsNullOrEmpty(value) ? 99 : int.Parse(value);
    }

    [XmlIgnore] public bool Guarantee { get; set; }

    [XmlAttribute("guarantee")]
    public string? GuaranteeString
    {
        get => Guarantee.ToString().ToLower();
        set => Guarantee = string.Equals(value, "true", StringComparison.OrdinalIgnoreCase);
    }

    [XmlElement("roll")] public required List<Roll> Rolls { get; set; }
}

[Serializable]
public class Roll
{
    [XmlIgnore] public double Percent { get; set; }

    [XmlAttribute("percent")]
    public string? PercentString
    {
        get => Percent.ToString(CultureInfo.InvariantCulture);
        set => Percent = string.IsNullOrEmpty(value) ? 0 : double.Parse(value, CultureInfo.InvariantCulture);
    }

    [XmlIgnore] public double Divisor { get; set; } = 1;

    [XmlAttribute("divisor")]
    public string? DivisorString
    {
        get => Divisor.ToString(CultureInfo.InvariantCulture);
        set => Divisor = string.IsNullOrEmpty(value) ? 1 : double.Parse(value, CultureInfo.InvariantCulture);
    }

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

[Serializable]
public class RandomModifierPacksContainer
{
    [XmlElement("randomModifiers")] public List<RandomModifierPack>? Packs { get; set; }
}

[Serializable]
public class RandomModifierPack
{
    [XmlAttribute("id")] public required string Id { get; set; }

    [XmlElement("modifier")] public List<RandomModifier>? Modifiers { get; set; }
}

[Serializable]
public class RandomModifierGroupsContainer
{
    [XmlElement("group")] public List<RandomModifierGroup>? Groups { get; set; }
}

[Serializable]
public class RandomModifierGroup
{
    [XmlAttribute("name")] public required string Name { get; set; }

    [XmlElement("modifiers")] public List<RandomModifierGroupEntry>? Entries { get; set; }
}

[Serializable]
public class RandomModifierGroupEntry
{
    [XmlAttribute("name")] public required string Name { get; set; }

    [XmlIgnore] public double Percent { get; set; }

    [XmlAttribute("percent")]
    public string? PercentString
    {
        get => Percent.ToString(CultureInfo.InvariantCulture);
        set => Percent = string.IsNullOrEmpty(value) ? 0 : double.Parse(value, CultureInfo.InvariantCulture);
    }
}
