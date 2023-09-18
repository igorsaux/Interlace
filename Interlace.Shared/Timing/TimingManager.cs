using System.Diagnostics;
using Interlace.Shared.Application.Hooks;

namespace Interlace.Shared.Timing;

public sealed class TimingManager : ITimingManager, IInitializeHook
{
    private Stopwatch _sw = default!;

    public void Initialize()
    {
        _sw = new Stopwatch();
        _sw.Start();
    }

    public TimeSpan UpTime => _sw.Elapsed;
}
