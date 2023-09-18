using JetBrains.Annotations;

namespace Interlace.Shared.Logging;

[PublicAPI]
public enum LogLevel
{
    Trace,
    Debug,
    Info,
    Warning,
    Error,
    Fatal
}

public static class LogLevelExtensions
{
    public static string ToShortString(this LogLevel value)
    {
        return value switch
        {
            LogLevel.Trace => "TRCE",
            LogLevel.Debug => "DEBG",
            LogLevel.Info => "INFO",
            LogLevel.Warning => "WARN",
            LogLevel.Error => "ERRO",
            LogLevel.Fatal => "FATL",
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
    }
}
