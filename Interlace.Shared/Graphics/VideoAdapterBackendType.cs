using JetBrains.Annotations;

namespace Interlace.Shared.Graphics;

[PublicAPI]
public enum VideoAdapterBackendType
{
    Any,
    WebGpu,
    D3D11,
    D3D12,
    Metal,
    Vulkan,
    OpenGl,
    OpenGles
}
