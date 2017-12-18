using System.Collections.Generic;

namespace ReportBuilder
{
    public interface ISerchable
    {
        string GetQuery();
        List<string> Columns();
    }
}