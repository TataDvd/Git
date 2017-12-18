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

namespace Tempo2012.UI.WPF.ViewModels
{
    
    public class CopyLookupFromFirmToFirmViewModel:BaseViewModel
    {
        
        public CopyLookupFromFirmToFirmViewModel()
            : base()
        {
            CurrentFirmaWraperDest=new FirmModelWraper(ConfigTempoSinglenton.GetInstance().CurrentFirma);
            _allFirms = new ObservableCollection<FirmModelWraper>();
            foreach ( var item in Context.GetAllFirma().Where(e=>e.Id!=CurrentFirmaWraperDest.Id))
            {
                _allFirms.Add(new FirmModelWraper(item));
            }
            if (_allFirms.Count > 0) CurrentFirmaWraperSource=_allFirms[0];
            CopyCommand = new DelegateCommand((o) => this.CopyFirmToFirm(),(o)=>this.CanCopyFirmToFirm());
            
            this._Lookups = new ObservableCollection<LookUpMetaData>(Context.GetAllLookups(""));
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
                MessageBoxWrapper.Show("Не е избрана фирма, от която да се копират номенклатури");
                return;
            }
            this.Fields = new ObservableCollection<ObservableCollection<string>>();
            this.SFields = new ObservableCollection<ObservableCollection<string>>();
            if (_Lookup == null) return;
            LookupModel lm = Context.GetLookup(_Lookup.Id);
            var title = new ObservableCollection<string>();
            foreach (var field in lm.Fields)
            {
                title.Add(field.Name);
            }
            title.Add("ФирмаКод");
            Fields.Add(title);
            string fields = string.Format(" \"{0}\"",lm.Fields[0].NameEng);
            foreach (var field in lm.Fields.Skip(1))
            {
                fields = string.Format("{0},\"{1}\"",fields, field.NameEng);
            }
            fields = string.Format("{0},FIRMAID ", fields);
            var list = Context.GetLookup(_Lookup.Tablename,CurrentFirmaWraperSource.Id,"","",fields);
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
            bw = new BackgroundWorker();
            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;
            bw.DoWork += new DoWorkEventHandler(dw_ProcessRecords);
            bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
            bw.RunWorkerAsync();
            
            
        }

        private void dw_ProcessRecords(object sender, DoWorkEventArgs e)
        {
            ProcessRecords();
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ProgresInfo = "";
            MessageBoxWrapper.Show(string.Format("Номенклатурата от Фирма {0} е копиран на {1}", CurrentFirmaWraperSource.CurrentFirma.Name, CurrentFirmaWraperDest.CurrentFirma.Name), "Известие");
        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgresInfo = string.Format("Копирани {0} записа от {1}", e.ProgressPercentage,SFields.Count);;
        }

        private void ProcessRecords()
        {
            int i=0;
            StringBuilder sb=new StringBuilder();
            foreach (ObservableCollection<string> observableCollection in SFields)
            {
                LookupModel contextGetLookup = Context.GetLookup(Lookup.Id);
                contextGetLookup.Fields.Add(new TableField
                                                {
                                                    Name = "Фирма Код",
                                                    DbField = "integer",
                                                    NameEng = "FIRMAID"
                                                });
                if (!Context.SaveRow(new List<string>(observableCollection), contextGetLookup, CurrentFirmaWraperDest.Id))
                {
                    sb.Append("Повторение на елемент или грешка за ");
                    foreach (var s in observableCollection)
                    {
                        sb.Append(s);
                        sb.Append(",");
                    }
                    sb.AppendLine();
                }
               
                i++;
                bw.ReportProgress(i);
            }
            ReportString = sb.ToString();
        }

        public string ReportString
        {
            get { return _reportString; }
            set { _reportString = value; OnPropertyChanged("ReportString");}
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
        private string _progresInfo;
        private BackgroundWorker bw;
        private string _reportString;

        public string ProgresInfo
        {
            get { return _progresInfo; }
            set { _progresInfo = value; OnPropertyChanged("ProgresInfo"); }
        }
    }
}
