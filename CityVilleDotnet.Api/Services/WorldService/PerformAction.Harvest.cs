﻿using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using FluorineFx;

namespace CityVilleDotnet.Api.Services.WorldService;

internal sealed partial class PerformAction
{
    private async Task<CityVilleResponse> PerformHarvest(User user, object[] _params, Guid userId, CancellationToken cancellationToken)
    {
        var building = _params[1] as ASObject ?? throw new Exception($"Building can't be null");

        // TODO: Remove (or make debug)
        foreach (var item in building)
        {
            _logger.LogInformation($"{item.Key} = {item.Value}");
        }

        var position = building["position"] as ASObject ?? throw new Exception("Can't find position inside building element");
        var world = user.GetWorld();

        var obj = world.GetBuildingByCoord((int)position["x"], (int)position["y"], (int)position["z"]) ?? throw new Exception($"Can't find building");

        // TODO: Remove
        _logger.LogInformation($"{obj.State}");
        _logger.LogInformation($"{obj.PlantTime}");

        var className = (string)building["className"];
        var coinYield = 0;

        if (className == "Plot")
        {
            var contract = (string)building["contractName"];
            var gameItem = GameSettingsManager.Instance.GetItem(contract);

            if (gameItem is not null)
                coinYield = gameItem.CoinYield ?? 0;

            obj.State = "plowed";
        }
        else
        {
            var itemName = (string)building["itemName"];
            var gameItem = GameSettingsManager.Instance.GetItem(itemName);

            if (gameItem is not null)
                coinYield = gameItem.CoinYield ?? 0;
        }

        if (obj.State == "open")
            obj.State = "closed";

        if (obj.State == "grown")
        {
            obj.State = "planted";
            obj.PlantTime = DateTimeOffset.Now.ToUnixTimeSeconds() * 1000;
        }

        var secureRands = user.CollectDoobersRewards(obj.ItemName);

        _logger.LogInformation($"Secure rands {secureRands}");
        _logger.LogInformation($"Secure rands {secureRands.Count}");

        _logger.LogInformation($"{obj.State}");
        _logger.LogInformation($"{obj.PlantTime}");

        user.HandleQuestProgress(itemName: obj.ItemName == "plot_crop" ? obj.ClassName : obj.ItemName);
        user.CheckCompletedQuests();

        await _context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse(333, new ASObject
        {
            ["retCoinYield"] = coinYield,
            //response["doobers"] = AmfConverter.Convert(new List<int>());
            ["secureRands"] = AmfConverter.Convert(secureRands)
        });
    }
}
