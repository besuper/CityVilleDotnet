using FluorineFx;
using System.Collections;
using System.Text.Json;

namespace CityVilleDotnet.Api.Common.Amf;

public class AmfConverter
{
    public static ASObject JsonToASObject(string json)
    {
        try
        {
            using JsonDocument doc = JsonDocument.Parse(json);
            return ConvertJsonElementToASObject(doc.RootElement);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}");
            return new ASObject();
        }
    }

    public static ASObject ConvertJsonElementToASObject(JsonElement element)
    {
        var result = new ASObject();

        if (element.ValueKind == JsonValueKind.Object)
        {
            foreach (var property in element.EnumerateObject())
            {
                switch (property.Value.ValueKind)
                {
                    case JsonValueKind.Object:
                        result[property.Name] = ConvertJsonElementToASObject(property.Value);
                        break;
                    case JsonValueKind.Array:
                        result[property.Name] = ConvertJsonElementToArrayList(property.Value);
                        break;
                    case JsonValueKind.String:
                        result[property.Name] = property.Value.GetString();
                        break;
                    case JsonValueKind.Number:
                        if (property.Value.TryGetInt32(out int intValue))
                            result[property.Name] = intValue;
                        else if (property.Value.TryGetDouble(out double doubleValue))
                            result[property.Name] = doubleValue;
                        else
                            result[property.Name] = property.Value.GetRawText();
                        break;
                    case JsonValueKind.True:
                        result[property.Name] = true;
                        break;
                    case JsonValueKind.False:
                        result[property.Name] = false;
                        break;
                    case JsonValueKind.Null:
                        result[property.Name] = null;
                        break;
                    default:
                        result[property.Name] = property.Value.GetRawText();
                        break;
                }
            }
        }

        return result;
    }

    public static ArrayList ConvertJsonElementToArrayList(JsonElement element)
    {
        var result = new ArrayList();

        foreach (var item in element.EnumerateArray())
        {
            switch (item.ValueKind)
            {
                case JsonValueKind.Object:
                    result.Add(ConvertJsonElementToASObject(item));
                    break;
                case JsonValueKind.Array:
                    result.Add(ConvertJsonElementToArrayList(item));
                    break;
                case JsonValueKind.String:
                    result.Add(item.GetString());
                    break;
                case JsonValueKind.Number:
                    if (item.TryGetInt32(out int intValue))
                        result.Add(intValue);
                    else if (item.TryGetDouble(out double doubleValue))
                        result.Add(doubleValue);
                    else
                        result.Add(item.GetRawText());
                    break;
                case JsonValueKind.True:
                    result.Add(true);
                    break;
                case JsonValueKind.False:
                    result.Add(false);
                    break;
                case JsonValueKind.Null:
                    result.Add(null);
                    break;
                default:
                    result.Add(item.GetRawText());
                    break;
            }
        }

        return result;
    }
}