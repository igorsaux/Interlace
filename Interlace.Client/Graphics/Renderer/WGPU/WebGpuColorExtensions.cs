using Interlace.Vendor.WGPU;

namespace Interlace.Client.Graphics.Renderer.WGPU;

public static class WebGpuColorExtensions
{
    public static WebGpu.Color ToWebGpuColor(this Color value)
    {
        return new WebGpu.Color
        {
            R = value.R / 255.0,
            G = value.G / 255.0,
            B = value.B / 255.0,
            A = value.A / 255.0
        };
    }
}
