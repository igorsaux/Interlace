using System.Diagnostics;
using System.Runtime.InteropServices;
using Interlace.Vendor.WGPU;

namespace Interlace.Client.Graphics.Renderer.WGPU;

public sealed class WGpuCommandBuffer : IOwnedCommandBuffer
{
    private IntPtr _label;
    internal IntPtr Handle;

    public WGpuCommandBuffer(IntPtr handle, IntPtr label)
    {
        Debug.Assert(handle != IntPtr.Zero);

        Handle = handle;
        _label = label;
    }

    public void Dispose()
    {
        Destroy();
        GC.SuppressFinalize(this);
    }

    private void Destroy()
    {
        if (Handle == IntPtr.Zero)
            return;

        if (_label != IntPtr.Zero)
        {
            Marshal.FreeHGlobal(_label);
            _label = IntPtr.Zero;
        }

        WebGpu.ReleaseCommandBuffer(Handle);
        Handle = IntPtr.Zero;
    }

    ~WGpuCommandBuffer()
    {
        Destroy();
    }
}
