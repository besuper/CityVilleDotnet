using CityVilleDotnet.Domain.Enums;

namespace CityVilleDotnet.Domain.EnumExtensions;

public static class BuildingClassTypeExtension
{
    private static readonly List<BuildingClassType> AllowedBusiness = [BuildingClassType.Business, BuildingClassType.SocialBusiness, BuildingClassType.Hotel];
    private static readonly List<BuildingClassType> IsStackableList = [BuildingClassType.Decoration, BuildingClassType.Road, BuildingClassType.ParkingLot , BuildingClassType.Sidewalk, BuildingClassType.GreenHouse, BuildingClassType.Airplane, BuildingClassType.Ship, BuildingClassType.HarvestShip, BuildingClassType.Amphitheater];

    public static bool IsBusiness(this BuildingClassType type)
    {
        return AllowedBusiness.Contains(type);
    }
    
    public static bool IsStackable(this BuildingClassType type)
    {
        return IsStackableList.Contains(type);
    }
}