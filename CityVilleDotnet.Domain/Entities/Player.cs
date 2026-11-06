using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Common.Global;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Settings.GameSettings;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Domain.EnumExtensions;
using CityVilleDotnet.Domain.GameEntities;
using Microsoft.Extensions.Logging;

namespace CityVilleDotnet.Domain.Entities;

public partial class Player
{
    public Guid Id { get; }
    public int Snuid { get; set; }
    public static readonly TimeSpan OnlineThreshold = TimeSpan.FromMinutes(1);

    public DateTimeOffset LastTrackingTimestamp { get; private set; }
    public bool SfxDisabled { get; private set; }
    public bool MusicDisabled { get; private set; }
    public bool NightModeActive { get; private set; }
    public bool WeatherEffectDisabled { get; private set; }
    public List<InventoryItem> InventoryItems { get; set; } = [];
    public int Gold { get; private set; }
    public int Goods { get; private set; }
    public int PremiumGoods { get; private set; }
    public int Cash { get; private set; }
    public int Level { get; private set; } = 1;
    public int Xp { get; private set; }
    public int SocialLevel { get; private set; } = 1;
    public int SocialXp { get; private set; }
    public int Energy { get; private set; }
    public int EnergyMax { get; private set; }
    public long TimeBeforeNextEnergy { get; private set; }
    public List<SeenFlag> SeenFlags { get; set; } = new();
    public List<Coupon> Coupons { get; set; } = [];
    public int ExpansionsPurchased { get; private set; }
    public List<Collection> Collections { get; private set; } = [];
    public List<LicenseItem> Licenses { get; set; } = [];
    public List<Franchise> Franchises { get; set; } = [];
    public List<PreyGroup> PreyGroups { get; private set; } = [];
    public int RollCounter { get; private set; }
    public List<FeatureRollCounter> FeatureRollCounters { get; private set; } = [];
    public List<Flag> Flags { get; private set; } = [];
    public List<GlobalTableOverride> GlobalTableOverrides { get; private set; } = [];
    public List<WishlistItem> Wishlist { get; private set; } = [];
    public bool IsNew { get; private set; } = true;
    public bool FirstDay { get; private set; } = true;
    public DateTimeOffset CreationTimestamp { get; private set; }
    public string Username { get; private set; } = string.Empty;
    public List<LotOrder> LotOrders { get; set; } = [];
    public List<VisitorHelpOrder> VisitorHelpOrders { get; set; } = [];
    public List<Mastery> Masteries { get; set; } = [];
    public List<World> Worlds { get; set; } = [];
    public string? ProfilePictureUrl { get; set; }
    public List<Quest> Quests { get; } = [];
    public List<Friend> Friends { get; } = [];
    public ApplicationUser? AppUser { get; private set; }
    public WorldType LastPlayedWorldType { get; private set; } = WorldType.Main;

    public Player()
    {
    }

    public Player(ApplicationUser appUser, World world)
    {
        var settings = GameSettingsManager.Instance.GetSettings();
        
        Id = Guid.NewGuid();
        Cash = settings.StartingCash;
        Gold = settings.StartingGold;
        Energy = settings.StartingEnergy;
        EnergyMax = settings.StartingEnergyMax;
        TimeBeforeNextEnergy = ServerUtils.GetCurrentTime();
        Goods = settings.StartingCommodities;
        Xp = settings.StartingXp;
        Level = settings.StartingLevel;
        PremiumGoods = 0;
        Username = appUser.UserName!;
        CreationTimestamp = DateTimeOffset.Now;
        Worlds.Add(world);
        AppUser = appUser;

        Quests.Add(Quest.Create("q_rename_city", 1, QuestType.Active));
    }

    public void AddItemToCollection(string collectionName, string itemName, int amount = 1)
    {
        GetOrCreateCollection(collectionName).AddItem(itemName, amount);
    }

    private Collection GetOrCreateCollection(string collectionName)
    {
        var collection = Collections.FirstOrDefault(x => x.Name == collectionName);

        if (collection is null)
        {
            collection = new Collection(collectionName);
            Collections.Add(collection);
        }

        return collection;
    }

    // From IMysteryCollectionManager::grantCollectibleToken, the collection is traded in from the inventory once all its tokens are owned
    public List<InventoryItem> GrantMysteryCollectibleToken(string upgradedItemName)
    {
        var tokenName = $"{upgradedItemName}_token";
        var collectionName = GameSettingsManager.Instance.GetCollectionByItemName(tokenName);

        if (HasItem(tokenName) || Collections.Any(x => x.Name == collectionName && x.TradeIns > 0)) return [];

        var collectionSetting = collectionName is null ? null : GameSettingsManager.Instance.GetCollectionByName(collectionName);
        var otherTokens = collectionSetting?.Collectables.Collectables.Where(x => x.Name != tokenName).ToList() ?? [];

        if (collectionSetting is null || !otherTokens.All(x => HasItem(x.Name)))
        {
            AddItem(tokenName);
            return [];
        }

        var removed = otherTokens
            .Select(x => RemoveItem(x.Name))
            .OfType<InventoryItem>()
            .ToList();

        GrantCollectionRewards(collectionSetting);
        GetOrCreateCollection(collectionSetting.Name).TradeIn();

        return removed;
    }

    public void AddItem(string itemName, int amount = 1, string? storageKey = null, WorldObject? storedObject = null)
    {
        if (storageKey is null)
        {
            var inventoryLimit = GameSettingsManager.Instance.GetItem(itemName)?.InventoryLimit ?? 0;

            if (inventoryLimit > 0)
            {
                var currentAmount = CountInventoryItem(itemName);

                if (currentAmount >= inventoryLimit)
                {
                    StaticLogger.Current.LogDebug("Inventory limit {InventoryLimit} reached for {ItemName}", inventoryLimit, itemName);
                    return;
                }

                amount = Math.Min(amount, inventoryLimit - currentAmount);
            }
        }

        var item = InventoryItems.FirstOrDefault(x => x.Name == itemName && x.StorageType == storageKey);

        if (item is null)
            InventoryItems.Add(new InventoryItem(itemName, amount, storageKey, storedObject));
        else
            item.AddAmount(amount);
    }

    public InventoryItem? RemoveItem(string itemName, int amount = 1, string? storageKey = null)
    {
        var item = InventoryItems.FirstOrDefault(x => x.Name == itemName && x.StorageType == storageKey);

        if (item is null)
            throw new Exception($"Item not found in player inventory {itemName}");

        if (item.Amount < amount)
            throw new Exception($"Not enough items {itemName}");

        item.RemoveAmount(amount);

        if (item.Amount <= 0)
        {
            InventoryItems.Remove(item);
            return item;
        }

        return null;
    }

    public void BuyStatusGate(WorldObject obj, GatesItem gate)
    {
        var keys = gate.Keys.Where(x => x is not null).Select(x => x!).ToList();
        var totalAmount = keys.Sum(x => x.Amount);
        var currentAmount = keys.Sum(x => obj.GetStatusGateKeyValue(x.Name));

        // WorldObjectStatusGate::computeTotalCost
        if (gate.CashCost > 0 && totalAmount > 0 && currentAmount < totalAmount)
        {
            var progress = 1 - (double)(totalAmount - currentAmount) / totalAmount;
            RemoveCash(Math.Max((int)Math.Floor((1 - progress) * gate.CashCost), 1));
        }

        foreach (var key in keys)
        {
            obj.CompleteStatusGateKey(key.Name, key.Amount);
        }
    }

    public List<InventoryItem> ConsumeInventoryGate(GameItem buildingItem, string gateName)
    {
        var removed = new List<InventoryItem>();
        
        foreach (var key in buildingItem.GetInventoryGateKeys(gateName))
        {
            if (key.KeepOnComplete) continue;

            var owned = CountInventoryItem(key.Name);

            if (owned < key.Amount) throw new Exception($"Not enough item {key.Name} to upgrade building");

            var removeItem = RemoveItem(key.Name, key.RemoveAllOnComplete ? owned : key.Amount);

            if (removeItem is not null)
                removed.Add(removeItem);
        }

        return removed;
    }

    public int CountInventoryItems()
    {
        return InventoryItems.Where(x => x.StorageType is null).Sum(x => x.Amount);
    }

    public int CountInventoryItem(string itemName)
    {
        return InventoryItems.Where(x => x.Name == itemName && x.StorageType is null).Sum(x => x.Amount);
    }

    public int CountStorageItemsByType(string type)
    {
        return InventoryItems
            .Where(x => x.StorageType is not null)
            .Where(x => GameSettingsManager.Instance.GetItem(x.Name)?.Type == type)
            .Sum(x => x.Amount);
    }

    public bool HasItem(string itemName)
    {
        return InventoryItems.Any(x => x.StorageType is null && x.Name == itemName && x.Amount > 0);
    }

    public void UpdateTracking()
    {
        LastTrackingTimestamp = DateTimeOffset.Now;
    }

    public void UpdateSettings(bool musicDisabled, bool sfxDisabled)
    {
        MusicDisabled = musicDisabled;
        SfxDisabled = sfxDisabled;
    }

    public int GetEnergyMax()
    {
        return EnergyMax + GetEnergyModifierTotal();
    }

    public int GetEnergyModifierTotal()
    {
        return GetWorld().Objects.Where(o => o.EnergyModifier > 0).Sum(o => o.EnergyModifier);
    }

    private Energy CalculateCurrentEnergy()
    {
        var maxEnergy = GetEnergyMax();
        var elapsedTime = ServerUtils.GetCurrentTime() - TimeBeforeNextEnergy;
        var timeToRegen = GameSettingsManager.Instance.GetSettings().EnergyRegenerationSeconds * 1000;
        var toRecover = Math.Floor(elapsedTime / timeToRegen);
        var currentNewEnergy = Math.Min(Energy + (int)toRecover, maxEnergy);
        var timeSinceLastRegen = elapsedTime % timeToRegen;
        var timeUntilNextRegen = timeToRegen - timeSinceLastRegen;

        if (timeSinceLastRegen < 0)
        {
            currentNewEnergy = maxEnergy;
            timeSinceLastRegen = 0;
        }

        return new Energy(currentNewEnergy, timeToRegen, timeUntilNextRegen, timeSinceLastRegen);
    }

    public void RemoveEnergy(int amount)
    {
        var currentEnergy = CalculateCurrentEnergy();
        
        var availableEnergy = Math.Max(Energy, currentEnergy.CurrentNewEnergy);
        
        if (availableEnergy < amount) throw new DomainException(GameErrorType.ForceReload);

        var maxEnergy = GetEnergyMax();
        var wasAtMax = availableEnergy >= maxEnergy;

        Energy = availableEnergy - amount;

        if (wasAtMax && Energy < maxEnergy)
        {
            TimeBeforeNextEnergy = ServerUtils.GetCurrentTime();
        }
        else if (Energy < maxEnergy)
        {
            TimeBeforeNextEnergy = ServerUtils.GetCurrentTime() - (long)currentEnergy.TimeSinceLastRegen;
        }
    }

    public void UpdateEnergy()
    {
        var currentEnergy = CalculateCurrentEnergy();

        if (Energy >= GetEnergyMax())
        {
            TimeBeforeNextEnergy = ServerUtils.GetCurrentTime();
        }
        else
        {
            StaticLogger.Current.LogDebug("Current player energy {Energy}/{EnergyMax}", Energy, EnergyMax);
            StaticLogger.Current.LogDebug("Updating energy for player {PlayerId} - Current: {CurrentEnergy}", Id, currentEnergy);

            Energy = currentEnergy.CurrentNewEnergy;
            TimeBeforeNextEnergy = ServerUtils.GetCurrentTime() - (long)currentEnergy.TimeSinceLastRegen;
        }
    }

    public void AddEnergy(int amount)
    {
        StaticLogger.Current.LogDebug("Adding {Amount} energy to player {PlayerId}", amount, Id);

        var currentEnergy = CalculateCurrentEnergy();

        Energy = Math.Max(Energy, currentEnergy.CurrentNewEnergy) + amount;

        StaticLogger.Current.LogDebug("New energy after addition: {NewEnergy}", Energy);

        if (Energy >= GetEnergyMax())
        {
            TimeBeforeNextEnergy = ServerUtils.GetCurrentTime();
        }
        else
        {
            TimeBeforeNextEnergy = ServerUtils.GetCurrentTime() - (long)currentEnergy.TimeSinceLastRegen;
        }
    }

    public int GetLastCheckEnergyTimestamp()
    {
        if (Energy >= GetEnergyMax())
            return (int)ServerUtils.GetCurrentTimeSeconds();

        var currentEnergy = CalculateCurrentEnergy();

        var currentTimeSeconds = (int)ServerUtils.GetCurrentTimeSeconds();
        var timeSinceLastRegenSeconds = (int)(currentEnergy.TimeSinceLastRegen / 1000);

        return currentTimeSeconds - timeSinceLastRegenSeconds;
    }

    public void SetGold(int amount) => Gold = amount;
    public void SetCash(int amount) => Cash = amount;
    public void SetGoods(int amount) => Goods = amount;
    public void SetPremiumGoods(int amount) => PremiumGoods = amount;
    public void SetEnergy(int amount) => Energy = amount;

    public void UpdateProgression(int level, int xp)
    {
        SetLevel(level);
        Xp = xp;
    }

    public void UpdateSocialProgression(int socialLevel, int socialXp)
    {
        SocialLevel = socialLevel;
        SocialXp = socialXp;
    }

    public void SetXp(int xp)
    {
        Xp = xp;
        ComputeLevel();
    }

    public void SetLevel(int level)
    {
        Level = level;

        var levelData = GameSettingsManager.Instance.GetLevels().FirstOrDefault(x => x.Num == level);

        if (levelData is not null)
        {
            EnergyMax = levelData.EnergyMax;
            Xp = Math.Max(Xp, levelData.RequiredXp);
        }
    }

    public void RemoveCash(int amount)
    {
        if (amount > Cash) throw new DomainException(GameErrorType.NotEnoughMoney);

        Cash -= amount;
    }

    public void AddXp(int xp)
    {
        Xp += xp;

        ComputeLevel();
    }

    private void ComputeLevel()
    {
        var reachedLevel = GameSettingsManager.Instance.GetLevels()
            .Where(x => Xp >= x.RequiredXp)
            .MaxBy(x => x.Num);

        if (reachedLevel is null || reachedLevel.Num <= Level) return;

        var levelsGained = reachedLevel.Num - Level;
        var energyMax = reachedLevel.EnergyMax;

        // TODO: Add heldEnergy
        Energy = energyMax + Math.Max(Energy - energyMax, 0);
        Level = reachedLevel.Num;
        EnergyMax = energyMax;
        TimeBeforeNextEnergy = ServerUtils.GetCurrentTime();
        AddCash(GameSettingsManager.Instance.GetSettings().CashGainedPerLevel * levelsGained);

        UpdateEnergy();
        HandleQuestsProgress("gainLevels", amount: levelsGained);
    }

    public void CompleteTutorial()
    {
        IsNew = false;
        FirstDay = false;
        SetFlag("completed_tutorial", 1);
    }

    public void CompleteBridge()
    {
        SetFlag("completed_bridge", 1);
    }

    private void ComputeSocialLevel()
    {
        foreach (var item in GameSettingsManager.Instance.GetSocialLevels())
        {
            if (SocialXp < item.RequiredXp) continue;

            var level = item.Num;

            if (level <= SocialLevel) continue;

            SocialLevel = level;

            AddGoods(item.Reward);

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
        if (Gold < amount) throw new DomainException(GameErrorType.NotEnoughMoney);

        Gold -= amount;
    }

    public void RemoveGoods(int amount)
    {
        if (Goods < amount) throw new DomainException(GameErrorType.NotEnoughMoney);
        
        Goods -= amount;
    }

    public void RemovePremiumGoods(int amount)
    {
        PremiumGoods -= amount;
    }

    public void AddGoods(int amount)
    {
        Goods += amount;
    }

    public void AddPremiumGoods(int amount)
    {
        PremiumGoods += amount;
    }

    public void AddCash(int cash)
    {
        Cash += cash;
    }

    public void PayContract(GameItem contract)
    {
        if (contract.Type == "factory_textile_contract")
        {
            RemoveGoods(contract.Goods ?? 0);
            return;
        }

        if (contract.Cash > 0)
        {
            RemoveCash(contract.Cash.Value);
            return;
        }

        if (contract.Cost > 0)
            RemoveCoins(contract.Cost.Value);
    }

    // PlantContractMechanic::plant
    public void PlantMechanicContract(WorldObject obj, GameItem buildingItem, GameItem contract)
    {
        // MapResource::canPlantContract
        if (contract.ContractPlaceableOn is null || !buildingItem.HasKeyword(contract.ContractPlaceableOn)) throw new Exception($"Can't plant {contract.Name} on {obj.ItemName}");

        var costMultiplier = Math.Max(buildingItem.GetAnyDataDouble("contractPlantCostMultiplier", 1), 1);

        if (contract.Cash > 0)
        {
            RemoveCash((int)(contract.Cash.Value * costMultiplier));
        }
        else if (contract.Cost > 0)
        {
            RemoveCoins((int)(contract.Cost.Value * costMultiplier));
        }
        else if ((contract.CommodityRequired > 0 ? contract.CommodityRequired : contract.Goods) is > 0 and var goodsCost)
        {
            RemoveGoods((int)(goodsCost * costMultiplier));
        }
        else
        {
            throw new Exception($"Plant cost of {contract.Name} is not supported");
        }

        obj.PlantMechanicContract(contract.Name);
    }

    // InstaReadyContractMechanic::makeReady
    public void InstaReadyMechanicContract(WorldObject obj, GameItem buildingItem)
    {
        var costMultiplier = buildingItem.GetAnyDataDouble("contractInstaReadyCostMultiplier", 1);

        RemoveCash(obj.GetMechanicContractInstaReadyCost(costMultiplier));

        obj.SetFullGrown();
    }

    // HarvestContractMechanic::processHarvest
    public string HarvestMechanicContract(WorldObject obj, GameItem buildingItem, MechanicItem mechanic)
    {
        if (obj.ContractName is null || !obj.HasGrown()) throw new Exception($"Contract of {obj.ItemName} is not ready to harvest");

        var contractName = obj.ContractName;
        var goodsMultiplier = mechanic.PayoutMultipliers?.Contains("goods") == true ? 1 + (buildingItem.HarvestMultiplier ?? 0) / 100.0 : 1;

        CollectDoobersRewards(contractName, goodsMultiplier: goodsMultiplier);

        if (GameSettingsManager.Instance.GetItem(contractName)?.HasMasteries() == true)
        {
            IncrementMastery(contractName, mechanic.Mastery ?? 1);
        }

        obj.Plow();

        return contractName;
    }

    public bool HasSeenFlag(string flag)
    {
        return SeenFlags.Any(x => x.Key == flag);
    }

    public void SetSeenFlag(string flag)
    {
        var seenFlag = SeenFlags.FirstOrDefault(x => x.Key == flag);

        if (seenFlag is null)
        {
            SeenFlags.Add(new SeenFlag(flag));
        }
        else if (seenFlag.Value == 0)
        {
            seenFlag.Value = null;
        }
    }

    public void SetSeenFlagInt(string flag, int? value)
    {
        var seenFlag = SeenFlags.FirstOrDefault(x => x.Key == flag);

        if (seenFlag is null)
        {
            SeenFlags.Add(new SeenFlag(flag, value));
        }
        else
        {
            seenFlag.Value = value;
        }
    }

    public void IncrementSeenFlag(string flag)
    {
        var seenFlag = SeenFlags.FirstOrDefault(x => x.Key == flag);

        SetSeenFlagInt(flag, seenFlag is null ? 1 : (seenFlag.Value ?? 1) + 1);
    }

    public bool GiveCoupon(string name, int? worldFlatId = null)
    {
        if (Coupons.Any(x => x.WorldFlatId == worldFlatId && x.Name == name)) return false;

        Coupons.Add(new Coupon(name, worldFlatId));

        return true;
    }

    public void AddToWishlist(string itemName, int wishlistSize)
    {
        if (Wishlist.Any(x => x.ItemName == itemName))
        {
            return;
        }

        if (Wishlist.Count >= wishlistSize)
        {
            throw new DomainException(GameErrorType.InvalidState);
        }

        Wishlist.Add(new WishlistItem(itemName));
    }

    public void RemoveFromWishlist(string itemName)
    {
        Wishlist.RemoveAll(x => x.ItemName == itemName);
    }

    public bool HasCoupon(string name)
    {
        return Coupons.Any(x => x.WorldFlatId is null && x.Name == name);
    }

    // InventoryGate::checkForKeys
    public bool HasInventoryGateKeys(string itemName, string gateName)
    {
        var gate = GameSettingsManager.Instance.GetItem(itemName)?.GetGates().FirstOrDefault(x => x.Name == gateName);

        if (gate is null) return false;

        return gate.Keys
            .Where(x => x is not null)
            .All(key => CountInventoryItem(key!.Name) >= GameSettingsManager.Instance.GetGateTunedAmount(gate.DynamicTuning, "inventory", Level, key.Amount));
    }

    public void SetFlag(string name, int value)
    {
        var flag = Flags.FirstOrDefault(x => x.Name == name);

        if (flag is null)
            Flags.Add(new Flag(name, value));
        else
            flag.SetValue(value);
    }

    private void IncrementRollCounter()
    {
        RollCounter++;
    }

    private int RollFeature(string feature, int min, int max)
    {
        var counter = FeatureRollCounters.FirstOrDefault(x => x.Feature == feature);

        if (counter is null)
        {
            counter = new FeatureRollCounter(feature, 0);
            FeatureRollCounters.Add(counter);
        }

        counter.Increment();

        return SecureRand.GenerateRand(min, max, counter.Count, Snuid.ToString(), feature);
    }
    
    public string RollUpgradeItemName(WorldObject obj)
    {
        if (obj.UpgradeItemName is not null) return obj.UpgradeItemName;

        var upgrade = GameSettingsManager.Instance.GetItem(obj.ItemName)?.Upgrade;

        if (upgrade is not { MysteryCollectionManager: not null, LootTableBaseName: not null })
            throw new DomainException(GameErrorType.InvalidState);

        var group = GetWorld().GetMysteryCollectionUpgradeGroup(upgrade.MysteryCollectionManager);

        var lootTable = GameSettingsManager.Instance.GetLootTable($"{upgrade.LootTableBaseName}_{group}") ?? throw new DomainException(GameErrorType.InvalidData);

        var upgradeItemName = lootTable.GetItemNameForRoll(RollFeature("lootTables", 0, 1000));

        obj.SetUpgradeItemName(upgradeItemName);

        return upgradeItemName;
    }

    public void IncrementExpansionsPurchased()
    {
        ExpansionsPurchased++;
    }

    public void SetWeatherEffectDisabled(bool disabled)
    {
        WeatherEffectDisabled = disabled;
    }

    public void SetNightMode(bool active)
    {
        NightModeActive = active;

        if (active)
        {
            HandleQuestsProgress("isNight");
        }
    }

    public void RemoveFranchiseLocation(string friendSnuid, int worldFlatId)
    {
        foreach (var franchise in Franchises)
        {
            var location = franchise.Locations.FirstOrDefault(l => l.Uid == friendSnuid && l.ObjectId == $"{worldFlatId}");

            if (location is null) continue;

            franchise.Locations.Remove(location);

            if (franchise.Locations.Count == 0)
                Franchises.Remove(franchise);

            return;
        }
    }

    private bool HasCompletedOrTradedInCollection(string collectionName)
    {
        var collectionSetting = GameSettingsManager.Instance.GetCollectionByName(collectionName);

        if (collectionSetting is null) return false;
        if (HasCompletedCollection(collectionSetting)) return true;

        return Collections.Any(x => x.Name == collectionName && x.Completed > 0);
    }

    private bool HasCompletedCollection(CollectionSetting targetCollection)
    {
        var collection = Collections.FirstOrDefault(x => x.Name == targetCollection.Name);

        if (collection is null) return false;

        var itemsToComplete = targetCollection.Collectables.Collectables.Count;
        var currentItems = 0;

        foreach (var items in targetCollection.Collectables.Collectables)
        {
            var userItem = collection.Items.FirstOrDefault(x => x.Name == items.Name);

            if (userItem is null) return false;
            if (userItem.Amount < 1) return false;

            currentItems++;
        }

        return itemsToComplete == currentItems;
    }

    public List<CollectionItem> CompleteCollection(CollectionSetting targetCollection)
    {
        if (!HasCompletedCollection(targetCollection)) return [];

        var collection = Collections.FirstOrDefault(x => x.Name == targetCollection.Name);

        if (collection is null) return [];

        var toRemove = new List<CollectionItem>();

        foreach (var items in targetCollection.Collectables.Collectables)
        {
            var removedItem = collection.RemoveItem(items.Name);

            if (removedItem is not null)
                toRemove.Add(removedItem);
        }

        GrantCollectionRewards(targetCollection);

        collection.Complete();
        
        return toRemove;
    }

    private void GrantCollectionRewards(CollectionSetting collectionSetting)
    {
        foreach (var (type, reward) in collectionSetting.TradeInRewards.Rewards)
        {
            switch (type)
            {
                case "xp":
                    AddXp(reward.Sum(x => int.Parse(x.Amount ?? "0")));
                    StaticLogger.Current.LogDebug("Added xp {XpAmount}", reward.Sum(x => int.Parse(x.Amount ?? "0")));
                    break;
                case "coin":
                    AddCoins(reward.Sum(x => int.Parse(x.Amount ?? "0")));
                    StaticLogger.Current.LogDebug("Added coins {CoinAmount}", reward.Sum(x => int.Parse(x.Amount ?? "0")));
                    break;
                case "goods":
                    AddGoods(reward.Sum(x => int.Parse(x.Amount ?? "0")));
                    StaticLogger.Current.LogDebug("Added goods {GoodsAmount}", reward.Sum(x => int.Parse(x.Amount ?? "0")));
                    break;
                case "premium_goods":
                    AddPremiumGoods(reward.Sum(x => int.Parse(x.Amount ?? "0")));
                    StaticLogger.Current.LogDebug("Added premium goods {GoodsAmount}", reward.Sum(x => int.Parse(x.Amount ?? "0")));
                    break;
                case "energy":
                    AddEnergy(reward.Sum(x => int.Parse(x.Amount ?? "0")));
                    StaticLogger.Current.LogDebug("Added energy {EnergyAmount}", reward.Sum(x => int.Parse(x.Amount ?? "0")));
                    break;
                case "item":
                    foreach (var item in reward)
                    {
                        AddItem(item.Name!);
                        StaticLogger.Current.LogDebug("Added item {ItemName}", item.Name);
                    }

                    break;
            }
        }
    }

    public int CountCollectableByName(string itemName)
    {
        return Collections.Sum(x => x.Items.Where(y => y.Name == itemName).Sum(y => y.Amount));
    }

    public bool HasLicense(string licenseName)
    {
        return Licenses.Any(x => x.Name == licenseName);
    }

    public void AddLicense(string licenseName)
    {
        var license = Licenses.FirstOrDefault(x => x.Name == licenseName);

        if (license is null)
            Licenses.Add(new LicenseItem(licenseName, 1));
        else
            license.Add(1);
    }

    public LicenseItem? DeleteLicense(string licenseName)
    {
        var license = Licenses.FirstOrDefault(x => x.Name == licenseName);

        if (license is null) return null;

        Licenses.Remove(license);
        return license;
    }

    public void UpdateFranchiseName(string franchiseType, string franchiseName)
    {
        var franchise = Franchises.FirstOrDefault(x => x.FranchiseType == franchiseType);

        if (franchise is null)
        {
            franchise = new Franchise(franchiseType, franchiseName);

            Franchises.Add(franchise);
        }

        franchise.SetFranchiseName(franchiseName);
    }

    public int CountOpenFranchiseLocations(string[]? franchiseTypes)
    {
        var franchises = franchiseTypes is null
            ? Franchises
            : Franchises.Where(x => franchiseTypes.Contains(x.FranchiseType));

        return franchises.Sum(x => x.Locations.Count(location => location.IsOpen()));
    }

    public void GrantCitySamHeadquarters()
    {
        foreach (var franchise in Franchises.Where(x => x.HasCitySamLocation()))
        {
            var hqName = GameSettingsManager.Instance.GetItem(franchise.FranchiseType)?.HeadquartersName;

            if (hqName is null || HasItem(hqName)) continue;

            AddItem(hqName);
        }
    }

    public void AddLotOrder(LotOrder lotOrder)
    {
        LotOrders.Add(lotOrder);
    }

    public void AddVisitorHelpOrder(VisitorHelpOrder order)
    {
        VisitorHelpOrders.Add(order);
    }

    public int GetExpansionTier()
    {
        var world = GetWorld();
        var lastTier = world.LastExpansionTier == 0 && world.Type != WorldType.Downtown ? ExpansionsPurchased : world.LastExpansionTier;

        return Math.Min(ExpansionsPurchased + 1, lastTier + GameSettingsManager.Instance.GetSettings().ExpansionDeltaMax);
    }

    public int GetRequiredExpansionPermits(int tier)
    {
        var isDowntown = GetWorld().Type == WorldType.Downtown;
        var expansions = GameSettingsManager.Instance.GetExpansions(isDowntown ? "appraisal_downtown" : "population");
        var rowCount = isDowntown ? Math.Min(60, expansions.Count) : expansions.Count;

        return expansions[Math.Min(tier, rowCount) - 1].Permits;
    }

    public int GetMasteryLevel(string itemName)
    {
        return Masteries.FirstOrDefault(x => x.ItemName == itemName)?.Level ?? 0;
    }

    public void BuyMasteryGate(string itemName, GatesItem gate)
    {
        var flag = $"buyout_{itemName}_{gate.Name}";

        if (HasSeenFlag(flag)) return;

        RemoveCash(GetMasteryGateCost(gate));
        SetSeenFlag(flag);
    }

    private int GetMasteryGateCost(GatesItem gate)
    {
        if (gate.Name != "mastery_crop") return gate.CashCost;

        var cost = gate.CashCost;

        foreach (var key in gate.Keys.OfType<GateKey>())
        {
            var total = GameSettingsManager.Instance.GetItem(key.Name)?.MasteryItems.ElementAtOrDefault(gate.MasteryLevel)?.RequiredCount ?? 0;
            var remaining = total - (Masteries.FirstOrDefault(x => x.ItemName == key.Name)?.Count ?? 0);

            if (remaining <= 0) return 0;

            cost = Math.Min(cost, (int)Math.Floor((double)remaining / total * gate.CashCost));
        }

        return Math.Max(cost, 1);
    }

    public void IncrementMastery(string itemName, int amount = 1)
    {
        // TODO: Implement bonusMultiplier with doobers collect

        var mastery = Masteries.FirstOrDefault(x => x.ItemName == itemName);

        if (mastery is null)
        {
            mastery = new Mastery(itemName);
            Masteries.Add(mastery);
        }

        mastery.AddCount(amount);

        var gameItem = GameSettingsManager.Instance.GetItem(itemName);

        if (gameItem is null) return;

        foreach (var masteryItem in gameItem.MasteryItems.OrderByDescending(x => x.Level))
        {
            if (masteryItem.RequiredCount is null || masteryItem.Level is null) continue;

            if (mastery.Count >= masteryItem.RequiredCount && mastery.Level != masteryItem.Level)
            {
                // TODO: Give rewards or implement MasteryRewardTransaction 
                mastery.LevelUp(masteryItem.Level.Value);
                break;
            }
        }
    }

    public void CollectFranchisesDailyBonus(string franchiseType)
    {
        var franchise = Franchises.FirstOrDefault(x => x.FranchiseType == franchiseType);

        if (franchise is null) throw new Exception($"Can't find franchise with type {franchiseType}");

        // TODO: Implement bonus based on index 1 => 25 coins, 2 => 50 coins ...
        var baseBonus = GameSettingsManager.Instance.GetSettings().Franchise1DailyBonus;

        var currentTime = ServerUtils.GetCurrentTimeSeconds();

        // TODO: Add server check
        franchise.TimeLastCollected = currentTime;

        AddCoins(baseBonus * franchise.Locations.Count);
    }

    public bool IsSamantha()
    {
        return Snuid == -1;
    }

    public void SetWorld(World world)
    {
        if (!IsSamantha())
            throw new Exception("SetWorld is only accessible to Samantha's city");

        Worlds.RemoveAll(x => x.Type == world.Type);
        Worlds.Add(world);
    }

    public World GetWorld()
    {
        return Worlds.FirstOrDefault(x => x.Type == LastPlayedWorldType)
               ?? throw new Exception($"GetWorld called but world {LastPlayedWorldType} is not loaded");
    }

    public void ToggleWorldTheme(string themeName, bool enable)
    {
        var item = GameSettingsManager.Instance.GetItem(themeName);

        if (item?.Type != "theme_collection") throw new DomainException(GameErrorType.InvalidData);

        if (!item.IsExpired(ServerUtils.GetCurrentUtcDateTime()))
        {
            if (item.Cost > 0)
                RemoveCoins(item.Cost.Value);
            else if (item.Cash > 0)
                RemoveCash(item.Cash.Value);
        }

        GetWorld().UpdateTheme(themeName, enable);
    }

    public World? GetWorldByType(WorldType type)
    {
        return Worlds.FirstOrDefault(x => x.Type == type);
    }

    public void AddWorld(World world)
    {
        if (Worlds.Any(x => x.Type == world.Type))
            throw new Exception($"Player already owns a world of type {world.Type}");

        Worlds.Add(world);
    }

    public bool IsWorldLoaded()
    {
        var world = Worlds.FirstOrDefault(x => x.Type == LastPlayedWorldType);

        return world != null && world.Objects.Count != 0;
    }

    public List<SocialNetworkUserDto> GetSocialNetworkUserFriendsList(string baseUrl)
    {
        return Friends.Where(f => f.FriendPlayer is not null && !f.FriendPlayer.IsSamantha()).Select(friend => friend.ToSocialNetworkUserDto(baseUrl)).ToList();
    }

    public bool HasFriend(Player friend)
    {
        return Friends.Any(x => x.GetFriend().Id == friend.Id);
    }

    public void SendFriendRequest(Player targetPlayer)
    {
        if (targetPlayer.Id == Id) throw new Exception("You cannot add yourself as a friend");
        if (HasFriend(targetPlayer)) throw new Exception("You are already friends");

        var friendship1 = new Friend(targetPlayer, this, true);
        var friendship2 = new Friend(this, targetPlayer, false);

        targetPlayer.Friends.Add(friendship1);
        Friends.Add(friendship2);
    }

    public void SwitchWorld(WorldType type)
    {
        LastPlayedWorldType = type;
    }

    private int GetGoodsByType(string goodType)
    {
        return goodType switch
        {
            "goods" => Goods,
            "premium_goods" => PremiumGoods,
            _ => 0
        };
    }

    private void RemoveGoodByType(string goodType, int amount)
    {
        switch (goodType)
        {
            case "goods":
                RemoveGoods(amount);
                break;
            case "premium_goods":
                RemovePremiumGoods(amount);
                break;
        }
    }

    public void ProcessGoods(GameItem item, string desiredGoodType = "goods", int? leftToPay = null)
    {
        var commodityReq = item.CommodityRequired;
        
        if (commodityReq is null) throw new Exception("Can't supply item without commodity req");
        if (item.Commodity.Count == 0) throw new Exception("Can't supply item without commodity");
        if (!item.Commodity.Any(x => x.Name == "goods")) desiredGoodType = "premium_goods";

        var toPay = leftToPay ?? commodityReq.Value;
        var leftGoods = toPay - GetGoodsByType(desiredGoodType);

        if (leftGoods > 0)
        {
            if (desiredGoodType == "premium_goods" || !item.Commodity.Any(x => x.Name == "premium_goods")) throw new DomainException(GameErrorType.NotEnoughMoney);

            var toRemove = toPay - leftGoods;

            if (toRemove > 0)
            {
                RemoveGoodByType(desiredGoodType, toRemove);
            }

            ProcessGoods(item, "premium_goods", leftGoods);
        }
        else
        {
            RemoveGoodByType(desiredGoodType, toPay);

            // Only trigger openBusinessByCommodityType if we use 100% of the resource
            HandleQuestsProgress("openBusinessByCommodityType", itemName: desiredGoodType);
        }
    }

    public void GiveUpgradeRewards(List<UpgradeReward> rewards)
    {
        foreach (var reward in rewards)
        {
            switch (reward.Type)
            {
                case "energy":
                    AddEnergy(reward.IntValue);
                    break;
                case "coin":
                    AddCoins(reward.IntValue);
                    break;
                case "itemUnlock":
                    SetSeenFlag(reward.Value);
                    break;
                case "xp":
                    AddXp(reward.IntValue);
                    break;
            }
            
            StaticLogger.Current.LogDebug("Added upgrade reward {Type} {Amount}", reward.Type, reward.Value);
        }
    }
}