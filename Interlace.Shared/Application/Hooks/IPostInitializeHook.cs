using JetBrains.Annotations;

namespace Interlace.Shared.Application.Hooks;

[PublicAPI]
public interface IPostInitializeHook
{
    void PostInitialize();
}
