using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.UserService;

public class UpdateRegenerableResource(CityVilleDbContext context) : AmfService<UpdateRegenerableResourceRequest>
{
    public override async Task<ASObject> HandlePacket(UpdateRegenerableResourceRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        player.RegenerateGas(ServerUtils.GetCurrentTimeSeconds());

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse();
    }
}

public class UpdateRegenerableResourceRequest
{
    [AmfParam(0)] public string ResourceName { get; set; } = string.Empty;
}

public class UpdateRegenerableResourceRequestValidator : AbstractValidator<UpdateRegenerableResourceRequest>
{
    public UpdateRegenerableResourceRequestValidator()
    {
        RuleFor(x => x.ResourceName).Equal("gas");
    }
}
