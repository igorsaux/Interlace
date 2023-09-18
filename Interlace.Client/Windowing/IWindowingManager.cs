using Interlace.Shared.Application;
using JetBrains.Annotations;

namespace Interlace.Client.Windowing;

[PublicAPI]
public interface IWindowingManager : IManager
{
    IWindow? MainWindow { get; }

    event Action? Quit;

    void FrameUpdate();
}
