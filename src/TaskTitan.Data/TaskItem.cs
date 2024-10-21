namespace TaskTitan.Data;

public class TaskItem
{
    public TaskItem(string description)
    {
        Description = description;
    }

    public int Id { get; set; }
    public string Description { get; set; }
    public int[] Depends { get; set; } = [];
    public DateTime? Due { get; set; }
    public DateTime? End { get; set; }
    public DateTime? Entry { get; set; } = DateTime.UtcNow;
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
    public Guid Uuid { get; set; } = Guid.NewGuid();

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
    public Dictionary<string, AttributeColumnConfig> ColumnConfiguration { get; set; } = [];
}
