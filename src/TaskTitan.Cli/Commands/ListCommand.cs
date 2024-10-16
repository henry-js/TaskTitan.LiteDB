using Microsoft.Extensions.Logging;
using Spectre.Console;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace MyProjectNamespace;

internal sealed class ListCommand : Command
{
    public ListCommand() : base("list", "Add a task to the list")
    {
        AddOptions(this);
    }

    public static void AddOptions(Command command)
    {
    }

    new public class Handler(IAnsiConsole console, ILogger<ListCommand> logger) : ICommandHandler
    {
        public int Invoke(InvocationContext context)
        {
            return InvokeAsync(context).Result;
        }

        public async Task<int> InvokeAsync(InvocationContext context)
        {
            return 0;
        }
    }
}
