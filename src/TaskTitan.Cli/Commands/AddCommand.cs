using System.CommandLine;
using System.CommandLine.Hosting;
using System.CommandLine.Invocation;
using System.Text.Json;
using System.Windows.Input;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using TaskTitan.Configuration;
using TaskTitan.Data;
using TaskTitan.Data.Expressions;
using TaskTitan.Data.Parsers;

namespace TaskTitan.Cli.Commands;

public sealed class AddCommand : Command
{
    public AddCommand() : base("add", "Add a task to the list")
    {
        AddOptions(this);
    }

    public static void AddOptions(Command command)
    {
        var descriptionArgument = new Argument<string>("description",
        parse: ar => string.Join(' ', ar.Tokens)
        )
        {
            Arity = ArgumentArity.OneOrMore,
        };

        command.Add(descriptionArgument);

        var modificationOption = new Option<CommandExpression?>(description: "modify",
         parseArgument: ar => ExpressionParser.ParseCommand(string.Join(' ', ar.Tokens)),
         aliases: ["-m", "--modify"])
        {
            AllowMultipleArgumentsPerToken = true,
            Arity = ArgumentArity.OneOrMore
        };
        command.Add(modificationOption);

        // var descriptionArgument = new CliArgument<string>("description")
        // {
        //     CustomParser = ar => string.Join(' ', ar.Tokens),
        //     Arity = ArgumentArity.OneOrMore,
        // };

        // command.Add(descriptionArgument);

        // var modificationOption = new CliOption<CommandExpression?>(
        //     name: "modify",
        //     aliases: ["-m", "--modify"]
        //  )
        // {
        //     CustomParser = ar => ExpressionParser.ParseCommand(string.Join(' ', ar.Tokens)),
        //     AllowMultipleArgumentsPerToken = true,
        //     Arity = ArgumentArity.OneOrMore
        // };
        // command.Add(modificationOption);
    }

    new public class Handler(IAnsiConsole console, LiteDbContext dbContext) : ICommandHandler
    {
        public required string Description { get; set; }
        public CommandExpression? Modify { get; set; }

        public int Invoke(InvocationContext context) => InvokeAsync(context).Result;
        public async Task<int> InvokeAsync(InvocationContext context)
        {
            var config = new DefaultConfiguration();
            var tasks = dbContext.db.GetCollection<TaskItem>("tasks", LiteDB.BsonAutoId.Guid);
            var orderedTasks = dbContext.db.GetCollection<TaskItem>("tasks", LiteDB.BsonAutoId.Guid)
                .FindAll()
                .Select((item, index) => item.Id = index);
            var task = new TaskItem(Description);

            tasks.Insert(task);

            var fetchedTask = tasks.FindById(task.Uuid);

            console.WriteLine(JsonSerializer.Serialize(fetchedTask));

            console.WriteLine($"Added task {tasks.Count()}");
            return 0;
        }

        // public override async Task<int> InvokeAsync(ParseResult parseResult, CancellationToken cancellationToken = default)
        // {
        //     var config = new DefaultConfiguration();
        //     var tasks = dbContext.db.GetCollection<TaskItem>("tasks", LiteDB.BsonAutoId.Guid);
        //     var orderedTasks = dbContext.db.GetCollection<TaskItem>("tasks", LiteDB.BsonAutoId.Guid)
        //         .FindAll()
        //         .Select((item, index) => item.RowId = index);
        //     var task = new TaskItem(Description);

        //     tasks.Insert(task);

        //     var fetchedTask = tasks.FindById(task.Id);

        //     console.WriteLine(JsonSerializer.Serialize(fetchedTask));

        //     console.WriteLine($"Added task {tasks.Count()}");
        //     return 0;
        // }
    }
}
