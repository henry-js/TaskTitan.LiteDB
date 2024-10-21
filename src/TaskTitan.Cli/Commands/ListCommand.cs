using Microsoft.Extensions.Logging;
using Spectre.Console;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Windows.Input;
using TaskTitan.Data;
using TaskTitan.Data.Expressions;
using TaskTitan.Data.Parsers;
using TaskTitan.Data.Reports;
using static TaskTitan.Data.Extensions.DynamicLinq;

namespace TaskTitan.Cli.Commands;

public sealed class ListCommand : Command
{
    public ListCommand(ReportDictionary reports) : base("list", "Add a task to the list")
    {
        AddOptions(this, reports);
    }

    public static void AddOptions(Command command, ReportDictionary reports)
    {
        Option<FilterExpression?> option = new(
            aliases: ["-f", "--filter"],
            description: "Filter tasks by",
            parseArgument: ar =>
            {
                return ExpressionParser.ParseFilter(string.Join(' ', ar.Tokens));
            })
        {
            AllowMultipleArgumentsPerToken = true,
            Arity = ArgumentArity.ZeroOrMore,
        };
        command.AddOption(option);

        Argument<CustomReport?> report = new(
            name: "Report",
            description: "Use a report instead of filter",
            parse: ar => reports.TryGetValue(ar.Tokens.FirstOrDefault()!.Value, out var report) ? report : null
        )
        {
            Arity = ArgumentArity.ZeroOrOne
        };
        command.AddArgument(report);
    }

    new public class Handler(IAnsiConsole console, LiteDbContext context, ILogger<ListCommand> logger) : ICommandHandler
    {
        public FilterExpression? Filter { get; set; }
        public CustomReport? Report { get; set; }
        public int Invoke(InvocationContext context) => InvokeAsync(context).Result;

        public async Task<int> InvokeAsync(InvocationContext context)
        {
            string linqFilterText = Report switch
            {
                not null => ExpressionParser.ParseFilter(Report.Filter).ToDynamicLinq(),
                _ => (Filter is not null) ? Filter.ToDynamicLinq() : ""
            };

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
