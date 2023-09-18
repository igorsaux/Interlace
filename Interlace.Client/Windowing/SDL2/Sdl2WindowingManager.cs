using System.Diagnostics;
using Interlace.Shared.Application;
using Interlace.Shared.Application.Hooks;
using Interlace.Shared.Configuration;
using Interlace.Shared.Graphics;
using Interlace.Shared.IoC;
using Interlace.Shared.Logging;
using Vendor.SDL2;

namespace Interlace.Client.Windowing.SDL2;

internal sealed class Sdl2WindowingManager : IWindowingManager, IInitializeHook, IPostInitializeHook, IShutdownHook
{
    [Dependency] private readonly IApplication _application = default!;
    [Dependency] private readonly IConfigurationManager _cfg = default!;
    [Dependency] private readonly ILogManager _log = default!;

    private Sdl2Window? _mainWindow;
    private ISawmill _sawmill = default!;
    private WindowMode _windowMode = WindowMode.Windowed;

    public void Initialize()
    {
        _sawmill = _log.GetSawmill("windowing.sdl2");

        Sdl2.Init(Sdl2.InitFlags.Events);
        Sdl2.SetAllLogPriority(Sdl2.LogPriority.Verbose);

        _sawmill.Info("SDL2 initialized");

        _cfg.SubscribeOnValueChanged<WindowMode>(CVars.GraphicsWindowMode, newValue =>
        {
            _windowMode = newValue;
            _mainWindow?.SetWindowMode(_windowMode);
        });
    }

    public void PostInitialize()
    {
        _sawmill.Debug("Creating main window");

        _mainWindow = Sdl2Window.CreateWindow(new WindowOptions
        {
            Title = _application.Title,
            Width = 300,
            Height = 300,
            Position = WindowPosition.Centered,
            Mode = _windowMode
        });
    }

    public void Shutdown()
    {
        _sawmill.Info("Destroying main window");

        _mainWindow?.Destroy();
        _mainWindow = null;

        Sdl2.Quit();

        _sawmill.Info("SDL2 shutdown");
    }

    public event Action? Quit;

    public IWindow? MainWindow => _mainWindow;

    public void FrameUpdate()
    {
        unsafe
        {
            var ev = new Sdl2.Event();

            while (Sdl2.PollEvent(&ev) != 0)
                if (ev.Type == Sdl2.EventType.Quit)
                    Quit?.Invoke();
                else if (ev.Type == Sdl2.EventType.WindowEvent)
                    OnWindowEvent(ev.Window);
        }
    }

    private void OnWindowEvent(Sdl2.WindowEvent ev)
    {
        Debug.Assert(_mainWindow is not null);
        Debug.Assert(ev.Type == Sdl2.EventType.WindowEvent);

        switch (ev.WindowEventType)
        {
            case Sdl2.WindowEventType.None:
                break;
            case Sdl2.WindowEventType.Shown:
                _mainWindow.OnWindowShown();

                break;
            case Sdl2.WindowEventType.Hidden:
                _mainWindow.OnWindowHidden();

                break;
            case Sdl2.WindowEventType.Exposed:
                break;
            case Sdl2.WindowEventType.Moved:
                break;
            case Sdl2.WindowEventType.Resized:
                break;
            case Sdl2.WindowEventType.SizeChanged:
                _mainWindow.OnWindowResized(ev.Data1, ev.Data2);

                break;
            case Sdl2.WindowEventType.Minimized:
                _mainWindow.OnWindowMinimized();

                break;
            case Sdl2.WindowEventType.Maximized:
                _mainWindow.OnWindowMaximized();

                break;
            case Sdl2.WindowEventType.Restored:
                break;
            case Sdl2.WindowEventType.Enter:
                break;
            case Sdl2.WindowEventType.Leave:
                break;
            case Sdl2.WindowEventType.FocusGained:
                break;
            case Sdl2.WindowEventType.FocusLost:
                break;
            case Sdl2.WindowEventType.Close:
                break;
            case Sdl2.WindowEventType.TakeFocus:
                break;
            case Sdl2.WindowEventType.HitTest:
                break;
            case Sdl2.WindowEventType.IccprofChanged:
                break;
            case Sdl2.WindowEventType.DisplayChanged:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
