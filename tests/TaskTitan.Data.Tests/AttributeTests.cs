using Microsoft.Extensions.Time.Testing;
using TaskTitan.Data.Expressions;
using TaskTitan.Data.Parsers;

namespace TaskTitan.Data.Tests;

public class AttributeTests
{
    private readonly FakeTimeProvider _timeProvider;
    private readonly DateParser _dateParser;

    public AttributeTests()
    {
        _timeProvider = new FakeTimeProvider(new DateTime(2024, 06, 06));
        _dateParser = new DateParser(_timeProvider);
    }
    [Test]
    public async Task AnAttributeCanBeCreatedWithRelativeDateTimeValue()
    {
        string text = "due:tomorrow";

        var pair = text.Split(':');
        var attribute = TaskAttribute.Create(pair[0], pair[1], _dateParser);

        var actual = attribute as Attribute<DateTime>;
        await Assert.That(actual).IsNotNull();
        await Assert.That(actual?.Value).IsNotNull()
        .And
        .IsTypeOf<DateTime>();
        await Assert.That(actual.Value).IsEqualTo(new DateTime(2024, 06, 07, 0, 0, 0));
    }
    [Test]
    public async Task AnAttributeCanBeCreatedWithActualDateTimeValue()
    {
        string text = "due:2024-12-12";

        var pair = text.Split(':');
        var attribute = TaskAttribute.Create(pair[0], pair[1], _dateParser);

        var actual = attribute as Attribute<DateTime>;
        await Assert.That(actual).IsNotNull();
        await Assert.That(actual?.Value).IsNotNull()
        .And
        .IsTypeOf<DateTime>();
    }
    [Test]
    public async Task AnAttributeCanBeCreatedWithModifierAndRelativeDateTimeValue()
    {
        string text = "due.after:tomorrow";

        var pair = text.Split(':');
        var attribute = TaskAttribute.Create(pair[0], pair[1], _dateParser);

        var actual = attribute as Attribute<DateTime>;
        await Assert.That(actual).IsNotNull();
        await Assert.That(actual?.Value).IsNotNull()
        .And
        .IsTypeOf<DateTime>();
    }
    [Test]
    public async Task AnAttributeCanBeCreatedWithStringValue()
    {
        string text = "project:home";

        var pair = text.Split(':');
        var attribute = TaskAttribute.Create(pair[0], pair[1], _dateParser);

        var actual = attribute as Attribute<string>;
        await Assert.That(actual).IsNotNull();
        await Assert.That(actual?.Value).IsNotNull()
        .And
        .IsTypeOf<string>();
    }
}
