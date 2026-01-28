using FastEndpoints;

namespace CityVilleDotnet.Api.Features.Error.Endpoint;

internal sealed class RecordError(ILogger<RecordError> logger) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Post("/error.php");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        using var reader = new StreamReader(HttpContext.Request.Body);
        
        var body = await reader.ReadToEndAsync(ct);
        logger.LogDebug("Error received: {Body}", body);

        await Send.OkAsync(cancellation: ct);
    }
}