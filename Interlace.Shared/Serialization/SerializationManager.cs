using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Interlace.Shared.Application.Hooks;
using Interlace.Shared.Logging;
using Interlace.Shared.Reflection;
using Interlace.Shared.Serialization.Node;
using Interlace.Shared.Serialization.Parser;
using Interlace.Shared.Serialization.TypeSerializer;

namespace Interlace.Shared.Serialization;

public sealed class SerializationManager : ISerializationManager, IInitializeHook
{
    [IoC.Dependency] private readonly ILogManager _log = default!;
    [IoC.Dependency] private readonly IReflectionManager _reflection = default!;

    private TomlParser _tomlParser = default!;
    private BinaryParser _binaryParser = default!;
    private ISawmill _sawmill = default!;
    private readonly Dictionary<Type, object> _typeSerializers = new();

    public void Initialize()
    {
        _sawmill = _log.GetSawmill("ser");

        RegisterTypeSerializers();
        
        _sawmill.Debug("Registered type serializers: {0}", _typeSerializers.Values.Count);

        _tomlParser = new TomlParser();
        _binaryParser = new BinaryParser();
    }

    private void RegisterTypeSerializers()
    {
        var typeSerializerType = typeof(ITypeSerializer<,>);
        var typeSerializers = _reflection.GetImplementationsOf(typeSerializerType);

        foreach (var type in typeSerializers)
        foreach (var i in type.GetInterfaces())
        {
            if (!i.IsGenericType || i.GetGenericTypeDefinition() != typeSerializerType)
                continue;

            var typeSerializerInstance = _reflection.CreateInstanceOf(type);
            var originalType = i.GetGenericArguments()[0];

            if (_typeSerializers.ContainsKey(originalType))
            {
                _sawmill.Error("Found extra type serializer '{0}' for type '{1}'", i.ToString(), originalType.ToString());
                continue;
            }

            _typeSerializers.Add(originalType, typeSerializerInstance);
        }
    }

    public bool TryDeserializeFromToml(string data, [NotNullWhen(true)] out DataNode? result)
    {
        return _tomlParser.TryParse(data, out result);
    }

    public bool TrySerializeValue(object? value, [NotNullWhen(true)] out DataNode? result)
    {
        result = null;

        if (value is null)
        {
            result = new ValueDataNode(null);

            return true;
        }
        
        var type = value.GetType();

        if (!_typeSerializers.TryGetValue(type, out var typeSerializer))
        {
            if (type.IsClass)
            {
                if (TrySerializeClass(value, out result, out var reason)) 
                    return true;
                
                _sawmill.Error(reason);

                return false;

            }
            
            _sawmill.Error("Can't serialize type '{0}'", type.ToString());
            
            return false;
        }

        var serializeMethod = typeSerializer.GetType().GetMethod("Serialize", BindingFlags.Instance | BindingFlags.Public)!;

        result = Unsafe.As<DataNode>(serializeMethod.Invoke(typeSerializer, new[] { value }))!;
        
        return true;
    }

    private bool TrySerializeClass(object value, [NotNullWhen(true)] out DataNode? result, [NotNullWhen(false)] out string? reason)
    {
        result = null;
        reason = null;
        
        var type = value.GetType();

        if (!_reflection.HasCustomAttribute<SerializableAttribute>(type))
        {
            reason = $"Class '{type}' is not marked as serializable";

            return false;
        }
        
        var mapping = new MappingDataNode();

        foreach (var fieldInfo in type.GetFields())
        {
            if (!_reflection.TryGetCustomAttribute<DataMemberAttribute>(fieldInfo, out var dataMemberAttribute))
                continue;
            
            if (!TrySerializeField(value, fieldInfo, dataMemberAttribute, out var fieldResult))
                continue;

            var (name, data) = fieldResult.Value;
            
            mapping.Add(name, data);
        }

        result = mapping;
        
        return true;
    }

    private bool TrySerializeField(object instance, FieldInfo memberInfo, DataMemberAttribute dataMemberAttribute, [NotNullWhen(true)] out (string key, DataNode data)? result)
    {
        result = null;
        
        var name = dataMemberAttribute.Name ?? memberInfo.Name;
        var value = memberInfo.GetValue(instance);

        if (!TrySerializeValue(value, out var data))
            return false;

        result = (name, data);
        
        return true;
    }

    public byte[] SerializeToBinary(DataNode? value)
    {
        return _binaryParser.ToBinary(value).ToArray();
    }

    public DataNode? DeserializeFromBinary(byte[] data)
    {
        return _binaryParser.FromBinary(data);
    }
}
