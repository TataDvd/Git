using System;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using Tempo2012.EntityFramework;
using Tempo2012.EntityFramework.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Tempo2012.UI.WPF.Models;
using Tempo2012.UI.WPF.ViewModels.FirmManagment;
using System.Windows;

namespace Tempo2012.UI.WPF.ViewModels
{
    public class NomenclatureYN
    {
        public String Name{get;set;}
        public int Id{get;set;}
    }
    public class FirmViewModel:BaseViewModel,IDataErrorInfo
    {
        private EditMode _Mode;
        private int _oldindex=-1;
        private int _currentIndex=0;
        public FirmViewModel():base()
        {
            
            _AllFirms = new ObservableCollection<FirmModelWraper>();
            foreach ( var item in Context.GetAllFirma())
            {
                _AllFirms.Add(new FirmModelWraper(item));
            }
            _Mode = EditMode.View;
            _Cities =new ObservableCollection<City>(Context.GetAllCity());
            //_Cities2 =new ObservableCollection<City>(Context.GetAllCity());
           
            _Countries = new ObservableCollection<Country>(Context.GetAllCountry());
            if (_AllFirms.Count > 0)
            {
                CurrentFirma = _AllFirms.Last().CurrentFirma;
            }
            else CurrentFirma = new FirmModel();
            SetCityLookup();
        }

        private void SetCityLookup()
        {
            CityItem = new SaldoItem();
            CityItem.Name = "Град ПК";
            CityItem.SysLookup = true;
            CityItem.RCODELOOKUP = 6;
            CityItem.Relookup = 6;
            var firstOrDefault = _Cities.FirstOrDefault(e => e.Id == CurrentFirma.City);
            if (firstOrDefault != null)
            {
                CityItem.Lookupval = firstOrDefault.Zip;
                CityItem.Value = firstOrDefault.Name;
            }
            CityItem.LiD = CurrentFirma.City;
            CityItem1 = new SaldoItem();
            CityItem1.SysLookup = true;
            CityItem1.Name = "Град ПК";
            CityItem1.RCODELOOKUP = 6;
            CityItem1.Relookup = 6;
            CityItem1.LiD = CurrentFirma.City1;
            firstOrDefault = _Cities.FirstOrDefault(e => e.Id == CurrentFirma.City1);
            if (firstOrDefault != null)
            {
                CityItem1.Lookupval = firstOrDefault.Zip;
                CityItem1.Value = firstOrDefault.Name;
            }
        }

        public FirmViewModel(FirmModel firmaModel)
            :this()
        {
            if (firmaModel == null)
            {
                throw new NullReferenceException("firmamodel");
            }
            SetCityLookup();
            this._CurrentFirma = firmaModel;
        }
        protected override void MoveNext()
        {
            _oldindex = _currentIndex;
            if (Mode == EditMode.Edit) OldFirma = CurrentFirma.Clone();
            _currentIndex++;
            if (_currentIndex >= _AllFirms.Count)
            {
                _currentIndex = 0;
                
            }
            //CurrentFirma = _AllFirms[_currentIndex].CurrentFirma;
            CurrentFirmaWraper = _AllFirms[_currentIndex];
           
            
        }

        private bool hasSelection;
        public bool HasSelection 
        {
            get { return hasSelection; }
            set { hasSelection = value;
            OnPropertyChanged("HasSelection");
            } 
        }
        private FirmModelWraper _currentFirmaWraper;
        public FirmModelWraper CurrentFirmaWraper
        {
            get { return _currentFirmaWraper; }
            set {
                _currentFirmaWraper = value;
                if (value!=null)
                {
                    if (Mode == EditMode.Edit)
                    {
                        
                        if (_oldindex != -1)
                        {
                            AllFirms[_oldindex].CurrentFirma = OldFirma;
                        }
                        OldFirma = value.CurrentFirma.Clone();
                    }
                    CurrentFirma = value.CurrentFirma;
                    HasSelection = true;
                }
                OnPropertyChanged("CurrentFirmaWraper");
                
            }

        }

        public SaldoItem CityItem { get; set;}
        public SaldoItem CityItem1 { get; set; }
        protected override void MovePrevius()
        {
            _oldindex = _currentIndex;
            _currentIndex--;
            if (_currentIndex < 0 && _AllFirms.Count>0)
            {
                _currentIndex = _AllFirms.Count-1;

            }
            //CurrentFirma = _AllFirms[_currentIndex].CurrentFirma;
            CurrentFirmaWraper = _AllFirms[_currentIndex];
            
        }
        protected override void MoveFirst()
        {
            _oldindex = _currentIndex;
            if (_AllFirms.Count > 0)
            {
                //CurrentFirma = _AllFirms.First().CurrentFirma;
                if (Mode == EditMode.Edit) _AllFirms[_currentIndex].CurrentFirma = OldFirma;
                _currentIndex = 0;
                CurrentFirmaWraper = _AllFirms[_currentIndex];
                
            }
        }
        protected override void MoveLast()
        {
            _oldindex = _currentIndex;
            if (_AllFirms.Count > 0)
            {
                //CurrentFirma = _AllFirms.Last().CurrentFirma;

                if (Mode == EditMode.Edit) _AllFirms[_currentIndex].CurrentFirma = OldFirma;
                _currentIndex = AllFirms.Count - 1;
                CurrentFirmaWraper = _AllFirms[_currentIndex];
                OldFirma = CurrentFirma.Clone();
            }
        }

        protected override void  Save()
        {
            
            if (this.CanSave())
            {
                //FirmModel transport = CurrentFirma.Clone();
                CurrentFirma.City = CityItem.LiD;
                CurrentFirma.City1 = CityItem1.LiD;
                if (Mode == EditMode.Add)
                {

                    if (Context.Save<FirmModel>(CurrentFirma, true))
                    {
                        _AllFirms.Add(new FirmModelWraper(CurrentFirma));
                        ConfigTempoSinglenton currentconfig = ConfigTempoSinglenton.GetInstance();
                        foreach (var item in currentconfig.ConfigNames)
                        {
                            var spliter = item.Split('|');
                            FirmSettingModel newsett = new FirmSettingModel();
                            newsett.Key = spliter[0];
                            if (spliter.Length > 0)
                            { 
                                newsett.Name = spliter[1];
                            }
                            if (spliter.Length > 1)
                            {
                                newsett.Value = spliter[2];
                            }
                            newsett.FirmaId = CurrentFirma.Id;
                            newsett.HoldingId = currentconfig.ActiveHolding;
                            currentconfig.FirmSettings.Add(newsett);
                        }
                        if (CurrentFirma.Id==currentconfig.CurrentFirma.Id)
                        {
                            currentconfig.CurrentFirma = CurrentFirma.Clone();
                            
                        }
                        currentconfig.SaveConfiguration();
                        base.Save();
                        
                    }
                    else
                    {
                        MessageBoxWrapper.Show("Грешка при запис");
                    }
                }
                else
                {
                    if (!Context.Save<FirmModel>(CurrentFirma, false))
                    {
                        MessageBoxWrapper.Show("Грешка при запис");
                        
                    }
                    else
                    {
                        if (CurrentFirmaWraper!=null) CurrentFirmaWraper.CurrentFirma = CurrentFirma;
                        ConfigTempoSinglenton currentconfig = ConfigTempoSinglenton.GetInstance();
                        if (CurrentFirma.Id == currentconfig.CurrentFirma.Id)
                        {
                            currentconfig.CurrentFirma = CurrentFirma.Clone();
                            currentconfig.SaveConfiguration();
                        }
                        OnPropertyChanged("CurrentFirmaWraper");
                        base.Save();
                    }
                }
            }
            else
            {
                MessageBoxWrapper.Show("Невалидни данни! Моля проверете данните маркирани с червено!");
            }
           
        }
        protected override void View()
        {
            if (Mode==EditMode.Add)
            {
                MoveLast();
      
            }
            base.View();
        }
        protected override void Add()
        {
            CurrentFirma=new FirmModel();
            base.Add();
        }
        protected override void Update()
        {
            OldFirma = CurrentFirma.Clone();
            base.Update();
        }
        protected override bool CanSave()
        {
            return (_CurrentFirma.IsValid);
        }

        protected override void  Delete()
        {

            if (MessageBoxWrapper.Show("Сигурни ли сте, че желаете да изтриете този запис?", "Предупреждение", MessageBoxWrapperButton.YesNo) == MessageBoxWrapperResult.Yes)
            {
                if (CurrentIndex != -1)
                {
                    if (Context.DeleteFirma(CurrentFirma.Id))
                    {

                        AllFirms.Remove(AllFirms[CurrentIndex]);
                        MoveNext();


                        MessageBoxWrapper.Show("Записът е изтрит");
                    }
                    else
                    {
                        MessageBoxWrapper.Show("Грешка при триене");
                    }
                }
                else
                {
                    MessageBoxWrapper.Show("Не е избрана фирма за триене");
                }

            }
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
        public int CurrentIndex { get { return _currentIndex; } set
                                                                {
                                                                    if (Mode == EditMode.Edit) _oldindex = _currentIndex;    
                                                                	_currentIndex = value;
                                                                    OnPropertyChanged("CurrentIndex");
                                                               } }
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

        
        private ObservableCollection<FirmModelWraper> _AllFirms;
        public ObservableCollection<FirmModelWraper> AllFirms
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
        private bool _isBudjet;
        private bool _is510;
        private bool _isNormal;
       

        public FirmModel CurrentFirma 
        {
            get { return _CurrentFirma; }
            set
            {
                if (value==null)
                {
                    return;
                }
                if (_CurrentFirma != null && _CurrentFirma == value) return;
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
                OnPropertyChanged("RegisterDds");
                if (_CurrentFirma.AccType == 0)
                {
                    IsNormal = true;
                    IsBudjet = false;
                    Is510 = false;
                }
                if (_CurrentFirma.AccType == 1)
                {
                    IsNormal = false;
                    IsBudjet = false;
                    Is510 = true;
                }
                if (_CurrentFirma.AccType == 2)
                {
                    IsNormal = false;
                    IsBudjet = true;
                    Is510 = false;
                }
            }
        }

        public string Name {
            get { 
                if (_CurrentFirma != null)
                    return _CurrentFirma.Name;
                return "";
            }
            set {
                if (_CurrentFirma != null && _CurrentFirma.Name == value) return;
                _CurrentFirma.Name = value;
                OnPropertyChanged("Name");
            }
        }

        public string Bulstad
        {
            get { if (_CurrentFirma != null) return _CurrentFirma.Bulstad;
                return "";
            }
            set
            {
                if (_CurrentFirma != null && _CurrentFirma.Bulstad == value) return;
                _CurrentFirma.Bulstad = value;
                _CurrentFirma.DDSnum = "BG" + value;
                OnPropertyChanged("Bulstad");
                OnPropertyChanged("DDSnum");
                
            }
        }

        public bool RegisterDds
        {
            get { return _CurrentFirma != null && _CurrentFirma.RegisterDds;}
            set
            {
                if (_CurrentFirma != null) _CurrentFirma.RegisterDds = value;
                OnPropertyChanged("RegisterDds");
            }
        }

        public string DDSnum {
            get { 
                if (_CurrentFirma != null) return _CurrentFirma.DDSnum;
                return "";
            }
            set
            {
                if (_CurrentFirma.DDSnum == value) return;
                _CurrentFirma.DDSnum = value;
                OnPropertyChanged("DDSnum");
                
            }
        }
        public string Address
        {
            get { if (_CurrentFirma != null) return _CurrentFirma.Address;
                return "";
            }
            set
            {
                if (_CurrentFirma.Address == value) return;
                _CurrentFirma.Address = value;
                OnPropertyChanged("Address");
            }
        }
        public string Telefon
        {
            get { if (_CurrentFirma != null) return _CurrentFirma.Telefon;
                return "";
            }
            set
            {
                if (_CurrentFirma.Telefon == value) return;
                _CurrentFirma.Telefon = value;
                OnPropertyChanged("Telefon");
            }
        }
        public string Presentor
        {
            get { if (_CurrentFirma != null) return _CurrentFirma.Presentor;
                return "";
            }
            set
            {
                if (_CurrentFirma.Presentor == value) return;
                _CurrentFirma.Presentor = value;
                OnPropertyChanged("Presentor");
            }
        }
        public string NameBoss
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
        public string EGN
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
        public int PresentorYN
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
        public string Names
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
        public string Tel
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
        public string FirstName
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
        public string SurName
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
        public string LastName
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
        public string Address2
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
        public int City
        {
            get
            {
                return CityItem.LiD;
            }
            set
            {
                if (CityItem.LiD == value) return;
                CityItem.LiD = value;
                OnPropertyChanged("City");
            }
        }
        public int City1
        {
            get
            {
                return CityItem1.LiD;
            }
            set
            {
                if (CityItem1.LiD == value) return;
                CityItem1.LiD = value;
                OnPropertyChanged("City1");
            }
        }
        public int Country
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

        public bool IsBudjet
        {
            get { return _isBudjet; }
            set { _isBudjet = value; OnPropertyChanged("IsBudjet");
               if (value) AccType = 2;
            }
        }

        public bool Is510
        {
            get { return _is510; }
            set { _is510 = value; OnPropertyChanged("Is510");
                if (value)AccType = 1;
            }
        }

        public bool IsNormal
        {
            get { return _isNormal; }
            set { _isNormal = value; OnPropertyChanged("IsNormal");
               if (value) AccType = 0;
            }
        }

        public int AccType
        {
            get { return CurrentFirma.AccType; }
            set
            {
                _CurrentFirma.AccType = value;
                
            }
        }

        public string Error
        {
            get {return (_CurrentFirma as IDataErrorInfo).Error;}
        }

        public string this[string columnName]
        {
            get
            {
                string error = "";
                if (_CurrentFirma!=null)
                {
                    error = (_CurrentFirma as IDataErrorInfo)[columnName];
                    // Dirty the commands registered with CommandManager,
                    // such as our Save command, so that they are queried
                    // to see if they can execute now.
                    CommandManager.InvalidateRequerySuggested();
                }
                return error;
                
            }

        }


        public FirmModel OldFirma { get; set; }
        protected override void Cancel()
        {
            base.Cancel();
            MoveLast();
        }
    }
}
