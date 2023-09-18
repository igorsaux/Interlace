using Interlace.Shared.Graphics;
using JetBrains.Annotations;
using Silk.NET.Maths;

namespace Interlace.Client.Windowing;

[PublicAPI]
public interface IWindow
{
    public delegate void WindowResizedDelegate(int width, int height);

    bool IsVisible { get; }

    Vector2D<int> Size { get; }

    event WindowResizedDelegate? WindowResized;

    event Action? WindowMinimized;

    event Action? WindowMaximized;

    event Action? WindowShown;

    event Action? WindowHidden;

    NativeWindowInfo GetNativeWindowInfo();

    void SetWindowMode(WindowMode mode);
}
