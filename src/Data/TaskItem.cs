namespace Data;

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
    public Dictionary<string, ColumnConfiguration> ColumnConfiguration { get; set; } = [];
}

public class ColumnConfiguration
{
    public string Name { get; set; }
    public bool IsModifiable { get; set; }
    public string SelectedFormat { get; set; }
    public int MyProperty { get; set; }
}
