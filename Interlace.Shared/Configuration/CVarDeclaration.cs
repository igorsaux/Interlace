using JetBrains.Annotations;

namespace Interlace.Shared.Configuration;

[PublicAPI]
public sealed class CVarDeclaration
{
    public readonly object? DefaultValue;

    public readonly CVarFlag Flags;
    public readonly string Path;

    public readonly Type Type;

    public object? Value;

    private CVarDeclaration(string path, Type type, object? defaultValue, CVarFlag flags)
    {
        Path = path;
        Type = type;
        DefaultValue = defaultValue;
        Flags = flags;
    }

    public static CVarDeclaration Create(string path, Type type, object? defaultValue, CVarFlag flags)
    {
        return new CVarDeclaration(path, type, defaultValue, flags);
    }

    public static CVarDeclaration Create<T>(string path, T? defaultValue, CVarFlag flags)
    {
        return new CVarDeclaration(path, typeof(T), defaultValue, flags);
    }
}
