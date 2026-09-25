using System.Text.Json;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.EnumExtensions;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Domain.GameEntities;
using CityVilleDotnet.Persistence;
using Humanizer;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Common.GameWorlds;

public static class GameWorldFactory
{
    private const string FtueHouseItemName = "res_portal2";
    private const int WorldSize = 36;
    private const int MapRectSize = 12;

    public static async Task<bool> EnsureCreatedAsync(CityVilleDbContext context, Guid playerId, int ownerSnuid, WorldType type, CancellationToken cancellationToken)
    {
        if (type is not (WorldType.Downtown or WorldType.Lakefront)) return false;

        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Worlds.Where(w => w.Type == type || w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(w => w.Objects.Where(o => o.EnergyModifier > 0))
            .FirstOrDefaultAsync(x => x.Id == playerId && x.Snuid == ownerSnuid, cancellationToken);

        if (player is null) return false;

        if (player.GetWorldByType(type) is not null) return false;

        var world = type == WorldType.Downtown
            ? await CreateDowntownAsync(cancellationToken)
            : CreateLakefront();

        world.CalculatePopulation();
        player.AddWorld(world);

        // client applies the ftueGrants on firstTimeLoaded
        var worldConfig = GameSettingsManager.Instance.GetWorldConfig(type.ToDescriptionString());

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

    private static async Task<World> CreateDowntownAsync(CancellationToken cancellationToken)
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

    private static World CreateLakefront()
    {
        var worldRect = GameSettingsManager.Instance.GetWorldRect(WorldType.Lakefront.ToDescriptionString())
                        ?? throw new Exception("Can't find world_lakefront worldRect");

        var mapRects = new List<MapRect>();

        foreach (var rect in worldRect.MapRects)
        {
            for (var x = rect.X; x < rect.X + rect.Width; x += MapRectSize)
            {
                for (var y = rect.Y; y < rect.Y + rect.Height; y += MapRectSize)
                {
                    mapRects.Add(new MapRect { X = x, Y = y, Width = MapRectSize, Height = MapRectSize });
                }
            }
        }

        var objects = new List<WorldObject>();

        foreach (var rectObj in worldRect.Objects.Objects)
        {
            var item = GameSettingsManager.Instance.GetItem(rectObj.ItemName)
                       ?? throw new Exception($"Can't find item {rectObj.ItemName} from world_lakefront worldRect");

            var className = Enum.Parse<BuildingClassType>(item.Type.Pascalize());

            objects.Add(WorldObject.CreateFromWorldRect(rectObj, className, -1, rectObj.X, rectObj.Y, 0, objects.Count + 1));
        }

        return new World("LakeFront", WorldSize, WorldSize, 0, 0, 0, 0, 0, mapRects, objects, WorldType.Lakefront);
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
