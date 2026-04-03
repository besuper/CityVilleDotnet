using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.UserService;

public class SetSeenFlag(CityVilleDbContext context) : AmfService<SetSeenFlagRequest>
{
    public override async Task<ASObject> HandlePacket(SetSeenFlagRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var user = await context.Set<Player>()
            .Include(x => x!.SeenFlags)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        user.SetSeenFlag(request.FlagName);

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class SetSeenFlagRequest
{
    [AmfParam(0)] public string FlagName { get; set; } = string.Empty;
}

public class SetSeenFlagValidator : AbstractValidator<SetSeenFlagRequest>
{
    public SetSeenFlagValidator()
    {
        RuleFor(x => x.FlagName).NotEmpty().MaximumLength(64);
    }
}