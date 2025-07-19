using CityVilleDotnet.Common.Global;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Utils;
using Microsoft.Extensions.Logging;

namespace CityVilleDotnet.Domain.Entities;

public class Player
{
    public Guid Id { get; set; }
    public string Uid { get; set; } = "333";
    public int Snuid { get; set; }
    public int LastTrackingTimestamp { get; set; } = 0;
    public List<object> PlayerNews { get; set; } = [];
    public List<object> Wishlist { get; set; } = [];
    public bool SfxDisabled { get; set; } = false;
    public bool MusicDisabled { get; set; } = false;
    public List<InventoryItem> InventoryItems { get; set; } = [];
    public int Gold { get; set; } = 500;
    public int Goods { get; set; } = 100;
    public int Cash { get; set; } = 0;
    public int Level { get; set; } = 1;
    public int Xp { get; set; } = 0;
    public int SocialLevel { get; set; } = 1;
    public int SocialXp { get; set; } = 0;
    public int Energy { get; set; } = 12;
    public int EnergyMax { get; set; } = 12;
    public int TimeBeforeNextEnergy { get; set; }
    public List<SeenFlag> SeenFlags { get; set; } = new();
    public int ExpansionsPurchased { get; set; }
    public List<Collection> Collections { get; set; } = [];
    public Dictionary<object, object> Licenses { get; set; } = [];
    public int RollCounter { get; set; }
    public bool IsNew { get; set; } = true;
    public bool FirstDay { get; set; } = true;
    public int CreationTimestamp { get; set; }
    public string Username { get; set; } = "";

    public void AddItemToCollection(string collectionName, string itemName, int amount = 1)
    {
        var collection = Collections.FirstOrDefault(x => x.Name == collectionName);

        if (collection is null)
        {
            collection = new Collection(collectionName);
            Collections.Add(collection);
        }

        collection.AddItem(itemName, amount);
    }

    public CollectionItem? RemoveItemFromCollection(string collectionName, string itemName, int amount = 1)
    {
        var collection = Collections.FirstOrDefault(x => x.Name == collectionName);

        if (collection is null)
            throw new Exception($"Collection not found: {collectionName}");

        return collection.RemoveItem(itemName, amount);
    }

    public void AddItem(string itemName, int amount = 1)
    {
        var item = InventoryItems.FirstOrDefault(x => x.Name == itemName);

        if (item is null)
            InventoryItems.Add(new InventoryItem(itemName, amount));
        else
            item.AddAmount(amount);
    }

    public InventoryItem? RemoveItem(string itemName, int amount = 1)
    {
        var item = InventoryItems.FirstOrDefault(x => x.Name == itemName);

        if (item is null)
            throw new Exception($"Item not found in player inventory {itemName}");

        if (item.Amount < amount)
            throw new Exception("Not enough items");

        item.RemoveAmount(amount);

        if (item.Amount <= 0)
        {
            InventoryItems.Remove(item);
            return item;
        }

        return null;
    }

    public int CountIventoryItems()
    {
        return InventoryItems.Sum(x => x.Amount);
    }

    public bool HasItem(string itemName)
    {
        return InventoryItems.Any(x => x.Name == itemName && x.Amount > 0);
    }

    public void UpdateTracking()
    {
        LastTrackingTimestamp = (int)ServerUtils.GetCurrentTime();
    }

    public void UpdateSettings(bool musicDisabled, bool sfxDisabled)
    {
        MusicDisabled = musicDisabled;
        SfxDisabled = sfxDisabled;
    }

    private Energy CalculateCurrentEnergy()
    {
        var elapsedTime = (int)ServerUtils.GetCurrentTime() - TimeBeforeNextEnergy;
        var timeToRegen = GameSettingsManager.Instance.GetDouble("EnergyRegenerationSeconds") * 1000;
        var toRecover = Math.Floor(elapsedTime / timeToRegen);
        var currentNewEnergy = Math.Min(Energy + (int)toRecover, EnergyMax);
        var timeSinceLastRegen = elapsedTime % timeToRegen;
        var timeUntilNextRegen = timeToRegen - timeSinceLastRegen;

        return new Energy(currentNewEnergy, timeToRegen, timeUntilNextRegen, timeSinceLastRegen);
    }

    public bool RemoveEnergy(int amount)
    {
        StaticLogger.Current.LogDebug("Removing {Amount} energy from player {PlayerId}", amount, Id);
        
        var currentEnergy = CalculateCurrentEnergy();
        if (currentEnergy.CurrentNewEnergy < amount) return false;

        var wasAtMax = Energy >= EnergyMax;
        Energy = currentEnergy.CurrentNewEnergy - amount;

        if (wasAtMax && Energy < EnergyMax)
        {
            TimeBeforeNextEnergy = (int)ServerUtils.GetCurrentTime();
        }
        else if (Energy < EnergyMax)
        {
            TimeBeforeNextEnergy = (int)ServerUtils.GetCurrentTime() - (int)(currentEnergy.TimeToRegen - currentEnergy.TimeUntilNextRegen);
        }

        return true;
    }

    public void UpdateEnergy()
    {
        var currentEnergy = CalculateCurrentEnergy();

        if (Energy >= EnergyMax)
        {
            TimeBeforeNextEnergy = (int)ServerUtils.GetCurrentTime();
        }
        else
        {
            StaticLogger.Current.LogDebug("Current player energy {Energy}/{EnergyMax}", Energy, EnergyMax);
            StaticLogger.Current.LogDebug("Updating energy for player {PlayerId} - Current: {CurrentEnergy}", Id, currentEnergy);

            Energy = currentEnergy.CurrentNewEnergy;
            TimeBeforeNextEnergy = (int)ServerUtils.GetCurrentTime() - (int)currentEnergy.TimeSinceLastRegen;
        }
    }

    public void AddEnergy(int amount)
    {
        StaticLogger.Current.LogDebug("Adding {Amount} energy to player {PlayerId}", amount, Id);

        var currentEnergy = CalculateCurrentEnergy();

        Energy = currentEnergy.CurrentNewEnergy + amount;

        StaticLogger.Current.LogDebug("New energy after addition: {NewEnergy}", Energy);

        if (Energy >= EnergyMax)
        {
            TimeBeforeNextEnergy = (int)ServerUtils.GetCurrentTime();
        }
        else
        {
            TimeBeforeNextEnergy = (int)ServerUtils.GetCurrentTime() - (int)(currentEnergy.TimeToRegen - currentEnergy.TimeUntilNextRegen);
        }
    }

    public int GetLastCheckEnergyTimestamp()
    {
        if (Energy >= EnergyMax)
            return (int)ServerUtils.GetCurrentTimeSeconds();

        var currentEnergy = CalculateCurrentEnergy();

        var currentTimeSeconds = (int)ServerUtils.GetCurrentTimeSeconds();
        var timeSinceLastRegenSeconds = (int)(currentEnergy.TimeSinceLastRegen / 1000);

        return currentTimeSeconds - timeSinceLastRegenSeconds;
    }

    public void RemoveCash(int amount)
    {
        Cash -= amount;
    }
}