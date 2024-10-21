using Microsoft.Extensions.Logging;
using Spectre.Console;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Windows.Input;
using TaskTitan.Data;

namespace TaskTitan.Cli.Commands;

public sealed class ListCommand : Command
{
    public ListCommand() : base("list", "Add a task to the list")
    {
        AddOptions(this);
    }

    public static void AddOptions(Command command)
    {
    }

    new public class Handler(IAnsiConsole console, LiteDbContext context, ILogger<ListCommand> logger) : ICommandHandler
    {
        public int Invoke(InvocationContext context) => InvokeAsync(context).Result;

        public async Task<int> InvokeAsync(InvocationContext context)
        {
            // var tasks = await context.GetAllTasks();
            logger.LogInformation("Information logged");
            console.WriteLine("Hello from list command");

            return await Task.FromResult(1);
        }
    }

    // public class Handler(IAnsiConsole console, LiteDbContext context) : AsynchronousCliAction
    // {
    //     public override async Task<int> InvokeAsync(ParseResult parseResult, CancellationToken cancellationToken = default)
    //     {
    //         // var tasks = await context.GetAllTasks();
    //         console.WriteLine("Hello from list command");

    //         return await Task.FromResult(1);
    //     }
    // }
}
