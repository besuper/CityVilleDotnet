using CityVilleDotnet.Common.Global;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Utils;
using Microsoft.Extensions.Logging;

namespace CityVilleDotnet.Domain.Entities;

public class Player
{
    public Guid Id { get; }
    public string Uid { get; set; } = "333";
    public int Snuid { get; set; }
    public int LastTrackingTimestamp { get; private set; }
    public List<object> PlayerNews { get; set; } = [];
    public List<object> Wishlist { get; set; } = [];
    public bool SfxDisabled { get; private set; }
    public bool MusicDisabled { get; private set; }
    public List<InventoryItem> InventoryItems { get; set; } = [];
    public int Gold { get; private set; }
    public int Goods { get; private set; }
    public int Cash { get; private set; }
    public int Level { get; private set; } = 1;
    public int Xp { get; private set; }
    public int SocialLevel { get; private set; } = 1;
    public int SocialXp { get; private set; }
    public int Energy { get; private set; }
    public int EnergyMax { get; private set; }
    public int TimeBeforeNextEnergy { get; private set; }
    public List<SeenFlag> SeenFlags { get; set; } = new();
    public int ExpansionsPurchased { get; private set; }
    public List<Collection> Collections { get; set; } = [];
    public Dictionary<object, object> Licenses { get; set; } = [];
    public int RollCounter { get; private set; }
    public bool IsNew { get; private set; } = true;
    public bool FirstDay { get; private set; } = true;
    public int CreationTimestamp { get; private set; }
    public string Username { get; private set; }

    public Player(string username)
    {
        Id = Guid.NewGuid();
        Cash = 900;
        Gold = 50000;
        Energy = 12;
        EnergyMax = 12;
        Goods = 100;
        Username = username;
        CreationTimestamp = (int)ServerUtils.GetCurrentTime();
    }

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

    public void AddXp(int xp)
    {
        Xp += xp;

        ComputeLevel();
    }

    private void ComputeLevel()
    {
        foreach (var item in GameSettingsManager.Instance.GetLevels())
        {
            if (Xp < int.Parse(item.RequiredXp)) continue;

            var level = int.Parse(item.Num);

            if (level <= Level) continue;

            // Level up!
            StaticLogger.Current.LogDebug("Level up! New level: {Level}", level);

            var energyMax = int.Parse(item.EnergyMax);

            // TODO: Add heldEnergy and cash
            var energy = energyMax + Math.Max(Energy - energyMax, 0);

            Level = level;
            Energy = energy;
            EnergyMax = energyMax;
            TimeBeforeNextEnergy = (int)ServerUtils.GetCurrentTime();

            UpdateEnergy();

            break;
        }
    }

    public void CompleteTutorial()
    {
        IsNew = false;
        FirstDay = false;

        Uid = $"{Snuid}";
    }

    private void ComputeSocialLevel()
    {
        foreach (var item in GameSettingsManager.Instance.GetSocialLevels())
        {
            if (SocialXp < int.Parse(item.RequiredXp)) continue;

            var level = int.Parse(item.Num);

            if (level <= SocialLevel) continue;

            StaticLogger.Current.LogDebug("Social level up! New level: {Level}", level);

            SocialLevel = level;

            // FIXME: Give the reward

            break;
        }
    }

    public void AddSocialXp(int amount)
    {
        SocialXp += amount;

        ComputeSocialLevel();
    }

    public void AddCoins(int amount)
    {
        Gold += amount;
    }

    public void RemoveCoins(int amount)
    {
        Gold += amount;
    }

    public void RemoveGoods(int amount)
    {
        Goods -= amount;
    }

    public void AddGoods(int amount)
    {
        Goods += amount;
    }

    public void SetSeenFlag(string flag)
    {
        if (!SeenFlags.Any(x => x.Key == flag))
        {
            SeenFlags.Add(new SeenFlag(flag));
        }
    }

    public void IncrementRollCounter()
    {
        RollCounter++;
    }

    public void IncrementExpansionsPurchased()
    {
        ExpansionsPurchased++;
    }

    // From Player::processRandomModifiersFromConfig
    public List<int> CollectDoobersRewards(string itemName, List<string>? allowedDooberTypes = null)
    {
        var gameItem = GameSettingsManager.Instance.GetItem(itemName);

        if (gameItem?.RandomModifiers?.Modifiers is null) return [];

        var secureRands = new List<int>();

        foreach (var itemModifier in gameItem.RandomModifiers.Modifiers)
        {
            IncrementRollCounter();

            var debugName = gameItem.Name;
            var secureRand = SecureRand.GenerateRand(0, 99, RollCounter, Uid);

            StaticLogger.Current.LogDebug("SecureRand for {DebugName}: rollCounter={PlayerRollCounter} => {SecureRand}", debugName, RollCounter, secureRand);

            secureRands.Add(secureRand);

            var modifierTable = GameSettingsManager.Instance.GetRandomModifier(itemModifier.TableName);

            if (modifierTable is null) continue;

            StaticLogger.Current.LogDebug("Checking random table named {ModifierTableName} type {ModifierTableType} with rand {SecureRand}", modifierTable.Name, modifierTable.Type, secureRand);

            var previousRollPercent = 0;
            var found = false;

            foreach (var roll in modifierTable.Rolls)
            {
                if (roll.Percent > 0)
                {
                    var currentRollPercent = roll.Percent + previousRollPercent;

                    StaticLogger.Current.LogDebug("Percent {CurrentRollPercent}", currentRollPercent);

                    if (secureRand < currentRollPercent && !found)
                    {
                        StaticLogger.Current.LogDebug("FOUND WITH PERCENT : {CurrentRollPercent}", currentRollPercent);
                        StaticLogger.Current.LogDebug("SECURE RAND : {SecureRand}", secureRand);

                        foreach (var (key, value) in roll.Rewards)
                        {
                            // FIXME: Implement a better skip
                            if (allowedDooberTypes is not null && !allowedDooberTypes.Contains(key))
                            {
                                StaticLogger.Current.LogDebug("Skipping doober type {Key} as it is not allowed", key);
                                continue;
                            }

                            StaticLogger.Current.LogDebug("TYPE : {Key}", key);

                            switch (key)
                            {
                                case "coin":
                                    StaticLogger.Current.LogDebug("Found coin {CoinAmount}", value.Sum(x => x.Amount));
                                    AddCoins((int)value.Sum(x => x.Amount));
                                    break;
                                case "xp":
                                    AddXp((int)value.Sum(x => x.Amount));
                                    StaticLogger.Current.LogDebug("Found xp {XpAmount}", value.Sum(x => x.Amount));
                                    break;
                                case "energy":
                                    AddEnergy((int)value.Sum(x => x.Amount));
                                    break;
                                case "collectable":
                                    StaticLogger.Current.LogDebug("Found collectable {CollectableName}", string.Join(", ", value.Select(x => x.Name).ToList()));

                                    foreach (var element in value)
                                    {
                                        var collectionName = GameSettingsManager.Instance.GetCollectionByItemName(element.Name);

                                        if (collectionName is not null)
                                        {
                                            AddItemToCollection(collectionName, element.Name);
                                            StaticLogger.Current.LogDebug("Added {CollectableName} to collection {CollectionName}", element.Name, collectionName);
                                        }
                                        else
                                        {
                                            StaticLogger.Current.LogWarning("Collection for item {CollectableName} not found", element.Name);
                                        }
                                    }

                                    break;
                                case "food":
                                    AddGoods((int)value.Sum(x => x.Amount));
                                    StaticLogger.Current.LogDebug("Found food {FoodAmount}", value.Sum(x => x.Amount));
                                    break;
                            }
                        }

                        found = true;
                    }

                    previousRollPercent = currentRollPercent;
                }
            }
        }

        return secureRands;
    }
}