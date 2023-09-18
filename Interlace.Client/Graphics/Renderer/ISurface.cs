using JetBrains.Annotations;

namespace Interlace.Client.Graphics.Renderer;

[PublicAPI]
public interface ISurface
{
}

[PublicAPI]
public interface IOwnedSurface : ISurface, IDisposable
{
}

[PublicAPI]
public sealed class SurfaceOptions
{
    public readonly string? Label;

    public SurfaceOptions(string? label = null)
    {
        Label = label;
    }
}
