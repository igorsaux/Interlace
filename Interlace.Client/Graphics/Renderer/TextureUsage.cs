using JetBrains.Annotations;

namespace Interlace.Client.Graphics.Renderer;

[PublicAPI]
public enum TextureUsage
{
    None = 0x00000000,
    CopySrc = 0x00000001,
    CopyDst = 0x00000002,
    TextureBinding = 0x00000004,
    StorageBinding = 0x00000008,
    RenderAttachment = 0x00000010
}
