using Interlace.Shared.Logging;
using JetBrains.Annotations;

namespace Interlace.Client.Graphics.Renderer;

[PublicAPI]
public interface IVideoDevice
{
    public delegate void VideoDeviceErrorCallback(IVideoDevice videoDevice, object error, string message);

    VideoAdapterProperties Properties { get; }

    VideoAdapterLimits Limits { get; }

    ICommandQueue Queue { get; }

    IOwnedCommandEncoder CreateCommandEncoder(CommandEncoderOptions options);

    IOwnedSwapChain CreateSwapChain(ISurface surface, SwapChainOptions options);

    void SetErrorCallback(VideoDeviceErrorCallback callback);

    void LogInfo(ISawmill sawmill);
}

public interface IOwnedVideoDevice : IVideoDevice, IDisposable
{
}

public sealed class RequestVideoDeviceOptions
{
    public readonly string? Label;
    public readonly VideoAdapterLimits Limits;

    public RequestVideoDeviceOptions(VideoAdapterLimits limits, string? label = null)
    {
        Limits = limits;
        Label = label;
    }
}
