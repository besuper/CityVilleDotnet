using CityVilleDotnet.Common.Settings.GameSettings;
using CityVilleDotnet.Domain.Enums;

namespace CityVilleDotnet.Domain.EnumExtensions;

public static class BuildingClassTypeExtension
{
    private static readonly List<BuildingClassType> AllowedBusiness = [BuildingClassType.Business, BuildingClassType.SocialBusiness, BuildingClassType.Hotel, BuildingClassType.TimedBusiness];
    private static readonly List<BuildingClassType> IsStackableList = [BuildingClassType.Decoration, BuildingClassType.Road, BuildingClassType.ParkingLot , BuildingClassType.Sidewalk, BuildingClassType.GreenHouse, BuildingClassType.Airplane, BuildingClassType.Ship, BuildingClassType.HarvestableShip, BuildingClassType.Amphitheater];

    private static readonly List<BuildingClassType> HarvestableResources =
    [
        BuildingClassType.Corporation, BuildingClassType.CustomResidence, BuildingClassType.FerryShip,
        BuildingClassType.HarvestableShip, BuildingClassType.Landmark,
        BuildingClassType.Municipal, BuildingClassType.Plot, BuildingClassType.Residence,
        BuildingClassType.Ship, BuildingClassType.Skyscraper, BuildingClassType.University,
        BuildingClassType.XPromoBuilding, BuildingClassType.ZooEnclosure
    ];

    // Copy of the client ItemClassNameDefinitions
    private static readonly Dictionary<string, string> TypeToClassName = new()
    {
        ["business"] = "Business", ["decoration"] = "Decoration", ["npc"] = "NPC", ["pier"] = "Pier",
        ["plot"] = "Plot", ["ship"] = "Ship", ["harvest_ship"] = "HarvestableShip", ["residence"] = "Residence",
        ["road"] = "Road", ["sidewalk"] = "Sidewalk", ["parking_lot"] = "ParkingLot", ["bridge"] = "Bridge",
        ["bridgepart"] = "BridgePart", ["storage"] = "Storage", ["train_station"] = "TrainStation",
        ["moneyVault"] = "MoneyVault", ["wilderness"] = "Wilderness", ["municipal"] = "Municipal",
        ["landmark"] = "Landmark", ["itemStorage"] = "ItemStorage", ["factory"] = "Factory",
        ["dockHouse"] = "DockHouse", ["environmentTile"] = "EnvironmentTile", ["dock"] = "Dock",
        ["zooEnclosure"] = "ZooEnclosure", ["customResidence"] = "CustomResidence", ["greenHouse"] = "GreenHouse",
        ["mimicItem"] = "MapResourceMimic", ["garden"] = "Garden", ["neighborhood"] = "Neighborhood",
        ["mall"] = "Mall", ["municipalcenter"] = "MunicipalCenter", ["skyscraper"] = "Skyscraper",
        ["hotel"] = "Hotel", ["socialbusiness"] = "SocialBusiness", ["powerstation"] = "PowerStation",
        ["taxidispatch"] = "TaxiDispatch", ["craftingcenter"] = "CraftingCenter", ["attraction"] = "Attraction",
        ["wonder"] = "Wonder", ["energyToggle"] = "EnergyToggleDeco", ["university"] = "University",
        ["holiday_town"] = "HolidayTown", ["dock_house"] = "DockHouse", ["airplane"] = "Airplane",
        ["truck"] = "Truck", ["mechanicMapResource"] = "MechanicMapResource",
        ["peepCapacityResource"] = "PeepCapacityResource", ["citySign"] = "CitySign",
        ["timedHarvestableResource"] = "TimedHarvestableResource", ["baseShip"] = "BaseShip",
        ["basePier"] = "BasePier", ["ferryShip"] = "FerryShip", ["timed_business"] = "TimedBusiness",
        ["corporation"] = "Corporation", ["heliport"] = "Heliport", ["launchpad"] = "LaunchPad",
        ["themePark"] = "ThemePark", ["portal"] = "Portal", ["farmers_market"] = "SimpleOpenResource",
        ["subway"] = "Subway", ["subwayReceiver"] = "SubwayReceiver", ["busStop"] = "BusStop",
        ["busStation"] = "BusStation", ["party"] = "Party", ["amphitheater"] = "Amphitheater",
        ["aircraftcarrier"] = "Aircraftcarrier", ["xpromoBuilding"] = "XPromoBuilding",
        ["space_shuttle"] = "SpaceShuttle", ["rainbowDecoration"] = "RainbowDecoration",
        ["PartnerBuild"] = "PartnerBuild", ["monorailstation"] = "MonorailStation",
        ["monorailtrack"] = "MonorailTrack"
    };

    // ItemClassDefinitions.getClassByType: item className, then type mapping, else Decoration
    public static BuildingClassType GetBuildingClassType(this GameItem item)
    {
        var className = item.ClassName ?? TypeToClassName.GetValueOrDefault(item.Type);

        return Enum.TryParse<BuildingClassType>(className, out var classType) && Enum.IsDefined(classType)
            ? classType
            : BuildingClassType.Decoration;
    }

    // Client class inheritance  extends MechanicMapResource
    private static readonly Dictionary<BuildingClassType, BuildingClassType> ParentClass = new()
    {
        [BuildingClassType.Wilderness] = BuildingClassType.Decoration,
        [BuildingClassType.Storage] = BuildingClassType.Decoration,
        [BuildingClassType.Road] = BuildingClassType.Decoration,
        [BuildingClassType.Sidewalk] = BuildingClassType.Decoration,
        [BuildingClassType.ItemStorage] = BuildingClassType.Decoration,
        [BuildingClassType.HolidayTree] = BuildingClassType.Decoration,
        [BuildingClassType.RainbowDecoration] = BuildingClassType.Decoration,
        [BuildingClassType.Stadium] = BuildingClassType.Decoration,
        [BuildingClassType.MonorailTrack] = BuildingClassType.Decoration,
        [BuildingClassType.ParkingLot] = BuildingClassType.Road,
        [BuildingClassType.Bridge] = BuildingClassType.ParkingLot,
        [BuildingClassType.BridgePart] = BuildingClassType.ParkingLot,
        [BuildingClassType.Landmark] = BuildingClassType.Municipal,
        [BuildingClassType.University] = BuildingClassType.Municipal,
        [BuildingClassType.ZooEnclosure] = BuildingClassType.Municipal,
        [BuildingClassType.XPromoBuilding] = BuildingClassType.Municipal,
        [BuildingClassType.Skyscraper] = BuildingClassType.Residence,
        [BuildingClassType.Corporation] = BuildingClassType.Residence,
        [BuildingClassType.CustomResidence] = BuildingClassType.Residence,
        [BuildingClassType.Factory] = BuildingClassType.Plot,
        [BuildingClassType.Pier] = BuildingClassType.BasePier,
        [BuildingClassType.DockHouse] = BuildingClassType.BasePier,
        [BuildingClassType.SuperNeighborhood] = BuildingClassType.Neighborhood,
        [BuildingClassType.LaunchPad] = BuildingClassType.Airplane,
        [BuildingClassType.FarmersMarketConstructionSite] = BuildingClassType.ConstructionSite
    };

    private static readonly List<BuildingClassType> RootClasses = [BuildingClassType.MechanicMapResource, BuildingClassType.LotSite, BuildingClassType.TrainTracks, BuildingClassType.Headquarter, BuildingClassType.Dock];

    // AS3 `is` operator
    public static bool IsA(this BuildingClassType type, BuildingClassType parent)
    {
        while (type != parent)
        {
            if (RootClasses.Contains(type))
            {
                return false;
            }

            type = ParentClass.GetValueOrDefault(type, BuildingClassType.MechanicMapResource);
        }

        return true;
    }

    public static bool IsBusiness(this BuildingClassType type)
    {
        return AllowedBusiness.Contains(type);
    }

    public static bool IsHarvestableResource(this BuildingClassType type)
    {
        return HarvestableResources.Contains(type);
    }

    public static bool IsStackable(this BuildingClassType type)
    {
        return IsStackableList.Contains(type);
    }
}