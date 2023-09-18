using JetBrains.Annotations;

namespace Interlace.Shared.Application;

[PublicAPI]
public interface IApplication : IManager
{
    string Title { get; }
    
    bool Quit { get; set; }

    void Run();
}
