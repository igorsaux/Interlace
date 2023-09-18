using System.Runtime.InteropServices;
using Interlace.Shared.Graphics;
using Interlace.Vendor.WGPU;

namespace Interlace.Client.Graphics.Renderer.WGPU;

internal static class WebGpuAdapterPropertiesExtensions
{
    public static VideoAdapterProperties ToVideoAdapterOptions(this WebGpu.AdapterProperties value)
    {
        unsafe
        {
            var name = Marshal.PtrToStringAnsi((IntPtr)value.Name);

            if (string.IsNullOrEmpty(name))
                name = "UNKNOWN";

            var driverDescription = Marshal.PtrToStringAnsi((IntPtr)value.DriverDescription);

            if (string.IsNullOrEmpty(driverDescription))
                driverDescription = "UNKNOWN";

            var vendorName = Marshal.PtrToStringAnsi((IntPtr)value.VendorName);

            if (string.IsNullOrEmpty(vendorName))
                vendorName = "UNKNOWN";

            var architecture = Marshal.PtrToStringAnsi((IntPtr)value.Architecture);

            if (string.IsNullOrEmpty(architecture))
                architecture = "UNKNOWN";

            var backendType = value.BackendType switch
            {
                WebGpu.BackendType.Undefined => VideoAdapterBackendType.Any,
                WebGpu.BackendType.Null => VideoAdapterBackendType.Any,
                WebGpu.BackendType.WebGPU => VideoAdapterBackendType.WebGpu,
                WebGpu.BackendType.D3D11 => VideoAdapterBackendType.D3D11,
                WebGpu.BackendType.D3D12 => VideoAdapterBackendType.D3D12,
                WebGpu.BackendType.Metal => VideoAdapterBackendType.Metal,
                WebGpu.BackendType.Vulkan => VideoAdapterBackendType.Vulkan,
                WebGpu.BackendType.OpenGL => VideoAdapterBackendType.OpenGl,
                WebGpu.BackendType.OpenGLES => VideoAdapterBackendType.OpenGles,
                _ => throw new ArgumentOutOfRangeException()
            };

            var type = value.AdapterType switch
            {
                WebGpu.AdapterType.DiscreteGPU => VideoAdapterType.DiscreteGpu,
                WebGpu.AdapterType.IntegratedGPU => VideoAdapterType.IntegratedGpu,
                WebGpu.AdapterType.CPU => VideoAdapterType.Cpu,
                WebGpu.AdapterType.Unknown => VideoAdapterType.Unknown,
                _ => throw new ArgumentOutOfRangeException()
            };

            return new VideoAdapterProperties(name, driverDescription, vendorName, architecture, (int)value.VendorID,
                (int)value.DeviceID, backendType, type);
        }
    }
}
