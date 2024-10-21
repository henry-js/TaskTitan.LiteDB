using TaskTitan.Data.Parsers;
using static TaskTitan.Data.Enums;

namespace TaskTitan.Data.Expressions;

public abstract record TaskAttribute : Expr
{
    public object? Value { get; set; }
    public TaskAttribute(string field, string value, ColModifier? modifier)
    {
        Field = field;
        StringValue = value;
        Modifier = modifier;
    }

    public string Field { get; }
    public string PropertyName => _taskItemProperties.SingleOrDefault(p => p.Equals(Field, StringComparison.OrdinalIgnoreCase)) ?? Field;
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
        // else modifier = col?.AllowedModifiers.FirstOrDefault(m => m.ToString().Equals(split[1], StringComparison.OrdinalIgnoreCase));
        else modifier = Enum.GetValues<ColModifier>().FirstOrDefault(m => m.ToString().Equals(split[1], StringComparison.OrdinalIgnoreCase));

        if (col is null) throw new Exception();
        switch (col.ColType)
        {
            case ColType.Date:
                var dateVal = _dateParser.Parse(value);
                return new TaskAttribute<DateTime>(split[0], value, dateVal, modifier);
            case ColType.Text:
                return new TaskAttribute<string>(split[0], value, value, modifier);
            case ColType.Number:
                return new TaskAttribute<double>(split[0], value, Convert.ToDouble(value), modifier);
            default:
                throw new Exception();
        }
    }

    private static readonly string[] _taskItemProperties = typeof(TaskItem).GetProperties().Select(x => x.Name).ToArray();
}

public record TaskAttribute<T> : TaskAttribute
{
    internal TaskAttribute(string field, string stringValue, T value, ColModifier? modifier = null) : base(field, stringValue, modifier)
    {
        Value = value;
    }
    public new T Value { get; }
}
