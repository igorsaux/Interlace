using System.Diagnostics;
using System.Runtime.InteropServices;
using Interlace.Vendor.WGPU;

namespace Interlace.Client.Graphics.Renderer.WGPU;

public sealed class WGpuCommandEncoderDebugGroup : ICommandEncoderDebugGroup
{
    private IntPtr _encoder;
    private IntPtr _label;

    public unsafe WGpuCommandEncoderDebugGroup(IntPtr encoder, string label)
    {
        Debug.Assert(encoder != IntPtr.Zero);

        _encoder = encoder;
        _label = Marshal.StringToHGlobalAnsi(label);

        WebGpu.PushCommandEncoderDebugGroup(_encoder, (byte*)_label);
    }

    public void Dispose()
    {
        Destroy();
        GC.SuppressFinalize(this);
    }

    private void Destroy()
    {
        if (_encoder != IntPtr.Zero)
        {
            WebGpu.PopCommandEncoderDebugGroup(_encoder);
            _encoder = IntPtr.Zero;
        }

        if (_label != IntPtr.Zero)
        {
            Marshal.FreeHGlobal(_label);
            _label = IntPtr.Zero;
        }
    }

    ~WGpuCommandEncoderDebugGroup()
    {
        Destroy();
    }
}
