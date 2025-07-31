using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace JiraClient.Mapping;

public static class DynamicMapper
{
    public static TTarget Map<TTarget>(object source, IDictionary<string, string> mapping)
        where TTarget : new()
    {
        var target = new TTarget();
        Map(source, target, mapping);
        return target;
    }

    public static void Map(object source, object target, IDictionary<string, string> mapping)
    {
        foreach (var kvp in mapping)
        {
            var targetPath = kvp.Key.Split('.');
            var sourcePath = kvp.Value.Split('.');
            var value = GetValue(source, sourcePath, 0);
            SetValue(target, targetPath, 0, value);
        }
    }

    private static object? GetValue(object? obj, string[] path, int index)
    {
        if (obj is null)
            return null;

        if (index >= path.Length)
            return obj;

        var segment = path[index];
        var prop = obj.GetType().GetProperty(segment, BindingFlags.Public | BindingFlags.Instance);
        if (prop is null)
            return null;

        var value = prop.GetValue(obj);
        if (value is null)
            return null;

        if (value is IEnumerable enumerable && value is not string && index + 1 < path.Length)
        {
            var list = new List<object?>();
            foreach (var item in enumerable)
            {
                list.Add(GetValue(item, path, index + 1));
            }
            return list;
        }

        return GetValue(value, path, index + 1);
    }

    private static void SetValue(object target, string[] path, int index, object? value)
    {
        var prop = target.GetType().GetProperty(path[index], BindingFlags.Public | BindingFlags.Instance);
        if (prop is null)
            return;

        if (index == path.Length - 1)
        {
            if (value is IEnumerable srcEnum && value is not string && typeof(IEnumerable).IsAssignableFrom(prop.PropertyType) && prop.PropertyType != typeof(string))
            {
                var elementType = prop.PropertyType.IsArray
                    ? prop.PropertyType.GetElementType()!
                    : prop.PropertyType.GetGenericArguments().FirstOrDefault() ?? typeof(object);

                var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType))!;
                foreach (var item in srcEnum)
                {
                    list.Add(ChangeType(item, elementType));
                }

                if (prop.PropertyType.IsArray)
                {
                    var array = Array.CreateInstance(elementType, list.Count);
                    list.CopyTo(array, 0);
                    prop.SetValue(target, array);
                }
                else
                {
                    prop.SetValue(target, list);
                }
            }
            else
            {
                var targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                prop.SetValue(target, ChangeType(value, targetType));
            }
        }
        else
        {
            var child = prop.GetValue(target);
            if (child is null)
            {
                child = Activator.CreateInstance(prop.PropertyType)!;
                prop.SetValue(target, child);
            }
            SetValue(child, path, index + 1, value);
        }
    }

    private static object? ChangeType(object? value, Type targetType)
    {
        if (value is null)
            return null;

        if (targetType.IsAssignableFrom(value.GetType()))
            return value;

        return Convert.ChangeType(value, targetType);
    }
}
