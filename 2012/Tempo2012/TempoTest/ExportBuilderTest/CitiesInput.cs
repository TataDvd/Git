using System.Collections.Generic;
using System.Text;
using ReportBuilder;

namespace TempoTest.ExportBuilderTest
{
    public class CitiesInput : IExportable
    {
        public List<List<string>> GetAllItems()
        {
            List<List<string>> rezi = new List<List<string>>();
            List<Sity> rez = ReadCities();
            foreach (Sity contr in rez)
            {
                var item = new List<string>();
                //item.Add(contr.Id.ToString());
                item.Add(contr.Name);
                item.Add(contr.Zip);
                item.Add(contr.CountryId.ToString());
                rezi.Add(item);
            }
            return rezi;
        }

        private List<Sity> ReadCities()
        {
            List<Sity> rez = new List<Sity>();
            var enc = Encoding.GetEncoding("Windows-1251");
            string[] lines = System.IO.File.ReadAllLines(SourceFile, enc);
            int i = 0;
            foreach (string line in lines)
            {
                var splitline = line.Split('|');
                if (splitline.Length < 1) continue;
                Sity item = new Sity { CountryId = 1};
                item.Id = i++;
                item.Zip = splitline[1].Trim();
                item.Name = splitline[0].Trim();
                rez.Add(item);
            }
            return rez;
        }

        public List<IDbFields> GetAllFields()
        {
            return new List<IDbFields> { new DbField { DbType = "varchar", Title = "Name" },
                                         new DbField { DbType = "varchar", Title = "Zip" },
                                         new DbField { DbType = "int", Title = "CountryId" }
            };
        }

        public string FileName { get; set;}

        public string TableName { get; set;}

        public string SourceFile { get; set;}
    }
}