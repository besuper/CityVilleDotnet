using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Enums;

namespace CityVilleDotnet.Domain.Entities;

public partial class World
{
    // GameQuestUtility::firstTimeHotelSupply
    public bool HasOpenHotel()
    {
        return GetHotels().Any(x => x.State is WorldObjectState.Open or WorldObjectState.ClosedHarvestable);
    }

    // GameQuestUtility::resortBusinessThreshold -> ResortManager::parseResorts
    public int GetMaxResortBusinessCount()
    {
        var hotels = GetHotels()
            .Select(x => (Hotel: x, Center: GetCenter(x), Radius: (int)(GameSettingsManager.Instance.GetItem(x.ItemName)?.Shockwave?.Radius ?? 0)))
            .ToList();

        if (hotels.Count == 0)
        {
            return 0;
        }

        return Objects
            .Where(x => x.StoredInId is null && x.ClassName is BuildingClassType.Business or BuildingClassType.SocialBusiness)
            .Select(business =>
            {
                var center = GetCenter(business);

                return hotels
                    .Select(h => (h.Hotel, Distance: Math.Sqrt(Math.Pow(center.X - h.Center.X, 2) + Math.Pow(center.Y - h.Center.Y, 2)), h.Radius))
                    .Where(h => h.Distance <= h.Radius)
                    .OrderBy(h => h.Distance)
                    .Select(h => h.Hotel)
                    .FirstOrDefault();
            })
            .OfType<WorldObject>()
            .GroupBy(x => x)
            .Select(x => x.Count())
            .DefaultIfEmpty(0)
            .Max();
    }

    private IEnumerable<WorldObject> GetHotels()
    {
        return Objects.Where(x => x.StoredInId is null && x.ClassName == BuildingClassType.Hotel);
    }

    // GameObject::centerPosition
    private static (double X, double Y) GetCenter(WorldObject obj)
    {
        var item = GameSettingsManager.Instance.GetItem(obj.ItemName);
        var (width, height) = item is null ? (0, 0) : obj.GetSize(item);

        return (obj.X + width / 2.0, obj.Y + height / 2.0);
    }
}
