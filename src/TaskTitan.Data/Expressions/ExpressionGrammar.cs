using Pidgin.Configuration;
using TaskTitan.Data.Parsers;
using static TaskTitan.Data.Enums;

namespace TaskTitan.Data.Expressions;

public static class Constants
{
    public static readonly string[] BuiltInKeys = ["due", "until", "project", "end", "entry", "estimate", "id", "modified", "parent", "priority", "recur", "scheduled", "start", "status", "wait"];

}

public abstract record Key
{
    public Key(string name)
    {
        var values = name.Split('.');
        if (values.Length > 2 || values.Length == 0) throw new Exception();

        Name = values[0];

        if (values.Length == 1) return;


        if (Enum.TryParse(Name, true, out ColModifier modifier))
        {
            Modifier = modifier;
        }
    }

    public string Name { get; }
    public ColModifier? Modifier { get; }
}
// public record BuiltInAttributeKey(string name) : Key(name);
// public record UserDefinedAttributeKey(string name) : Key(name);
// public record AttributePair(Key Key, string Value) : Expr;
public abstract record Expr;
public record BinaryFilter(Expr Left, BinaryOperator Operator, Expr Right) : Expr;
public record Tag(TagOperator Modifier, string Value) : Expr;
public enum TagOperator { Include, Exclude }
public enum BinaryOperator { And, Or }
public record CommandExpression(IEnumerable<Expr> Children, string Input) : Expr;
public record FilterExpression(Expr Expr) : Expr;

public abstract record TaskAttribute : Expr
{
    public TaskAttribute(string field, string value, ColModifier? modifier)
    {
        Field = field;
        StringValue = value;
        Modifier = modifier;
    }

    public string Field { get; }
    public ColModifier? Modifier { get; }
    public string? StringValue { get; }

    public static TaskAttribute Create(string field, string value, DateParser _dateParser)
    {
        var split = field.Split('.');
        field = split[0];
        Configuration.DefaultConfiguration.Columns.TryGetValue(field, out var col);
        if (col is null)
        {
            //TODO: check for UDA
        }

        ColModifier? modifier = null;

        if (split.Length < 2) modifier = null;
        else modifier = col?.AllowedModifiers.FirstOrDefault(m => m.ToString().Equals(split[1]));

        if (col is null) throw new Exception();
        switch (col.ColType)
        {
            case ColType.Date:
                var dateVal = _dateParser.Parse(value);
                return new Attribute<DateTime>(split[0], value, dateVal, modifier);
            case ColType.Text:
                return new Attribute<string>(split[0], value, value, modifier);
            case ColType.Number:
                return new Attribute<double>(split[0], value, Convert.ToDouble(value), modifier);
            default:
                throw new Exception();
        }
    }

}

public record Attribute<T> : TaskAttribute
{
    internal Attribute(string field, string stringValue, T value, ColModifier? modifier = null) : base(field, stringValue, modifier)
    {
        Value = value;

    }
    public T Value { get; }

}
