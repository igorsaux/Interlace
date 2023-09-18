using Interlace.Client.Graphics.Renderer;
using Interlace.Shared.Application;
using JetBrains.Annotations;

namespace Interlace.Client.Graphics;

[PublicAPI]
public interface IGraphicsManager : IManager
{
    ISurface Surface { get; }

    IVideoAdapter VideoAdapter { get; }

    IVideoDevice VideoDevice { get; }

    IEnumerable<Viewport> Viewports { get; }

    void Initialize();

    void PostInitialize();

    void Shutdown();

    void Render();

    void AddViewport(Viewport viewport);

    void RemoveViewport(Viewport viewport);
}
