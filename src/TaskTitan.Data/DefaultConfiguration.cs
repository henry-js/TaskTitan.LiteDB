using TaskTitan.Data.Reports;
using static TaskTitan.Data.Enums.ColFormat;
using static TaskTitan.Data.Enums.ColType;

namespace TaskTitan.Data;

public class DefaultConfiguration
{
    public Dictionary<string, CustomReport> Reports { get; set; } = new()
{
    { "all", new CustomReport()
        {
            Name = "all",
            Description = "All tasks",
            Labels = ["ID","St","UUID","A","Age","Done","D","P","Project","Tags","Recur","W","Sch","Due","Until","Description"],
            Columns = ["id","status.short","uuid.short","start.active","entry.age","end.age","depends.indicator","priority","project.parent","tags.count","recur.indicator","wait.remaining","scheduled.remaining","due","until.remaining","description"],
        }
    },
};
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
