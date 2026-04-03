using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Services.WorldService.Common;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.WorldService;

public class SendToStorage(CityVilleDbContext context) : AmfService<SendToStorageRequest>
{
    public override async Task<ASObject> HandlePacket(SendToStorageRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var user = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.World)
            .ThenInclude(x => x!.Objects)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (user is null) throw new Exception("Player not found");

        // TODO: Implement this

        return new CityVilleResponse();
    }
}

public class SendToStorageRequest
{
    [AmfParam(1)] public BuildingSendToStorageRequest Building { get; set; } = new();
    [AmfParam(3)] public StorageDetails[] Storage { get; set; } = [];
}

public class BuildingSendToStorageRequest
{
    [AmfParam("position")] public PerformActionPositionRequest Position { get; set; } = new();
}

public class StorageInfoRequest
{
    [AmfParam(0)] public StorageDetails Details { get; set; } = new();
}

public class StorageDetails
{
    [AmfParam("storageKey")] public string Key { get; set; } = string.Empty;
}