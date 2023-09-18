using JetBrains.Annotations;

namespace Interlace.Shared.Configuration;

[PublicAPI]
[Flags]
public enum CVarFlag
{
    None = 0,
    Archive = 1 << 0,
    ClientOnly = 1 << 1,
    ServerOnly = 1 << 2,
    Repliacted = 1 << 3,
    Confidential = 1 << 4
}
