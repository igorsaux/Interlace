using JetBrains.Annotations;

namespace Interlace.Client.Graphics.Renderer;

[PublicAPI]
public interface IRenderPass
{
    void InsertDebugMarker(string label);

    IRenderPassDebugGroup CreateDebugGroupScope(string label);
}

[PublicAPI]
public interface IOwnedRenderPass : IRenderPass, IDisposable
{
}

[PublicAPI]
public sealed class RenderPassOptions
{
    public readonly IColorAttachment? ColorAttachment;
    public readonly string? Label;

    public RenderPassOptions(IColorAttachment? colorAttachment, string? label = null)
    {
        ColorAttachment = colorAttachment;
        Label = label;
    }
}
