using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.CollectionsService;

public class OnTradeIn(CityVilleDbContext context) : AmfService
{
    public override async Task<ASObject> HandlePacket(object[] @params, Guid userId, CancellationToken cancellationToken)
    {
        var collectionName = (string)@params[0];

        if (collectionName is null)
            throw new Exception("Collection name is null");

        var collection = GameSettingsManager.Instance.GetCollectionByName(collectionName);

        if (collection is null)
            throw new Exception($"Can't find collection {collectionName}");

        var player = await context.Set<User>()
            .AsSplitQuery()
            .Include(x => x.Player)
            .ThenInclude(x => x!.Collections)
            .ThenInclude(x => x!.Items)
            .Include(x => x.Player)
            .ThenInclude(x => x!.InventoryItems)
            .Where(x => x.UserId == userId)
            .Select(x => x.Player)
            .FirstOrDefaultAsync(cancellationToken);

        if (player is null)
            throw new Exception("Can't find user");

        if (player.HasCompletedCollection(collection))
        {
            var removeItems = player.CompleteCollection(collection);

            context.Set<CollectionItem>().RemoveRange(removeItems);
        }

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}