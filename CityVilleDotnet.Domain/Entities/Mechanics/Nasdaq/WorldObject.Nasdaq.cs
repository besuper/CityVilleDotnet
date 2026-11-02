namespace CityVilleDotnet.Domain.Entities;

public partial class WorldObject
{
    public int InvestmentReturns { get; private set; }
    public int InvestmentCount { get; private set; }

    public void HarvestInvestment(int returns)
    {
        InvestmentReturns += returns;
        InvestmentCount++;
        Plow();
    }

    public void CompleteInvestmentReturns(int amount)
    {
        InvestmentReturns = Math.Max(InvestmentReturns, amount);
    }
}
