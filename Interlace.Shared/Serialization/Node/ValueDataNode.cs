namespace Interlace.Shared.Serialization.Node;

public sealed class ValueDataNode : DataNode
{
    public object? Value;

    public ValueDataNode(object? value)
    {
        Value = value;
    }
}
