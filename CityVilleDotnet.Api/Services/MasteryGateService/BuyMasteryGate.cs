using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.MasteryGateService;

public class BuyMasteryGate(CityVilleDbContext context) : AmfService<BuyMasteryGateRequest>
{
    public override async Task<ASObject> HandlePacket(BuyMasteryGateRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var gate = GameSettingsManager.Instance.GetItem(request.ItemName)?.GetGates().FirstOrDefault(x => x.Name == request.GateName && x.Type == "mastery")
                   ?? throw new Exception($"Can't find mastery gate {request.GateName} on {request.ItemName}");

        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.SeenFlags)
            .Include(x => x.Masteries)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        player.BuyMasteryGate(request.ItemName, gate);

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class BuyMasteryGateRequest
{
    [AmfParam(0)] public string ItemName { get; set; } = string.Empty;
    [AmfParam(1)] public string GateName { get; set; } = string.Empty;
}

public class BuyMasteryGateValidator : AbstractValidator<BuyMasteryGateRequest>
{
    public BuyMasteryGateValidator()
    {
        RuleFor(x => x.ItemName).NotEmpty().MaximumLength(64);
        RuleFor(x => x.GateName).NotEmpty().MaximumLength(32);
    }
}
