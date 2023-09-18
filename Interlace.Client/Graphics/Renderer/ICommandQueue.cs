using JetBrains.Annotations;

namespace Interlace.Client.Graphics.Renderer;

[PublicAPI]
public interface ICommandQueue
{
    void Submit(ICommandBuffer commands);
}

[PublicAPI]
public interface IOwnedCommandQueue : ICommandQueue, IDisposable
{
}
