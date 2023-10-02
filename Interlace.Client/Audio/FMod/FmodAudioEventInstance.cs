using FMOD;
using FMOD.Studio;
using Silk.NET.Maths;

namespace Interlace.Client.Audio.FMod;

internal sealed class FmodAudioEventInstance : IAudioEventInstance
{
    private EventInstance _eventInstance;

    internal FmodAudioEventInstance(EventInstance eventInstance, IAudioEventDescription eventDescription)
    {
        _eventInstance = eventInstance;
        EventDescription = eventDescription;
    }

    public IAudioEventDescription EventDescription { get; }

    public void Start()
    {
        var result = _eventInstance.start();

        if (result != RESULT.OK)
            throw new InvalidOperationException(Error.String(result));
    }

    public void Stop(StopMode stopMode = StopMode.AllowFadeOut)
    {
        var mode = stopMode switch
        {
            StopMode.Immediate => STOP_MODE.IMMEDIATE,
            StopMode.AllowFadeOut => STOP_MODE.ALLOWFADEOUT,
            _ => throw new ArgumentOutOfRangeException(nameof(stopMode), stopMode, null)
        };

        var result = _eventInstance.stop(mode);

        if (result != RESULT.OK)
            throw new InvalidOperationException(Error.String(result));
    }

    public void SetPaused(bool paused)
    {
        var result = _eventInstance.setPaused(paused);

        if (result != RESULT.OK)
            throw new InvalidOperationException(Error.String(result));
    }

    public bool GetPaused()
    {
        var result = _eventInstance.getPaused(out var paused);

        if (result != RESULT.OK)
            throw new InvalidOperationException(Error.String(result));

        return paused;
    }

    public PlaybackState GetPlaybackState()
    {
        var result = _eventInstance.getPlaybackState(out var state);

        if (result != RESULT.OK)
            throw new InvalidOperationException(Error.String(result));

        return state switch
        {
            PLAYBACK_STATE.PLAYING => PlaybackState.Playing,
            PLAYBACK_STATE.SUSTAINING => PlaybackState.Sustaining,
            PLAYBACK_STATE.STOPPED => PlaybackState.Stopped,
            PLAYBACK_STATE.STARTING => PlaybackState.Starting,
            PLAYBACK_STATE.STOPPING => PlaybackState.Stopping,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public void SetParameter(string name, float value)
    {
        var result = _eventInstance.setParameterByName(name, value);

        if (result != RESULT.OK)
            throw new InvalidOperationException(Error.String(result));
    }

    public void SetParameter(string name, string value)
    {
        var result = _eventInstance.setParameterByNameWithLabel(name, value);

        if (result != RESULT.OK)
            throw new InvalidOperationException(Error.String(result));
    }

    public float GetParameter(string name)
    {
        var result = _eventInstance.getParameterByName(name, out var value);

        if (result != RESULT.OK)
            throw new InvalidOperationException(Error.String(result));

        return value;
    }

    public void Set3DAttributes(Vector3D<float> position, Vector3D<float> velocity, Vector3D<float> forward, Vector3D<float> up)
    {
        var result = _eventInstance.set3DAttributes(new ATTRIBUTES_3D
        {
            position = new VECTOR
            {
                x = position.X,
                y = position.Y,
                z = position.Z
            },
            velocity = new VECTOR
            {
                x = velocity.X,
                y = velocity.Y,
                z = velocity.Z
            },
            forward = new VECTOR
            {
                x = forward.X,
                y = forward.Y,
                z = forward.Z
            },
            up = new VECTOR
            {
                x = up.X,
                y = up.Y,
                z = up.Z
            }
        });

        if (result != RESULT.OK)
            throw new InvalidOperationException(Error.String(result));
    }

    public bool IsDisposed()
    {
        return !_eventInstance.isValid();
    }

    public void Dispose()
    {
        if (IsDisposed())
            return;

        _eventInstance.release();
        GC.SuppressFinalize(this);
    }

    ~FmodAudioEventInstance()
    {
        _eventInstance.release();
    }
}
