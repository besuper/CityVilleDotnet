using System.ComponentModel;

namespace CityVilleDotnet.Domain.EnumExtensions;

public static class EnumExtensions
{
    public static string ToDescriptionString(this Enum val)
    {
        var field = val.GetType().GetField(val.ToString());

        if (field is null) return val.ToString();

        var attributes = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);

        return attributes.Length > 0 ? attributes[0].Description : string.Empty;
    }

    public static T ParseFromDescription<T>(string description) where T : struct, Enum
    {
        foreach (var field in typeof(T).GetFields())
        {
            if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
            {
                if (attribute.Description == description)
                    return (T)field.GetValue(null)!;
            }
        }

        throw new ArgumentException($"No enum value found with description '{description}'", nameof(description));
    }
}