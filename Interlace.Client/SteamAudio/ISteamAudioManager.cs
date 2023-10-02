using Interlace.Shared.Application;
using JetBrains.Annotations;

namespace Interlace.Client.SteamAudio;

[PublicAPI]
public interface ISteamAudioManager : IManager
{
    public void Initialize();
}
