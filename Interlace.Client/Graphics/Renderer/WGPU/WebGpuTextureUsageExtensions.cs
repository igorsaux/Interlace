using Interlace.Vendor.WGPU;

namespace Interlace.Client.Graphics.Renderer.WGPU;

internal static class WebGpuTextureUsageExtensions
{
    public static WebGpu.TextureUsage ToWebGpuTextureUsage(this TextureUsage value)
    {
        return (WebGpu.TextureUsage)value;
    }
}
