using System.Diagnostics;
using System.Runtime.InteropServices;
using Interlace.Vendor.WGPU;

namespace Interlace.Client.Graphics.Renderer.WGPU;

internal sealed class WGpuSurface : IOwnedSurface
{
    private IntPtr _label;

    public WGpuSurface(IntPtr handle, IntPtr label)
    {
        Debug.Assert(handle != IntPtr.Zero);

        Handle = handle;
        _label = label;
    }

    internal IntPtr Handle { get; private set; }

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

        WebGpu.ReleaseSurface(Handle);

        Handle = IntPtr.Zero;
    }

    ~WGpuSurface()
    {
        Destroy();
    }
}
