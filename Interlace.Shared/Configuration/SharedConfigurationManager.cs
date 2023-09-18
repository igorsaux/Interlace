using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using Interlace.Shared.Application.Hooks;
using Interlace.Shared.Logging;
using Interlace.Shared.Reflection;
using Interlace.Shared.Serialization;
using Tomlyn;
using Tomlyn.Model;

namespace Interlace.Shared.Configuration;

public abstract class SharedConfigurationManager : IConfigurationManager, IInitializeHook, IShutdownHook
{
    [IoC.Dependency] private readonly ILogManager _log = default!;
    [IoC.Dependency] private readonly ISerializationManager _serialization = default!;
    
    private readonly Dictionary<string, CVarDeclaration> _declarations = new();
    private readonly Dictionary<string, List<object>> _updateCallbacks = new();

    private ISawmill _sawmill = default!;

    protected abstract string? ConfigFile { get; }

    public void SetValue(string path, object? value)
    {
        if (!_declarations.TryGetValue(path, out var decl))
            throw new InvalidOperationException($"Invalid configuration variable '{path}'");

        SetValue(decl, value);
    }

    public void SetValue(CVarDeclaration decl, object? value)
    {
        decl.Value = Convert.ChangeType(value, decl.Type);

        _sawmill.Debug("{0} = {1}", decl.Path, decl.Value?.ToString() ?? "null");

        if (!_updateCallbacks.ContainsKey(decl.Path))
            return;

        foreach (var callback in _updateCallbacks[decl.Path])
            if (callback is Delegate deg)
                deg.DynamicInvoke(decl.Value);
    }

    public void SetDefaultValue(string path)
    {
        if (!_declarations.TryGetValue(path, out var decl))
            throw new InvalidOperationException($"Invalid configuration variable '{path}'");

        SetDefaultValue(decl);
    }

    public void SetDefaultValue(CVarDeclaration decl)
    {
        SetValue(decl, decl.DefaultValue);
    }

    public bool TryGetValue(string path, out object? value)
    {
        value = null;

        if (!_declarations.TryGetValue(path, out var decl))
            return false;

        value = _declarations[decl.Path].Value;
        
        return true;
    }

    public T? GetValue<T>(CVarDeclaration decl)
    {
        return (T?)_declarations[decl.Path].Value;
    }

    public void SubscribeOnValueChanged<T>(CVarDeclaration decl,
        IConfigurationManager.ValueChangedDelegate<T?> callback,
        bool callImmediately = true)
    {
        if (!_updateCallbacks.ContainsKey(decl.Path))
            _updateCallbacks.Add(decl.Path, new List<object>());

        _updateCallbacks[decl.Path].Add(callback);

        if (callImmediately)
            callback(GetValue<T>(decl));
    }

    public void SubscribeOnValueChanged<T>(string path, IConfigurationManager.ValueChangedDelegate<T?> callback,
        bool callImmediately = true)
    {
        if (!_declarations.TryGetValue(path, out var decl))
            throw new InvalidOperationException($"Invalid configuration variable '{path}'");

        SubscribeOnValueChanged(decl, callback, callImmediately);
    }

    public void UnsubscribeOnValueChanged<T>(CVarDeclaration decl,
        IConfigurationManager.ValueChangedDelegate<T> callback)
    {
        if (!_updateCallbacks.ContainsKey(decl.Path))
            return;

        _updateCallbacks[decl.Path].Remove(callback);
    }

    public void UnsubscribeOnValueChanged<T>(string path, IConfigurationManager.ValueChangedDelegate<T> callback)
    {
        if (!_declarations.TryGetValue(path, out var decl))
            throw new InvalidOperationException($"Invalid configuration variable '{path}'");

        UnsubscribeOnValueChanged(decl, callback);
    }

    public void Initialize()
    {
        _sawmill = _log.GetSawmill("cfg");

        PopulateDeclarations();
        
        if (ConfigFile is null)
            return;
        
        var cfgPath = Path.GetFullPath(ConfigFile, Environment.CurrentDirectory);

        _sawmill.Info("Config path: '{0}'", cfgPath);
        
        LoadFromFile(cfgPath);
    }

    public void Shutdown()
    {
        _updateCallbacks.Clear();
        _declarations.Clear();
    }

    private void LoadFromFile(string path)
    {
        if (!File.Exists(path))
        {
            _sawmill.Error("Configuration file '{0}' not found", path);
            return;
        }

        
        var fileContent = File.ReadAllText(path);
        _serialization.TryDeserializeFromToml(fileContent, out var result);
        var document = Toml.Parse(fileContent, path);

        LoadFromTable(document.ToModel(), null, new Stack<string>());
    }

    private void LoadFromTable(TomlTable table, string? key, Stack<string> path)
    {
        if (key is null)
        {
            foreach (var tableKey in table.Keys) LoadFromTable(table, tableKey, path);

            return;
        }

        path.Push(key);

        if (!table.TryGetValue(key, out var value))
        {
            _sawmill.Error("Couldn't find '{0}' in config", key);

            path.Pop();
            return;
        }

        if (value is TomlTable nextTable)
        {
            LoadFromTable(nextTable, null, path);
        }
        else
        {
            var fullPath = string.Join('.', path.Reverse());

            if (!_declarations.TryGetValue(fullPath, out var decl))
            {
                _sawmill.Error("Couldn't find config variable '{0}'!", fullPath);

                path.Pop();
                return;
            }

            ParseAndSetValue(decl, value);
        }

        path.Pop();
    }

    private void ParseAndSetValue(CVarDeclaration decl, object? value)
    {
        if (decl.Type.IsEnum)
        {
            if (value is null)
            {
                SetValue(decl, null);
            }
            else if (value is string stringValue)
            {
                if (!Enum.TryParse(decl.Type, stringValue, out var result))
                {
                    _sawmill.Error("Configuration variable '{0}' with enum type has invalid value '{1}'", decl.Path,
                        stringValue);

                    return;
                }

                SetValue(decl, result);
            }
            else if (ReflectionHelpers.IsIntegerType(value.GetType()))
            {
                SetValue(decl, Enum.ToObject(decl.Type, value));
            }
            else
            {
                _sawmill.Error("Configuration variable '{0}' with enum type has invalid value '{1}' with type '{2}'",
                    decl.Path, value, value.GetType().ToString());
            }
        }
        else if (decl.Type == typeof(string))
        {
            if (value is null)
                SetValue(decl, null);
            else if (value is string stringValue)
                SetValue(decl, stringValue);
            else if (ReflectionHelpers.IsNumericType(value.GetType()))
                SetValue(decl, value.ToString());
            else
                _sawmill.Error("Configuration variable '{0}' with string type has invalid value '{1}' with type '{2}'",
                    decl.Path, value, value.GetType().ToString());
        }
        else if (ReflectionHelpers.IsIntegerType(decl.Type))
        {
            if (value is null)
            {
                SetValue(decl, null);
            }
            else if (value is string stringValue)
            {
                if (!long.TryParse(stringValue, CultureInfo.InvariantCulture, out var result))
                {
                    _sawmill.Error("Can't parse '{0}' as integer for config variable '{1}'!", stringValue, decl.Path);
                    return;
                }

                SetValue(decl, result);
            }
            else if (value is bool boolValue)
            {
                SetValue(decl, boolValue ? 1 : 0);
            }
            else if (ReflectionHelpers.IsIntegerType(value.GetType()))
            {
                SetValue(decl, value);
            }
            else
            {
                _sawmill.Error("Configuration variable '{0}' with integer type has invalid value '{1}' with type '{2}'",
                    decl.Path, value, value.GetType().ToString());
            }
        }
        else if (ReflectionHelpers.IsFloatType(decl.Type))
        {
            if (value is null)
            {
                SetValue(decl, null);
            }
            else if (value is string stringValue)
            {
                if (!double.TryParse(stringValue, CultureInfo.InvariantCulture, out var result))
                {
                    _sawmill.Error("Can't parse '{0}' as float for config variable '{1}'!", stringValue, decl.Path);
                    return;
                }

                SetValue(decl, result);
            }
            else if (ReflectionHelpers.IsNumericType(value.GetType()))
            {
                SetValue(decl, value);
            }
            else
            {
                _sawmill.Error("Configuration variable '{0}' with float type has invalid value '{1}' with type '{2}'",
                    decl.Path, value, value.GetType().ToString());
            }
        }
        else if (decl.Type == typeof(bool))
        {
            if (value is null)
            {
                SetValue(decl, null);
            }
            else if (value is string stringValue)
            {
                if (!bool.TryParse(stringValue, out var result))
                {
                    _sawmill.Error("Can't parse '{0}' as bool for config variable '{1}'!", stringValue, decl.Path);
                    return;
                }

                SetValue(decl, result);
            }
            else if (ReflectionHelpers.IsIntegerType(value.GetType()))
            {
                SetValue(decl, (bool)value);
            }
            else if (value is bool boolValue)
            {
                SetValue(decl, boolValue);
            }
            else
            {
                _sawmill.Error("Configuration variable '{0}' with bool type has invalid value '{1}' with type '{2}'",
                    decl.Path, value, value.GetType().ToString());
            }
        }
        else
        {
            _sawmill.Error("Configuration variable '{0}' with type '{1}' can't be parsed from file", decl.Path,
                decl.Type.ToString());
        }
    }

    private void PopulateDeclarations()
    {
        _declarations.Clear();

        var cvarsType = typeof(CVars);
        const BindingFlags attrs = BindingFlags.Public | BindingFlags.Static | BindingFlags.GetField |
                                   BindingFlags.GetProperty;

        foreach (var propertyInfo in cvarsType.GetProperties(attrs))
        {
            var decl = Unsafe.As<CVarDeclaration>(propertyInfo.GetValue(null))!;

            AddDeclaration(decl);
        }

        foreach (var fieldInfo in cvarsType.GetFields(attrs))
        {
            var decl = Unsafe.As<CVarDeclaration>(fieldInfo.GetValue(null))!;

            AddDeclaration(decl);
        }

        _sawmill.Debug("Initialized {0} config variables", _declarations.Count);
    }

    private void AddDeclaration(CVarDeclaration decl)
    {
        if (_declarations.ContainsKey(decl.Path))
            throw new InvalidOperationException($"Configuration variable with path '{decl.Path}' already exist!");

        decl.Value = decl.DefaultValue;

        _declarations.Add(decl.Path, decl);
    }
}
