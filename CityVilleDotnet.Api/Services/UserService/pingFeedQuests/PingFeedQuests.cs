using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Services.QuestService.Domain;
using FluorineFx;
using System.Text.Json;

namespace CityVilleDotnet.Api.Services.UserService.pingFeedQuests;

internal sealed class PingFeedQuests : AmfService
{
    public override async Task<ASObject> HandlePacket(object[] _params)
    {
        string jsonContent = File.ReadAllText("Resources/defaultQuests.json");

        var quest = JsonSerializer.Deserialize<UserQuest>(jsonContent);

        ASObject questObj = AmfConverter.JsonToASObject(JsonSerializer.Serialize(quest));

        return new CityVilleResponse(0, 333, questObj);
    }
}
