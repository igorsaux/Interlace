using Interlace.Shared.Graphics;
using JetBrains.Annotations;

namespace Interlace.Client.Windowing;

[PublicAPI]
public sealed class WindowOptions
{
    public required int Height;

    public WindowMode Mode = WindowMode.Windowed;

    public WindowPosition Position = WindowPosition.Centered;

    public required string Title;

    public required int Width;

    public int X;

    public int Y;
}

[PublicAPI]
public enum WindowPosition
{
    Custom,
    Centered,
    Undefined
}
