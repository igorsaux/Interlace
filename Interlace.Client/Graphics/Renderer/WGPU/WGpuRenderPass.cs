using System.Diagnostics;
using System.Runtime.InteropServices;
using Interlace.Vendor.WGPU;

namespace Interlace.Client.Graphics.Renderer.WGPU;

public sealed class WGpuRenderPass : IOwnedRenderPass
{
    private readonly List<IntPtr> _markers = new();
    private IntPtr _handle;
    private IntPtr _label;

    public WGpuRenderPass(IntPtr handle, IntPtr label)
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

    public void InsertDebugMarker(string label)
    {
        unsafe
        {
            Debug.Assert(_handle != IntPtr.Zero);

            var marker = Marshal.StringToHGlobalAnsi(label);

            WebGpu.InsertRenderPassDebugMarker(_handle, (byte*)marker);
            _markers.Add(marker);
        }
    }

    public IRenderPassDebugGroup CreateDebugGroupScope(string label)
    {
        Debug.Assert(_handle != IntPtr.Zero);

        return new WGpuRenderPassDebugGroup(_handle, label);
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

        foreach (var marker in _markers) Marshal.FreeHGlobal(marker);

        _markers.Clear();

        WebGpu.EndRenderPassEncoder(_handle);
        WebGpu.ReleaseRenderPassEncoder(_handle);

        _handle = IntPtr.Zero;
    }

    ~WGpuRenderPass()
    {
        Destroy();
    }
}
