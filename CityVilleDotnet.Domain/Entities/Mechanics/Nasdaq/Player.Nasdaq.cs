using CityVilleDotnet.Common.Settings.GameSettings;

namespace CityVilleDotnet.Domain.Entities;

public partial class Player
{
    // MechanicDataGate::buyAll, only the investment returns of HarvestInvestmentMechanic are stored
    public void BuyInvestmentGate(WorldObject obj, GatesItem gate)
    {
        if (gate.Type != "mechanic" || gate.Data != "investmentData") throw new Exception($"Gate {gate.Name} of {obj.ItemName} is not an investment gate");

        var keys = gate.Keys.OfType<GateKey>().ToList();

        if (keys.Any(x => x.Name != "coin")) throw new Exception($"Gate {gate.Name} of {obj.ItemName} has unsupported keys");

        var totalAmount = keys.Sum(x => x.Amount);
        var currentAmount = Math.Min(obj.InvestmentReturns, totalAmount);

        if (gate.CashCost > 0 && totalAmount > 0 && currentAmount < totalAmount)
        {
            var progress = 1 - (double)(totalAmount - currentAmount) / totalAmount;
            RemoveCash(Math.Max((int)Math.Floor((1 - progress) * gate.CashCost), 1));
        }

        obj.CompleteInvestmentReturns(totalAmount);
    }

    // HarvestInvestmentMechanic + InvestmentFormulas::applyStockExchangePayouts
    public string HarvestInvestment(WorldObject obj, GameItem buildingItem)
    {
        if (obj.ContractName is null || !obj.HasGrown()) throw new Exception($"Investment of {obj.ItemName} is not ready to harvest");

        var contractName = obj.ContractName;
        var goldBefore = Gold;
        var modifiersName = obj.InvestmentCount switch
        {
            0 => "firstTime",
            1 => "secondTime",
            _ => null
        };

        CollectDoobersRewards(contractName, coinMultiplier: 1 + (buildingItem.HarvestMultiplier ?? 0) / 100.0, modifiersName: modifiersName);

        // Shares are rolled once and paid for each share owned, without the building multiplier
        var shareName = contractName + "_share";
        var shares = CountInventoryItem(shareName);

        if (shares > 0)
        {
            CollectDoobersRewards(shareName, coinMultiplier: shares);
            RemoveItem(shareName, shares);
        }

        obj.HarvestInvestment(Gold - goldBefore);

        return contractName;
    }
}
