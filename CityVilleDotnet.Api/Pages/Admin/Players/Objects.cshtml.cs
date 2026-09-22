using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CityVilleDotnet.Api.Pages.Admin.Players;

public class ObjectsModel(CityVilleDbContext dbContext, IStringLocalizer<Resources.SharedResource> localizer) : PageModel
{
    private const int PageSize = 50;
    private const int MaxItemNameLength = 64;

    [BindProperty(SupportsGet = true)] public WorldType SelectedWorld { get; set; } = WorldType.Main;
    [BindProperty(SupportsGet = true)] public string? Search { get; set; }
    [BindProperty(SupportsGet = true)] public int CurrentPage { get; set; } = 1;

    public Guid PlayerId { get; set; }
    public string Username { get; set; } = string.Empty;
    public List<WorldType> AvailableWorlds { get; set; } = [];
    public List<ObjectRow> Objects { get; set; } = [];
    public int TotalCount { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

    public async Task<IActionResult> OnGetAsync(Guid id, CancellationToken ct)
    {
        var player = await dbContext.Set<Player>()
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new { x.Username, Worlds = x.Worlds.Select(w => w.Type).OrderBy(w => w).ToList() })
            .FirstOrDefaultAsync(ct);

        if (player is null)
            return NotFound();

        PlayerId = id;
        Username = player.Username;
        AvailableWorlds = player.Worlds;
        CurrentPage = Math.Max(1, CurrentPage);

        var query = dbContext.Set<World>()
            .AsNoTracking()
            .Where(x => x.Player!.Id == id && x.Type == SelectedWorld)
            .SelectMany(x => x.Objects);

        if (!string.IsNullOrWhiteSpace(Search))
        {
            var search = Search.Trim();
            query = query.Where(x => x.ItemName.Contains(search));
        }

        TotalCount = await query.CountAsync(ct);

        Objects = await query
            .OrderBy(x => x.WorldFlatId)
            .Skip((CurrentPage - 1) * PageSize)
            .Take(PageSize)
            .Select(x => new ObjectRow(x.WorldFlatId, x.ItemName, x.ClassName, x.State, x.X, x.Y, x.Z, x.Direction))
            .ToListAsync(ct);

        return Page();
    }

    public async Task<IActionResult> OnPostUpdateObjectAsync(Guid id, ObjectInput input, CancellationToken ct)
    {
        var itemName = input.ItemName?.Trim();

        if (string.IsNullOrEmpty(itemName) || itemName.Length > MaxItemNameLength || GameSettingsManager.Instance.GetItem(itemName) is null)
        {
            TempData["Error"] = localizer["AdminUnknownItem"].Value;
            return RedirectToList(id);
        }

        if (!Enum.IsDefined(input.ClassName) || !Enum.IsDefined(input.State) || input.Direction is < 0 or > 3)
        {
            TempData["Error"] = localizer["AdminInvalidValues"].Value;
            return RedirectToList(id);
        }

        var world = await LoadWorldAsync(id, ct);
        var obj = world?.GetBuildingById(input.WorldFlatId);

        if (world is null || obj is null)
            return NotFound();

        obj.ReplaceWith(itemName, input.ClassName, input.Direction, input.X, input.Y, input.Z, input.State);
        world.CalculatePopulation();

        await dbContext.SaveChangesAsync(ct);

        TempData["Success"] = localizer["AdminObjectUpdated"].Value;
        return RedirectToList(id);
    }

    public async Task<IActionResult> OnPostDeleteObjectAsync(Guid id, int worldFlatId, CancellationToken ct)
    {
        var world = await LoadWorldAsync(id, ct);
        var obj = world?.GetBuildingById(worldFlatId);

        if (world is null || obj is null)
            return NotFound();

        world.RemoveBuilding(obj);

        if (obj.FranchiseLocation is not null && obj.ItemOwner is not null)
        {
            var owner = await dbContext.Set<Player>()
                .AsSplitQuery()
                .Include(x => x.Franchises)
                .ThenInclude(x => x.Locations)
                .FirstOrDefaultAsync(x => x.Snuid.ToString() == obj.ItemOwner, ct);

            owner?.RemoveFranchiseLocation(world.Player!.Snuid.ToString(), obj.WorldFlatId);
        }

        dbContext.Remove(obj);
        world.CalculatePopulation();

        await dbContext.SaveChangesAsync(ct);

        TempData["Success"] = localizer["AdminObjectDeleted"].Value;
        return RedirectToList(id);
    }

    private Task<World?> LoadWorldAsync(Guid id, CancellationToken ct)
    {
        return dbContext.Set<World>()
            .AsSplitQuery()
            .Include(x => x.Player)
            .Include(x => x.Objects)
            .ThenInclude(x => x.MechanicCounters)
            .Include(x => x.Objects)
            .ThenInclude(x => x.FranchiseLocation)
            .FirstOrDefaultAsync(x => x.Player!.Id == id && x.Type == SelectedWorld, ct);
    }

    private RedirectToPageResult RedirectToList(Guid id)
    {
        return RedirectToPage("/Admin/Players/Objects", new { id, SelectedWorld, Search, CurrentPage });
    }

    public record ObjectRow(int WorldFlatId, string ItemName, BuildingClassType ClassName, WorldObjectState State, int X, int Y, int? Z, int Direction);

    public class ObjectInput
    {
        public int WorldFlatId { get; set; }
        public string? ItemName { get; set; }
        public BuildingClassType ClassName { get; set; }
        public WorldObjectState State { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int? Z { get; set; }
        public int Direction { get; set; }
    }
}
