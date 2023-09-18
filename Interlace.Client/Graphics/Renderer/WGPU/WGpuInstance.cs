using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Interlace.Vendor.WGPU;

namespace Interlace.Client.Graphics.Renderer.WGPU;

internal sealed class WGpuInstance : IDisposable
{
    private IntPtr _handle;

    public WGpuInstance(IntPtr handle)
    {
        Debug.Assert(handle != IntPtr.Zero);

        _handle = handle;
    }

    public void Dispose()
    {
        Destroy();
        GC.SuppressFinalize(this);
    }

    public bool TryRequestWGpuAdapter(WebGpu.RequestAdapterOptions options,
        [NotNullWhen(true)] out WGpuAdapter? adapter)
    {
        unsafe
        {
            adapter = null;

            var userdata = new UserData();

            WebGpu.RequestAdapter(_handle, &options, &RequestAdapterCallback, Unsafe.AsPointer(ref userdata));

            while (userdata.Ready == false)
            {
            }

            if (userdata.AdapterHandle == IntPtr.Zero || userdata.AdapterStatus != WebGpu.RequestAdapterStatus.Success)
                return false;

            adapter = new WGpuAdapter(userdata.AdapterHandle);

            return true;
        }
    }

    public WGpuSurface CreateWGpuSurface(WebGpu.SurfaceDescriptor descriptor)
    {
        unsafe
        {
            var handle = WebGpu.CreateSurface(_handle, ref descriptor);

            if (handle == IntPtr.Zero)
                throw new InvalidOperationException("Can't create a surface");

            return new WGpuSurface(handle, (IntPtr)descriptor.Label);
        }
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static unsafe void RequestAdapterCallback(WebGpu.RequestAdapterStatus status, IntPtr adapter, byte* message,
        void* userdata)
    {
        var data = Unsafe.AsRef<UserData>(userdata);

        data.AdapterStatus = status;
        data.AdapterHandle = adapter;
        data.Ready = true;
    }

    private void Destroy()
    {
        if (_handle == IntPtr.Zero)
            return;

        WebGpu.ReleaseInstance(_handle);

        _handle = IntPtr.Zero;
    }

    ~WGpuInstance()
    {
        Destroy();
    }

    private sealed class UserData
    {
        public IntPtr AdapterHandle = IntPtr.Zero;
        public WebGpu.RequestAdapterStatus AdapterStatus = WebGpu.RequestAdapterStatus.Unknown;
        public bool Ready;
    }
}
