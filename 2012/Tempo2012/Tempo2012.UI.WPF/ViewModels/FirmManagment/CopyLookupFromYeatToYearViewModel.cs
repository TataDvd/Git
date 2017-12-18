using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;
using Tempo2012.EntityFramework;
using Tempo2012.EntityFramework.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Tempo2012.UI.WPF.ViewModels.FirmManagment;
using MessageBox = System.Windows.MessageBox;

namespace Tempo2012.UI.WPF.ViewModels
{
    
    public class CopyLookupFromFirmToFirmViewModel:BaseViewModel
    {

        public CopyLookupFromFirmToFirmViewModel()
            : base()
        {
            CurrentFirmaWraperDest=new FirmModelWraper(ConfigTempoSinglenton.GetInstance().CurrentFirma);
            _allFirms = new ObservableCollection<FirmModelWraper>();
            foreach ( var item in context.GetAllFirma().Where(e=>e.Id!=CurrentFirmaWraperDest.Id))
            {
                _allFirms.Add(new FirmModelWraper(item));
            }
            if (_allFirms.Count > 0) CurrentFirmaWraperSource=_allFirms[0];
            CopyCommand = new DelegateCommand((o) => this.CopyFirmToFirm(),(o)=>this.CanCopyFirmToFirm());
            
            this._Lookups = new ObservableCollection<LookUpMetaData>(context.GetAllLookups(""));
            if (this._Lookups != null)
            {
                _Lookup = _Lookups.Count > 0 ? _Lookups[0] : null;
                this.CalculateFields();
            }
        }

        private void CalculateFields()
        {
            if (CurrentFirmaWraperSource == null) 
            {
                MessageBox.Show("Не е избрана фирма, от която да се копират номенклатури");
                return;
            }
            this.Fields = new ObservableCollection<ObservableCollection<string>>();
            this.SFields = new ObservableCollection<ObservableCollection<string>>();
            if (_Lookup == null) return;
            LookupModel lm = context.GetLookup(_Lookup.Id);
            var title = new ObservableCollection<string>();
            foreach (var field in lm.Fields)
            {
                title.Add(field.Name);
            }
            title.Add("ФирмаКод");
            Fields.Add(title);
            var list = context.GetLookup(_Lookup.Tablename,CurrentFirmaWraperSource.Id);
            foreach (var li in list)
            {
                var ader = new ObservableCollection<string>(li);
                Fields.Add(ader);
            }
        }

        private bool CanCopyFirmToFirm()
        {
            return (CurrentFirmaWraperDest != null) && (CurrentFirmaWraperSource != null);
        }

        private void CopyFirmToFirm()
        {
            foreach (ObservableCollection<string> observableCollection in SFields)
            {
                context.SaveRow(new List<string>(observableCollection), context.GetLookup(Lookup.Id),CurrentFirmaWraperDest.Id);
            }
            MessageBox.Show(string.Format("Номенклатурата от Фирма {0} е копиран на {1}", CurrentFirmaWraperSource.CurrentFirma.Name,CurrentFirmaWraperDest.CurrentFirma.Name),"Известие");


        }

        public ICommand CopyCommand { get; private set;}

        private ObservableCollection<FirmModelWraper> _allFirms;
        public ObservableCollection<FirmModelWraper> AllFirms
        {
            get { return _allFirms; }
            set { _allFirms = value; }
        }

        private FirmModelWraper _currentFirmaWraperDest;
        public FirmModelWraper CurrentFirmaWraperDest
        {
            get { return _currentFirmaWraperDest; }
            set {
                _currentFirmaWraperDest = value;
                 OnPropertyChanged("CurrentFirmaWraperDest");
                
            }

        }
        private FirmModelWraper _currentFirmaWraperSource;
    
  
        public FirmModelWraper CurrentFirmaWraperSource
        {
            get { return _currentFirmaWraperSource; }
            set {
                _currentFirmaWraperSource = value;
                CalculateFields();
                OnPropertyChanged("Fields");
                OnPropertyChanged("Lookup");
                OnPropertyChanged("CurrentFirmaWraperSource");
       
            }
        }

        
        public ObservableCollection<ObservableCollection<string>> Fields
        {
            get;
            set;
        }

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
                CalculateFields();
                OnPropertyChanged("Fields");
                OnPropertyChanged("Lookup");
            }
        }

        public ObservableCollection<ObservableCollection<string>> SFields { get; set; }
    }
}
