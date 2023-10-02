using JetBrains.Annotations;

namespace Interlace.Client.Audio;

[PublicAPI]
public interface IAudioVca
{
    public void SetVolume(float value);

    public float GetVolume();
}
