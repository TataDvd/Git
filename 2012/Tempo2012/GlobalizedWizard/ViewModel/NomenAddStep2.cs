using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using CoffeeLibrary;
using GlobalizedWizard.Command;
using Tempo2012.EntityFramework.Models;

namespace GlobalizedWizard.ViewModel
{
    
    public class NomenAddStep2 : NomenWizardPageViewModelBase
    {
        public NomenAddStep2(NomenclatureWizardModel nomenclatureWizardModel) : base(nomenclatureWizardModel)
        {

            this._NomenclatureFields = new ObservableCollection<LookUpMetaData>(nomenclatureWizardModel.AllSelectedItems);
            this._NomenclatureFieldsSelected = new ObservableCollection<LookUpMetaData>();
       
        }
        
        private ObservableCollection<LookUpMetaData> _NomenclatureFields;
        public ObservableCollection<LookUpMetaData> NomenclatureFields
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
        private ObservableCollection<LookUpMetaData> _NomenclatureFieldsSelected;
        public ObservableCollection<LookUpMetaData> NomenclatureFieldsSelected
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
        private LookUpMetaData _CurrentLookUpSpecificFieldSelected;
        public LookUpMetaData CurrentLookUpSpecificFieldSelected
        {
            get { return _CurrentLookUpSpecificFieldSelected; }
            set
            {
                _CurrentLookUpSpecificFieldSelected = value;
                OnPropertyChanged("CurrentLookUpSpecificFieldSelected");
            }
        }
        private LookUpMetaData _CurrentLookUpSpecificField;
        public LookUpMetaData CurrentLookUpSpecificField
        {
            get { return _CurrentLookUpSpecificField; }
            set
            {
                _CurrentLookUpSpecificField = value;
                OnPropertyChanged("CurrentLookUpSpecificField");
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
            NomenclatureFields = new ObservableCollection<LookUpMetaData>(NomenclatureWizardModel.AllSelectedItems); 
            NomenclatureFieldsSelected=new ObservableCollection<LookUpMetaData>();
            NomenclatureWizardModel.SelectedItems=new List<LookUpMetaData>();
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
                NomenclatureWizardModel.SelectedItems.Remove(CurrentLookUpSpecificFieldSelected);
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
            NomenclatureWizardModel.SelectedItems =new List<LookUpMetaData>(NomenclatureWizardModel.AllSelectedItems);
            NomenclatureFieldsSelected = new ObservableCollection<LookUpMetaData>(NomenclatureWizardModel.AllSelectedItems);
            NomenclatureFields = new ObservableCollection<LookUpMetaData>();
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
                NomenclatureWizardModel.SelectedItems.Add(CurrentLookUpSpecificField);
                NomenclatureFields.Remove(CurrentLookUpSpecificField);
                OnPropertyChanged("NomenclatureFieldsSelected");
                OnPropertyChanged("NomenclatureFields");
                if (NomenclatureFields.Count > 0) {CurrentLookUpSpecificField=NomenclatureFields[0];}
            }
        }
    }
}
