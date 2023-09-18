using JetBrains.Annotations;

namespace Interlace.Shared.Reflection;

[PublicAPI]
public static class ReflectionHelpers
{
    private static readonly HashSet<Type> NumericTypes = new()
    {
        typeof(byte),
        typeof(ushort),
        typeof(short),
        typeof(uint),
        typeof(int),
        typeof(ulong),
        typeof(long),
        typeof(float),
        typeof(double)
    };

    private static readonly HashSet<Type> IntegerTypes = new()
    {
        typeof(byte),
        typeof(ushort),
        typeof(short),
        typeof(uint),
        typeof(int),
        typeof(ulong),
        typeof(long)
    };

    private static readonly HashSet<Type> FloatTypes = new()
    {
        typeof(float),
        typeof(double)
    };

    public static bool IsNumericType(Type type)
    {
        return NumericTypes.Contains(type);
    }

    public static bool IsNumericType<T>()
    {
        return IsNumericType(typeof(T));
    }

    public static bool IsIntegerType(Type type)
    {
        return IntegerTypes.Contains(type);
    }

    public static bool IsIntegerType<T>()
    {
        return IsIntegerType(typeof(T));
    }

    public static bool IsFloatType(Type type)
    {
        return FloatTypes.Contains(type);
    }

    public static bool IsFloatType<T>()
    {
        return IsFloatType(typeof(T));
    }
}
