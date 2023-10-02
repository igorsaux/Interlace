using Interlace.Shared.Application.Hooks;
using Interlace.Shared.Configuration;
using Interlace.Shared.IoC;
using Interlace.Shared.Logging;
using SteamAudio;

namespace Interlace.Client.SteamAudio;

internal sealed class SteamAudioManager : ISteamAudioManager, IInitializeHook
{
    [Dependency] private readonly ILogManager _log = default!;
    [Dependency] private readonly IConfigurationManager _cfg = default!;

    private IPL.Context _context;
    private IPL.Hrtf _hrtf;
    private IPL.AudioSettings _audioSettings;
    private IPL.SimulationSettings _simulationSettings;
    private ISawmill _sawmill = default!;
    private static ISawmill _steamAudioSawmill = default!;

    public void Initialize()
    {
        _sawmill = _log.GetSawmill("clyde.steam_audio");

        _sawmill.Debug($"Steam Audio version: {IPL.VersionMajor}.{IPL.VersionMinor}.{IPL.VersionPatch}");

        InitializeContext();
        InitializeHrtf();
        InitializeSimulationSettings();
        InitializePlugin();
    }

    private void InitializeContext()
    {
        _steamAudioSawmill = _log.GetSawmill("steam_audio");

        var ctxSettings = new IPL.ContextSettings
        {
            Version = IPL.Version,
            LogCallback = LogCallback
        };

        var result = IPL.ContextCreate(in ctxSettings, out _context);

        if (result != IPL.Error.Success)
            throw new InvalidOperationException();
    }

    private void InitializePlugin()
    {
        _sawmill.Debug("Steam Audio FMOD plugin initialization");

        Plugin.Initialize(_context.Handle);
        Plugin.SetSimulationSettings(_simulationSettings);
        Plugin.SetHRTF(_hrtf.Handle);
    }

    private void InitializeHrtf()
    {
        _sawmill.Debug("HRTF initialization");

        _audioSettings = new IPL.AudioSettings
        {
            FrameSize = _cfg.GetValue<int>(CVars.SteamAudioFrameSize),
            SamplingRate = _cfg.GetValue<int>(CVars.SteamAudioSamplingRate)
        };

        var hrtfSettings = new IPL.HrtfSettings
        {
            Type = IPL.HrtfType.Default
        };

        var result = IPL.HrtfCreate(_context, in _audioSettings, in hrtfSettings, out _hrtf);

        if (result != IPL.Error.Success)
            throw new InvalidOperationException();
    }

    private void InitializeSimulationSettings()
    {
        var enableDirectSimulation = _cfg.GetValue<bool>(CVars.SteamAudioDirectSimulation);
        var enablePathingSimulation = _cfg.GetValue<bool>(CVars.SteamAudioPathingSimulation);
        var enableReflectionSimulation = _cfg.GetValue<bool>(CVars.SteamAudioReflectionSimulation);

        IPL.SimulationFlags flags = 0;

        if (enableDirectSimulation)
            flags |= IPL.SimulationFlags.Direct;

        if (enablePathingSimulation)
            flags |= IPL.SimulationFlags.Pathing;

        if (enableReflectionSimulation)
            flags |= IPL.SimulationFlags.Reflections;

        if (!Enum.TryParse<IPL.ReflectionEffectType>(_cfg.GetValue<string>(CVars.SteamAudioReflectionType), out var reflectionType))
            reflectionType = IPL.ReflectionEffectType.Hybrid;

        _simulationSettings = new IPL.SimulationSettings
        {
            Flags = flags,
            SceneType = IPL.SceneType.Default,
            ReflectionType = reflectionType,
            MaxNumRays = _cfg.GetValue<int>(CVars.SteamAudioMaxNumRays),
            NumDiffuseSamples = _cfg.GetValue<int>(CVars.SteamAudioNumDiffuseSamples),
            MaxDuration = _cfg.GetValue<float>(CVars.SteamAudioMaxDuration),
            MaxOrder = _cfg.GetValue<int>(CVars.SteamAudioMaxOrder),
            MaxNumSources = _cfg.GetValue<int>(CVars.SteamAudioMaxNumSources),
            NumThreads = _cfg.GetValue<int>(CVars.SteamAudioNumThreads),
            FrameSize = _audioSettings.FrameSize,
            SamplingRate = _audioSettings.SamplingRate
        };

        _sawmill.Debug("Simulation settings:");
        _sawmill.Debug($"Flags: Direct: {enableDirectSimulation}, Pathing: {enablePathingSimulation}, Reflection: {enableReflectionSimulation}");
        _sawmill.Debug($"SceneType: {_simulationSettings.SceneType}");
        _sawmill.Debug($"ReflectionType: {_simulationSettings.ReflectionType}");
        _sawmill.Debug($"MaxNumRays: {_simulationSettings.MaxNumRays}");
        _sawmill.Debug($"NumDiffuseSamples: {_simulationSettings.NumDiffuseSamples}");
        _sawmill.Debug($"MaxDuration: {_simulationSettings.MaxDuration}");
        _sawmill.Debug($"MaxOrder: {_simulationSettings.MaxOrder}");
        _sawmill.Debug($"MaxNumSources: {_simulationSettings.MaxNumSources}");
        _sawmill.Debug($"NumThreads: {_simulationSettings.NumThreads}");
        _sawmill.Debug($"FrameSize: {_simulationSettings.FrameSize}");
        _sawmill.Debug($"SamplingRate: {_simulationSettings.SamplingRate}");
    }

    private static void LogCallback(IPL.LogLevel level, string message)
    {
        var logLevel = level switch
        {
            IPL.LogLevel.Info => LogLevel.Info,
            IPL.LogLevel.Warning => LogLevel.Warning,
            IPL.LogLevel.Error => LogLevel.Error,
            IPL.LogLevel.Debug => LogLevel.Debug,
            _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
        };

        _steamAudioSawmill.Log(logLevel, message);
    }
}
