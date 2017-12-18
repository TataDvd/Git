using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReportBuilder;

namespace TempoTest.ExportBuilderTest
{
    public class DbField : IDbFields
    {
        public string Title { get; set;}

        public string DbType { get; set;}
    }
}
