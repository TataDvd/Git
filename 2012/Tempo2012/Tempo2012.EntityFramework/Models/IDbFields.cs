using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReportBuilder
{
    public interface IDbFields
    {
        string Title { get; set;}
        string DbType { get; set;}
    }
}
