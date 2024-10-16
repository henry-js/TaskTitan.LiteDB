using TUnit.Assertions.Extensions.Generic;
using TaskTitan.Cli.Utils;

namespace TaskTitan.Cli.Tests;

public class PidginParserTests
{
    [Test]
    [Arguments("due:tomorrow", "due", "tomorrow")]
    [Arguments("until:2w", "until", "2w")]
    [Arguments("project:home", "project", "home")]
    [Arguments("project:WORK", "project", "WORK")]
    [Arguments("due:2024-01-02T00:00:00", "due", "2024-01-02T00:00:00")]
    [Arguments("due:'i am a quoted string'", "due", "i am a quoted string")]
    [Arguments("uda:'i am a UDA'", "uda", "i am a UDA")]

    public async Task AnAttributePairCanBeParsedFromText(string text, string key, string value)
    {
        var result = ExpressionParser.ParseFilter(text);

        await Assert.That((result.Expr as AttributePair)?.Key)
        .IsEquivalentTo(new BuiltInAttributeKey(key))
        .Or
        .IsEquivalentTo(new UserDefinedAttributeKey(key));
    }


    [Test]
    [Arguments("due:8w and until:7w", BinaryOperator.And)]
    [Arguments("due:9w until:8w", BinaryOperator.And)]
    [Arguments("due:10w or until:9w", BinaryOperator.Or)]
    [Arguments("project:work or project:notWork", BinaryOperator.Or)]
    public async Task ABinaryExpressionCanBeParsedFromText(string text, BinaryOperator @operator)
    {
        var result = ExpressionParser.ParseFilter(text);

        // await Assert.That(result.Success).IsTrue();
        await Assert.That(result).IsAssignableTo(typeof(BinaryFilter));

        var resultVal = result.Expr as BinaryFilter;
        await Assert.That(resultVal?.Operator).IsEquivalentTo(@operator);
    }

    [Test]
    [Arguments("+test", TagOperator.Include)]
    [Arguments("-test", TagOperator.Exclude)]
    public async Task ATagExpressionCanBeParsedFromText(string tagText, TagOperator modifier)
    {
        var result = ExpressionParser.ParseFilter(tagText);

        await Assert.That(result).IsAssignableTo(typeof(Tag));
        var tag = result.Expr as Tag;
        await Assert.That(tag?.Modifier).IsEqualTo(modifier);
    }

    [Test]
    [Arguments("due:tomorrow", typeof(AttributePair))]
    [Arguments("+test or due:tomorrow", typeof(BinaryFilter))]
    [Arguments("due:tomorrow or project:home", typeof(BinaryFilter))]
    public async Task DifferentExpressionsCanBeParsedFromText(string text, Type t)
    {
        var result = ExpressionParser.ParseFilter(text);

        await Assert.That(result).IsAssignableTo(t);
    }

    [Test]
    [Arguments("due:1w", 1)]
    [Arguments("due:1w until:3d", 2)]
    [Arguments("due:1w until:3d project:work", 3)]
    [Arguments("due:1w until:3d project:work +fun", 4)]
    public async Task CommandExpressionCanBeParsedFromText(string text, int quantity)
    {
        var result = ExpressionParser.ParseCommand(text);

        await Assert.That(result).IsNotNull();

        await Assert.That(result.Children).HasCount().EqualTo(quantity);
    }
}
