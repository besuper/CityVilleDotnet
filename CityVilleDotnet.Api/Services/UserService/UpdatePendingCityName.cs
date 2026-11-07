using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.UserService;

public class UpdatePendingCityName(CityVilleDbContext context) : AmfService<UpdatePendingCityNameRequest>
{
    public override async Task<ASObject> HandlePacket(UpdatePendingCityNameRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        player.UpdatePendingCityName(request.CityName);

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse();
    }
}

public class UpdatePendingCityNameRequest
{
    [AmfParam(0)] public string CityName { get; set; } = string.Empty;
}

public class UpdatePendingCityNameRequestValidator : AbstractValidator<UpdatePendingCityNameRequest>
{
    public UpdatePendingCityNameRequestValidator()
    {
        RuleFor(x => x.CityName).NotEmpty().MaximumLength(22);
    }
}
