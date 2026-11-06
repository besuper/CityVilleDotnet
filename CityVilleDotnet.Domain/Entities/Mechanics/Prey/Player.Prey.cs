using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Common.Global;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Settings.GameSettings;
using CityVilleDotnet.Domain.EnumExtensions;
using Microsoft.Extensions.Logging;

namespace CityVilleDotnet.Domain.Entities;

public partial class Player
{
    public PreyGroup GetOrCreatePreyGroup(string groupId)
    {
        var group = PreyGroups.FirstOrDefault(x => x.GroupId == groupId);

        if (group is not null)
            return group;

        group = new PreyGroup(groupId);
        PreyGroups.Add(group);

        return group;
    }

    public int GetNumPreysCaptured(string groupId)
    {
        return PreyGroups.FirstOrDefault(x => x.GroupId == groupId)?.NumPreysCaptured ?? 0;
    }

    public void RefreshPreyGroups(World world, long now)
    {
        foreach (var setting in GameSettingsManager.Instance.GetPreyGroups())
        {
            var hubLevel = world.GetHubLevel(setting);

            if (hubLevel == 0)
                continue;

            var group = GetOrCreatePreyGroup(setting.Id);
            group.SyncHunters(setting, hubLevel, now);
            group.RefillQueue(setting, hubLevel, now);
        }
    }

    public void RollHarvestStatePreySpawns(World world, WorldObject obj, GameItem item, IReadOnlyDictionary<string, int> experiments)
    {
        switch (item.GetGameEventMechanic("harvestState")?.ClassName)
        {
            case "MallHarvestMechanic" or "CustomEnergyCostMallHarvestMechanic":
                return;
            case "NeighborhoodHarvestMechanic":
                RollPreySpawns(world, item, experiments);
                RollPreySpawns(world, item, experiments);
                return;
            case "SuperNeighborhoodHarvestMechanic":
                RollPreySpawns(world, item, experiments);

                foreach (var neighborhood in world.GetStoredObjects(obj))
                {
                    RollPreySpawns(world, GameSettingsManager.Instance.GetItem(neighborhood.ItemName) ?? throw new Exception($"Can't find game item for {neighborhood.ItemName}"), experiments);
                }

                RollPreySpawns(world, item, experiments);
                return;
            default:
                RollPreySpawns(world, item, experiments);
                return;
        }
    }

    public void RollPreySpawns(World world, GameItem item, IReadOnlyDictionary<string, int> experiments)
    {
        var worldId = world.Type.ToDescriptionString();

        var sources = GameSettingsManager.Instance.GetPreyGroups()
            .SelectMany(group => group.SpawnsFrom.Select(source => (Group: group, Source: source)))
            .Where(x => x.Source.Name == item.Type || item.HasKeyword(x.Source.Name))
            .Where(x => string.IsNullOrEmpty(x.Source.World) || x.Source.World == worldId)
            .Where(x => string.IsNullOrEmpty(x.Source.ExperimentName) || IsInExperimentVariant(experiments, x.Source.ExperimentName, x.Source.ExperimentVariant ?? ""))
            .OrderBy(x => x.Group.Id, StringComparer.Ordinal);

        foreach (var (group, source) in sources)
        {
            var hubLevel = world.GetHubLevel(group);
            var mode = group.GetActiveMode(hubLevel);

            if (hubLevel < mode.LevelRequired || Level < mode.SpawnPreyPlayerLevelRequired)
                continue;

            if (mode.SpawnPreyExperimentName is not null && !IsInExperimentVariant(experiments, mode.SpawnPreyExperimentName, mode.SpawnPreyExperimentVariant ?? "1"))
                continue;

            if (mode.SpawnPreyWorldId is not null && mode.SpawnPreyWorldId != worldId)
                continue;

            // The client stops once a prey spawns, the server can't know which queued preys it already spawned this session
            if (RollFeature(group.Id, 0, 99) < source.SpawnChance && PreyGroups.Any(x => x.GroupId == group.Id && x.Preys.Count > 0))
                return;
        }
    }

    private static bool IsInExperimentVariant(IReadOnlyDictionary<string, int> experiments, string name, string variants)
    {
        var variant = experiments.GetValueOrDefault(name);

        return variants.Split(',').Any(x => int.TryParse(x, out var value) && value == variant);
    }

    public void BuyPreyResource(PreyGroupSetting setting, int position, long now)
    {
        RemoveCash(setting.QueueInfo.ResourceCashCost);
        GetOrCreatePreyGroup(setting.Id).WakeHunter(position, now);
    }

    public void CapturePrey(PreyGroupSetting setting, int hubLevel, int preyId, int[] crewSlots, bool bought, long now)
    {
        var prey = setting.GetPrey(preyId) ?? throw new DomainException(GameErrorType.InvalidData, $"Prey {preyId} not found in {setting.Id}");

        if (prey.LevelRequired > hubLevel)
            throw new DomainException(GameErrorType.InvalidState, $"Prey {preyId} requires hub level {prey.LevelRequired}");

        var preyMode = prey.GetMode(setting.GetActiveMode(hubLevel).Id) ?? throw new DomainException(GameErrorType.InvalidState, $"Prey {preyId} has no active mode");
        var captureCount = GetOrCreatePreyGroup(setting.Id).Capture(preyId, crewSlots, preyMode.HuntersRequired, bought, setting.QueueInfo.HunterSleepTime, now);

        HandleQuestsProgress("capturePreyByID", className: setting.Id, itemName: preyId.ToString());

        // The client never sends the doobers back
        foreach (var reward in preyMode.GetRewards(captureCount))
        {
            switch (reward.Type)
            {
                case "coin":
                    AddCoins(reward.IntValue);
                    break;
                case "xp":
                    AddXp(reward.IntValue);
                    break;
                case "energy":
                    AddEnergy(reward.IntValue);
                    break;
                case "goods":
                    AddGoods(reward.IntValue);
                    break;
                case "premium_goods":
                    AddPremiumGoods(reward.IntValue);
                    break;
                case "cash":
                    AddCash(reward.IntValue);
                    break;
                case "collectable":
                    var collectionName = GameSettingsManager.Instance.GetCollectionByItemName(reward.Value);

                    if (collectionName is not null)
                        AddItemToCollection(collectionName, reward.Value);
                    else
                        StaticLogger.Current.LogWarning("Collection for item {CollectableName} not found", reward.Value);

                    break;
                case "item":
                    AddItem(reward.Value, reward.Quantity);
                    break;
                default:
                    StaticLogger.Current.LogWarning("Unhandled prey reward type {RewardType}", reward.Type);
                    break;
            }
        }
    }
}
