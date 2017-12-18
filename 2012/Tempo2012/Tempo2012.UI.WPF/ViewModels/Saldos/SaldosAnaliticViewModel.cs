using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using System.Windows.Forms;
using Tempo2012.EntityFramework;
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.Models;
using Tempo2012.UI.WPF.Views.Saldos;
using Tempo2012.UI.WPF.ViewModels.ContragenManager;
using ReportBuilder;
using Tempo2012.UI.WPF.Views.ReportManager;

namespace Tempo2012.UI.WPF.ViewModels.Saldos
{
    public class SaldosAnaliticViewModel:BaseViewModel, IReportBuilder
    {

        public SaldosAnaliticViewModel()
            : base()
        {
            _sumaLvD = Vf.LevFormatUI;
            _sumaLvK = Vf.LevFormatUI;
            _sumaKK = "0.0000";
            _sumaKD = "0.0000";
            _sumaVK = "0.0000";
            _sumaVD = "0.0000";
            this.IsShowNavigation = false;
           
        }
        public void LoadSettings(string Path)
        {
            ReportItems = SerializeUtil.DeserializeFromXML<List<ReportItem>>(Path);
        }

        public void SaveSettings(string Path)
        {
            SerializeUtil.SerializeToXML<List<ReportItem>>(Path, ReportItems);
        }

        private int _CurrentRowIndex;
        public int CurrentRowIndex
        {
            get
            {
                return _CurrentRowIndex;
            }
            set
            {
                if (_CurrentRowIndex == value) return;
                _CurrentRowIndex = value;
            }
        }
        public DelegateCommand MoveNextPageCommand { get; set; }

        public DelegateCommand MovePreviusPageCommand { get; set; }

        public DelegateCommand MoveLastPageCommand { get; set; }

        public DelegateCommand MoveFirstPageCommand { get; set; }
        private void SetPager()
        {
            AllPages = FetchCount() / pagesize + 1;
            CurrentPage = 1;
            FromToPages = string.Format("{0} от {1}", CurrentPage, AllPages);
            this.MoveNextPageCommand = new DelegateCommand((o) => this.MoveNextPage());
            this.MovePreviusPageCommand = new DelegateCommand((o) => this.MovePreviusPage());
            this.MoveLastPageCommand = new DelegateCommand((o) => this.MoveLastPage());
            this.MoveFirstPageCommand = new DelegateCommand((o) => this.MoveFirstPage());
            FetchRange((CurrentPage - 1) * pagesize, pagesize);
        }

        private void MoveFirstPage()
        {
            if (CurrentPage == 1) return;
            CurrentPage = 1;
            FetchRange((CurrentPage - 1) * pagesize, pagesize);
        }

        internal void Refresh(Filter tag)
        {
            switch (tag.FilterField)
            {
                case "0":
                    {
                        AllSaldos = AllSaldoss.Where(e => e[0].Contains(tag.FilterWord)).ToList();
                        FetchCount();
                        CurrentPage = 1;
                        FetchRange(0, pagesize);
                    }
                    break;
                case "1":
                    {
                        AllSaldos = AllSaldoss.Where(e => e[1].Contains(tag.FilterWord)).ToList();
                        FetchCount();
                        CurrentPage = 1;
                        FetchRange(0, pagesize);
                    }
                    break;
                case "2":
                    {
                        AllSaldos = AllSaldoss.Where(e => e[2].Contains(tag.FilterWord)).ToList();
                        FetchCount();
                        CurrentPage = 1;
                        FetchRange(0, pagesize);
                    }
                    break;
                case "3":
                    {
                        AllSaldos = AllSaldoss.Where(e => e[3].Contains(tag.FilterWord)).ToList();
                        FetchCount();
                        CurrentPage = 1;
                        FetchRange(0, pagesize);
                    }
                    break;
                case "4":
                    {
                        AllSaldos = AllSaldoss.Where(e => e[4].Contains(tag.FilterWord)).ToList();
                        FetchCount();
                        CurrentPage = 1;
                        FetchRange(0, pagesize);
                    }
                    break;
                case "5":
                    {
                        AllSaldos = AllSaldoss.Where(e => e[5].Contains(tag.FilterWord)).ToList();
                        FetchCount();
                        CurrentPage = 1;
                        FetchRange(0, pagesize);
                    }
                    break;
                case "6":
                    {
                        AllSaldos = AllSaldoss.Where(e => e[6].Contains(tag.FilterWord)).ToList();
                        FetchCount();
                        CurrentPage = 1;
                        FetchRange(0, pagesize);
                    }
                    break;
                case "7":
                    {
                        AllSaldos = AllSaldoss.Where(e => e[7].Contains(tag.FilterWord)).ToList();
                        FetchCount();
                        CurrentPage = 1;
                        FetchRange(0, pagesize);
                    }
                    break;
                case "8":
                    {
                        AllSaldos = AllSaldoss.Where(e => e[8].Contains(tag.FilterWord)).ToList();
                        FetchCount();
                        CurrentPage = 1;
                        FetchRange(0, pagesize);
                    }
                    break;
            }
        }

        private int _currentPage;
        public int CurrentPage
        {
            get { return _currentPage; }
            set
            {
                _currentPage = value;
                FromToPages = string.Format("{0} от {1}", value, AllPages);
            }
        }


        private void MoveLastPage()
        {
            if (CurrentPage == AllPages) return;
            CurrentPage = AllPages;
            FetchRange((CurrentPage - 1) * pagesize, pagesize);
        }

        private int _allPages;
        public int AllPages
        {
            get { return _allPages; }
            set { _allPages = value; }
        }

        private string _fromToPages;
        public string FromToPages
        {
            get { return _fromToPages; }
            set { _fromToPages = value; OnPropertyChanged("FromToPages"); }
        }

        private void MoveNextPage()
        {
            if (CurrentPage == AllPages)
            {
                CurrentPage = 1;
            }
            else
            {
                CurrentPage += 1;
            }
            FetchRange((CurrentPage - 1) * pagesize, pagesize);
        }

        private void MovePreviusPage()
        {
            if (CurrentPage == 1)
            {
                CurrentPage = AllPages;
            }
            else
            {
                CurrentPage -= 1;
            }
            FetchRange((CurrentPage - 1) * pagesize, pagesize);
        }
        public int FetchCount()
        {
            if (AllSaldos == null) return 0;
            return AllSaldos.Count;
        }

        public void FetchRange(int startIndex, int count)
        {
            var list = AllSaldos.Skip(startIndex).Take(count);
          
            Contents = new ObservableCollection<ObservableCollection<string>>();
            if (Titles!=null) Contents.Add(new ObservableCollection<string>(Titles));
            foreach (var item in list)
            {
                Contents.Add(new ObservableCollection<string>(item));
            }
            OnPropertyChanged("Contents");
        }
        public ObservableCollection<string> Titles { get; set; }
        public ObservableCollection<ObservableCollection<string>> Contents { get; set; }

        private string _sumaLvD;
        public string SumaLvD
        {
            get { return accountsModel.SaldoDL.ToString("F2"); }
            set
            {
                
                _sumaLvD = value;
                decimal tempvar = 0;
                accountsModel.SaldoDL = decimal.TryParse(_sumaLvD,out tempvar)?tempvar:0;
                OnPropertyChanged("SumaLvD");
            }
        }

        private string _sumaLvK;
        public string SumaLvK
        {
            get { return accountsModel.SaldoKL.ToString("F2"); }
            set
            {
                _sumaLvK = value;
                decimal tempvar = 0;
                accountsModel.SaldoKL =  decimal.TryParse(_sumaLvK,out tempvar)?tempvar:0;
                OnPropertyChanged("SumaLvK");
           
            }
        }

        private ObservableCollection<SaldoItem> _saldoItems;
        public ObservableCollection<SaldoItem> SaldoItems
        {
            get { return _saldoItems; }
            set { _saldoItems = value; }
        }

        private ObservableCollection<SaldoItem> _saldoItemDebitCredits;
        public ObservableCollection<SaldoItem> SaldoItemDebitCredits
        {
            get { return _saldoItemDebitCredits; }
            set { _saldoItemDebitCredits = value; }
        }

        private List<List<String>> _movements = new List<List<string>>();
       

        private int _accID;
        public int  AccID
        {
            get { return _accID; }
            set { _accID = value; OnPropertyChanged("AccId");}
        }

       
        private ObservableCollection<AnaliticalFields> observableCollection;
        private AnaliticalAccountType analiticalAccountType;
        private ObservableCollection<AnaliticalFields> staticFields;
        private ObservableCollection<AnaliticalFields> observableCollection_2;
        public AccountsModel accountsModel;
        private AccountsModel CurrentAccount;
        public string Title { get; set;}

        protected override void Report()
        {
            ReportDialog report = new ReportDialog(this);
            report.ShowDialog();
        }
        public SaldosAnaliticViewModel(ObservableCollection<AnaliticalFields> observableCollection, ObservableCollection<AnaliticalFields> observableCollection_2, ObservableCollection<AnaliticalFields> staticFields, AccountsModel accountsModel)
        {
            Title = string.Format("Начални аналитични салда на сметка {0}",accountsModel);
            IsCol = accountsModel.SaldoDK > 0 || accountsModel.SaldoKK > 0;
            IsVal = accountsModel.SaldoDV > 0 || accountsModel.SaldoKV > 0;
            this.IsShowNavigation = false;
            this.observableCollection = observableCollection;
            this.observableCollection_2 = observableCollection_2;
            this.staticFields = staticFields;
            this.accountsModel = accountsModel;
            this.IsShowReport = true;
            Titles = new ObservableCollection<string>();
            Contents = new ObservableCollection<ObservableCollection<string>>();
            ObservableCollection<string> vals = new ObservableCollection<string>();
            _saldoItems = new ObservableCollection<SaldoItem>();
            foreach (AnaliticalFields analiticalFields in staticFields)
            {
                Titles.Add(analiticalFields.Name + "Кредит");
                Titles.Add(analiticalFields.Name + "Дебит");

                SaldoItemTypes saldotype = SaldoItemTypes.String;
                if (analiticalFields.FieldType == "integer")
                {
                    saldotype = SaldoItemTypes.Integer;
                }
                if (analiticalFields.FieldType.Contains("DECIMAL"))
                {
                    saldotype = SaldoItemTypes.Currency;
                }
                if (analiticalFields.FieldType == "Date")
                {
                    saldotype = SaldoItemTypes.Date;
                }
                SaldoItem saldoItem = new SaldoItem
                {
                    Name = analiticalFields.Name,
                    Type = saldotype,
                    Length = 50,
                    Value = Vf.LevFormatUI,
                    Valued = Vf.LevFormatUI,
                    IsDK = true,
                    Fieldkey = analiticalFields.Id,
                    IsInUnigroup = analiticalFields.Requared,
                    Group = analiticalFields.Group
                    
                };
                _saldoItems.Add(saldoItem);

               
            }
            foreach (AnaliticalFields analiticalFields in observableCollection)
            {
                Titles.Add(analiticalFields.Name);
                SaldoItemTypes saldotype = SaldoItemTypes.String;
                string defvalue = "";
                if (analiticalFields.FieldType == "integer")
                {
                    saldotype = SaldoItemTypes.Integer;
                    defvalue = "0";
                }
                if (analiticalFields.FieldType == "decimal")
                {
                    saldotype = SaldoItemTypes.Currency;
                    defvalue = Vf.LevFormatUI;
                }
                if (analiticalFields.FieldType == "Date")
                {
                    saldotype = SaldoItemTypes.Date;
                    defvalue = DateTime.Now.ToShortDateString();
                }
                SaldoItem saldoItem = new SaldoItem
                {
                    Name = analiticalFields.Name,
                    Type = saldotype,
                    Value = defvalue,
                    Fieldkey = analiticalFields.Id,
                    IsInUnigroup = analiticalFields.Requared,
                    Group = analiticalFields.Group
                };
                if (analiticalFields.IdLookUp != 0)
                {
                    saldoItem.IsLookUp = true;
                    saldoItem.Relookup = analiticalFields.IdLookUp;
                }
                //    LookupModel lm = Context.GetLookup(analiticalFields.IdLookUp);
                //    var list = Context.GetLookup(lm.LookUpMetaData.Tablename, ConfigTempoSinglenton.GetInstance().CurrentFirma.Id);
                //    int k = 0;
                //    foreach (IEnumerable<string> enumerable in list)
                //    {
                //        int i = 0;
                //        SaldoItem saldoitem = new SaldoItem();
                //        saldoitem.Name = saldoItem.Name;
                //        List<string> s = new List<string>(enumerable);
                //        saldoitem.Key = s[1];
                //        saldoitem.Value = s[2];
                //        saldoItem.LookUp.Add(saldoitem);

                //    }
                //}
                _saldoItems.Add(saldoItem);

            }
            foreach (AnaliticalFields analiticalFields in observableCollection_2)
            {
                Titles.Add(analiticalFields.Name);
                string defvalue = "";
                SaldoItemTypes saldotype = SaldoItemTypes.String;
                if (analiticalFields.FieldType == "integer")
                {
                    saldotype = SaldoItemTypes.Integer;
                    defvalue = "0";
                }
                if (analiticalFields.FieldType == "decimal")
                {
                    saldotype = SaldoItemTypes.Currency;
                    defvalue = Vf.LevFormatUI;
                }
                if (analiticalFields.FieldType == "Date")
                {
                    saldotype = SaldoItemTypes.Date;
                    defvalue = DateTime.Now.ToShortDateString();
                }
                SaldoItem saldoItem = new SaldoItem
                {
                    Name = analiticalFields.Name,
                    Type = saldotype,
                    Value = defvalue,
                    Fieldkey = analiticalFields.Id,
                    IsInUnigroup = analiticalFields.Requared,
                    Group = analiticalFields.Group
                };
                if (analiticalFields.IdLookUp != 0)
                {
                    saldoItem.IsLookUp = true;
                    saldoItem.Relookup = analiticalFields.IdLookUp;
                }
                //    LookupModel lm = Context.GetLookup(analiticalFields.IdLookUp);
                //    var list = Context.GetLookup(lm.LookUpMetaData.Tablename, ConfigTempoSinglenton.GetInstance().CurrentFirma.Id);
                //    foreach (IEnumerable<string> enumerable in list)
                //    {
                //        int i = 0;
                //        SaldoItem saldoitem = new SaldoItem();
                //        saldoitem.Name = saldoItem.Name;
                //        List<string> s = new List<string>(enumerable);
                //        saldoitem.Key = s[1];
                //        saldoitem.Value = s[2];
                //        saldoItem.LookUp.Add(saldoitem);

                //    }
                //}
                //if (!string.IsNullOrWhiteSpace(analiticalFields.RTABLENAME))
                //{
                    
                //    saldoItem.IsLookUp = true;
                //    saldoItem.RCODELOOKUP = analiticalFields.RCODELOOKUP;
                //    var list = Context.GetLookup(analiticalFields.RTABLENAME, ConfigTempoSinglenton.GetInstance().CurrentFirma.Id);
                //    int k= 0;
                //    foreach (IEnumerable<string> enumerable in list)
                //    {
                //        int i = 0;
                //        SaldoItem saldoitem = new SaldoItem();
                //        saldoitem.Name = saldoItem.Name;
                //        List<string> s = new List<string>(enumerable);
                //        saldoitem.Key = s[1];
                //        saldoitem.Value = s[2];
                //        saldoItem.LookUp.Add(saldoitem);

                //    }
                //}
               
                _saldoItems.Add(saldoItem);
            }
           
            GetValue(accountsModel);
        }

        public SaldosAnaliticViewModel(AccountsModel CurrentAccount)
        {
            IsCol = CurrentAccount.SaldoDK > 0 || CurrentAccount.SaldoKK > 0;
            IsVal = CurrentAccount.SaldoDV > 0 || CurrentAccount.SaldoKV > 0;
            this.IsShowReport = true;
            this.GetValue(CurrentAccount);
            this.IsShowNavigation = false;
        }

        private void GetValue(AccountsModel accountsModel)
        {
            Title = string.Format("Начални аналитични салда на сметка {0}", accountsModel);
            string sumalvk, sumalvd,sumalvksub, sumalvdsub;
            this.accountsModel = accountsModel;
            AllSaldoss = Context.GetAllMovementsDetails(accountsModel.Id,accountsModel.Num,accountsModel.FirmaId,out sumalvk, out sumalvd,out sumalvdsub,out sumalvksub);

            if (AllSaldoss != null && AllSaldoss.Count > 0)
            {
                var _reportItems = new List<ReportItem>();
                int k = 0;
                foreach (var item in AllSaldoss[0])
                {
                    if (k <= 1)
                    {
                        _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = item, Width = 30, IsSuma = true, Sborno = true });
                    }
                    else
                    {
                        _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = item, Width = 30 });
                    }
                    k++;
                }
                ReportItems = _reportItems;
                Titles = new ObservableCollection<string>(AllSaldoss[0]);
                AllSaldoss.Remove(AllSaldoss[0]);
                Filters = new ObservableCollection<Filter>();
                var i = 0;
                foreach (var item in Titles)
                {
                    var fil = new Filter();
                    fil.FilterName = item;
                    fil.FilterField = i.ToString();
                    Filters.Add(fil);
                    i++;
                }
            }
            else
            {
                var _reportItems = new List<ReportItem>();
                this.ReportItems = _reportItems;
            }
            AllSaldos = new List<List<string>>(AllSaldoss);
            SumaLvD = sumalvd;
            SumaLvK = sumalvk;
            string sumavd, sumavk, sumakd, sumakk;
            Context.GetAllMovementsSalosVK(accountsModel.Id, out sumalvk, out sumalvd, out sumavd, out sumavk, out sumakd, out sumakk);
            SumaKD = sumakd;
            SumaKK = sumakk;
            SumaVD = sumavd;
            SumaVK = sumavk;
            decimal test = 0;
            accountsModel.SubSaldoDL = decimal.TryParse(sumalvdsub,out test)?test:0;
            accountsModel.SubSaldoKL=decimal.TryParse(sumalvksub,out test)?test:0;
            Contents=new ObservableCollection<ObservableCollection<string>>();
            SetPager();
        }

        private string _sumaVD;
        public string SumaVD
        {
            get { return _sumaVD; }
            set
            {
                _sumaVD = value;
                OnPropertyChanged("SumaVD");
                
            }
        }

        private string _sumaVK;
        public string SumaVK
        {
            get { return _sumaVK; }
            set
            {
                _sumaVK = value;
                OnPropertyChanged("SumaVK");
                
            }
        }
        private string _sumaKD;
        public string SumaKD
        {
            get { return _sumaKD; }
            set
            {
                _sumaKD = value;
                OnPropertyChanged("SumaKD");
                
            }
        }

        private string _sumaKK;
        public string SumaKK
        {
            get { return _sumaKK; }
            set
            {
                _sumaKK = value;
                OnPropertyChanged("SumaKK");
               
            }
        }
        
        private bool _isVal;
        public bool IsVal
        {
            get { return _isVal; }
            set { _isVal = value; OnPropertyChanged("IsVal"); }
        }

        private bool _isCol;
        private const int pagesize=20;

        public bool IsCol
        {
            get { return _isCol; }
            set { _isCol = value; OnPropertyChanged("IsCol"); }
        }
        
        public string AccName
        {
            get { return accountsModel.ShortName; }
        }

        public List<List<string>> AllSaldos { get; private set; }
        public List<List<string>> AllSaldoss { get; private set; }
        public ObservableCollection<Filter> Filters { get; private set; }

        public string Filename
        {
            get { return "SaldosByClients"; }
        }

        

        public IEnumerable<ReportItem> ReportItems
        {
            get;set;
        }

        protected override void Update()
        {
            if (CurrentRowIndex != -1)
            {
                ObservableCollection<string> currow = Contents[CurrentRowIndex+1];
                EditSaldo window = new EditSaldo(accountsModel, int.Parse(currow[currow.Count - 1]), (int)accountsModel.TypeAnaliticalKey);
                window.ShowDialog();
                if (window.DialogResult.HasValue && window.DialogResult.Value)
                {
                    GetValue(accountsModel);
                }
            }
        }
        protected override void Add()
        {

            EditorAddSaldo window = new EditorAddSaldo(accountsModel, SaldoItems);
            window.ShowDialog();
            if (window.DialogResult.HasValue && window.DialogResult.Value)
            {
                GetValue(accountsModel);
            }
        }

        protected override void Delete()
        {
           if (CurrentRowIndex != -1 && Contents.Count>0)
           {
               if (MessageBoxWrapper.Show("Сигурни ли сте, че желаете да изтриете този запис?", "Предупреждение", MessageBoxWrapperButton.YesNo) == MessageBoxWrapperResult.Yes)
               {
                
                    ObservableCollection<string> currow = Contents[CurrentRowIndex+1];
                    int selectedgroup = int.Parse(currow[currow.Count - 1]);
                    Context.DeleteMovement(accountsModel.Id, selectedgroup);
                    Context.UpdateMovement(new SaldoAnaliticModel { ACCID = accountsModel.Id });
                    GetValue(accountsModel);
               }

            }
            
        }
        private DateTime fromDate;
        public DateTime FromDate
        {
            get
            {
                return fromDate;
            }

            set
            {
                fromDate = value;
            }
        }

        private DateTime toDate;
        public DateTime ToDate
        {
            get
            {
                return toDate;
            }

            set
            {
                toDate = value;
            }
        }
        public List<List<string>> GetItems()
        {
            return AllSaldos;
        }

        public List<string> GetTitles()
        {
            return ReportItems.Select(reportItem => reportItem.Name).ToList();
        }

        public List<string> GetHeader()
        {
            return null;
        }

        public List<string> GetFuther()
        {
            return null;
        }

        public List<string> GetSubTitles()
        {
            return null;
        }

        public List<List<string>> GetTXTAntetka()
        {
            return null;
        }
    }

}
