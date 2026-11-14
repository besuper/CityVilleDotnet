using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.WorldService;

public class UpdateState(CityVilleDbContext context) : AmfService<UpdateStateRequest>
{
    public override async Task<ASObject> HandlePacket(UpdateStateRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var states = request.Params.FirstOrDefault() ?? [];

        if (states.Count == 0)
        {
            return new CityVilleResponse();
        }

        var ids = states.Keys.Select(int.Parse).ToList();

        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects.Where(o => ids.Contains(o.WorldFlatId) || ids.Contains(o.TempId)))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        var world = player.GetWorld();

        foreach (var (id, state) in states)
        {
            var obj = world.GetBuildingByClientId(int.Parse(id));
            obj.UpdateTrackState(Domain.EnumExtensions.EnumExtensions.ParseFromDescription<WorldObjectState>((string)state));
        }

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse();
    }
}

public class UpdateStateRequest
{
    [AmfParam(3)] public Dictionary<string, object>[] Params { get; set; } = [];
}

public class UpdateStateRequestValidator : AbstractValidator<UpdateStateRequest>
{
    public UpdateStateRequestValidator()
    {
        RuleForEach(x => x.Params)
            .Must(states => states.Count <= 64 && states.All(s => int.TryParse(s.Key, out _) && s.Value is string { Length: <= 32 }));
    }
}
