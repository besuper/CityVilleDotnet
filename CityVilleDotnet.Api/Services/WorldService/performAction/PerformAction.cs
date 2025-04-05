using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Common.Persistence;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Api.Services.UserService.Domain;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.WorldService.performAction;

internal sealed class PerformAction(CityVilleDbContext context) : AmfService(context)
{
    public override async Task<ASObject> HandlePacket(object[] _params, Guid userId)
    {
        var user = await context.Set<User>()
            .AsNoTracking()
            .Include(x => x.UserInfo)
            .ThenInclude(x => x.World)
            .ThenInclude(x => x.Objects)
            .Where(x => x.UserId == userId)
            .FirstOrDefaultAsync();

        if (user is null)
        {
            throw new Exception("User can't be null");
        }

        var actionType = _params[0] as string;

        // TODO: Load gameSettings.xml
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

            var obj = new WorldObject()
            {
                ClassName = (string)building["className"],
                ItemName = (string)building["itemName"],
                TempId = (int)building["tempId"],
                WorldFlatId = (int)building["id"],
                Deleted = (bool)building["deleted"],
                Direction = (int)building["direction"],
                State = (string)building["state"],
                Position = new WorldObjectPosition()
                {
                    X = (int)position["x"],
                    Y = (int)position["y"],
                    Z = (int)position["z"]
                }
            };

            user.GetWorld().AddBuilding(obj);

            await context.SaveChangesAsync();
        }

        return GatewayService.CreateEmptyResponse();
    }
}
