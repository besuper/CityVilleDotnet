using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.CollectionsService;

public class OnTradeIn(CityVilleDbContext context) : AmfService<OnTradeInRequest>
{
    public override async Task<ASObject> HandlePacket(OnTradeInRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var collection = GameSettingsManager.Instance.GetCollectionByName(request.CollectionName);

        if (collection is null)
            throw new Exception($"Can't find collection {request.CollectionName}");

        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Collections)
            .ThenInclude(x => x.Items)
            .Include(x => x.InventoryItems)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null)
            throw new Exception("Can't find user");

        var removeItems = player.CompleteCollection(collection);

        context.Set<CollectionItem>().RemoveRange(removeItems);

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class OnTradeInRequest
{
    [AmfParam(0)] public string CollectionName { get; set; } = string.Empty;
}