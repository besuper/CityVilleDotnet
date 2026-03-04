using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.EnumExtensions;
using CityVilleDotnet.Domain.Enums;
using FluorineFx;

namespace CityVilleDotnet.Api.Services.WorldService;

internal sealed partial class PerformAction
{
    private async Task<CityVilleResponse> PerformStartContract(User user, object[] @params, Guid userId, CancellationToken cancellationToken)
    {
        var building = @params[1] as ASObject ?? throw new Exception("Building can't be null when action type is start contract");

        foreach (var item in building)
        {
            logger.LogDebug("{ItemKey} = {ItemValue}", item.Key, item.Value);
        }

        var position = building["position"] as ASObject ?? throw new Exception("Can't find position inside building element");
        var contractName = (string)building["contractName"];
        var state = (string)building["state"];

        var obj = user.World?.GetBuildingByCoord(Convert.ToInt32(position["x"]), Convert.ToInt32(position["y"]), Convert.ToInt32(position["z"]));

        if (obj is null)
            throw new Exception("Can't find building with coords");

        var contractItem = GameSettingsManager.Instance.GetItem(contractName);

        if (contractItem is null)
            throw new Exception($"Can't find item with contractName {contractName}");
        
        if (contractItem.Cost is not null)
        {
            if (contractItem.Cost > user.Player!.Gold)
                return new CityVilleResponse().Error(GameErrorType.NotEnoughMoney);

            user.Player!.RemoveCoins(contractItem.Cost.Value);
        }

        obj.StartContract(contractName, EnumExtensions.ParseFromDescription<WorldObjectState>(state));

        user.HandleQuestsProgress("startContractByClass", className: obj.ClassName.ToString());
        user.CheckCompletedQuests();

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse();
    }
}