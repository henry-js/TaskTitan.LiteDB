namespace TaskTitan.Cli.Utils;

public static class Constants
{
    public static readonly string[] BuiltInKeys = ["due", "until", "project", "end", "entry", "estimate", "id", "modified", "parent", "priority", "recur", "scheduled", "start", "status", "wait"];

}

public abstract record Key(string Name);
public record BuiltInAttributeKey(string Name) : Key(Name);
public record UserDefinedAttributeKey(string Name) : Key(Name);
public record AttributePair(Key Key, string Value) : Expr;
public abstract record Expr;
public record BinaryFilter(Expr Left, BinaryOperator Operator, Expr Right) : Expr;
public record Tag(TagOperator Modifier, string Value) : Expr;
public enum TagOperator { Include, Exclude }
public enum BinaryOperator { And, Or }
public record CommandExpression(IEnumerable<Expr> Children, string Input) : Expr;
public record FilterExpression(Expr Expr) : Expr;
