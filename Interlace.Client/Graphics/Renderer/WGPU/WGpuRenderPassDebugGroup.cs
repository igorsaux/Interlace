using System.Diagnostics;
using System.Runtime.InteropServices;
using Interlace.Vendor.WGPU;

namespace Interlace.Client.Graphics.Renderer.WGPU;

public sealed class WGpuRenderPassDebugGroup : IRenderPassDebugGroup
{
    private IntPtr _label;
    private IntPtr _renderPass;

    public WGpuRenderPassDebugGroup(IntPtr renderPass, string label)
    {
        unsafe
        {
            Debug.Assert(renderPass != IntPtr.Zero);

            _renderPass = renderPass;
            _label = Marshal.StringToHGlobalAnsi(label);

            WebGpu.PushRenderPassDebugGroup(_renderPass, (byte*)_label);
        }
    }

    public void Dispose()
    {
        Destroy();
        GC.SuppressFinalize(this);
    }

    private void Destroy()
    {
        if (_renderPass != IntPtr.Zero)
        {
            WebGpu.PopRenderPassDebugGroup(_renderPass);
            _renderPass = IntPtr.Zero;
        }

        if (_label != IntPtr.Zero)
        {
            Marshal.FreeHGlobal(_label);
            _label = IntPtr.Zero;
        }
    }

    ~WGpuRenderPassDebugGroup()
    {
        Destroy();
    }
}
