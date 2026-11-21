using CityVilleDotnet.Api.Common.Identity;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Features.Social.Endpoint;

// crew.php?mId=<objectId>[&gname=<gate>][&pos=<slot>] opened by the client crew buttons
internal sealed class CrewRequest(CityVilleDbContext dbContext) : Endpoint<CrewRequestRequest>
{
    public override void Configure()
    {
        Post("/social/crew-request");
    }

    public override async Task HandleAsync(CrewRequestRequest req, CancellationToken ct)
    {
        if (HttpContext.User.GetPlayerId() is not { } playerId)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var player = await dbContext.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects.Where(o => o.WorldFlatId == req.ObjectId || o.TempId == req.ObjectId))
            .ThenInclude(x => x.CrewMembers)
            .Include(x => x.SocialPosts.Where(p => p.Type == SocialPostType.Crew && p.TargetObjectId != null))
            .FirstOrDefaultAsync(x => x.Id == playerId, ct);

        if (player is null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var building = player.GetWorld().Objects.FirstOrDefault(x => x.WorldFlatId == req.ObjectId || x.TempId == req.ObjectId);

        if (building is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        player.RequestCrew(building, req.GateName, ServerUtils.TimeProvider.GetUtcNow().UtcDateTime);

        await dbContext.SaveChangesAsync(ct);

        await Send.OkAsync(cancellation: ct);
    }
}

internal sealed class CrewRequestRequest
{
    public int ObjectId { get; set; }
    public string? GateName { get; set; }
}

internal sealed class CrewRequestValidator : Validator<CrewRequestRequest>
{
    public CrewRequestValidator()
    {
        RuleFor(x => x.GateName).MaximumLength(32);
    }
}
