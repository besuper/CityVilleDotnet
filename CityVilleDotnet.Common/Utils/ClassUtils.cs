namespace CityVilleDotnet.Common.Utils;

public static class ClassUtils
{
    public static Dictionary<string, object> ToDictionary(this object obj)
    {
        var result = new Dictionary<string, object>();
        var properties = obj.GetType().GetProperties();
    
        foreach (var prop in properties)
        {
            var value = prop.GetValue(obj)?.ToString();
            if (value == null) continue;
        
            if (int.TryParse(value, out int intValue))
                result[prop.Name] = intValue;
            else if (double.TryParse(value, out double doubleValue))
                result[prop.Name] = doubleValue;
            else
                result[prop.Name] = value;
        }
    
        return result;
    }
}