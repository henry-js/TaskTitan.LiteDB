using System.Collections;
using TaskTitan.Data.Expressions;

namespace TaskTitan.Data.Reports;

public class CustomReport : IReport
{
    public string Name { get; set; }
    public string Description { get; set; }
    public FilterExpression Filter { get; set; }
    public IEnumerable<string> Columns { get; set; } = [];
    public IEnumerable<string> Labels { get; set; } = [];

    // TODO: Add support for sorting
}

public interface IReport
{

}

public enum ReportType { Modifiable, Static, Custom }
