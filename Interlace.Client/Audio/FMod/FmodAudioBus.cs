using FMOD;
using FMOD.Studio;

namespace Interlace.Client.Audio.FMod;

public sealed class FmodAudioBus : IAudioBus
{
    private readonly Bus _bus;

    public FmodAudioBus(Bus bus)
    {
        _bus = bus;
    }

    public void SetVolume(float volume)
    {
        var result = _bus.setVolume(volume);

        if (result != RESULT.OK)
            throw new InvalidOperationException(Error.String(result));
    }

    public float GetVolume()
    {
        var result = _bus.getVolume(out var volume);

        if (result != RESULT.OK)
            throw new InvalidOperationException(Error.String(result));

        return volume;
    }
}
