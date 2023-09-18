using System.Diagnostics;
using Interlace.Client.Graphics.Renderer;
using Interlace.Shared.IoC;
using JetBrains.Annotations;
using Silk.NET.Maths;

namespace Interlace.Client.Graphics;

[PublicAPI]
public sealed class Viewport
{
    [Dependency] private readonly IGraphicsManager _graphics = default!;

    public Viewport(Vector2D<int> size)
    {
        IoCManager.Instance.InjectDependencies(this);

        SetSize(size);
    }

    public Vector2D<int> Size { get; private set; } = Vector2D<int>.Zero;

    public void SetSize(Vector2D<int> newSize)
    {
        Debug.Assert(newSize.X != 0 && newSize.Y != 0, "X and Y should not be zero");

        Size = newSize;
    }

    public void Render(IVideoDevice device)
    {
        var queue = device.Queue;

        using var encoder = device.CreateCommandEncoder(new CommandEncoderOptions("Viewport encoder"));

        encoder.InsertDebugMarker("Start encoding");

        // using (var _ = encoder.CreateDebugGroupScope("Drawing something"))
        // {
        //     using var view = _swapChain.TryGetTextureView();
        //     
        //     if (view is null)
        //         return;
        // }

        using var commands = encoder.Finish(new CommandBufferOptions("Viewport commands"));

        queue.Submit(commands);
    }
}
