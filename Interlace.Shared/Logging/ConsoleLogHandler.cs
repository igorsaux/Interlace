using Interlace.Shared.Console;
using Interlace.Shared.IoC;
using JetBrains.Annotations;

namespace Interlace.Shared.Logging;

public sealed class ConsoleLogHandler : ILogHandler
{
    [Dependency] private readonly IConsoleManager _console = default!;
    
    private const string AnsiCsi = "\x1B[";
    private const string AnsiFgDefault = AnsiCsi + "39m";
    private const string AnsiFgBlack = AnsiCsi + "30m";
    private const string AnsiFgRed = AnsiCsi + "31m";
    private const string AnsiFgBrightRed = AnsiCsi + "91m";
    private const string AnsiFgGreen = AnsiCsi + "32m";
    private const string AnsiFgBrightGreen = AnsiCsi + "92m";
    private const string AnsiFgYellow = AnsiCsi + "33m";
    private const string AnsiFgBrightYellow = AnsiCsi + "93m";
    private const string AnsiFgBlue = AnsiCsi + "34m";
    private const string AnsiFgBrightBlue = AnsiCsi + "94m";
    private const string AnsiFgMagenta = AnsiCsi + "35m";
    private const string AnsiFgBrightMagenta = AnsiCsi + "95m";
    private const string AnsiFgCyan = AnsiCsi + "36m";
    private const string AnsiFgBrightCyan = AnsiCsi + "96m";
    private const string AnsiFgWhite = AnsiCsi + "37m";
    private const string AnsiFgBrightWhite = AnsiCsi + "97m";

    public ConsoleLogHandler()
    {
        IoCManager.Instance.InjectDependencies(this);
    }
    
    public void Log(string name, LogLevel level, [StructuredMessageTemplate] string format, params object?[]? args)
    {
        string text;

        if (args is not null)
            text = string.Format(format, args);
        else
            text = string.Format(format);

        var levelString = level.ToShortString();
        var colorString = level switch
        {
            LogLevel.Trace => AnsiFgGreen,
            LogLevel.Debug => AnsiFgBlue,
            LogLevel.Info => AnsiFgBrightCyan,
            LogLevel.Warning => AnsiFgBrightYellow,
            LogLevel.Error => AnsiFgBrightRed,
            LogLevel.Fatal => AnsiFgBrightMagenta,
            _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
        };

        _console.WriteLine($"{AnsiFgDefault}{colorString}[{levelString}]{AnsiFgDefault} {name}: {text}");
    }
}
