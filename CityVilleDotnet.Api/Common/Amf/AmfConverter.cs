using FluorineFx;
using System.Collections;
using System.Collections.Concurrent;
using System.Reflection;
using System.Text.Json.Serialization;

namespace CityVilleDotnet.Api.Common.Amf;

public static class AmfConverter
{
    public static object? Convert(object? obj)
    {
        if (obj is null)
            return null;

        // Convert Dictionary with DTOs instead of only creating a new ASObject
        if (obj is IDictionary dict && IsStringKeyedDictionary(obj.GetType()))
            return ConvertDictionary(dict);

        if (obj is ASObject asObject)
        {
            foreach (var key in new List<string>(asObject.Keys))
            {
                asObject[key] = Convert(asObject[key]);
            }

            return asObject;
        }

        if (IsSimpleType(obj.GetType()))
            return obj;

        if (obj is IEnumerable arrayList)
            return ConvertToList(arrayList);

        return ConvertToAsObject(obj);
    }

    private static ASObject ConvertDictionary(IDictionary dict)
    {
        var result = new ASObject();

        foreach (DictionaryEntry entry in dict)
        {
            result[(string)entry.Key] = Convert(entry.Value);
        }

        return result;
    }

    private static bool IsStringKeyedDictionary(Type type)
    {
        return type.IsGenericType
               && type.GetGenericTypeDefinition() == typeof(Dictionary<,>)
               && type.GetGenericArguments()[0] == typeof(string);
    }

    private static ASObject ConvertToAsObject(object obj)
    {
        var result = new ASObject();

        foreach (var property in PropertyCache.GetOrAdd(obj.GetType(), BuildProperties))
        {
            var value = property.Property.GetValue(obj);

            if (value is null)
            {
                // the client tests some keys with "in": a null value must not create the key
                if (property.SkipWhenNull) continue;

                result[property.Name] = null;
                continue;
            }

            result[property.Name] = Convert(value);
        }

        return result;
    }

    private static readonly ConcurrentDictionary<Type, AmfProperty[]> PropertyCache = new();

    private static AmfProperty[] BuildProperties(Type type)
    {
        var result = new List<AmfProperty>();

        foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var jsonIgnore = property.GetCustomAttribute<JsonIgnoreAttribute>();

            if (jsonIgnore is not null && jsonIgnore.Condition != JsonIgnoreCondition.WhenWritingNull) continue;

            var name = property.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name ?? property.Name;

            result.Add(new AmfProperty(property, name, jsonIgnore is not null));
        }

        return result.ToArray();
    }

    private sealed record AmfProperty(PropertyInfo Property, string Name, bool SkipWhenNull);

    private static List<object?> ConvertToList(IEnumerable collection)
    {
        var result = new List<object?>();

        foreach (var item in collection)
        {
            result.Add(Convert(item));
        }

        return result;
    }

    private static bool IsSimpleType(Type type)
    {
        return type.IsPrimitive
               || type == typeof(string)
               || type == typeof(decimal)
               || type == typeof(DateTime)
               || type == typeof(TimeSpan)
               || type == typeof(Guid)
               || type.IsEnum;
    }
}