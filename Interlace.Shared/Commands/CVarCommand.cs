using Interlace.Shared.Configuration;
using Interlace.Shared.IoC;
using Interlace.Shared.Shell;
using JetBrains.Annotations;

namespace Interlace.Shared.Commands;

[UsedImplicitly]
public sealed class CVarCommand : IShellCommand
{
    [Dependency] private readonly IConfigurationManager _cfg = default!;
    
    public string Name => "cvar";

    public string Description => "Returns or sets config variable's value";

    public string Help => $"Usage: {Name} <cvar> [value]";
    
    public string? Execute(IReadOnlyList<string> args)
    {
        switch (args.Count)
        {
            case > 2:
                return Help;
            case 1 when _cfg.TryGetValue(args[0], out var value):
                return value?.ToString() ?? "null";
            case 1:
                return null;
            default:
                _cfg.SetValue(args[0], args[1]);

                return null;
        }
    }
}
