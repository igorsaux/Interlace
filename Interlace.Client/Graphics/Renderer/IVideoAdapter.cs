using System.Diagnostics.CodeAnalysis;
using Interlace.Shared.Graphics;
using JetBrains.Annotations;

namespace Interlace.Client.Graphics.Renderer;

[PublicAPI]
public interface IVideoAdapter
{
    public VideoAdapterProperties Properties { get; }

    public VideoAdapterLimits Limits { get; }

    TextureFormat GetPreferredTextureFormat(ISurface surface);

    bool TryRequestVideoDevice(RequestVideoDeviceOptions options, [NotNullWhen(true)] out IOwnedVideoDevice? device);
}

[PublicAPI]
public interface IOwnedVideoAdapter : IVideoAdapter, IDisposable
{
}

[PublicAPI]
public sealed class RequestAdapterOptions
{
    public readonly VideoAdapterBackendType BackendType;
    public readonly VideoAdapterPowerPreference PowerPreference;
    public readonly ISurface Surface;

    public RequestAdapterOptions(ISurface surface,
        VideoAdapterPowerPreference powerPreference = VideoAdapterPowerPreference.Any,
        VideoAdapterBackendType backendType = VideoAdapterBackendType.Any)
    {
        Surface = surface;
        PowerPreference = powerPreference;
        BackendType = backendType;
    }
}
