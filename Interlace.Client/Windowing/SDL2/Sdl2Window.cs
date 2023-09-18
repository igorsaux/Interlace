using System.Diagnostics;
using Interlace.Shared.Graphics;
using Silk.NET.Maths;
using Vendor.SDL2;

namespace Interlace.Client.Windowing.SDL2;

internal sealed class Sdl2Window : IWindow
{
    private IntPtr _handle;
    private bool _isHidden;
    private bool _isMinimized;

    private Sdl2Window(IntPtr handle)
    {
        Debug.Assert(handle != IntPtr.Zero);

        _handle = handle;
    }

    public event IWindow.WindowResizedDelegate? WindowResized;
    public event Action? WindowMinimized;
    public event Action? WindowMaximized;
    public event Action? WindowShown;
    public event Action? WindowHidden;

    public bool IsVisible => !_isHidden && !_isMinimized;

    public Vector2D<int> Size
    {
        get
        {
            Debug.Assert(_handle != IntPtr.Zero);
            var width = 0;
            var height = 0;

            Sdl2.GetWindowSize(_handle, ref width, ref height);

            return new Vector2D<int>(width, height);
        }
    }

    public NativeWindowInfo GetNativeWindowInfo()
    {
        if (_handle == IntPtr.Zero)
            throw new NullReferenceException($"{nameof(_handle)} is zero");

        var systemInfo = new Sdl2.SystemWmInfo
        {
            Version = Sdl2.GetVersion()
        };

        Sdl2.GetWindowWmInfo(_handle, ref systemInfo);

        switch (systemInfo.WmType)
        {
            case Sdl2.SystemWmType.Unknown:
                throw new NotSupportedException();
            case Sdl2.SystemWmType.Windows:
                var windowsInfo = systemInfo.AnySystemMessage.Win;

                return new WindowsWindowInfo(windowsInfo.Hwnd, windowsInfo.HInstance);
            case Sdl2.SystemWmType.X11:
            case Sdl2.SystemWmType.DirectFb:
            case Sdl2.SystemWmType.Cocoa:
            case Sdl2.SystemWmType.UiKit:
            case Sdl2.SystemWmType.Wayland:
            case Sdl2.SystemWmType.Mir:
            case Sdl2.SystemWmType.WinRt:
            case Sdl2.SystemWmType.Android:
            case Sdl2.SystemWmType.Vivante:
            case Sdl2.SystemWmType.Os2:
            case Sdl2.SystemWmType.Haiku:
            case Sdl2.SystemWmType.Kmsdrm:
            case Sdl2.SystemWmType.RiscOs:

                throw new NotSupportedException();
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void SetWindowMode(WindowMode mode)
    {
        Debug.Assert(_handle != IntPtr.Zero);

        Sdl2.SetWindowFullscreen(_handle,
            mode == WindowMode.Windowed ? 0 : Sdl2.WindowFlags.FullscreenDesktop);
    }

    public static Sdl2Window CreateWindow(WindowOptions options)
    {
        var position = options.Position;
        var mode = options.Mode;
        var x = options.X;
        var y = options.Y;

        if (position != WindowPosition.Custom)
            x = y = position == WindowPosition.Centered ? Sdl2.WindowCentered : Sdl2.WindowUndefined;

        var flags = Sdl2.WindowFlags.Shown | Sdl2.WindowFlags.AllowHighDpi;

        if (mode == WindowMode.Windowed)
            flags |= Sdl2.WindowFlags.Resizable;
        else
            flags |= Sdl2.WindowFlags.FullscreenDesktop | Sdl2.WindowFlags.Borderless;

        var handle = Sdl2.CreateWindow(options.Title, x, y, options.Width, options.Height, flags);

        if (handle == IntPtr.Zero)
            throw new InvalidOperationException("Can't create a window!");

        return new Sdl2Window(handle);
    }

    public void Destroy()
    {
        Sdl2.DestroyWindow(_handle);

        _handle = IntPtr.Zero;
    }

    ~Sdl2Window()
    {
        Destroy();
    }

    internal void OnWindowResized(int x, int y)
    {
        WindowResized?.Invoke(x, y);
    }

    internal void OnWindowMinimized()
    {
        _isMinimized = true;

        WindowMinimized?.Invoke();
    }

    internal void OnWindowShown()
    {
        _isHidden = false;

        WindowShown?.Invoke();
    }

    internal void OnWindowHidden()
    {
        _isHidden = true;

        WindowHidden?.Invoke();
    }

    internal void OnWindowMaximized()
    {
        _isMinimized = false;

        WindowMaximized?.Invoke();
    }
}
