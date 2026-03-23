using CityVilleDotnet.Domain.Enums;

namespace CityVilleDotnet.Domain.EnumExtensions;

public static class BuildingClassTypeExtension
{
    private static readonly List<BuildingClassType> AllowedBusiness = [BuildingClassType.Business, BuildingClassType.SocialBusiness, BuildingClassType.Hotel];

    public static bool IsBusiness(this BuildingClassType type)
    {
        return AllowedBusiness.Contains(type);
    }
}