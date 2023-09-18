using JetBrains.Annotations;

namespace Interlace.Client.Graphics.Renderer;

[PublicAPI]
public enum VideoAdapterType
{
    DiscreteGpu,
    IntegratedGpu,
    Cpu,
    Unknown
}
