using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Common.Global;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Settings.GameSettings;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Domain.GameEntities;
using Microsoft.Extensions.Logging;

namespace CityVilleDotnet.Domain.Entities;

public class Player
{
    public Guid Id { get; }
    public int Snuid { get; set; }
    public DateTimeOffset LastTrackingTimestamp { get; private set; }
    public bool SfxDisabled { get; private set; }
    public bool MusicDisabled { get; private set; }
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
    public int ExpansionsPurchased { get; private set; }
    public List<Collection> Collections { get; private set; } = [];
    public List<LicenseItem> Licenses { get; set; } = [];
    public List<Franchise> Franchises { get; set; } = [];
    public int RollCounter { get; private set; }
    public bool IsNew { get; private set; } = true;
    public bool FirstDay { get; private set; } = true;
    public DateTimeOffset CreationTimestamp { get; private set; }
    public string Username { get; private set; }
    public List<LotOrder> LotOrders { get; set; } = [];
    public List<VisitorHelpOrder> VisitorHelpOrders { get; set; } = [];
    public List<Mastery> Masteries { get; set; } = [];
    public List<World> Worlds { get; set; } = [];
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
        var collection = Collections.FirstOrDefault(x => x.Name == collectionName);

        if (collection is null)
        {
            collection = new Collection(collectionName);
            Collections.Add(collection);
        }

        collection.AddItem(itemName, amount);
    }

    public void AddItem(string itemName, int amount = 1, string? storageKey = null, WorldObject? storedObject = null)
    {
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
            throw new Exception("Not enough items");

        item.RemoveAmount(amount);

        if (item.Amount <= 0)
        {
            InventoryItems.Remove(item);
            return item;
        }

        return null;
    }

    public int CountInventoryItems()
    {
        return InventoryItems.Where(x => x.StorageType is null).Sum(x => x.Amount);
    }

    public int CountInventoryItem(string itemName)
    {
        return InventoryItems.Where(x => x.Name == itemName && x.StorageType is null).Sum(x => x.Amount);
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
        return EnergyMax + GetWorld().Objects.Where(o => o.EnergyModifier > 0).Sum(o => o.EnergyModifier);
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

    public bool RemoveEnergy(int amount)
    {
        var currentEnergy = CalculateCurrentEnergy();
        if (currentEnergy.CurrentNewEnergy < amount) throw new DomainException(GameErrorType.NotEnoughMoney);

        var maxEnergy = GetEnergyMax();
        var wasAtMax = Energy >= maxEnergy;
        Energy -= amount;

        if (wasAtMax && Energy < maxEnergy)
        {
            TimeBeforeNextEnergy = ServerUtils.GetCurrentTime();
        }
        else if (Energy < maxEnergy)
        {
            TimeBeforeNextEnergy = ServerUtils.GetCurrentTime() - (long)(currentEnergy.TimeToRegen - currentEnergy.TimeUntilNextRegen);
        }

        return true;
    }

    public void UpdateEnergy()
    {
        var currentEnergy = CalculateCurrentEnergy();
        var maxEnergy = GetEnergyMax();
        
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

        Energy += amount;

        StaticLogger.Current.LogDebug("New energy after addition: {NewEnergy}", Energy);

        if (Energy >= GetEnergyMax())
        {
            TimeBeforeNextEnergy = ServerUtils.GetCurrentTime();
        }
        else
        {
            TimeBeforeNextEnergy = ServerUtils.GetCurrentTime() - (long)(currentEnergy.TimeToRegen - currentEnergy.TimeUntilNextRegen);
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
        foreach (var item in GameSettingsManager.Instance.GetLevels())
        {
            if (Xp < item.RequiredXp) continue;

            var level = item.Num;

            if (level <= Level) continue;

            var energyMax = item.EnergyMax;

            // TODO: Add heldEnergy and cash
            var energy = energyMax + Math.Max(Energy - energyMax, 0);

            Level = level;
            Energy = energy;
            EnergyMax = energyMax;
            TimeBeforeNextEnergy = ServerUtils.GetCurrentTime();
            AddCash(GameSettingsManager.Instance.GetSettings().CashGainedPerLevel);

            UpdateEnergy();

            break;
        }
    }

    public void CompleteTutorial()
    {
        IsNew = false;
        FirstDay = false;
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

    public void SetSeenFlag(string flag)
    {
        if (!SeenFlags.Any(x => x.Key == flag))
        {
            SeenFlags.Add(new SeenFlag(flag));
        }
    }

    private void IncrementRollCounter()
    {
        RollCounter++;
    }

    public void IncrementExpansionsPurchased()
    {
        ExpansionsPurchased++;
    }

    // From Player::processRandomModifiers → processRandomModifiersWithTable → processRandomModifiersFromConfig
    public List<int> CollectDoobersRewards(string itemName, string modifierGroupName = "default", int coinMultiplier = 1, bool construction = false)
    {
        var gameItem = GameSettingsManager.Instance.GetItem(itemName);
        if (gameItem is null) return [];

        var secureRands = new List<int>();

        // From Player::selectLocalRandomModifiers
        var modifiers = SelectRandomModifiers(gameItem, modifierGroupName, secureRands);

        if (modifiers is null || modifiers.Count == 0) return secureRands;

        // From Player::processRandomModifiersWithTable with (defaultenergyactionpack)
        var packModifiers = GameSettingsManager.Instance.GetRandomModifierPackModifiers("defaultenergyactionpack");

        if (packModifiers is not null && packModifiers.Count > 0)
            modifiers.AddRange(packModifiers);

        // process each modifier (processRandomModifiersFromConfig)
        ProcessModifiers(gameItem, modifiers, secureRands, coinMultiplier, construction);

        return secureRands;
    }

    // From Player::selectLocalRandomModifiers + chooseRandomModifiersXml
    private List<RandomModifier>? SelectRandomModifiers(GameItem gameItem, string modifierGroupName, List<int> secureRands)
    {
        var groups = gameItem.RandomModifierGroups?.Groups;

        if (groups is not null && groups.Count > 0)
        {
            var group = groups.FirstOrDefault(g => g.Name == modifierGroupName);

            if (group?.Entries is not null && group.Entries.Count > 0)
            {
                // choose which modifier set to use (chooseRandomModifiersXml)
                IncrementRollCounter();

                var secureRand = SecureRand.GenerateRand(0, 99, RollCounter, Snuid.ToString());
                secureRands.Add(secureRand);

                StaticLogger.Current.LogDebug("RandomModifierGroup roll for {ItemName} group {GroupName}: rollCounter={RollCounter} => {SecureRand}", gameItem.Name, modifierGroupName, RollCounter, secureRand);

                double runningPercent = 0;
                string? selectedName = null;

                foreach (var entry in group.Entries)
                {
                    runningPercent += entry.Percent;

                    if (secureRand < runningPercent)
                    {
                        selectedName = entry.Name;
                        break;
                    }
                }

                if (selectedName is not null)
                {
                    var namedModifiers = gameItem.RandomModifiersList.FirstOrDefault(rm => rm.Name == selectedName);

                    if (namedModifiers?.Modifiers is not null)
                        return namedModifiers.Modifiers;
                }

                // Client falls back to the default table when the group roll doesn't resolve (selectLocalRandomModifiers)
            }
        }

        // Client fallback is the table named "default", else the first one (Item::randomModifiersXml)
        var defaultModifiers = gameItem.RandomModifiersList.FirstOrDefault(rm => rm.Name == "default") ?? gameItem.RandomModifiersList.FirstOrDefault();

        return defaultModifiers?.Modifiers;
    }

    // From Player::processRandomModifiersFromConfig
    private void ProcessModifiers(GameItem gameItem, List<RandomModifier> modifiers, List<int> secureRands, int coinMultiplier = 1, bool construction = false)
    {
        foreach (var itemModifier in modifiers)
        {
            // From client: ConstructionSite modifier filtering (processRandomModifiersWithTable XML filter)
            // Client keeps a modifier only if allowOnBuild="true", or if allowOnBuild is absent AND type="xp"
            if (construction)
            {
                if (itemModifier.HasAllowOnBuildAttribute && !itemModifier.AllowOnBuild)
                    continue;
                if (!itemModifier.HasAllowOnBuildAttribute && itemModifier.Type != "xp")
                    continue;
            }

            // Skip validates by default, we should validate before but not necessary
            if (!string.IsNullOrEmpty(itemModifier.Validate))
            {
                StaticLogger.Current.LogWarning("Skipping modifier with validate={Validate} for {ItemName}", itemModifier.Validate, gameItem.Name);
                continue;
            }

            // TODO: Maybe implement this
            // client use ExperimentManager.getVariant()
            if (!string.IsNullOrEmpty(itemModifier.ExperimentName))
            {
                var variants = itemModifier.Variants?.Split(',') ?? [];

                if (!variants.Contains("0"))
                {
                    StaticLogger.Current.LogDebug("Skipping modifier experimentName={ExperimentName} variants={Variants} for {ItemName}", itemModifier.ExperimentName, itemModifier.Variants, gameItem.Name);
                    continue;
                }
            }

            var modifierTable = GameSettingsManager.Instance.GetRandomModifier(itemModifier.TableName);

            // Client only rolls when the table exists, otherwise the roll counter must not move (processRandomModifiersFromConfig)
            if (modifierTable is null) continue;

            IncrementRollCounter();

            var secureRand = SecureRand.GenerateRand(0, modifierTable.RollRange, RollCounter, Snuid.ToString());

            StaticLogger.Current.LogDebug("SecureRand for {DebugName}: rollCounter={PlayerRollCounter} => {SecureRand}", gameItem.Name, RollCounter, secureRand);

            secureRands.Add(secureRand);

            StaticLogger.Current.LogDebug("Checking random table named {ModifierTableName} type {ModifierTableType} with rand {SecureRand}", modifierTable.Name, modifierTable.Type, secureRand);

            double previousRollPercent = 0;
            var found = false;

            foreach (var roll in modifierTable.Rolls)
            {
                if (roll.Percent > 0)
                {
                    var currentRollPercent = roll.Percent + previousRollPercent;

                    if (secureRand < currentRollPercent && !found)
                    {
                        ApplyRollRewards(roll, itemModifier.Multiplier, coinMultiplier);
                        found = true;
                    }

                    previousRollPercent = currentRollPercent;
                }
            }
        }
    }

    private void ApplyRollRewards(Roll roll, double multiplier, int coinMultiplier = 1)
    {
        foreach (var (rewardType, rewardElements) in roll.Rewards)
        {
            foreach (var element in rewardElements)
            {
                var amount = (int)Math.Ceiling(element.Amount / roll.Divisor * multiplier);

                switch (rewardType)
                {
                    case "coin":
                        var coinAmount = (int)Math.Ceiling(element.Amount / roll.Divisor * multiplier * coinMultiplier);
                        if (coinAmount <= 0) break;
                        AddCoins(coinAmount);
                        StaticLogger.Current.LogDebug("Found coin {CoinAmount}", coinAmount);
                        break;
                    case "xp":
                        if (amount <= 0) break;
                        AddXp(amount);
                        StaticLogger.Current.LogDebug("Found xp {XpAmount}", amount);
                        break;
                    case "energy":
                        if (amount <= 0) break;
                        AddEnergy(amount);
                        StaticLogger.Current.LogDebug("Found energy {EnergyAmount}", amount);
                        break;
                    case "collectable":
                        var collectionName = GameSettingsManager.Instance.GetCollectionByItemName(element.Name);

                        if (collectionName is not null)
                        {
                            AddItemToCollection(collectionName, element.Name);
                            StaticLogger.Current.LogDebug("Added {CollectableName} to collection {CollectionName}",
                                element.Name, collectionName);
                        }
                        else
                        {
                            StaticLogger.Current.LogWarning("Collection for item {CollectableName} not found",
                                element.Name);
                        }

                        break;
                    case "food" or "goods":
                        if (amount <= 0) break;
                        AddGoods(amount);
                        StaticLogger.Current.LogDebug("Found goods {GoodsAmount}", amount);
                        break;
                    case "premium_goods":
                        if (amount <= 0) break;
                        AddPremiumGoods(amount);
                        StaticLogger.Current.LogDebug("Found premium goods {PremiumGoodsAmount}", amount);
                        break;
                    case "cash":
                        if (amount <= 0) break;
                        AddCash(amount);
                        StaticLogger.Current.LogDebug("Found cash {CashAmount}", amount);
                        break;
                    case "rep":
                        if (amount <= 0) break;
                        AddSocialXp(amount);
                        StaticLogger.Current.LogDebug("Found rep {RepAmount}", amount);
                        break;
                    case "appraisal":
                        if (amount <= 0) break;
                        var addedAppraisal = GetWorld().AddBonusAppraisal(amount);
                        StaticLogger.Current.LogDebug("Found appraisal {AppraisalAmount}, distributed {DistributedAppraisal}", amount, addedAppraisal);
                        break;
                    case "item" or "profit":
                        // client never stores population items => InventoryCheckManager::onPopulationAdd
                        var populationQuantity = GameSettingsManager.Instance.GetItem(element.Name)?.GetPopulationAddQuantity();

                        if (populationQuantity is not null)
                        {
                            var addedPopulation = GetWorld().AddBonusPopulation(populationQuantity.Value);
                            StaticLogger.Current.LogDebug("Found population item {ItemName}, distributed {Population} population", element.Name, addedPopulation);
                            break;
                        }

                        AddItem(element.Name);
                        StaticLogger.Current.LogDebug("Found item drop {ItemName}", element.Name);
                        break;
                    default:
                        StaticLogger.Current.LogWarning("Unhandled reward type {RewardType}", rewardType);
                        break;
                }
            }
        }
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

        foreach (var (type, reward) in targetCollection.TradeInRewards.Rewards)
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

        collection.Complete();

        StaticLogger.Current.LogDebug("Collection {CollectionName} completed with {ItemsCount} items removed", collection.Name, toRemove.Count);

        return toRemove;
    }

    public int CountCollectableByName(string itemName)
    {
        return Collections.Sum(x => x.Items.Count(y => y.Name == itemName));
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

    public void AddLotOrder(LotOrder lotOrder)
    {
        LotOrders.Add(lotOrder);
    }

    public void AddVisitorHelpOrder(VisitorHelpOrder order)
    {
        VisitorHelpOrders.Add(order);
    }

    public int GetNextPermitCost()
    {
        var expansionData = GetExpansionData();

        return expansionData != null ? expansionData[2] : 1;
    }

    public int[]? GetExpansionData()
    {
        var nextNum = ExpansionsPurchased + 1;

        foreach (var expansion in GameSettingsManager.Instance.GetExpansions())
        {
            var expansionNum = int.Parse(expansion.Num);

            // TODO: Support "MAX" num
            if (expansionNum == nextNum)
            {
                return [int.Parse(expansion.Level), int.Parse(expansion.Permits), int.Parse(expansion.Cost)];
            }
        }

        return null;
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

    public void HandleQuestsProgress(string actionType, string? className = null, string? itemName = null, int amount = 0)
    {
        if (StaticLogger.IsReady()) StaticLogger.Current.LogDebug("Handle quest actionType = {ActionType}, className = {ClassName}, itemName = {ItemName}, amount = {Amount}", actionType, className, itemName, amount);

        var calculatedResults = new Dictionary<string, int>();

        foreach (var quest in Quests.Where(x => x.QuestType == QuestType.Active))
        {
            var questItem = QuestSettingsManager.Instance.GetItem(quest.Name);

            if (questItem is null) continue;

            var index = -1;

            foreach (var task in questItem.Tasks.Tasks)
            {
                index++;

                if (quest.Progress[index] + quest.Purchased[index] >= int.Parse(task.Total)) continue;

                var actionTask = task.Action;
                var taskType = task.Type ?? "";
                var splitType = taskType.Contains(',') ? taskType.Split(',') : null;

                var gameItem = itemName is not null ? GameSettingsManager.Instance.GetItem(itemName) : null;

                // When user performs an action
                if (!string.IsNullOrEmpty(actionType) && actionTask.Equals(actionType))
                {
                    switch (actionType)
                    {
                        case "seenQuest":
                        case "popNews":
                        case "sendTrain":
                        case "welcomeTrain":
                        case "neighborVisit":
                        case "onValidCityName":
                        case "incrementalExpansionCount":
                        case "expand":
                        case "buildingremodeled":
                            quest.Progress[index] += 1;
                            break;
                        case "harvestByClass":
                        case "startContractByClass":
                        case "placeByClass":
                        case "harvestBusinessByClass":
                        case "clearByClass":
                        case "openBusinessByClass":
                        case "storeItemByClass":
                        case "finishConstructionByClass":
                        {
                            if (className is null)
                                throw new Exception("Can't validate byClass action without className");

                            if (taskType.Equals(className) || (splitType is not null && splitType.Contains(className)))
                                quest.Progress[index] += 1;

                            break;
                        }
                        case "harvestResidenceByName":
                        case "harvestPlotByName":
                        case "openBusinessByName":
                        case "harvestBusinessByName":
                        case "placeBuildingByName":
                        case "sendTourNeighborBusinessByName":
                        case "finishConstructionByName":
                        case "openBusinessByCommodityType":
                        case "transferFromStorageToDisplay":
                        case "travel":
                        case "harvestContractByName":
                        case "harvestItemByName":
                        {
                            if (itemName is null)
                                throw new Exception("Can't validate byName action without itemName");

                            if (taskType.Equals(itemName) || (splitType is not null && splitType.Contains(itemName)))
                                quest.Progress[index] += 1;

                            break;
                        }
                        case "placeByKeyword":
                        case "harvestByKeyword":
                        case "openBusinessByKeyword":
                        case "finishConstructionByKeyword":
                            if (itemName is null)
                                throw new Exception("Can't validate byKeyword action without itemName");

                            if (gameItem is null)
                                throw new Exception("Can't validate byKeyword action without gameItem");

                            if (gameItem.HasKeyword(taskType))
                                quest.Progress[index] += 1;

                            break;
                        case "visitorHelp":
                            // plotHarvest, businessSendTour, ...
                            if (task.Type == className)
                                quest.Progress[index] += 1;

                            break;
                        case "deliver":
                            if (task.Type == itemName)
                                quest.Progress[index] += amount;
                            break;
                        case "incrementalPopulationCount":
                            quest.Progress[index] += amount;
                            break;
                    }
                }

                // Here we can check global values like counting population or buildings

                if (!IsWorldLoaded() || task.Type is null) continue;

                var resultKey = $"{task.Action}_{taskType}";
                var value = 0;

                switch (actionTask)
                {
                    case "countWorldObjectByName":
                    case "countConstructionOrBuildingByName":
                    {
                        //bus_toyota1_zyngage,bus_toyota1_zyngage_2,bus_toyota1_zyngage_3
                        var names = splitType ?? [taskType];

                        if (!calculatedResults.TryGetValue(resultKey, out value))
                        {
                            value = actionTask.Equals("countConstructionOrBuildingByName")
                                ? names.Sum(x => GetWorld().CountConstructionOrBuildingByName(x))
                                : names.Sum(x => GetWorld().CountBuildingByName(x));

                            calculatedResults[resultKey] = value;
                        }

                        quest.Progress[index] = value;

                        continue;
                    }
                    case "countWorldObjectByRegEx":
                    {
                        if (!calculatedResults.TryGetValue(resultKey, out value))
                            calculatedResults[resultKey] = value = GetWorld().CountBuildingByRegex(task.Type);

                        quest.Progress[index] = value;
                        continue;
                    }
                    case "countUpgradeItemByRootName":
                    {
                        if (!calculatedResults.TryGetValue(resultKey, out value))
                        {
                            var roots = splitType ?? [taskType];
                            var descendants = roots
                                .SelectMany(root => GameSettingsManager.Instance.GetOrderedUpgradeChainByRoot(root).Skip(1))
                                .ToList();

                            value = descendants.Count > 0 ? GetWorld().CountBuildingByNames(descendants) : 0;
                            calculatedResults[resultKey] = value;
                        }

                        quest.Progress[index] = value;
                        continue;
                    }
                    case "countPlayerResourceByType":
                        quest.Progress[index] = task.Type switch
                        {
                            // population,ghost
                            "population" => GetWorld().GetCurrentPopulation(),
                            "coin" => Gold,
                            "goods" => Goods,
                            _ => 0
                        };

                        continue;
                    case "countCollectableByName":
                        if (!calculatedResults.TryGetValue(resultKey, out value))
                            calculatedResults[resultKey] = value = CountCollectableByName(task.Type);

                        quest.Progress[index] = value;
                        continue;
                    case "isQuestCompleted":
                        quest.Progress[index] = Quests.Count(q => q.Name == task.Type);
                        continue;
                    case "countWorldObjectByKeyword":
                        if (!calculatedResults.TryGetValue(resultKey, out value))
                            calculatedResults[resultKey] = value = GetWorld().CountWorldObjectByKeyword(task.Type);

                        quest.Progress[index] = value;
                        continue;
                    case "countAnimalsObjects":
                        if (!calculatedResults.TryGetValue(resultKey, out value))
                            calculatedResults[resultKey] = value = GetWorld().CountZooAnimals(taskType);
                        
                        if (value > quest.Progress[index])
                            quest.Progress[index] = value;

                        continue;
                    case "countNumAtThisStreak":
                        var count = GetWorld().CountStreakByItemName(task.Type);

                        if (count >= task.Streak)
                            quest.Progress[index] = count;
                        continue;
                    case "checkStreakEffect":
                        var effect = GetWorld().GetStreakEffectByItemName(task.Type);

                        if (effect >= task.Streak)
                            quest.Progress[index] = effect;
                        continue;
                }
            }
        }
    }

    public void CheckCompletedQuests()
    {
        var newQuests = new List<Quest>();

        foreach (var item in Quests.Where(x => x.QuestType == QuestType.Active))
        {
            if (item.IsCompleted())
            {
                item.QuestType = QuestType.Completed;
                item.ClaimRewards(this);

                newQuests = item.StartSequels();
            }
        }

        Quests.AddRange(newQuests);
    }

    public void ExpireQuest(string questName)
    {
        var quest = Quests.FirstOrDefault(x => x.Name == questName && x.QuestType == QuestType.Active);

        if (quest is null) return;

        quest.QuestType = QuestType.Expired;
    }

    public List<SocialNetworkUserDto> GetSocialNetworkUserFriendsList(string baseUrl)
    {
        return Friends.Where(f => !f.FriendPlayer.IsSamantha()).Select(friend => friend.ToSocialNetworkUserDto(baseUrl)).ToList();
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
        var commodityReq = item.GetCommodityRequired();
        
        if (commodityReq is null) throw new Exception("Can't supply item without commodity req");
        if (item.Commodity.Count == 0)
        {
            var parentItem = item.GetFirstDeriveItem(item);

            if (parentItem is null || parentItem.Commodity.Count == 0) throw new Exception("Can't supply item without commodity");

            item.Commodity = parentItem.Commodity;
        }

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
                //HandleQuestsProgress("openBusinessByCommodityType", itemName: desiredGoodType);
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