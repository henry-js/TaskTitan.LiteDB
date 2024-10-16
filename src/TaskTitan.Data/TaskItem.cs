using System.Text.Json.Serialization;
using static TaskTitan.Data.Enums;

namespace TaskTitan.Data;

public class TaskItem
{
    public int RowId { get; set; }
    public string Description { get; set; }
    public string Depends { get; set; }
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

public record TaskItemStatus(string Value)
{
    public static TaskItemStatus Pending => new("Pending");
    public static TaskItemStatus Started => new("Started");
    public static TaskItemStatus Done => new("Done");

    public override string ToString()
    {
        return Value;
    }
}

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

    public ColumnConfig(string name, bool IsModifiable, ColFormat format, ColType type, List<ColFormat>? allowedFormats = null)
    {
        Name = name;
        this.IsModifiable = IsModifiable;
        Format = format;
        ColType = type;
        AllowedFormats = allowedFormats ?? ColumnTypeFormats.AllowedFormats[type];
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
