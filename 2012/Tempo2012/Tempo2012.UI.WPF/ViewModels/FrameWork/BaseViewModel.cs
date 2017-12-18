using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Tempo2012.UI.WPF.ViewModels
{
    using System.ComponentModel;
    using Tempo2012.EntityFramework.Models;
    using System.Windows;
    using System.Windows.Input;
    using Tempo2012.EntityFramework;
    public class UIValidationErrorMessage
    {
        public string ErorrMessage { get; set;}
        public string PropertyName { get; set;}
        public override string ToString()
        {
            return string.Format("Поле {0} Грешка {1}",this.PropertyName,this.ErorrMessage);
        }
    }

    /// <summary>
    /// Abstract base to consolidate common functionality of all ViewModels
    /// </summary>
    [Serializable]
    public abstract class BaseViewModel: INotifyPropertyChanged
    {
        [XmlIgnore]
        [NonSerialized]
        private Dictionary<string,UIValidationErrorMessage> errorMessages=new Dictionary<string, UIValidationErrorMessage>();
        public void AddError()
        {
        }
        public UIValidationErrorMessage ReadError(string key)
        {
            if (errorMessages.ContainsKey(key))
            {
                return errorMessages[key];
            }
            return null;
        }
        public void RemoveError(string key)
        {
            if (errorMessages.ContainsKey(key))
            {
                errorMessages.Remove(key);
            }
        }

        [XmlIgnore]
        public EntityFramework.IDataBaseContext Context { get; set;}
        protected BaseViewModel()
        {
            Context = new EntityFramework.TempoDataBaseContext();
            this.AddCommand = new DelegateCommand((o) => this.Add(), (o) => this.CanAdd());
            this.UpdateCommand = new DelegateCommand((o) => this.Update(),(o) => this.CanUpdate());
            this.DeleteCommand = new DelegateCommand((o) => this.Delete(),(o) => this.CanDelete());
            this.MoveNextCommand = new DelegateCommand((o) => this.MoveNext());
            this.MovePreviusCommand = new DelegateCommand((o) => this.MovePrevius());
            this.MoveLastCommand = new DelegateCommand((o) => this.MoveLast());
            this.MoveFirstCommand = new DelegateCommand((o) => this.MoveFirst());
            this.SearchCommand = new DelegateCommand((o) => this.Search());
            this.ReportCommand = new DelegateCommand((o) => this.Report());
            this.ViewCommand = new DelegateCommand((o) => this.View());
            this.CancelCommand = new DelegateCommand((o) => this.Cancel());
            this.SaveCommand = new DelegateCommand((o) => this.Save(),(o)=>this.CanSave());
            this.AddNewCommand = new DelegateCommand((o) => this.AddNew(), (o) => this.CanAddNew());
            this.SaveWithRepeatCommand=new DelegateCommand((o) => this.AddNewRepeat(), (o) => this.CanAddNewRepeat());
            this._isShowAdd = true;
            this._isShowDelete = true;
            this._isShowEdit = true;
            this._isShowNavigation = true;
            this._isShowView = true;
            this._isShowSave = true;
            this._isShowSearch = true;
            this._isShowAddNew = false;
            this._isShowReport = false;
        }

        protected virtual bool CanDelete()
        {
            return true;
        }

        protected virtual bool CanUpdate()
        {
            return true;
        }

        protected virtual bool CanAddNewRepeat()
        {
            return false;
        }

        protected virtual void AddNewRepeat()
        {
            
        }

        protected virtual bool CanAdd()
        {
            return true;
        }

        protected virtual bool CanAddNew()
        {
            return true;
        }


        protected virtual bool CanSave()
        {
            return true;
        }
 
        
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        public bool DisebleEdits
        {
            get
            {
                return !(_enableUpdate || _enableInsert);
            }
        }
        
        public bool EnableEdits
        {
            get
            {
                return (_enableUpdate || _enableInsert);
            }
        
        }
        private bool _enableUpdate;
        public bool EnableUpdate
        {
            get
            {
                return _enableUpdate && IsShowEdit;
            }
            set
            {
                if (_enableUpdate == value) return;
                _enableUpdate = value;
                if (value) _enableInsert = false;
                OnPropertyChanged("EnableUpdate");
                OnPropertyChanged("DisebleUpdate");
                OnPropertyChanged("EnableEdits");
                OnPropertyChanged("DisebleEdits");
                OnPropertyChanged("EnableInsert");
                OnPropertyChanged("DisebleInsert");
                
            }
        }
        public bool DisebleUpdate
        {
            get
            {
                return !_enableUpdate&&IsShowEdit;
            }
        }
        public bool DisebleInsert
        {
            get
            {
                return !_enableInsert && IsShowAdd;
            }
        }
        private bool _enableInsert;
        public bool EnableInsert
        {
            get
            {
                return _enableInsert && IsShowAdd;
            }
            set
            {
                if (_enableInsert==value) return;
                _enableInsert = value;
                if (value) _enableUpdate = false;
                OnPropertyChanged("EnableInsert");
                OnPropertyChanged("DisebleInsert");
                OnPropertyChanged("EnableEdits");
                OnPropertyChanged("DisebleEdits");
                OnPropertyChanged("EnableUpdate");
                OnPropertyChanged("DisebleUpdate");
               
            }
        }
        private bool _isShowAdd;
        public bool IsShowAdd
        {
            get
            {
                return _isShowAdd;
            }
            set
            {
                _isShowAdd = value;
                OnPropertyChanged("IsShowAdd");
            }
        }
        private bool _isShowEdit;
        public bool IsShowEdit
        {
            get
            {
                return _isShowEdit;
            }
            set
            {
                _isShowEdit = value;
                OnPropertyChanged("IsShowEdit");
            }
        }
        private bool _isShowDelete;
        public bool IsShowDelete
        {
            get
            {
                return _isShowDelete;
            }
            set
            {
                _isShowDelete = value;
                OnPropertyChanged("IsShowDelete");
            }
        }
        private bool _isShowNavigation;
        public bool IsShowNavigation
        {
            get
            {
                return _isShowNavigation;
            }
            set
            {
                _isShowNavigation = value;
                OnPropertyChanged("IsShowNavigation");
            }
        }
        private bool _isShowView;
        public bool IsShowView
        {
            get
            {
                return _isShowView;
            }
            set
            {
                _isShowView = value;
                OnPropertyChanged("IsShowView");
            }
        }
        private bool _isShowSearch;
        public bool IsShowSearch
        {
            get
            {
                return _isShowSearch;
            }
            set
            {
                _isShowSearch = value;
                OnPropertyChanged("IsShowSearch");
            }
        }
        private bool _isShowSave;
        public bool IsShowSave
        {
            get
            {
                return _isShowSave;
            }
            set
            {
                _isShowSave = value;
                OnPropertyChanged("IsShowSave");
            }
        }
        private bool _isShowReport;
        public bool IsShowReport
        {
            get
            {
                return _isShowReport;
            }
            set
            {
                _isShowReport = value;
                OnPropertyChanged("IsShowReport");
            }
        }
        private EditMode _mode;
        public EditMode Mode
        {
            get { return _mode; }
            set 
            {
                if (_mode == value) return;
                _mode = value;
                //if (_mode==EditMode.Edit)
                //{
                //    Update();
                //}
                //if (_mode==EditMode.Add)
                //{
                //    Add();
                //}
                OnPropertyChanged("Mode");
            }
        }
        private bool _isShowAddNew;
        public bool IsShowAddNew
        {
            get
            {
                return _isShowAddNew;
            }
            set
            {
                _isShowAddNew = value;
                OnPropertyChanged("IsShowAddNew");
            }
        }
        protected virtual void Save()
        {
            View();
        }
        protected virtual void Cancel()
        {
            View();
        }
        protected virtual void View()
        {
            EnableUpdate = false;
            EnableInsert = false;
            Mode = EditMode.View;
        }
        protected virtual void Report()
        {
            MessageBoxWrapper.Show("Pressed report");
        }
        protected virtual void Search()
        {
            MessageBoxWrapper.Show("Press Search");
        }
        protected virtual void MoveFirst()
        {
            MessageBoxWrapper.Show("Pressed MoveFirst");
        }
        protected virtual void MoveLast()
        {
            MessageBoxWrapper.Show("Pressed Move Last");
        }
        protected virtual void MovePrevius()
        {
            MessageBoxWrapper.Show("Pressed Move Previus");
        }
        protected virtual void MoveNext()
        {
            MessageBoxWrapper.Show("Pressed Move Next");
        }
        protected virtual void Update()
        {
            Mode = EditMode.Edit;
            EnableUpdate = true;
            EnableInsert = false;  
        }
        protected virtual void Delete()
        {
            MessageBoxWrapper.Show("Pressed Delete");
        }
        protected virtual void Add()
        {
            Mode = EditMode.Add;
            EnableInsert = true;
            EnableUpdate = false;
            
        }
        protected virtual void AddNew()
        {
            Mode = EditMode.Add;
            EnableInsert = true;
            EnableUpdate = false;
        }
        [XmlIgnore]
        public virtual ICommand AddCommand { get; private set; }
        [XmlIgnore]
        public virtual ICommand AddNewCommand { get; private set; }
        [XmlIgnore]
        public virtual ICommand UpdateCommand { get; private set; }
        [XmlIgnore]
        public virtual ICommand DeleteCommand { get; private set; }
        [XmlIgnore]
        public virtual ICommand MoveNextCommand { get; private set; }
        [XmlIgnore]
        public virtual ICommand MovePreviusCommand { get; private set; }
        [XmlIgnore]
        public virtual ICommand MoveLastCommand { get; private set; }
        [XmlIgnore]
        public virtual ICommand MoveFirstCommand { get; private set; }
        [XmlIgnore]
        public virtual ICommand SearchCommand { get; private set; }
        [XmlIgnore]
        public virtual ICommand ReportCommand { get; private set; }
        [XmlIgnore]
        public virtual ICommand ViewCommand { get; private set; }
        [XmlIgnore]
        public virtual ICommand CancelCommand { get; private set; }
        [XmlIgnore]
        public virtual ICommand SaveCommand { get; private set; }
        [XmlIgnore]
        public virtual ICommand SaveWithRepeatCommand { get; private set; }

        public virtual string TitleInsert
        {
            get { return "F2-Добавяне";}
        }
        protected virtual void First()
        {
            IsShowNavigation = false;
            IsShowView = false;
            IsShowSearch = false;
            IsShowEdit = false;
            IsShowDelete = false;
            EnableUpdate = false;
        }
        protected virtual void Second()
        {
            IsShowNavigation = true;
            IsShowView = true;
            IsShowSearch = true;
            IsShowEdit = true;
            IsShowDelete = true;
            EnableUpdate = true;
        }
    }
    
}
