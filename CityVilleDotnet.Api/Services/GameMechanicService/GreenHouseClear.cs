using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.GameMechanicService;

public class GreenHouseClear(CityVilleDbContext context) : AmfService<GreenHouseClearRequest>
{
    public override async Task<ASObject> HandlePacket(GreenHouseClearRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects.Where(o => o.WorldFlatId == request.ObjectId || o.TempId == request.ObjectId))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        var worldObj = player.GetWorld().GetBuildingByClientId(request.ObjectId);

        if (worldObj.GetClassName() != BuildingClassType.GreenHouse) throw new Exception($"Can't clear green house for {worldObj.GetClassName()}");

        var gameItem = GameSettingsManager.Instance.GetItem(worldObj.ItemName) ?? throw new Exception($"Can't find game item for {worldObj.ItemName}");

        if (gameItem.NumCrop is null) throw new Exception("Num crop not found");

        if (!worldObj.GetGreenHousePlot().IsWithered()) throw new DomainException(GameErrorType.ForceReload, "Green plot is not withered on server side");

        var contractItem = GameSettingsManager.Instance.GetItem(worldObj.ContractName!) ?? throw new Exception($"Can't find game item for {worldObj.ContractName}");

        // ClearPlotMechanic.getDoobers: int(cost * witherRefundMultiplier) coins per crop
        var refund = (int)((contractItem.Cost ?? 0) * GameSettingsManager.Instance.GetSettings().WitherRefundMultiplier) * gameItem.NumCrop.Value;

        if (refund > 0)
            player.AddCoins(refund);

        worldObj.Plow();

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class GreenHouseClearRequest
{
    [AmfParam(0)] public int ObjectId { get; set; }
}
