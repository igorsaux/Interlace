using System.Diagnostics;
using System.Runtime.CompilerServices;
using Interlace.Vendor.WGPU;

namespace Interlace.Client.Graphics.Renderer.WGPU;

internal sealed class WGpuQueue : IOwnedCommandQueue
{
    private IntPtr _handle;

    public WGpuQueue(IntPtr handle)
    {
        Debug.Assert(handle != IntPtr.Zero);

        _handle = handle;
    }

    public void Dispose()
    {
        Destroy();
        GC.SuppressFinalize(this);
    }

    public void Submit(ICommandBuffer commands)
    {
        unsafe
        {
            Debug.Assert(_handle != IntPtr.Zero);

            var wgpuCommands = Unsafe.As<WGpuCommandBuffer>(commands);

            Debug.Assert(wgpuCommands.Handle != IntPtr.Zero);

            var commandsArray = stackalloc[] { wgpuCommands.Handle };

            WebGpu.SubmitQueue(_handle, 1, (IntPtr)commandsArray);
        }
    }

    private void Destroy()
    {
        if (_handle == IntPtr.Zero)
            return;

        WebGpu.ReleaseQueue(_handle);
        _handle = IntPtr.Zero;
    }

    ~WGpuQueue()
    {
        Destroy();
    }
}
