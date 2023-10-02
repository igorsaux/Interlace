using JetBrains.Annotations;

namespace Interlace.Client.Audio;

[PublicAPI]
public interface IAudioEventDescription
{
    public bool IsSnapshot();

    public bool Is3D();

    public IAudioEventInstance CreateInstance();
}
