using Interlace.Shared.Graphics;
using JetBrains.Annotations;

namespace Interlace.Client.Graphics.Renderer;

[PublicAPI]
public sealed class VideoAdapterProperties
{
    public readonly string Architecture;

    public readonly VideoAdapterBackendType BackendType;

    public readonly int DeviceId;

    public readonly string DriverDescription;
    public readonly string Name;

    public readonly VideoAdapterType Type;

    public readonly int VendorId;

    public readonly string VendorName;

    public VideoAdapterProperties(string name, string driverDescription, string vendorName, string architecture,
        int vendorId, int deviceId, VideoAdapterBackendType backendType, VideoAdapterType type)
    {
        Name = name;
        DriverDescription = driverDescription;
        VendorName = vendorName;
        Architecture = architecture;
        VendorId = vendorId;
        DeviceId = deviceId;
        BackendType = backendType;
        Type = type;
    }
}
