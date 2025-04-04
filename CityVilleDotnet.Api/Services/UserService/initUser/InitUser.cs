using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Services.UserService.Domain;
using FluorineFx;
using System.Text.Json;

namespace CityVilleDotnet.Api.Services.UserService.initUser;

internal sealed class InitUser : AmfService
{
    public override async Task<ASObject> HandlePacket(object[] _params)
    {
        string jsonContent = File.ReadAllText("Resources/defaultUser.json");

        var user = JsonSerializer.Deserialize<User>(jsonContent);
        //user.UserInfo.Player.Gold = 55;

        ASObject userObj = AmfConverter.JsonToASObject(JsonSerializer.Serialize(user));

        var response = new CityVilleResponse(333, userObj);

        return response;
    }
}