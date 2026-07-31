using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.FranchiseService;

public sealed class GrantHeadquarters(CityVilleDbContext context) : AmfService<GrantHeadquartersRequest>
{
    public override async Task<ASObject> HandlePacket(GrantHeadquartersRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var hqName = GameSettingsManager.Instance.GetItem(request.FranchiseType)?.HeadquartersName;
        if (hqName is null) throw new Exception($"No headquarters for {request.FranchiseType}");

        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Franchises.Where(f => f.FranchiseType == request.FranchiseType))
            .Include(x => x.InventoryItems)
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x!.Objects.Where(o => o.ItemName == hqName || o.TargetBuildingName == hqName))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("Player not found");

        if (player.Franchises.FirstOrDefault() is null)
            throw new Exception($"Can't find franchise {request.FranchiseType}");

        if (!player.HasItem(hqName) && !player.GetWorld().Objects.Any(x => x.GetItemName() == hqName))
            player.AddItem(hqName);

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class GrantHeadquartersRequest
{
    [AmfParam(0)] public string FranchiseType { get; set; } = string.Empty;
}
