using Interlace.Shared.Serialization.Node;
using JetBrains.Annotations;

namespace Interlace.Shared.Serialization.TypeSerializer.Primitive;

[UsedImplicitly]
public sealed class BoolSerializer : ITypeSerializer<bool, ValueDataNode>
{
    public DataNode Serialize(bool source)
    {
        return new ValueDataNode(source);
    }

    public bool TryDeserialize(ValueDataNode source, out bool result)
    {
        result = false;

        if (source.Value is bool boolValue)
            result = boolValue;

        return true;
    }
}
