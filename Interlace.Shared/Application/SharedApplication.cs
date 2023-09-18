using System.Diagnostics;
using Interlace.Shared.Application.Hooks;
using Interlace.Shared.Configuration;
using Interlace.Shared.Console;
using Interlace.Shared.IoC;
using Interlace.Shared.Logging;
using Interlace.Shared.Timing;
using JetBrains.Annotations;

namespace Interlace.Shared.Application;

public abstract class SharedApplication : IApplication
{
    [Dependency] protected readonly IConfigurationManager Cfg = default!;
    [Dependency] protected readonly ILogManager Log = default!;
    [Dependency] protected readonly IConsoleManager Console = default!;
    [Dependency] protected readonly ITimingManager Timing = default!;

    private readonly List<IManager> _managers = new();
    
    private string _configPath = "interlace.cfg";
    private TimeSpan _tickRate = TimeSpan.Zero;
    private int _ticks;
    private TimeSpan _tickUpdateAcc = TimeSpan.Zero;

    public string Title => "Interlace";

    public bool Quit { get; set; }

    public void Run()
    {
        var ioc = IoCManager.Instance;

        PreInitialize();

        foreach (var (_, instance) in ioc.RegisteredTypes) ioc.InjectDependencies(instance);

        ioc.InjectDependencies(this);

        Initialize();

        ParseArgs();
        ExecuteConfig();

        PostInitialize();

        Log.Root.Info("Initialization completed");

        var sw = new Stopwatch();
        sw.Start();

        Cfg.SubscribeOnValueChanged<int>(CVars.GameTicks, RefreshUpdateTimings);

        while (!Quit)
        {
            var deltaTime = sw.Elapsed;
            sw.Restart();

            _tickUpdateAcc += deltaTime;

            while (_tickUpdateAcc > _tickRate)
            {
                TickUpdate(deltaTime);

                _tickUpdateAcc -= _tickRate;
            }

            FrameUpdate(deltaTime);
            Render(deltaTime);
        }

        Shutdown();
    }

    private void RefreshUpdateTimings(int ticks)
    {
        _ticks = ticks;
        _tickRate = TimeSpan.FromSeconds(1) / _ticks;
        _tickUpdateAcc = TimeSpan.Zero;

        Log.Root.Debug("Tick rate: {0} ({1} ms)", _ticks, _tickRate.Milliseconds);
    }

    private void ParseArgs()
    {
        var args = Environment.GetCommandLineArgs()[1..];

        Log.Root.Info("Args: {0}", string.Join(' ', args));

        for (var i = 0; i < args.Length; i++)
        {
            var arg = args[i];

            if (arg == "--config")
            {
                if (args.Length <= i + 1)
                {
                    Log.Root.Error("Missing argument for '--config' option");
                    return;
                }

                _configPath = args[i + 1];
                i += 2;
            }
        }
    }

    private void ExecuteConfig()
    {
        if (!File.Exists(_configPath))
            return;

        Log.Root.Info("Executing config file '{0}'", _configPath);

        var cfg = File.ReadAllText(_configPath);
        var lines = cfg.ReplaceLineEndings("\n").Split('\n');

        foreach (var line in lines)
        {
            var trimmed = line.Trim();

            if (string.IsNullOrEmpty(trimmed))
                continue;

            if (trimmed.StartsWith("#"))
                continue;

            Console.Input(line);
        }
    }

    protected void AddManager<TInterface>(IManager manager) where TInterface : IManager
    {
        IoCManager.Instance.Register<TInterface>(manager);

        _managers.Add(manager);
    }

    protected void AddManager<TInterface, TImplementation>()
        where TInterface : IManager
        where TImplementation : IManager, new()
    {
        var implType = typeof(TImplementation);
        IManager? instance = null;

        foreach (var manager in _managers)
        {
            if (manager.GetType() != implType)
                continue;
            
            instance = manager;
            break;
        }

        if (instance is null)
        {
            instance = new TImplementation();
            _managers.Add(instance);
        }
        
        IoCManager.Instance.Register<TInterface>(instance);
    }

    [PublicAPI]
    protected abstract void TickUpdate(TimeSpan deltaTime);

    [PublicAPI]
    protected abstract void FrameUpdate(TimeSpan deltaTime);

    [PublicAPI]
    protected abstract void Render(TimeSpan deltaTime);

    // Life cycle hooks

    private void PreInitialize()
    {
        var managers = new List<IManager>(_managers);

        foreach (var manager in managers)
            if (manager is IPreInitializeHook hook)
                hook.PreInitialize();
    }

    private void Initialize()
    {
        var managers = new List<IManager>(_managers);

        foreach (var manager in managers)
            if (manager is IInitializeHook hook)
                hook.Initialize();
    }

    private void PostInitialize()
    {
        var managers = new List<IManager>(_managers);

        foreach (var manager in managers)
            if (manager is IPostInitializeHook hook)
                hook.PostInitialize();
    }

    private void Shutdown()
    {
        var managers = new List<IManager>(_managers);

        managers.Reverse();

        foreach (var manager in managers)
            if (manager is IShutdownHook hook)
                hook.Shutdown();
    }
}
