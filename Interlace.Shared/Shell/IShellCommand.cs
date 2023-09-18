using JetBrains.Annotations;

namespace Interlace.Shared.Shell;

[PublicAPI]
public interface IShellCommand
{
    string Name { get; }
    
    string Description { get; }
    
    string Help { get; }

    string? Execute(IReadOnlyList<string> args);
}
