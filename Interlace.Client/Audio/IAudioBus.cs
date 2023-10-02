using JetBrains.Annotations;

namespace Interlace.Client.Audio;

[PublicAPI]
public interface IAudioBus
{
    public void SetVolume(float volume);

    public float GetVolume();
}
