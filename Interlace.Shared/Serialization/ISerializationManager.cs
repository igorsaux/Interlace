using System.Diagnostics.CodeAnalysis;
using Interlace.Shared.Application;
using Interlace.Shared.Serialization.Node;
using JetBrains.Annotations;

namespace Interlace.Shared.Serialization;

[PublicAPI]
public interface ISerializationManager : IManager
{
    bool TryDeserializeFromToml(string data, [NotNullWhen(true)] out DataNode? result);

    bool TrySerializeValue(object? value, [NotNullWhen(true)] out DataNode? result);

    byte[] SerializeToBinary(DataNode? value);

    DataNode? DeserializeFromBinary(byte[] data);
}
