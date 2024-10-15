using System.CommandLine;
using System.CommandLine.Invocation;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Spectre.Console;
using TaskTitan.Cli.Utils;
using TaskTitan.Data;

namespace TaskTitan.Cli.Commands;

public sealed class AddCommand : Command
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

        var modificationOption = new Option<CommandExpression?>(
            aliases: ["-m", "--modify"],
            parseArgument: ar => ExpressionParser.ParseCommand(string.Join(' ', ar.Tokens)),
            description: "Due date etc")
        {
            AllowMultipleArgumentsPerToken = true,
            Arity = ArgumentArity.OneOrMore
        };
        command.AddOption(modificationOption);
    }

    new public class Handler(IAnsiConsole console, ILogger<AddCommand> logger, LiteDbContext dbContext) : ICommandHandler
    {
        public required string Description { get; set; }
        public CommandExpression? Modify { get; set; }
        public int Invoke(InvocationContext context)
        {
            return InvokeAsync(context).Result;
        }

        public async Task<int> InvokeAsync(InvocationContext context)
        {
            var config = new DefaultConfiguration();
            var tasks = dbContext.db.GetCollection<TaskItem>("tasks", LiteDB.BsonAutoId.Guid);
            var orderedTasks = dbContext.db.GetCollection<TaskItem>("tasks", LiteDB.BsonAutoId.Guid)
                .FindAll()
                .Select((item, index) => item.RowId = index);
            var task = new TaskItem
            {
                Description = Description
            };

            tasks.Insert(task);

            var fetchedTask = tasks.FindById(task.Id);

            console.WriteLine(JsonSerializer.Serialize(fetchedTask));

            console.WriteLine($"Added task {tasks.Count()}");
            return 0;
        }
    }
}
