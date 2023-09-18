using Interlace.Vendor.WGPU;

namespace Interlace.Client.Graphics.Renderer.WGPU;

public static class WebGpuStoreOpExtensions
{
    public static WebGpu.StoreOp ToWebGpuStoreOp(this StoreOp value)
    {
        return value switch
        {
            StoreOp.None => WebGpu.StoreOp.Undefined,
            StoreOp.Store => WebGpu.StoreOp.Store,
            StoreOp.Discard => WebGpu.StoreOp.Discard,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
    }

    public static StoreOp ToStoreOp(this WebGpu.StoreOp value)
    {
        return value switch
        {
            WebGpu.StoreOp.Undefined => StoreOp.None,
            WebGpu.StoreOp.Store => StoreOp.Store,
            WebGpu.StoreOp.Discard => StoreOp.Discard,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
    }
}
