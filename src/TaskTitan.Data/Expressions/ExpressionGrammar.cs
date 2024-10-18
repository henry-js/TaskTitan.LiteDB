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
public record BuiltInAttributeKey(string name) : Key(name);
public record UserDefinedAttributeKey(string name) : Key(name);
public record AttributePair(Key Key, string Value) : Expr;
public abstract record Expr;
public record BinaryFilter(Expr Left, BinaryOperator Operator, Expr Right) : Expr;
public record Tag(TagOperator Modifier, string Value) : Expr;
public enum TagOperator { Include, Exclude }
public enum BinaryOperator { And, Or }
public record CommandExpression(IEnumerable<Expr> Children, string Input) : Expr;
public record FilterExpression(Expr Expr) : Expr;

public record Attribute : Expr
{
    private Attribute(string field, ColModifier? modifier, ColType? colType, string value)
    {
        Field = field;
        Modifier = modifier;
        ColType = colType;
        Value = value;
    }
    public static Attribute Create(string field, string value)
    {
        var config = new DefaultConfiguration();

        var split = field.Split('.');
        field = split[0];
        config.Columns.TryGetValue(field, out var col);
        ColModifier? modifier = null;
        if (split.Length < 2) modifier = null;
        modifier = col?.AllowedModifiers.FirstOrDefault(m => m.ToString().Equals(split[1]));

        return new Attribute(field, modifier, col?.ColType, value);
    }

    public string Field { get; }
    public ColModifier? Modifier { get; }
    public ColType? ColType { get; }
    public string Value { get; }
}
