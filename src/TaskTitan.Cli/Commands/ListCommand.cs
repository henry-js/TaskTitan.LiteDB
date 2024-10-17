using Microsoft.Extensions.Logging;
using Spectre.Console;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace TaskTitan.Cli.Commands;

internal sealed class ListCommand : CliCommand
{
    public ListCommand() : base("list", "Add a task to the list")
    {
        AddOptions(this);
    }

    public static void AddOptions(CliCommand command)
    {
    }

    new public class Handler(IAnsiConsole console, ILogger<ListCommand> logger) : AsynchronousCliAction
    {

        public override async Task<int> InvokeAsync(ParseResult parseResult, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
