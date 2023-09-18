using JetBrains.Annotations;

namespace Interlace.Shared.IoC;

[PublicAPI]
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public sealed class DependencyAttribute : Attribute
{
}
