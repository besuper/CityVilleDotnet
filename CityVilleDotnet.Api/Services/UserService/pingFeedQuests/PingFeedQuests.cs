using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Common.Persistence;
using CityVilleDotnet.Api.Services.QuestService.Domain;
using FluorineFx;
using System.Text.Json;

namespace CityVilleDotnet.Api.Services.UserService.pingFeedQuests;

internal sealed class PingFeedQuests(CityVilleDbContext context) : AmfService(context)
{
    public override async Task<ASObject> HandlePacket(object[] _params, Guid userId)
    {
        string jsonContent = File.ReadAllText("Resources/defaultQuests.json");

        var quest = JsonSerializer.Deserialize<UserQuest>(jsonContent);

        ASObject questObj = AmfConverter.JsonToASObject(JsonSerializer.Serialize(quest));

        return new CityVilleResponse(0, 333, questObj);
    }
}
