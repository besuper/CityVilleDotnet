using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Persistence;
using FluorineFx;

namespace CityVilleDotnet.Api.Services.UserService;

public class PurchaseCrewMember(CityVilleDbContext context) : AmfService
{
    public override async Task<ASObject> HandlePacket(object[] @params, Guid userId, CancellationToken cancellationToken)
    {
        // TODO: Implement crew members logic

        return GatewayService.CreateEmptyResponse();
    }
}