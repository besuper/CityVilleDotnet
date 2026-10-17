using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.UserService;

public class UpdateFlag(CityVilleDbContext context) : AmfService<UpdateFlagRequest>
{
    public override async Task<ASObject> HandlePacket(UpdateFlagRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .Include(x => x.Flags.Where(f => f.Name == request.Params.Name))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        player.SetFlag(request.Params.Name, request.Params.Value);

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse();
    }
}

public class UpdateFlagRequest
{
    [AmfParam(0)] public UpdateFlagParams Params { get; set; } = new();
}

public class UpdateFlagParams
{
    [AmfParam("name")] public string Name { get; set; } = string.Empty;
    [AmfParam("value")] public int Value { get; set; }
}

public class UpdateFlagValidator : AbstractValidator<UpdateFlagRequest>
{
    public UpdateFlagValidator()
    {
        RuleFor(x => x.Params.Name).NotEmpty().MaximumLength(64);
    }
}
