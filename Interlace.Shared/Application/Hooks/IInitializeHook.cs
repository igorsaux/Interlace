using JetBrains.Annotations;

namespace Interlace.Shared.Application.Hooks;

[PublicAPI]
public interface IInitializeHook
{
    void Initialize();
}
