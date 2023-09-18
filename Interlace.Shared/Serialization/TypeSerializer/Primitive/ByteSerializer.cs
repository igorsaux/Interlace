using Interlace.Shared.Serialization.Node;
using JetBrains.Annotations;

namespace Interlace.Shared.Serialization.TypeSerializer.Primitive;

[UsedImplicitly]
public sealed class ByteSerializer : ITypeSerializer<byte, ValueDataNode>
{
    public DataNode Serialize(byte source)
    {
        return new ValueDataNode(source);
    }

    public bool TryDeserialize(ValueDataNode source, out byte result)
    {
        result = 0;

        if (source.Value is not byte byteValue)
            return false;

        result = byteValue;
        
        return true;
    }
}
