using FastEndpoints;

namespace CityVilleDotnet.Api.Features.Stats.Endpoint;

internal sealed class RecordStats : EndpointWithoutRequest
{
    public override void Configure()
    {
        Post("/record_stats.php");
        AllowAnonymous(); // FIXME: Add auth
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendOkAsync(ct);
    }
}