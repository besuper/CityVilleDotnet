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
    public int LastTrackingTimestamp { get; private set; }
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
    public int TimeBeforeNextEnergy { get; private set; }
    public List<SeenFlag> SeenFlags { get; set; } = new();
    public int ExpansionsPurchased { get; private set; }
    public List<Collection> Collections { get; private set; } = [];
    public List<LicenseItem> Licenses { get; set; } = [];
    public List<Franchise> Franchises { get; set; } = [];
    public int RollCounter { get; private set; }
    public bool IsNew { get; private set; } = true;
    public bool FirstDay { get; private set; } = true;
    public int CreationTimestamp { get; private set; }
    public string Username { get; private set; }
    public List<LotOrder> LotOrders { get; set; } = [];
    public List<VisitorHelpOrder> VisitorHelpOrders { get; set; } = [];
    public List<Mastery> Masteries { get; set; } = [];
    public World? World { get; private set; }
    public List<Quest> Quests { get; } = [];
    public List<Friend> Friends { get; } = [];
    public ApplicationUser? AppUser { get; private set; }

    public Player()
    {
    }

    public Player(ApplicationUser appUser, World world)
    {
        Id = Guid.NewGuid();
        Cash = 900;
        Gold = 50000;
        Energy = 12;
        EnergyMax = 12;
        Goods = 100;
        PremiumGoods = 0;
        Username = appUser.UserName!;
        CreationTimestamp = (int)ServerUtils.GetCurrentTime();
        World = world;
        AppUser = appUser;
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

    public int CountInventoryItems()
    {
        return InventoryItems.Sum(x => x.Amount);
    }

    public int CountInventoryItem(string itemName)
    {
        return InventoryItems.Where(x => x.Name == itemName).Sum(x => x.Amount);
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

        if (timeSinceLastRegen < 0)
        {
            currentNewEnergy = EnergyMax;
            timeSinceLastRegen = 0;
        }

        return new Energy(currentNewEnergy, timeToRegen, timeUntilNextRegen, timeSinceLastRegen);
    }

    public bool RemoveEnergy(int amount)
    {
        var currentEnergy = CalculateCurrentEnergy();
        if (currentEnergy.CurrentNewEnergy < amount) return false;

        var wasAtMax = Energy >= EnergyMax;
        Energy -= amount;

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

        Energy += amount;

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

        var levelData = GameSettingsManager.Instance.GetLevels()
            .FirstOrDefault(x => int.Parse(x.Num) == level);

        if (levelData is not null)
        {
            EnergyMax = int.Parse(levelData.EnergyMax);
            Xp = Math.Max(Xp, int.Parse(levelData.RequiredXp));
        }
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
        Gold -= amount;
    }

    public void RemoveGoods(int amount)
    {
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

                return null;
            }
        }

        return gameItem.RandomModifiersList.FirstOrDefault()?.Modifiers;
    }

    // From Player::processRandomModifiersFromConfig
    private void ProcessModifiers(GameItem gameItem, List<RandomModifier> modifiers, List<int> secureRands, int coinMultiplier = 1, bool construction = false)
    {
        foreach (var itemModifier in modifiers)
        {
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

            IncrementRollCounter();

            var modifierTable = GameSettingsManager.Instance.GetRandomModifier(itemModifier.TableName);
            var rollRange = modifierTable?.RollRange ?? 99;
            var secureRand = SecureRand.GenerateRand(0, rollRange, RollCounter, Snuid.ToString());

            StaticLogger.Current.LogDebug("SecureRand for {DebugName}: rollCounter={PlayerRollCounter} => {SecureRand}", gameItem.Name, RollCounter, secureRand);

            secureRands.Add(secureRand);

            if (modifierTable is null) continue;

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
                        ApplyRollRewards(roll, itemModifier.Multiplier, coinMultiplier, construction);
                        found = true;
                    }

                    previousRollPercent = currentRollPercent;
                }
            }
        }
    }

    private void ApplyRollRewards(Roll roll, double multiplier, int coinMultiplier = 1, bool construction = false)
    {
        foreach (var (rewardType, rewardElements) in roll.Rewards)
        {
            // Construction build stages only apply xp and item/profit (check: ConstructionSite.makeDoobers)
            if (construction && rewardType is not ("xp" or "item" or "profit"))
                continue;

            var totalAmount = rewardElements.Sum(x => x.Amount) / roll.Divisor;

            switch (rewardType)
            {
                case "coin":
                    var coinAmount = (int)Math.Ceiling(totalAmount * multiplier * coinMultiplier);
                    AddCoins(coinAmount);
                    StaticLogger.Current.LogDebug("Found coin {CoinAmount}", coinAmount);
                    break;
                case "xp":
                    var xpAmount = (int)(totalAmount * multiplier);
                    AddXp(xpAmount);
                    StaticLogger.Current.LogDebug("Found xp {XpAmount}", xpAmount);
                    break;
                case "energy":
                    var energyAmount = (int)(totalAmount * multiplier);
                    AddEnergy(energyAmount);
                    StaticLogger.Current.LogDebug("Found energy {EnergyAmount}", energyAmount);
                    break;
                case "collectable":
                    foreach (var element in rewardElements)
                    {
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
                    }

                    break;
                case "food" or "goods":
                    var goodsAmount = (int)(totalAmount * multiplier);
                    AddGoods(goodsAmount);
                    StaticLogger.Current.LogDebug("Found goods {GoodsAmount}", goodsAmount);
                    break;
                case "premium_goods":
                    var premiumGoodsAmount = (int)(totalAmount * multiplier);
                    AddPremiumGoods(premiumGoodsAmount);
                    StaticLogger.Current.LogDebug("Found premium goods {PremiumGoodsAmount}", premiumGoodsAmount);
                    break;
                case "cash":
                    var cashAmount = (int)(totalAmount * multiplier);
                    AddCash(cashAmount);
                    StaticLogger.Current.LogDebug("Found cash {CashAmount}", cashAmount);
                    break;
                case "rep":
                    var repAmount = (int)(totalAmount * multiplier);
                    AddSocialXp(repAmount);
                    StaticLogger.Current.LogDebug("Found rep {RepAmount}", repAmount);
                    break;
                case "item" or "profit":
                    foreach (var element in rewardElements)
                    {
                        AddItem(element.Name, 1);
                        StaticLogger.Current.LogDebug("Found item drop {ItemName}", element.Name);
                    }

                    break;
                default:
                    StaticLogger.Current.LogWarning("Unhandled reward type {RewardType}", rewardType);
                    break;
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

    public void IncrementMastery(string itemName)
    {
        // TODO: Implement bonusMultiplier with doobers collect

        var mastery = Masteries.FirstOrDefault(x => x.ItemName == itemName);

        if (mastery is null)
        {
            mastery = new Mastery(itemName);
            Masteries.Add(mastery);
        }

        mastery.AddCount();

        var gameItem = GameSettingsManager.Instance.GetItem(itemName);

        if (gameItem is null) return;

        foreach (var masteryItem in gameItem.MasteryItems)
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
        var baseBonus = GameSettingsManager.Instance.GetInt("Franchise1DailyBonus");

        var currentTime = (long)ServerUtils.GetCurrentTimeSeconds();

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

        World = world;
    }

    public World GetWorld()
    {
        if (World is null) throw new Exception("GetWorld called on not loaded world");

        return World;
    }

    public bool IsWorldLoaded()
    {
        return World != null && World.Objects.Count != 0;
    }

    public void SetupNewPlayer(ApplicationUser user)
    {
        // Setup first quest
        Quests.Add(Quest.Create("q_rename_city", 1, QuestType.Active));
    }

    public void HandleQuestsProgress(string actionType, string? className = null, string? itemName = null)
    {
        StaticLogger.Current.LogDebug("Handle quest actionType = {ActionType}, className = {ClassName}, itemName = {ItemName}", actionType, className, itemName);

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
                            quest.Progress[index] += 1;
                            break;
                        case "harvestByClass":
                        case "startContractByClass":
                        case "placeByClass":
                        case "harvestBusinessByClass":
                        case "clearByClass":
                        case "openBusinessByClass":
                        {
                            if (className is null)
                                throw new Exception("Can't validate byClass action without className");

                            if (taskType.Equals(className))
                                quest.Progress[index] += 1;

                            break;
                        }
                        case "harvestResidenceByName":
                        case "harvestPlotByName":
                        case "openBusinessByName":
                        case "harvestBusinessByName":
                        case "placeBuildingByName":
                        case "sendTourNeighborBusinessByName":
                        {
                            if (itemName is null)
                                throw new Exception("Can't validate byName action without itemName");

                            if (taskType.Equals(itemName) || (splitType is not null && splitType.Contains(itemName)))
                                quest.Progress[index] += 1;

                            break;
                        }
                        case "placeByKeyword":
                        case "harvestByKeyword":
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
                    }
                }

                // Here we can check global values like counting population or buildings

                if (!IsWorldLoaded() || task.Type is null) continue;

                var resultKey = $"{task.Action}_{taskType}";
                var value = 0;

                switch (actionTask)
                {
                    // FIXME: countConstructionOrBuildingByName
                    case "countWorldObjectByName":
                    case "countConstructionOrBuildingByName":
                    {
                        if (splitType is null)
                        {
                            if (!calculatedResults.TryGetValue(resultKey, out value))
                                calculatedResults[resultKey] = value = GetWorld().CountBuildingByName(task.Type);
                        }
                        else
                        {
                            //bus_toyota1_zyngage,bus_toyota1_zyngage_2,bus_toyota1_zyngage_3
                            if (!calculatedResults.TryGetValue(resultKey, out value))
                                calculatedResults[resultKey] = value = splitType.Sum(x => GetWorld().CountBuildingByName(x));
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
    
    public List<SocialNetworkUserDto> GetSocialNetworkUserFriendsList(string baseUrl)
    {
        return Friends.Where(f => !f.FriendPlayer.IsSamantha()).Select(friend => friend.ToSocialNetworkUserDto(baseUrl)).ToList();
    }
}