using System.Runtime.InteropServices;

namespace SteamAudio;

public static class Plugin
{
    public const string Library = "phonon_fmod";

    [DllImport(Library, EntryPoint = "iplFMODInitialize", CallingConvention = CallingConvention.Cdecl)]
    public static extern void Initialize(IntPtr ctx);

    [DllImport(Library, EntryPoint = "iplFMODSetHRTF", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetHRTF(IntPtr hrtf);

    [DllImport(Library, EntryPoint = "iplFMODSetSimulationSettings", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetSimulationSettings(IPL.SimulationSettings simulationSettings);

    [DllImport(Library, EntryPoint = "iplFMODSetReverbSource", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetReverbSource(IntPtr reverbSource);

    [DllImport(Library, EntryPoint = "iplFMODTerminate", CallingConvention = CallingConvention.Cdecl)]
    public static extern void Terminate();

    [DllImport(Library, EntryPoint = "iplFMODAddSource", CallingConvention = CallingConvention.Cdecl)]
    public static extern int AddSource(IntPtr source);

    [DllImport(Library, EntryPoint = "iplFMODRemoveSource", CallingConvention = CallingConvention.Cdecl)]
    public static extern void RemoveSource(int handle);
}
