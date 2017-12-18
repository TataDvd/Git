using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tempo2012.UI.WPF.ViewModels
{
    using System.ComponentModel;
    using Tempo2012.EntityFramework.Models;
    using System.Windows;
    using System.Windows.Input;
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
    public abstract class BaseViewModel: INotifyPropertyChanged
    {
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

        protected Tempo2012.EntityFramework.Tempo2012DataBaseContext context = new EntityFramework.Tempo2012DataBaseContext();
        protected BaseViewModel()
        {
            this.AddCommand = new DelegateCommand((o) => this.Add());
            this.UpdateCommand = new DelegateCommand((o) => this.Update());
            this.DeleteCommand = new DelegateCommand((o) => this.Delete());
            this.MoveNextCommand = new DelegateCommand((o) => this.MoveNext());
            this.MovePreviusCommand = new DelegateCommand((o) => this.MovePrevius());
            this.MoveLastCommand = new DelegateCommand((o) => this.MoveLast());
            this.MoveFirstCommand = new DelegateCommand((o) => this.MoveFirst());
            this.SearchCommand = new DelegateCommand((o) => this.Search());
            this.ReportCommand = new DelegateCommand((o) => this.Report());
            this.ViewCommand = new DelegateCommand((o) => this.View());
            this.CancelCommand = new DelegateCommand((o) => this.Cancel());
            this.SaveCommand = new DelegateCommand((o) => this.Save(),(o)=>this.CanSave());
            this._IsShowAdd = true;
            this._IsShowDelete = true;
            this._IsShowEdit = true;
            this._IsShowNavigation = true;
            this._IsShowView = true;
            this._IsShowSave = true;
            this._IsShowSearch = true;
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
        private bool _EnableEdits;
        public bool EnableEdits
        {
            get
            {
                return _EnableEdits;
            }
            set
            {
                _EnableEdits = value;
                OnPropertyChanged("EnableEdits");
                OnPropertyChanged("DisebleEdits");
            }
        }
        private bool _IsShowAdd;
        public bool IsShowAdd
        {
            get
            {
                return _IsShowAdd;
            }
            set
            {
                _IsShowAdd = value;
                OnPropertyChanged("IsShowAdd");
            }
        }

        private bool _IsShowEdit;
        public bool IsShowEdit
        {
            get
            {
                return _IsShowEdit;
            }
            set
            {
                _IsShowEdit = value;
                OnPropertyChanged("IsShowEdit");
            }
        }

        private bool _IsShowDelete;
        public bool IsShowDelete
        {
            get
            {
                return _IsShowDelete;
            }
            set
            {
                _IsShowDelete = value;
                OnPropertyChanged("IsShowDelete");
            }
        }

        private bool _IsShowNavigation;
        public bool IsShowNavigation
        {
            get
            {
                return _IsShowNavigation;
            }
            set
            {
                _IsShowNavigation = value;
                OnPropertyChanged("IsShowNavigation");
            }
        }

        private bool _IsShowView;
        public bool IsShowView
        {
            get
            {
                return _IsShowView;
            }
            set
            {
                _IsShowView = value;
                OnPropertyChanged("IsShowView");
            }
        }

        private bool _IsShowSearch;
        public bool IsShowSearch
        {
            get
            {
                return _IsShowSearch;
            }
            set
            {
                _IsShowSearch = value;
                OnPropertyChanged("IsShowSearch");
            }
        }

        private bool _IsShowSave;
        public bool IsShowSave
        {
            get
            {
                return _IsShowSave;
            }
            set
            {
                _IsShowSave = value;
                OnPropertyChanged("IsShowSave");
            }
        }

        public bool DisebleEdits
        {
            get
            {
                return !_EnableEdits;
            }
        }
        private EditMode _Mode;
        public EditMode Mode
        {
            get { return _Mode; }
            set 
            {
                _Mode = value;
                OnPropertyChanged("Mode");
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
            Mode = EditMode.View;
            if (EnableEdits)
            {
                EnableEdits = false;
            }
        }
        protected virtual void Report()
        {
            MessageBox.Show("Pressed report");
        }
        protected virtual void Search()
        {
            MessageBox.Show("Press Search");
        }
        protected virtual void MoveFirst()
        {
            MessageBox.Show("Pressed MoveFirst");
        }
        protected virtual void MoveLast()
        {
            MessageBox.Show("Pressed Move Last");
        }
        protected virtual void MovePrevius()
        {
            MessageBox.Show("Pressed Move Previus");
        }
        protected virtual void MoveNext()
        {
            MessageBox.Show("Pressed Move Next");
        }
        protected virtual void Update()
        {
            Mode = EditMode.Edit;
            if (!EnableEdits)
            {
                EnableEdits = true;
            }
        }
        protected virtual void Delete()
        {
            MessageBox.Show("Pressed Delete");
        }
        protected virtual void Add()
        {
            Mode = EditMode.Add;
            if (!EnableEdits)
            {
                EnableEdits = true;
            }
        }
        public virtual ICommand AddCommand { get; private set; }
        public virtual ICommand UpdateCommand { get; private set; }
        public virtual ICommand DeleteCommand { get; private set; }
        public virtual ICommand MoveNextCommand { get; private set; }
        public virtual ICommand MovePreviusCommand { get; private set; }
        public virtual ICommand MoveLastCommand { get; private set; }
        public virtual ICommand MoveFirstCommand { get; private set; }
        public virtual ICommand SearchCommand { get; private set; }
        public virtual ICommand ReportCommand { get; private set; }
        public virtual ICommand ViewCommand { get; private set; }
        public virtual ICommand CancelCommand { get; private set; }
        public virtual ICommand SaveCommand { get; private set; }

        
    }
}
