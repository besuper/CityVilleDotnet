using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.UserService;

public class SetCityName(CityVilleDbContext context) : AmfService<SetCityNameRequest>
{
    public override async Task<ASObject> HandlePacket(SetCityNameRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var world = await context.Set<Player>()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        var name = world.GetWorld().SetWorldName(request.CityName);

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse().Data(new ASObject
        {
            ["name"] = name
        });
    }
}

public class SetCityNameRequest
{
    [AmfParam(1)] public string CityName { get; set; } = string.Empty;
}

public class SetCityNameValidator : AbstractValidator<SetCityNameRequest>
{
    public SetCityNameValidator()
    {
        RuleFor(x => x.CityName).NotEmpty().MaximumLength(32);
    }
}