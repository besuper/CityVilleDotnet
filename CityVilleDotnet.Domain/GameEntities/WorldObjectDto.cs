using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Settings.GameSettings;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Entities;
using System.Text.Json.Serialization;
using CityVilleDotnet.Domain.EnumExtensions;
using CityVilleDotnet.Domain.Enums;

namespace CityVilleDotnet.Domain.GameEntities;

public class WorldObjectDto
{
    [JsonPropertyName("itemName")] public required string ItemName { get; set; }

    [JsonPropertyName("className")] public required string ClassName { get; set; }

    [JsonPropertyName("contractName")] public string? ContractName { get; set; }

    [JsonPropertyName("deleted")] public bool Deleted { get; set; }

    [JsonPropertyName("tempId")] public int TempId { get; set; }

    [JsonPropertyName("buildTime")] public double? BuildTime { get; set; }

    [JsonPropertyName("plantTime")] public double? PlantTime { get; set; }

    [JsonPropertyName("witherOn")] public bool WitherOn { get; set; }

    [JsonPropertyName("state")] public required string State { get; set; }

    [JsonPropertyName("direction")] public int Direction { get; set; }

    [JsonPropertyName("position")] public required WorldObjectPositionDto Position { get; set; }

    [JsonPropertyName("id")] public int WorldFlatId { get; set; }

    [JsonPropertyName("targetBuildingClass")]
    public string? TargetBuildingClass { get; set; }

    [JsonPropertyName("targetBuildingName")]
    public string? TargetBuildingName { get; set; }

    [JsonPropertyName("stage")] public int? Stage { get; set; }
    [JsonPropertyName("currentState")] public int? CurrentState { get; set; }

    [JsonPropertyName("finishedBuilds")] public int? FinishedBuilds { get; set; }

    [JsonPropertyName("builds")] public int? Builds { get; set; }
    [JsonPropertyName("visits")] public int? Visits { get; set; }
    [JsonPropertyName("harvestCounter")] public int HarvestCounter { get; set; }

    [JsonPropertyName("upgradeActionCount")]
    public int UpgradeActionCount { get; set; }

    [JsonPropertyName("upgradeActionCount2")]
    public int UpgradeActionCount2 { get; set; }

    [JsonPropertyName("upgradeItemName")] public string? UpgradeItemName { get; set; }

    [JsonPropertyName("neverOpened")] public bool NeverOpened { get; set; }

    [JsonPropertyName("isMoveLocked")] public bool IsMoveLocked { get; set; }

    [JsonPropertyName("endPosition")] public WorldObjectPositionDto? EndPosition { get; set; }

    [JsonPropertyName("mechanicData")] public Dictionary<string, object?> MechanicData { get; set; } = new Dictionary<string, object?>();

    // TODO: Implement Gates
    [JsonPropertyName("gates")] public List<GateDto> Gates { get; set; } = [];

    [JsonPropertyName("itemOwner")] public string? ItemOwner { get; set; }

    [JsonPropertyName("franchise_info")] public FranchiseInfoDto? FranchiseInfo { get; set; }
    [JsonPropertyName("builtFloorCount")] public int? BuiltFloorCount { get; set; }
}

public class FranchiseInfoDto
{
    [JsonPropertyName("star_rating")] public int StarRating { get; set; }
    [JsonPropertyName("commodity_left")] public int CommodityLeft { get; set; }
    [JsonPropertyName("franchise_name")] public string FranchiseName { get; set; } = string.Empty;
}

public static class WorldObjectDtoMapper
{
    public static WorldObjectDto ToDto(this WorldObject model, IReadOnlyCollection<WorldObject>? worldObjects = null, IReadOnlyCollection<string>? coupons = null)
    {
        if (model.HasGrown())
            model.SetReadyToHarvest();

        var effectiveItem = GameSettingsManager.Instance.GetItem(model.GetItemName());

        var dto = new WorldObjectDto()
        {
            ItemName = model.ItemName,
            ClassName = model.ClassName.ToString(),
            ContractName = model.ContractName,
            Deleted = model.Deleted,
            TempId = model.TempId,
            BuildTime = model.BuildTime,
            PlantTime = model.PlantTime,
            WitherOn = effectiveItem?.AllowWither ?? false, // TODO: Check if WitherOn is related to AllowWither
            CurrentState = model.CurrentState is null ? (int)ConstructionState.Idle : (int)model.CurrentState,
            Stage = model.Stage,
            State = model.State.ToDescriptionString(),
            Direction = model.Direction,
            Position = new WorldObjectPositionDto
            {
                X = model.X,
                Y = model.Y,
                Z = model.Z ?? 0
            },
            WorldFlatId = model.WorldFlatId,
            TargetBuildingClass = model.TargetBuildingClass?.ToString(),
            TargetBuildingName = model.TargetBuildingName,
            FinishedBuilds = model.FinishedBuilds,
            Builds = model.Builds,
            Visits = model.Visits,
            NeverOpened = model.NeverOpened,
            IsMoveLocked = model.IsMoveLocked,
            HarvestCounter = model.UpgradeActionCount ?? 0, // This is for Plot
            UpgradeActionCount = model.UpgradeActionCount ?? 0, // This is for Business
            UpgradeActionCount2 = model.UpgradeActionCount2 ?? 0, // This is for DockHouse and TrainStation
            UpgradeItemName = model.UpgradeItemName,
            ItemOwner = model.ItemOwner,
            FranchiseInfo = model.FranchiseLocation is not null
                ? new FranchiseInfoDto
                {
                    StarRating = model.FranchiseLocation.StarRating,
                    CommodityLeft = model.FranchiseLocation.CommodityLeft,
                    FranchiseName = model.FranchiseLocation.FranchiseName
                }
                : null,
            BuiltFloorCount = model.BuiltFloorCount,
            MechanicData = new Dictionary<string, object?>(),
            Gates = []
        };

        foreach (var counter in model.MechanicCounters)
        {
            dto.MechanicData[counter.MechanicType] = counter.Count;
        }

        if (model.ParentMacroObjectId is not null)
        {
            dto.MechanicData["macroObject"] = new MacroObjectDataDto
            {
                ParentMacroObjectId = model.ParentMacroObjectId,
                IsCandidate = model.IsMacroObjectCandidate ? "true" : null
            };
        }

        if (model.CustomName is not null)
        {
            dto.MechanicData["name"] = model.CustomName;
        }

        if (model.UniversityLogo is not null)
        {
            dto.MechanicData["universityLogo"] = model.UniversityLogo;
        }

        if (model.HarvestEndTime is not null)
        {
            dto.MechanicData["harvestEndTS"] = ServerUtils.ToUnixTimeSeconds(model.HarvestEndTime.Value);
        }

        if (model.InvestmentCount > 0 || model.InvestmentReturns > 0)
        {
            dto.MechanicData["investmentData"] = new InvestmentDataDto
            {
                Coin = model.InvestmentReturns,
                CoinNumInvestments = model.InvestmentCount
            };
        }

        if (model.RemodelItemName is not null)
        {
            dto.MechanicData["remodel"] = new RemodelDto
            {
                ItemName = model.RemodelItemName,
                Gate = new RemodelGateDto { Builds = model.RemodelBuilds ?? 0 },
                Complete = false
            };
        }

        if (model.ClassName == BuildingClassType.Bridge)
        {
            var item = GameSettingsManager.Instance.GetItem(model.ItemName);

            var rightPart = item?.BridgeParts?.Parts.FirstOrDefault(p => p.Type == "right");

            if (rightPart?.X != null && rightPart.Y != null)
            {
                dto.EndPosition = new WorldObjectPositionDto
                {
                    X = rightPart.X.Value,
                    Y = rightPart.Y.Value
                };
            }
        }

        if (model.ClassName == BuildingClassType.SocialBusiness || model.ClassName == BuildingClassType.Hotel)
        {
            var item = GameSettingsManager.Instance.GetItem(model.ItemName);

            dto.MechanicData["harvestState"] = model.State == WorldObjectState.Open || model.State == WorldObjectState.ClosedHarvestable
                ? new CustomerHarvestStateDto
                {
                    Customers = model.Visits ?? 0,
                    CustomersReq = item?.CustomerCapacity ?? 0
                }
                : null;
        }

        var gameItem = GameSettingsManager.Instance.GetItem(model.ItemName);

        if (gameItem?.HarvestLoopConfig is { } harvestLoopConfig)
        {
            // TimedHarvestableResource (wonder terminal) reads mechanicData.harvestLoop, appliedCoupon and peeps directly
            if (model.ClassName == BuildingClassType.TimedHarvestableResource)
            {
                if (model.HarvestLoopStartTime is not null)
                {
                    dto.MechanicData["harvestLoop"] = new HarvestLoopDataDto { StartTs = ServerUtils.ToUnixTimeSeconds(model.HarvestLoopStartTime.Value) };
                }

                if (model.AppliedCoupon is not null)
                {
                    dto.MechanicData["appliedCoupon"] = model.AppliedCoupon;
                }

                // the VIPs waiting at the terminal are the plane coupons the player still holds
                var peeps = (coupons ?? []).Intersect(harvestLoopConfig.GetRequiredCoupons()).Select(x => new WonderPeepDto { Coupon = x }).ToList();

                if (peeps.Count > 0)
                {
                    dto.MechanicData["peeps"] = peeps;
                }
            }
            // Wonder::set harvestLoop
            else if (model.HarvestLoopStartTime is not null)
            {
                var openTs = ServerUtils.ToUnixTimeSeconds(model.HarvestLoopStartTime.Value);
                var closeTs = openTs + harvestLoopConfig.GetTimerDuration(model.AppliedCoupon);

                dto.MechanicData["harvestState"] = new HarvestLoopStateDto { OpenTs = openTs, CloseTs = closeTs };
                dto.MechanicData["sleepState"] = new SleepStateDto { SleepTs = closeTs, WakeTs = closeTs + harvestLoopConfig.SleepDuration };
            }
        }

        if (model.ContractName is not null && gameItem?.GetGameEventMechanic("contractPlant") is not null)
        {
            dto.MechanicData["contract"] = new ContractDataDto
            {
                ContractName = model.ContractName,
                PlantTime = (model.PlantTime ?? 0) / 1000
            };
        }
        
        if (gameItem?.IsTimedSupplyState() == true)
        {
            if (model.ClassName == BuildingClassType.TimedBusiness)
            {
                dto.State = WorldObjectState.Static.ToDescriptionString(); // timed businesses only have a static image
            }

            if (model.State == WorldObjectState.Open && model.PlantTime is not null)
            {
                var timestamp = (long)(model.PlantTime.Value / 1000);

                dto.MechanicData["harvestState"] = new TimedHarvestStateDto
                {
                    State = "supplied",
                    Timestamp = timestamp,
                    EndTimestamp = timestamp + (gameItem.HarvestLoopConfig?.TimerDuration ?? 0)
                };
            }
            else
            {
                dto.MechanicData["harvestState"] = new TimedHarvestStateDto
                {
                    State = "closed",
                    Timestamp = -1
                };
            }
        }
        else if (gameItem?.IsCustomerSupplyState() == true)
        {
            var customersReq = gameItem.CommodityRequired ?? 0;

            dto.MechanicData["harvestState"] = model.State == WorldObjectState.Open || model.State == WorldObjectState.ClosedHarvestable
                ? new CustomerHarvestStateDto
                {
                    Customers = Math.Min(model.Visits ?? 0, customersReq),
                    CustomersReq = customersReq
                }
                : null;
        }

        if (gameItem?.Mechanics is not null)
        {
            var loadGameMode = gameItem.Mechanics.GetMechanicByGameMode("load");

            foreach (var mechanic in loadGameMode?.Mechanics ?? [])
            {
                if (mechanic.Type == "streakData")
                {
                    dto.MechanicData["streakData"] = new StreakDataDto
                    {
                        ActivationTime = model.ActivationTime ?? -1,
                        InactiveTime = model.InactiveTime ?? -1,
                        StreakLength = model.StreakLength
                    };
                }

                if (mechanic.Type == "givenFreeItem")
                {
                    dto.MechanicData["givenFreeItem"] = model.GivenFreeItem;
                }
            }
        }

        if (model.ClassName == BuildingClassType.GreenHouse)
        {
            dto.MechanicData["greenHouseStorage"] = new GreenHouseStorageDto { Item = model.GetGreenHousePlot().ToDto() };

            dto.State = WorldObjectState.Static.ToDescriptionString(); // restore to static state
        }

        if (gameItem?.GetObjectStorageMechanic() is not null)
        {
            dto.MechanicData["slots"] = (worldObjects ?? [])
                .Where(x => x.StoredInId == model.WorldFlatId)
                .OrderBy(x => x.StorageSlotIndex)
                .Select(x => (object)x.ToDto(worldObjects))
                .ToList();
        }

        if (gameItem?.IsStorageSupply() == true)
        {
            if (model.State is WorldObjectState.Open or WorldObjectState.ClosedHarvestable)
            {
                dto.State = WorldObjectState.Static.ToDescriptionString(); // malls only have a static image
            }

            // StorageSupplyMechanic::generateHarvestState
            if (model.IsStorageSupplied())
            {
                var customersReq = model.GetSupplyCustomersReq();

                dto.MechanicData["harvestState"] = new CustomerHarvestStateDto
                {
                    Customers = Math.Min(model.Visits ?? 0, customersReq),
                    CustomersReq = customersReq,
                    Resources = model.GetSupplyResourceNames()
                };
            }
        }

        if (model.ClassName == BuildingClassType.ThemePark && gameItem?.ThemeParkStorage is not null)
        {
            dto.MechanicData["slots"] = Enumerable.Range(0, gameItem.ThemeParkStorage.Slots.Count)
                .Select(i => (object)(model.Slots.Any(x => x.SlotIndex == i) ? "filled" : "empty"))
                .ToList();

            if (model.State is WorldObjectState.Open or WorldObjectState.ClosedHarvestable)
            {
                dto.State = WorldObjectState.Static.ToDescriptionString(); // theme parks only have a static image
            }

            // ThemeParkSupplyMechanic::generateHarvestState
            if (model.IsStorageSupplied())
            {
                var customersReq = model.GetThemeParkCustomersReq();

                dto.MechanicData["harvestState"] = new CustomerHarvestStateDto
                {
                    Customers = Math.Min(model.Visits ?? 0, customersReq),
                    CustomersReq = customersReq,
                    FilledSlotCount = model.SupplyResources.Count
                };
            }
        }

        if (model.ClassName == BuildingClassType.ZooEnclosure)
        {
            var numSlots = gameItem?.Mechanics?.GetMechanicByGameMode("all")?.GetMechanicItemByType("slots")?.NumSlots ?? 3;
            var slots = new List<object?>();

            for (var i = 0; i < numSlots; i++)
            {
                slots.Add(model.Slots.FirstOrDefault(x => x.SlotIndex == i)?.ItemName);
            }

            dto.MechanicData["storage"] = model.StorageItems.ToDictionary(x => x.Name, x => x.Amount);
            dto.MechanicData["slots"] = slots;
            dto.MechanicData["giftSenders"] = new EmptyDto();
        }

        if (model.ClassName == BuildingClassType.ConstructionSite)
        {
            var gates = effectiveItem?.GetGates() ?? [];

            foreach (var buildGate in gates.Where(x => x.Name == "build"))
            {
                var keys = new Dictionary<string, int>();

                foreach (var key in buildGate.Keys.OfType<GateKey>())
                {
                    keys[key.Name] = key.Amount;
                }

                dto.Gates.Add(new GateDto
                {
                    Name = buildGate.Name,
                    Type = buildGate.Type,
                    Keys = keys
                });
            }
        }

        return dto;
    }
}