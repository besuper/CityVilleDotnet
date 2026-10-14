using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.GameMechanicService;

public class Name(CityVilleDbContext context) : AmfService<NameRequest>
{
    public override async Task<ASObject> HandlePacket(NameRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        if (!request.ExtraData.TryGetValue("name", out var value) || value is not string name || string.IsNullOrWhiteSpace(name) || name.Length > 30)
            throw new DomainException(GameErrorType.ForceReload, "Invalid request, missing name param");

        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects.Where(o => o.WorldFlatId == request.ObjectId || o.TempId == request.ObjectId))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");
        
        var obj = player.GetWorld().GetBuildingByClientId(request.ObjectId);

        var gameItem = GameSettingsManager.Instance.GetItem(obj.ItemName) ?? throw new Exception($"Can't find game item for {obj.ItemName}");

        if (gameItem.Mechanics?.GetMechanicByGameMode(request.GameMode)?.GetMechanicItemByType("name") is null)
            throw new Exception($"No name mechanic found for {obj.ItemName} in game mode {request.GameMode}");

        obj.Rename(name);

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class NameRequest
{
    [AmfParam(0)] public int ObjectId { get; set; }
    [AmfParam(2)] public string GameMode { get; set; } = string.Empty;
    [AmfParam(3)] public Dictionary<string, object> ExtraData { get; set; } = new();
}
