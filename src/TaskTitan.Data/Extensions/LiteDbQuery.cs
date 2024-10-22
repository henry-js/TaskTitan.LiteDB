using System.Runtime.CompilerServices;
using LiteDB;
using TaskTitan.Data.Expressions;
using static TaskTitan.Data.Enums;

namespace TaskTitan.Data.Extensions;

public static class FilterToBson
{
    public static BsonExpression ToBsonExpression(this FilterExpression filter)
    {
        return ExprToBsonExpression(filter.Expr);
    }

    private static BsonExpression ExprToBsonExpression(Expr expr, int depth = 0)
    {
        return expr switch
        {
            BinaryFilter bf => BinaryFilterToBsonExpression(bf, depth),
            TaskProperty attr => AttributeToBsonExpression(attr),
        };
    }

    private static BsonExpression AttributeToBsonExpression(TaskProperty attr)
    {
        if (attr is TaskAttribute<DateTime> t)
        {
            return ParseDateTimeAttribute(t);
        }
        else if (attr is TaskAttribute<double> d)
        {
            return ParseNumberAttribute(d);
        }
        else
        {
            return ParseTextAttribute(attr as TaskAttribute<string>);
        }

        BsonExpression ParseDateTimeAttribute(TaskAttribute<DateTime> attribute)
        {
            return attribute.Modifier switch
            {
                ColModifier.Equals or ColModifier.Is or null => Query.EQ(attribute.PropertyName, attribute.Value),
                ColModifier.Before => Query.LT(attribute.PropertyName, attribute.Value),
                ColModifier.After => Query.GTE(attribute.PropertyName, attribute.Value),
                ColModifier.Not => Query.Not(attribute.PropertyName, attribute.Value),
                _ => throw new SwitchExpressionException($"Modifier {attribute.Modifier} is not supported for Date attributes"),
            };
        }
        BsonExpression ParseTextAttribute(TaskAttribute<string> attribute)
        {
            return attribute.Modifier switch
            {
                ColModifier.Equals or ColModifier.Is or null => Query.EQ(attribute.PropertyName, attribute.Value),
                ColModifier.Isnt => Query.Not(attribute.PropertyName, attribute.Value),
                ColModifier.Has or ColModifier.Contains => Query.Contains(attribute.PropertyName, attribute.Value),
                ColModifier.Hasnt => Query.Not(attribute.PropertyName, attribute.Value),
                ColModifier.Startswith => Query.StartsWith(attribute.PropertyName, attribute.Value),
                ColModifier.Endswith => BsonExpression.Create($"{attribute.PropertyName} LIKE {new BsonValue("%" + attribute.Value)}"),
                _ => throw new SwitchExpressionException($"Modifier {attribute.Modifier} is not supported for Text attributes"),
            };
        }
        BsonExpression ParseNumberAttribute(TaskAttribute<double> attribute)
        {
            return attribute.Modifier switch
            {
                ColModifier.Equals or ColModifier.Is or null => Query.EQ(attribute.PropertyName, attribute.Value),
                ColModifier.Below => Query.LT(attribute.PropertyName, attribute.Value),
                ColModifier.Above => Query.GTE(attribute.PropertyName, attribute.Value),
                ColModifier.Not or ColModifier.Isnt => Query.Not(attribute.PropertyName, attribute.Value),
                _ => throw new SwitchExpressionException($"Modifier {attribute.Modifier} is not supported for Text attributes"),
            };
        }
    }

    private static BsonExpression BinaryFilterToBsonExpression(BinaryFilter bf, int depth)
    {
        depth++;
        var left = ExprToBsonExpression(bf.Left, depth);

        var right = ExprToBsonExpression(bf.Right, depth);

        var expression = bf.Operator switch
        {
            BinaryOperator.And => Query.And(left, right),
            BinaryOperator.Or => Query.Or(left, right),
            _ => throw new Exception()
        };
        depth--;
        // return depth == 0 ? $"{left} {Operator} {right}" : $"({left} {Operator} {right})";
        return expression;
    }
}
