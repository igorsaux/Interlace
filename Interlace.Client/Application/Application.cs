using Interlace.Client.Configuration;
using Interlace.Client.Ecs;
using Interlace.Client.Graphics;
using Interlace.Client.Logging;
using Interlace.Client.Windowing;
using Interlace.Client.Windowing.SDL2;
using Interlace.Shared.Application;
using Interlace.Shared.Application.Hooks;
using Interlace.Shared.Configuration;
using Interlace.Shared.Console;
using Interlace.Shared.Crypto;
using Interlace.Shared.Crypto.Sodium;
using Interlace.Shared.Ecs;
using Interlace.Shared.IoC;
using Interlace.Shared.Localization;
using Interlace.Shared.Logging;
using Interlace.Shared.Reflection;
using Interlace.Shared.Resources;
using Interlace.Shared.Serialization;
using Interlace.Shared.Shell;
using Interlace.Shared.Timing;

namespace Interlace.Client.Application;

public sealed class Application : SharedApplication, IPostInitializeHook
{
    [Dependency] private readonly IGraphicsManager _graphics = default!;
    [Dependency] private readonly IWindowingManager _windowing = default!;

    public Application()
    {
        AddManager<IApplication>(this);
        AddManager<IConsoleManager, ConsoleManager>();
        AddManager<IReflectionManager, ReflectionManager>();
        AddManager<ICryptoManager, SodiumCryptoManager>();
        AddManager<IResourcesManager, DynamicResourcesManager>();
        AddManager<ILogManager, LogManager>();
        AddManager<ISerializationManager, SerializationManager>();
        AddManager<IConfigurationManager, ConfigurationManager>();
        AddManager<ILocalizationManager, LocalizationManager>();
        AddManager<ITimingManager, TimingManager>();
        AddManager<IWindowingManager, Sdl2WindowingManager>();
        AddManager<IGraphicsManager, GraphicsManager>();
        AddManager<IEntityManager, EntityManager>();
        AddManager<IShellManager, ShellManager>();
    }

    public void PostInitialize()
    {
        _windowing.Quit += () => Quit = true;
    }

    protected override void TickUpdate(TimeSpan deltaTime)
    {
    }

    protected override void FrameUpdate(TimeSpan deltaTime)
    {
        _windowing.FrameUpdate();
    }

    protected override void Render(TimeSpan deltaTime)
    {
        if (_windowing.MainWindow is null)
            return;

        _graphics.Render();
    }
}
