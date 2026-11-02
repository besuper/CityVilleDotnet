using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.EnumExtensions;
using CityVilleDotnet.Domain.Enums;

namespace CityVilleDotnet.Domain.Entities;

public partial class WorldObject
{
    public bool IsTaxiReady { get; private set; }

    public int GetTaxiMaxInstaHarvests()
    {
        return ItemName switch
        {
            "mun_taxi_dispatch_2" => 6,
            "mun_taxi_dispatch_3" => 9,
            _ => 3
        };
    }

    public bool MakeReadyByTaxi()
    {
        if (!GetClassName().IsBusiness() || (State != WorldObjectState.Open && State != WorldObjectState.ClosedHarvestable))
        {
            return false;
        }

        if (State == WorldObjectState.Open)
        {
            Visits = GameSettingsManager.Instance.GetItem(ItemName)?.CommodityRequired ?? throw new Exception("Can't find max visits for business building");
            State = WorldObjectState.ClosedHarvestable;
        }

        IsTaxiReady = true;
        return true;
    }
}
