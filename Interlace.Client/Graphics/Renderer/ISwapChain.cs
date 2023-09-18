using JetBrains.Annotations;
using Silk.NET.Maths;

namespace Interlace.Client.Graphics.Renderer;

[PublicAPI]
public interface ISwapChain
{
    public void Present();

    public IOwnedTextureView? TryGetTextureView();
}

[PublicAPI]
public interface IOwnedSwapChain : ISwapChain, IDisposable
{
}

[PublicAPI]
public sealed class SwapChainOptions
{
    public readonly string? Label;
    public readonly PresentMode PresentMode;
    public readonly Vector2D<int> Size;
    public readonly TextureFormat TextureFormat;
    public readonly TextureUsage TextureUsage;

    public SwapChainOptions(Vector2D<int> size, PresentMode presentMode = PresentMode.Fifo,
        TextureFormat textureFormat = TextureFormat.Any,
        TextureUsage textureUsage = TextureUsage.RenderAttachment, string? label = null)
    {
        PresentMode = presentMode;
        Size = size;
        TextureFormat = textureFormat;
        TextureUsage = textureUsage;
        Label = label;
    }
}

[PublicAPI]
public enum PresentMode
{
    Immediate,
    Mailbox,
    Fifo
}
