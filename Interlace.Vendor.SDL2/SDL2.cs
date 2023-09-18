using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace Vendor.SDL2;

[PublicAPI]
public static partial class Sdl2
{
    [PublicAPI]
    public enum EventType : uint
    {
        FirstEvent = 0,
        Quit = 0x100,
        Terminating,
        LowMemory,
        WillEnterBackground,
        DidEnterBackground,
        WillEnterForeground,
        DidEnterForeground,
        LocalChanged,
        DisplayEvent = 0x150,
        WindowEvent = 0x200,
        SysWmEvent,
        KeyDown = 0x300,
        KeyUp,
        TextEditing,
        TextInput,
        KeymapChanged,
        TextEditingExt,
        MouseMotion = 0x400,
        MouseButtonDown,
        MouseButtonUp,
        MouseWheel,
        JoyAxisMotion = 0x600,
        JoyBallMotion,
        JoyHatMotion,
        JoyButtonDown,
        JoyButtonUp,
        JoyDeviceAdded,
        JoyDeviceRemoved,
        JoyBatteryUpdated,
        ControllerAxisMotion = 0x650,
        ControllerButtonDown,
        ControllerButtonUp,
        ControllerDeviceAdded,
        ControllerDeviceRemoved,
        ControllerDeviceRemapped,
        ControllerTouchPadDown,
        ControllerTouchPadMotion,
        ControllerTouchPadUp,
        ControllerSensorUpdate,
        FingerDown = 0x700,
        FingerUp,
        FingerMotion,
        DollarGesture = 0x800,
        DollarRecord,
        MultiGesture,
        ClipboardUpdate = 0x900,
        DropFile = 0x1000,
        DropText,
        DropBegin,
        DropComplete,
        AudioDeviceAdded = 0x1100,
        AudioDeviceRemoved,
        SensorUpdate = 0x1200,
        RenderTargetsReset = 0x2000,
        RenderDeviceReset,
        PollSentinel = 0x7F00,
        UserEvent = 0x8000,
        LastEvent = 0xFFFF
    }

    [PublicAPI]
    [Flags]
    public enum InitFlags
    {
        Timer = 0x00000001,
        Audio = 0x00000010,
        Video = 0x00000020,
        Joystick = 0x00000200,
        Haptic = 0x00001000,
        GameController = 0x00002000,
        Events = 0x00004000,
        Sensor = 0x00008000
    }

    [PublicAPI]
    public enum LogPriority
    {
        Verbose = 1,
        Debug,
        Info,
        Warn,
        Error,
        Critical
    }

    [PublicAPI]
    public enum SystemWmType
    {
        Unknown,
        Windows,
        X11,
        DirectFb,
        Cocoa,
        UiKit,
        Wayland,
        Mir,
        WinRt,
        Android,
        Vivante,
        Os2,
        Haiku,
        Kmsdrm,
        RiscOs
    }

    [PublicAPI]
    public enum WindowEventType : byte
    {
        None,
        Shown,
        Hidden,
        Exposed,
        Moved,
        Resized,
        SizeChanged,
        Minimized,
        Maximized,
        Restored,
        Enter,
        Leave,
        FocusGained,
        FocusLost,
        Close,
        TakeFocus,
        HitTest,
        IccprofChanged,
        DisplayChanged
    }

    [PublicAPI]
    [Flags]
    public enum WindowFlags
    {
        Fullscreen = 0x00000001,
        OpenGl = 0x00000002,
        Shown = 0x00000004,
        Hidden = 0x00000008,
        Borderless = 0x00000010,
        Resizable = 0x00000020,
        Minimized = 0x00000040,
        Maximized = 0x00000080,
        MouseGrabbed = 0x00000100,
        InputFocus = 0x00000200,
        MouseFocus = 0x00000400,
        FullscreenDesktop = Fullscreen | 0x00001000,
        Foreign = 0x00000800,
        AllowHighDpi = 0x00002000,
        Capture = 0x00004000,
        AlwaysOnTop = 0x00008000,
        SkipTaskbar = 0x00010000,
        Utility = 0x00020000,
        Tooltip = 0x00040000,
        PopupMenu = 0x00080000,
        KeyboardGrabbed = 0x00100000,
        Vulkan = 0x10000000,
        Metal = 0x20000000
    }

    public const string LibraryName = "SDL2";

    public const int WindowCentered = 0x2FFF0000 | 0;
    public const int WindowUndefined = 0x1FFF0000 | 0;
    private static Version? _version;

    [PublicAPI]
    public static Version GetVersion()
    {
        if (_version is not null)
            return _version.Value;

        var newVersion = new Version();

        GetVersion(ref newVersion);
        _version = newVersion;

        return _version.Value;
    }

    [LibraryImport(LibraryName, EntryPoint = "SDL_Init")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void Init(InitFlags flags);

    [LibraryImport(LibraryName, EntryPoint = "SDL_Quit")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void Quit();

    [LibraryImport(LibraryName, EntryPoint = "SDL_LogSetAllPriority")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void SetAllLogPriority(LogPriority priority);

    [LibraryImport(LibraryName, EntryPoint = "SDL_CreateWindow")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr CreateWindow([MarshalAs(UnmanagedType.LPUTF8Str)] string title, int x, int y,
        int width, int height, WindowFlags flags);

    [LibraryImport(LibraryName, EntryPoint = "SDL_DestroyWindow")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void DestroyWindow(IntPtr handle);

    [LibraryImport(LibraryName, EntryPoint = "SDL_PollEvent")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static unsafe partial int PollEvent(Event* anyEvent);

    [LibraryImport(LibraryName, EntryPoint = "SDL_GetWindowWMInfo")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void GetWindowWmInfo(IntPtr window, ref SystemWmInfo info);

    [LibraryImport(LibraryName, EntryPoint = "SDL_GetVersion")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void GetVersion(ref Version version);

    [LibraryImport(LibraryName, EntryPoint = "SDL_GetWindowSize")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void GetWindowSize(IntPtr window, ref int width, ref int height);

    [LibraryImport(LibraryName, EntryPoint = "SDL_SetWindowFullscreen")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void SetWindowFullscreen(IntPtr window, WindowFlags flags);

    [PublicAPI]
    [StructLayout(LayoutKind.Sequential)]
    public struct Version
    {
        public byte Major;
        public byte Minor;
        public byte Patch;
    }

    [PublicAPI]
    [StructLayout(LayoutKind.Sequential)]
    public struct SystemWmInfo
    {
        public Version Version;
        public SystemWmType WmType;
        public AnySystemInfo AnySystemMessage;
    }

    [PublicAPI]
    [StructLayout(LayoutKind.Explicit)]
    public struct AnySystemInfo
    {
        [FieldOffset(0)] public WindowsSystemInfo Win;
    }

    [PublicAPI]
    [StructLayout(LayoutKind.Sequential)]
    public struct WindowsSystemInfo
    {
        public IntPtr Hwnd;
        public IntPtr HDC;
        public IntPtr HInstance;
    }

    [PublicAPI]
    [StructLayout(LayoutKind.Explicit, Size = 128)]
    public struct Event
    {
        [FieldOffset(0)] public EventType Type;
        [FieldOffset(0)] public WindowEvent Window;
        [FieldOffset(0)] public QuitEvent Quit;
    }

    [PublicAPI]
    [StructLayout(LayoutKind.Sequential)]
    public struct WindowEvent
    {
        public EventType Type;
        public uint Timestamp;
        public uint WindowId;
        public WindowEventType WindowEventType;
        private byte _padding1;
        private byte _padding2;
        private byte _padding3;
        public int Data1;
        public int Data2;
    }

    [PublicAPI]
    [StructLayout(LayoutKind.Sequential)]
    public struct QuitEvent
    {
        public EventType Type;
        public int Timestamp;
    }
}
