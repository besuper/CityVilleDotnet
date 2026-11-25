using System.Text.Json;
using CityVilleDotnet.Api.Common.Identity;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Features.DataServices.Endpoint;

internal sealed class DataServices(CityVilleDbContext dbContext) : Endpoint<DataServicesRequest, List<string>>
{
    public override void Configure()
    {
        Post("/dataServices.php");
        AllowFormData(urlEncoded: true);
    }

    public override async Task HandleAsync(DataServicesRequest req, CancellationToken ct)
    {
        var queryName = JsonSerializer.Deserialize<string>(req.QueryName);
        var days = JsonSerializer.Deserialize<int[]>(req.QueryArgs)?.FirstOrDefault() ?? 0;

        if (queryName is not ("getPlayedInLastNDays" or "getInactiveFriends") || days <= 0 || HttpContext.User.GetPlayerId() is not { } playerId)
        {
            await Send.OkAsync([], ct);
            return;
        }

        var activeSince = ServerUtils.TimeProvider.GetUtcNow().AddDays(-days);
        var active = queryName == "getPlayedInLastNDays";

        var snuids = await dbContext.Set<Friend>()
            .Where(f => f.Player!.Id == playerId && f.Status == FriendshipStatus.Accepted && f.FriendPlayer!.Snuid != -1)
            .Where(f => active ? f.FriendPlayer!.LastTrackingTimestamp >= activeSince : f.FriendPlayer!.LastTrackingTimestamp < activeSince)
            .Select(f => f.FriendPlayer!.Snuid)
            .ToListAsync(ct);

        await Send.OkAsync(snuids.ConvertAll(x => x.ToString()), ct);
    }
}

internal sealed class DataServicesRequest
{
    public string QueryName { get; set; } = string.Empty;
    public string QueryArgs { get; set; } = string.Empty;
}

internal sealed class DataServicesRequestValidator : Validator<DataServicesRequest>
{
    public DataServicesRequestValidator()
    {
        RuleFor(x => x.QueryName).NotEmpty().MaximumLength(64);
        RuleFor(x => x.QueryArgs).NotEmpty().MaximumLength(256);
    }
}
