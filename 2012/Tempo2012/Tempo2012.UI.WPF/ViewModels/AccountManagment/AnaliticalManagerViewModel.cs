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
    public class AnaliticalManagerViewModel:BaseViewModel
    {
        public AnaliticalManagerViewModel()
        {
            AllAnaliticalAccount = new ObservableCollection<AnaliticalAccount>(Context.GetAllAnaliticalAccount());
            AllAnaliticTypes = new ObservableCollection<AnaliticalAccountType>(Context.GetAllAnaliticalAccountType());
            Fields = new ObservableCollection<AnaliticalFields>(Context.GetAllAnaliticalFields());
            AllConnectors = new ObservableCollection<MapAnanaliticAccToAnaliticField>(Context.GetAllConnectorAnaliticField());
            FieldsSelected = new ObservableCollection<AnaliticalFields>();
            SelectedAnaliticalTypeFields = new ObservableCollection<AnaliticalFields>();
            AlaMapToType = new ObservableCollection<MapAnanaliticAccToAnaliticField>(Context.GetAllConnectorTypeField());
            AllAnaliticalFields = new ObservableCollection<AnaliticalFields>(Context.GetAllAnaliticalFields());
            if (AllAnaliticalAccount.Count > 0)
            {
                CurrentAA = 0;
            }
        }
        public ObservableCollection<AnaliticalAccount> AllAnaliticalAccount { get; set; }
        public ObservableCollection<AnaliticalFields> FieldsSelected{get;set;}
        public ObservableCollection<AnaliticalFields> Fields { get; set; }
        public ObservableCollection<MapAnanaliticAccToAnaliticField> AllConnectors { get; set; }
        public ObservableCollection<AnaliticalAccountType> AllAnaliticTypes { get; set; } 
        public ObservableCollection<MapAnanaliticAccToAnaliticField> SelectedConnectors { get; set; }
        public ObservableCollection<AnaliticalFields> SelectedAnaliticalTypeFields { get; set; }
        public ObservableCollection<MapAnanaliticAccToAnaliticField> AlaMapToType { get; set; }
        public ObservableCollection<AnaliticalFields> AllAnaliticalFields { get; set; }
        
        private AnaliticalAccount _currentAnaliticalAccount;
        public AnaliticalAccount CurrentAnaliticalAccount { 
            get
            {
                return _currentAnaliticalAccount;
            }
            set 
            {
                if (value==null) return;
                _currentAnaliticalAccount = value;

                _currentAllTypeAccount=AllAnaliticTypes.Where(e=>e.Id==value.TypeID).FirstOrDefault();
                
                if (value != null)
                {
                        Fields = new ObservableCollection<AnaliticalFields>(Context.GetAllAnaliticalFields());
                        SelectedConnectors = new ObservableCollection<MapAnanaliticAccToAnaliticField>(AllConnectors.Where(e => e.AnaliticalNameID == value.Id));
                       
                        FieldsSelected=new ObservableCollection<AnaliticalFields>();
                        foreach (var curr in SelectedConnectors)
                        {
                            var addfield = Fields.Where(e => e.Id == curr.AnaliticalFieldId).FirstOrDefault();
                            if (addfield != null)
                            {
                                addfield.Requared = curr.Required;
                                FieldsSelected.Add(addfield);
                                Fields.Remove(addfield);
                            }

                        }
                      var SelectedConnectors1 =
                      new ObservableCollection<MapAnanaliticAccToAnaliticField>(
                          AlaMapToType.Where(e => e.AnaliticalFieldId == value.TypeID));
                        SelectedAnaliticalTypeFields.Clear();
                        foreach (var curr in SelectedConnectors1)
                        {
                            var addfield = AllAnaliticalFields.FirstOrDefault(e => e.Id == curr.AnaliticalNameID);
                            if (addfield != null)
                            {
                                addfield.Requared = curr.Required;
                                SelectedAnaliticalTypeFields.Add(addfield);
                            }
                        }

                    //}
                }
                OnPropertyChanged("CurrentAnaliticalAccount");
                OnPropertyChanged("CurrentAllTypeAccount");
                OnPropertyChanged("Fields");
                OnPropertyChanged("FieldsSelected");
                OnPropertyChanged("Title");
            }
        }
       
       
        public ObservableCollection<AnaliticalAccountType> SelectedAnaliticTypes { get; set; }
        public string Title 
        {
            get { if (CurrentAnaliticalAccount != null) { return CurrentAnaliticalAccount.Name; } return ""; }
            set
            { 
                CurrentAnaliticalAccount.Name = value;
                OnPropertyChanged("Title");
            }
        }
        private AnaliticalAccountType _currentAllTypeAccount;
        public AnaliticalAccountType CurrentAllTypeAccount
        { 
            get { return _currentAllTypeAccount; }
            set {
                _currentAllTypeAccount = value;
                _currentAnaliticalAccount.TypeID = value.Id;
               
                OnPropertyChanged("CurrentAllTypeAccount");
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
            if (Mode == EditMode.Add)
            {
                Context.SaveAA(CurrentAnaliticalAccount, FieldsSelected);
                AllConnectors = new ObservableCollection<MapAnanaliticAccToAnaliticField>(Context.GetAllConnectorAnaliticField());
                AllAnaliticalAccount = new ObservableCollection<AnaliticalAccount>(Context.GetAllAnaliticalAccount());
                AllAnaliticTypes = new ObservableCollection<AnaliticalAccountType>(Context.GetAllAnaliticalAccountType());
            }
            else
            {
                if (Mode==EditMode.Edit)
                {
                    Context.UpdateAA(CurrentAnaliticalAccount, FieldsSelected);
                    AllConnectors = new ObservableCollection<MapAnanaliticAccToAnaliticField>(Context.GetAllConnectorAnaliticField());
                    AllAnaliticalAccount = new ObservableCollection<AnaliticalAccount>(Context.GetAllAnaliticalAccount());
                    AllAnaliticTypes = new ObservableCollection<AnaliticalAccountType>(Context.GetAllAnaliticalAccountType());
                }
            }
            base.Save();
        }

        protected override void Add()
        {
            Fields = new ObservableCollection<AnaliticalFields>(Context.GetAllAnaliticalFields());
            FieldsSelected=new ObservableCollection<AnaliticalFields>();
            Title = "";
            OnPropertyChanged("Title");
            OnPropertyChanged("Fields");
            OnPropertyChanged("FieldsSelected");
            base.Add();
        }
        protected override void Cancel()
        {
            MoveLast();
            base.Cancel();
        }
        protected override void MoveFirst()
        {
            CurrentAA = 0;
        }
        protected override void MoveLast()
        {
            CurrentAA = this.AllAnaliticalAccount.Count - 1;
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
                if (_currentAA >= AllAnaliticalAccount.Count)
                {
                    _currentAA = AllAnaliticalAccount.Count - 1;
                }
                if (_currentAA < 0)
                {
                    _currentAA = 0;
                }

                CurrentAnaliticalAccount = AllAnaliticalAccount[_currentAA];
                
            }
        }
        protected override void Delete()
        {
            if (MessageBoxWrapper.Show("Сигурни ли сте, че желаете да изтриете този запис?", "Предупреждение", MessageBoxWrapperButton.YesNo) == MessageBoxWrapperResult.Yes)
                {
                   if (Context.DeleteAA(CurrentAnaliticalAccount.Id))
                   {
                         MessageBoxWrapper.Show("Записът е изтрит");
                         AllAnaliticalAccount.Remove(AllAnaliticalAccount.Where(e=>e.Id==CurrentAnaliticalAccount.Id).FirstOrDefault());
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
