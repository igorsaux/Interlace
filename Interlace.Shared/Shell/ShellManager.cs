using Interlace.Shared.Application.Hooks;
using Interlace.Shared.Console;
using Interlace.Shared.IoC;
using Interlace.Shared.Logging;
using Interlace.Shared.Reflection;

namespace Interlace.Shared.Shell;

public sealed class ShellManager : IShellManager, IInitializeHook
{
    [Dependency] private readonly ILogManager _log = default!;
    [Dependency] private readonly IReflectionManager _reflection = default!;
    
    private readonly Dictionary<string, IShellCommand> _commands = new();
    private ISawmill _sawmill = default!;

    public void Initialize()
    {
        _sawmill = _log.GetSawmill("shell");

        var commandTypes = _reflection.GetImplementationsOf<IShellCommand>();

        foreach (var commandType in commandTypes)
        {
            if (commandType.IsAbstract)
                continue;

            try
            {
                RegisterCommandInner((IShellCommand)_reflection.CreateInstanceOf(commandType));
            }
            catch (Exception)
            {
                // ignored
            }
        }

        _sawmill.Debug("Registered {0} commands", _commands.Count);
    }

    public string? Execute(string text)
    {
        var args = text.Split(' ');

        if (args.Length == 0)
        {
            _sawmill.Error("Invalid command: '{0}'", text);
            
            return null;
        }

        var commandName = args[0];

        if (!_commands.TryGetValue(commandName, out var command))
        {
            _sawmill.Error("Command '{0}' not found", commandName);
            
            return null;
        }

        _sawmill.Trace("Executing command '{0}'", text);

        string? result;

        try
        {
            result = command.Execute(args[1..]);
        }
        catch (Exception e)
        {
            _sawmill.Error("Error occurred while executing command '{0}':\n{1}", text, e);
            
            return e.ToString();
        }

        return result;
    }

    public void RegisterCommand(AnonymousCommand command)
    {
        RegisterCommandInner(command);
    }

    private void RegisterCommandInner(IShellCommand commandInstance)
    {
        if (_commands.ContainsKey(commandInstance.Name))
        {
            _sawmill.Error("Command with name '{0}' already registered!", commandInstance.Name);

            return;
        }

        _commands.Add(commandInstance.Name, commandInstance);
    }
}
