using JetBrains.Annotations;

namespace Interlace.Client.Graphics.Renderer;

[PublicAPI]
public interface IColorAttachment
{
}

[PublicAPI]
public sealed class ColorAttachmentOptions
{
    public readonly Color ClearValue;
    public readonly LoadOp LoadOp;
    public readonly StoreOp StoreOp;

    public ColorAttachmentOptions(LoadOp loadOp, StoreOp storeOp, Color clearValue)
    {
        LoadOp = loadOp;
        StoreOp = storeOp;
        ClearValue = clearValue;
    }
}
