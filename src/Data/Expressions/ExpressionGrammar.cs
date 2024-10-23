using TaskTitan.Data.Reports;
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
public record ReportExpression(CustomReport Report) : Expr;
