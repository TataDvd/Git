using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Tempo2012.EntityFramework.Models
{
    public class TableField : INotifyPropertyChanged
    {
        public int Id;
        public string Name { get; set;}
        public string NameEng { get; set; }
        public string DbField { get; set;}
        private bool _isUnique;
        public bool IsUnique
        {
            get { return _isUnique; }
            set
            {
                _isUnique = value;
                OnPropertyChanged("IsUnique");
            }
        }

        private bool _isRequared;
        public bool IsRequared
        {
            get { return _isRequared; }
            set
            {
                _isRequared = value;
                OnPropertyChanged("IsRequared");
            }
        }

        

        public int Length { get; set;}
        public string RFIELDNAME { get; set;}
        private string _rtablename;
        public string RTABLENAME
        {
            get { return _rtablename; }
            set { _rtablename = value;
            OnPropertyChanged("RTABLENAME");
            }
        }
        private string _rtn;
        public string Tn
        {
            get { return _rtn; }
            set {
                _rtn = value;
            OnPropertyChanged("Tn");
            }
        }

        private LookUpMetaData _sl;
        public LookUpMetaData Sl
        {
            get { return _sl; }
            set
            {
                _sl = value;
                if (Sl != null)
                {
                    Tn = Sl.Tablename;
                }
                OnPropertyChanged("Sl");
            }
        }

        public string RFIELDKEY { get; set;}
        public int RCODELOOKUP { get; set;}
        public int GROUP { get; set;}

        public override string  ToString()
        {
            StringBuilder stringBuilder=new StringBuilder();
            stringBuilder.AppendFormat("\"{0}\" {1}", NameEng, DbField);
            if (IsRequared) stringBuilder.Append(" NOT NULL");
            return stringBuilder.ToString();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
    
    public class TableCreator
    {
        private const string Title="CREATE TABLE \"{0}\"";
        public TableCreator()
        {
            this.Fields = new List<TableField>();
        }
        public TableCreator(string tableName,List<TableField> fields)
        {
            this.Fields = fields;
            this.Name = tableName;
        }
        public string Name;
        public List<TableField> Fields {get;set;}

        public override string ToString()
        {
            StringBuilder sb=new StringBuilder();
            sb.AppendFormat(Title, Name);
            sb.Append("(");
            sb.AppendFormat("{0},",new TableField
                           {
                               DbField = "Integer",
                               NameEng = "Id",
                               Name="Служебен код",
                               IsRequared = true,
                            });
            foreach (TableField field in Fields)
            {
                if (field.NameEng.ToLower() != "id")
                {
                    sb.AppendFormat("{0},", field);
                }
            }
            sb.AppendFormat("{0},",(new TableField
            {
                DbField = "Integer",
                NameEng = "FIRMAID",
                Name = "Фирма",
                IsRequared = true,
            }));
            sb.AppendFormat(" CONSTRAINT \"PK_{0}\" PRIMARY KEY (\"Id\")", Name);
            sb.Append(")");
            return sb.ToString();
        }
    }
}
