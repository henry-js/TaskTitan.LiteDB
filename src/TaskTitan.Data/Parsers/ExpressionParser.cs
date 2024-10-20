using System.Net;
using Pidgin;
using Pidgin.Expression;
using TaskTitan.Data.Expressions;
using static Pidgin.Parser;
using static Pidgin.Parser<char>;
using static Pidgin.Parser<string>;

namespace TaskTitan.Data.Parsers;

public static class ExpressionParser
{
    private static DateParser _dateParser = new DateParser(TimeProvider.System);
    private static readonly Parser<char, char> _colon
        = Token(':');
    private static readonly Parser<char, char> _dash
        = Token('-');
    private static readonly Parser<char, char> _lParen
        = Try(Char('('));
    private static readonly Parser<char, char> _rParen
        = Try(SkipWhitespaces.Then(Char(')')));
    private static readonly Parser<char, string> _string
        = Token(c => c != '\'')
            .ManyString()
            .Between(Char('\''));
    private static Parser<char, Func<Expr, Expr, Expr>> Binary(Parser<char, BinaryOperator> op)
        => op.Select<Func<Expr, Expr, Expr>>(type => (l, r) => new BinaryFilter(l, type, r));
    private static readonly Parser<char, Func<Expr, Expr, Expr>> _and
        = Binary(
            Try(OneOf(
                Try(String("and").Between(SkipWhitespaces)),
                WhitespaceString
            )).ThenReturn(BinaryOperator.And)
        );
    private static readonly Parser<char, Func<Expr, Expr, Expr>> _or
        = Binary(
            Try(String("or").Between(SkipWhitespaces)).ThenReturn(BinaryOperator.Or)
        );
    private static readonly Parser<char, TagOperator> _tagOperator
        = OneOf(
            Char('+').ThenReturn(TagOperator.Include),
            Char('-').ThenReturn(TagOperator.Exclude)
        );
    private static readonly Parser<char, string> _attributeValue
        = OneOf(
            LetterOrDigit,
            _dash,
            _colon
        ).ManyString();

    // // FIX: might be broken, make sure working with key modifiers
    // private static readonly Parser<char, Key> _builtInAttribute
    //     = OneOf(
    //     Constants.BuiltInKeys.Select(k => String(k))
    //     )
    //     .Select(a => new BuiltInAttributeKey(a))
    //     .Cast<Key>();
    // private static readonly Parser<char, Key> _udaAttribute
    //     = Letter
    //         .AtLeastOnceString()
    //         .Select(s => new UserDefinedAttributeKey(s))
    //         .Cast<Key>();
    // internal static readonly Parser<char, Key> _attributePairKey
    //     = OneOf(
    //         Try(_builtInAttribute),
    //         _udaAttribute
    //     );
    // internal static readonly Parser<char, string> _attributePairValue = _string.Or(_attributeValue);
    // internal static readonly Parser<char, Expr> _attributePair
    //     = Map(
    //         (key, value) => new AttributePair(key, value),
    //         _attributePairKey,
    //         _colon.Then(_attributePairValue)
    //     ).TraceResult().Cast<Expr>();

    internal static readonly Parser<char, Expr> _attribute
        = Map(
            (field, _, value) => TaskAttribute.Create(field, value, _dateParser),
            LetterOrDigit.Or(Token('.')).AtLeastOnceString(),
            Token(':'),
            LetterOrDigit.Or(Token('-')).Or(Token(':')).ManyString()
        ).Cast<Expr>();
    internal static readonly Parser<char, Expr> _tagExpression
        = Map(
            (modifier, value) => new Tag(modifier, value),
            _tagOperator,
            LetterOrDigit.AtLeastOnceString()
        ).Cast<Expr>();

    private static readonly Parser<char, Expr> _filtExpr = Pidgin.Expression.ExpressionParser.Build(
        expr =>
            OneOf(
                expr.Between(_lParen, _rParen),
                _attribute,
                _tagExpression
            ),
            [
                Operator.InfixL(_or),
                Operator.InfixL(_and),
            ]
        );

    public static void SetTimeProvider(TimeProvider timeProvider) =>
        _dateParser = (timeProvider is null)
            ? new DateParser(TimeProvider.System)
            : new DateParser(timeProvider);

    public static FilterExpression ParseFilter(string input)
        => _filtExpr
            .Select(expr => new FilterExpression(expr))
            .ParseOrThrow(input);
    public static CommandExpression ParseCommand(string input)
        => OneOf(
            _attribute,
            _tagExpression
        ).SeparatedAtLeastOnce(Token(' '))
        .Select(exprs => new CommandExpression(exprs, input))
        .ParseOrThrow(input);

    internal static (int, char) ParseDateQuantity(string input)
        => Map(
            (a, b) => (Convert.ToInt32(a), b),
            Digit.AtLeastOnceString(),
            Token('w').Or(Token('d'))
            .Or(Token('m'))
        ).ParseOrThrow(input);
}
