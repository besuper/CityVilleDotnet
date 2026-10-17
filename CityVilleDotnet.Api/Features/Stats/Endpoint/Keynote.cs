using FastEndpoints;

namespace CityVilleDotnet.Api.Features.Stats.Endpoint;

internal sealed class Keynote : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("/v{revision}/keynote.php");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await Send.OkAsync(cancellation: ct);
    }
}
