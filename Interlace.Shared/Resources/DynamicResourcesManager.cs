using System.Text;
using Interlace.Shared.Application.Hooks;
using Interlace.Shared.IoC;
using Interlace.Shared.Logging;

namespace Interlace.Shared.Resources;

public sealed class DynamicResourcesManager : IResourcesManager, IInitializeHook
{
    [Dependency] private readonly ILogManager _log = default!;
    
    private string _resourcesFolder = default!;
    private ISawmill _sawmill = default!;

    public void Initialize()
    {
        _sawmill = _log.GetSawmill("resources");

        ApplicationDataPath =
            $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}{Path.DirectorySeparatorChar}Interlace";

        var cwd = Path.GetFullPath(
            $"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..",
            Environment.CurrentDirectory);
        _resourcesFolder = $"{cwd}{Path.DirectorySeparatorChar}Resources";

        _sawmill.Info("Application data folder: '{0}'", ApplicationDataPath);
        _sawmill.Debug("Resources folder: '{0}'", _resourcesFolder);
    }

    public string ApplicationDataPath { get; private set; } = default!;

    public string ReadTextResource(ResourcePath resourcePath)
    {
        var path = TransformPath(resourcePath.Path);

        return File.ReadAllText(path, Encoding.UTF8);
    }

    public Stream OpenStream(ResourcePath resourcePath)
    {
        var path = TransformPath(resourcePath.Path);

        return File.OpenRead(path);
    }

    public ResourcePath[] GetDirectories(ResourcePath resourcePath)
    {
        var path = TransformPath(resourcePath.Path);
        var directories = Directory.GetDirectories(path);
        var transformedPaths = new ResourcePath[directories.Length];

        for (var i = 0; i < directories.Length; i++)
            transformedPaths[i] =
                new ResourcePath(
                    $"{Path.DirectorySeparatorChar}{Path.GetRelativePath(_resourcesFolder, directories[i])}");

        return transformedPaths;
    }

    public ResourcePath[] GetFiles(ResourcePath resourcePath)
    {
        var path = TransformPath(resourcePath.Path);
        var files = Directory.GetFiles(path);
        var transformedPaths = new ResourcePath[files.Length];

        for (var i = 0; i < files.Length; i++)
            transformedPaths[i] =
                new ResourcePath($"{Path.DirectorySeparatorChar}{Path.GetRelativePath(_resourcesFolder, files[i])}");

        return transformedPaths;
    }

    private string TransformPath(string path)
    {
        return $"{_resourcesFolder}{Path.DirectorySeparatorChar}{path.TrimStart(Path.DirectorySeparatorChar)}";
    }
}
