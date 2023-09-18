namespace Interlace.Client.Windowing;

public sealed class WindowsWindowInfo : NativeWindowInfo
{
    public readonly IntPtr HInstance;
    public readonly IntPtr Hwnd;

    public WindowsWindowInfo(IntPtr hwnd, IntPtr hInstance)
    {
        Hwnd = hwnd;
        HInstance = hInstance;
    }
}
