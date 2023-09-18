using System.Collections;

namespace Interlace.Shared.Serialization.Node;

public sealed class SequenceDataNode : DataNode, IList<DataNode>
{
    private readonly List<DataNode> _nodes;

    public SequenceDataNode()
    {
        _nodes = new List<DataNode>();
    }

    public SequenceDataNode(int capacity)
    {
        _nodes = new List<DataNode>(capacity);
    }
    
    public IEnumerator<DataNode> GetEnumerator()
    {
        return _nodes.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(DataNode item)
    {
        _nodes.Add(item);
    }

    public void Clear()
    {
        _nodes.Clear();
    }

    public bool Contains(DataNode item)
    {
        return _nodes.Contains(item);
    }

    public void CopyTo(DataNode[] array, int arrayIndex)
    {
        _nodes.CopyTo(array, arrayIndex);
    }

    public bool Remove(DataNode item)
    {
        return _nodes.Remove(item);
    }

    public int Count => _nodes.Count;

    public bool IsReadOnly => false;

    public int IndexOf(DataNode item)
    {
        return _nodes.IndexOf(item);
    }

    public void Insert(int index, DataNode item)
    {
        _nodes.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
        _nodes.RemoveAt(index);
    }

    public DataNode this[int index]
    {
        get => _nodes[index];
        set => _nodes[index] = value;
    }
}
