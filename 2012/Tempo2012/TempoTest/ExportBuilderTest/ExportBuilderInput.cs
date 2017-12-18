using System;
using System.Collections.Generic;
using System.Text;
using ReportBuilder;
using System.Linq;

namespace TempoTest.ExportBuilderTest
{
    public class ExportBuilderInput:IExportable
    {
        public ExportBuilderInput(string filename,string tablename,string source)
        {
            FileName = filename;
            TableName = tablename;
            SourceFile = source;
        }
        public List<List<string>> GetAllItems()
        {
            List<List<string>> rezi=new List<List<string>>();
            List<contr> rez=ReadCity();
            foreach (contr contr in rez)
            {
                var item = new List<string>();
                item.Add(contr.Kontragent.ToString());
                item.Add(contr.Name);
                item.Add(contr.NameEng);
                item.Add(contr.Bulstat);
                item.Add(contr.Vav);
                item.Add(contr.FirmaId.ToString());
                rezi.Add(item);
            }
            return rezi;
        }

        private List<contr> ReadCity()
        {
           List<contr> rez=new List<contr>();
           var enc = Encoding.GetEncoding("Windows-1251");
           string[] lines = System.IO.File.ReadAllLines(SourceFile, enc);
            int i = 0;
           foreach (string line in lines)
           {
               var splitline = line.Split('|');
               if (splitline.Length<2) continue;
               contr item = new contr { FirmaId = 2 };
               item.Kontragent = i++;
               item.Bulstat = splitline[0].Trim();
               item.Vav = splitline[0].Trim();
               item.Name = splitline[1].Trim();
               item.NameEng = splitline[1].Trim();
               item.FirmaId = 1;
               rez.Add(item);
           }
           return rez;
        }

        public List<IDbFields> GetAllFields()
        {
           // SELECT a."Id", a.KONTRAGENT, a."Name", a.NAMEENG, a.BULSTAT, a.VAT, a.FIRMAID
            //FROM "nom_17" a
            return new List<IDbFields> { new DbField { DbType = "int", Title = "KONTRAGENT" },
                                         new DbField { DbType = "varchar", Title = "Name" },
                                         new DbField { DbType = "varchar", Title = "NAMEENG" },
                                         new DbField { DbType = "varchar", Title = "BULSTAT" },
                                         new DbField { DbType = "varchar", Title = "VAT" },
                                         new DbField { DbType = "int", Title = "FIRMAID" }
            };
        }

        public string FileName { get; set;}

        public string TableName { get; set;}


        public string SourceFile { get; set; }
    }
}