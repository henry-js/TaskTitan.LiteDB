using System.Text.Json.Serialization;
using static TaskTitan.Data.Enums;

namespace TaskTitan.Data;

public class TaskItem
{
    public int Id { get; set; }
    public string Description { get; set; }
    public string Depends { get; set; }
    public string Estimate { get; set; }
    public DateTime? Due { get; set; }
    public DateTime? End { get; set; }
    public DateTime? Entry { get; set; }
    public DateTime? Modified { get; set; }
    public Guid? Parent { get; set; }
    public string Project { get; set; }
    public Recurrence Recur { get; set; }
    public DateTime? Scheduled { get; set; }
    public DateTime? Start { get; set; }
    public string Status { get; set; }
    public string[] Tags { get; set; } = [];
    public DateTime? Until { get; set; }
    public double Urgency { get; set; }
    public DateTime? Wait { get; set; }
    public Guid Uuid { get; set; }

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
