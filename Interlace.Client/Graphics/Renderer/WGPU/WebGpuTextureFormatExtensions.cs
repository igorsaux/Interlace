using Interlace.Vendor.WGPU;

namespace Interlace.Client.Graphics.Renderer.WGPU;

internal static class WebGpuTextureFormatExtensions
{
    public static WebGpu.TextureFormat ToWebGpuTextureFormat(this TextureFormat value)
    {
        return value switch
        {
            TextureFormat.Any => WebGpu.TextureFormat.Undefined,
            _ => (WebGpu.TextureFormat)value
        };
    }

    public static TextureFormat ToTextureFormat(this WebGpu.TextureFormat value)
    {
        return value switch
        {
            WebGpu.TextureFormat.Undefined => TextureFormat.Any,
            _ => (TextureFormat)value
        };
    }
}
