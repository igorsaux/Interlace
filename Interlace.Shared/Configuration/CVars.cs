using Interlace.Shared.Graphics;
using Interlace.Shared.Logging;
using JetBrains.Annotations;

namespace Interlace.Shared.Configuration;

[PublicAPI]
public static class CVars
{
    // GRAPHICS

    public static readonly CVarDeclaration GraphicsVsync =
        CVarDeclaration.Create("graphics.vsync", true, CVarFlag.Archive | CVarFlag.ClientOnly);

    public static readonly CVarDeclaration GraphicsBackend =
        CVarDeclaration.Create("graphics.backend", VideoAdapterBackendType.Any, CVarFlag.Archive | CVarFlag.ClientOnly);

    public static readonly CVarDeclaration GraphicsWindowMode =
        CVarDeclaration.Create("graphics.window_mode", WindowMode.Windowed, CVarFlag.Archive | CVarFlag.ClientOnly);

   
    // GAME

    public static readonly CVarDeclaration GameTicks =
        CVarDeclaration.Create("game.ticks", 30, CVarFlag.Repliacted | CVarFlag.Archive);

    public static readonly CVarDeclaration GameLanguage =
        CVarDeclaration.Create("game.language", "en-US", CVarFlag.Repliacted | CVarFlag.Archive);

    public static readonly CVarDeclaration GameFallbackLanguage =
        CVarDeclaration.Create("game.fallback_language", "en-US", CVarFlag.Repliacted | CVarFlag.Archive);
    
    // LOGGING

    public static readonly CVarDeclaration LoggingLevel =
        CVarDeclaration.Create("logging.level", LogLevel.Trace, CVarFlag.Archive);
    
    // AUDIO
    
    public static readonly CVarDeclaration AudioChannels =
        CVarDeclaration.Create("audio.channels", 128, CVarFlag.ClientOnly | CVarFlag.Archive);
    
    /*
     * STEAM AUDIO
     */

    // https://valvesoftware.github.io/steam-audio/doc/capi/simulation.html#_CPPv421IPLSimulationSettings

    public static readonly CVarDeclaration SteamAudioFrameSize =
        CVarDeclaration.Create("steam_audio.frame_size", 1024, CVarFlag.ClientOnly | CVarFlag.Archive);

    public static readonly CVarDeclaration SteamAudioSamplingRate =
        CVarDeclaration.Create("steam_audio.sampling_rate", 44100, CVarFlag.ClientOnly | CVarFlag.Archive);

    public static readonly CVarDeclaration SteamAudioDirectSimulation =
        CVarDeclaration.Create("steam_audio.direct_simulation", true, CVarFlag.ClientOnly | CVarFlag.Archive);

    public static readonly CVarDeclaration SteamAudioPathingSimulation =
        CVarDeclaration.Create("steam_audio.pathing_simulation", true, CVarFlag.ClientOnly | CVarFlag.Archive);

    public static readonly CVarDeclaration SteamAudioReflectionSimulation =
        CVarDeclaration.Create("steam_audio.reflection_simulation", true, CVarFlag.ClientOnly | CVarFlag.Archive);

    public static readonly CVarDeclaration SteamAudioReflectionType =
        CVarDeclaration.Create("steam_audio.reflection_type", "Hybrid", CVarFlag.ClientOnly | CVarFlag.Archive);

    public static readonly CVarDeclaration SteamAudioMaxNumRays =
        CVarDeclaration.Create("steam_audio.max_num_rays", 4096, CVarFlag.ClientOnly | CVarFlag.Archive);

    public static readonly CVarDeclaration SteamAudioNumDiffuseSamples =
        CVarDeclaration.Create("steam_audio.num_diffuse_samples", 32, CVarFlag.ClientOnly | CVarFlag.Archive);

    public static readonly CVarDeclaration SteamAudioMaxDuration =
        CVarDeclaration.Create("steam_audio.max_duration", 2.0f, CVarFlag.ClientOnly | CVarFlag.Archive);

    public static readonly CVarDeclaration SteamAudioMaxOrder =
        CVarDeclaration.Create("steam_audio.max_order", 2, CVarFlag.ClientOnly | CVarFlag.Archive);

    public static readonly CVarDeclaration SteamAudioMaxNumSources =
        CVarDeclaration.Create("steam_audio.max_num_sources", 8, CVarFlag.ClientOnly | CVarFlag.Archive);

    public static readonly CVarDeclaration SteamAudioNumThreads =
        CVarDeclaration.Create("steam_audio.num_thread", 2, CVarFlag.ClientOnly | CVarFlag.Archive);
}
