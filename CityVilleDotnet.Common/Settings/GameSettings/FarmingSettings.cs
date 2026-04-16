using System.Xml.Serialization;

namespace CityVilleDotnet.Common.Settings.GameSettings;

[Serializable]
[XmlRoot("farming")]
public class FarmingSettings
{
    [XmlAttribute("growMultiplier")] public required int GrowMultiplier { get; set; }
    [XmlAttribute("inGameDaySeconds")] public required int InGameDaySeconds { get; set; }
    [XmlAttribute("energyRegenerationSeconds")] public required double EnergyRegenerationSeconds { get; set; }

    [XmlAttribute("friendVisitShipRepGain")]
    public required int FriendVisitShipRepGain { get; set; }

    [XmlAttribute("friendVisitConstructionRepGain")]
    public required int FriendVisitConstructionRepGain { get; set; }

    [XmlAttribute("friendVisitPlotRepGain")]
    public required int FriendVisitPlotRepGain { get; set; }

    [XmlAttribute("friendHelpDefaultGoodsReward")]
    public required int FriendHelpDefaultGoodsReward { get; set; }

    [XmlAttribute("friendVisitBusinessRepGain")]
    public required int FriendVisitBusinessRepGain { get; set; }

    [XmlAttribute("friendHelpDefaultCoinReward")]
    public required int FriendHelpDefaultCoinReward { get; set; }

    [XmlAttribute("friendVisitWildernessRepGain")]
    public required int FriendVisitWildernessRepGain { get; set; }

    [XmlAttribute("friendVisitResidenceRepGain")]
    public required int FriendVisitResidenceRepGain { get; set; }
    
    [XmlAttribute("franchise1DailyBonus")]
    public required int Franchise1DailyBonus { get; set; }

    [XmlAttribute("welcomeTrainQuestAmount")] public int WelcomeTrainQuestAmount { get; set; }
    
    [XmlAttribute("boostGrowMultiplier")] public double BoostGrowMultiplier { get; set; }
    [XmlAttribute("boostGrowInstantHourLimit")] public int BoostGrowInstantHourLimit { get; set; }
}