using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Tempo2012.EntityFramework.Models
{
    public class AccHelper
    {
        public AccountsModel current{get;set;}
        public AnaliticHelper linked { get; set;}

    }
    public class AnaliticHelper 
    {
        public AnaliticalAccount Current { get; set;}
        public List<TableField>  AFields { get; set;}
        public List<LookupModel> Linked { get; set;}
        public List<MapAnaliticAccToLookUp> AnalitictoLookup { get; set; }
    }
    [Serializable]
    public class AnaliticalAccount
    {
        public virtual long Id { get; set; }
        public virtual string Name { get; set;}
        public virtual long TypeID { get; set;}
        public AnaliticalAccount Clone()
        {
            return (AnaliticalAccount)this.MemberwiseClone();
        }
    }

    [Serializable]
    public class AnaliticalAccountType
    {
        public virtual long Id { get; set; }
        public virtual string Name { get; set;}
        public AnaliticalAccountType Clone()
        {
            return (AnaliticalAccountType)this.MemberwiseClone();
        }
        public virtual bool Sl { get; set; }
        public virtual bool Sv { get; set;}
        public virtual bool Kol { get; set;}
    }

    [Serializable]
    public class AnaliticalFields : INotifyPropertyChanged
    {
        public  int Id { get; set; }
        public string Name { get; set; }
        public string NameAcc { get; set;}
        public string FieldType { get; set;}
        public int SortOrder { get; set; }
        private bool _requared;
        public  bool Requared
        {
            get { return _requared; }
            set
            {
                _requared = value;
                OnPropertyChanged("Requared");
            }
        }

        public AnaliticalFields Clone()
        {
            return (AnaliticalFields)this.MemberwiseClone();
        }

        public string NameLookUp { get; set;}
        public string NameFieldLookUp { get; set;}
        public int IdLookUp { get; set;}
        public int IdField { get; set;}
        public string RFIELDNAME { get; set; }
        public string RTABLENAME { get; set; }
        public string RFIELDKEY { get; set; }
        public int RCODELOOKUP { get; set; }
        
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string info)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(info));
            }
        }

        public int Group { get; set; }
    }

    [Serializable]
    public class MapAnaliticalFieldsToLookUp
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string FieldType { get; set; }
        public MapAnaliticalFieldsToLookUp Clone()
        {
            return (MapAnaliticalFieldsToLookUp)this.MemberwiseClone();
        }

        public virtual long LookUpId { get; set;}
        public virtual long LookUpFieldId { get; set;}
        public virtual string LookUpName { get; set;}
        public virtual string LookUpFieldName { get; set;}
    }

    [Serializable]
    public class MapAnanaliticAccToAnaliticField
    {
        public virtual int Id { get; set; }
        public virtual int AnaliticalNameID { get; set; }
        public virtual int AnaliticalFieldId { get; set; }
        public bool Required { get; set;}
        public virtual int SortOrder { get; set;}
        public MapAnanaliticAccToAnaliticField Clone()
        {
            return (MapAnanaliticAccToAnaliticField)this.MemberwiseClone();
        }
    }
    [Serializable]
    public class MapAnaliticAccToLookUp
    {
        public virtual int AnaliticalID { get; set; }
        public virtual int AnaliticalFieldId { get; set; }
        public virtual string LookupName { get; set; }
        public virtual string FieldName { get; set; }
        public MapAnaliticAccToLookUp Clone()
        {
            return (MapAnaliticAccToLookUp)this.MemberwiseClone();
        }
    }
}
