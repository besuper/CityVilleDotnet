using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.WorldService;

public class RecordFTUEProgress(CityVilleDbContext context) : AmfService<RecordFTUEProgressRequest>
{
    public override async Task<ASObject> HandlePacket(RecordFTUEProgressRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("Can't find player with UserId");

        player.GetWorld().SetWorldCreated(request.ResumeFrom);

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class RecordFTUEProgressRequest
{
    [AmfParam(0)] public string? ResumeFrom { get; set; }
}

public class RecordFTUEProgressRequestValidator : AbstractValidator<RecordFTUEProgressRequest>
{
    public RecordFTUEProgressRequestValidator()
    {
        RuleFor(x => x.ResumeFrom).MaximumLength(32);
    }
}
