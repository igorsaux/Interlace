using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Interlace.Vendor.WGPU;

namespace Interlace.Client.Graphics.Renderer.WGPU;

public sealed class WGpuCommandEncoder : IOwnedCommandEncoder
{
    private readonly List<IntPtr> _markerLabels = new();
    private IntPtr _handle;
    private IntPtr _label;

    public WGpuCommandEncoder(IntPtr handle, IntPtr label)
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

            var handle = Marshal.StringToHGlobalAnsi(label);

            _markerLabels.Add(handle);
            WebGpu.InsertCommandEncoderDebugMarker(_handle, (byte*)handle);
        }
    }

    public IOwnedCommandBuffer Finish(CommandBufferOptions options)
    {
        unsafe
        {
            Debug.Assert(_handle != IntPtr.Zero);

            var label = string.IsNullOrEmpty(options.Label) ? IntPtr.Zero : Marshal.StringToHGlobalAnsi(options.Label);
            var descriptor = new WebGpu.CommandBufferDescriptor
            {
                Label = (byte*)label,
                NextInChain = null
            };

            var handle = WebGpu.CommandEncoderFinish(_handle, ref descriptor);

            return new WGpuCommandBuffer(handle, label);
        }
    }

    public ICommandEncoderDebugGroup CreateDebugGroupScope(string label)
    {
        Debug.Assert(_handle != IntPtr.Zero);

        return new WGpuCommandEncoderDebugGroup(_handle, label);
    }

    public IOwnedRenderPass CreateRenderPassScope(RenderPassOptions options)
    {
        unsafe
        {
            Debug.Assert(_handle != IntPtr.Zero);

            var label = string.IsNullOrEmpty(options.Label) ? null : (byte*)Marshal.StringToHGlobalAnsi(options.Label);
            WebGpu.RenderPassColorAttachment? wGpuColorAttachment = options.ColorAttachment is null
                ? null
                : Unsafe.As<WGpuColorAttachment>(options.ColorAttachment).Attachment;

            WebGpu.RenderPassColorAttachment colorAttachment = new()
            {
                View = wGpuColorAttachment?.View ?? IntPtr.Zero,
                ResolveTarget = wGpuColorAttachment?.ResolveTarget ?? IntPtr.Zero,
                StoreOp = wGpuColorAttachment?.StoreOp ?? WebGpu.StoreOp.Undefined,
                LoadOp = wGpuColorAttachment?.LoadOp ?? WebGpu.LoadOp.Undefined,
                ClearValue = wGpuColorAttachment?.ClearValue ?? new WebGpu.Color()
            };

            var descriptor = new WebGpu.RenderPassDescriptor
            {
                NextInChain = null,
                Label = label,
                ColorAttachmentsCount = options.ColorAttachment is null ? 0 : 1,
                ColorAttachments = options.ColorAttachment is null ? null : &colorAttachment,
                DepthStencilAttachment = null,
                OcclusionQuerySet = IntPtr.Zero,
                TimestampWriteCount = 0,
                TimestampWrites = null
            };
            var handle = WebGpu.BeginCommandEncoderRenderPass(_handle, ref descriptor);

            if (handle == IntPtr.Zero)
                throw new InvalidOperationException("Can't create a render pass");

            return new WGpuRenderPass(handle, (IntPtr)label);
        }
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

        foreach (var label in _markerLabels) Marshal.FreeHGlobal(label);

        _markerLabels.Clear();

        WebGpu.ReleaseCommandEncoder(_handle);
        _handle = IntPtr.Zero;
    }

    ~WGpuCommandEncoder()
    {
        Destroy();
    }
}
