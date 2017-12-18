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
using Tempo2012.EntityFramework;


namespace Tempo2012.UI.WPF.ViewModels
{
    public class LookupsViewModel : BaseViewModel,IDataErrorInfo
    {

        public LookupsViewModel():base()
        {

            this._NomenclatureFields = new ObservableCollection<TableField>(Context.GetAllLookupsFields());
            this._NomenclatureFieldsSelected = new ObservableCollection<TableField>();
            this._Lookups = new ObservableCollection<LookUpMetaData>(Context.GetAllLookups(""));
            this.IsShowDelete = true;
            this.IsShowEdit = true;
            this.IsShowSearch = false;
            this.IsShowView = true;
            _LookupModel = new LookupModel();
            //base.Add();
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
                _LookupModel = Context.GetLookup(this._Lookups[_CurrentLookup].Id);
                NomenclatureFieldsSelected =new ObservableCollection<TableField>(_LookupModel.Fields);
                NomenclatureFields = new ObservableCollection<TableField>(Context.GetAllLookupsFields());
                foreach (var current in NomenclatureFieldsSelected)
                {
                    NomenclatureFields.Remove(NomenclatureFields.FirstOrDefault(e => e.Id == current.Id));
                }
                OnPropertyChanged("Title");
                OnPropertyChanged("Description");
            }
        }
        protected override bool CanSave()
        {
            return ! this.IsStringMissing(this.Title); 
        }
        protected override void  Save()
        {
            if (Mode == EditMode.Edit)
            {
                if (_LookupModel != null)
                {

                    if (Context.UpdateLookup(_LookupModel))
                    {
                        MessageBoxWrapper.Show("Записа е променен");
                       
                    }
                    else
                    {
                        MessageBoxWrapper.Show("Грешка по време на запис");
                    }

                }
            }
            if (Mode == EditMode.Add)
            {
                 _LookupModel.Fields = new List<TableField>(this._NomenclatureFieldsSelected);
                 //this._LookupModel.Fields.Add(new TableField { Name = "Номер", DbField = "Integer", NameEng = "Id", Length = 10, IsNull = false });
                 Context.CreateTable(this._LookupModel);
                 this._Lookups = new ObservableCollection<LookUpMetaData>(Context.GetAllLookups(""));
                 OnPropertyChanged("Lookups");
                 
            }
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
        protected override void Delete()
        {
            if (CurrentLookup != null)
            {
                if (MessageBoxWrapper.Show("Сигурни ли сте, че желаете да изтриете този запис?", "Предупреждение", MessageBoxWrapperButton.YesNo) == MessageBoxWrapperResult.Yes)
                {
                   
                        if (Context.DeleteLookUp(this._Lookups[_CurrentLookup]))
                        {
                            MessageBoxWrapper.Show("Записът е изтрит");
                            this._Lookups = new ObservableCollection<LookUpMetaData>(Context.GetAllLookups(""));
                            OnPropertyChanged("Lookups");
                            MoveNext();
                        }
                        else
                        {
                            MessageBoxWrapper.Show("Записа не е изтрит");
                        }
                    
                }

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
                    case "NomenclatureFieldsSelected":
                        if (NomenclatureFieldsSelected.Count<1)
                            error = "Трябва да има поне едно добавено поле";
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
        public ObservableCollection<LookUpMetaData> Lookups
        {
            get
            {
                return _Lookups;
            }
            set
            {
                _Lookups = value;
                OnPropertyChanged("Lookups");
            }
        }
        private LookUpMetaData _Lookup;
        public LookUpMetaData Lookup
        {
            get
            {
                return _Lookup;
            }
            set
            {
                _Lookup = value;
                CurrentLookup=Lookups.IndexOf(value);
                OnPropertyChanged("Lookup");
            }
        }
    }
}
