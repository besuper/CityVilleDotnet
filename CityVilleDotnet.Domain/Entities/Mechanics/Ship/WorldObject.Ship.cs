using CityVilleDotnet.Common.Settings;

namespace CityVilleDotnet.Domain.Entities;

public partial class WorldObject
{
    private double ShipCostToMakeReady()
    {
        var baseCost = PlotCostToMakeReady();
        if (baseCost <= 0) return 0;

        var gameItem = GameSettingsManager.Instance.GetItem(GetItemName());
        if (gameItem is null || !gameItem.UseHarvestMultForCost) return baseCost;

        return Math.Max(Math.Ceiling(baseCost), 1) * (1 + (gameItem.HarvestMultiplier ?? 0) / 100.0);
    }

    // BaseShip::connectedToPierNonExclusive, BasePier::isValidShipBerth
    public bool IsBerthedAt(WorldObject pier)
    {
        var shipItem = GameSettingsManager.Instance.GetItem(ItemName);
        var pierItem = GameSettingsManager.Instance.GetItem(pier.ItemName);

        if (shipItem is null || pierItem is null || !pierItem.SupportsShip(shipItem)) return false;

        var isRotated = Direction % 2 == 1;
        var width = (isRotated ? shipItem.SizeY : shipItem.SizeX) ?? 0;
        var height = (isRotated ? shipItem.SizeX : shipItem.SizeY) ?? 0;

        return pierItem.GetBerthSquares().Any(square =>
            pier.X + square.X >= X && pier.X + square.X <= X + width &&
            pier.Y + square.Y >= Y && pier.Y + square.Y <= Y + height);
    }

    // CruiseShipLogicComponent::harvest
    public void CountCruiseUpgradeAction(string contractName)
    {
        switch (contractName)
        {
            case "cruise_contract_alaskan" or "cruise_contract_scandinavian":
                UpgradeActionCount = (UpgradeActionCount ?? 0) + 1;
                break;
            case "cruise_contract_mediterranean" or "cruise_contract_caribbean":
                UpgradeActionCount2 = (UpgradeActionCount2 ?? 0) + 1;
                break;
        }
    }
}
