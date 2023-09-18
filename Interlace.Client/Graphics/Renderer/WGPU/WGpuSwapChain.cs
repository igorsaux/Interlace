using System.Diagnostics;
using System.Runtime.InteropServices;
using Interlace.Vendor.WGPU;

namespace Interlace.Client.Graphics.Renderer.WGPU;

internal sealed class WGpuSwapChain : IOwnedSwapChain
{
    private IntPtr _handle;
    private IntPtr _label;

    public WGpuSwapChain(IntPtr handle, IntPtr label)
    {
        Debug.Assert(handle != IntPtr.Zero);

        _handle = handle;
        _label = label;
    }

    public void Dispose()
    {
        Destroy();
        GC.SuppressFinalize(this);
    }

    public void Present()
    {
        Debug.Assert(_handle != IntPtr.Zero);

        WebGpu.PresentSwapChain(_handle);
    }

    public IOwnedTextureView? TryGetTextureView()
    {
        Debug.Assert(_handle != IntPtr.Zero);

        var handle = WebGpu.GetCurrentSwapChainTextureView(_handle);

        if (handle == IntPtr.Zero)
            return null;

        return new WGpuTextureView(handle);
    }

    private void Destroy()
    {
        if (_handle == IntPtr.Zero)
            return;

        if (_label != IntPtr.Zero)
        {
            Marshal.FreeHGlobal(_label);
            _label = IntPtr.Zero;
        }

        WebGpu.ReleaseSwapChain(_handle);
        _handle = IntPtr.Zero;
    }

    ~WGpuSwapChain()
    {
        Destroy();
    }
}
