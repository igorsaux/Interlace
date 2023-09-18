using Interlace.Shared.Application;
using JetBrains.Annotations;

namespace Interlace.Shared.Shell;

[PublicAPI]
public interface IShellManager : IManager
{
    string? Execute(string command);
    
    void RegisterCommand(AnonymousCommand command);
}
