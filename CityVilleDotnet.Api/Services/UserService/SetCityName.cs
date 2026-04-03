using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.UserService;

public class SetCityName(CityVilleDbContext context) : AmfService<SetCityNameRequest>
{
    public override async Task<ASObject> HandlePacket(SetCityNameRequest request, Guid userId, CancellationToken cancellationToken)
    {
        var world = await context.Set<User>()
            .Where(x => x.UserId == userId)
            .Include(x => x.Player)
            .ThenInclude(x => x.World)
            .FirstOrDefaultAsync(cancellationToken) ?? throw new Exception("Can't to find world with UserId");

        var name = world.GetPlayer().GetWorld().SetWorldName(request.CityName);

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