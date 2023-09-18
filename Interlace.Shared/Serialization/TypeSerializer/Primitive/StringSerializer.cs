using System.Diagnostics.CodeAnalysis;
using Interlace.Shared.Serialization.Node;
using JetBrains.Annotations;

namespace Interlace.Shared.Serialization.TypeSerializer.Primitive;

[UsedImplicitly]
public sealed class StringSerializer : ITypeSerializer<string, ValueDataNode>
{
    public DataNode Serialize(string source)
    {
        return new ValueDataNode(source);
    }

    public bool TryDeserialize(ValueDataNode source, [NotNullWhen(true)] out string? result)
    {
        result = null;

        if (source.Value is not string stringValue)
            return false;

        result = stringValue;
        
        return true;
    }
}
