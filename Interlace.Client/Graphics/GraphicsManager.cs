using System.Diagnostics;
using Interlace.Client.Graphics.Renderer;
using Interlace.Client.Graphics.Renderer.WGPU;
using Interlace.Client.Windowing;
using Interlace.Shared.Application.Hooks;
using Interlace.Shared.Configuration;
using Interlace.Shared.Graphics;
using Interlace.Shared.IoC;
using Interlace.Shared.Logging;

namespace Interlace.Client.Graphics;

public sealed class GraphicsManager : IGraphicsManager, IInitializeHook, IPostInitializeHook, IShutdownHook
{
    [Dependency] private readonly IConfigurationManager _cfg = default!;
    [Dependency] private readonly ILogManager _log = default!;
    [Dependency] private readonly IWindowingManager _windowing = default!;

    private readonly HashSet<Viewport> _viewports = new();
    private VideoAdapterBackendType _backendType = VideoAdapterBackendType.Any;

    private IOwnedRenderer? _renderer;
    private IOwnedSurface? _surface;
    private IOwnedSwapChain? _swapChain;
    private IOwnedVideoAdapter? _videoAdapter;
    private IOwnedVideoDevice? _videoDevice;

    private bool _vsync;
    private ISawmill _sawmill = default!;

    public ISurface Surface => _surface!;
    public IVideoAdapter VideoAdapter => _videoAdapter!;
    public IVideoDevice VideoDevice => _videoDevice!;
    public IEnumerable<Viewport> Viewports => _viewports;

    public void Initialize()
    {
        _sawmill = _log.GetSawmill("graphics");

        _renderer = new WGpuRenderer();
        _renderer.Initialize();
        _renderer.SetLogLevel(LogLevel.Warning);

        var wgpuSawmill = _log.GetSawmill("wgpu_native");

        _renderer.SetLogCallback((level, message) => { wgpuSawmill.Log(level, "{0}", message); });

        _cfg.SubscribeOnValueChanged<bool>(CVars.GraphicsVsync, newValue =>
        {
            _vsync = newValue;
            UpdateSurface();
        });
        _cfg.SubscribeOnValueChanged<VideoAdapterBackendType>(CVars.GraphicsBackend,
            newValue =>
            {
                _backendType = newValue;
                UpdateVideoDevice();
            });
    }

    public void PostInitialize()
    {
        Debug.Assert(_windowing.MainWindow is not null, $"Initialize {nameof(IWindowingManager)} first");
        Debug.Assert(_renderer is not null);

        _sawmill.Trace("Creating a surface...");

        _surface = _renderer.CreateSurface(_windowing.MainWindow, new SurfaceOptions("Main window surface"));

        _sawmill.Trace("Requesting an adapter...");

        if (!_renderer.TryRequestWGpuAdapter(
                new RequestAdapterOptions(Surface, VideoAdapterPowerPreference.HighPower, _backendType),
                out var adapter))
            throw new InvalidOperationException("Can't get an adapter");

        _videoAdapter = adapter;

        _sawmill.Trace("Requesting a device...");

        if (!VideoAdapter.TryRequestVideoDevice(new RequestVideoDeviceOptions(VideoAdapter.Limits, "Main device"),
                out var device))
            throw new InvalidOperationException("Can't get a device");

        device.LogInfo(_sawmill);

        _videoDevice = device;
        _videoDevice.SetErrorCallback((_, _, message) => { _sawmill.Fatal("{0}", message); });

        _swapChain = _videoDevice.CreateSwapChain(_surface,
            new SwapChainOptions(_windowing.MainWindow.Size, _vsync ? PresentMode.Fifo : PresentMode.Immediate,
                label: "Window swap chain"));

        _windowing.MainWindow.WindowResized += OnWindowResized;
    }

    public void Shutdown()
    {
        _sawmill.Info("Shutdown...");

        _swapChain?.Dispose();
        _videoDevice?.Dispose();
        _videoAdapter?.Dispose();
        _surface?.Dispose();
        _renderer?.Dispose();
    }

    public void Render()
    {
        Debug.Assert(_swapChain is not null);

        using var view = _swapChain.TryGetTextureView();

        if (view is null)
            return;

        var viewAttachment =
            view.AsColorAttachment(new ColorAttachmentOptions(LoadOp.Clear, StoreOp.Store, new Color(255, 20, 147)));

        using (var encoder = VideoDevice.CreateCommandEncoder(new CommandEncoderOptions("Main encoder")))
        {
            using (var renderPass =
                   encoder.CreateRenderPassScope(new RenderPassOptions(viewAttachment, "Main render pass")))
            {
            }

            using var commands = encoder.Finish(new CommandBufferOptions("Main commands buffer"));

            VideoDevice.Queue.Submit(commands);
        }

        _swapChain.Present();
    }

    public void AddViewport(Viewport viewport)
    {
        if (!_viewports.Add(viewport))
            _sawmill.Warning("Viewport already added");
    }

    public void RemoveViewport(Viewport viewport)
    {
        if (!_viewports.Remove(viewport))
            _sawmill.Warning("Trying to delete invalid viewport");
    }

    private void OnWindowResized(int width, int height)
    {
        UpdateSurface();
    }

    private void UpdateVideoDevice()
    {
        _videoDevice?.Dispose();
        _videoAdapter?.Dispose();

        if (_renderer is null)
            return;

        if (_surface is null)
            return;

        if (!_renderer.TryRequestWGpuAdapter(
                new RequestAdapterOptions(_surface, VideoAdapterPowerPreference.HighPower, _backendType),
                out var adapter))
            throw new InvalidOperationException("Can't get an adapter");

        _videoAdapter = adapter;

        UpdateSurface();
    }

    private void UpdateSurface()
    {
        _swapChain?.Dispose();
        _surface?.Dispose();

        if (_swapChain is null)
            return;

        if (_surface is null)
            return;

        if (_renderer is null)
            return;

        if (_videoDevice is null)
            return;

        if (_windowing.MainWindow is null)
            return;

        var presentMode = _vsync ? PresentMode.Fifo : PresentMode.Immediate;

        _sawmill.Trace("Creating a surface...");

        _surface = _renderer.CreateSurface(_windowing.MainWindow, new SurfaceOptions("Main window surface"));

        _sawmill.Trace("Creating a swap chain...");

        _swapChain = _videoDevice.CreateSwapChain(_surface,
            new SwapChainOptions(_windowing.MainWindow.Size, presentMode, label: "Window swap chain"));
    }
}
