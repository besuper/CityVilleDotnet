using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Common.Settings;

namespace CityVilleDotnet.Domain.Entities;

public partial class Player
{
    // Those constants come from the client itself
    private const int BaseSignatureRequirement = 3;
    private const int CitySignatureRequirement = 5;
    private const int MaxSignatureRequirement = 10;

    public List<RequestInventoryItem> RequestInventoryItems { get; set; } = [];

    public void BuyRequestItem(string itemName, string inventoryName)
    {
        var gameItem = GameSettingsManager.Instance.GetItem(itemName);

        if (gameItem is null || gameItem.Type != "requestItem")
        {
            throw new DomainException(GameErrorType.InvalidData);
        }

        var item = RequestInventoryItems.FirstOrDefault(x => x.InventoryName == inventoryName && x.ItemName == itemName);

        if (item?.Amount >= gameItem.InventoryLimit)
        {
            throw new DomainException(GameErrorType.InvalidState);
        }

        RemoveCash(gameItem.Cash ?? 0);

        if (item is null)
        {
            item = new RequestInventoryItem(inventoryName, itemName);
            RequestInventoryItems.Add(item);
        }

        item.AddAmount(1);
    }

    public void UpdatePendingCityName(string name)
    {
        GetWorld().SetPendingWorldName(name);
    }

    public string FinalizePendingCityName()
    {
        var world = GetWorld();
        var name = world.FinalizePendingWorldName();

        ConsumeSignatures("cityName", world.CanRenameWithoutSignatures() ? 0 : CitySignatureRequirement);

        return name;
    }

    public void UpdateFranchisePendingName(string franchiseType, string name)
    {
        GetFranchise(franchiseType).SetPendingName(name);
    }

    public string FinalizeFranchisePendingName(string franchiseType)
    {
        var franchise = GetFranchise(franchiseType);
        var required = GetWorld().CanRenameWithoutSignatures() ? 0 : Math.Min(BaseSignatureRequirement + franchise.Locations.Count, MaxSignatureRequirement);

        var name = franchise.FinalizePendingName();

        ConsumeSignatures(franchiseType, required);

        return name;
    }

    private Franchise GetFranchise(string franchiseType)
    {
        return Franchises.FirstOrDefault(x => x.FranchiseType == franchiseType) ?? throw new DomainException(GameErrorType.InvalidData);
    }

    private void ConsumeSignatures(string inventoryName, int required)
    {
        var signatures = RequestInventoryItems.FirstOrDefault(x => x.InventoryName == inventoryName && x.ItemName == "signature");

        if ((signatures?.Amount ?? 0) < required)
        {
            throw new DomainException(GameErrorType.InvalidState);
        }

        if (signatures is not null)
        {
            RequestInventoryItems.Remove(signatures);
        }
    }
}