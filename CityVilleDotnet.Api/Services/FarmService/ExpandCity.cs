using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.FarmService;

public class ExpandCity(CityVilleDbContext context, ILogger<ExpandCity> logger) : AmfService
{
    public override async Task<ASObject> HandlePacket(object[] @params, Guid userId, CancellationToken cancellationToken)
    {
        var user = await context.Set<User>()
            .AsSplitQuery()
            .Include(x => x.Player)
            .Include(x => x.World)
            .ThenInclude(x => x.Objects)
            .Include(x => x.World)
            .ThenInclude(x => x.MapRects)
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

        if (user is null)
            throw new Exception("Can't find user");

        var itemName = (string)@params[0]; // expand_12_12
        var item = GameSettingsManager.Instance.GetItem(itemName);

        if (item is null)
            throw new Exception($"Can't find item {itemName}");

        var coordinates = (ASObject)@params[1];
        var x = (int?)coordinates["x"];
        var y = (int?)coordinates["y"];
        var weight = item.Height;
        var width = item.Width;

        if (x is null || y is null || weight is null || width is null)
            throw new Exception("Can't find MapRect coordinates");

        var world = user.GetWorld();

        // Add the new map area

        var newMapRect = new MapRect
        {
            Id = Guid.NewGuid(),
            X = x.Value,
            Y = y.Value,
            Height = int.Parse(item.Height), // FIXME: Change these value to the right type when loading the settings
            Width = int.Parse(item.Width)
        };

        world.AddMapRect(newMapRect);

        // Add new trees

        var trees = (object[])@params[2];

        foreach (ASObject tree in trees)
        {
            var newTree = new WorldObject
            {
                Id = Guid.NewGuid(),
                WorldFlatId = world.GetAvailableBuildingId(),
                TempId = -1,
                ItemName = (string)tree["itemName"],
                ClassName = "Wilderness",
                State = "static",
                Direction = (int)tree["dir"],
                Deleted = false,
                Position = new WorldObjectPosition
                {
                    X = (int)tree["x"],
                    Y = (int)tree["y"],
                    Z = 0
                }
            };

            world.AddBuilding(newTree);
        }

        user.Player.ExpansionsPurchased += 1;

        await context.SaveChangesAsync(cancellationToken);

        // FIXME : Return array of new trees with newId property to have a correct remapIds
        return GatewayService.CreateEmptyResponse();
    }
}