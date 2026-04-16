namespace CityVilleDotnet.Common.Utils;

public static class ClassUtils
{
    [Obsolete("Remove this after removing _settings")]
    public static Dictionary<string, object> ToDictionary(this object obj)
    {
        var result = new Dictionary<string, object>();
        var properties = obj.GetType().GetProperties();
    
        foreach (var prop in properties)
        {
            var value = prop.GetValue(obj);
            if (value == null) continue;
            
            result[prop.Name] = value;
        }
    
        return result;
    }
}