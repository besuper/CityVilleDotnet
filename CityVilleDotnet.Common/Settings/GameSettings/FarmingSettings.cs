using System.Xml.Serialization;

namespace CityVilleDotnet.Common.Settings.GameSettings;

[Serializable]
[XmlRoot("farming")]
public class FarmingSettings
{
    [XmlAttribute("inGameDaySeconds")] public required string InGameDaySeconds { get; set; }

    [XmlAttribute("friendVisitShipRepGain")]
    public required string FriendVisitShipRepGain { get; set; }

    [XmlAttribute("friendVisitConstructionRepGain")]
    public required string FriendVisitConstructionRepGain { get; set; }

    [XmlAttribute("friendVisitPlotRepGain")]
    public required string FriendVisitPlotRepGain { get; set; }

    [XmlAttribute("friendHelpDefaultGoodsReward")]
    public required string FriendHelpDefaultGoodsReward { get; set; }

    [XmlAttribute("friendVisitBusinessRepGain")]
    public required string FriendVisitBusinessRepGain { get; set; }

    [XmlAttribute("friendHelpDefaultCoinReward")]
    public required string FriendHelpDefaultCoinReward { get; set; }

    [XmlAttribute("friendVisitWildernessRepGain")]
    public required string FriendVisitWildernessRepGain { get; set; }

    [XmlAttribute("friendVisitResidenceRepGain")]
    public required string FriendVisitResidenceRepGain { get; set; }
}