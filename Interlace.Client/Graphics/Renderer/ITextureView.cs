using JetBrains.Annotations;

namespace Interlace.Client.Graphics.Renderer;

[PublicAPI]
public interface ITextureView
{
    IColorAttachment AsColorAttachment(ColorAttachmentOptions options);
}

[PublicAPI]
public interface IOwnedTextureView : ITextureView, IDisposable
{
}
