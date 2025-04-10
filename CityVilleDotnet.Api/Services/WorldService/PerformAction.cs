using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.WorldService;

internal sealed class PerformAction(CityVilleDbContext context, ILogger<PerformAction> _logger) : AmfService(context)
{
    public override async Task<ASObject> HandlePacket(object[] _params, Guid userId, CancellationToken cancellationToken)
    {
        var user = await context.Set<User>()
            .Include(x => x.UserInfo)
            .ThenInclude(x => x.World)
            .ThenInclude(x => x.Objects)
            .Include(x => x.UserInfo)
            .ThenInclude(x => x.World)
            .ThenInclude(x => x.CitySim)
            .Include(x => x.UserInfo)
            .ThenInclude(x => x.Player)
            .Include(x => x.Quests)
            .Where(x => x.UserId == userId)
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            throw new Exception("User can't be null");
        }

        var actionType = _params[0] as string;

        _logger.LogInformation($"PerformAction type {actionType}");

        if (actionType == "place")
        {
            var building = _params[1] as ASObject;

            if (building is null)
            {
                throw new Exception("Building can't be null when action type is place");
            }

            // TODO: Implement components
            // ignore components for now

            var position = building["position"] as ASObject;
            var className = (string)building["className"];
            var itemName = (string)building["itemName"];

            var obj = new WorldObject(
                itemName,
                className,
                null,
                (bool)building["deleted"],
                (int)building["tempId"],
                (string)building["state"],
                (int)building["direction"],
                new WorldObjectPosition()
                {
                    X = (int)position["x"],
                    Y = (int)position["y"],
                    Z = (int)position["z"]
                },
                (int)building["id"]
            );

            var gameItem = GameSettingsManager.Instance.GetItem(itemName);

            if (gameItem is not null)
            {
                if (gameItem.Cost is not null)
                {
                    user.RemoveCoin(gameItem.Cost.Value);
                }

                if (gameItem.Construction is not null)
                {
                    obj.SetAsConstructionSite();
                }
            }

            user.GetWorld().AddBuilding(obj);

            // TODO: Check coins, goods, energy, etc...
            // Add population

            await context.SaveChangesAsync(cancellationToken);
        }

        if (actionType == "build")
        {
            var building = _params[1] as ASObject;

            if (building is null)
            {
                throw new Exception("Building can't be null when action type is place");
            }

            var position = building["position"] as ASObject;
            var itemId = (int)building["id"];

            var obj = user.GetWorld().GetBuilding(itemId, (int)position["x"], (int)position["y"], (int)position["z"]);

            if (obj is null)
            {
                throw new Exception($"Can't find building with ID {itemId}");
            }

            if (obj.Builds is null)
            {
                throw new Exception($"Can't find `builds`");
            }

            obj.AddConstructionStage();

            await context.SaveChangesAsync(cancellationToken);
        }

        if (actionType == "finish")
        {
            var building = _params[1] as ASObject;

            if (building is null)
            {
                throw new Exception($"Building can't be null when action type is {actionType}");
            }

            var position = building["position"] as ASObject;
            var itemId = (int)building["id"];
            var world = user.GetWorld();

            var obj = world.GetBuilding(itemId, (int)position["x"], (int)position["y"], (int)position["z"]);

            if (obj is null)
            {
                throw new Exception($"Can't find building with ID {itemId}");
            }

            if (obj.Builds is null)
            {
                throw new Exception($"Can't find `builds`");
            }

            obj.FinishConstruction();

            world.calculateCurrentPopulation();
            world.calculatePopulationCap();

            user.handleQuestProgress("");
            user.CheckCompletedQuests();

            await context.SaveChangesAsync(cancellationToken);
        }

        return GatewayService.CreateEmptyResponse();
    }
}
