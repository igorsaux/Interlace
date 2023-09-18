using System.Diagnostics.CodeAnalysis;
using Interlace.Client.Windowing;
using Interlace.Shared.Logging;
using JetBrains.Annotations;

namespace Interlace.Client.Graphics.Renderer;

[PublicAPI]
internal interface IRenderer
{
    public delegate void RendererLogCallback(LogLevel level, string message);

    IOwnedSurface CreateSurface(IWindow window, SurfaceOptions options);

    bool TryRequestWGpuAdapter(RequestAdapterOptions options, [NotNullWhen(true)] out IOwnedVideoAdapter? adapter);

    void Initialize();

    void SetLogLevel(LogLevel? level);

    void SetLogCallback(RendererLogCallback callback);
}

internal interface IOwnedRenderer : IRenderer, IDisposable
{
}
