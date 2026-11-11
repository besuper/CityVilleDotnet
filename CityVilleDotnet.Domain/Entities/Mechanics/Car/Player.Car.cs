using System.Text.RegularExpressions;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Settings.GameSettings;

namespace CityVilleDotnet.Domain.Entities;

public partial class Player
{
    private const string CarCraftingTableSource = "crafting";

    public bool CarsEnabled { get; private set; }
    public bool CarCraftEnabled { get; private set; }
    public List<CarItem> Cars { get; private set; } = [];
    public List<CraftingPartItem> CraftingParts { get; private set; } = [];

    public bool EnableCars()
    {
        if (CarsEnabled)
        {
            return false;
        }

        CarsEnabled = true;
        
        // Starter cars granted by the client in CarManager::enableCars
        AddCar("car_cvbee", 3);
        AddCar("car_tinycruiser", 5);
        
        return true;
    }

    public void EnableCarCrafting()
    {
        CarCraftEnabled = true;
    }

    public bool HasCar(string carName)
    {
        return CarsEnabled && GetCarAmount(carName) > 0;
    }

    // From CarManager::processCarHarvest
    public void CollectCarHarvestRewards(string carName)
    {
        var dropTable = CarCraftEnabled ? RollCarCraftDropTable() : null;

        if (dropTable is not null)
        {
            GlobalTableOverrides.Add(new GlobalTableOverride("cars", dropTable, CarCraftingTableSource));
        }

        CollectDoobersRewards(carName);

        if (dropTable is not null)
        {
            GlobalTableOverrides.RemoveAll(x => x.Source == CarCraftingTableSource && x.Table == dropTable);
        }
    }

    private string? RollCarCraftDropTable()
    {
        var uncraftedCollections = GameSettingsManager.Instance.GetCraftCollections()
            .Where(x => GetCarAmount(x.TradeInReward.Item.Name) == 0)
            .ToList();

        if (uncraftedCollections.Count == 0)
        {
            return null;
        }

        return uncraftedCollections[RollFeature("cars", 0, uncraftedCollections.Count - 1)].DropTable?.Table;
    }

    // From CraftingManager::craftCollection
    public bool CraftCollection(CraftCollection recipe)
    {
        if (!CarCraftEnabled || recipe.Collectables.Any(x => GetCraftingPartAmount(x.Name) < x.Quantity))
        {
            return false;
        }

        foreach (var collectable in recipe.Collectables)
        {
            AddCraftingPart(collectable.Name, -collectable.Quantity);
        }

        AddItem(recipe.TradeInReward.Item.Name, recipe.TradeInReward.Item.Amount);
        return true;
    }

    // From CustomCarCraftingDialog::onBuyAllPartsButtonClick
    public void BuyMissingCraftParts(CraftCollection recipe, double bulkDiscount)
    {
        var missingParts = recipe.Collectables
            .Select(x => (Part: x, Count: Math.Max(0, x.Quantity - GetCraftingPartAmount(x.Name))))
            .Where(x => x.Count > 0)
            .ToList();

        var cost = missingParts.Sum(x => (int)(x.Count * (GameSettingsManager.Instance.GetItem(x.Part.Name)?.Cash ?? 0) * (1 - bulkDiscount)));

        RemoveCash(cost);

        foreach (var (part, count) in missingParts)
        {
            AddCraftingPart(part.Name, count);
        }
    }

    private int GetCraftingPartAmount(string partName)
    {
        return CraftingParts.FirstOrDefault(x => x.Name == partName)?.Amount ?? 0;
    }

    // From CarData::getNumberOfCars
    private int GetCarAmount(string carName)
    {
        return Cars.FirstOrDefault(x => x.Name == carName)?.Amount ?? 0;
    }

    // From CraftingData::updateItemAmount
    private void AddCraftingPart(string partName, int amount)
    {
        var part = CraftingParts.FirstOrDefault(x => x.Name == partName);
        var newAmount = ClampFeatureAmount(partName, (part?.Amount ?? 0) + amount);

        if (newAmount == 0)
        {
            if (part is not null)
            {
                // the client treats a missing part like 0 so empty rows are removed
                CraftingParts.Remove(part);
            }

            return;
        }

        if (part is null)
        {
            CraftingParts.Add(new CraftingPartItem(partName, newAmount));
            return;
        }

        part.SetAmount(newAmount);
    }

    // From CarData::updateItemAmount, bundles = "3*car_name"
    private void AddCar(string carName, int amount)
    {
        var bundle = Regex.Match(carName, @"^(\d+)\*(\w+)");

        if (bundle.Success)
        {
            carName = bundle.Groups[2].Value;
            amount = int.Parse(bundle.Groups[1].Value);
        }

        var car = Cars.FirstOrDefault(x => x.Name == carName);

        if (car is null)
        {
            car = new CarItem(carName, 0);
            Cars.Add(car);
        }

        car.SetAmount(ClampFeatureAmount(carName, car.Amount + amount));
    }

    private static int ClampFeatureAmount(string itemName, int amount)
    {
        var inventoryLimit = GameSettingsManager.Instance.GetItem(itemName)?.InventoryLimit ?? 0;

        return Math.Max(inventoryLimit > 0 ? Math.Min(amount, inventoryLimit) : amount, 0);
    }
}
