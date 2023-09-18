using System.Diagnostics.CodeAnalysis;
using Interlace.Shared.Serialization.Node;
using JetBrains.Annotations;

namespace Interlace.Shared.Serialization.TypeSerializer;

[PublicAPI]
public interface ITypeSerializer<TOriginal, in TSerialized> where TSerialized: DataNode
{
    DataNode Serialize(TOriginal source);

    bool TryDeserialize(TSerialized source, [NotNullWhen(true)] out TOriginal? result);
}
