using System.CommandLine;
using TaskTitan.Cli.Utils;

public static class GlobalOptions
{
    public static readonly Option<CommandExpression?> ModificationOption = new(
        aliases: ["-m", "--modify"],
        parseArgument: ar => ExpressionParser.ParseCommand(string.Join(' ', ar.Tokens)),
        description: "Due date etc")
    {
        AllowMultipleArgumentsPerToken = true,
        Arity = ArgumentArity.OneOrMore
    };

    public static readonly Option<FilterExpression> FilterOption = new(
        aliases: ["-f", "--filter"],
        parseArgument: ar => ExpressionParser.ParseFilter(string.Join(' ', ar.Tokens)),
        description: "Filter tasks by")
    {
        AllowMultipleArgumentsPerToken = true,
        Arity = ArgumentArity.ZeroOrMore
    };
}
