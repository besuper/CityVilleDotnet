using System.Xml.Serialization;

namespace CityVilleDotnet.Common.Settings.GameSettings;

[Serializable]
[XmlRoot("farming")]
public class FarmingSettings
{
    [XmlAttribute("growMultiplier")] public required int GrowMultiplier { get; set; }
    [XmlAttribute("witherMultiplier")] public required double WitherMultiplier { get; set; }
    [XmlAttribute("witherRefundMultiplier")] public required double WitherRefundMultiplier { get; set; }
    [XmlAttribute("plowCost")] public int PlowCost { get; set; }
    [XmlAttribute("plowXp")] public int PlowXp { get; set; }
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

    [XmlAttribute("trainBonusMult")] public double TrainBonusMult { get; set; }

    [XmlAttribute("remodelingRequiredLevel")] public int RemodelingRequiredLevel { get; set; }
    
    [XmlAttribute("boostGrowMultiplier")] public double BoostGrowMultiplier { get; set; }
    [XmlAttribute("boostGrowInstantHourLimit")] public int BoostGrowInstantHourLimit { get; set; }
    [XmlAttribute("instantReadyCropCostConstant2")] public double InstantReadyCropCostConstant2 { get; set; }
    [XmlAttribute("instantReadyCropCostConstant3")] public double InstantReadyCropCostConstant3 { get; set; }
    [XmlAttribute("instantReadyCropCostConstant4")] public double InstantReadyCropCostConstant4 { get; set; }
    [XmlAttribute("instantReadyCropCostConstant5")] public double InstantReadyCropCostConstant5 { get; set; }
    [XmlAttribute("instantReadyCropCostConstant6")] public double InstantReadyCropCostConstant6 { get; set; }

    [XmlAttribute("instantReadyResidenceCostConstant2")] public double InstantReadyResidenceCostConstant2 { get; set; }
    [XmlAttribute("instantReadyResidenceCostConstant3")] public double InstantReadyResidenceCostConstant3 { get; set; }
    [XmlAttribute("instantReadyResidenceCostConstant4")] public double InstantReadyResidenceCostConstant4 { get; set; }
    [XmlAttribute("instantReadyResidenceCostConstant5")] public double InstantReadyResidenceCostConstant5 { get; set; }
    [XmlAttribute("instantReadyResidenceCostConstant6")] public double InstantReadyResidenceCostConstant6 { get; set; }
    [XmlAttribute("instantReadyResidenceCostConstant7")] public double InstantReadyResidenceCostConstant7 { get; set; }
    
    [XmlAttribute("ZooDonationNPCPrice_enclosure_jungle")] public int ZooDonationNpcPriceEnclosureJungle { get; set; }
    [XmlAttribute("ZooDonationNPCPrice_enclosure_savannah")] public int ZooDonationNpcPriceEnclosureSavannah { get; set; }
    [XmlAttribute("ZooDonationNPCPrice_enclosure_mountain")] public int ZooDonationNpcPriceEnclosureMountain { get; set; }
    [XmlAttribute("ZooDonationNPCPrice_enclosure_arctic")] public int ZooDonationNpcPriceEnclosureArctic { get; set; }

    [XmlAttribute("startingEnergy")] public int StartingEnergy { get; set; }
    [XmlAttribute("startingEnergyMax")] public int StartingEnergyMax { get; set; }
    [XmlAttribute("startingGold")] public int StartingGold { get; set; }
    [XmlAttribute("startingCash")] public int StartingCash { get; set; }
    [XmlAttribute("startingLevel")] public int StartingLevel { get; set; }
    [XmlAttribute("startingLightLevel")] public int StartingLightLevel { get; set; }
    [XmlAttribute("startingXp")] public int StartingXp { get; set; }
    [XmlAttribute("startingCommodities")] public int StartingCommodities { get; set; }
    [XmlAttribute("cashGainedPerLevel")] public int CashGainedPerLevel { get; set; }
    [XmlAttribute("crewMemberCashCost")] public int CrewMemberCashCost { get; set; }

    public int GetZooDonationNpcPrice(string enclosureItemName)
    {
        var price = enclosureItemName switch
        {
            "enclosure_jungle" => ZooDonationNpcPriceEnclosureJungle,
            "enclosure_savannah" => ZooDonationNpcPriceEnclosureSavannah,
            "enclosure_mountain" => ZooDonationNpcPriceEnclosureMountain,
            "enclosure_arctic" => ZooDonationNpcPriceEnclosureArctic,
            _ => 0
        };

        // same fallback as the client: getInt("ZooDonationNPCPrice_" + name, 20)
        return price > 0 ? price : 20;
    }
}