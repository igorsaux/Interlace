using JetBrains.Annotations;

namespace Interlace.Client.Graphics.Renderer;

[PublicAPI]
public interface ICommandEncoder
{
    void InsertDebugMarker(string label);

    IOwnedCommandBuffer Finish(CommandBufferOptions options);

    ICommandEncoderDebugGroup CreateDebugGroupScope(string label);

    IOwnedRenderPass CreateRenderPassScope(RenderPassOptions options);
}

[PublicAPI]
public interface IOwnedCommandEncoder : ICommandEncoder, IDisposable
{
}

[PublicAPI]
public sealed class CommandEncoderOptions
{
    public readonly string? Label;

    public CommandEncoderOptions(string? label = null)
    {
        Label = label;
    }
}
