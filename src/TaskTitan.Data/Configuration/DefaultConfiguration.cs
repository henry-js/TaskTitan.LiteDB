using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using TaskTitan.Data;
using TaskTitan.Data.Reports;
using static TaskTitan.Data.Enums.ColFormat;
using static TaskTitan.Data.Enums.ColType;

namespace TaskTitan.Configuration;

public class DefaultConfiguration
{
    [DataMember(Name = "Report")]
    public ReportDictionary Reports { get; set; } = new()
    {
        ["active"] = new CustomReport("active")
        {
            Columns = ["id", "start", "start.age", "entry.age", "depends.indicator", "priority", "project", "tags", "recur", "wait", "scheduled.remaining", "due", "until", "description"],
            Description = "Active tasks",
            Filter = "status:pending and +ACTIVE",
            Labels = ["ID", "Started", "Active", "Age", "D", "P", "Project", "Tags", "Recur", "W", "Sch", "Due", "Until", "Description"],
        },
        ["all"] = new CustomReport("all")
        {
            Columns = ["id", "status.short", "uuid.short", "start.active", "entry.age", "end.age", "depends.indicator", "priority", "project.parent", "tags.count", "recur.indicator", "wait.remaining", "scheduled.remaining", "due", "until.remaining", "description"],
            Description = "All tasks",
            Labels = ["ID", "St", "UUID", "A", "Age", "Done", "D", "P", "Project", "Tags", "R", "Wait", "Sch", "Due", "Until", "Description"],
        },
        ["blocked"] = new CustomReport("blocked")
        {
            Columns = ["id", "depends", "project", "priority", "due", "start.active", "entry.age", "description"],
            Description = "Blocked tasks",
            Filter = "status:pending -WAITING +BLOCKED",
            Labels = ["ID", "Deps", "Proj", "Pri", "Due", "Active", "Age", "Description"],
        },
        ["blocking"] = new CustomReport("blocking")
        {
            Columns = ["id", "uuid.short", "start.active", "depends", "project", "tags", "recur", "wait", "scheduled.remaining", "due.relative", "until.remaining", "description.count", "urgency"],
            Description = "Blocking tasks",
            Filter = "status:pending -WAITING +BLOCKING",
            Labels = ["ID", "UUID", "A", "Deps", "Project", "Tags", "R", "W", "Sch", "Due", "Until", "Description", "Urg"],
        },
        ["completed"] = new CustomReport("completed")
        {
            Columns = ["id", "uuid.short", "entry", "end", "entry.age", "depends", "priority", "project", "tags", "recur.indicator", "due", "description"],
            Description = "Completed tasks",
            Filter = "status:completed",
            Labels = ["ID", "UUID", "Created", "Completed", "Age", "Deps", "P", "Project", "Tags", "R", "Due", "Description"],
        },
        ["list"] = new CustomReport("list")
        {
            Columns = ["id", "start.age", "entry.age", "depends.indicator", "priority", "project", "tags", "recur.indicator", "scheduled.countdown", "due", "until.remaining", "description.count", "urgency"],
            Description = "Most details of tasks",
            Filter = "status:pending -WAITING",
            Labels = ["ID", "Active", "Age", "D", "P", "Project", "Tags", "R", "Sch", "Due", "Until", "Description", "Urg"],
        },
        ["long"] = new CustomReport("long")
        {
            Columns = ["id", "start.active", "entry", "modified.age", "depends", "priority", "project", "tags", "recur", "wait.remaining", "scheduled", "due", "until", "description"],
            Description = "All details of tasks",
            Filter = "status:pending -WAITING",
            Labels = ["ID", "A", "Created", "Mod", "Deps", "P", "Project", "Tags", "Recur", "Wait", "Sched", "Due", "Until", "Description"],
        },
        ["ls"] = new CustomReport("ls")
        {
            Columns = ["id", "start.active", "depends.indicator", "project", "tags", "recur.indicator", "wait.remaining", "scheduled.countdown", "due.countdown", "until.countdown", "description.count"],
            Description = "Few details of tasks",
            Filter = "status:pending -WAITING",
            Labels = ["ID", "A", "D", "Project", "Tags", "R", "Wait", "S", "Due", "Until", "Description"],
        },
        ["minimal"] = new CustomReport("minimal")
        {
            Columns = ["id", "project", "tags.count", "description.count"],
            Description = "Minimal details of tasks",
            Filter = "status:pending",
            Labels = ["ID", "Project", "Tags", "Description"],
        },
        ["newest"] = new CustomReport("newest")
        {
            Columns = ["id", "start.age", "entry", "entry.age", "modified.age", "depends.indicator", "priority", "project", "tags", "recur.indicator", "wait.remaining", "scheduled.countdown", "due", "until.age", "description"],
            Description = "Newest tasks",
            Filter = "status:pending",
            Labels = ["ID", "Active", "Created", "Age", "Mod", "D", "P", "Project", "Tags", "R", "Wait", "Sch", "Due", "Until", "Description"],
        },
        ["next"] = new CustomReport("next")
        {
            Columns = ["id", "start.age", "entry.age", "depends", "priority", "project", "tags", "recur", "scheduled.countdown", "due.relative", "until.remaining", "description", "urgency"],
            Description = "Most urgent tasks",
            Filter = "status:pending -WAITING limit:page",
            Labels = ["ID", "Active", "Age", "Deps", "P", "Project", "Tag", "Recur", "S", "Due", "Until", "Description", "Urg"],
        },
        ["oldest"] = new CustomReport("oldest")
        {
            Columns = ["id", "start.age", "entry", "entry.age", "modified.age", "depends.indicator", "priority", "project", "tags", "recur.indicator", "wait.remaining", "scheduled.countdown", "due", "until.age", "description"],
            Description = "Oldest tasks",
            Filter = "status:pending",
            Labels = ["ID", "Active", "Created", "Age", "Mod", "D", "P", "Project", "Tags", "R", "Wait", "Sch", "Due", "Until", "Description"],
        },
        ["overdue"] = new CustomReport("overdue")
        {
            Columns = ["id", "start.age", "entry.age", "depends", "priority", "project", "tags", "recur.indicator", "scheduled.countdown", "due", "until", "description", "urgency"],
            Description = "Overdue tasks",
            Filter = "status:pending and +OVERDUE",
            Labels = ["ID", "Active", "Age", "Deps", "P", "Project", "Tag", "R", "S", "Due", "Until", "Description", "Urg"],
        },
        ["ready"] = new CustomReport("ready")
        {
            Columns = ["id", "start.age", "entry.age", "depends.indicator", "priority", "project", "tags", "recur.indicator", "scheduled.countdown", "due.countdown", "until.remaining", "description", "urgency"],
            Description = "Most urgent actionable tasks",
            Filter = "+READY",
            Labels = ["ID", "Active", "Age", "D", "P", "Project", "Tags", "R", "S", "Due", "Until", "Description", "Urg"],
        },
        ["recurring"] = new CustomReport("recurring")
        {
            Columns = ["id", "start.age", "entry.age", "depends.indicator", "priority", "project", "tags", "recur", "scheduled.countdown", "due", "until.remaining", "description", "urgency"],
            Description = " Tasks",
            Filter = "status:pending and (+PARENT or +CHILD)",
            Labels = ["ID", "Active", "Age", "D", "P", "Project", "Tags", "Recur", "Sch", "Due", "Until", "Description", "Urg"],
        },
        ["unblocked"] = new CustomReport("unblocked")
        {
            Columns = ["id", "depends", "project", "priority", "due", "start.active", "entry.age", "description"],
            Description = " tasks",
            Filter = "status:pending -WAITING -BLOCKED",
            Labels = ["ID", "Deps", "Proj", "Pri", "Due", "Active", "Age", "Description"],
        },
        ["waiting"] = new CustomReport("waiting")
        {
            Columns = ["id", "start.active", "entry.age", "depends.indicator", "priority", "project", "tags", "recur.indicator", "wait", "wait.remaining", "scheduled", "due", "until", "description"],
            Description = "Waiting (hidden) tasks",
            Filter = "+WAITING",
            Labels = ["ID", "A", "Age", "D", "P", "Project", "Tags", "R", "Wait", "Remaining", "Sched", "Due", "Until", "Description"],
        },
    };
    public DefaultConfiguration()
    {
    }

    [IgnoreDataMember]
    public static Dictionary<string, AttributeColumnConfig> Columns => new(){

        {"depends", new AttributeColumnConfig("depends",true, List,Text,  [Standard,Indicator,List] )},
        {"description", new AttributeColumnConfig("description",true, Standard,Text,  [Combined,Desc,Oneline,Truncated, Count, TruncatedCount] )},
        {"due", new AttributeColumnConfig("due",true, Formatted,Date)},
        {"end", new AttributeColumnConfig("end",true, Formatted,Date )},
        {"entry", new AttributeColumnConfig("entry",true, Formatted,Date )},
        {"estimate", new AttributeColumnConfig("estimate",true, Standard,Text,  [Standard,Indicator] )},
        {"modified", new AttributeColumnConfig("modified",true, Formatted,Date )},
        {"parent", new AttributeColumnConfig("parent",false, Long,Text ,[Long, Short])},
        {"project", new AttributeColumnConfig("project",false, Full,Text, [ Full, Parent, Indented ] )},
        {"recur", new AttributeColumnConfig("recur",false, Full,Text, [ Duration, Indicator ] )},
        {"scheduled", new AttributeColumnConfig("scheduled",true, Formatted,Date )},
        {"start", new AttributeColumnConfig("start",true, Formatted,Date )},
        {"status", new AttributeColumnConfig("status",true, Long, Text,[Long, Short] )},
        {"tags", new AttributeColumnConfig("tags",true, List, Text,[List,Indicator, Count] )},
        {"until", new AttributeColumnConfig("until",true, Formatted,Date )},
        {"uuid", new AttributeColumnConfig("uuid",false, Long,Text, [Long, Short] )},
        {"wait", new AttributeColumnConfig("wait",true, Standard,Date )},
    };
}
