using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;

namespace CityVilleDotnet.Domain.Entities;

public partial class Player
{
    // ThemeParkSupplyMechanic::updatePlayerCommodities
    public void SupplyThemePark(WorldObject themePark)
    {
        var cost = themePark.GetThemeParkSupplyCost();

        if (cost <= 0) throw new DomainException(GameErrorType.InvalidState);
        if (Goods < cost) throw new DomainException(GameErrorType.NotEnoughMoney);

        RemoveGoods(cost);
        themePark.OpenThemeParkSupply(Level);
    }
}
