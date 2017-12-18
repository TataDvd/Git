using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tempo2012.EntityFramework.Models
{
    public class NomenclatureHedar:IField
    {
        public int Id { get; set;}
        public string Name{ get; set; }
        public string Description { get; set; }
    }

    public class NomeclatureMetaData:IField
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set;}
        public string Type { get; set;}

    }
    public class Conector:IConector
    {
        public int ParentId {get;set;}
        public int ChildId  {get;set;}
    }
    public class LookupModel
    {
        public LookupModel()
        {
            this.LookUpHedar = new NomenclatureHedar();
            this.Fields = new List<TableField>();
        }
        public LookupModel(List<TableField> fields,NomenclatureHedar lookupheader)
        {
            this.Fields = fields;
            this.LookUpHedar = lookupheader;
        }
        public NomenclatureHedar LookUpHedar{get;set;}
        public List<TableField> Fields{ get; set;}
    }
}
