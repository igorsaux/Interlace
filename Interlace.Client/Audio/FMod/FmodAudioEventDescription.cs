using FMOD;
using FMOD.Studio;

namespace Interlace.Client.Audio.FMod;

internal sealed class FmodEventDescription : IAudioEventDescription
{
    private EventDescription _eventDescription;

    internal FmodEventDescription(EventDescription eventDescription)
    {
        _eventDescription = eventDescription;
    }

    public bool IsSnapshot()
    {
        var result = _eventDescription.isSnapshot(out var isSnapshot);

        if (result != RESULT.OK)
            throw new InvalidOperationException(Error.String(result));

        return isSnapshot;
    }

    public bool Is3D()
    {
        var result = _eventDescription.is3D(out var is3D);

        if (result != RESULT.OK)
            throw new InvalidOperationException(Error.String(result));

        return is3D;
    }

    public IAudioEventInstance CreateInstance()
    {
        var result = _eventDescription.createInstance(out var instance);

        if (result != RESULT.OK)
            throw new InvalidOperationException(Error.String(result));

        return new FmodAudioEventInstance(instance, this);
    }
}
