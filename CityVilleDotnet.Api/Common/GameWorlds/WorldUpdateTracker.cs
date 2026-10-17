using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.GameEntities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CityVilleDotnet.Api.Common.GameWorlds;

// Allows the client to have an up to date world cache. We patch it with metadata.worldUpdate (WorldStateTransactionListener).
public sealed class WorldUpdateTracker : IDisposable
{
    private const string WorldIdKey = "WorldId";
    private const string WorldObjectIdKey = "WorldObjectId";

    private readonly CityVilleDbContext _context;
    private readonly HashSet<WorldObject> _changedObjects = [];
    private readonly HashSet<int> _changedObjectIds = [];
    private readonly List<(int WorldId, int FlatId)> _removedObjects = [];
    private readonly HashSet<int> _mapRectWorldIds = [];
    private readonly HashSet<int> _macroWorldIds = [];
    private readonly List<string> _removedMacroNames = [];
    private World? _renamedWorld;

    public WorldUpdateTracker(CityVilleDbContext context)
    {
        _context = context;
        _context.SavingChanges += OnSavingChanges;
    }

    public void Dispose()
    {
        _context.SavingChanges -= OnSavingChanges;
    }

    private void OnSavingChanges(object? sender, SavingChangesEventArgs e)
    {
        foreach (var entry in _context.ChangeTracker.Entries().Where(x => x.State is EntityState.Added or EntityState.Modified or EntityState.Deleted))
        {
            switch (entry.Entity)
            {
                case WorldObject obj:
                    TrackObject(entry, obj);
                    break;
                case WorldObjectMechanicCounter or WorldObjectStorageItem or WorldObjectSlot:
                    if (GetForeignKey(entry, WorldObjectIdKey) is { } objectId) _changedObjectIds.Add(objectId);
                    break;
                case MapRect:
                    if (GetForeignKey(entry, WorldIdKey) is { } mapRectWorldId) _mapRectWorldIds.Add(mapRectWorldId);
                    break;
                case MacroObject macro:
                    if (GetForeignKey(entry, WorldIdKey) is { } macroWorldId) _macroWorldIds.Add(macroWorldId);
                    if (entry.State == EntityState.Deleted) _removedMacroNames.Add(macro.Name);
                    break;
                case World world when entry.State == EntityState.Modified && entry.Property(nameof(World.WorldName)).IsModified:
                    _renamedWorld = world;
                    break;
            }
        }
    }

    private void TrackObject(EntityEntry entry, WorldObject obj)
    {
        var worldId = entry.Property(WorldIdKey);
        var originalWorldId = entry.State == EntityState.Added ? null : (int?)worldId.OriginalValue;

        if (originalWorldId is not null && (entry.State == EntityState.Deleted || !Equals(worldId.CurrentValue, worldId.OriginalValue)))
            _removedObjects.Add((originalWorldId.Value, (int)entry.Property(nameof(WorldObject.WorldFlatId)).OriginalValue!));

        if (originalWorldId is not null && entry.Property(nameof(WorldObject.ParentMacroObjectId)).OriginalValue is not null)
            _macroWorldIds.Add(originalWorldId.Value);

        if (entry.State != EntityState.Deleted)
            _changedObjects.Add(obj);
    }

    private static int? GetForeignKey(EntityEntry entry, string name)
    {
        var property = entry.Property(name);

        if (property.IsTemporary) return null;

        return (int?)(entry.State == EntityState.Deleted ? property.OriginalValue : property.CurrentValue);
    }

    public async Task AppendToAsync(ASObject response, CancellationToken cancellationToken)
    {
        _changedObjectIds.UnionWith(_changedObjects.Select(x => x.Id));

        if (_changedObjectIds.Count == 0 && _removedObjects.Count == 0 && _mapRectWorldIds.Count == 0 && _macroWorldIds.Count == 0 && _renamedWorld is null)
            return;

        var objectWorldIds = await _context.Set<WorldObject>()
            .AsNoTracking()
            .Where(x => _changedObjectIds.Contains(x.Id) && EF.Property<int?>(x, WorldIdKey) != null)
            .Select(x => EF.Property<int>(x, WorldIdKey))
            .Distinct()
            .ToListAsync(cancellationToken);

        var worldIds = objectWorldIds
            .Concat(_removedObjects.Select(x => x.WorldId))
            .Concat(_mapRectWorldIds)
            .Concat(_macroWorldIds)
            .Concat(_renamedWorld is null ? [] : new[] { _renamedWorld.Id })
            .Distinct()
            .ToList();

        // we don't know which world the client tagged the transaction with, so a transaction touching several worlds sends nothing
        if (worldIds.Count != 1) return;

        var worldUpdate = new ASObject();

        if (_changedObjectIds.Count > 0 || _removedObjects.Count > 0)
            worldUpdate["objects"] = await BuildObjectsAsync(cancellationToken);

        if (_mapRectWorldIds.Count > 0)
            worldUpdate["mapRects"] = await BuildMapRectsAsync(worldIds[0], cancellationToken);

        if (_macroWorldIds.Count > 0 || _changedObjects.Any(x => x.ParentMacroObjectId is not null))
            worldUpdate["macroObjects"] = await BuildMacroObjectsAsync(worldIds[0], cancellationToken);

        if (_renamedWorld is not null)
            worldUpdate["name"] = _renamedWorld.WorldName;

        ((ASObject)response["metadata"])["worldUpdate"] = worldUpdate;
    }

    private async Task<ASObject> BuildObjectsAsync(CancellationToken cancellationToken)
    {
        var objects = await _context.Set<WorldObject>()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.MechanicCounters)
            .Include(x => x.StorageItems)
            .Include(x => x.Slots)
            .Include(x => x.FranchiseLocation)
            .Where(x => _changedObjectIds.Contains(x.Id) && EF.Property<int?>(x, WorldIdKey) != null)
            .ToListAsync(cancellationToken);

        var result = new ASObject();

        foreach (var (_, flatId) in _removedObjects)
        {
            result[flatId.ToString()] = null;
        }

        foreach (var obj in objects)
        {
            result[obj.WorldFlatId.ToString()] = AmfConverter.Convert(obj.ToDto());
        }

        return result;
    }

    private async Task<object?> BuildMapRectsAsync(int worldId, CancellationToken cancellationToken)
    {
        var mapRects = await _context.Set<MapRect>()
            .AsNoTracking()
            .Where(x => EF.Property<int>(x, WorldIdKey) == worldId)
            .ToListAsync(cancellationToken);

        return AmfConverter.Convert(mapRects.Select(x => x.ToDto()).ToList());
    }

    private async Task<ASObject> BuildMacroObjectsAsync(int worldId, CancellationToken cancellationToken)
    {
        var world = await _context.Set<World>()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.MacroObjects)
            .Include(x => x.Objects.Where(o => o.ParentMacroObjectId != null))
            .FirstAsync(x => x.Id == worldId, cancellationToken);

        var macroObjects = world.ToMacroObjectsAsObject();

        foreach (var name in _removedMacroNames)
        {
            macroObjects[name] = null;
        }

        return macroObjects;
    }
}
