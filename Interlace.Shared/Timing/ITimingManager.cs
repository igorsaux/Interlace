using Interlace.Shared.Application;
using JetBrains.Annotations;

namespace Interlace.Shared.Timing;

[PublicAPI]
public interface ITimingManager : IManager
{
    TimeSpan UpTime { get; }
}
