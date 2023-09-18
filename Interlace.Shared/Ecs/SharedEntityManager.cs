using Arch.Core;

namespace Interlace.Shared.Ecs;

public abstract class SharedEntityManager : IEntityManager
{
    protected World? World;

    public abstract void Initialize();

    public abstract void Shutdown();
}
