using JetBrains.Annotations;

namespace Interlace.Client.Graphics.Renderer;

[PublicAPI]
public interface ICommandBuffer
{
}

[PublicAPI]
public interface IOwnedCommandBuffer : ICommandBuffer, IDisposable
{
}

[PublicAPI]
public sealed class CommandBufferOptions
{
    public readonly string? Label;

    public CommandBufferOptions(string? label)
    {
        Label = label;
    }
}
