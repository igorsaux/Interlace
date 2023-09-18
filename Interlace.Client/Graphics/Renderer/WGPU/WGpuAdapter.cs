using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Interlace.Vendor.WGPU;

namespace Interlace.Client.Graphics.Renderer.WGPU;

internal sealed class WGpuAdapter : IOwnedVideoAdapter
{
    private IntPtr _handle;

    public WGpuAdapter(IntPtr handle)
    {
        Debug.Assert(handle != IntPtr.Zero);

        _handle = handle;

        Properties = GetWGpuAdapterProperties().ToVideoAdapterOptions();
        Limits = GetWGpuSupportedLimits().ToVideoAdapterLimits();
    }

    public void Dispose()
    {
        Destroy();
        GC.SuppressFinalize(this);
    }

    public TextureFormat GetPreferredTextureFormat(ISurface surface)
    {
        Debug.Assert(_handle != IntPtr.Zero);

        var wGpuSurface = Unsafe.As<WGpuSurface>(surface);

        return WebGpu.GetPreferredTextureFormat(wGpuSurface.Handle, _handle).ToTextureFormat();
    }

    public bool TryRequestVideoDevice(RequestVideoDeviceOptions options,
        [NotNullWhen(true)] out IOwnedVideoDevice? device)
    {
        Debug.Assert(_handle != IntPtr.Zero);

        unsafe
        {
            var requiredLimits = new WebGpu.RequiredLimits
            {
                NextInChain = null,
                Limits = options.Limits.ToWebGpuLimits()
            };
            var label = string.IsNullOrEmpty(options.Label) ? null : (byte*)Marshal.StringToHGlobalAnsi(options.Label);
            var webGpuOptions = new WebGpu.DeviceDescriptor
            {
                NextInChain = null,
                RequiredLimits = &requiredLimits,
                Label = label,
                DefaultQueue = new WebGpu.QueueDescriptor
                {
                    Label = null,
                    NextInChain = null
                },
                RequiredFeatures = null,
                RequiredFeaturesCount = 0,
                DeviceLostUserdata = null,
                DeviceLostCallback = null
            };

            var result = TryRequestWGpuDevice(webGpuOptions, out var webGpuDevice);
            device = webGpuDevice;

            return result;
        }
    }

    public VideoAdapterProperties Properties { get; }
    public VideoAdapterLimits Limits { get; }

    private WebGpu.Limits GetWGpuSupportedLimits()
    {
        Debug.Assert(_handle != IntPtr.Zero);

        var supportedLimits = new WebGpu.SupportedLimits();
        WebGpu.GetAdapterLimits(_handle, ref supportedLimits);

        return supportedLimits.Limits;
    }

    private WebGpu.AdapterProperties GetWGpuAdapterProperties()
    {
        Debug.Assert(_handle != IntPtr.Zero);

        var adapterProperties = new WebGpu.AdapterProperties();
        WebGpu.GetAdapterProperties(_handle, ref adapterProperties);

        return adapterProperties;
    }

    private bool TryRequestWGpuDevice(WebGpu.DeviceDescriptor descriptor, [NotNullWhen(true)] out WGpuDevice? device)
    {
        Debug.Assert(_handle != IntPtr.Zero);

        unsafe
        {
            device = null;

            var userdata = new UserData();

            WebGpu.RequestDevice(_handle, ref descriptor, &RequestDeviceCallback, Unsafe.AsPointer(ref userdata));

            while (userdata.Ready == false)
            {
            }

            if (userdata.DeviceHandle == IntPtr.Zero || userdata.RequestStatus != WebGpu.RequestDeviceStatus.Success)
                return false;

            device = new WGpuDevice(userdata.DeviceHandle, this, descriptor.RequiredLimits->Limits,
                (IntPtr)descriptor.Label);

            return true;
        }
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static unsafe void RequestDeviceCallback(WebGpu.RequestDeviceStatus status, IntPtr device, byte* message,
        void* userdata)
    {
        var data = Unsafe.AsRef<UserData>(userdata);

        data.RequestStatus = status;
        data.DeviceHandle = device;
        data.Ready = true;
    }

    private void Destroy()
    {
        if (_handle == IntPtr.Zero)
            return;

        WebGpu.ReleaseAdapter(_handle);

        _handle = IntPtr.Zero;
    }

    ~WGpuAdapter()
    {
        Destroy();
    }

    private sealed class UserData
    {
        public IntPtr DeviceHandle = IntPtr.Zero;
        public bool Ready;
        public WebGpu.RequestDeviceStatus RequestStatus = WebGpu.RequestDeviceStatus.Unknown;
    }
}
