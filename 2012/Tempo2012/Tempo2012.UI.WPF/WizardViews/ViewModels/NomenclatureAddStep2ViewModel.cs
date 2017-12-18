using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Tempo2012.EntityFramework.Models;
using Tempo2012.EntityFramework.FakeData;
using Tempo2012.UI.WPF.ViewModels;
using System.Windows.Input;

namespace Tempo2012.UI.WPF.WizardViews.ViewModels
{
    class NomenclatureAddStep2ViewModel:WizardStepBase
    {
        public NomenclatureAddStep2ViewModel():base(null)
        {
            this._NomenclatureFields = new ObservableCollection<NomeclatureMetaData>(FakeDataContext.GetAllNomenclatureFields());
            this._NomenclatureFieldsSelected = new ObservableCollection<NomeclatureMetaData>();
        }
        private ObservableCollection<NomeclatureMetaData> _NomenclatureFields;
        public ObservableCollection<NomeclatureMetaData> NomenclatureFields
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
        private ObservableCollection<NomeclatureMetaData> _NomenclatureFieldsSelected;
        public ObservableCollection<NomeclatureMetaData> NomenclatureFieldsSelected
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
        private NomeclatureMetaData _CurrentNomenclaturesFieldSelected;
        public NomeclatureMetaData CurrentNomenclaturesFieldSelected
        {
            get { return _CurrentNomenclaturesFieldSelected; }
            set
            {
                _CurrentNomenclaturesFieldSelected = value;
                OnPropertyChanged("CurrentNomenclaturesFieldSelected");
            }
        }
        private NomeclatureMetaData _CurrentNomenclaturesField;
        public NomeclatureMetaData CurrentNomenclaturesField
        {
            get { return _CurrentNomenclaturesField; }
            set
            {
                _CurrentNomenclaturesField = value;
                OnPropertyChanged("CurrentNomenclaturesField");
            }
        }
        public override string DisplayName
        {
            get { return "Стъпка 2";}
        }

        public override bool IsValid()
        {
            return true;
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
            NomenclatureFields = new ObservableCollection<NomeclatureMetaData>(FakeDataContext.GetAllNomenclatureFields()); 
            NomenclatureFieldsSelected=new ObservableCollection<NomeclatureMetaData>();
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
            if (CurrentNomenclaturesFieldSelected != null)
            {
                NomenclatureFields.Add(CurrentNomenclaturesFieldSelected);
                NomenclatureFieldsSelected.Remove(CurrentNomenclaturesFieldSelected);
                OnPropertyChanged("NomenclatureFieldsSelected");
                OnPropertyChanged("NomenclatureFields");
                if (NomenclatureFieldsSelected.Count > 0) { CurrentNomenclaturesFieldSelected = NomenclatureFieldsSelected[0]; }
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
            NomenclatureFieldsSelected = new ObservableCollection<NomeclatureMetaData>(FakeDataContext.GetAllNomenclatureFields());
            NomenclatureFields = new ObservableCollection<NomeclatureMetaData>();
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
            if (CurrentNomenclaturesField != null)
            {
                NomenclatureFieldsSelected.Add(CurrentNomenclaturesField);
                NomenclatureFields.Remove(CurrentNomenclaturesField);
                OnPropertyChanged("NomenclatureFieldsSelected");
                OnPropertyChanged("NomenclatureFields");
                if (NomenclatureFields.Count > 0) {CurrentNomenclaturesField=NomenclatureFields[0];}
            }
        }
    }
}
