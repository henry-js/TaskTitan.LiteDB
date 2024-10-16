using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using static TaskTitan.Data.Enums;

namespace TaskTitan.Data;

public class TaskItem
{
    public TaskItem()
    {

    }
    public TaskItem(string description)
    {
        Description = description;
    }

    public int RowId { get; set; }
    public string Description { get; set; }
    public int[] Depends { get; set; } = [];
    public DateTime? Due { get; set; }
    public DateTime? End { get; set; }
    public DateTime? Entry { get; set; }
    public DateTime? Modified { get; set; }
    public Guid? Parent { get; set; }
    public string? Project { get; set; }
    public Recurrence? Recur { get; set; }
    public DateTime? Scheduled { get; set; }
    public DateTime? Start { get; set; }
    public TaskItemStatus Status { get; set; } = TaskItemStatus.Pending;
    public string[] Tags { get; set; } = [];
    public DateTime? Until { get; set; }
    public double Urgency { get; set; }
    public DateTime? Wait { get; set; }
    public Guid Id { get; set; } = Guid.CreateVersion7(DateTime.UtcNow);

}

public enum TaskItemStatus { Pending, Started, Done }
// public record TaskItemStatus(string Value)
// {
//     public static TaskItemStatus Pending => new("Pending");
//     public static TaskItemStatus Started => new("Started");
//     public static TaskItemStatus Done => new("Done");

//     public override string ToString()
//     {
//         return Value;
//     }
// }

public class Recurrence
{
}

public class UserConfiguration
{
    public Dictionary<string, ColumnConfig> ColumnConfiguration { get; set; } = [];
}
public class ColumnConfig
{
    [JsonIgnore]
    public string Name { get; set; }
    public bool IsModifiable { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ColFormat Format { get; private set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ColType ColType { get; set; }
    public IReadOnlyList<ColFormat>? AllowedFormats { get; }
    public HashSet<ColModifier> AllowedModifiers { get; }

    public ColumnConfig(string name, bool isModifiable, ColFormat format, ColType type, List<ColFormat>? allowedFormats = null)
    {
        Name = name;
        IsModifiable = isModifiable;
        Format = format;
        ColType = type;
        AllowedFormats = allowedFormats ?? ColumnTypeFormats.AllowedFormats[type];
        AllowedModifiers = ColType switch
        {
            ColType.Date => [ColModifier.None, ColModifier.Not, ColModifier.Before, ColModifier.After, ColModifier.Is],
            ColType.Text => [ColModifier.None, ColModifier.Any, ColModifier.Is, ColModifier.Not, ColModifier.Has, ColModifier.Hasnt, ColModifier.Startswith, ColModifier.Endswith],
            ColType.Number => [ColModifier.None, ColModifier.Any, ColModifier.Below, ColModifier.Above, ColModifier.Is, ColModifier.Not]
        };
    }

    public void SetFormat(ColFormat format)
    {
        if (!ColumnTypeFormats.AllowedFormats.TryGetValue(ColType, out List<ColFormat>? allowed))
            throw new ArgumentException($"Unknown column type '{ColType}'");

        if (allowed.Contains(format))
        {
            Format = format;
        }
    }
}
