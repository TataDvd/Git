using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Tempo2012.EntityFramework.Models;

namespace Tempo2012.UI.WPF.ViewModels
{
    public class SetUpMapAnaliticAccToLookupViewModel:BaseViewModel
    {
        public SetUpMapAnaliticAccToLookupViewModel()
        {
            _lookups = new ObservableCollection<LookUpMetaData>(Context.GetAllLookups(""));
            AccName = "102 Основен капитал";
            AccField = "Име";
            if (_lookups.Count > 0)
            {
                Lookup = _lookups[0];
            }
        }
        public SetUpMapAnaliticAccToLookupViewModel(AnaliticalFields analiticalFields)
        {
            this._workedItem = analiticalFields;
            List<LookUpMetaData> lookup = new List<LookUpMetaData>(Context.GetAllLookups(""));
            _lookups = new ObservableCollection<LookUpMetaData>();
            foreach (var item in lookup)
            {
                LookupModel lm = Context.GetLookup(item.Id);
                var fields = new ObservableCollection<TableField>(lm.Fields).Where(e => e.Name == analiticalFields.Name);
                if (fields.Count() >0)
                {
                    _lookups.Add(item);
                }
            }

            AccName = analiticalFields.NameAcc;
            AccField = analiticalFields.Name;
            if (_lookups.Count > 0)
            {
                Lookup = _lookups[0];
            }
           
            
           
        }

        public SetUpMapAnaliticAccToLookupViewModel(AnaliticalFields analiticalFields, AccountsModel accountsModel)
        {
            this._workedItem = analiticalFields;
            List<LookUpMetaData> lookup = new List<LookUpMetaData>(Context.GetAllLookups(""));
            _lookups = new ObservableCollection<LookUpMetaData>();
            foreach (var item in lookup)
            {
                LookupModel lm = Context.GetLookup(item.Id);
                var fields = new ObservableCollection<TableField>(lm.Fields).Where(e => e.Name == analiticalFields.Name);
                if (fields.Count() > 0)
                {
                    _lookups.Add(item);
                }
            }

            AccName = accountsModel.ToString();
            AccField = analiticalFields.Name;
            Lookup = _lookups.FirstOrDefault(w => w.Name == analiticalFields.NameLookUp);

        }
        private AnaliticalFields _workedItem;
        public AnaliticalFields WorkedItem
        {
            get { return _workedItem; }
            set
            {
                _workedItem = value;
            }
        }
        private string _accName;
        public string AccName
        {
            get { return _accName; }
            set { _accName = value; OnPropertyChanged("AccName"); }
        }
        
        public ObservableCollection<TableField> Fields { get; set;}
        private TableField _field;
        public TableField Field
        {
            get { return _field;}
            set
            {
                if (value==null) return;
                _field = value;
                _workedItem.IdField = value.Id;
                _workedItem.NameFieldLookUp = value.Name;
                OnPropertyChanged("Field");
            }
        }
        private ObservableCollection<LookUpMetaData> _lookups;
        public ObservableCollection<LookUpMetaData> Lookups
        {
            get
            {
                return _lookups;
            }
            set
            {
                _lookups = value;
                
                OnPropertyChanged("Lookups");
                OnPropertyChanged("ChainResult");
            }
        }

        private LookUpMetaData _lookup;
        public LookUpMetaData Lookup
        {
            get
            {
                return _lookup;
            }
            set
            {
                if (_lookup==value) return;
                _lookup = value;
                _workedItem.IdLookUp = value.Id;
                _workedItem.NameLookUp = value.Name;
                LookupModel lm = Context.GetLookup(_lookup.Id);
                Fields = new ObservableCollection<TableField>(lm.Fields.Where(e => e.DbField.ToUpper() == this._workedItem.FieldType.ToUpper()).ToList());
                _workedItem.IdLookUp = _lookup.Id;
                _workedItem.NameLookUp = _lookup.Name;
                if (Fields.Count > 0)
                {
                    Field = Fields[0];
                    OnPropertyChanged("ChainResult");
                }
                OnPropertyChanged("Fields");
                OnPropertyChanged("Lookup");

            }
        }

        private string _accField;
        private AnaliticalFields analiticalFields;
        private AccountsModel accountsModel;
        public string AccField
        {
            get { return _accField; }
            set { _accField = value; OnPropertyChanged("AccField");  }
        }
        
        public string ChainResult
        {
            get {
                if (_field!=null) return string.Format("{0}",_lookup.Name);
                return " ";
            }
       
        }

        
    }
}
