using System.Diagnostics.CodeAnalysis;
using Interlace.Shared.Application;
using JetBrains.Annotations;

namespace Interlace.Client.Audio;

[PublicAPI]
public interface IAudioManager : IManager
{
    bool TryGetEvent(string path, [NotNullWhen(true)] out IAudioEventDescription? ev);

    void SetParameter(string key, float value);

    void SetParameter(string key, string value);

    bool TryGetVca(string path, [NotNullWhen(true)] out IAudioVca? vca);

    bool TryGetBus(string path, [NotNullWhen(true)] out IAudioBus? bus);

    void TickUpdate();
}
