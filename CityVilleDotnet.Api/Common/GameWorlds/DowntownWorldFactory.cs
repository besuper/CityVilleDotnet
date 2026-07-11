using System.Text.Json;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.EnumExtensions;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Domain.GameEntities;
using CityVilleDotnet.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Common.GameWorlds;

public static class DowntownWorldFactory
{
    private const string FtueHouseItemName = "res_portal2";

    public static async Task<bool> EnsureCreatedAsync(CityVilleDbContext context, Guid playerId, int ownerSnuid, WorldType type, CancellationToken cancellationToken)
    {
        if (type != WorldType.Downtown) return false;

        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Worlds.Where(w => w.Type == WorldType.Downtown || w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(w => w.Objects.Where(o => o.EnergyModifier > 0))
            .FirstOrDefaultAsync(x => x.Id == playerId && x.Snuid == ownerSnuid, cancellationToken);

        if (player is null) return false;

        if (player.GetWorldByType(WorldType.Downtown) is not null) return false;

        var world = await CreateAsync(cancellationToken);
        player.AddWorld(world);

        // client applies the ftueGrants on firstTimeLoaded
        var worldConfig = GameSettingsManager.Instance.GetWorldConfig(WorldType.Downtown.ToDescriptionString());

        foreach (var grant in worldConfig?.FtueGrants ?? [])
        {
            switch (grant.Type)
            {
                case "gold":
                    player.AddCoins(grant.Value);
                    break;
                case "energy":
                    player.AddEnergy(grant.Value);
                    break;
                case "goods":
                    player.AddGoods(grant.Value);
                    break;
            }
        }

        await context.SaveChangesAsync(cancellationToken);

        return true;
    }

    private static async Task<World> CreateAsync(CancellationToken cancellationToken)
    {
        var jsonContent = await File.ReadAllTextAsync("Resources/startWorldDowntown.json", cancellationToken);
        var layout = JsonSerializer.Deserialize<WorldDto>(jsonContent) ?? throw new Exception("Downtown WorldDto can't be null");

        var mapRects = layout.MapRects.Select(x => new MapRect()
        {
            Height = x.Height,
            Width = x.Width,
            X = x.X,
            Y = x.Y,
        }).ToList();

        var objects = layout.Objects.Select(x => new WorldObject().LoadObject(x)).ToList();

        SetupFtueHouse(objects);

        var world = new World("Downtown", layout.SizeX, layout.SizeY, 0, 0, 0, 0, 0, mapRects, objects, WorldType.Downtown);

        // starts the downtown ftue guide from this marker until WorldService.completeTutorial clears it
        world.SetWorldCreated(WorldType.Downtown.ToDescriptionString());

        return world;
    }

    private static void SetupFtueHouse(List<WorldObject> objects)
    {
        var house = objects.FirstOrDefault(x => x.ItemName == FtueHouseItemName);

        if (house is null) throw new Exception($"Downtown layout is missing the {FtueHouseItemName} FTUE house");

        var houseItem = GameSettingsManager.Instance.GetItem(FtueHouseItemName);

        if (houseItem?.Construction is null)
            throw new Exception($"Can't find construction item for {FtueHouseItemName}");

        var constructionItem = GameSettingsManager.Instance.GetItem(houseItem.Construction);

        if (constructionItem?.NumberOfStages is null)
            throw new Exception($"Construction item not found with {houseItem.Construction}");

        house.SetAsConstructionSite(houseItem.Construction, constructionItem.NumberOfStages.Value);

        for (var stage = 1; stage < constructionItem.NumberOfStages.Value; stage++)
            house.AddConstructionStage();
    }
}
