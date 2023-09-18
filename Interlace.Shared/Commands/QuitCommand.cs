using Interlace.Shared.Application;
using Interlace.Shared.IoC;
using Interlace.Shared.Shell;
using JetBrains.Annotations;

namespace Interlace.Shared.Commands;

[UsedImplicitly]
public sealed class QuitCommand : IShellCommand
{
    [Dependency] private readonly IApplication _application = default!;
    
    public string Name => "quit";

    public string Description => "Quits the game";
    
    public string Help => string.Empty;

    public string? Execute(IReadOnlyList<string> args)
    {
        _application.Quit = true;

        return null;
    }
}
