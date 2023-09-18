using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Interlace.Client.Windowing;
using Interlace.Shared.Graphics;
using Interlace.Shared.IoC;
using Interlace.Shared.Logging;
using Interlace.Vendor.WGPU;

namespace Interlace.Client.Graphics.Renderer.WGPU;

internal sealed class WGpuRenderer : IOwnedRenderer
{
    [Shared.IoC.Dependency] private readonly ILogManager _log = default!;
    
    private static IRenderer.RendererLogCallback? _logCallback;

    private WGpuInstance? _instance;
    private ISawmill _sawmill = default!;

    public WGpuRenderer()
    {
        IoCManager.Instance.InjectDependencies(this);
    }

    public IOwnedSurface CreateSurface(IWindow window, SurfaceOptions options)
    {
        unsafe
        {
            Debug.Assert(_instance is not null, "Initialize WebGPU first");

            _sawmill.Trace("Creating a surface");

            var label = string.IsNullOrEmpty(options.Label) ? null : (byte*)Marshal.StringToHGlobalAnsi(options.Label);
            var nativeWindowInfo = window.GetNativeWindowInfo();

            switch (nativeWindowInfo)
            {
                case WindowsWindowInfo windowsInfo:
                    var windowsDescriptor =
                        new WebGpu.SurfaceDescriptorFromWindowsHWND
                        {
                            Chain = new WebGpu.ChainedStruct
                            {
                                Type = WebGpu.SurfaceType.SurfaceDescriptorFromWindowsHWND,
                                Next = null
                            },
                            // Cursed, but it's the only way it works
                            HInstance = windowsInfo.HInstance,
                            HWND = windowsInfo.Hwnd
                        };

                    var descriptor = new WebGpu.SurfaceDescriptor
                    {
                        NextInChain = (WebGpu.ChainedStruct*)&windowsDescriptor,
                        Label = label
                    };

                    return _instance.CreateWGpuSurface(descriptor);
                default:
                    throw new NotSupportedException();
            }
        }
    }

    public bool TryRequestWGpuAdapter(RequestAdapterOptions options,
        [NotNullWhen(true)] out IOwnedVideoAdapter? adapter)
    {
        Debug.Assert(_instance is not null, $"Initialize {nameof(IRenderer)} first");

        var surface = (WGpuSurface)options.Surface;
        var webGpuOptions = new WebGpu.RequestAdapterOptions
        {
            CompatibleSurface = surface.Handle,
            BackendType = options.BackendType switch
            {
                VideoAdapterBackendType.Any => WebGpu.BackendType.Undefined,
                VideoAdapterBackendType.WebGpu => WebGpu.BackendType.WebGPU,
                VideoAdapterBackendType.D3D11 => WebGpu.BackendType.D3D11,
                VideoAdapterBackendType.D3D12 => WebGpu.BackendType.D3D12,
                VideoAdapterBackendType.Metal => WebGpu.BackendType.Metal,
                VideoAdapterBackendType.Vulkan => WebGpu.BackendType.Vulkan,
                VideoAdapterBackendType.OpenGl => WebGpu.BackendType.OpenGL,
                VideoAdapterBackendType.OpenGles => WebGpu.BackendType.OpenGLES,
                _ => throw new ArgumentOutOfRangeException()
            },
            PowerPreference = options.PowerPreference switch
            {
                VideoAdapterPowerPreference.Any => WebGpu.PowerPreference.Undefined,
                VideoAdapterPowerPreference.LowPower => WebGpu.PowerPreference.LowPower,
                VideoAdapterPowerPreference.HighPower => WebGpu.PowerPreference.HighPerformance,
                _ => throw new ArgumentOutOfRangeException()
            },
            NextInChain = null,
            ForceFallbackAdapter = 0
        };

        var result = _instance.TryRequestWGpuAdapter(webGpuOptions, out var webGpuAdapter);

        adapter = webGpuAdapter;

        return result;
    }

    public void Initialize()
    {
        _sawmill = _log.GetSawmill("renderer.webgpu");

        var instanceDescriptor = new WebGpu.InstanceDescriptor
        {
            NextInChain = null
        };
        var handle = WebGpu.CreateInstance(ref instanceDescriptor);

        if (handle == IntPtr.Zero)
            throw new InvalidOperationException("Can't create an WebGPU instance");

        _instance = new WGpuInstance(handle);

        _sawmill.Info("WebGPU initialized");
    }

    public void SetLogLevel(LogLevel? level)
    {
        if (level is null)
            WebGpu.SetLogLevel(WebGpu.LogLevel.Off);
        else
            WebGpu.SetLogLevel(level.Value.ToWebGpuLogLevel());
    }

    public void SetLogCallback(IRenderer.RendererLogCallback callback)
    {
        unsafe
        {
            if (_logCallback is not null)
                throw new InvalidOperationException("Log callback already set");

            _logCallback = callback;

            WebGpu.SetLogCallback(&ErrorCallback, null);
        }
    }

    public void Dispose()
    {
        if (_instance is null)
            return;

        _instance?.Dispose();

        _sawmill.Info("WebGPU dispose");
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static unsafe void ErrorCallback(WebGpu.LogLevel level, byte* message, void* userdata)
    {
        var msg = Marshal.PtrToStringAnsi((IntPtr)message) ?? string.Empty;

        _logCallback?.Invoke(level.ToLogLevel(), msg);
    }
}
