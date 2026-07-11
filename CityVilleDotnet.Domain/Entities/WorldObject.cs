using System.Buffers.Binary;
using System.Security.Cryptography;
using System.Text;
using CityVilleDotnet.Common.Global;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Settings.GameSettings;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.EnumExtensions;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Domain.GameEntities;
using Microsoft.Extensions.Logging;

namespace CityVilleDotnet.Domain.Entities;

public class WorldObject
{
    public WorldObject(string itemName, BuildingClassType className, string? contractName, bool deleted, int tempId, WorldObjectState state, int direction, double? buildTime, double? plantTime, int x, int y, int z, int worldFlatId)
    {
        ItemName = itemName;
        ClassName = className;
        ContractName = contractName;
        Deleted = deleted;
        TempId = tempId;
        State = state;
        Direction = direction;
        WorldFlatId = worldFlatId;
        BuildTime = buildTime;
        PlantTime = plantTime;
        X = x;
        Y = y;
        Z = z;
        NeverOpened = true;
    }

    public WorldObject()
    {
    }

    public int Id { get; set; }
    public string ItemName { get; set; }
    public BuildingClassType ClassName { get; set; }
    public string? ContractName { get; set; }

    /*[JsonPropertyName("components")]
    public object? Components { get; set; }*/
    public bool Deleted { get; private set; }
    public int TempId { get; private set; }
    public double? BuildTime { get; private set; }
    public double? PlantTime { get; private set; }
    public WorldObjectState State { get; private set; }
    public int Direction { get; private set; }
    public int X { get; private set; }
    public int Y { get; private set; }
    public int? Z { get; private set; }
    public int WorldFlatId { get; private set; }
    public BuildingClassType? TargetBuildingClass { get; private set; }
    public string? TargetBuildingName { get; private set; }
    public int? Stage { get; private set; }
    public int? FinishedBuilds { get; private set; }
    public int? Builds { get; private set; }
    public int? RequiredStages { get; private set; }
    public ConstructionState? CurrentState { get; private set; }
    public FranchiseLocation? FranchiseLocation { get; private set; }
    public string? ItemOwner { get; private set; }
    public int? Visits { get; private set; }
    public bool NeverOpened { get; private set; }
    public int? UpgradeActionCount { get; private set; }
    public int? BuiltFloorCount { get; private set; }
    public long? ActivationTime { get; private set; }
    public long? InactiveTime { get; private set; }
    public int StreakLength { get; private set; }
    public int EnergyModifier { get; private set; }
    public bool GivenFreeItem { get; private set; }
    public string? RemodelItemName { get; private set; }
    public int? RemodelBuilds { get; private set; }
    public List<CrewMember> CrewMembers { get; private set; } = [];
    public List<WorldObjectMechanicCounter> MechanicCounters { get; private set; } = [];
    public List<WorldObjectStorageItem> StorageItems { get; private set; } = [];
    public List<WorldObjectSlot> Slots { get; private set; } = [];

    public void IncrementMechanicCounter(string mechanicType)
    {
        var counter = MechanicCounters.FirstOrDefault(x => x.MechanicType == mechanicType);

        if (counter is null)
        {
            counter = new WorldObjectMechanicCounter(mechanicType);
            MechanicCounters.Add(counter);
        }

        counter.Increment();
    }

    public int GetBonusPopulation()
    {
        return MechanicCounters.FirstOrDefault(x => x.MechanicType == "population_value_increase")?.Count ?? 0;
    }

    public int GetBonusAppraisal()
    {
        return MechanicCounters.FirstOrDefault(x => x.MechanicType == "appraisal_value_increase")?.Count ?? 0;
    }

    public int AddBonusAppraisal(int amount, string appraisalId)
    {
        var appraisal = GameSettingsManager.Instance.GetItem(ItemName)?.GetAppraisal(appraisalId);

        if (appraisal is null) return 0;

        var maxIncrease = appraisal.EffectiveMax - appraisal.EffectiveMin;

        if (maxIncrease <= 0) return 0;

        var current = GetBonusAppraisal();
        var newValue = Math.Clamp(current + amount, 0, maxIncrease);

        if (newValue == current) return 0;

        var counter = MechanicCounters.FirstOrDefault(x => x.MechanicType == "appraisal_value_increase");

        if (counter is null)
        {
            counter = new WorldObjectMechanicCounter("appraisal_value_increase");
            MechanicCounters.Add(counter);
        }

        counter.Add(newValue - current);

        return newValue - current;
    }

    public int AddBonusPopulation(int amount)
    {
        var gameItem = GameSettingsManager.Instance.GetItem(ItemName);
        var population = gameItem?.Population ?? gameItem?.GetFirstDeriveItem(gameItem).Population;

        if (population?.Min is null || population.Max is null) return 0;

        var maxIncrease = population.Max.Value - population.Min.Value;

        if (maxIncrease <= 0) return 0;

        var current = GetBonusPopulation();
        var newValue = Math.Clamp(current + amount, 0, maxIncrease);

        if (newValue == current) return 0;

        var counter = MechanicCounters.FirstOrDefault(x => x.MechanicType == "population_value_increase");

        if (counter is null)
        {
            counter = new WorldObjectMechanicCounter("population_value_increase");
            MechanicCounters.Add(counter);
        }

        counter.Add(newValue - current);

        return newValue - current;
    }

    public void UpdateStreakData(int activeDuration, int inactiveDuration, int maxStreakLength, int[] negativeStreakRewards)
    {
        var now = ServerUtils.GetCurrentTimeSeconds();

        if (ActivationTime.HasValue)
        {
            if (now - ActivationTime.Value < activeDuration)
                return;

            InactiveTime = ActivationTime.Value + activeDuration;
            ActivationTime = null;
        }

        if (!InactiveTime.HasValue || inactiveDuration <= 0)
            return;
        
        var remaining = inactiveDuration - (now - InactiveTime.Value);

        while (remaining < 0)
        {
            ApplyNegativeStreak(maxStreakLength, negativeStreakRewards);
            remaining += inactiveDuration;
        }

        InactiveTime = now - (inactiveDuration - remaining);
    }

    private void ApplyNegativeStreak(int maxStreakLength, int[] negativeStreakRewards)
    {
        if (StreakLength > 0)
            StreakLength = 0;

        if (maxStreakLength <= 0 || StreakLength > -maxStreakLength)
            StreakLength--;

        if (negativeStreakRewards.Length == 0)
            return;

        var index = Math.Min(-StreakLength, negativeStreakRewards.Length) - 1;

        EnergyModifier = Math.Max(0, EnergyModifier - negativeStreakRewards[index]);
    }

    public void MarkFreeItemGiven()
    {
        GivenFreeItem = true;
    }

    public void AddToStorage(string itemName, int amount = 1)
    {
        var item = StorageItems.FirstOrDefault(x => x.Name == itemName);

        if (item is null)
            StorageItems.Add(new WorldObjectStorageItem(itemName, amount));
        else
            item.AddAmount(amount);
    }

    public void RemoveFromStorage(string itemName, int amount = 1)
    {
        var item = StorageItems.FirstOrDefault(x => x.Name == itemName);

        if (item is null || item.Amount < amount)
            throw new Exception($"Not enough {itemName} in storage of object {WorldFlatId}");

        item.RemoveAmount(amount);

        if (item.Amount <= 0)
            StorageItems.Remove(item);
    }

    public void FillSlot(int slotIndex, string itemName, int numSlots)
    {
        if (slotIndex < 0 || slotIndex >= numSlots)
            throw new Exception($"Invalid slot index {slotIndex} on object {WorldFlatId}");

        if (Slots.Any(x => x.SlotIndex == slotIndex))
            throw new Exception($"Slot {slotIndex} is already filled on object {WorldFlatId}");

        Slots.Add(new WorldObjectSlot(slotIndex, itemName));
    }

    public string EmptySlot(int slotIndex)
    {
        var slot = Slots.FirstOrDefault(x => x.SlotIndex == slotIndex);

        if (slot is null)
            throw new Exception($"Slot {slotIndex} is already empty on object {WorldFlatId}");

        Slots.Remove(slot);

        return slot.ItemName;
    }

    public int FillNextSlot(string itemName, int numSlots)
    {
        for (var i = 0; i < numSlots; i++)
        {
            if (Slots.Any(x => x.SlotIndex == i)) continue;

            Slots.Add(new WorldObjectSlot(i, itemName));

            return i;
        }

        throw new Exception($"No empty slot available on object {WorldFlatId}");
    }

    public string RollRandomZooAnimal()
    {
        var lootTable = GameSettingsManager.Instance.GetLootTable($"zoo_animals_{GetItemName()}")
            ?? throw new Exception($"No loot table defined for enclosure {GetItemName()}");

        var animalName = lootTable.RollItemName();

        AddToStorage(animalName);

        return animalName;
    }

    public void GrantInitialZooAnimal(int snuid)
    {
        var commons = GameSettingsManager.Instance.GetItemsByKeyword(GetItemName())
            .Where(x => x.HasKeyword("zoo_animal") && x.Rarity == "common")
            .ToList();

        if (commons.Count == 0) return;

        // client grants itself the animal locally without sending a transaction
        // (ZooEnclosure.addRandomAnimal): index = first 8 hex chars of MD5("{uid}::{itemName}") % count
        var hash = MD5.HashData(Encoding.UTF8.GetBytes($"{snuid}::{GetItemName()}"));
        var index = (int)(BinaryPrimitives.ReadUInt32BigEndian(hash) % (uint)commons.Count);

        AddToStorage(commons[index].Name);
    }

    public void Supply(int maxStreakLength, int[] positiveStreakRewards, int maxEffect)
    {
        ActivationTime = ServerUtils.GetCurrentTimeSeconds();
        InactiveTime = null;

        if (StreakLength < 0)
            StreakLength = 0;

        if (maxStreakLength <= 0 || StreakLength < maxStreakLength)
            StreakLength++;

        if (positiveStreakRewards.Length == 0)
            return;

        var index = Math.Min(StreakLength, positiveStreakRewards.Length) - 1;

        EnergyModifier += positiveStreakRewards[index];

        if (maxEffect > 0 && EnergyModifier > maxEffect)
            EnergyModifier = maxEffect;
    }

    public void SetAsConstructionSite(string itemName, int maxStages)
    {
        Stage = 0;
        FinishedBuilds = 0;
        Builds = 0;
        RequiredStages = maxStages;
        TargetBuildingName = ItemName;
        TargetBuildingClass = ClassName;
        CurrentState = ConstructionState.Idle;

        ItemName = itemName;
        ClassName = BuildingClassType.ConstructionSite;
    }

    public void AddConstructionStage()
    {
        Builds += 1;
        Stage += 1;
        FinishedBuilds = Builds;

        if (FinishedBuilds >= RequiredStages)
        {
            CurrentState = ConstructionState.AtGate;
        }
    }

    public List<WorldObject> FinishConstruction()
    {
        if (TargetBuildingName is null || TargetBuildingClass is null)
            throw new Exception("Can't finish build");

        ItemName = TargetBuildingName;
        ClassName = TargetBuildingClass.Value;

        Stage = null;
        FinishedBuilds = null;
        Builds = null;
        TargetBuildingName = null;
        TargetBuildingClass = null;
        CurrentState = null;
        RequiredStages = null;

        var newObjects = new List<WorldObject>();

        // We need to construct the all bridge parts by ourselves
        if (ItemName == "bridge_standard")
        {
            var item = GameSettingsManager.Instance.GetItem(ItemName);

            if (item?.BridgeParts is null || item.BridgeCenterPart is null || item.BridgeRightPart is null) return [];

            foreach (var part in item.BridgeParts.Parts)
            {
                if (part.X is null || part.Y is null) continue;

                var partItemName = part.Type switch
                {
                    "center" => item.BridgeCenterPart,
                    "right" => item.BridgeRightPart,
                    _ => "bridge_left"
                };

                var newObject = new WorldObject(partItemName, BuildingClassType.BridgePart, null, false, 0, WorldObjectState.Open, 0, null, null, part.X.Value, part.Y.Value, 0, 1);

                newObject.State = WorldObjectState.Static;
                newObjects.Add(newObject);
            }
        }

        if (ClassName == BuildingClassType.Headquarter)
        {
            BuiltFloorCount = 1;
        }

        return newObjects;
    }

    public bool CanHarvest()
    {
        if (GetClassName().IsBusiness())
            return State == WorldObjectState.ClosedHarvestable;

        return HasGrown();
    }

    public bool HasGrown()
    {
        if (State == WorldObjectState.Grown) return true;
        if (State != WorldObjectState.Planted || PlantTime is null) return false;

        var currentTime = ServerUtils.GetCurrentTime();
        var timeElapsed = currentTime - PlantTime.Value;

        var item = GameSettingsManager.Instance.GetItem(GetItemName());
        var growTime = item?.GetGrowTime();

        if (growTime is null) return false;

        var settings = GameSettingsManager.Instance.GetSettings();
        var inGameDaySeconds = settings.InGameDaySeconds;
        var growMultiplier = settings.GrowMultiplier;
        var growTimeMs = growTime * (inGameDaySeconds * 1000.0) * growMultiplier;

        return timeElapsed >= growTimeMs;
    }

    public void SetReadyToHarvest()
    {
        State = WorldObjectState.Grown;
    }

    public (int CoinYield, int CashYield) Harvest()
    {
        var coinYield = 0;
        var cashYield = 0;

        if (ContractName is not null)
        {
            var gameItem = GameSettingsManager.Instance.GetItem(ContractName);

            if (gameItem is not null)
            {
                coinYield = gameItem.CoinYield ?? 0;
                cashYield = gameItem.CashYield ?? 0;
            }

            State = WorldObjectState.Plowed;
            UpgradeActionCount = (UpgradeActionCount ?? 0) + 1;
            ContractName = null;
        }
        else
        {
            var gameItem = GameSettingsManager.Instance.GetItem(ItemName);

            if (gameItem is not null)
            {
                coinYield = gameItem.CoinYield ?? 0;
                cashYield = gameItem.CashYield ?? 0;
            }
        }

        // This is harvesting Residence
        if (HasGrown())
        {
            State = WorldObjectState.Planted;
            PlantTime = ServerUtils.GetCurrentTime();
        }

        if (ClassName.IsBusiness())
        {
            State = WorldObjectState.Closed;
            Visits = 0;
        }

        return (coinYield, cashYield);
    }

    public void HarvestGreenHouse()
    {
        ContractName = null;
        State = WorldObjectState.Plowed;
    }

    public void OpenBusiness()
    {
        if (!ClassName.IsBusiness()) throw new Exception("Can't open other than business building, class name is: " + ClassName + "");
        if (State == WorldObjectState.Open || State == WorldObjectState.ClosedHarvestable) throw new Exception("Building is already open");

        Visits = 0;
        PlantTime = ServerUtils.GetCurrentTime();
        State = WorldObjectState.Open;
        NeverOpened = false;
        UpgradeActionCount = (UpgradeActionCount ?? 0) + 1;

        if (FranchiseLocation is not null)
        {
            FranchiseLocation.TimeLastOperated = ServerUtils.GetCurrentTimeSeconds();
        }
    }

    public void SetFranchiseLocation(FranchiseLocation franchiseLocation, string itemOwner)
    {
        FranchiseLocation = franchiseLocation;
        ItemOwner = itemOwner;
    }

    public void UpdateVisits(int visits)
    {
        if (!ClassName.IsBusiness()) throw new Exception($"Can't update visits on non business building {Id} {ClassName} {State}");
        if (State != WorldObjectState.Open)
        {
            if (State == WorldObjectState.ClosedHarvestable) return; // If client get out of sync and try to send processVisit while visits are completed, just ignore

            throw new Exception($"Can't update visits on non open business building {Id} {ClassName} {State}");
        }

        var gameItem = GameSettingsManager.Instance.GetItem(ItemName);

        if (gameItem is null)
            throw new Exception("Can't find game item for business building");

        var maxVisits = gameItem.GetCommodityRequired();

        if (maxVisits is null)
            throw new Exception("Can't find max visits for business building");

        Visits += visits;

        if (Visits >= maxVisits)
        {
            State = WorldObjectState.ClosedHarvestable;
        }
    }

    public void UpgradeBuilding(GameItem item, string newItemName)
    {
        ItemName = newItemName;

        if (ClassName == BuildingClassType.Municipal && item.Behavior == "upgradable")
        {
            State = WorldObjectState.Grown;
        }

        if (ClassName == BuildingClassType.Business)
        {
            State = WorldObjectState.ClosedHarvestable;

            var gameItem = GameSettingsManager.Instance.GetItem(ItemName);

            if (gameItem is null)
                throw new Exception("Can't find game item for business building");

            var maxVisits = gameItem.GetCommodityRequired();

            if (maxVisits is null)
                throw new Exception("Can't find max visits for business building");

            Visits = maxVisits;
        }
    }

    public void StartRemodel(string skinItemName)
    {
        RemodelItemName = skinItemName;
        RemodelBuilds = 0;
    }

    public bool IsRemodeling()
    {
        return RemodelItemName is not null;
    }

    public bool AddRemodelBuild()
    {
        if (RemodelItemName is null) throw new Exception("Building is not being remodeled");

        RemodelBuilds = (RemodelBuilds ?? 0) + 1;

        return RemodelBuilds >= GetRemodelRequiredBuilds();
    }

    public int GetRemodelRequiredBuilds()
    {
        if (RemodelItemName is null) throw new Exception("Building is not being remodeled");

        var gameItem = GameSettingsManager.Instance.GetItem(ItemName);
        var baseItem = gameItem?.GetFirstDeriveItem(gameItem);
        var definition = baseItem?.GetRemodelDefinitionByName(RemodelItemName);
        var gate = baseItem?.GetGates().FirstOrDefault(x => x.Name == definition?.GateName);
        var amount = gate?.Keys.FirstOrDefault(x => x?.Name == "builds")?.Amount;

        if (amount is null) throw new Exception($"Can't find remodel gate for {RemodelItemName}");

        return amount.Value;
    }

    public int FinishRemodel()
    {
        if (RemodelItemName is null) throw new Exception("Building is not being remodeled");

        var skinItem = GameSettingsManager.Instance.GetItem(RemodelItemName);

        ItemName = RemodelItemName;
        RemodelItemName = null;
        RemodelBuilds = null;

        return skinItem?.RemodelXp ?? 0;
    }

    public void MoveTo(int x, int y, int z, int direction)
    {
        // TODO: Check if position is free
        X = x;
        Y = y;
        Z = z;
        Direction = direction;
    }

    public void StartContract(string contractName, WorldObjectState state)
    {
        ContractName = contractName;
        PlantTime = ServerUtils.GetCurrentTime();
        State = state;

        // Why don't ships work like plots?
        // plowed means ready
        // planted, planted
        // grown, grown
        // but receive plowed in start contract transaction
        if (ClassName == BuildingClassType.HarvestableShip)
        {
            State = WorldObjectState.Planted;
        }
    }

    public void UpdateWorldFlatId(int id)
    {
        if (WorldFlatId != 1) throw new Exception($"Can't modify WorldFlatId {WorldFlatId} to {id}");

        WorldFlatId = id;
    }

    public WorldObject LoadObject(WorldObjectDto worldObjectDto)
    {
        Builds = worldObjectDto.Builds;
        BuildTime = worldObjectDto.BuildTime;
        ClassName = Enum.Parse<BuildingClassType>(worldObjectDto.ClassName);
        ContractName = worldObjectDto.ContractName;
        Deleted = worldObjectDto.Deleted;
        Direction = worldObjectDto.Direction;
        FinishedBuilds = worldObjectDto.FinishedBuilds;
        ItemName = worldObjectDto.ItemName;
        PlantTime = worldObjectDto.PlantTime;
        X = worldObjectDto.Position.X;
        Y = worldObjectDto.Position.Y;
        Z = worldObjectDto.Position.Z;
        Stage = worldObjectDto.Stage;
        State = EnumExtensions.EnumExtensions.ParseFromDescription<WorldObjectState>(worldObjectDto.State);
        TargetBuildingClass = worldObjectDto.TargetBuildingClass is null ? null : Enum.Parse<BuildingClassType>(worldObjectDto.TargetBuildingClass);
        TargetBuildingName = worldObjectDto.TargetBuildingName;
        TempId = -1;
        WorldFlatId = worldObjectDto.WorldFlatId;

        return this;
    }

    public void SetTempId(int id)
    {
        if (TempId != -1) throw new Exception("Can't define TempId twice");

        TempId = id;
    }

    public void CleanTempId()
    {
        TempId = -1;
    }

    public void Close()
    {
        if (!ClassName.IsBusiness()) return;

        State = WorldObjectState.Closed;
    }

    public string GetItemName()
    {
        if (TargetBuildingName is not null)
            return TargetBuildingName;

        if (ContractName is not null)
            return ContractName;

        return ItemName;
    }

    public BuildingClassType GetClassName()
    {
        var className = ClassName;

        if (ClassName == BuildingClassType.ConstructionSite)
        {
            if (TargetBuildingClass is null)
                throw new Exception("TargetBuildingClass can't be null with ConstructionSite");

            className = TargetBuildingClass.Value;
        }

        return className;
    }

    public void UpgradeHeadquarterFloor()
    {
        if (BuiltFloorCount is null) throw new Exception("Floor count can't be null for Headquarters");

        BuiltFloorCount++;
    }

    public void BoostPlot()
    {
        if (GetClassName() != BuildingClassType.Plot)
            throw new Exception($"Can't water {ClassName}");

        var item = GameSettingsManager.Instance.GetItem(GetItemName());
        var growTime = item?.GetGrowTime();

        if (growTime is null) throw new Exception("Building can't be watered without growTime");

        var settings = GameSettingsManager.Instance.GetSettings();
        var inGameDaySeconds = settings.InGameDaySeconds;
        var growMultiplier = settings.GrowMultiplier;
        var boostGrowMultiplier = settings.BoostGrowMultiplier;
        //var boostGrowInstantHourLimit = settings.BoostGrowInstantHourLimit; // TODO

        var visitBoost = growTime * (inGameDaySeconds * 1000) * growMultiplier * boostGrowMultiplier;

        PlantTime -= visitBoost;

        if (HasGrown()) SetReadyToHarvest();
    }

    public void SetDirection(int direction)
    {
        Direction = direction;
    }

    public WorldObject Clone(int x, int y, int z, int id)
    {
        return new WorldObject(ItemName, ClassName, ContractName, Deleted, TempId, State, Direction, ServerUtils.GetCurrentTime(), ServerUtils.GetCurrentTime(), x, y, z, id);
    }

    private double PlotCostToMakeReady()
    {
        if (State != WorldObjectState.Planted || PlantTime is null) return 0;

        var settings = GameSettingsManager.Instance.GetSettings();
        var currentTime = ServerUtils.GetCurrentTime();
        var timeUntilReady = (GameSettingsManager.Instance.GetItem(GetItemName())?.GetGrowTime() ?? 0) * 1000.0;
        var growTimeMs = timeUntilReady * settings.InGameDaySeconds * settings.GrowMultiplier;

        var hoursLeft = Math.Max((growTimeMs - (currentTime - PlantTime.Value)) / 3600000.0, 0);
        var exponent = 0.4;
        var multiplier = settings.InstantReadyCropCostConstant3;

        return multiplier * Math.Pow(hoursLeft, exponent);
    }

    private double ResidenceCostToMakeReady()
    {
        if (State != WorldObjectState.Planted || PlantTime is null) return 0;

        var settings = GameSettingsManager.Instance.GetSettings();
        var currentTime = ServerUtils.GetCurrentTime();
        var timeUntilReady = (GameSettingsManager.Instance.GetItem(GetItemName())?.GetGrowTime() ?? 0) * 1000.0;
        var growTimeMs = timeUntilReady * settings.InGameDaySeconds * settings.GrowMultiplier;

        var hoursLeft = Math.Max((growTimeMs - (currentTime - PlantTime.Value)) / 3600000.0, 0);
        var exponent = 0.25;
        var multiplier = settings.InstantReadyResidenceCostConstant5;

        return multiplier * Math.Pow(hoursLeft, exponent);
    }

    public int GetCostToMakeReady()
    {
        return ClassName switch
        {
            BuildingClassType.Plot => (int)Math.Ceiling(PlotCostToMakeReady()),
            BuildingClassType.Ship => (int)Math.Ceiling(PlotCostToMakeReady()),
            BuildingClassType.HarvestableShip => (int)Math.Ceiling(HarvestableShipCostToMakeReady()),
            BuildingClassType.Residence => (int)Math.Ceiling(ResidenceCostToMakeReady()),
            _ => throw new NotImplementedException()
        };
    }

    private double HarvestableShipCostToMakeReady()
    {
        var baseCost = PlotCostToMakeReady();
        if (baseCost <= 0) return 0;

        var gameItem = GameSettingsManager.Instance.GetItem(GetItemName());
        var harvestMultiplier = gameItem?.HarvestMultiplier ?? 0;
        var useHarvestMultForCost = gameItem?.UseHarvestMultForCost ?? false;

        if (!useHarvestMultForCost || harvestMultiplier <= 0) return baseCost;

        return Math.Max(Math.Ceiling(baseCost), 1) * (1 + harvestMultiplier / 100.0);
    }

    public string GetDeepItemName()
    {
        var defaultItemName = GetItemName();
        var gameItem = GameSettingsManager.Instance.GetItem(defaultItemName);

        if (gameItem?.DerivesFrom is null) return defaultItemName;

        var derivedItem = gameItem.GetFirstDeriveItem(gameItem);

        return derivedItem.Name;
    }

    public WorldObject GetGreenHousePlot()
    {
        return new WorldObject("plot_crop", BuildingClassType.Plot, ContractName, Deleted, TempId, State, Direction, BuildTime, PlantTime, X, Y, Z ?? 0, 0);
    }

    public void AddCrewMember(Player? crew)
    {
        CrewMembers.Add(new CrewMember(crew));
    }

    public void SetUpgradeAction(int amount)
    {
        UpgradeActionCount = amount;
    }
}