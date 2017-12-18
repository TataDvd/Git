using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Tempo2012.EntityFramework.Models
{
    public class FieldsNomenclature
    {
        public int Minlen { get; set;}
        public int Minlen { get; set;}
        public string Name { get; set;}
        public string DataType { get; set;}
    }
    [Serializable]
    public class NomenclatureMenager
    { 
        private const string META = ".met";
        private const string DATA = ".dat";
        [Serializable]
        List<FieldsNomenclature> fields=new List<FieldsNomenclature>();
        [Serializable]
        List<Dictionary<string,object>> content=new List<Dictionary<string, object>>();
        [Serializable]

        public NomenclatureMenager(string name)
        {
            Name = name;
        }
        public string Name { get; set;}
        public void AddField(FieldsNomenclature fieldsNomenclature)
        {
            fields.Add(fieldsNomenclature);
        }
        public void AddContent(Dictionary<string,object> newelement)
        {
            content.Add(newelement);
        }
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            foreach (var item in content)
            {
                foreach (var field in item)
                {
                    result.Append(field.Value);
                    result.Append("|");
                }
                result.AppendLine();
            }
            return result.ToString();
        }
        public void LoadNomenclature()
        {
            if (File.Exists(Name+META))
            {
                fields = SerializeUtil.DeserializeFromXML<List<FieldsNomenclature>>(Name + META);
            }
            if (File.Exists(Name + DATA))
            {
                content = SerializeUtil.DeserializeFromXML<List<Dictionary<string, object>>>(Name + DATA);
            }

        }
        public void SaveNomenclature()
        {
            SerializeUtil.SerializeToXML<List<FieldsNomenclature>>(Name+META,fields);
            SerializeUtil.SerializeToXML<List<Dictionary<string, object>>>(Name+DATA,content);
        }

    }
}
