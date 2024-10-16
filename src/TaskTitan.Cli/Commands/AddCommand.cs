using System.CommandLine;
using System.CommandLine.Invocation;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using TaskTitan.Data;
using TaskTitan.Data.Expressions;
using TaskTitan.Data.Parsers;

namespace TaskTitan.Cli.Commands;

public sealed class AddCommand : CliCommand
{
    public AddCommand() : base("add", "Add a task to the list")
    {
        AddOptions(this);
    }

    public static void AddOptions(CliCommand command)
    {
        var descriptionArgument = new CliArgument<string>("description")
        {
            CustomParser = ar => string.Join(' ', ar.Tokens),
            Arity = ArgumentArity.OneOrMore,
        };

        command.Add(descriptionArgument);

        var modificationOption = new CliOption<CommandExpression?>("modify", ["-m", "--modify"])
        {
            CustomParser = ar => ExpressionParser.ParseCommand(string.Join(' ', ar.Tokens)),
            AllowMultipleArgumentsPerToken = true,
            Arity = ArgumentArity.OneOrMore
        };
        command.Add(modificationOption);
    }

    new public class Handler(IAnsiConsole console, ILogger<AddCommand> logger, LiteDbContext dbContext) : AsynchronousCliAction
    {
        public required string Description { get; set; }
        public CommandExpression? Modify { get; set; }

        public override async Task<int> InvokeAsync(ParseResult parseResult, CancellationToken cancellationToken = default)
        {
            var config = new DefaultConfiguration();
            var tasks = dbContext.db.GetCollection<TaskItem>("tasks", LiteDB.BsonAutoId.Guid);
            var orderedTasks = dbContext.db.GetCollection<TaskItem>("tasks", LiteDB.BsonAutoId.Guid)
                .FindAll()
                .Select((item, index) => item.RowId = index);
            var task = new TaskItem(Description);

            tasks.Insert(task);

            var fetchedTask = tasks.FindById(task.Id);

            console.WriteLine(JsonSerializer.Serialize(fetchedTask));

            console.WriteLine($"Added task {tasks.Count()}");
            return 0;
        }
    }
}
