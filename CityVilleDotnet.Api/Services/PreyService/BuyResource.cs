using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.PreyService;

internal sealed class BuyResource(CityVilleDbContext context) : AmfService<BuyResourceRequest>
{
    public override async Task<ASObject> HandlePacket(BuyResourceRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var setting = GameSettingsManager.Instance.GetPreyGroup(request.GroupId) ?? throw new DomainException(GameErrorType.InvalidData, $"Unknown prey group {request.GroupId}");

        var player = await context.Set<Player>()
            .Include(x => x.PreyGroups.Where(g => g.GroupId == request.GroupId))
            .ThenInclude(x => x.Hunters)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        player.BuyPreyResource(setting, request.CrewSlot, ServerUtils.GetCurrentTimeSeconds());

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class BuyResourceRequest
{
    [AmfParam(0)] public string GroupId { get; set; } = string.Empty;
    [AmfParam(1)] public int CrewSlot { get; set; }
}

public class BuyResourceValidator : AbstractValidator<BuyResourceRequest>
{
    public BuyResourceValidator()
    {
        RuleFor(x => x.GroupId).NotEmpty().MaximumLength(32);
    }
}
