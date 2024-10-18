using static TaskTitan.Data.Enums;

namespace TaskTitan.Data.Expressions;

public static class Constants
{
    public static readonly string[] BuiltInKeys = ["due", "until", "project", "end", "entry", "estimate", "id", "modified", "parent", "priority", "recur", "scheduled", "start", "status", "wait"];

}

public abstract record Key(string[] Name);
public record BuiltInAttributeKey(string[] Name) : Key(Name);
public record UserDefinedAttributeKey(string[] Name) : Key(Name);
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
