using Interlace.Vendor.WGPU;

namespace Interlace.Client.Graphics.Renderer.WGPU;

internal static class WebGpuPresentModeExtensions
{
    public static WebGpu.PresentMode ToWebGpuPresentMode(this PresentMode value)
    {
        return value switch
        {
            PresentMode.Immediate => WebGpu.PresentMode.Immediate,
            PresentMode.Mailbox => WebGpu.PresentMode.Mailbox,
            PresentMode.Fifo => WebGpu.PresentMode.Fifo,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
    }
}
