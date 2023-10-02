using FMOD;
using FMOD.Studio;

namespace Interlace.Client.Audio.FMod;

internal sealed class FmodAudioVca : IAudioVca
{
    private VCA _vca;

    public FmodAudioVca(VCA vca)
    {
        _vca = vca;
    }

    public void SetVolume(float value)
    {
        var result = _vca.setVolume(value);

        if (result != RESULT.OK)
            throw new InvalidOperationException(Error.String(result));
    }

    public float GetVolume()
    {
        var result = _vca.getVolume(out var value);

        if (result != RESULT.OK)
            throw new InvalidOperationException(Error.String(result));

        return value;
    }
}
