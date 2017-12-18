using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Tempo2012.EntityFramework.Models;
using System.Windows.Input;
using System.Windows;
using Tempo2012.EntityFramework;


namespace Tempo2012.UI.WPF.ViewModels.AccountManagment
{
    public class AnaliticalTypeManagerViewModel:BaseViewModel
    {
        public AnaliticalTypeManagerViewModel()
        {
            AllAnaliticTypes = new ObservableCollection<AnaliticalAccountType>(Context.GetAllAnaliticalAccountType());
            Fields = new ObservableCollection<AnaliticalFields>(Context.GetAllAnaliticalFields());
            AllConnectors = new ObservableCollection<MapAnanaliticAccToAnaliticField>(Context.GetAllConnectorTypeField());
            FieldsSelected = new ObservableCollection<AnaliticalFields>();
            if (AllAnaliticTypes.Count > 0)
            {
                CurrentAA = 0;
            }
        }

        protected ObservableCollection<MapAnanaliticAccToAnaliticField> AllConnectors { get; set; }

        public ObservableCollection<AnaliticalFields> FieldsSelected{get;set;}
        public ObservableCollection<AnaliticalFields> Fields { get; set; }
        public ObservableCollection<AnaliticalAccountType> AllAnaliticTypes { get; set; }

        
        public ObservableCollection<MapAnanaliticAccToAnaliticField> SelectedConnectors { get; set; }
        public ObservableCollection<AnaliticalAccountType> SelectedAnaliticTypes { get; set; }
        public string Title 
        {
            get { if (CurrentAllTypeAccount != null) { return CurrentAllTypeAccount.Name; } return ""; }
            set
            {
                CurrentAllTypeAccount.Name = value;
                OnPropertyChanged("Title");
            }
        }
        private AnaliticalAccountType _currentAllTypeAccount;
        public AnaliticalAccountType CurrentAllTypeAccount
        { 
            get { return _currentAllTypeAccount; }
            set {
                _currentAllTypeAccount = value;
                if (value != null)
                {
                    Fields = new ObservableCollection<AnaliticalFields>(Context.GetAllAnaliticalFields());
                    SelectedConnectors = new ObservableCollection<MapAnanaliticAccToAnaliticField>(AllConnectors.Where(e => e.AnaliticalFieldId == value.Id));

                    FieldsSelected.Clear();
                    foreach (var curr in SelectedConnectors)
                    {
                        var addfield = Fields.Where(e => e.Id == curr.AnaliticalNameID).FirstOrDefault();
                        if (addfield != null)
                        {
                            FieldsSelected.Add(addfield);
                            Fields.Remove(addfield);
                        }

                    }
                    //}
                }
                 OnPropertyChanged("CurrentAllTypeAccount");
                 OnPropertyChanged("Title");
                 OnPropertyChanged("FieldsSelected");
                 OnPropertyChanged("Fields");
                }
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
        private AnaliticalFields _currentFieldSelected;
        public AnaliticalFields CurrentFieldSelected
        {
            get { return _currentFieldSelected; }
            set { _currentFieldSelected = value; OnPropertyChanged("CurrentFieldSelected"); }
        }
        private AnaliticalFields _currentField;
        public AnaliticalFields CurrentField
        {
            get { return _currentField; }
            set { _currentField = value; OnPropertyChanged("CurrentField"); }
        }
        private void MoveSelectedRight()
        {
            if (CurrentFieldSelected != null)
            {
                Fields.Add(CurrentFieldSelected);
                FieldsSelected.Remove(CurrentFieldSelected);
                OnPropertyChanged("NomenclatureFieldsSelected");
                OnPropertyChanged("NomenclatureFields");
                if (FieldsSelected.Count > 0) { CurrentFieldSelected = FieldsSelected[0]; }
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
            OnPropertyChanged("FieldsSelected");
            OnPropertyChanged("Fields");

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
            if (CurrentField != null)
            {
                FieldsSelected.Add(CurrentField);
                Fields.Remove(CurrentField);
                OnPropertyChanged("FieldsSelected");
                OnPropertyChanged("Fields");
                if (Fields.Count > 0) { CurrentField = Fields[0]; }
            }
        }

        public string Error
        {
            get { return null; }
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
                    case "FieldsSelected":
                        if (FieldsSelected.Count < 1)
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

        protected override void Save()
        {
            if (Mode==EditMode.Edit)
            {
                Context.UpdateAT(CurrentAllTypeAccount, FieldsSelected);
                AllConnectors = new ObservableCollection<MapAnanaliticAccToAnaliticField>(Context.GetAllConnectorTypeField());
            }
            else
            {
                if (Mode==EditMode.Add)
                {
                    Context.SaveAT(CurrentAllTypeAccount, FieldsSelected);
                    AllConnectors = new ObservableCollection<MapAnanaliticAccToAnaliticField>(Context.GetAllConnectorTypeField());
                }
            }
            base.Save();
        }
        

        protected override void MoveFirst()
        {
            CurrentAA = 0;
        }
        protected override void MoveLast()
        {
            CurrentAA = this.AllAnaliticTypes.Count - 1;
        }
        protected override void MoveNext()
        {
            CurrentAA++;
        }
        protected override void MovePrevius()
        {
            CurrentAA--;
        }

        
        private int _currentAA;
        public int CurrentAA
        {
            get
            {
                return _currentAA;
            }
            set
            {
                _currentAA = value;
                if (_currentAA >= AllAnaliticTypes.Count)
                {
                    _currentAA = AllAnaliticTypes.Count - 1;
                }
                if (_currentAA < 0)
                {
                    _currentAA = 0;
                }

                CurrentAllTypeAccount = AllAnaliticTypes[_currentAA];
                
            }
        }
        protected override void Delete()
        {
            if (MessageBoxWrapper.Show("Сигурни ли сте, че желаете да изтриете този запис?", "Предупреждение", MessageBoxWrapperButton.YesNo) == MessageBoxWrapperResult.Yes)
                {
                    if (Context.DeleteAt(CurrentAllTypeAccount.Id))
                   {
                         MessageBoxWrapper.Show("Записът е изтрит");
                         AllAnaliticTypes.Remove(AllAnaliticTypes.Where(e => e.Id == CurrentAllTypeAccount.Id).FirstOrDefault());
                         CurrentAA++;
                         //base.Delete();
                    }
                    else
                    {
                        MessageBoxWrapper.Show("Грешка при триене");
                    }
                    
                }
               
           
        }
    }
    
}
