using FluorineFx;
using System.Collections;
using System.Reflection;
using System.Text.Json.Serialization;

namespace CityVilleDotnet.Api.Common.Amf;

public static class AmfConverter
{
    public static object? Convert(object? obj)
    {
        if (obj is null)
            return null;

        if (obj is ASObject || IsSimpleType(obj.GetType()))
            return obj;

        if (obj is IEnumerable arrayList)
            return ConvertToList(arrayList);

        return ConvertToAsObject(obj);
    }

    private static ASObject ConvertToAsObject(object obj)
    {
        var result = new ASObject();

        var type = obj.GetType();
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            if (property.GetCustomAttribute<JsonIgnoreAttribute>() != null) continue;

            var jsonPropertyNameAttr = property.GetCustomAttribute<JsonPropertyNameAttribute>();
            var propertyName = jsonPropertyNameAttr != null
                ? jsonPropertyNameAttr.Name
                : property.Name;

            var value = property.GetValue(obj);

            if (value is null)
            {
                result[propertyName] = null;
                continue;
            }

            result[propertyName] = Convert(value);
        }

        return result;
    }

    private static List<object?> ConvertToList(IEnumerable collection)
    {
        var result = new List<object?>();

        foreach (var item in collection)
        {
            if (item is null)
            {
                result.Add(null);
            }
            else if (item is ASObject || IsSimpleType(item.GetType()))
            {
                result.Add(item);
            }
            else if (item is IEnumerable enumerable)
            {
                result.Add(ConvertToList(enumerable));
            }
            else
            {
                result.Add(ConvertToAsObject(item));
            }
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