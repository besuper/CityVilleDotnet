using System.Xml.Serialization;

namespace CityVilleDotnet.Common.Settings.GameSettings;

[Serializable]
public class ViralSetting
{
    [XmlAttribute("name")] public required string Name { get; set; }
    [XmlElement("timeTillReset")] public int? TimeTillReset { get; set; }
    [XmlElement("maxHostRewards")] public int? MaxHostRewards { get; set; }
    [XmlElement("reward")] public ViralRewardBlock? Reward { get; set; }
}

[Serializable]
public class ViralRewardBlock
{
    [XmlElement("expirationTime")] public int? ExpirationTime { get; set; }
    [XmlElement("maxHelpers")] public int? MaxHelpers { get; set; }
    [XmlArray("hostRewards"), XmlArrayItem("reward")] public List<ViralReward> HostRewards { get; set; } = [];
    [XmlArray("helperRewards"), XmlArrayItem("reward")] public List<ViralReward> HelperRewards { get; set; } = [];
}

[Serializable]
public class ViralReward
{
    [XmlAttribute("type")] public required string Type { get; set; }
    [XmlElement("data")] public ViralRewardData? Data { get; set; }

    [XmlIgnore] public int Amount => Data?.Amount ?? 0;
    [XmlIgnore] public string? ItemName => Data?.ItemName;
}

[Serializable]
public class ViralRewardData
{
    [XmlAttribute("amount")] public int Amount { get; set; }
    [XmlAttribute("itemName")] public string? ItemName { get; set; }
}
