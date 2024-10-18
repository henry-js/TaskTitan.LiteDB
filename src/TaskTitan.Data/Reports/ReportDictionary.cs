using System.Runtime.Serialization;
using TaskTitan.Data.Reports;

namespace TaskTitan.Data.Reports;

public class ReportDictionary : Dictionary<string, CustomReport>
{
    public ReportDictionary() : base()
    {
    }

    public ReportDictionary(int capacity) : base(capacity)
    {
    }

    public ReportDictionary(IEqualityComparer<string> comparer) : base(comparer)
    {
    }

    public ReportDictionary(IDictionary<string, CustomReport> dictionary) : base(dictionary)
    {
    }

    public ReportDictionary(int capacity, IEqualityComparer<string> comparer) : base(capacity, comparer)
    {
    }

    public ReportDictionary(IDictionary<string, CustomReport> dictionary, IEqualityComparer<string> comparer) : base(dictionary, comparer)
    {
    }


}
