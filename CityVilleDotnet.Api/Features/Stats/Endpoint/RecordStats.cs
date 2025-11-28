using FastEndpoints;

namespace CityVilleDotnet.Api.Features.Stats.Endpoint;

internal sealed class RecordStats : EndpointWithoutRequest
{
    public override void Configure()
    {
        Post("/record_stats.php");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await Send.OkAsync(cancellation: ct);
    }
}