using CityVilleDotnet.Domain.Enums;

namespace CityVilleDotnet.Domain.Entities;

public partial class World
{
    public void CountCruiseUpgradeActions(WorldObject ship, string contractName)
    {
        foreach (var dockHouse in Objects.Where(x => x.ClassName == BuildingClassType.DockHouse && ship.IsBerthedAt(x)))
        {
            dockHouse.CountCruiseUpgradeAction(contractName);
        }
    }
}
