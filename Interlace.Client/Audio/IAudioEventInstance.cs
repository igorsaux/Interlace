using JetBrains.Annotations;
using Silk.NET.Maths;

namespace Interlace.Client.Audio;

[PublicAPI]
public interface IAudioEventInstance : IDisposable
{
    public IAudioEventDescription EventDescription { get; }

    public void Start();

    public void Stop(StopMode stopMode = StopMode.AllowFadeOut);

    public void SetPaused(bool paused);

    public bool GetPaused();

    public PlaybackState GetPlaybackState();

    public void SetParameter(string name, float value);

    public void SetParameter(string name, string value);

    public float GetParameter(string name);

    public void Set3DAttributes(Vector3D<float> position, Vector3D<float> velocity, Vector3D<float> forward, Vector3D<float> up);

    public bool IsDisposed();
}

[PublicAPI]
public enum StopMode : byte
{
    Immediate,
    AllowFadeOut
}

[PublicAPI]
public enum PlaybackState : byte
{
    Playing,
    Sustaining,
    Stopped,
    Starting,
    Stopping
}
