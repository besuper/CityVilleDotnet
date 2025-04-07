using FluorineFx;
using System.Collections;
using System.Reflection;
using System.Text.Json.Serialization;

namespace CityVilleDotnet.Api.Common.Amf;

public class AmfConverter
{
    public static object Convert(object obj)
    {
        if (obj == null)
            return null;

        if (obj is ASObject)
            return obj;

        if (IsSimpleType(obj.GetType()))
        {
            return obj;
        }

        if (obj is IEnumerable arrayList)
        {
            return ConvertToArrayList(arrayList);
        }

        return ConvertToASObject(obj);
    }

    public static ASObject ConvertToASObject(object obj)
    {
        var result = new ASObject();

        if (obj == null)
            return result;

        Type type = obj.GetType();
        PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (PropertyInfo property in properties)
        {
            if (property.GetCustomAttribute<JsonIgnoreAttribute>() != null)
            {
                continue;
            }

            var jsonPropertyNameAttr = property.GetCustomAttribute<JsonPropertyNameAttribute>();
            string propertyName = jsonPropertyNameAttr != null
                ? jsonPropertyNameAttr.Name
                : property.Name;

            object value = property.GetValue(obj);
            result[propertyName] = Convert(value);
        }

        return result;
    }

    private static ArrayList ConvertToArrayList(IEnumerable collection)
    {
        var result = new ArrayList();

        foreach (var item in collection)
        {
            if (item == null)
            {
                result.Add(null);
            }
            else if (IsSimpleType(item.GetType()))
            {
                result.Add(item);
            }
            else if (item is IEnumerable enumerable)
            {
                result.Add(ConvertToArrayList(enumerable));
            }
            else
            {
                result.Add(ConvertToASObject(item));
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