using System.CommandLine;
using TaskTitan.Cli.Utils;

public static class GlobalOptions
{
    public static readonly CliOption<CommandExpression?> ModificationOption = new("--modify", ["-m"])
    {
        Description = "Due date etc",
        CustomParser = ar => ExpressionParser.ParseCommand(string.Join(' ', ar.Tokens)),
        AllowMultipleArgumentsPerToken = true,
        Arity = ArgumentArity.OneOrMore
    };

    public static readonly CliOption<FilterExpression> FilterOption = new("--filter", ["-f"])
    {
        Description = "Filter tasks by",
        CustomParser = ar => ExpressionParser.ParseFilter(string.Join(' ', ar.Tokens)),
        AllowMultipleArgumentsPerToken = true,
        Arity = ArgumentArity.ZeroOrMore
    };
}
