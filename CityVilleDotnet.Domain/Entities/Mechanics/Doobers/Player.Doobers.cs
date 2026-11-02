using CityVilleDotnet.Common.Global;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Settings.GameSettings;
using CityVilleDotnet.Common.Settings.QuestSettings;
using CityVilleDotnet.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace CityVilleDotnet.Domain.Entities;

public partial class Player
{
    // From Player::processRandomModifiers → processRandomModifiersWithTable → processRandomModifiersFromConfig
    public List<int> CollectDoobersRewards(string itemName, string modifierGroupName = "default", double coinMultiplier = 1, bool construction = false, double premiumGoodsMultiplier = 1, string? modifiersName = null, double goodsMultiplier = 1)
    {
        var gameItem = GameSettingsManager.Instance.GetItem(itemName);
        if (gameItem is null) return [];

        var secureRands = new List<int>();

        // From Player::selectLocalRandomModifiers
        var selectedModifiers = modifiersName is null
            ? SelectRandomModifiers(gameItem, modifierGroupName, secureRands)
            : gameItem.RandomModifiersList.FirstOrDefault(rm => rm.Name == modifiersName)?.Modifiers;

        if (selectedModifiers is null || selectedModifiers.Count == 0) return secureRands;

        // Client copies the modifiers before appending, the game settings must stay untouched
        var modifiers = new List<RandomModifier>(selectedModifiers);

        // From Player::processRandomModifiersWithTable with (defaultenergyactionpack)
        var packModifiers = GameSettingsManager.Instance.GetRandomModifierPackModifiers("defaultenergyactionpack");

        if (packModifiers is not null && packModifiers.Count > 0)
            modifiers.AddRange(packModifiers);

        // Client skips the global tables for construction sites (processRandomModifiersWithTable)
        if (!construction)
            modifiers.AddRange(SelectGlobalTableModifiers(gameItem));

        // process each modifier (processRandomModifiersFromConfig)
        ProcessModifiers(gameItem, modifiers, secureRands, coinMultiplier, construction, premiumGoodsMultiplier, goodsMultiplier);

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

    // From Player::processRandomModifiersWithTable, tables registered by GlobalTableModifierMechanic and by the active quests
    private List<RandomModifier> SelectGlobalTableModifiers(GameItem gameItem)
    {
        if (gameItem.Keywords.Count == 0) return [];

        var globalTables = GetWorld().GetGlobalTableModifiers()
            .Where(x => GameSettingsManager.Instance.IsValidatorSatisfied(x.Validate, Level))
            .Select(x => (Keyword: x.TargetKeyword!, Table: x.Table!))
            .Concat(GlobalTableOverrides.Select(x => (x.Keyword, x.Table)))
            .ToList();

        var modifiers = new List<RandomModifier>();
        
        foreach (var keyword in gameItem.Keywords)
        {
            var tableNames = globalTables
                .Where(x => x.Keyword == keyword)
                .Select(x => x.Table)
                .Distinct()
                .Order(StringComparer.Ordinal);

            foreach (var tableName in tableNames)
            {
                var table = GameSettingsManager.Instance.GetRandomModifier(tableName);

                if (table is null) continue;

                modifiers.Add(new RandomModifier { Type = table.Type, TableName = table.Name });

                StaticLogger.Current.LogDebug("Added global table {TableName} for {ItemName} with keyword {Keyword}", table.Name, gameItem.Name, keyword);
            }
        }

        return modifiers;
    }

    public bool AddQuestTableOverride(string questName, int taskId, string keyword, string table)
    {
        if (!Quests.Any(x => x.Name == questName && x.QuestType == QuestType.Active)) return false;

        var task = QuestSettingsManager.Instance.GetItem(questName)?.Tasks.Tasks.ElementAtOrDefault(taskId);

        if (task is null || !task.OverrideTables.Any(x => x.Keyword == keyword && x.Table == table)) return false;

        GlobalTableOverrides.Add(new GlobalTableOverride(keyword, table, questName));

        return true;
    }

    public void RemoveQuestTableOverride(string questName, string keyword, string table)
    {
        var tableOverride = GlobalTableOverrides.FirstOrDefault(x => x.Keyword == keyword && x.Table == table && x.Source == questName);

        if (tableOverride is null) return;

        GlobalTableOverrides.Remove(tableOverride);
    }

    public void AddObjectTableOverrides(int objectId, List<QuestOverrideTable> tables)
    {
        var source = objectId.ToString();

        foreach (var table in tables)
        {
            if (!GlobalTableOverrides.Any(x => x.Keyword == table.Keyword && x.Table == table.Table && x.Source == source))
            {
                GlobalTableOverrides.Add(new GlobalTableOverride(table.Keyword, table.Table, source));
            }
        }
    }

    public void RemoveObjectTableOverrides(int objectId)
    {
        GlobalTableOverrides.RemoveAll(x => x.Source == objectId.ToString());
    }

    // From Player::processRandomModifiersFromConfig
    private void ProcessModifiers(GameItem gameItem, List<RandomModifier> modifiers, List<int> secureRands, double coinMultiplier = 1, bool construction = false, double premiumGoodsMultiplier = 1, double goodsMultiplier = 1)
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
                        ApplyRollRewards(roll, itemModifier.Multiplier, coinMultiplier, premiumGoodsMultiplier, goodsMultiplier);
                        found = true;
                    }

                    previousRollPercent = currentRollPercent;
                }
            }
        }
    }

    private void ApplyRollRewards(Roll roll, double multiplier, double coinMultiplier = 1, double premiumGoodsMultiplier = 1, double goodsMultiplier = 1)
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
                        var goodsAmount = (int)Math.Ceiling(element.Amount / roll.Divisor * multiplier * goodsMultiplier);
                        if (goodsAmount <= 0) break;
                        AddGoods(goodsAmount);
                        StaticLogger.Current.LogDebug("Found goods {GoodsAmount}", goodsAmount);
                        break;
                    case "premium_goods":
                        var premiumGoodsAmount = (int)Math.Ceiling(element.Amount / roll.Divisor * multiplier * premiumGoodsMultiplier);
                        if (premiumGoodsAmount <= 0) break;
                        AddPremiumGoods(premiumGoodsAmount);
                        StaticLogger.Current.LogDebug("Found premium goods {PremiumGoodsAmount}", premiumGoodsAmount);
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
                    // Player::awardNotSpawnableDoober
                    case "not_spawnable" when element.Type == "coupon":
                        GiveCoupon(element.Name);
                        StaticLogger.Current.LogDebug("Found coupon {CouponName}", element.Name);
                        break;
                    // display only on client side (Doober::DOOBER_POPULATION)
                    case "population":
                        break;
                    default:
                        StaticLogger.Current.LogWarning("Unhandled reward type {RewardType}", rewardType);
                        break;
                }
            }
        }
    }
}
