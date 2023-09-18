using Interlace.Vendor.WGPU;

namespace Interlace.Client.Graphics.Renderer.WGPU;

public static class WebGpuLoadOpExtensions
{
    public static WebGpu.LoadOp ToWebGpuLoadOp(this LoadOp value)
    {
        return value switch
        {
            LoadOp.None => WebGpu.LoadOp.Undefined,
            LoadOp.Clear => WebGpu.LoadOp.Clear,
            LoadOp.Load => WebGpu.LoadOp.Load,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
    }

    public static LoadOp ToLoadOp(this WebGpu.LoadOp value)
    {
        return value switch
        {
            WebGpu.LoadOp.Undefined => LoadOp.None,
            WebGpu.LoadOp.Clear => LoadOp.Clear,
            WebGpu.LoadOp.Load => LoadOp.Load,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
    }
}
