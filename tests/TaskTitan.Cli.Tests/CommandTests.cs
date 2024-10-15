using System.CommandLine;
using System.CommandLine.Parsing;
using TaskTitan.Cli.Commands;
using TaskTitan.Cli.Utils;

namespace TaskTitan.Cli.Tests;

public class CommandTests
{
    [Test]
    public async Task AddCommandCanParseDescriptionArgument()
    {
        var command = new AddCommand();
        string description = "i am a description argument";
        var result = command.Parse(description);
        var argRes = result.CommandResult.Children[0] as ArgumentResult;
        var value = result.GetValueForArgument(argRes.Argument) as string;
        await Assert.That(value).IsEqualTo(description);
    }

    [Test]
    [Arguments(new string[] { "-m", "due:tomorrow" }, "due:tomorrow")]
    [Arguments(new string[] { "-m", "due:tomorrow" }, "due:tomorrow")]
    [Arguments(new string[] { "-m", "due:tomorrow", "-m", "project:work" }, "due:tomorrow project:work")]
    public async Task AddCommandCanParseModifyOption(string[] modifyText, string expected)
    {
        var command = new AddCommand();
        string[] input = ["description placeholder", .. modifyText];
        var result = command.Parse(input);

        var optRes = result.CommandResult.Children[^1] as OptionResult;

        var value = result.GetValueForOption(optRes.Option) as CommandExpression;

        await Assert.That(value.Input).IsEqualTo(expected);
    }
}
