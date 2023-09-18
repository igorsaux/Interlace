using JetBrains.Annotations;

namespace Interlace.Shared.Logging;

public interface ILogHandler
{
    void Log(string name, LogLevel level, [StructuredMessageTemplate] string format, params object?[]? args);
}
