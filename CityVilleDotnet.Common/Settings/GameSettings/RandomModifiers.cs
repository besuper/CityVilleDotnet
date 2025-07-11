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

                var amount = amountAttribute == "" ? 0 : double.Parse(amountAttribute);
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