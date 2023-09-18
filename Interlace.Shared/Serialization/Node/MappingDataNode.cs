using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Interlace.Shared.Serialization.Node;

public sealed class MappingDataNode : DataNode, IDictionary<string, DataNode>
{
    private readonly Dictionary<string, DataNode> _nodes;

    public MappingDataNode()
    {
        _nodes = new Dictionary<string, DataNode>();
    }

    public MappingDataNode(int capacity)
    {
        _nodes = new Dictionary<string, DataNode>(capacity);
    }
    
    public IEnumerator<KeyValuePair<string, DataNode>> GetEnumerator()
    {
        return _nodes.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(KeyValuePair<string, DataNode> item)
    {
        _nodes.Add(item.Key, item.Value);
    }

    public void Clear()
    {
        _nodes.Clear();
    }

    public bool Contains(KeyValuePair<string, DataNode> item)
    {
        return _nodes.Contains(item);
    }

    public void CopyTo(KeyValuePair<string, DataNode>[] array, int arrayIndex)
    {
        ((ICollection<KeyValuePair<string, DataNode>>)_nodes).CopyTo(array, arrayIndex);
    }

    public bool Remove(KeyValuePair<string, DataNode> item)
    {
        return _nodes.Remove(item.Key);
    }

    public int Count => _nodes.Count;

    public bool IsReadOnly => false;

    public void Add(string key, DataNode value)
    {
        _nodes.Add(key, value);
    }

    public bool ContainsKey(string key)
    {
        return _nodes.ContainsKey(key);
    }

    public bool Remove(string key)
    {
        return _nodes.Remove(key);
    }

    public bool TryGetValue(string key, [NotNullWhen(true)] out DataNode? value)
    {
        return _nodes.TryGetValue(key, out value);
    }

    public DataNode this[string key]
    {
        get => _nodes[key];
        set => _nodes[key] = value;
    }

    public ICollection<string> Keys => _nodes.Keys;

    public ICollection<DataNode> Values => _nodes.Values;
}
