using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.GameMechanicService;

public class ContractInstaReady(CityVilleDbContext context) : AmfService<ContractInstaReadyRequest>
{
    public override async Task<ASObject> HandlePacket(ContractInstaReadyRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects.Where(o => o.WorldFlatId == request.ObjectId || o.TempId == request.ObjectId))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        var obj = player.GetWorld().GetBuildingByClientId(request.ObjectId);

        var gameItem = GameSettingsManager.Instance.GetItem(obj.ItemName) ?? throw new Exception($"Can't find game item for {obj.ItemName}");

        if (gameItem.Mechanics?.GetMechanicByGameMode(request.GameMode)?.GetMechanicItemByType("contractInstaReady") is null)
        {
            throw new Exception($"No contractInstaReady mechanic found for {obj.ItemName} in game mode {request.GameMode}");
        }

        player.InstaReadyMechanicContract(obj, gameItem);

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class ContractInstaReadyRequest
{
    [AmfParam(0)] public int ObjectId { get; set; }
    [AmfParam(2)] public string GameMode { get; set; } = string.Empty;
}

public class ContractInstaReadyRequestValidator : AbstractValidator<ContractInstaReadyRequest>
{
    public ContractInstaReadyRequestValidator()
    {
        RuleFor(x => x.GameMode).NotEmpty().MaximumLength(64);
    }
}
