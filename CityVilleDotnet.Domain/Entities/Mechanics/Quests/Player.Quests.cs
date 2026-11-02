using CityVilleDotnet.Common.Global;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Settings.QuestSettings;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Domain.EnumExtensions;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace CityVilleDotnet.Domain.Entities;

public partial class Player
{
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
                        case "acceptTrain":
                        case "welcomeTrain":
                        //case "neighborVisit":
                        case "onValidCityName":
                        case "incrementalExpansionCount":
                        case "expand":
                        case "buildingremodeled":
                        case "citySamHQ":
                        case "sendFranchiseSupply":
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
                        case "instantReadyByClass":
                        case "neighborVisit": // This case is special, if type is empty we accept everything, if not we have to check if it matches who we visit
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
                        case "startContractByName":
                        case "moveByName":
                        case "upgradeItemByName":
                        case "acceptTrainByName":
                        {
                            if (itemName is null)
                                throw new Exception("Can't validate byName action without itemName");

                            if (taskType.Equals(itemName) || (splitType is not null && splitType.Contains(itemName)))
                                quest.Progress[index] += 1;

                            break;
                        }
                        case "harvestResidenceByRegEx":
                        {
                            if (itemName is null)
                                throw new Exception("Can't validate byRegEx action without itemName");

                            if (Regex.IsMatch(itemName, taskType))
                                quest.Progress[index] += 1;

                            break;
                        }
                        case "placeByKeyword":
                        case "harvestByKeyword":
                        case "harvestMechanicContractByKeywords":
                        case "harvestBusinessByKeyword":
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
                        case "gainLevels":
                            quest.Progress[index] += amount;
                            break;
                        case "capturePreyByID":
                            // itemName is the prey id, className the prey group
                            if (itemName is null)
                                throw new Exception("Can't validate capturePreyByID action without itemName");

                            if ((taskType == "" || taskType.Equals(itemName) || (splitType is not null && splitType.Contains(itemName))) && (task.GroupId is null || task.GroupId == className))
                                quest.Progress[index] += 1;

                            break;
                    }
                }

                if (actionTask.StartsWith("checkFarmville2Level"))
                {
                    quest.Progress[index] = 1;
                    continue;
                }

                if (actionTask == "gainLevels" && Level >= GameSettingsManager.Instance.GetSettings().HighestLevel)
                {
                    quest.Progress[index] = int.Parse(task.Total);
                    continue;
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
                    case "countPlayerMapExpansions":
                        quest.Progress[index] = ExpansionsPurchased;
                        continue;
                    case "countPlayerCropMasteryByType":
                    {
                        var names = splitType ?? [taskType];

                        quest.Progress[index] = names.Max(GetMasteryLevel);
                        continue;
                    }
                    case "countStorageObjectsByType":
                        if (!calculatedResults.TryGetValue(resultKey, out value))
                            calculatedResults[resultKey] = value = CountStorageItemsByType(task.Type);

                        quest.Progress[index] = value;
                        continue;
                    case "countCrewByItemName":
                    {
                        var names = splitType ?? [taskType];

                        quest.Progress[index] = names.Sum(x => GetWorld().CountCrewMembersByItemName(x));
                        continue;
                    }
                    case "completeCollectionByName":
                    {
                        var names = splitType ?? [taskType];

                        quest.Progress[index] = names.Any(HasCompletedOrTradedInCollection) ? 1 : 0;
                        continue;
                    }
                    case "haveInventoryItems":
                    {
                        var names = splitType ?? [taskType];

                        quest.Progress[index] = names.Sum(CountInventoryItem);
                        continue;
                    }
                    case "haveEnergyBonus":
                        quest.Progress[index] = int.TryParse(taskType, out var energyBonus) && GetEnergyModifierTotal() >= energyBonus ? 1 : 0;
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
                        var count = GetWorld().CountStreakByItemNames(splitType ?? [taskType]);

                        if (count >= task.Streak)
                            quest.Progress[index] = 1;
                        continue;
                    case "checkStreakEffect":
                        var effect = GetWorld().GetStreakEffectByItemNames(splitType ?? [taskType]);

                        if (effect >= task.Streak)
                            quest.Progress[index] = 1;
                        continue;
                    case "countFranchiseExpansionsByName":
                        if (!calculatedResults.TryGetValue(resultKey, out value))
                            calculatedResults[resultKey] = value = CountOpenFranchiseLocations(taskType.Length == 0 ? null : splitType ?? [taskType]);

                        if (value > quest.Progress[index])
                            quest.Progress[index] = value;

                        continue;
                    case "countHeadquarters":
                        if (!calculatedResults.TryGetValue(resultKey, out value))
                            calculatedResults[resultKey] = value = GetWorld().CountBuildingByClassName(taskType);
                        
                        if (value > quest.Progress[index])
                            quest.Progress[index] = value;
                        
                        continue;
                    // TODO: Check how to correctly implement sticky quests
                    case "itemGateComplete":
                        if (HasInventoryGateKeys(taskType, task.Gate ?? "build"))
                        {
                            quest.Progress[index] = 1;
                        }

                        continue;
                    case "couponObtained":
                        if (HasCoupon(taskType))
                        {
                            quest.Progress[index] = 1;
                        }

                        continue;
                    case "getExpansionByCoord":
                    {
                        // x,y[,worldId]
                        var coords = taskType.Split(',');

                        if (coords.Length > 2 && coords[2] != GetWorld().Type.ToDescriptionString()) continue;

                        if (GetWorld().IsPointInTerritory(int.Parse(coords[0]), int.Parse(coords[1])))
                        {
                            quest.Progress[index] = 1;
                        }

                        continue;
                    }
                }
            }
        }
    }

    public List<InventoryItem> CheckCompletedQuests()
    {
        var newQuests = new List<Quest>();
        var consumedItems = new List<InventoryItem>();

        foreach (var item in Quests.Where(x => x.QuestType == QuestType.Active))
        {
            if (item.IsCompleted())
            {
                item.QuestType = QuestType.Completed;
                item.ClaimRewards(this);
                consumedItems.AddRange(ConsumeQuestItems(item.Name));

                newQuests.AddRange(item.StartSequels());
            }
        }

        Quests.AddRange(newQuests);

        foreach (var quest in newQuests)
        {
            var questItem = QuestSettingsManager.Instance.GetItem(quest.Name);

            if (questItem is not null)
                GrantQuestStartItems(questItem);
        }

        return consumedItems;
    }

    // GameQuest::doQuestCompleteConsumptions
    private List<InventoryItem> ConsumeQuestItems(string questName)
    {
        var consumptions = QuestSettingsManager.Instance.GetItem(questName)?.Consumption?.Items ?? [];
        var removed = new List<InventoryItem>();

        foreach (var consumption in consumptions.Where(x => x.Type == "inventory"))
        {
            var owned = CountInventoryItem(consumption.Name);
            var amount = consumption.Number == "all" ? owned : Math.Min(int.Parse(consumption.Number ?? "0"), owned);

            if (amount <= 0) continue;

            var removedItem = RemoveItem(consumption.Name, amount);

            if (removedItem is not null)
            {
                removed.Add(removedItem);
            }
        }

        return removed;
    }

    // GameQuestUtility::consumeItemsAtInit
    public List<InventoryItem> ConsumeQuestInitItems()
    {
        var removed = new List<InventoryItem>();

        foreach (var quest in Quests.Where(x => x.QuestType == QuestType.Active))
        {
            var tasks = QuestSettingsManager.Instance.GetItem(quest.Name)?.Tasks.Tasks ?? [];

            for (var index = 0; index < tasks.Count; index++)
            {
                var task = tasks[index];

                if (task.Action != "consumeItemsAtInit" || task.Type is null) continue;
                if (quest.Progress[index] + quest.Purchased[index] >= int.Parse(task.Total)) continue;

                foreach (var name in task.Type.Split(','))
                {
                    var owned = CountInventoryItem(name);

                    if (owned <= 0) continue;

                    quest.Progress[index] += owned;

                    var removedItem = RemoveItem(name, owned);

                    if (removedItem is not null)
                    {
                        removed.Add(removedItem);
                    }
                }
            }
        }

        return removed;
    }

    public void SpawnEligibleQuests()
    {
        if (IsNew) return;

        var worldId = LastPlayedWorldType.ToDescriptionString();
        var population = GetWorld().GetCurrentPopulation();

        foreach (var item in QuestSettingsManager.Instance.GetAllQuests())
        {
            if (item.SpawnsFrom != "noparent") continue;
            if (item.Visible != "true" && !QuestSettingsManager.HiddenSpawnableQuests.Contains(item.Name)) continue;
            if (!string.IsNullOrEmpty(item.Validate)) continue;
            if (item.Experiments is { Count: > 0 }) continue;
            if (item.RequiredLevel is not null && Level < item.RequiredLevel) continue;
            if (item.RequiredPopulation is not null && population < item.RequiredPopulation) continue;

            var worlds = string.IsNullOrEmpty(item.Worlds)
                ? [WorldType.Main.ToDescriptionString()]
                : item.Worlds.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            if (!worlds.Contains(worldId)) continue;

            StartQuest(item);
        }
    }

    public bool StartQuest(QuestItem questItem)
    {
        if (Quests.Any(q => q.Name == questItem.Name)) return false;

        Quests.Add(Quest.Create(questItem.Name, questItem.Tasks.Tasks.Count, QuestType.Active));
        GrantQuestStartItems(questItem);

        return true;
    }

    public void GrantQuestStartItems(QuestItem questItem)
    {
        var granted = false;

        if (questItem.Init?.Functions is not null)
        {
            foreach (var function in questItem.Init.Functions.Where(f => f.Name == "grantItemOnInit"))
            {
                AddItem(function.ItemName);
                granted = true;
            }
        }

        if (!granted && QuestSettingsManager.QuestStartInventoryItem.TryGetValue(questItem.Name, out var items))
        {
            foreach (var item in items)
                AddItem(item);
        }
    }

    public void ExpireQuest(string questName)
    {
        var quest = Quests.FirstOrDefault(x => x.Name == questName && x.QuestType == QuestType.Active);

        if (quest is null) return;

        quest.QuestType = QuestType.Expired;
    }
}
