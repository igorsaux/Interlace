using System.Diagnostics.CodeAnalysis;
using Interlace.Shared.IoC;
using Interlace.Shared.Serialization.Node;
using Tomlyn;
using Tomlyn.Model;

namespace Interlace.Shared.Serialization.Parser;

internal sealed class TomlParser
{
    [Dependency] private readonly ISerializationManager _serialization = default!;

    public TomlParser()
    {
        IoCManager.Instance.InjectDependencies(this);
    }

    public bool TryParse(string data, [NotNullWhen(true)] out DataNode? node)
    {
        var document = Toml.Parse(data);
        var model = document.ToModel();

        node = ParseTomlModel(model);

        return true;
    }

    private DataNode ParseTomlModel(object root)
    {
        switch (root)
        {
            case TomlTable tomlTable:
            {
                var mappingNode = new MappingDataNode();

                foreach (var (key, value) in tomlTable)
                {
                    mappingNode.Add(key, ParseTomlModel(value));
                }

                return mappingNode;
            }
            case TomlArray tomlArray:
            {
                var sequenceNode = new SequenceDataNode();

                foreach (var value in tomlArray)
                {
                    sequenceNode.Add(ParseTomlModel(value!));
                }

                return sequenceNode;
            }
            default:
            {
                if (!_serialization.TrySerializeValue(root, out var result))
                    throw new NotSupportedException($"Can't deserialize {root}");

                return result;
            }
        }
    }
}
