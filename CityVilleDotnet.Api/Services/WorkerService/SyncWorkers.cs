using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Services.WorkerService.Common;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Domain.GameEntities;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.WorkerService;

public class SyncWorkers(CityVilleDbContext context) : AmfService<SyncWorkersRequest>
{
    public override async Task<ASObject> HandlePacket(SyncWorkersRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        if (request.Feature is not (WorkerBucket.FactoriesFeature or WorkerBucket.TrainsFeature))
            throw new Exception($"Unsupported worker feature {request.Feature}");

        var isTrains = request.Feature == WorkerBucket.TrainsFeature;

        var player = await context.Set<Player>()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects.Where(o => !isTrains && o.ClassName == BuildingClassType.Factory))
            .ThenInclude(x => x.Workers)
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.TrainOrder)
            .ThenInclude(x => x!.Workers)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        var world = player.GetWorld();

        return new CityVilleResponse().Data(new ASObject
        {
            ["workers"] = isTrains ? world.ToTrainWorkersAsObject() : world.ToFactoryWorkersAsObject()
        });
    }
}

public class SyncWorkersRequest
{
    [AmfParam(0)] public string Feature { get; set; } = string.Empty;
}

public class SyncWorkersValidator : AbstractValidator<SyncWorkersRequest>
{
    public SyncWorkersValidator()
    {
        RuleFor(x => x.Feature).NotEmpty().MaximumLength(64);
    }
}
