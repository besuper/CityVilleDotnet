using System.Collections;
using System.Reflection;
using System.Xml.Serialization;

namespace CityVilleDotnet.Common.Settings.GameSettings;

// Mirrors the client side XMLMergeAssistant: an item declaring derivesFrom inherits every
// element and attribute its parent declares, except the ones listed in doNotInherit.
internal static class GameItemInheritance
{
    private const string MechanicsXmlName = "mechanics";

    private static readonly List<(PropertyInfo Property, string XmlName)> InheritableProperties = typeof(GameItem)
        .GetProperties(BindingFlags.Public | BindingFlags.Instance)
        .Where(x => x.CanRead && x.CanWrite)
        .Select(x => (Property: x, XmlName: GetXmlName(x)))
        .Where(x => x.XmlName is not null and not ("name" or "derivesFrom" or "doNotInherit" or MechanicsXmlName))
        .Select(x => (x.Property, XmlName: x.XmlName!))
        .ToList();

    public static int Resolve(Dictionary<string, GameItem?> items)
    {
        var resolved = new HashSet<string>();

        foreach (var item in items.Values)
        {
            if (item is not null) Resolve(item, items, resolved);
        }

        return resolved.Count;
    }

    private static void Resolve(GameItem item, Dictionary<string, GameItem?> items, HashSet<string> resolved)
    {
        if (item.DerivesFrom is null) return;
        if (!items.TryGetValue(item.DerivesFrom, out var parent) || parent is null) return;
        if (!resolved.Add(item.Name)) return;

        Resolve(parent, items, resolved);

        var excluded = item.DoNotInherit?.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries) ?? [];

        if (!excluded.Contains(MechanicsXmlName))
            item.Mechanics = MergeMechanics(item.Mechanics, parent.Mechanics);

        foreach (var (property, xmlName) in InheritableProperties)
        {
            if (excluded.Contains(xmlName)) continue;

            if (!IsUnset(property, property.GetValue(item))) continue;

            var parentValue = property.GetValue(parent);

            // Some properties are backed by a string that turns a null into a default value when set,
            // so an unset parent must never be copied over
            if (IsUnset(property, parentValue)) continue;

            property.SetValue(item, parentValue);
        }
    }

    private static MechanicsContainer? MergeMechanics(MechanicsContainer? child, MechanicsContainer? parent)
    {
        if (parent?.GameEventMechanics is null) return child;
        if (child?.GameEventMechanics is null) return parent;

        var merged = new List<GameEventMechanicsItem>();

        foreach (var childBlock in child.GameEventMechanics)
        {
            var parentBlock = parent.GameEventMechanics.FirstOrDefault(x => x.GameMode == childBlock.GameMode);

            var inherited = parentBlock?.Mechanics?
                .Where(x => childBlock.Mechanics?.Any(c => c.Type == x.Type) != true)
                .ToList() ?? [];

            merged.Add(inherited.Count == 0
                ? childBlock
                : new GameEventMechanicsItem
                {
                    GameMode = childBlock.GameMode,
                    Mechanics = [.. childBlock.Mechanics ?? [], .. inherited]
                });
        }

        merged.AddRange(parent.GameEventMechanics.Where(x => child.GameEventMechanics.All(c => c.GameMode != x.GameMode)));

        return new MechanicsContainer { GameEventMechanics = merged };
    }

    private static bool IsUnset(PropertyInfo property, object? value)
    {
        if (value is null) return true;
        if (value is string text) return text.Length == 0;
        if (value is ICollection collection) return collection.Count == 0;

        // A non nullable value type can't tell an absent XML node from an explicit zero,
        // so its default value is the only marker of an absent node
        return Nullable.GetUnderlyingType(property.PropertyType) is null
               && property.PropertyType.IsValueType
               && value.Equals(Activator.CreateInstance(property.PropertyType));
    }

    private static string? GetXmlName(PropertyInfo property)
    {
        var element = property.GetCustomAttribute<XmlElementAttribute>();

        if (element is not null) return element.ElementName;

        return property.GetCustomAttribute<XmlAttributeAttribute>()?.AttributeName;
    }
}
