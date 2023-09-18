using System.Text;
using Interlace.Shared.Application.Hooks;
using Interlace.Shared.IoC;
using Interlace.Shared.Shell;

namespace Interlace.Shared.Console;

public sealed class ConsoleManager : IConsoleManager, IInitializeHook
{
    [Dependency] private readonly IShellManager _shell = default!;
    
    public void Initialize()
    {
        System.Console.OutputEncoding = System.Console.InputEncoding = Encoding.UTF8;
    }

    public void Write(string text)
    {
        System.Console.Write(text);
    }

    public void WriteLine(string text)
    {
        System.Console.WriteLine(text);
    }

    public string? Input(string text)
    {
        WriteLine($"$ {text}");

        var result = _shell.Execute(text);

        if (!string.IsNullOrEmpty(result))
            WriteLine(result);

        return result;
    }
}
