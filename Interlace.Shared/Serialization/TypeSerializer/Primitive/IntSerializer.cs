using Interlace.Shared.Serialization.Node;
using JetBrains.Annotations;

namespace Interlace.Shared.Serialization.TypeSerializer.Primitive;

[UsedImplicitly]
public sealed class IntSerializer : ITypeSerializer<int, ValueDataNode>
{
    public DataNode Serialize(int source)
    {
        return new ValueDataNode(source);
    }

    public bool TryDeserialize(ValueDataNode source, out int result)
    {
        result = 0;
        
        if (source.Value is not int intValue)
            return false;

        result = intValue;

        return true;
    }
}
