using System.Diagnostics;
using JetBrains.Annotations;

namespace Interlace.Shared.Resources;

[PublicAPI]
public readonly struct ResourcePath
{
    public readonly string Path;

    public ResourcePath(string path)
    {
        Debug.Assert(path.StartsWith('/') || path.StartsWith(System.IO.Path.DirectorySeparatorChar));

        Path = path;
    }
}
