using Interlace.Shared.Application.Hooks;
using Interlace.Shared.Configuration;
using Interlace.Shared.IoC;
using JetBrains.Annotations;

namespace Interlace.Shared.Logging;

public abstract class SharedLogManager : ILogManager, IInitializeHook, IPostInitializeHook, IShutdownHook
{
    [Dependency] private readonly IConfigurationManager _cfg = default!;

    private readonly List<ILogHandler> _handlers = new();
    private readonly ReaderWriterLockSlim _lock = new();
    private readonly Dictionary<string, ISawmill> _sawmills = new();

    public virtual void Initialize()
    {
        Root = GetSawmill("root");

        AddHandler(new ConsoleLogHandler());
    }

    public ISawmill Root { get; private set; } = default!;
    public LogLevel Level { get; set; }

    public ISawmill GetSawmill(string name)
    {
        ISawmill? sawmill;

        _lock.EnterReadLock();

        try
        {
            _sawmills.TryGetValue(name, out sawmill);
        }
        finally
        {
            _lock.ExitReadLock();
        }

        if (sawmill is not null)
            return sawmill;

        _lock.EnterWriteLock();

        try
        {
            sawmill = new Sawmill(this, name);

            _sawmills.Add(name, sawmill);
        }
        finally
        {
            _lock.ExitWriteLock();
        }

        return sawmill;
    }

    public void AddHandler(ILogHandler handler)
    {
        _lock.EnterWriteLock();

        try
        {
            _handlers.Add(handler);
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    public void PostInitialize()
    {
        _cfg.SubscribeOnValueChanged<LogLevel>(CVars.LoggingLevel, newValue => Level = newValue);
    }

    public virtual void Shutdown()
    {
        Root.Info("Goodbye");
    }

    private void Log([StructuredMessageTemplate] string name, LogLevel level, string format, params object?[]? args)
    {
        if (Level > level)
            return;

        _lock.EnterReadLock();

        try
        {
            foreach (var handler in _handlers) handler.Log(name, level, format, args);
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    internal sealed class Sawmill : ISawmill
    {
        private readonly SharedLogManager _log;

        public Sawmill(SharedLogManager log, string name)
        {
            _log = log;
            Name = name;
        }

        public string Name { get; }

        public LogLevel Level { get; set; }

        public void Log(LogLevel level, [StructuredMessageTemplate] string format, params object?[]? args)
        {
            LogInner(level, format, args);
        }

        public void Trace([StructuredMessageTemplate] string format, params object?[]? args)
        {
            LogInner(LogLevel.Trace, format, args);
        }

        public void Debug([StructuredMessageTemplate] string format, params object?[]? args)
        {
            LogInner(LogLevel.Debug, format, args);
        }

        public void Info([StructuredMessageTemplate] string format, params object?[]? args)
        {
            LogInner(LogLevel.Info, format, args);
        }

        public void Warning([StructuredMessageTemplate] string format, params object?[]? args)
        {
            LogInner(LogLevel.Warning, format, args);
        }

        public void Error([StructuredMessageTemplate] string format, params object?[]? args)
        {
            LogInner(LogLevel.Error, format, args);
        }

        public void Fatal([StructuredMessageTemplate] string format, params object?[]? args)
        {
            LogInner(LogLevel.Fatal, format, args);
        }

        private void LogInner(LogLevel level, [StructuredMessageTemplate] string format, params object?[]? args)
        {
            if (Level > level)
                return;

            _log.Log(Name, level, format, args);
        }
    }
}
