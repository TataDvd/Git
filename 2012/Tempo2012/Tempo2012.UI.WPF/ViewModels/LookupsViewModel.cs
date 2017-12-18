using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using Tempo2012.EntityFramework.FakeData;
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.Views;
using System.Windows.Forms;
using System.Windows.Input;
using System.ComponentModel;


namespace Tempo2012.UI.WPF.ViewModels
{
    public class LookupsViewModel : BaseViewModel,IDataErrorInfo
    {

        public LookupsViewModel():base()
        {

            this._NomenclatureFields = new ObservableCollection<TableField>(context.GetAllLookupsFields());
            this._NomenclatureFieldsSelected = new ObservableCollection<TableField>();
            this._Lookups = new ObservableCollection<LookUpMetaData>(context.GetAllLookups());
            this.IsShowDelete = false;
            this.IsShowEdit = false;
            this.IsShowSearch = false;
            this.IsShowView = false;
            _LookupModel = new LookupModel();
        }
        private int _CurrentLookup;
        public int CurrentLookup {
            get {
                return _CurrentLookup;
            }
            set
            {
                _CurrentLookup = value;
                if (_CurrentLookup >= _Lookups.Count)
                {
                    _CurrentLookup = _Lookups.Count - 1;
                }
                if (_CurrentLookup < 0)
                {
                    _CurrentLookup = 0;
                }
                _LookupModel = context.GetLookup(this._Lookups[_CurrentLookup].Id);
                NomenclatureFieldsSelected =new ObservableCollection<TableField>(_LookupModel.Fields);
                NomenclatureFields = new ObservableCollection<TableField>(context.GetAllLookupsFields());
                foreach (var current in NomenclatureFieldsSelected)
                {
                    NomenclatureFields.Remove(NomenclatureFields.FirstOrDefault(e => e.Id == current.Id));
                }
                OnPropertyChanged("Title");

            }
        }
        protected override bool CanSave()
        {
            return ! this.IsStringMissing(this.Title); 
        }
        protected override void  Save()
        {
            this._LookupModel.Fields = new List<TableField>(this._NomenclatureFieldsSelected);
            this._LookupModel.Fields.Add(new TableField { Name = "Номер", DbField = "Char(38)", NameEng = "Id", Length = 38, IsNull = false });
            context.CreateTable(this._LookupModel);
 	        base.Save();
        }
          private LookupModel _LookupModel;
      
        public string Title
        {
            get { return _LookupModel.LookUpMetaData.Name; }
            set {
                if (_LookupModel.LookUpMetaData.Name == value) return;
                _LookupModel.LookUpMetaData.Name = value;
                OnPropertyChanged("Title");
            }
        }
        public string Description
        {
            get { return _LookupModel.LookUpMetaData.Description; }
            set
            {
                if (_LookupModel.LookUpMetaData.Description == value) return;
                _LookupModel.LookUpMetaData.Description = value;
                OnPropertyChanged("Description");
            }
        }
        private ObservableCollection<TableField> _NomenclatureFields;
        public ObservableCollection<TableField> NomenclatureFields
        {
            get
            {
                return _NomenclatureFields;
            }
            set
            {
                _NomenclatureFields = value;
                OnPropertyChanged("NomenclatureFields");
            }
        }
        private ObservableCollection<TableField> _NomenclatureFieldsSelected;
        public ObservableCollection<TableField> NomenclatureFieldsSelected
        {
            get
            {
                return _NomenclatureFieldsSelected;
            }
            set
            {
                _NomenclatureFieldsSelected = value;
                OnPropertyChanged("NomenclatureFieldsSelected");
            }
        }
        private TableField _CurrentLookUpSpecificFieldSelected;
        public TableField CurrentLookUpSpecificFieldSelected
        {
            get { return _CurrentLookUpSpecificFieldSelected; }
            set
            {
                _CurrentLookUpSpecificFieldSelected = value;
                OnPropertyChanged("CurrentLookUpSpecificFieldSelected");
            }
        }
        private TableField _CurrentLookUpSpecificField;
        public TableField CurrentLookUpSpecificField
        {
            get { return _CurrentLookUpSpecificField; }
            set
            {
                _CurrentLookUpSpecificField = value;
                OnPropertyChanged("CurrentLookUpSpecificField");
            }
        }
        
        RelayCommand _MoveAllLeftCommand;
        public ICommand MoveAllLeftCommand
        {
            get 
            {
                if (_MoveAllLeftCommand == null)
                {
                    _MoveAllLeftCommand=new RelayCommand( () => this.MoveAllLeft());
                }
                return _MoveAllLeftCommand;

            }
        }

        private void MoveAllLeft()
        {
            //NomenclatureFields = new ObservableCollection<TableField>();
            //NomenclatureFieldsSelected = new ObservableCollection<TableField>();
            OnPropertyChanged("NomenclatureFieldsSelected");
            OnPropertyChanged("NomenclatureFields");
        }
        RelayCommand _MoveSelectedLeftCommand;
        public ICommand MoveSelectedLeftCommand
        {
            get
            {
                if (_MoveSelectedLeftCommand == null)
                {
                    _MoveSelectedLeftCommand = new RelayCommand(() => this.MoveSelectedLeft());
                }
                return _MoveSelectedLeftCommand;

            }
        }

        private void MoveSelectedRight()
        {
            if (CurrentLookUpSpecificFieldSelected != null)
            {
                NomenclatureFields.Add(CurrentLookUpSpecificFieldSelected);
                NomenclatureFieldsSelected.Remove(CurrentLookUpSpecificFieldSelected);
                OnPropertyChanged("NomenclatureFieldsSelected");
                OnPropertyChanged("NomenclatureFields");
                if (NomenclatureFieldsSelected.Count > 0) { CurrentLookUpSpecificFieldSelected = NomenclatureFieldsSelected[0]; }
            }
        }
        RelayCommand _MoveAllRightCommand;
        public ICommand MoveAllRightCommand
        {
            get
            {
                if (_MoveAllRightCommand == null)
                {
                    _MoveAllRightCommand = new RelayCommand(() => this.MoveAllRight());
                }
                return _MoveAllRightCommand;

            }
        }

        private void MoveAllRight()
        {
           // NomenclatureFieldsSelected = new ObservableCollection<TableField>();
           // NomenclatureFields = new ObservableCollection<TableField>();
            OnPropertyChanged("NomenclatureFieldsSelected");
            OnPropertyChanged("NomenclatureFields");
           
        }
        
        RelayCommand _MoveSelectedRightCommand;
        public ICommand MoveSelectedRightCommand
        {
            get
            {
                if (_MoveSelectedRightCommand == null)
                {
                    _MoveSelectedRightCommand = new RelayCommand(() => this.MoveSelectedRight());
                }
                return _MoveSelectedRightCommand;

            }
        }

        private void MoveSelectedLeft()
        {
            if (CurrentLookUpSpecificField != null)
            {
                NomenclatureFieldsSelected.Add(CurrentLookUpSpecificField);
                NomenclatureFields.Remove(CurrentLookUpSpecificField);
                OnPropertyChanged("NomenclatureFieldsSelected");
                OnPropertyChanged("NomenclatureFields");
                if (NomenclatureFields.Count > 0) {CurrentLookUpSpecificField=NomenclatureFields[0];}
            }
        }

        public string Error
        {
            get { return null;}
        }

        public string this[string columnName]
        {
            get
            {
                string error = null;
                switch (columnName)
                {
                    case "Title":
                        if (this.IsStringMissing(this.Title))
                            error = "Задължително поле";
                        break;
                }
                return error;

            }
        }
        bool IsStringMissing(string value)
        {
            return
                String.IsNullOrEmpty(value) ||
                value.Trim() == String.Empty;
        }

        public ObservableCollection<LookUpMetaData> _Lookups { get; set; }
        protected override void MoveFirst()
        {
            CurrentLookup = 0;
        }
        protected override void MoveLast()
        {
            CurrentLookup = this._Lookups.Count-1;
        }
        protected override void MoveNext()
        {
            CurrentLookup++;
        }
        protected override void  MovePrevius()
        {
            CurrentLookup--;
        }
    }
}
