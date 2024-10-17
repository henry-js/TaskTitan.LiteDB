using System.CommandLine;
using System.CommandLine.Invocation;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Spectre.Console;
using TaskTitan.Cli.Utils;
using TaskTitan.Data;

namespace TaskTitan.Cli.Commands;

// public sealed class TaskCommand : RootCommand
// {
//     public TaskCommand() : base("The tasktitan cli")
//     {
//         this.AddArgument(GlobalOptions.FilterArgument);
//     }
//     new public class Handler(IAnsiConsole console, ILogger<TaskCommand> logger, LiteDbContext dbContext) : ICommandHandler
//     {
//         public int Invoke(InvocationContext context) => InvokeAsync(context).Result;

//         public Task<int> InvokeAsync(InvocationContext context)
//         {
//             return Task.FromResult(1);
//         }
//     }
// }

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
            Arity = ArgumentArity.OneOrMore,
            CustomParser = ar => string.Join(' ', ar.Tokens)
        };

        command.Add(descriptionArgument);

        var modificationOption = new CliOption<CommandExpression?>("modify", ["-m", "--modify"])
        {
            AllowMultipleArgumentsPerToken = true,
            Arity = ArgumentArity.OneOrMore
        };
        command.Add(modificationOption);
    }

    public class Handler(IAnsiConsole console, ILogger<AddCommand> logger, LiteDbContext dbContext) : AsynchronousCliAction
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
