using Interlace.Vendor.WGPU;

namespace Interlace.Client.Graphics.Renderer.WGPU;

public sealed class WGpuColorAttachment : IColorAttachment
{
    public WGpuColorAttachment(WebGpu.RenderPassColorAttachment attachment)
    {
        Attachment = attachment;
    }

    internal WebGpu.RenderPassColorAttachment Attachment { get; private set; }
}
