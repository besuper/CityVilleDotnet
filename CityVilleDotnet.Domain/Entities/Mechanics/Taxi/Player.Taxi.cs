namespace CityVilleDotnet.Domain.Entities;

public partial class Player
{
    public int TaxiNumInstaHarvests { get; private set; }
    public double TaxiLastResetTime { get; private set; }

    public bool UseTaxiInstaHarvest(IReadOnlyCollection<WorldObject> taxiDispatches)
    {
        if (Level < 25 || taxiDispatches.Count == 0 || TaxiNumInstaHarvests >= taxiDispatches.Max(x => x.GetTaxiMaxInstaHarvests()))
        {
            return false;
        }

        TaxiNumInstaHarvests++;
        return true;
    }

    public void ResetTaxiInstaHarvests(double resetTime)
    {
        TaxiNumInstaHarvests = 0;
        TaxiLastResetTime = resetTime;
    }
}
