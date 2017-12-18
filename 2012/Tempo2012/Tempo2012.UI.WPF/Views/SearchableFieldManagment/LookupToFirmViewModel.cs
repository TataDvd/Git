using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using Tempo2012.EntityFramework;
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.ViewModels;
using Tempo2012.UI.WPF.ViewModels.FirmManagment;

namespace Tempo2012.UI.WPF.Views.SearchableFieldManagment
{
    public class LookupToFirmViewModel:BaseViewModel
    {
        public LookupToFirmViewModel()
        {
            CurrentFirmaWraperDest = new FirmModelWraper(ConfigTempoSinglenton.GetInstance().CurrentFirma);
            _fn = CurrentFirmaWraperDest.Name;
            //_allFirms = new ObservableCollection<FirmModelWraper>();
            //foreach (var item in Context.GetAllFirma())
            //{
            //    _allFirms.Add(new FirmModelWraper(item));
            //}
            //if (_allFirms.Count > 0) CurrentFirmaWraperSource = _allFirms[0];
            

            this._Lookups = new ObservableCollection<LookUpMetaData>(Context.GetAllLookups(""));
            if (this._Lookups != null)
            {
                Lookup = _Lookups.Count > 0 ? _Lookups[0] : null;
                //this.CalculateFields();
            }

            Mappings=new ObservableCollection<ObservableCollection<string>>(Context.GetAllSearches());
        }

        //private ObservableCollection<FirmModelWraper> _allFirms;
        //public ObservableCollection<FirmModelWraper> AllFirms
        //{
        //    get { return _allFirms; }
        //    set { _allFirms = value; }
        //}

        private ObservableCollection<LookUpMetaData> _Lookups;
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
                if (value == null) return;
                CalculateFields();
                OnPropertyChanged("Fields");
                OnPropertyChanged("Lookup");
            }
        }

        private void CalculateFields()
        {
            LookupModel lm = Context.GetLookup(_Lookup.Id);
            Fields=new ObservableCollection<string>();
            lm.Fields.ForEach((TableField t) => Fields.Add(t.NameEng));
        }
        private FirmModelWraper _currentFirmaWraperDest;
        public FirmModelWraper CurrentFirmaWraperDest
        {
            get { return _currentFirmaWraperDest; }
            set
            {
                _currentFirmaWraperDest = value;
                OnPropertyChanged("CurrentFirmaWraperDest");

            }

        }
        private FirmModelWraper _currentFirmaWraperSource;
        private string _field;
        private string _fn;


        public FirmModelWraper CurrentFirmaWraperSource
        {
            get { return _currentFirmaWraperSource; }
            set
            {
                _currentFirmaWraperSource = value;
                //CalculateFields();
                OnPropertyChanged("Fields");
                OnPropertyChanged("Lookup");
                OnPropertyChanged("CurrentFirmaWraperSource");

            }
        }


        public ObservableCollection<string> Fields
        {
            get;
            set;
        }

        public string Field
        {
            get { return _field; }
            set { _field = value;OnPropertyChanged("Field"); }
        }

        public string Fn
        {
            get { return _fn; }
            set { _fn = value;OnPropertyChanged("Fn"); }
        }

        public ObservableCollection<ObservableCollection<string>> Mappings
        {
            get;
            set;
        }
        public int  SelectedIndex
        {
            get;
            set;
        }
        protected override void Add()
        {
            Context.SaveMap(CurrentFirmaWraperDest.Id, Lookup.Id, Field);
            Mappings = new ObservableCollection<ObservableCollection<string>>(Context.GetAllSearches());
            OnPropertyChanged("Mappings");
            Entrence.ConfigFirmaToLookup.LoadFromDb();
        }

        protected override void Delete()
        {
            if (SelectedIndex >= 0)
            {
                int fid = int.Parse(Mappings[SelectedIndex+1][0]);
                int lid = int.Parse(Mappings[SelectedIndex+1][2]);
                Context.DeleteMap(fid, lid);
                Mappings = new ObservableCollection<ObservableCollection<string>>(Context.GetAllSearches());
                OnPropertyChanged("Mappings");
                Entrence.ConfigFirmaToLookup.LoadFromDb();
            }
        }
    }
}
