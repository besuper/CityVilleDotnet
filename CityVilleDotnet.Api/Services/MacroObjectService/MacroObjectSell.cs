using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.MacroObjectService;

public class MacroObjectSell(CityVilleDbContext context) : AmfService<MacroObjectSellRequest>
{
    public override async Task<ASObject> HandlePacket(MacroObjectSellRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects)
            .ThenInclude(x => x.MechanicCounters)
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.MacroObjects.Where(m => m.Name == request.MacroObjectName))
            .Include(x => x.InventoryItems)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("Player not found");

        var world = player.GetWorld();

        var macroObject = world.GetMacroObjectByName(request.MacroObjectName) ?? throw new DomainException(GameErrorType.InvalidData);

        var parentItem = GameSettingsManager.Instance.GetItem(macroObject.ParentItemName) ?? throw new Exception($"Can't find game item for {macroObject.ParentItemName}");

        var children = world.GetMacroObjectChildrenByClientIds(macroObject, request.SoldObjects);
        
        if (parentItem.IsSellSendsToInventory)
            player.AddItem(parentItem.Name);

        foreach (var child in children)
        {
            var childItemName = child.TargetBuildingName ?? child.ItemName;
            var childSendsToInventory = GameSettingsManager.Instance.GetItem(childItemName)?.IsSellSendsToInventory == true;

            if (parentItem.IsSellSendsToInventory && childSendsToInventory)
                player.AddItem(childItemName);
            else if (!parentItem.IsSellSendsToInventory && !childSendsToInventory)
                player.AddCoins(child.GetSellPrice());
        }

        world.RemoveMacroObject(macroObject, children);
        context.RemoveRange(children);
        context.Remove(macroObject);

        world.CalculatePopulation();

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class MacroObjectSellRequest
{
    [AmfParam(0)] public string MacroObjectName { get; set; } = string.Empty;
    [AmfParam(1)] public int[] SoldObjects { get; set; } = [];
}

public class MacroObjectSellValidator : AbstractValidator<MacroObjectSellRequest>
{
    public MacroObjectSellValidator()
    {
        RuleFor(x => x.MacroObjectName).NotEmpty().MaximumLength(64);
        RuleFor(x => x.SoldObjects).NotEmpty();
    }
}
