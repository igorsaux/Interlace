using Interlace.Shared.Application;
using JetBrains.Annotations;

namespace Interlace.Shared.Logging;

[PublicAPI]
public interface ILogManager : IManager
{
    ISawmill Root { get; }

    LogLevel Level { get; set; }

    ISawmill GetSawmill(string name);

    void AddHandler(ILogHandler handler);
}
