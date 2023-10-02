using Interlace.Shared.Application;
using JetBrains.Annotations;

namespace Interlace.Shared.Resources;

[PublicAPI]
public interface IResourcesManager : IManager
{
    public string ApplicationDataPath { get; }

    string ReadTextResource(ResourcePath resourcePath);

    ResourcePath[] GetDirectories(ResourcePath resourcePath);

    ResourcePath[] GetFiles(ResourcePath resourcePath);

    // TODO: remove the hack in future
    string ResourcePathToNative(ResourcePath resourcePath);
}
