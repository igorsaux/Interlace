using JetBrains.Annotations;

namespace Interlace.Shared.Application.Hooks;

[PublicAPI]
public interface IShutdownHook
{
    void Shutdown();
}
