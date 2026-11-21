using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.ViralAckService;

public sealed class Seen(CityVilleDbContext context) : AmfService<SeenRequest>
{
    public override async Task<ASObject> HandlePacket(SeenRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .Include(x => x.SocialPosts.Where(p => p.TargetObjectId == request.TargetObjectId && p.TargetWorld == p.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Helpers)
            .ThenInclude(x => x.Helper)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        player.MarkViralAckSeen(player.LastPlayedWorldType, request.TargetObjectId, int.TryParse(request.HelperZid, out var helperSnuid) ? helperSnuid : null);

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class SeenRequest
{
    [AmfParam(0)] public int TargetObjectId { get; set; }
    [AmfParam(1)] public string? HelperZid { get; set; }
}

public class SeenRequestValidator : AbstractValidator<SeenRequest>
{
    public SeenRequestValidator()
    {
        RuleFor(x => x.HelperZid).MaximumLength(32);
    }
}
