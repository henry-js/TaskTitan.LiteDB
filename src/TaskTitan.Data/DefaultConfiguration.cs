using TaskTitan.Data.Reports;
using static TaskTitan.Data.Enums.ColFormat;
using static TaskTitan.Data.Enums.ColType;

namespace TaskTitan.Data;

public class DefaultConfiguration
{
    public ReportDictionary Reports { get; set; } = new()
    {
        ["active"] = new CustomReport()
        {
            Name = "active",
            Columns = ["id", "start", "start.age", "entry.age", "depends.indicator", "priority", "project", "tags", "recur", "wait", "scheduled.remaining", "due", "until", "description"],
            Description = "Active tasks",
            Filter = "status:pending and +ACTIVE",
            Labels = ["ID", "Started", "Active", "Age", "D", "P", "Project", "Tags", "Recur", "W", "Sch", "Due", "Until", "Description"],
        },
        ["all"] = new CustomReport()
        {
            Name = "all",
            Columns = ["id", "status.short", "uuid.short", "start.active", "entry.age", "end.age", "depends.indicator", "priority", "project.parent", "tags.count", "recur.indicator", "wait.remaining", "scheduled.remaining", "due", "until.remaining", "description"],
            Description = "All tasks",
            Labels = ["ID", "St", "UUID", "A", "Age", "Done", "D", "P", "Project", "Tags", "R", "Wait", "Sch", "Due", "Until", "Description"],
        },
        ["blocked"] = new CustomReport()
        {
            Name = "blocked",
            Columns = ["id", "depends", "project", "priority", "due", "start.active", "entry.age", "description"],
            Description = "Blocked tasks",
            Filter = "status:pending -WAITING +BLOCKED",
            Labels = ["ID", "Deps", "Proj", "Pri", "Due", "Active", "Age", "Description"],
        },
        ["blocking"] = new CustomReport()
        {
            Name = "blocking",
            Columns = ["id", "uuid.short", "start.active", "depends", "project", "tags", "recur", "wait", "scheduled.remaining", "due.relative", "until.remaining", "description.count", "urgency"],
            Description = "Blocking tasks",
            Filter = "status:pending -WAITING +BLOCKING",
            Labels = ["ID", "UUID", "A", "Deps", "Project", "Tags", "R", "W", "Sch", "Due", "Until", "Description", "Urg"],
        },
        ["completed"] = new CustomReport()
        {
            Name = "completed",
            Columns = ["id", "uuid.short", "entry", "end", "entry.age", "depends", "priority", "project", "tags", "recur.indicator", "due", "description"],
            Description = "Completed tasks",
            Filter = "status:completed",
            Labels = ["ID", "UUID", "Created", "Completed", "Age", "Deps", "P", "Project", "Tags", "R", "Due", "Description"],
        },
        ["list"] = new CustomReport()
        {
            Name = "list",
            Columns = ["id", "start.age", "entry.age", "depends.indicator", "priority", "project", "tags", "recur.indicator", "scheduled.countdown", "due", "until.remaining", "description.count", "urgency"],
            Description = "Most details of tasks",
            Filter = "status:pending -WAITING",
            Labels = ["ID", "Active", "Age", "D", "P", "Project", "Tags", "R", "Sch", "Due", "Until", "Description", "Urg"],
        },
        ["long"] = new CustomReport()
        {
            Name = "long",
            Columns = ["id", "start.active", "entry", "modified.age", "depends", "priority", "project", "tags", "recur", "wait.remaining", "scheduled", "due", "until", "description"],
            Description = "All details of tasks",
            Filter = "status:pending -WAITING",
            Labels = ["ID", "A", "Created", "Mod", "Deps", "P", "Project", "Tags", "Recur", "Wait", "Sched", "Due", "Until", "Description"],
        },
        ["ls"] = new CustomReport()
        {
            Name = "ls",
            Columns = ["id", "start.active", "depends.indicator", "project", "tags", "recur.indicator", "wait.remaining", "scheduled.countdown", "due.countdown", "until.countdown", "description.count"],
            Description = "Few details of tasks",
            Filter = "status:pending -WAITING",
            Labels = ["ID", "A", "D", "Project", "Tags", "R", "Wait", "S", "Due", "Until", "Description"],
        },
        ["minimal"] = new CustomReport()
        {
            Name = "minimal",
            Columns = ["id", "project", "tags.count", "description.count"],
            Description = "Minimal details of tasks",
            Filter = "status:pending",
            Labels = ["ID", "Project", "Tags", "Description"],
        },
        ["newest"] = new CustomReport()
        {
            Name = "newest",
            Columns = ["id", "start.age", "entry", "entry.age", "modified.age", "depends.indicator", "priority", "project", "tags", "recur.indicator", "wait.remaining", "scheduled.countdown", "due", "until.age", "description"],
            Description = "Newest tasks",
            Filter = "status:pending",
            Labels = ["ID", "Active", "Created", "Age", "Mod", "D", "P", "Project", "Tags", "R", "Wait", "Sch", "Due", "Until", "Description"],
        },
        ["next"] = new CustomReport()
        {
            Name = "next",
            Columns = ["id", "start.age", "entry.age", "depends", "priority", "project", "tags", "recur", "scheduled.countdown", "due.relative", "until.remaining", "description", "urgency"],
            Description = "Most urgent tasks",
            Filter = "status:pending -WAITING limit:page",
            Labels = ["ID", "Active", "Age", "Deps", "P", "Project", "Tag", "Recur", "S", "Due", "Until", "Description", "Urg"],
        },
        ["oldest"] = new CustomReport()
        {
            Name = "oldest",
            Columns = ["id", "start.age", "entry", "entry.age", "modified.age", "depends.indicator", "priority", "project", "tags", "recur.indicator", "wait.remaining", "scheduled.countdown", "due", "until.age", "description"],
            Description = "Oldest tasks",
            Filter = "status:pending",
            Labels = ["ID", "Active", "Created", "Age", "Mod", "D", "P", "Project", "Tags", "R", "Wait", "Sch", "Due", "Until", "Description"],
        },
        ["overdue"] = new CustomReport()
        {
            Name = "overdue",
            Columns = ["id", "start.age", "entry.age", "depends", "priority", "project", "tags", "recur.indicator", "scheduled.countdown", "due", "until", "description", "urgency"],
            Description = "Overdue tasks",
            Filter = "status:pending and +OVERDUE",
            Labels = ["ID", "Active", "Age", "Deps", "P", "Project", "Tag", "R", "S", "Due", "Until", "Description", "Urg"],
        },
        ["ready"] = new CustomReport()
        {
            Name = "ready",
            Columns = ["id", "start.age", "entry.age", "depends.indicator", "priority", "project", "tags", "recur.indicator", "scheduled.countdown", "due.countdown", "until.remaining", "description", "urgency"],
            Description = "Most urgent actionable tasks",
            Filter = "+READY",
            Labels = ["ID", "Active", "Age", "D", "P", "Project", "Tags", "R", "S", "Due", "Until", "Description", "Urg"],
        },
        ["recurring"] = new CustomReport()
        {
            Name = "recurring",
            Columns = ["id", "start.age", "entry.age", "depends.indicator", "priority", "project", "tags", "recur", "scheduled.countdown", "due", "until.remaining", "description", "urgency"],
            Description = " Tasks",
            Filter = "status:pending and (+PARENT or +CHILD)",
            Labels = ["ID", "Active", "Age", "D", "P", "Project", "Tags", "Recur", "Sch", "Due", "Until", "Description", "Urg"],
        },
        ["unblocked"] = new CustomReport()
        {
            Name = "unblocked",
            Columns = ["id", "depends", "project", "priority", "due", "start.active", "entry.age", "description"],
            Description = " tasks",
            Filter = "status:pending -WAITING -BLOCKED",
            Labels = ["ID", "Deps", "Proj", "Pri", "Due", "Active", "Age", "Description"],
        },
        ["waiting"] = new CustomReport()
        {
            Name = "waiting",
            Columns = ["id", "start.active", "entry.age", "depends.indicator", "priority", "project", "tags", "recur.indicator", "wait", "wait.remaining", "scheduled", "due", "until", "description"],
            Description = "Waiting (hidden) tasks",
            Filter = "+WAITING",
            Labels = ["ID", "A", "Age", "D", "P", "Project", "Tags", "R", "Wait", "Remaining", "Sched", "Due", "Until", "Description"],
        },
    };
    public DefaultConfiguration()
    {
    }
    public Dictionary<string, ColumnConfig> Columns { get; set; } = new(){

        {"depends", new ColumnConfig("depends",true, List,Text,  [Standard,Indicator,List] )},
        {"description", new ColumnConfig("description",true, Standard,Text,  [Combined,Desc,Oneline,Truncated, Count, TruncatedCount] )},
        {"due", new ColumnConfig("due",true, Formatted,Date)},
        {"end", new ColumnConfig("end",true, Formatted,Date )},
        {"entry", new ColumnConfig("entry",true, Formatted,Date )},
        {"estimate", new ColumnConfig("estimate",true, Standard,Text,  [Standard,Indicator] )},
        {"modified", new ColumnConfig("modified",true, Formatted,Date )},
        {"parent", new ColumnConfig("parent",false, Long,Text ,[Long, Short])},
        {"project", new ColumnConfig("project",false, Full,Text, [ Full, Parent, Indented ] )},
        {"recur", new ColumnConfig("recur",false, Full,Text, [ Duration, Indicator ] )},
        {"scheduled", new ColumnConfig("scheduled",true, Formatted,Date )},
        {"start", new ColumnConfig("start",true, Formatted,Date )},
        {"status", new ColumnConfig("status",true, Long, Text,[Long, Short] )},
        {"tags", new ColumnConfig("tags",true, List, Text,[List,Indicator, Count] )},
        {"until", new ColumnConfig("until",true, Formatted,Date )},
        {"uuid", new ColumnConfig("uuid",false, Long,Text, [Long, Short] )},
        {"wait", new ColumnConfig("wait",true, Standard,Date )},
    };
}
