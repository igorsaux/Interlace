using Interlace.Shared.Application;
using JetBrains.Annotations;

namespace Interlace.Shared.Console;

[PublicAPI]
public interface IConsoleManager : IManager
{
    void Write(string text);

    void WriteLine(string text);

    string? Input(string text);
}
