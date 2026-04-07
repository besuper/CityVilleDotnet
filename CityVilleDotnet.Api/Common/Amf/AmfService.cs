using System.Collections;
using System.Collections.Concurrent;
using System.Reflection;
using CityVilleDotnet.Domain.EnumExtensions;
using CityVilleDotnet.Domain.Enums;
using FluentValidation;
using FluorineFx;

namespace CityVilleDotnet.Api.Common.Amf;

public class AmfService
{
    internal IServiceProvider? ServiceProvider { get; set; }

    public virtual Task<ASObject> HandlePacket(object[] @params, Guid userId, CancellationToken cancellationToken)
    {
        throw new Exception("Not implemented");
    }
}

public abstract class AmfService<TRequest> : AmfService where TRequest : new()
{
    private static readonly AmfPropertyInfo[] CachedProperties = BuildPropertyCache(typeof(TRequest));

    public override async Task<ASObject> HandlePacket(object[] @params, Guid userId, CancellationToken cancellationToken)
    {
        var request = MapRequest(@params);

        if (ServiceProvider?.GetService(typeof(IValidator<TRequest>)) is IValidator<TRequest> validator)
        {
            var result = await validator.ValidateAsync(request, cancellationToken);

            if (!result.IsValid)
                throw new ValidationException(result.Errors);
        }

        return await HandlePacket(request, userId, cancellationToken);
    }

    public abstract Task<ASObject> HandlePacket(TRequest request, Guid userId, CancellationToken cancellationToken);

    private static TRequest MapRequest(object[] @params)
    {
        var request = new TRequest();
        var requestType = typeof(TRequest);

        foreach (var info in CachedProperties)
        {
            if (info.Index < 0) continue;

            if (info.Index >= @params.Length)
            {
                if (info.IsRequired)
                    throw new ArgumentException($"Required property '{info.Property.Name}' on '{requestType.Name}' is missing.");
                continue;
            }

            var value = @params[info.Index];
            SetPropertyValue(request, info, value, requestType);
        }
        return request;
    }

    private static object? MapFromDictionary(IDictionary dictionary, Type targetType)
    {
        var instance = Activator.CreateInstance(targetType)!;
        var properties = GetOrBuildPropertyCache(targetType);

        foreach (var info in properties)
        {
            if (info.Key is null) continue;

            if (!dictionary.Contains(info.Key))
            {
                if (info.IsRequired)
                    throw new ArgumentException($"Required property '{info.Property.Name}' on '{targetType.Name}' is missing.");
                continue;
            }

            var value = dictionary[info.Key];
            SetPropertyValue(instance, info, value, targetType);
        }
        return instance;
    }

    private static void SetPropertyValue(object instance, AmfPropertyInfo info, object? value, Type ownerType)
    {
        if (value is null)
        {
            if (info.IsRequired)
                throw new ArgumentException($"Required property '{info.Property.Name}' on '{ownerType.Name}' is null.");

            if (info.Property.PropertyType.IsValueType && Nullable.GetUnderlyingType(info.Property.PropertyType) is null)
                return;
        }

        info.Property.SetValue(instance, ConvertValue(value, info.Property.PropertyType));
    }

    private static readonly ConcurrentDictionary<Type, AmfPropertyInfo[]> PropertyCache = new();

    private static AmfPropertyInfo[] GetOrBuildPropertyCache(Type type)
    {
        return PropertyCache.GetOrAdd(type, BuildPropertyCache);
    }

    private static AmfPropertyInfo[] BuildPropertyCache(Type type)
    {
        var nullabilityContext = new NullabilityInfoContext();
        var properties = type.GetProperties();
        var result = new List<AmfPropertyInfo>(properties.Length);

        foreach (var prop in properties)
        {
            var attr = prop.GetCustomAttribute<AmfParamAttribute>();
            if (attr is null) continue;

            var isRequired = IsRequired(prop, nullabilityContext);

            result.Add(new AmfPropertyInfo
            {
                Property = prop,
                Index = attr.Index,
                Key = attr.Key,
                IsRequired = isRequired,
                IsMappable = IsMappableClass(prop.PropertyType)
            });
        }

        return result.ToArray();
    }

    private static bool IsRequired(PropertyInfo prop, NullabilityInfoContext nullabilityContext)
    {
        if (Nullable.GetUnderlyingType(prop.PropertyType) is not null)
            return false;

        if (prop.PropertyType.IsValueType)
            return true;

        return nullabilityContext.Create(prop).WriteState == NullabilityState.NotNull;
    }

    private static bool IsMappableClass(Type type)
    {
        if (type.IsValueType || type == typeof(string) || typeof(IDictionary).IsAssignableFrom(type)
            || type == typeof(object) || type.IsArray)
            return false;

        return type.GetProperties().Any(p => p.GetCustomAttribute<AmfParamAttribute>() is not null);
    }

    private static object? ConvertValue(object? value, Type targetType)
    {
        if (value is null) return null;

        var underlyingType = Nullable.GetUnderlyingType(targetType);
        if (underlyingType is not null)
            targetType = underlyingType;

        if (value is IDictionary dict && IsMappableClass(targetType))
            return MapFromDictionary(dict, targetType);

        if (targetType.IsInstanceOfType(value)) return value;

        if (targetType.IsEnum)
        {
            if (value is not string s)
                return Enum.ToObject(targetType, Convert.ToInt32(value));

            if (targetType == typeof(WorldObjectState))
                return EnumExtensions.ParseFromDescription<WorldObjectState>(s);
            
            if (targetType == typeof(WorldType))
                return EnumExtensions.ParseFromDescription<WorldType>(s);

            return Enum.Parse(targetType, s);
        }

        if (targetType == typeof(int)) return Convert.ToInt32(value);
        if (targetType == typeof(long)) return Convert.ToInt64(value);
        if (targetType == typeof(float)) return Convert.ToSingle(value);
        if (targetType == typeof(double)) return Convert.ToDouble(value);
        if (targetType == typeof(bool)) return Convert.ToBoolean(value);
        if (targetType == typeof(string)) return Convert.ToString(value);

        if (targetType.IsArray && value is Array sourceArray)
        {
            var elementType = targetType.GetElementType()!;
            var typedArray = Array.CreateInstance(elementType, sourceArray.Length);
            
            for (var i = 0; i < sourceArray.Length; i++)
                typedArray.SetValue(ConvertValue(sourceArray.GetValue(i), elementType), i);
            
            return typedArray;
        }

        return value;
    }

    private sealed class AmfPropertyInfo
    {
        public required PropertyInfo Property { get; init; }
        public int Index { get; init; } = -1;
        public string? Key { get; init; }
        public bool IsRequired { get; init; }
        public bool IsMappable { get; init; }
    }
}
