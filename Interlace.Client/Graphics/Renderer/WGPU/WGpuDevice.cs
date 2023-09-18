using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Interlace.Shared.Logging;
using Interlace.Vendor.WGPU;

namespace Interlace.Client.Graphics.Renderer.WGPU;

internal sealed class WGpuDevice : IOwnedVideoDevice
{
    private static readonly Dictionary<IntPtr, (WGpuDevice, IVideoDevice.VideoDeviceErrorCallback)> ErrorCallbacks =
        new();

    private static readonly ReaderWriterLockSlim Lock = new();
    private readonly WGpuAdapter _adapter;
    private readonly IOwnedCommandQueue _queue;
    private readonly WebGpu.Limits _requiredLimits;

    private IntPtr _handle;
    private IntPtr _label;

    public WGpuDevice(IntPtr handle, WGpuAdapter adapter, WebGpu.Limits requiredLimits, IntPtr label)
    {
        Debug.Assert(handle != IntPtr.Zero);

        _handle = handle;
        _adapter = adapter;
        _requiredLimits = requiredLimits;
        _label = label;

        Properties = _adapter.Properties;
        Limits = requiredLimits.ToVideoAdapterLimits();
        _queue = CreateQueue();
    }

    public void Dispose()
    {
        Destroy();
        GC.SuppressFinalize(this);
    }

    public void LogInfo(ISawmill sawmill)
    {
        sawmill.Info("--- VIDEO DEVICE INFO ---");
        sawmill.Info("Type: {0}", Properties.Type.ToString());
        sawmill.Info("DeviceID: 0x{0:X}", Properties.DeviceId);
        sawmill.Info("Name: {0}", Properties.Name);
        sawmill.Info("Vendor: {0} (0x{1:X})", Properties.VendorName, Properties.VendorId);
        sawmill.Info("Architecture: {0}", Properties.Architecture);
        sawmill.Info("Driver description: {0}", Properties.DriverDescription);
        sawmill.Info("Backend: {0}", Properties.BackendType.ToString());
        sawmill.Info("Limits:");
        sawmill.Info("MaxTextureDimension1D: {0}", _requiredLimits.MaxTextureDimension1D);
        sawmill.Info("MaxTextureDimension2D: {0}", _requiredLimits.MaxTextureDimension2D);
        sawmill.Info("MaxTextureDimension3D: {0}", _requiredLimits.MaxTextureDimension3D);
        sawmill.Info("MaxTextureArrayLayers: {0}", _requiredLimits.MaxTextureArrayLayers);
        sawmill.Info("MaxBindGroups: {0}", _requiredLimits.MaxBindGroups);
        sawmill.Info("MaxBindingsPerBindGroup: {0}", _requiredLimits.MaxBindingsPerBindGroup);
        sawmill.Info("MaxDynamicUniformBuffersPerPipelineLayout: {0}",
            _requiredLimits.MaxDynamicUniformBuffersPerPipelineLayout);
        sawmill.Info("MaxDynamicStorageBuffersPerPipelineLayout: {0}",
            _requiredLimits.MaxDynamicStorageBuffersPerPipelineLayout);
        sawmill.Info("MaxSampledTexturesPerShaderStage: {0}", _requiredLimits.MaxSampledTexturesPerShaderStage);
        sawmill.Info("MaxSamplersPerShaderStage: {0}", _requiredLimits.MaxSamplersPerShaderStage);
        sawmill.Info("MaxStorageBuffersPerShaderStage: {0}", _requiredLimits.MaxStorageBuffersPerShaderStage);
        sawmill.Info("MaxStorageTexturesPerShaderStage: {0}", _requiredLimits.MaxStorageTexturesPerShaderStage);
        sawmill.Info("MaxUniformBuffersPerShaderStage: {0}", _requiredLimits.MaxUniformBuffersPerShaderStage);
        sawmill.Info("MaxUniformBufferBindingSize: {0}", _requiredLimits.MaxUniformBufferBindingSize);
        sawmill.Info("MaxStorageBufferBindingSize: {0}", _requiredLimits.MaxStorageBufferBindingSize);
        sawmill.Info("MinUniformBufferOffsetAlignment: {0}", _requiredLimits.MinUniformBufferOffsetAlignment);
        sawmill.Info("MinStorageBufferOffsetAlignment: {0}", _requiredLimits.MinStorageBufferOffsetAlignment);
        sawmill.Info("MaxVertexBuffers: {0}", _requiredLimits.MaxVertexBuffers);
        sawmill.Info("MaxBufferSize: {0}", _requiredLimits.MaxBufferSize);
        sawmill.Info("MaxVertexAttributes: {0}", _requiredLimits.MaxVertexAttributes);
        sawmill.Info("MaxVertexBufferArrayStride: {0}", _requiredLimits.MaxVertexBufferArrayStride);
        sawmill.Info("MaxInterStageShaderComponents: {0}", _requiredLimits.MaxInterStageShaderComponents);
        sawmill.Info("MaxInterStageShaderVariables: {0}", _requiredLimits.MaxInterStageShaderVariables);
        sawmill.Info("MaxColorAttachments: {0}", _requiredLimits.MaxColorAttachments);
        sawmill.Info("MaxColorAttachmentBytesPerSample: {0}", _requiredLimits.MaxColorAttachmentBytesPerSample);
        sawmill.Info("MaxComputeWorkgroupStorageSize: {0}", _requiredLimits.MaxComputeWorkgroupStorageSize);
        sawmill.Info("MaxComputeInvocationsPerWorkgroup: {0}", _requiredLimits.MaxComputeInvocationsPerWorkgroup);
        sawmill.Info("MaxComputeWorkgroupSizeX: {0}", _requiredLimits.MaxComputeWorkgroupSizeX);
        sawmill.Info("MaxComputeWorkgroupSizeY: {0}", _requiredLimits.MaxComputeWorkgroupSizeY);
        sawmill.Info("MaxComputeWorkgroupSizeZ: {0}", _requiredLimits.MaxComputeWorkgroupSizeZ);
        sawmill.Info("MaxComputeWorkgroupsPerDimension: {0}", _requiredLimits.MaxComputeWorkgroupsPerDimension);
        sawmill.Info("--- VIDEO DEVICE INFO ---");
    }

    public ICommandQueue Queue => _queue;

    public IOwnedCommandEncoder CreateCommandEncoder(CommandEncoderOptions options)
    {
        unsafe
        {
            Debug.Assert(_handle != IntPtr.Zero);

            var label = string.IsNullOrEmpty(options.Label) ? IntPtr.Zero : Marshal.StringToHGlobalAnsi(options.Label);
            var descriptor = new WebGpu.CommandEncoderDescriptor
            {
                Label = (byte*)label,
                NextInChain = null
            };

            var handle = WebGpu.CreateCommandEncoder(_handle, ref descriptor);

            if (handle == IntPtr.Zero)
                throw new InvalidOperationException("Can't create a command encoder");

            return new WGpuCommandEncoder(handle, label);
        }
    }

    public IOwnedSwapChain CreateSwapChain(ISurface surface, SwapChainOptions options)
    {
        Debug.Assert(_handle != IntPtr.Zero);

        unsafe
        {
            var webGpuSurface = (WGpuSurface)surface;
            var label = string.IsNullOrEmpty(options.Label) ? null : (byte*)Marshal.StringToHGlobalAnsi(options.Label);
            var textureFormat = options.TextureFormat == TextureFormat.Any
                ? _adapter.GetPreferredTextureFormat(webGpuSurface)
                : options.TextureFormat;
            var descriptor = new WebGpu.SwapChainDescriptor
            {
                NextInChain = null,
                Label = label,
                PresentMode = options.PresentMode.ToWebGpuPresentMode(),
                Format = textureFormat.ToWebGpuTextureFormat(),
                Usage = options.TextureUsage.ToWebGpuTextureUsage(),
                Width = (uint)options.Size.X,
                Height = (uint)options.Size.Y
            };

            return CreateWGpuSwapChain(webGpuSurface, descriptor);
        }
    }

    public void SetErrorCallback(IVideoDevice.VideoDeviceErrorCallback callback)
    {
        Lock.EnterWriteLock();

        try
        {
            unsafe
            {
                if (ErrorCallbacks.ContainsKey(_handle))
                    throw new InvalidOperationException("Callback already set");

                ErrorCallbacks.Add(_handle, (this, callback));

                WebGpu.SetUncapturedDeviceErrorCallback(_handle, &ErrorCallback, (void*)_handle);
            }
        }
        finally
        {
            Lock.ExitWriteLock();
        }
    }

    public VideoAdapterProperties Properties { get; }
    public VideoAdapterLimits Limits { get; }

    private IOwnedCommandQueue CreateQueue()
    {
        Debug.Assert(_handle != IntPtr.Zero);

        var handle = WebGpu.GetDeviceQueue(_handle);

        if (handle == IntPtr.Zero)
            throw new InvalidOperationException("Can't get device's queue");

        return new WGpuQueue(handle);
    }

    private WGpuSwapChain CreateWGpuSwapChain(WGpuSurface surface, WebGpu.SwapChainDescriptor descriptor)
    {
        unsafe
        {
            Debug.Assert(_handle != IntPtr.Zero);
            Debug.Assert(surface.Handle != IntPtr.Zero);

            var handle = WebGpu.CreateSwapChain(_handle, surface.Handle, ref descriptor);

            if (handle == IntPtr.Zero)
                throw new InvalidOperationException("Can't create a swap chain");

            return new WGpuSwapChain(handle, (IntPtr)descriptor.Label);
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

        _queue.Dispose();

        Lock.EnterWriteLock();

        try
        {
            ErrorCallbacks.Remove(_handle);
        }
        finally
        {
            Lock.ExitWriteLock();
        }

        WebGpu.ReleaseDevice(_handle);

        _handle = IntPtr.Zero;
    }

    ~WGpuDevice()
    {
        Destroy();
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static unsafe void ErrorCallback(WebGpu.ErrorType errorType, byte* message, void* userdata)
    {
        Lock.EnterReadLock();

        try
        {
            var (device, callback) = ErrorCallbacks[(IntPtr)userdata];
            var convertedMessage = Marshal.PtrToStringUTF8((IntPtr)message) ?? string.Empty;

            callback.Invoke(device, errorType, convertedMessage);
        }
        finally
        {
            Lock.ExitReadLock();
        }
    }
}
