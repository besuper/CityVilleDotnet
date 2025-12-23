using System.Collections;

namespace CityVilleDotnet.Api.Common.Extensions;

public static class ObjectReaderExtension
{
    public static object[] GetObjectArray(this object[] value, int index)
    {
        if (index < 0 || index >= value.Length)
            throw new Exception("Index out of range");

        var content = value[index];

        return content switch
        {
            // RUFFLE: Support ruffle sending dict instead of array
            IDictionary => ((Dictionary<string, object>)content).Values.ToArray(),
            object[] objectArray => objectArray,
            _ => throw new Exception("Invalid content type")
        };
    }
}