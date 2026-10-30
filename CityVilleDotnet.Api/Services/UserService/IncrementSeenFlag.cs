using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.UserService;

public class IncrementSeenFlag(CityVilleDbContext context) : AmfService<IncrementSeenFlagRequest>
{
    public override async Task<ASObject> HandlePacket(IncrementSeenFlagRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .Include(x => x.SeenFlags.Where(f => f.Key == request.FlagName))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        player.IncrementSeenFlag(request.FlagName);

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse();
    }
}

public class IncrementSeenFlagRequest
{
    [AmfParam(0)] public string FlagName { get; set; } = string.Empty;
}

public class IncrementSeenFlagValidator : AbstractValidator<IncrementSeenFlagRequest>
{
    public IncrementSeenFlagValidator()
    {
        RuleFor(x => x.FlagName).NotEmpty().MaximumLength(64);
    }
}
