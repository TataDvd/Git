using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tempo2012.EntityFramework.Models
{
    public class LookUpMetaData:IField
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set;}
        public string Type { get; set;}
        public string Tablename{ get; set;}
        public string NameEng { get; set; }
    }
    public class LookUpToFields:IConector
    {
        public int IdLookUp { get; set; }
        public int IdLookUpField { get; set; }
    }
    public class LookupModel
    {
        public LookupModel()
        {
            this.LookUpMetaData = new LookUpMetaData();
            this.Fields = new List<TableField>();
        }
        public LookupModel(List<TableField> fields,LookUpMetaData lookupheader)
        {
            this.Fields = fields;
            this.LookUpMetaData = lookupheader;
        }
        public LookUpMetaData LookUpMetaData{get;set;}
        public List<TableField> Fields{ get; set;}
    }
}
