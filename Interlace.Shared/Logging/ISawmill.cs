using JetBrains.Annotations;

namespace Interlace.Shared.Logging;

[PublicAPI]
public interface ISawmill
{
    string Name { get; }

    LogLevel Level { get; set; }

    void Log(LogLevel level, [StructuredMessageTemplate] string format, params object?[]? args);

    void Trace([StructuredMessageTemplate] string format, params object?[]? args);

    void Debug([StructuredMessageTemplate] string format, params object?[]? args);

    void Info([StructuredMessageTemplate] string format, params object?[]? args);

    void Warning([StructuredMessageTemplate] string format, params object?[]? args);

    void Error([StructuredMessageTemplate] string format, params object?[]? args);

    void Fatal([StructuredMessageTemplate] string format, params object?[]? args);
}
