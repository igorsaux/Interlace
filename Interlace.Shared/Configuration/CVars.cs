using Interlace.Shared.Graphics;
using Interlace.Shared.Logging;
using JetBrains.Annotations;

namespace Interlace.Shared.Configuration;

[PublicAPI]
public static class CVars
{
    // GRAPHICS

    public static readonly CVarDeclaration GraphicsVsync =
        CVarDeclaration.Create("graphics.vsync", true, CVarFlag.Archive | CVarFlag.ClientOnly);

    public static readonly CVarDeclaration GraphicsBackend =
        CVarDeclaration.Create("graphics.backend", VideoAdapterBackendType.Any, CVarFlag.Archive | CVarFlag.ClientOnly);

    public static readonly CVarDeclaration GraphicsWindowMode =
        CVarDeclaration.Create("graphics.window_mode", WindowMode.Windowed, CVarFlag.Archive | CVarFlag.ClientOnly);

    // GAME

    public static readonly CVarDeclaration GameTicks =
        CVarDeclaration.Create("game.ticks", 30, CVarFlag.Repliacted | CVarFlag.Archive);

    // LOGGING

    public static readonly CVarDeclaration LoggingLevel =
        CVarDeclaration.Create("logging.level", LogLevel.Trace, CVarFlag.Archive);
}
