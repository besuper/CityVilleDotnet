using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Common.Persistence;
using FluorineFx;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CityVilleDotnet.Api.Services.UserService.initNeighbors;

internal sealed class InitNeighbors(CityVilleDbContext context) : AmfService(context)
{
    public class NeighborResponse
    {
        [JsonPropertyName("neighbors")]
        public List<Neighbor> Neighbors { get; set; }
    }

    public class Neighbor
    {
        [JsonPropertyName("uid")]
        public int Uid { get; set; }

        [JsonPropertyName("fake")]
        public int? Fake { get; set; }
    }

    public override async Task<ASObject> HandlePacket(object[] _params, Guid userId, CancellationToken cancellationToken)
    {
        var response = new NeighborResponse()
        {
            Neighbors = new List<Neighbor>()
            {
                new Neighbor()
                {
                    Uid = -1,
                    Fake = 1
                },
                new Neighbor()
                {
                    Uid = 3
                },
                new Neighbor()
                {
                    Uid = 4
                },
                new Neighbor()
                {
                    Uid = 5
                },
                new Neighbor()
                {
                    Uid = 6
                }
            }
        };

        var obj = AmfConverter.JsonToASObject(JsonSerializer.Serialize(response));

        return new CityVilleResponse(333, obj);
    }
}
