using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReportBuilder
{
    public class ExportBuilder
    {
        private static string Generate(List<IDbFields> list,bool inout=true)
        {
            string values = "";
            bool first = true;
            foreach (var s in list)
            {
                if (first)
                {
                    values =inout?string.Format("\"{0}\"", s.Title): s.Title;
                    first = false;
                }
                else
                {
                    values = inout?string.Format("{0},\"{1}\"", values, s.Title):string.Format("{0},{1}", values, s.Title);
                }
            }
            return values;
        }

        private static string Generate(List<string> item,bool inout=true)
        {
            string values = "";
            bool first = true;
            foreach (string s in item)
            {
                if (first)
                {
                    values =inout?string.Format("'{0}'", s):s;
                    first = false;
                }
                else
                {
                    values = inout?string.Format("{0},'{1}'", values, s):string.Format("{0},{1}", values, s);
                }
            }
            return values;
        }

        public static IEnumerable<string> SqlScript(IExportable iExportable)
        {
            //INSERT INTO table_name (column1,column2,column3,...)
            //VALUES (value1,value2,value3,...);
            List<string> result = new List<string>();
            string colums = "";
            colums = Generate(iExportable.GetAllFields());
            foreach (var item in iExportable.GetAllItems())
            {
                var values = Generate(item);
                result.Add(string.Format("INSERT INTO \"{0}\" ({1}) VALUES({2});", iExportable.TableName, colums, values));
            }
            return result;
        }
        public static string SqlScriptString(IExportable iExportable)
        {
            //INSERT INTO table_name (column1,column2,column3,...)
            //VALUES (value1,value2,value3,...);
            StringBuilder sb=new StringBuilder();
            string colums = "";
            colums = Generate(iExportable.GetAllFields());
            foreach (var item in iExportable.GetAllItems())
            {
                var values = Generate(item);
                sb.AppendLine(string.Format("INSERT INTO \"{0}\" ({1}) VALUES({2});", iExportable.TableName, colums, values));
            }
            return sb.ToString();
        }
        public static bool GenerateExportCSV(IExportable iExportable)
        {
            //INSERT INTO table_name (column1,column2,column3,...)
            //VALUES (value1,value2,value3,...);
            bool result = true;
            StringBuilder sb=new StringBuilder();
            try
            {
                sb.AppendLine(Generate(iExportable.GetAllFields(),false));
                foreach (var item in iExportable.GetAllItems())
                {
                    var values = Generate(item,false);
                    sb.AppendLine(values);
                }
                SaveFile(sb.ToString(), iExportable.FileName);
            }
            catch (Exception)
            {
                result = false;
            }
            
            return result;
        }

        private static void SaveFile(string content, string filename)
        {
            string test = content;
        }
    }
}
