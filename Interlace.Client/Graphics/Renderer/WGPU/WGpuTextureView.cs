using System.Diagnostics;
using Interlace.Vendor.WGPU;

namespace Interlace.Client.Graphics.Renderer.WGPU;

internal sealed class WGpuTextureView : IOwnedTextureView
{
    private IntPtr _handle;

    public WGpuTextureView(IntPtr handle)
    {
        Debug.Assert(handle != IntPtr.Zero);

        _handle = handle;
    }

    public void Dispose()
    {
        Destroy();
        GC.SuppressFinalize(this);
    }

    public IColorAttachment AsColorAttachment(ColorAttachmentOptions options)
    {
        Debug.Assert(_handle != IntPtr.Zero);

        return new WGpuColorAttachment(new WebGpu.RenderPassColorAttachment
        {
            View = _handle,
            ResolveTarget = IntPtr.Zero,
            LoadOp = options.LoadOp.ToWebGpuLoadOp(),
            StoreOp = options.StoreOp.ToWebGpuStoreOp(),
            ClearValue = options.ClearValue.ToWebGpuColor()
        });
    }

    private void Destroy()
    {
        if (_handle == IntPtr.Zero)
            return;

        WebGpu.ReleaseTextureView(_handle);
        _handle = IntPtr.Zero;
    }

    ~WGpuTextureView()
    {
        Dispose();
    }
}
