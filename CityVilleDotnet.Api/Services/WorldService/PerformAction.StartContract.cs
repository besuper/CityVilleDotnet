using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.EnumExtensions;
using CityVilleDotnet.Domain.Enums;
using FluorineFx;

namespace CityVilleDotnet.Api.Services.WorldService;

internal sealed partial class PerformAction
{
    private async Task PerformStartContract(User user, object[] @params, Guid userId, CancellationToken cancellationToken)
    {
        var building = @params[1] as ASObject ?? throw new Exception("Building can't be null when action type is start contract");

        foreach (var item in building)
        {
            logger.LogDebug("{ItemKey} = {ItemValue}", item.Key, item.Value);
        }

        var position = building["position"] as ASObject ?? throw new Exception("Can't find position inside building element");
        var contractName = (string)building["contractName"];
        var plantTime = building.GetValueOrDefault("plantTime");
        var state = (string)building["state"];

        var obj = user.World?.GetBuildingByCoord(Convert.ToInt32(position["x"]), Convert.ToInt32(position["y"]), Convert.ToInt32(position["z"]));

        if (obj is null)
            throw new Exception("Can't find building with coords");

        obj.ContractName = contractName;
        obj.PlantTime = plantTime is null ? 0 : (double)plantTime;
        obj.State = EnumExtensions.ParseFromDescription<WorldObjectState>(state);

        user.HandleQuestsProgress("startContractByClass", className: obj.ClassName.ToString());
        user.CheckCompletedQuests();

        await context.SaveChangesAsync(cancellationToken);
    }
}