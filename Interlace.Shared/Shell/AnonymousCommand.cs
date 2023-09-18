using JetBrains.Annotations;

namespace Interlace.Shared.Shell;

[PublicAPI]
public sealed class AnonymousCommand : IShellCommand
{
    public readonly Func<IReadOnlyList<string>, string?> ExecuteCallbackCallback;

    public AnonymousCommand(string name, string description, string help, Func<IReadOnlyList<string>, string?> executeCallback)
    {
        Name = name;
        Description = description;
        Help = help;
        ExecuteCallbackCallback = executeCallback;
    }

    public string Name { get; }

    public string Description { get; }
    
    public string Help { get; }

    public string? Execute(IReadOnlyList<string> args)
    {
        return ExecuteCallbackCallback.Invoke(args);
    }
}
