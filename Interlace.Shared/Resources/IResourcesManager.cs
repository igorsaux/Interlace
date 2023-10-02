using Interlace.Shared.Application;
using JetBrains.Annotations;

namespace Interlace.Shared.Resources;

[PublicAPI]
public interface IResourcesManager : IManager
{
    public string ApplicationDataPath { get; }

    string ReadTextResource(ResourcePath resourcePath);

    Stream OpenStream(ResourcePath resourcePath);

    ResourcePath[] GetDirectories(ResourcePath resourcePath);

    ResourcePath[] GetFiles(ResourcePath resourcePath);
}
