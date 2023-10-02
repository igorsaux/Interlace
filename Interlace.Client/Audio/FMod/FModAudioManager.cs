using System.Diagnostics.CodeAnalysis;
using FMOD;
using FMOD.Studio;
using Interlace.Shared.Application.Hooks;
using Interlace.Shared.Configuration;
using Interlace.Shared.IoC;
using Interlace.Shared.Logging;
using Interlace.Shared.Resources;
using INITFLAGS = FMOD.Studio.INITFLAGS;

namespace Interlace.Client.Audio.FMod;

internal sealed class FmodAudioManager : IAudioManager, IInitializeHook, IShutdownHook
{
    [Dependency] private readonly IResourcesManager _resources = default!;
    [Dependency] private readonly IConfigurationManager _cfg = default!;
    [Dependency] private readonly ILogManager _log = default!;

    private ISawmill _sawmill = default!;
    private FMOD.System _coreSystem;
    private FMOD.Studio.System _studioSystem;

    public bool TryGetEvent(string path, [NotNullWhen(true)] out IAudioEventDescription? ev)
    {
        ev = null;

        var result = _studioSystem.getEvent(path, out var tryEv);

        if (result != RESULT.OK)
        {
            if (result == RESULT.ERR_EVENT_NOTFOUND)
                return false;

            var message = Error.String(result);
            _sawmill.Error(message);

            throw new InvalidOperationException(message);
        }

        ev = new FmodEventDescription(tryEv);
        return true;
    }

    public void SetParameter(string key, float value)
    {
        _studioSystem.setParameterByName(key, value);
    }

    public void SetParameter(string key, string value)
    {
        _studioSystem.setParameterByNameWithLabel(key, value);
    }

    public bool TryGetVca(string path, [NotNullWhen(true)] out IAudioVca? vca)
    {
        var result = _studioSystem.getVCA(path, out var fmodVca);

        if (result != RESULT.OK)
        {
            _sawmill.Error(Error.String(result));

            vca = null;
            return false;
        }

        vca = new FmodAudioVca(fmodVca);
        return true;
    }

    public bool TryGetBus(string path, [NotNullWhen(true)] out IAudioBus? bus)
    {
        var result = _studioSystem.getBus(path, out var fmodBus);

        if (result != RESULT.OK)
        {
            _sawmill.Error(Error.String(result));

            bus = null;
            return false;
        }

        bus = new FmodAudioBus(fmodBus);
        return true;
    }

    public void TickUpdate()
    {
        _studioSystem.update();
    }

    public void Initialize()
    {
        _sawmill = _log.GetSawmill("clyde.fmod");

        _sawmill.Info("Initializing FMOD");

        var result = FMOD.Studio.System.create(out _studioSystem);

        if (result != RESULT.OK)
        {
            _sawmill.Error(Error.String(result));
            return;
        }

        var studioFlags = INITFLAGS.NORMAL;
        var coreFlags = FMOD.INITFLAGS.NORMAL | FMOD.INITFLAGS._3D_RIGHTHANDED | FMOD.INITFLAGS.VOL0_BECOMES_VIRTUAL;

#if DEBUG
        studioFlags |= INITFLAGS.LIVEUPDATE | INITFLAGS.MEMORY_TRACKING;
        coreFlags |= FMOD.INITFLAGS.PROFILE_ENABLE | FMOD.INITFLAGS.MEMORY_TRACKING |
                     FMOD.INITFLAGS.PROFILE_METER_ALL;
#endif

        result = _studioSystem.initialize(_cfg.GetValue<int>(CVars.AudioChannels), studioFlags, coreFlags, IntPtr.Zero);

        if (result != RESULT.OK)
        {
            _sawmill.Error(Error.String(result));
            return;
        }

        _sawmill.Info("FMOD initialized");
        _studioSystem.getCoreSystem(out _coreSystem);

        LoadPlugins();
        LoadBanks();
    }

    public void Shutdown()
    {
        _sawmill.Info("Shutdown");

        _studioSystem.release();
    }

    private void LoadPlugins()
    {
        _sawmill.Info("Loading FMOD plugins");

        LoadSteamAudio();
    }

    private void LoadSteamAudio()
    {
        _sawmill.Info("Loading Steam Audio FMOD plugin");

        RESULT result;
        
        if (OperatingSystem.IsWindows())
            result = _coreSystem.loadPlugin("phonon_fmod.dll", out _);
        else
            result = _coreSystem.loadPlugin("libphonon_fmod.so", out _);

        if (result != RESULT.OK)
            throw new InvalidOperationException(Error.String(result));
    }

    private void LoadBanks()
    {
        _sawmill.Info("Loading banks");

        foreach (var bank in _resources.GetFiles(new ResourcePath("/Audio")))
        {
            _sawmill.Debug("Loading '{0}'", bank.Path);

            var result = _studioSystem.loadBankFile(_resources.ResourcePathToNative(bank), LOAD_BANK_FLAGS.NORMAL, out _);

            if (result != RESULT.OK)
                throw new InvalidOperationException(Error.String(result));
        }
    }
}
