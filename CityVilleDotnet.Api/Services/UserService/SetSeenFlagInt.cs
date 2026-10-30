using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.UserService;

public class SetSeenFlagInt(CityVilleDbContext context) : AmfService<SetSeenFlagIntRequest>
{
    public override async Task<ASObject> HandlePacket(SetSeenFlagIntRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .Include(x => x.SeenFlags.Where(f => f.Key == request.FlagName))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        player.SetSeenFlagInt(request.FlagName, request.Value);

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse();
    }
}

public class SetSeenFlagIntRequest
{
    [AmfParam(0)] public string FlagName { get; set; } = string.Empty;
    [AmfParam(1)] public int Value { get; set; }
}

public class SetSeenFlagIntValidator : AbstractValidator<SetSeenFlagIntRequest>
{
    public SetSeenFlagIntValidator()
    {
        RuleFor(x => x.FlagName).NotEmpty().MaximumLength(64);
    }
}
