using System.Xml.Serialization;

namespace CityVilleDotnet.Common.Settings.GameSettings;

[Serializable]
public class ItemsContainer
{
    [XmlElement("item")] public required List<GameItem?> Items { get; set; }
}

[Serializable]
public class GameItem
{
    [XmlAttribute("name")] public required string Name { get; set; }
    [XmlAttribute("derivesFrom")] public string? DerivesFrom { get; set; }
    [XmlAttribute("doNotInherit")] public string? DoNotInherit { get; set; }
    [XmlAttribute("type")] public required string Type { get; set; }
    [XmlAttribute("behavior")] public string? Behavior { get; set; }
    [XmlAttribute("sellSendsToInventory")] public string? SellSendsToInventory { get; set; }
    [XmlAttribute("interactOnLock")] public string? InteractOnLockString { get; set; }
    [XmlIgnore] public bool InteractOnLock => InteractOnLockString == "true";
    [XmlAttribute("allowWither")] public string? AllowWitherString { get; set; }
    [XmlIgnore] public bool AllowWither => AllowWitherString != "false";
    [XmlIgnore] public int? Height { get; set; }

    [XmlAttribute("height")]
    public string? HeightString
    {
        get => Height?.ToString();
        set => Height = string.IsNullOrEmpty(value) ? null : int.Parse(value);
    }

    [XmlIgnore] public int? Width { get; set; }

    [XmlAttribute("width")]
    public string? WidthString
    {
        get => Width?.ToString();
        set => Width = string.IsNullOrEmpty(value) ? null : int.Parse(value);
    }

    [XmlElement("requiredLevel")]
    public string? RequiredLevelString
    {
        get => RequiredLevel?.ToString();
        set => RequiredLevel = string.IsNullOrEmpty(value) ? 0 : int.Parse(value);
    }

    [XmlIgnore] public int? RequiredLevel { get; set; }

    [XmlElement("requiredPopulation")] public int? RequiredPopulation { get; set; }
    [XmlElement("headquarters")] public string? HeadquartersName { get; set; }

    [XmlElement("leftPart")] public string? BridgeLeftPart { get; set; }
    [XmlElement("centerPart")] public string? BridgeCenterPart { get; set; }
    [XmlElement("rightPart")] public string? BridgeRightPart { get; set; }

    [XmlElement("grantedExpansionType")] public string? GrantedExpansionType { get; set; }
    [XmlElement("grantedExpansionsOnPlace")] public string? GrantedExpansionsOnPlace { get; set; }
    [XmlElement("grantedExpansionsOnFinish")] public string? GrantedExpansionsOnFinish { get; set; }

    [XmlElement("population")] public PopulationItem? Population { get; set; }
    [XmlElement("appraisal")] public List<AppraisalDefinitionItem> Appraisals { get; set; } = [];
    [XmlElement("upgrade")] public UpgradeItem? Upgrade { get; set; }

    [XmlElement("cost")] public int? Cost { get; set; }
    [XmlElement("unlock")] public string? Unlock { get; set; }

    // Support empty tag <unlockCost/>
    [XmlElement("unlockCost")]
    public string? UnlockCostString
    {
        get => UnlockCost?.ToString();
        set => UnlockCost = string.IsNullOrEmpty(value) ? 0 : int.Parse(value);
    }

    [XmlIgnore] public int? UnlockCost { get; set; }

    [XmlElement("cash")]
    public string? CashString
    {
        get => Cash?.ToString();
        set => Cash = string.IsNullOrEmpty(value) ? 0 : int.Parse(value);
    }

    [XmlIgnore] public int? Cash { get; set; }

    [XmlElement("growTime")] public double? GrowTime { get; set; }

    [XmlElement("coinYield")] public int? CoinYield { get; set; }

    [XmlElement("cashYield")] public int? CashYield { get; set; }

    [XmlElement("xpYield")] public int? XpYield { get; set; }

    [XmlElement("goodsYield")] public int? GoodsYield { get; set; }

    [XmlElement("goodsReward")] public int? GoodsReward { get; set; }

    [XmlElement("goods")] public int? Goods { get; set; }

    [XmlElement("trainTripTime")] public int? TrainTripTime { get; set; }

    [XmlElement("trainSpeedUpCost")] public int? TrainSpeedUpCost { get; set; }

    [XmlElement("trainPayout")] public TieredValueReference? TrainPayout { get; set; }

    [XmlElement("trainTourist")] public TieredValueReference? TrainTourist { get; set; }

    [XmlElement("construction")] public string? Construction { get; set; }

    [XmlElement("commodityReq")] public int? CommodityRequired { get; set; }
    [XmlElement("customerCapacity")] public int? CustomerCapacity { get; set; }
    [XmlElement("numberOfStages")] public int? NumberOfStages { get; set; }
    [XmlElement("energyCostPerBuild")] public int? EnergyCostPerBuild { get; set; }
    [XmlElement("energyRewards")] public int? EnergyRewards { get; set; }
    [XmlElement("coinRewards")] public int? CoinRewards { get; set; }
    [XmlElement("harvestMultiplier")] public int? HarvestMultiplier { get; set; }
    [XmlElement("useHarvestMultForCost")] public bool UseHarvestMultForCost { get; set; }
    [XmlElement("commodity")] public required List<CommodityItem> Commodity { get; set; }
    [XmlElement("randomModifiers")] public List<RandomModifiers> RandomModifiersList { get; set; } = [];
    [XmlElement("randomModifierGroups")] public RandomModifierGroupsContainer? RandomModifierGroups { get; set; }
    [XmlElement("energyCost")] public EnergyCost? EnergyCost { get; set; }
    [XmlElement("mechanics")] public MechanicsContainer? Mechanics { get; set; }
    [XmlAttribute("mechanicPack")] public string? MechanicPack { get; set; }
    [XmlElement("gates")] public required GatesContainer? Gates { get; set; }
    [XmlElement("sizeX")] public int? SizeX { get; set; }
    [XmlElement("sizeY")] public int? SizeY { get; set; }
    [XmlElement("bridgeparts")] public BridgePartsContainer? BridgeParts { get; set; }
    [XmlElement("keyword")] public List<string> Keywords { get; set; } = [];
    [XmlElement("mastery")] public required List<MasteryItem> MasteryItems { get; set; }
    [XmlElement("storageUnit")] public StorageUnitItem? StorageUnit { get; set; }
    [XmlElement("inventoryChecks")] public InventoryChecksContainer? InventoryChecks { get; set; }
    [XmlElement("inventoryLimit")] public int InventoryLimit { get; set; }
    [XmlElement("positiveStreak")] public string? PositiveStreak { get; set; }
    [XmlElement("negativeStreak")] public string? NegativeStreak { get; set; }
    [XmlElement("positiveStreakMaxEffect")] public int PositiveStreakMaxEffect { get; set; }
    [XmlElement("startingStreakValue")] public int StartingStreakValue { get; set; }
    [XmlElement("remodelXp")] public int? RemodelXp { get; set; }
    [XmlElement("remodels")] public RemodelsContainer? Remodels { get; set; }
    [XmlElement("rarity")] public string? Rarity { get; set; }
    [XmlElement("unlocksItem")] public string? UnlocksItem { get; set; }
    [XmlElement("workers")] public WorkersDefinitionItem? Workers { get; set; }
    [XmlElement("harvestBonuses")] public HarvestBonusesContainer? HarvestBonuses { get; set; }
    [XmlElement("harvestLoopConfig")] public HarvestLoopConfigItem? HarvestLoopConfig { get; set; }
    [XmlIgnore] public int? NumCrop { get; set; }

    [XmlElement("numCrop")]
    public string? NumCropString
    {
        get => NumCrop?.ToString();
        set => NumCrop = string.IsNullOrEmpty(value) ? null : int.Parse(value);
    }

    public bool HasKeyword(string keyword)
    {
        return Keywords.Contains(keyword);
    }

    public int[] GetPositiveStreakRewards()
    {
        return ParseStreakRewards(PositiveStreak);
    }

    public int[] GetNegativeStreakRewards()
    {
        return ParseStreakRewards(NegativeStreak);
    }

    public int GetStreakMaxEffect()
    {
        return StartingStreakValue > 0 ? StartingStreakValue : PositiveStreakMaxEffect;
    }

    private static int[] ParseStreakRewards(string? rewards)
    {
        if (string.IsNullOrEmpty(rewards)) return [];

        return rewards.Split(',').Select(int.Parse).ToArray();
    }

    public List<GatesItem> GetGates()
    {
        return Gates?.Gates ?? [];
    }

    // Composite gates only reference their sub gates by name (CompositeGate::takeKeys)
    public List<GateKey> GetInventoryGateKeys(string gateName)
    {
        var gate = GetGates().FirstOrDefault(x => x.Name == gateName);

        if (gate is null) return [];

        var keys = gate.Keys.Where(x => x is not null).Select(x => x!);

        if (gate.Type == "composite")
            return keys.SelectMany(x => GetInventoryGateKeys(x.Name)).ToList();

        // if null => inventory by default
        if (!string.IsNullOrEmpty(gate.Type) && gate.Type is not ("inventory" or "upgrade_inventory")) return [];

        return keys.ToList();
    }

    public RemodelDefinitionItem? GetRemodelDefinitionByName(string itemName)
    {
        return Remodels?.Definitions.FirstOrDefault(x => x.Item == itemName);
    }

    public bool HasMasteries()
    {
        return MasteryItems.Count > 0;
    }

    public string? GetExplodeToRect()
    {
        if (Mechanics?.GameEventMechanics is null) return null;

        foreach (var gem in Mechanics.GameEventMechanics)
        {
            if (gem.Mechanics is null) continue;

            foreach (var m in gem.Mechanics)
            {
                if (m.ClassName == "ExplodableMacroObjectMechanic" && m.ExplodeToRect is not null)
                    return m.ExplodeToRect;
            }
        }

        return null;
    }

    public MechanicItem? GetGameEventMechanic(string type)
    {
        if (Mechanics?.GameEventMechanics is null) return null;

        foreach (var gem in Mechanics.GameEventMechanics)
        {
            var mechanic = gem.GetMechanicItemByType(type);

            if (mechanic is not null) return mechanic;
        }

        return null;
    }

    // From GlobalTableModifierMechanic, the "load" game mode is the state rebuilt every time the world is loaded
    public List<MechanicItem> GetGlobalTableModifiers()
    {
        if (Mechanics?.GameEventMechanics is null) return [];

        return Mechanics.GameEventMechanics
            .Where(x => x.GameMode == "load")
            .SelectMany(x => x.Mechanics ?? [])
            .Where(x => x.Type == "globalTableModifiers" && x.Action == "add" && x.TargetKeyword is not null && x.Table is not null)
            .ToList();
    }

    public string? GetSupplyStateMechanicClass()
    {
        return GetGameEventMechanic("supplyState")?.ClassName;
    }

    public bool IsCustomerSupplyState()
    {
        return GetSupplyStateMechanicClass() == "CustomerSupplyMechanic";
    }

    public bool IsTimedSupplyState()
    {
        return GetSupplyStateMechanicClass() == "SupplyMechanic";
    }

    public GameItem GetFirstDeriveItem(GameItem item)
    {
        if (item.DerivesFrom is not null)
        {
            var parentItem = GameSettingsManager.Instance.GetItem(item.DerivesFrom);

            if (parentItem is null) return item;

            return GetFirstDeriveItem(parentItem);
        }

        return item;
    }

    public string? GetDefaultCommodityName()
    {
        if (Commodity.Count > 1)
            return Commodity.FirstOrDefault(x => x.Default > 0)?.Name;

        return Commodity.FirstOrDefault()?.Name;
    }

    public int? GetPopulationAddQuantity()
    {
        var check = InventoryChecks?.Checks.FirstOrDefault(x => x.Params?.IsPopulationItem == true && x.Type == "add");

        return check?.Params?.Quantity;
    }

    public AppraisalDefinitionItem? GetAppraisal(string appraisalId)
    {
        return Appraisals.FirstOrDefault(x => x.Id == appraisalId);
    }

    public int GetMaxWorkers()
    {
        return Workers?.GetMaxWorkers() ?? 0;
    }

    public HarvestBonusItem? GetWorkerHarvestBonus()
    {
        return HarvestBonuses?.Bonuses.FirstOrDefault(x => x.Name == "workerBonus");
    }

    public bool CanYieldPopulationItems()
    {
        foreach (var randomModifiers in RandomModifiersList)
        {
            foreach (var modifier in randomModifiers.Modifiers ?? [])
            {
                var table = GameSettingsManager.Instance.GetRandomModifier(modifier.TableName);

                if (table is null) continue;

                foreach (var roll in table.Rolls)
                {
                    if (!roll.Rewards.TryGetValue("item", out var elements)) continue;

                    if (elements.Any(x => GameSettingsManager.Instance.GetItem(x.Name)?.GetPopulationAddQuantity() is not null))
                        return true;
                }
            }
        }

        return false;
    }
}

[Serializable]
public class InventoryChecksContainer
{
    [XmlElement("inventoryCheck")] public required List<InventoryCheckItem> Checks { get; set; }
}

[Serializable]
public class InventoryCheckItem
{
    [XmlAttribute("type")] public required string Type { get; set; }
    [XmlAttribute("callback")] public string? Callback { get; set; }
    [XmlElement("params")] public InventoryCheckParams? Params { get; set; }
}

[Serializable]
public class InventoryCheckParams
{
    [XmlIgnore] public int? Quantity { get; set; }

    [XmlAttribute("quantity")]
    public string? QuantityString
    {
        get => Quantity?.ToString();
        set => Quantity = string.IsNullOrEmpty(value) ? null : int.Parse(value);
    }

    [XmlAttribute("isPopulationItem")] public string? IsPopulationItemString { get; set; }
    [XmlIgnore] public bool IsPopulationItem => IsPopulationItemString == "true";
}

[Serializable]
public class BridgePartsContainer
{
    [XmlElement("part")] public required List<BridgePartItem> Parts { get; set; }
}

[Serializable]
public class BridgePartItem
{
    [XmlAttribute("type")] public required string Type { get; set; }
    [XmlIgnore] public int? X { get; set; }

    [XmlAttribute("x")]
    public string? XString
    {
        get => X?.ToString();
        set => X = string.IsNullOrEmpty(value) ? null : int.Parse(value);
    }

    [XmlIgnore] public int? Y { get; set; }

    [XmlAttribute("y")]
    public string? YString
    {
        get => Y?.ToString();
        set => Y = string.IsNullOrEmpty(value) ? null : int.Parse(value);
    }
}

[Serializable]
public class EnergyCost
{
    [XmlAttribute("build")] public string? Build { get; set; }
    [XmlAttribute("instantFinish")] public string? InstantFinish { get; set; }
    [XmlAttribute("harvest")] public string? Harvest { get; set; }
    [XmlAttribute("open")] public string? Open { get; set; }
    [XmlAttribute("clean")] public string? Clean { get; set; }
    [XmlAttribute("clear")] public string? Clear { get; set; }
}

[Serializable]
public class GatesContainer
{
    [XmlElement("gate")] public required List<GatesItem>? Gates { get; set; }
}

[Serializable]
public class RemodelsContainer
{
    [XmlElement("remodel")] public List<RemodelDefinitionItem> Definitions { get; set; } = [];
}

[Serializable]
public class RemodelDefinitionItem
{
    [XmlAttribute("item")] public required string Item { get; set; }
    [XmlAttribute("gateName")] public string? GateName { get; set; }
    [XmlAttribute("requirementsName")] public string? RequirementsName { get; set; }
    [XmlAttribute("overrideCost")] public int OverrideCost { get; set; }
    [XmlAttribute("overrideCash")] public int OverrideCash { get; set; }
    [XmlAttribute("isBaseModel")] public bool IsBaseModel { get; set; }
}

[Serializable]
public class GatesItem
{
    [XmlAttribute("name")] public string? Name { get; set; }
    [XmlAttribute("type")] public string? Type { get; set; }
    [XmlAttribute("instructions")] public string? Instructions { get; set; }
    [XmlElement("key")] public required List<GateKey?> Keys { get; set; }
}

[Serializable]
public class GateKey
{
    [XmlAttribute("name")] public required string Name { get; set; }
    [XmlAttribute("viral")] public string? Viral { get; set; }
    [XmlAttribute("amount")] public int Amount { get; set; }
    [XmlAttribute("keepOnComplete")] public bool KeepOnComplete { get; set; }
    [XmlAttribute("removeAllOnComplete")] public bool RemoveAllOnComplete { get; set; }

    [XmlAttribute("cashCost")]
    public string? CashCostString
    {
        get => CashCost?.ToString();
        set => CashCost = string.IsNullOrEmpty(value) ? null : int.Parse(value);
    }

    [XmlIgnore] public int? CashCost { get; set; }
    [XmlElement("member")] public List<MemberKey>? Members { get; set; }

    public int GetCrewCost()
    {
        return CashCost ?? GameSettingsManager.Instance.GetSettings().CrewMemberCashCost;
    }
}

[Serializable]
public class MemberKey
{
    [XmlAttribute("name")] public required string Name { get; set; }
    [XmlAttribute("amount")] public int Amount { get; set; }
}

[Serializable]
public class PopulationItem
{
    [XmlAttribute("min")]
    public string? MinString
    {
        get => Min?.ToString();
        set => Min = string.IsNullOrEmpty(value) ? 0 : int.Parse(value);
    }

    [XmlIgnore] public int? Min { get; set; }

    [XmlAttribute("max")]
    public string? MaxString
    {
        get => Max?.ToString();
        set => Max = string.IsNullOrEmpty(value) ? 0 : int.Parse(value);
    }

    [XmlIgnore] public int? Max { get; set; }

    [XmlAttribute("cap")]
    public string? CapString
    {
        get => Cap?.ToString();
        set => Cap = string.IsNullOrEmpty(value) ? 0 : int.Parse(value);
    }

    [XmlIgnore] public int? Cap { get; set; }
}

[Serializable]
public class AppraisalDefinitionItem
{
    [XmlAttribute("id")] public string Id { get; set; } = string.Empty;

    [XmlAttribute("min")]
    public string? MinString
    {
        get => Min?.ToString();
        set => Min = string.IsNullOrEmpty(value) ? 0 : int.Parse(value);
    }

    [XmlIgnore] public int? Min { get; set; }

    [XmlAttribute("max")]
    public string? MaxString
    {
        get => Max?.ToString();
        set => Max = string.IsNullOrEmpty(value) ? 0 : int.Parse(value);
    }

    [XmlIgnore] public int? Max { get; set; }

    [XmlAttribute("cap")]
    public string? CapString
    {
        get => Cap?.ToString();
        set => Cap = string.IsNullOrEmpty(value) ? 0 : int.Parse(value);
    }

    [XmlIgnore] public int? Cap { get; set; }

    // The client falls back on the other attribute when one is missing
    public int EffectiveMin => Min ?? Max ?? 0;
    public int EffectiveMax => Max ?? Min ?? 0;
}

[Serializable]
public class UpgradeItem
{
    [XmlAttribute("item")] public required string Name { get; set; }
    [XmlAttribute("cashcost")] public string? CashCost { get; set; }
    [XmlElement("requirements")] public UpgradeRequirementsContainer? Requirements { get; set; }
    [XmlElement("rewards")] public UpgradeRewardsContainer? Rewards { get; set; }
    [XmlElement("helpers")] public UpgradeHelpersContainer? Helpers { get; set; }

    public int GetRequiredLevel() => int.Parse(Requirements?.GetValue("level") ?? "0");
    public int GetRequiredUpgradeActions() => int.Parse(Requirements?.GetValue("upgrade_actions") ?? "0");
}

[Serializable]
public class UpgradeRequirementsContainer
{
    [XmlElement("requirement")] public List<UpgradeRequirement> Requirements { get; set; } = [];

    public string? GetValue(string type) => Requirements.FirstOrDefault(x => x.Type == type)?.Value;
}

[Serializable]
public class UpgradeRequirement
{
    [XmlAttribute("type")] public required string Type { get; set; }
    [XmlAttribute("value")] public required string Value { get; set; }
}

[Serializable]
public class UpgradeRewardsContainer
{
    [XmlElement("reward")] public List<UpgradeReward> Rewards { get; set; } = [];
}

[Serializable]
public class UpgradeReward
{
    [XmlAttribute("type")] public required string Type { get; set; }
    [XmlAttribute("value")] public required string Value { get; set; }

    [XmlIgnore]
    public int IntValue => int.Parse(Value);
}

[Serializable]
public class UpgradeHelpersContainer
{
    [XmlElement("helper")] public List<UpgradeHelper> Helpers { get; set; } = [];
}

[Serializable]
public class UpgradeHelper
{
    [XmlAttribute("type")] public required string Type { get; set; }
    [XmlAttribute("max")] public int Max { get; set; }
    [XmlAttribute("actionValue")] public int ActionValue { get; set; }
}

[Serializable]
public class MasteryItem
{
    [XmlIgnore] public int? Level { get; set; }

    [XmlAttribute("level")]
    public string? LevelString
    {
        get => Level?.ToString();
        set => Level = string.IsNullOrEmpty(value) ? null : int.Parse(value);
    }

    [XmlIgnore] public int? RequiredCount { get; set; }

    [XmlAttribute("req")]
    public string? Req
    {
        get => RequiredCount?.ToString();
        set => RequiredCount = string.IsNullOrEmpty(value) ? null : int.Parse(value);
    }
}

[Serializable]
public class MechanicsContainer
{
    [XmlElement("gameEventMechanics")] public List<GameEventMechanicsItem>? GameEventMechanics { get; set; }

    public GameEventMechanicsItem? GetMechanicByGameMode(string gameMode)
    {
        return GameEventMechanics?.FirstOrDefault(x => x.GameMode == gameMode);
    }
}

[Serializable]
public class MechanicPacksContainer
{
    [XmlElement("mechanicPack")] public List<MechanicPackItem> Packs { get; set; } = [];
}

[Serializable]
public class MechanicPackItem
{
    [XmlAttribute("name")] public string? Name { get; set; }
    [XmlElement("mechanics")] public MechanicsContainer? Mechanics { get; set; }
}

[Serializable]
public class GameEventMechanicsItem
{
    [XmlAttribute("gameMode")] public string? GameMode { get; set; }
    [XmlElement("mechanic")] public List<MechanicItem>? Mechanics { get; set; }

    public MechanicItem? GetMechanicItemByType(string type)
    {
        return Mechanics?.FirstOrDefault(x => x.Type == type);
    }
}

[Serializable]
public class MechanicItem
{
    [XmlAttribute("className")] public string? ClassName { get; set; }
    [XmlAttribute("explodeToRect")] public string? ExplodeToRect { get; set; }
    [XmlAttribute("macroPrefix")] public string? MacroPrefix { get; set; }
    [XmlAttribute("type")] public string? Type { get; set; }
    [XmlAttribute("priority")] public int Priority { get; set; }
    [XmlAttribute("consumableType")] public string? ConsumableType { get; set; }
    [XmlAttribute("consumableQuantity")] public int ConsumableQuantity { get; set; }
    [XmlAttribute("activeDuration")] public int ActiveDuration { get; set; }
    [XmlAttribute("inactiveDuration")] public int InactiveDuration { get; set; }
    [XmlAttribute("maxStreakLength")] public int MaxStreakLength { get; set; }
    [XmlAttribute("blockOthers")] public bool BlockOthers { get; set; }
    [XmlAttribute("pick")] public string? Pick { get; set; }
    [XmlAttribute("freeItem")] public string? FreeItem { get; set; }
    [XmlAttribute("itemNames")] public string? ItemNames { get; set; }
    [XmlAttribute("numSlots")] public int NumSlots { get; set; }
    [XmlAttribute("restrictByKeywords")] public string? RestrictByKeywords { get; set; }
    [XmlAttribute("xOffset")] public int XOffset { get; set; }
    [XmlAttribute("yOffset")] public int YOffset { get; set; }
    [XmlAttribute("action")] public string? Action { get; set; }
    [XmlAttribute("gateName")] public string? GateName { get; set; }
    [XmlAttribute("targetKeyword")] public string? TargetKeyword { get; set; }
    [XmlAttribute("table")] public string? Table { get; set; }
    [XmlAttribute("validate")] public string? Validate { get; set; }

    public bool AllowsItem(GameItem? item)
    {
        if (RestrictByKeywords is null) return true;
        if (item is null) return false;

        return RestrictByKeywords.Split(',').Any(item.HasKeyword);
    }
}

[Serializable]
public class StorageUnitItem
{
    [XmlAttribute("name")] public string? Name { get; set; }
    [XmlElement("storageType")] public string? StorageType { get; set; }
    [XmlElement("storageKey")] public string? StorageKey { get; set; }
    [XmlElement("initialCapacity")] public int InitialCapacity { get; set; }
    [XmlElement("maxCapacity")] public int MaxCapacity { get; set; }
}

[Serializable]
public class CommodityItem
{
    [XmlAttribute("name")] public required string Name { get; set; }
    [XmlAttribute("capacity")] public int Capacity { get; set; }
    [XmlAttribute("default")] public int Default { get; set; }
}

[Serializable]
public class WorkersDefinitionItem
{
    [XmlAttribute("cashCost")] public int CashCost { get; set; }
    [XmlAttribute("amount")] public int Amount { get; set; }
    [XmlElement("worker")] public List<WorkerSlotItem> Members { get; set; } = [];

    // client falls back to the amount attribute when no members are defined (WorkersDefinition::loadObject)
    public int GetMaxWorkers()
    {
        return Members.Count > 0 ? Members.Count : Amount;
    }
}

[Serializable]
public class WorkerSlotItem
{
}

[Serializable]
public class HarvestLoopConfigItem
{
    [XmlElement("timerDuration")] public int TimerDuration { get; set; }
}

[Serializable]
public class HarvestBonusesContainer
{
    [XmlElement("bonus")] public List<HarvestBonusItem> Bonuses { get; set; } = [];
}

[Serializable]
public class HarvestBonusItem
{
    [XmlAttribute("name")] public string? Name { get; set; }
    [XmlAttribute("field")] public string? Field { get; set; }
    [XmlAttribute("percentModifier")] public double PercentModifier { get; set; }
}

[Serializable]
public class WorldRectsContainer
{
    [XmlElement("worldRect")] public required List<WorldRectItem> WorldRects { get; set; }
}

[Serializable]
public class WorldRectItem
{
    [XmlAttribute("name")] public required string Name { get; set; }
    [XmlElement("width")] public int Width { get; set; }
    [XmlElement("height")] public int Height { get; set; }
    [XmlElement("objects")] public required WorldRectObjectsContainer Objects { get; set; }
}

[Serializable]
public class WorldRectObjectsContainer
{
    [XmlElement("object")] public required List<WorldRectObject> Objects { get; set; }
}

[Serializable]
public class WorldRectObject
{
    [XmlAttribute("id")] public required string Id { get; set; }
    [XmlAttribute("itemName")] public required string ItemName { get; set; }
    [XmlAttribute("useConstructionSite")] public string? UseConstructionSite { get; set; }
    [XmlAttribute("direction")] public int Direction { get; set; }
    [XmlAttribute("x")] public int X { get; set; }
    [XmlAttribute("y")] public int Y { get; set; }
}