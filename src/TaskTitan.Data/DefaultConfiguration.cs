using static TaskTitan.Data.Enums;
using static TaskTitan.Data.Enums.ColFormat;
using static TaskTitan.Data.Enums.ColType;

namespace TaskTitan.Data;

public class DefaultConfiguration
{
    public Dictionary<string, ColumnConfig> Columns { get; set; } = new(){

        {"depends", new ColumnConfig("depends",true, Standard,Text,  [Standard,Indicator,List] )},
        {"description", new ColumnConfig("description",true, Standard,Text,  [Combined,Desc,Oneline,Truncated, Count, TruncatedCount] )},
        {"estimate", new ColumnConfig("estimate",true, Standard,Text,  [Standard,Indicator,List] )},
        {"due", new ColumnConfig("due",true, Standard,Date)},
        {"end", new ColumnConfig("end",true, Standard,Date )},
        {"entry", new ColumnConfig("entry",true, Standard,Date )},
        {"modified", new ColumnConfig("modified",true, Standard,Date )},
        {"scheduled", new ColumnConfig("scheduled",true, Standard,Date )},
        {"start", new ColumnConfig("start",true, Standard,Date )},
        {"until", new ColumnConfig("until",true, Standard,Date )},
        {"wait", new ColumnConfig("wait",true, Standard,Date )},
    };
}
