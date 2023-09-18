using Interlace.Shared.Graphics;
using Interlace.Vendor.WGPU;

namespace Interlace.Client.Graphics.Renderer.WGPU;

public static class WebGpuBackendTypeExtensions
{
    public static WebGpu.BackendType ToWebGpuBackendType(this VideoAdapterBackendType value)
    {
        return value switch
        {
            VideoAdapterBackendType.Any => WebGpu.BackendType.Undefined,
            VideoAdapterBackendType.WebGpu => WebGpu.BackendType.WebGPU,
            VideoAdapterBackendType.D3D11 => WebGpu.BackendType.D3D11,
            VideoAdapterBackendType.D3D12 => WebGpu.BackendType.D3D12,
            VideoAdapterBackendType.Metal => WebGpu.BackendType.Metal,
            VideoAdapterBackendType.Vulkan => WebGpu.BackendType.Vulkan,
            VideoAdapterBackendType.OpenGl => WebGpu.BackendType.OpenGL,
            VideoAdapterBackendType.OpenGles => WebGpu.BackendType.OpenGLES,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
    }
}
