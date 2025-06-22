using System.Reflection;

namespace Virtual.Roulette.Application.UnitTests;

public static class ReflectionExtensions
{
    public static void SetPrivateProperty<T>(this T target, string propertyName, object? value)
    {
        var type = typeof(T);
        var prop = type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        if (prop == null)
            throw new ArgumentException($"Property '{propertyName}' not found on type '{type.FullName}'.");

        if (!prop.CanWrite)
            throw new ArgumentException($"Property '{propertyName}' does not have a setter.");

        prop.SetValue(target, value);
    }
}