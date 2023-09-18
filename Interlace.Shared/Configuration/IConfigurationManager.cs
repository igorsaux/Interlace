using Interlace.Shared.Application;
using JetBrains.Annotations;

namespace Interlace.Shared.Configuration;

[PublicAPI]
public interface IConfigurationManager : IManager
{
    public delegate void ValueChangedDelegate<in T>(T newValue);

    void SetValue(string path, object? value);

    void SetValue(CVarDeclaration decl, object? value);

    void SetDefaultValue(string path);

    void SetDefaultValue(CVarDeclaration decl);

    bool TryGetValue(string path, out object? value);

    T? GetValue<T>(CVarDeclaration decl);

    void SubscribeOnValueChanged<T>(CVarDeclaration decl, ValueChangedDelegate<T?> callback,
        bool callImmediately = true);

    void SubscribeOnValueChanged<T>(string path, ValueChangedDelegate<T?> callback, bool callImmediately = true);

    void UnsubscribeOnValueChanged<T>(CVarDeclaration decl, ValueChangedDelegate<T> callback);

    void UnsubscribeOnValueChanged<T>(string path, ValueChangedDelegate<T> callback);
}
