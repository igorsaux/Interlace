using Interlace.Shared.Serialization.Node;
using JetBrains.Annotations;

namespace Interlace.Shared.Serialization.TypeSerializer.Primitive;

[UsedImplicitly]
public sealed class LongSerializer : ITypeSerializer<long, ValueDataNode>
{
    public DataNode Serialize(long source)
    {
        return new ValueDataNode(source);
    }

    public bool TryDeserialize(ValueDataNode source, out long result)
    {
        result = 0;

        if (source.Value is not long longValue)
            return false;

        result = longValue;
        
        return true;
    }
}
