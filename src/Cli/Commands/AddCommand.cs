using System.CommandLine;
using System.CommandLine.Invocation;
using Microsoft.Extensions.Logging;
using Spectre.Console;

namespace Cli.Commands;

internal sealed class AddCommand : Command
{
    public AddCommand() : base("add", "Add a task to the list")
    {
        AddOptions(this);
    }

    public static void AddOptions(Command command)
    {
        var descriptionArgument = new Argument<string>("description"
        , parse: ar => string.Join(' ', ar.Tokens)
        )
        {
            Arity = ArgumentArity.OneOrMore,
        };
        command.AddArgument(descriptionArgument);

        var modificationOption = new Option<string?>(
            aliases: ["-m", "--modify"],
            parseArgument: ar => string.Join(' ', ar.Tokens),
            description: "Due date etc")
        { AllowMultipleArgumentsPerToken = true };
        command.AddOption(modificationOption);
    }

    new public class Handler(IAnsiConsole console, ILogger<AddCommand> logger) : ICommandHandler
    {
        public required string Description { get; set; }
        public string? Modify { get; set; }
        public int Invoke(InvocationContext context)
        {
            return InvokeAsync(context).Result;
        }

        public async Task<int> InvokeAsync(InvocationContext context)
        {

            console.WriteLine($"Created task.");
            return 0;
        }
    }
}
