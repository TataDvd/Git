using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Tempo2012.EntityFramework.Models;
using System.Windows;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Tempo2012.UI.WPF.ViewModels
{
    public class NomenclatureYN
    {
        public String Name{get;set;}
        public int Id{get;set;}
    }
    class FirmViewModel:BaseViewModel,IDataErrorInfo
    {
        private EditMode _Mode;
        private int _currentIndex=0;
        public FirmViewModel():base()
        {
            
            _AllFirms = new ObservableCollection<FirmModel>(context.GetAllFirma());
            _Mode = EditMode.View;
            _Cities =new ObservableCollection<City>(context.GetAllCity());
            _Cities2 =new ObservableCollection<City>(context.GetAllCity());
            _Countries = new ObservableCollection<Country>(context.GetAllCountry());
            if (_AllFirms.Count > 0) { CurrentFirma = _AllFirms.Last(); }
            else CurrentFirma = new FirmModel();
        }
        
        public FirmViewModel(FirmModel firmaModel)
            :this()
        {
            if (firmaModel == null)
            {
                throw new NullReferenceException("firmamodel");
            }
            this._CurrentFirma = firmaModel;
        }
        protected override void  Cancel()
        {
            if (Mode == EditMode.Add)
            {
                Delete();
            }
 	         base.Cancel();
        }
        protected override void MoveNext()
        {
            _currentIndex++;
            if (_currentIndex >= _AllFirms.Count)
            {
                _currentIndex = 0;
                
            }
            CurrentFirma = _AllFirms[_currentIndex];
        }
        protected override void MovePrevius()
        {
            _currentIndex--;
            if (_currentIndex <= 0 && _AllFirms.Count>0)
            {
                _currentIndex = _AllFirms.Count-1;

            }
            CurrentFirma = _AllFirms[_currentIndex];
        }
        protected override void MoveFirst()
        {
            if (_AllFirms.Count > 0) { CurrentFirma = _AllFirms.First(); _currentIndex = 0; }
        }
        protected override void MoveLast()
        {
            if (_AllFirms.Count > 0) { CurrentFirma = _AllFirms.Last(); _currentIndex = AllFirms.Count - 1; }
        }

        protected override void  Save()
        {
            OnPropertyChanged("CurrentFirma");
            if (this.CanSave())
            {
                FirmModel transport = CurrentFirma.Clone();
                if (Mode == EditMode.Add)
                {

                    if (context.Save<FirmModel>(transport, true))
                    {
                        base.Save();
                        CurrentFirma = transport;
                    }
                    else
                    {
                        MessageBox.Show("Грешка");
                    }
                }
                else
                {
                    if (!context.Save<FirmModel>(transport, false))
                    {
                        MessageBox.Show("Грешка");

                    }
                    else
                    {
                        base.Save();
                    }
                }
            }
            else
            {
                MessageBox.Show("Невалидни данни! Моля проверете данните маркирани с червено!");
            }
        }

        protected override bool CanSave()
        {
            return (_CurrentFirma.IsValid);
        }

        protected override void  Delete()
        {

            AllFirms.Remove(CurrentFirma);
            CurrentFirma = AllFirms.Last();
        }

        protected override void Add()
        {
            base.Add();
            //FirmModel model = CurrentFirma.Clone();
            FirmModel model = new FirmModel();
            _AllFirms.Add(model);
            CurrentFirma = model;
        }

        private static readonly ObservableCollection<NomenclatureYN> yesNo = new ObservableCollection<NomenclatureYN>
        {new NomenclatureYN{Name="ДА",Id=1},new NomenclatureYN{Name="Не",Id=0}};

        public ObservableCollection<NomenclatureYN> YesNo
        {
            get { return yesNo;}
        }
        private ObservableCollection<City> _Cities;
        public ObservableCollection<City> Cities
        {
            get
            {
                return _Cities;
            }
            set
            {
                _Cities = value;
                OnPropertyChanged("Cities");
            }
        }

        private ObservableCollection<City> _Cities2;
        public ObservableCollection<City> Cities2
        {
            get
            {
                return _Cities2;
            }
            set
            {
                _Cities2 = value;
                OnPropertyChanged("Cities2");
            }
        }
        private ObservableCollection<Country> _Countries;
        public ObservableCollection<Country> Countries
        {
            get 
            {
                return _Countries;
            }
            set
            {
                _Countries = value;
                OnPropertyChanged("Countries");
            }
        }

        
        private ObservableCollection<FirmModel> _AllFirms;
        public ObservableCollection<FirmModel> AllFirms
        {
            get 
            {
                return _AllFirms;
            }
            set
            {
                _AllFirms = value;
            }
        }

        private FirmModel _CurrentFirma;
        public FirmModel CurrentFirma 
        {
            get
            {
                return _CurrentFirma;
            }
            set
            {
                if (_CurrentFirma == value) return;
                _CurrentFirma = value;
                OnPropertyChanged("Name");
                OnPropertyChanged("Bulstad");
                OnPropertyChanged("DDSnum");
                OnPropertyChanged("Address");
                OnPropertyChanged("Telefon");
                OnPropertyChanged("Presentor");
                OnPropertyChanged("NameBoss");
                OnPropertyChanged("EGN");
                OnPropertyChanged("PresentorYN");
                OnPropertyChanged("Names");
                OnPropertyChanged("Tel");
                OnPropertyChanged("FirstName");
                OnPropertyChanged("SurName");
                OnPropertyChanged("LastName");
                OnPropertyChanged("Address2");
                OnPropertyChanged("City");
                OnPropertyChanged("City1");
                OnPropertyChanged("Country");
                OnPropertyChanged("CurrentFirma");
            
            }
        }

        public virtual string Name {
            get 
            {
                return _CurrentFirma.Name;
            }
            set {
                if (_CurrentFirma.Name == value) return;
                _CurrentFirma.Name = value;
                OnPropertyChanged("Name");
            }
        }
        public virtual string Bulstad
        {
            get
            {
                return "Bg"+_CurrentFirma.DDSnum;
            }
       }
        public virtual string DDSnum {
            get
            {
                return _CurrentFirma.DDSnum;
            }
            set
            {
                if (_CurrentFirma.DDSnum == value) return;
                _CurrentFirma.DDSnum = value;
                OnPropertyChanged("DDSnum");
                OnPropertyChanged("Bulstad");
            }
        }
        public virtual string Address
        {
            get
            {
                return _CurrentFirma.Address;
            }
            set
            {
                if (_CurrentFirma.Address == value) return;
                _CurrentFirma.Address = value;
                OnPropertyChanged("Address");
            }
        }
        public virtual string Telefon
        {
            get
            {
                return _CurrentFirma.Telefon;
            }
            set
            {
                if (_CurrentFirma.Telefon == value) return;
                _CurrentFirma.Telefon = value;
                OnPropertyChanged("Telefon");
            }
        }
        public virtual string Presentor
        {
            get
            {
                return _CurrentFirma.Presentor;
            }
            set
            {
                if (_CurrentFirma.Presentor == value) return;
                _CurrentFirma.Presentor = value;
                OnPropertyChanged("Presentor");
            }
        }
        public virtual string NameBoss
        {
            get
            {
                return _CurrentFirma.NameBoss;
            }
            set
            {
                if (_CurrentFirma.NameBoss == value) return;
                _CurrentFirma.NameBoss = value;
                OnPropertyChanged("NameBoss");
            }
        }
        public virtual string EGN
        {
            get
            {
                return _CurrentFirma.EGN;
            }
            set
            {
                if (_CurrentFirma.EGN == value) return;
                _CurrentFirma.EGN = value;
                OnPropertyChanged("EGN");
            }
        }
        public virtual int PresentorYN
        {
            get
            {
                return _CurrentFirma.PresentorYN;
            }
            set
            {
                if (_CurrentFirma.PresentorYN == value) return;
                _CurrentFirma.PresentorYN = value;
                OnPropertyChanged("PresentorYN");
            }
        }
        public virtual string Names
        {
            get
            {
                return _CurrentFirma.Names;
            }
            set
            {
                if (_CurrentFirma.Names == value) return;
                _CurrentFirma.Names = value;
                OnPropertyChanged("Names");
            }
        }
        public virtual string Tel
        {
            get
            {
                return _CurrentFirma.Tel;
            }
            set
            {
                if (_CurrentFirma.Tel == value) return;
                _CurrentFirma.Tel = value;
                OnPropertyChanged("Tel");
            }
        }
        public virtual string FirstName
        {
            get
            {
                return _CurrentFirma.FirstName;
            }
            set
            {
                if (_CurrentFirma.FirstName == value) return;
                _CurrentFirma.FirstName = value;
                OnPropertyChanged("FirstName");
            }
        }
        public virtual string SurName
        {
            get
            {
                return _CurrentFirma.SurName;
            }
            set
            {
                if (_CurrentFirma.SurName == value) return;
                _CurrentFirma.SurName = value;
                OnPropertyChanged("SurName");
            }
        }
        public virtual string LastName
        {
            get
            {
                return _CurrentFirma.LastName;
            }
            set
            {
                if (_CurrentFirma.LastName == value) return;
                _CurrentFirma.LastName = value;
                OnPropertyChanged("LastName");
            }
        }
        public virtual string Address2
        {
            get
            {
                return _CurrentFirma.Address2;
            }
            set
            {
                if (_CurrentFirma.Address2 == value) return;
                _CurrentFirma.Address2 = value;
                OnPropertyChanged("Address2");
            }
        }
        public virtual int City
        {
            get
            {
                return _CurrentFirma.City;
            }
            set
            {
                if (_CurrentFirma.City == value) return;
                _CurrentFirma.City = value;
                OnPropertyChanged("City");
            }
        }
        public virtual int City1
        {
            get
            {
                return _CurrentFirma.City1;
            }
            set
            {
                if (_CurrentFirma.City1 == value) return;
                _CurrentFirma.City1 = value;
                OnPropertyChanged("City1");
            }
        }
        public virtual int Country
        {
            get
            {
                return _CurrentFirma.Country;
            }
            set
            {
                if (_CurrentFirma.Country == value) return;
                _CurrentFirma.Country = value;
                OnPropertyChanged("Country");
            }
        }

        public string Error
        {
            get {return (_CurrentFirma as IDataErrorInfo).Error;}
        }

        public string this[string columnName]
        {
            get {
                
                string error = (_CurrentFirma as IDataErrorInfo)[columnName];
                // Dirty the commands registered with CommandManager,
                // such as our Save command, so that they are queried
                // to see if they can execute now.
                CommandManager.InvalidateRequerySuggested();

                return error;
                
            }

        }

    }
}