using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReportBuilder;

namespace TempoTest.ExportBuilderTest
{
    public class InporterNa:IExportable
    {
        public InporterNa(string filename, string tableName, string path)
        {
            FileName = filename;
            TableName = tableName;
            Path = path;
        }

        public List<List<string>> GetAllItems()
        {
            List<List<string>> rezi = new List<List<string>>();
            var items = ReadItems();
            foreach (Na item in items)
            {
                var item1 = new List<string>();
                item1.Add(item.CodetId);
                item1.Add(item.Name);
                item1.Add(item.Ap.ToString());
                rezi.Add(item1);
            }
            return rezi;
        }

        public List<IDbFields> GetAllFields()
        {
            return new List<IDbFields> { new DbField { DbType = "varchar", Title = "CodetId" },
                                         new DbField { DbType = "varchar", Title = "Name" },
                                         new DbField { DbType = "int", Title = "AP" },
                                         
            };
        }

        public string FileName { get; set; }

        public string TableName { get; set; }

        public string Path { get; set; }

        private List<Na>ReadItems()
        {
            var rez = new List<Na>();
            string[] lines = System.IO.File.ReadAllLines(Path);
            foreach (string line in lines)
            {
                var split=line.Split('|');
                Na n=new Na();
                n.CodetId = split[0].Trim();
                n.Name = split[1].Trim();
                if (split.Length > 2)
                {
                    n.Ap = 1;
                    if (split[2].Trim() == "п")
                    {
                        n.Ap = 2;
                    }
                }
                else
                {
                    n.Ap = 2;
                }
                rez.Add(n);
            }
            return rez;
        }


        public string SourceFile { get; set; }
    }
}
