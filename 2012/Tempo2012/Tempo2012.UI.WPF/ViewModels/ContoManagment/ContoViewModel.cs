using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Windows;
using System.Windows.Annotations;
using System.Windows.Input;
using WindowsInput;
using ReportBuilder;
using Tempo2012.EntityFramework;
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.Dialogs;
using Tempo2012.UI.WPF.Models;
using Tempo2012.UI.WPF.PaggingControlProject;
using Tempo2012.UI.WPF.ViewModels.Dnevnici;
using Tempo2012.UI.WPF.Views.Dnevnici;
using Tempo2012.UI.WPF.Views.ReportManager;
using Tempo2012.UI.WPF.Views.TetkaView;
using Tempo2012.UI.WPF.Views.Valuta;
using Tempo2012.EntityFramework.Interface;
using System.Threading;
using Microsoft.Win32;

namespace Tempo2012.UI.WPF.ViewModels.ContoManagment
{
    public delegate void SinkEvent(object sender, SinkEventArgs e);
    public partial class ContoViewModel : BaseViewModel, IDataErrorInfo, IReportBuilder, ISerchable, IItemsProvider<WraperConto> 
    {
        public Dictionary<int, List<string>> Rowfoother { get; set; }
        public int _numberOfRecords = 5;
        //private BackgroundWorker bw = new BackgroundWorker();
        private int _currentItem;
        public event SinkEvent SinkInfo;
        protected virtual void OnSinkExecuted(SinkEventArgs e)
        {
            if (SinkInfo != null)
            {
                SinkInfo(this, e);
            }
        }
        private string _LasdtFive;

        public bool _enableGrid;
        public bool EnableGrid
        {
            get { return _enableGrid;}
            set { _enableGrid = value; OnPropertyChanged("EnableGrid");}
        }
        public string LastFiveTitle
        {
            get
            { 
                return  _LasdtFive;
            }
            set
            {
                _LasdtFive = value;
                OnPropertyChanged("LastFiveTitle");
            }
        }
        public ContoViewModel():base()
        {
            _numberOfRecords = Entrence.InfoCount;
            ShowContoAll = Visibility.Hidden;
            ContoAll = 0;
            KindDocLookup = new ObservableCollection<LookUpSpecific>(Context.GetAllDocTypes());
            
            this.MoveNextPageCommand = new DelegateCommand((o) => this.MoveNextPage());
            this.MovePreviusPageCommand = new DelegateCommand((o) => this.MovePreviusPage());
            this.MoveLastPageCommand = new DelegateCommand((o) => this.MoveLastPage());
            this.MoveFirstPageCommand = new DelegateCommand((o) => this.MoveFirstPage());
            this.SumaDdsCommand=new DelegateCommand((o)=>this.SumaDdsAdd());
            _isDdsVisible = ConfigTempoSinglenton.GetInstance().CurrentFirma.RegisterDds;
            FirmaId = ConfigTempoSinglenton.GetInstance().CurrentFirma.Id;
            _dAccountsModel=new AccountsModel();
            _cAccountsModel=new AccountsModel();
            TypeDocuments = new ObservableCollection<LookUpSpecific>(Context.GetAllDocTypes());
            ItemsDdsDnevPurchases = new ObservableCollection<DdsItemModel>(Context.GetAllDnevItems(1));
            ItemsDdsDnevSales     = new ObservableCollection<DdsItemModel>(Context.GetAllDnevItems(0));
            VopsPurchases=new ObservableCollection<string>();
            VopsSales=new ObservableCollection<string>(); 
            foreach (DdsItemModel itemsDdsDnevPurchase in ItemsDdsDnevPurchases)
            {
                VopsPurchases.Add(itemsDdsDnevPurchase.Code);
            }
            foreach (DdsItemModel itemsDdsDnevSales in ItemsDdsDnevSales)
            {
                VopsSales.Add(itemsDdsDnevSales.Code);
            }
            AllAccountsK = new ObservableCollection<AccountsModel>(Context.GetAllAccounts(FirmaId));
            Osnovanie = new ObservableCollection<string>(Context.GetLookupByName("osnovanie", "Name"));
            Notes = new ObservableCollection<string>(Context.GetLookupByName("zabel", "Name"));
            
            this.SellsCommand = new DelegateCommand((o) => this.Sells());
            this.PurchaseCommand = new DelegateCommand((o) => this.Purchase());
            var reportItems = new List<ReportItem>();
            reportItems.Add(new ReportItem {Height = 30, IsShow = true, Name = "Запис",Width = 20});
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Номер Документ",Width = 20});
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Дебит", Width = 20 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Кредит", Width = 20 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Дата осчетоводяване", Width = 20 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Оборот", Width = 20,IsSuma=true});
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Оборот валута Кт", Width = 20, IsSuma = true });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Оборот валута Дт", Width = 20, IsSuma = true });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Оборот количество Кт", Width = 20, IsSuma = true });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Оборот количество Дт", Width = 20, IsSuma = true });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Папка", Width = 20 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Основание", Width = 20 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Забележка", Width = 20 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Дата на документ", Width = 20 });
            ReportItems = reportItems;
            IsShowNavigation = true;
            LoadLastRecordsAsynk();
            InputSimulator.SimulateKeyPress(VirtualKeyCode.F2);
            _isCurrentUser = true;
            LastFiveTitle = String.Format("Последни {0} записа",Entrence.InfoCount);


        }
        public void LoadSettings(string Path)
        {

            ReportItems = SerializeUtil.DeserializeFromXML<List<ReportItem>>(Path);

        }

        public void SaveSettings(string Path)
        {

            SerializeUtil.SerializeToXML<List<ReportItem>>(Path, ReportItems);

        }
        private void LoadLastRecordsAsynk()
        {
            EnableGrid = false;
            Visible = Visibility.Visible;
            var bw = new BackgroundWorker { WorkerReportsProgress = false, WorkerSupportsCancellation = true };
            bw.DoWork += new DoWorkEventHandler(bw_LoadLastRecordsAsynkDo);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_LoadLastRecordsAsynCompleted);
            bw.RunWorkerAsync();
        }

        private void bw_LoadLastRecordsAsynCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (AllWrapedConto != null && AllWrapedConto.Count > 0)
            {
                if (CurrentWraperConto==null) AllWrapedConto.Last();
            }
            Visible = Visibility.Hidden;
            if (Mode == EditMode.View || Mode == EditMode.Edit)
            {
                EnableGrid = true;
            }
        }

        private void bw_LoadLastRecordsAsynkDo(object sender, DoWorkEventArgs e)
        {
            Total = FetchCount();
            Count= FetchCountCurrUser();
            if (Mode == EditMode.Add)
            {
                CurrentWraperConto.CurrentConto.DocumentId = Total+1;
                CurrentWraperConto.CurrentConto.Nd = Total+1;
                CurrentWraperConto.CurrentConto.CartotecaCredit = Total+1;
                OnPropertyChanged("Index");
            }
            int i = Count > 0 ? 1 : 0;
            int pages = (Count - i) / _numberOfRecords;
            _currentPage = 1;
            AllPages = pages + 1;
            FromToPages = string.Format("{0} от {1}", CurrentPage, AllPages);
            if (!ShowLastFive) return;
            
                if (Count < _numberOfRecords)
                {
                    FetchRange(0,Count);
                }
                else
                {
                    FetchRange(0,_numberOfRecords);
                }
            
        }

        private void GoToLastRecord(bool getlast = true)
        {
            Count = FetchCountCurrUser();
            //Total = Count;
            
            int i = Count > 0 ? 1 : 0;
            int pages = (Count - i) / _numberOfRecords;
            _currentPage = pages + 1;
            AllPages = pages + 1;
            FromToPages = string.Format("{0} от {1}", CurrentPage, AllPages);
            //FetchRange(0, _numberOfRecords);
            if (!ShowLastFive) return;
            if (Count > _numberOfRecords)
            {
                GetLast10Conto(Count - _numberOfRecords, _numberOfRecords, null, false, getlast);
            }
            else
            {
                GetLast10Conto(0, _numberOfRecords, null, false, getlast);
            }

        }
        private void ReloadRecords()
        {
            EnableGrid = false;
            Visible = Visibility.Visible;
            var bw = new BackgroundWorker { WorkerReportsProgress = false, WorkerSupportsCancellation = true };
            bw.DoWork += new DoWorkEventHandler(ReloadLastFive);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
            bw.RunWorkerAsync();
            
        }
        private void SumaDdsAdd()
        {
            
            if (Mode == EditMode.Add)
            {
                WorkValuta = null;
                ShowContoAll = Visibility.Visible;
                ContoAll += CurrentWraperConto.CurrentConto.Oborot;
      

            }
        }
        
        private void MoveFirstPage()
        {
            if (CurrentPage == 1) return;
            CurrentPage = 1;
        }

        private void MoveLastPage()
        {
            if (CurrentPage == AllPages) return;
            CurrentPage = AllPages;
        }

        private void MoveNextPage()
        {
            first = true;
            if (CurrentPage==AllPages)
            {
                CurrentPage = 1;
            }
            else
            {
                CurrentPage += 1;
            }            
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
        }

        private string _fromToPages;
        public string FromToPages
        {
            get { return _fromToPages; }
            set { _fromToPages = value; OnPropertyChanged("FromToPages"); }
        }

        
        private string _progresInfo;
        public string ProgresInfo
        {
            get { return _progresInfo; }
            set { _progresInfo = value; OnPropertyChanged("ProgresInfo"); }
        }
 
        protected List<WraperConto> WrapedConto { get; set;}

        private void RefreshConto(ObservableCollection<Conto> AllConto)
        {
            AllWrapedConto=new ObservableCollection<WraperConto>();
            foreach (Conto conto in AllConto)
            {
                var firstOrDefault = AllAccountsK.FirstOrDefault(e => e.Id == conto.DebitAccount);
                if (firstOrDefault != null)
                    conto.DName = firstOrDefault.Short;
                var cfirstOrDefault = AllAccountsK.FirstOrDefault(e => e.Id == conto.CreditAccount);
                if (cfirstOrDefault != null)
                    conto.CName = cfirstOrDefault.Short;
                
                AllWrapedConto.Add(new WraperConto(conto));
                
            }
            OnPropertyChanged("AllWrapedConto");
           
        }

        
        
        private AccountsModel _dAccountsModel;
        public AccountsModel DAccountsModel
        {
            get { return _dAccountsModel; }
            set
            {
                if (_dAccountsModel != null && (value != null && _dAccountsModel.Id == value.Id)) return;
                _dAccountsModel = value;
                if (_dAccountsModel != null) if (value != null) _dAccountsModel.Search = value.Short;
                OnPropertyChanged("DAccountsModel");
                if (value != null)
                {
                    if (CurrentWraperConto.CurrentConto != null)
                    {
                        CurrentWraperConto.CurrentConto.DName = value.Short;
                        CurrentWraperConto.CurrentConto.OborotKolD = 0;
                        CurrentWraperConto.CurrentConto.OborotValutaD = 0;
                    }
                    var list = Context.LoadAllAnaliticfields(value.Id);
                    DebitAccount = value.Id;
                    IsKol = false;
                    IsValutna = false;
                    foreach (SaldoAnaliticModel saldoAnaliticModel in list)
                    {
                        if (saldoAnaliticModel.IsKol) IsKol = true;
                        if (saldoAnaliticModel.IsValutna) IsValutna = true;
                   }
                   ItemsDebit = new ObservableCollection<SaldoItem>(LoadCreditAnaliticAtributes(list.ToList(),0));
                   foreach (SaldoItem saldoItem in ItemsDebit)
                   {
                       if (saldoItem.Type == SaldoItemTypes.Date)
                       {
                           saldoItem.Value = CurrentWraperConto.Data;
                       }
                   }
                   
                }
              
            }
        }

        private AccountsModel _cAccountsModel;
        public AccountsModel CAccountsModel
        {
            get { return _cAccountsModel; }
            set
            {
                if (_cAccountsModel != null && (value != null && _cAccountsModel.Id == value.Id)) return;
                _cAccountsModel = value;
                if (_cAccountsModel != null) if (value != null) _cAccountsModel.Search = value.Short;
                OnPropertyChanged("CAccountsModel");
                if (value != null)
                {

                    var list = Context.LoadAllAnaliticfields(value.Id);
                    IsKol = false;
                    IsValutna = false;
                    if (CurrentWraperConto.CurrentConto != null)
                    {
                        CurrentWraperConto.CurrentConto.CName = value.Short;
                        CurrentWraperConto.CurrentConto.OborotKolK = 0;
                        CurrentWraperConto.CurrentConto.OborotValutaK = 0;
                    }
                    foreach (SaldoAnaliticModel saldoAnaliticModel in list)
                    {
                        if (saldoAnaliticModel.IsKol) IsKol = true;
                        if (saldoAnaliticModel.IsValutna) IsValutna = true;
                    }
                    CreditAccount = value.Id;

                    ItemsCredit = new ObservableCollection<SaldoItem>(LoadCreditAnaliticAtributes(list.ToList(),2));
                    foreach (SaldoItem saldoItem in ItemsCredit)
                    {
                        if (saldoItem.Type == SaldoItemTypes.Date)
                        {
                            saldoItem.Value = CurrentWraperConto.Data;
                        }
                    }
                }
              
            }
        }

        private bool _isDdsVisible;
        public bool IsDdsVisible
        {
            get { return _isDdsVisible; }
            set
            {
                _isDdsVisible = value;
                OnPropertyChanged("IsDdsVisible");
            }
        }
        
        public bool IsDdsInclude
        {
            get { return IsDdsIncludePurchases || IsDdsIncludeSales;}
            set
            {
                IsDdsIncludeSales = value;
                IsDdsIncludePurchases = value;
                OnPropertyChanged("IsDdsInclude");
            }
        }
        
        public bool IsDds
        {
            get { return IsDdsSales||IsDdsPurchases; }
            set
            {
                IsDdsPurchases = value;
                IsDdsSales = value;
                OnPropertyChanged("IsDds");
            }
        }

        public bool IsDdsPurchases
        {
            get
            {
                if (CurrentWraperConto != null && CurrentWraperConto.CurrentConto != null) return CurrentWraperConto.CurrentConto.IsDdsPurchases==1;
                  return false;
            }
            set
            {
                if (CurrentWraperConto != null && CurrentWraperConto.CurrentConto != null) CurrentWraperConto.CurrentConto.IsDdsPurchases =value?1:0;
                OnPropertyChanged("IsDdsPurchases");
            }
        }

        public bool IsDdsSales
        {
            get
            {
                if (CurrentWraperConto != null && CurrentWraperConto.CurrentConto != null) return CurrentWraperConto.CurrentConto.IsDdsSales==1;
                  return false;
            }
            set
            {
                if (CurrentWraperConto != null && CurrentWraperConto.CurrentConto != null) CurrentWraperConto.CurrentConto.IsDdsSales =value?1:0;
                OnPropertyChanged("IsDdsSales");
            }
        }

        private bool _isCurrentUser;
        public bool IsCurrentUser
        {
            get { return _isCurrentUser; }
            set {
                _isCurrentUser = value;
                Count = value ? Context.GetAllContoCount(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id, CreateAcc()):Context.GetAllContoCount(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id, ConfigTempoSinglenton.GetInstance().WorkDate.Year, ConfigTempoSinglenton.GetInstance().WorkDate.Month);
                OnPropertyChanged("IsCurrentUser");
            }
        }
        private byte _codeDnev;
        public byte CodeDnev
        {
            get { return _codeDnev; }
            set
            {
                _codeDnev = value; OnPropertyChanged("CodeDnev");
            }
        }
        public ObservableCollection<DdsItemModel> ItemsDdsDnevPurchases { get; set; }
        public ObservableCollection<DdsItemModel> ItemsDdsDnevSales { get; set; }
        private ObservableCollection<SaldoItem> _itemsdebit;
        public ObservableCollection<SaldoItem> ItemsDebit
        {
            get { return _itemsdebit; }
            set
            {
                _itemsdebit = value;
                OnPropertyChanged("ItemsDebit");
            }
        }
        private ObservableCollection<SaldoItem> _itemscredit;
        public ObservableCollection<SaldoItem> ItemsCredit
        {
          get
          {
            return _itemscredit;
          }
          set
          {
           _itemscredit = value;
            OnPropertyChanged("ItemsCredit");
          }
        }
        private long typeAnaliticalKey;
        public ObservableCollection<string> VopsPurchases { get; set; }
        public ObservableCollection<string> VopsSales { get; set; }
        public string VopSales
        {
            get { if (CurrentWraperConto != null && CurrentWraperConto.VopSales != null) return CurrentWraperConto.VopSales;
                return null;
            }
            set
            {
                if (CurrentWraperConto != null && (CurrentWraperConto.VopSales != null && CurrentWraperConto.VopSales == value)) return;
                if (CurrentWraperConto != null && CurrentWraperConto.VopSales != null) CurrentWraperConto.VopSales = value;
                var curitem =ItemsDdsDnevSales.FirstOrDefault(e => value != null && e.Code == value.ToUpper());
                if (curitem!=null)
                {
                    CurrItemDdsDnevSales= curitem;
                }
                //OnPropertyChanged("VopSales");
            }
        }
        private DdsItemModel _currItemDdsDnevSales;
        public DdsItemModel CurrItemDdsDnevSales
        {
            get { return _currItemDdsDnevSales;}
            set
            {
                _currItemDdsDnevSales = value;
                if (value != null && value.Code != null)
                {
                    if (CurrentWraperConto != null) CurrentWraperConto.VopSales = value.Code.ToUpper();
                    OnPropertyChanged("VopSales");
                    if (value.DdsPercent > 0)
                    {
                        IsDdsInclude = false;
                        IsDds = true;
                    }
                    else
                    {
                        IsDdsInclude = false;
                        IsDds = false;
                    }
                    IsSales = true;
                   
                }
                else
                {
                    if (CurrentWraperConto != null) CurrentWraperConto.VopSales = "";
                    OnPropertyChanged("VopSales");
                   
                }
                OnPropertyChanged("CurrItemDdsDnevSales");

            }
        }

        
        public string VopPurchases
        {
            get { if (CurrentWraperConto != null) return CurrentWraperConto.VopPurchases;
                return null;
            }
            set
            {
                if (CurrentWraperConto != null && CurrentWraperConto.VopPurchases == value) return;
                if (CurrentWraperConto != null) CurrentWraperConto.VopPurchases = value;
                var curitem = ItemsDdsDnevPurchases.FirstOrDefault(e => value != null && e.Code == value.ToUpper());
                if (curitem != null)
                {
                    CurrItemDdsDnevPurchases = curitem;
                }
                //OnPropertyChanged("VopPurchases");
            }
        }
        private DdsItemModel _currItemDdsDnevPurchases;
        public DdsItemModel CurrItemDdsDnevPurchases
        {
            get { return _currItemDdsDnevPurchases; }
            set
            {
                _currItemDdsDnevPurchases = value;
                if (value != null && value.Code != null)

                {
                    if (CurrentWraperConto != null)
                    {
                        CurrentWraperConto.VopPurchases = value.Code.ToUpper();
                        OnPropertyChanged("VopPurchases");
                        if (value.DdsPercent > 0)
                        {
                            IsDdsInclude = false;
                            IsDds = true;
                        }
                        else
                        {
                            IsDdsInclude = false;
                            IsDds = false;
                        }
                        IsPurchases = true;
                    }
                }
                else
                    {
                        if (CurrentWraperConto != null) CurrentWraperConto.VopPurchases = "";
                        OnPropertyChanged("VopPurchases");
                    }
                OnPropertyChanged("CurrItemDdsDnevPurchases");
            }
        }
        public DateTime DataInvoise
        {
            get
            {
                if (CurrentWraperConto != null && (CurrentWraperConto.CurrentConto != null && CurrentWraperConto.CurrentConto.DataInvoise.Year != 1))
                {
                    return CurrentWraperConto.CurrentConto.DataInvoise;
                }
                if (CurrentWraperConto != null && CurrentWraperConto.CurrentConto != null)
                    CurrentWraperConto.CurrentConto.DataInvoise = ConfigTempoSinglenton.GetInstance().WorkDate;
                return DateTime.Now;
            }
            set
            {
                if (value.Year != ConfigTempoSinglenton.GetInstance().WorkDate.Year)
                {
                    MessageBoxWrapper.Show(
                        "Годината на дата не е равна на датата от работната дата! Изберете дата с година равна на работната дата или променете работната дата",
                        "Предупреждение");
                    OnPropertyChanged("DataInvoise");
                    return;
                }
                if (CurrentWraperConto != null)
                    if (CurrentWraperConto.CurrentConto != null) CurrentWraperConto.CurrentConto.DataInvoise = value;
                OnPropertyChanged("DataInvoise");
            }
        }
        public string DocId
        {
            get { if (CurrentWraperConto != null && CurrentWraperConto.CurrentConto != null) return CurrentWraperConto.CurrentConto.DocNum;
                return "";
            }
            set
            {
                if (CurrentWraperConto != null && CurrentWraperConto.CurrentConto != null) CurrentWraperConto.CurrentConto.DocNum = value;
                OnPropertyChanged("DocId");
                OnPropertyChanged("AllAccounts");
            }
        }
        public string Folder
        {
            get { if (CurrentWraperConto != null && CurrentWraperConto.CurrentConto != null) return CurrentWraperConto.CurrentConto.Folder;
                return "";
            }
            set
            {
                if (CurrentWraperConto.CurrentConto != null) CurrentWraperConto.CurrentConto.Folder = value;
                OnPropertyChanged("Folder");
                OnPropertyChanged("AllAccounts");
            }
        }
        public string Pr1
        {
            get
            {
                if (CurrentWraperConto != null && CurrentWraperConto.CurrentConto != null) return CurrentWraperConto.CurrentConto.Pr1;
                return "";
            }
            set
            {
                if (CurrentWraperConto.CurrentConto != null) CurrentWraperConto.CurrentConto.Pr1 = value;
                OnPropertyChanged("Pr1");
               
            }
        }
        public string Pr2
        {
            get
            {
                if (CurrentWraperConto != null && CurrentWraperConto.CurrentConto != null) return CurrentWraperConto.CurrentConto.Pr2;
                return "";
            }
            set
            {
                if (CurrentWraperConto.CurrentConto != null) CurrentWraperConto.CurrentConto.Pr2 = value;
                OnPropertyChanged("Pr2");

            }
        }
        public string Kd
        {
            get
            {
                if (CurrentWraperConto != null && CurrentWraperConto.CurrentConto != null) return CurrentWraperConto.CurrentConto.KD;
                return "";
            }
            set
            {
                if (Kd != value)
                {
                    OborotChange = true;
                }
                if (CurrentWraperConto.CurrentConto != null) CurrentWraperConto.CurrentConto.KD = value;
                OnPropertyChanged("Kd");

            }
        }
        public string Reason
        {
            get { if (CurrentWraperConto != null && CurrentWraperConto.CurrentConto != null) return CurrentWraperConto.CurrentConto.Reason;
                return " ";
            }
            set
            {
                if (CurrentWraperConto != null && CurrentWraperConto.CurrentConto != null)
                {
                    OborotChange =  CurrentWraperConto.CurrentConto.Reason != value;
                    CurrentWraperConto.CurrentConto.Reason = value;
                }
                OnPropertyChanged("Reason");
            }
        }
        public string Note
        {
            get { if (CurrentWraperConto != null && CurrentWraperConto.CurrentConto != null) return CurrentWraperConto.CurrentConto.Note;
                return " ";
            }
            set
            {
                if (CurrentWraperConto != null && CurrentWraperConto.CurrentConto != null) CurrentWraperConto.CurrentConto.Note = value;
                OnPropertyChanged("Note");
            }
        }
        public DateTime MinDate
        {
            get { return new DateTime(ConfigTempoSinglenton.GetInstance().WorkDate.Year,1,1); }
            
        }
        public DateTime MaxDate
        {
            get { return new DateTime(ConfigTempoSinglenton.GetInstance().WorkDate.Year, 1, 1).AddYears(1).AddDays(-1);}

        }
        public DateTime Data
        {
            get
            {
                if (CurrentWraperConto != null && CurrentWraperConto.CurrentConto != null)
                {
                    return CurrentWraperConto.CurrentConto.Data;
                }
                return DateTime.Now;
            }
            set
            {
                if (CurrentWraperConto != null && (CurrentWraperConto.CurrentConto != null && value.Year == ConfigTempoSinglenton.GetInstance().WorkDate.Year))
                {
                    ConfigTempoSinglenton currentconfig = ConfigTempoSinglenton.GetInstance();
                    if (currentconfig.Periods.FirstOrDefault(e1 => e1.Fr <= value && e1.To >= value && e1.Firma == currentconfig.ActiveFirma && e1.Holding == currentconfig.ActiveHolding) != null)
                    {
                        MessageBoxWrapper.Show("Датата попада в забранен период. Моля изберете коректна дата!", "Предупреждение");
                        OnPropertyChanged("DataInvoise");
                        OnPropertyChanged("Data");
                    }
                    else
                    {
                        CurrentWraperConto.CurrentConto.Data = value;
                        CurrentWraperConto.CurrentConto.DataInvoise = value;
                        OnPropertyChanged("DataInvoise");
                        OnPropertyChanged("Data");
                        OnOborotChange();
                        if (!string.IsNullOrEmpty(WorkValuta))
                        {
                            UpdateValutenKurs(WorkValuta);
                        }
                        UpdateData();
                    }
                }
                else
                {
                    MessageBoxWrapper.Show("Годината на дата не е равна на датата от работната дата! Изберете дата с година равна на работната дата или променете работната дата", "Предупреждение");
                    OnPropertyChanged("DataInvoise");
                    OnPropertyChanged("Data");
                }


            }
        }

        internal void ClearContoAll()
        {
            ShowContoAll = Visibility.Hidden;
            ContoAll = 0;
        }

        public string WorkValuta { get; set;}
        public int NumberObject
        {
            get { if (CurrentWraperConto != null && CurrentWraperConto.CurrentConto != null) return CurrentWraperConto.CurrentConto.NumberObject;
                return 1;
            }
            set
            {
                if (CurrentWraperConto != null && CurrentWraperConto.CurrentConto != null) CurrentWraperConto.CurrentConto.NumberObject = value;
                OnPropertyChanged("NumberObject");
            }
        }
        //connect data
        public int DebitAccount
        {
            get { if (CurrentWraperConto != null && CurrentWraperConto.CurrentConto != null) return CurrentWraperConto.CurrentConto.DebitAccount;
                return 0;
            }
            set
            {
                if (CurrentWraperConto != null && CurrentWraperConto.CurrentConto != null) CurrentWraperConto.CurrentConto.DebitAccount = value;
                OnPropertyChanged("DebitAccount");
            }
        }

        public int CreditAccount
        {
            get { if (CurrentWraperConto != null && CurrentWraperConto.CurrentConto != null) return CurrentWraperConto.CurrentConto.CreditAccount;
                return 0;
            }
            set
            {
                if (CurrentWraperConto != null && CurrentWraperConto.CurrentConto != null) CurrentWraperConto.CurrentConto.CreditAccount = value;
                OnPropertyChanged("CreditAccount");
            }
        }
        
        public decimal Oborot
        {
            get { if (CurrentWraperConto != null && CurrentWraperConto.CurrentConto != null) return CurrentWraperConto.CurrentConto.Oborot;
                  return 0;
            }
            set
            {
                if (CurrentWraperConto != null && CurrentWraperConto.CurrentConto != null)
                {
                    bool obchange = CurrentWraperConto.CurrentConto.Oborot != value;
                    CurrentWraperConto.CurrentConto.Oborot =value;
                    if (!notupdated)
                    {
                        UpdateCol();
                        if (obchange) OnOborotChange();
                    }
                }
                OnPropertyChanged("Oborot");
            }
        }

        private void OnOborotChange()
        {
            OborotChange = true;
        }

        public bool OborotChange { get; set; }

        private void UpdateValuta()
        {
            if (Oborot == 0 || notupdated) return;
            foreach (var saldoItem in ItemsCredit)
            {
                if (saldoItem.IsVal && saldoItem.ValueVal != 0)
                {
                    saldoItem.ValueKurs = Oborot / saldoItem.ValueVal;
                }
            }
            foreach (var saldoItem in ItemsDebit)
            {
                if (saldoItem.IsVal && saldoItem.ValueVal != 0)
                {
                    saldoItem.ValueKurs = Oborot / saldoItem.ValueVal;
                }
            }
        }

        private void UpdateData()
        {
            if (Mode == EditMode.Edit) return;
            foreach (var saldoItem in ItemsCredit)
            {
                if (saldoItem.Name.Contains("Дата"))
                {
                    saldoItem.ValueDate = Data;
                }
            }
            foreach (var saldoItem in ItemsDebit)
            {
                if (saldoItem.Name.Contains("Дата"))
                {
                    saldoItem.ValueDate = Data;
                }
            }
        }
        private bool _isvalutna;
        public bool IsValutna
        {
            get { return _isvalutna; }
            set
            {
                _isvalutna = value;
                OnPropertyChanged("IsValutna");
            }
        }

        private bool _isKol;
        public bool IsKol
        {
            get { return _isKol; }
            set
            {
                _isKol = value;
                OnPropertyChanged("IsKol");
            }
        }

        public int RecordId
        {
            get { if (CurrentWraperConto != null && CurrentWraperConto.CurrentConto != null) return CurrentWraperConto.CurrentConto.Id;
                return 0;
            }
            set
            {
                if (CurrentWraperConto != null && CurrentWraperConto.CurrentConto != null) CurrentWraperConto.CurrentConto.Id = value;
                OnPropertyChanged("RecordId");
            }
        }

        public int Id
        {
            get
            {
                if (CurrentWraperConto != null && CurrentWraperConto.CurrentConto != null) return CurrentWraperConto.CurrentConto.Id;
                return 0;
            }
            set
            {
                if (CurrentWraperConto != null && CurrentWraperConto.CurrentConto != null) CurrentWraperConto.CurrentConto.Id = value;
                OnPropertyChanged("Id");
            }
        }
        public bool IsDdsIncludePurchases
        {
            get { if (CurrentWraperConto != null && CurrentWraperConto.CurrentConto != null) return CurrentWraperConto.CurrentConto.IsDdsPurchasesIncluded==1;
                  return false;
            }
            set
            {
                if (CurrentWraperConto != null && CurrentWraperConto.CurrentConto != null) CurrentWraperConto.CurrentConto.IsDdsPurchasesIncluded = value?1:0;
               OnPropertyChanged("IsDdsIncludePurchases");
            }
        }
        public bool IsDdsIncludeSales
        {
            get { if (CurrentWraperConto != null && CurrentWraperConto.CurrentConto != null) return CurrentWraperConto.CurrentConto.IsDdsSalesIncluded==1;
                  return false;
            }
            set
            {
                if (CurrentWraperConto != null && CurrentWraperConto.CurrentConto != null) CurrentWraperConto.CurrentConto.IsDdsSalesIncluded =value?1:0;
                OnPropertyChanged("IsDdsIncludeSales");
            }
        }
        
        private IEnumerable<SaldoItem> LoadDetails(int contoid, int typeconto)
        {
            if (contoid==0) return new List<SaldoItem>();
            var items = ContoRepository.Instance.ContoItems(string.Format("{0}-{1}", contoid, typeconto));
            if (items!=null)
            return items;
            var itemsforcache=LoadCreditAnaliticAtributes(Context.LoadContoDetails(contoid, typeconto),typeconto);
            ContoRepository.Instance.Add(string.Format("{0}-{1}", contoid, typeconto), itemsforcache.ToList());
           
            return itemsforcache;
        }
        public ObservableCollection<LookUpSpecific> TypeDocuments { get; set; }
        protected override void Update()
        {
            addsecond = false;
            base.Update();
            OnSetFocusExecuted(new SetFocusEventArg("DDS"));
        }

        protected override void Save()
        {
            WorkValuta = null;
            if (Mode == EditMode.Add)
            {
                if (SaveConto())
                {
                    //Count = FetchCount();
                    //Total = Count;
                    //int i = Count > 0 ? 1 : 0;
                    //int pages = (Count - i) / _numberOfRecords;
                    //_currentPage = pages + 1;
                    //AllPages = pages + 1;
                    //FromToPages = string.Format("{0} от {1}", CurrentPage, AllPages);
                    //DontChange = true;
                    //if (Count > _numberOfRecords)
                    //{
                    //    GetLast10Conto(Count - _numberOfRecords, _numberOfRecords);
                    //}
                    //else
                    //{
                    //    GetLast10Conto(0, _numberOfRecords, CurrentWraperConto);
                    //}
                    ReloadRecords();
                    DontChange = false;
                    Second();
                    Add();
                    OnSetFocusExecuted(string.IsNullOrWhiteSpace(Kd)
                        ? new SetFocusEventArg("DDS")
                        : new SetFocusEventArg("Ob"));
                    
                }
                
            }
            else
            {
                if (Mode == EditMode.Edit)
                {
                    addsecond = false;
                    ShowLastFive = false;
                    UpdateConto();
                    //CurrentWraperConto.CurrentConto = CurrentWraperConto.CurrentConto;
                    //OnPropertyChanged("CurrentWraperConto");
                    //var bw = new BackgroundWorker { WorkerReportsProgress = false, WorkerSupportsCancellation = true };
                    //bw.DoWork += new DoWorkEventHandler(GetLast10Async);
                    //bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_GetLast10AsynCompleted);
                    //bw.RunWorkerAsync();
                    
                 }
            }
           

        }

        private void bw_GetLast10AsynCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((Count > 0) && (AllWrapedConto.Count > 0))
            {
               
                    try
                    {
                          CurrentWraperConto = CurrentWraperConto ?? AllWrapedConto.Last();
                    }
                    catch (Exception e1)
                    {
                        Logger.Instance().WriteLogError(e1.Message, "private void bw_GetLast10AsynCompleted(object sender, RunWorkerCompletedEventArgs e)");
                    }
                

            }
            else
            {
                First();
                Add();
                _currentItem = 0;
            }
        }

        private void GetLast10Async(object sender, DoWorkEventArgs e)
        {
            GetLast10Conto((CurrentPage - 1) * _numberOfRecords, _numberOfRecords, CurrentWraperConto);
        }

        protected override void Delete()
        {
            addsecond = false;
            WorkValuta = null;
            
            if (MessageBoxWrapper.Show("Сигурни ли сте, че желаете да изтриете този запис?", "Предупреждение", MessageBoxWrapperButton.YesNo) == MessageBoxWrapperResult.Yes)
            {
                ShowLastFive=false;
                if (Context.DeleteConto(CurrentWraperConto.CurrentConto.Id))
                {
                    Count--;
                    Total--;
                    //GetLast10Conto((CurrentPage - 1) * _numberOfRecords, _numberOfRecords, null, true);
                    var id = CurrentWraperConto.Id;
                    MoveNext();
                    if (id == CurrentWraperConto.Id)
                    {
                        MovePrevius();
                    }
                    MessageBoxWrapper.Show("Записът е изтрит");
                }
                else
                {
                    MessageBoxWrapper.Show("Грешка при триене");
                }
            }
           
        }
        private void UpdateConto()
        {
            WorkValuta = null;
            CurrentWraperConto.CurrentConto.CDetails = "";
            CurrentWraperConto.CurrentConto.DDetails = "";
            CurrentWraperConto.CurrentConto.UserId = Config.CurrentUser.Id;
            foreach (SaldoItem currentsaldos in ItemsCredit)
            {
                if (currentsaldos.Fieldkey == 30)
                {
                    CurrentWraperConto.CurrentConto.OborotValutaK = currentsaldos.ValueVal;
                    CurrentWraperConto.CurrentConto.CDetails += string.Format("{0} - {1} {2}\n", currentsaldos.Name, currentsaldos.ValueVal, currentsaldos.Lookupval);
                    currentsaldos.Value = currentsaldos.ValueVal.ToString(Vf.LevFormatUI);
                }
                else
                {
                    if (currentsaldos.Fieldkey == 31)
                    {
                        CurrentWraperConto.CurrentConto.OborotKolK = currentsaldos.ValueKol;
                        CurrentWraperConto.CurrentConto.CDetails += string.Format("{0} - {1} {2}\n", currentsaldos.Name, currentsaldos.ValueKol, currentsaldos.Lookupval);
                        //currentsaldos.Value = currentsaldos.ValueVal.ToString(Vf.LevFormatUI);
                    }
                    else
                    {
                        CurrentWraperConto.CurrentConto.CDetails += string.Format("{0} - {1} {2}\n", currentsaldos.Name, currentsaldos.Value, currentsaldos.Lookupval);
                    }

                }
            }
            foreach (SaldoItem currentsaldos in ItemsDebit)
            {
                if (currentsaldos.Fieldkey == 30)
                {
                    CurrentWraperConto.CurrentConto.OborotValutaD = currentsaldos.ValueVal;
                    CurrentWraperConto.CurrentConto.DDetails += string.Format("{0} - {1} {2}\n", currentsaldos.Name, currentsaldos.ValueVal, currentsaldos.Lookupval);
                }
                else
                {
                    if (currentsaldos.Fieldkey == 31)
                    {
                        CurrentWraperConto.CurrentConto.OborotKolD = currentsaldos.ValueKol;
                        CurrentWraperConto.CurrentConto.DDetails += string.Format("{0} - {1} {2}\n", currentsaldos.Name, currentsaldos.ValueKol, currentsaldos.Lookupval);
                    }
                    else
                    {
                        CurrentWraperConto.CurrentConto.DDetails += string.Format("{0} - {1} {2}\n", currentsaldos.Name, currentsaldos.Value, currentsaldos.Lookupval);
                    }

                }
            }
            Context.UpdateConto(CurrentWraperConto.CurrentConto);
            int ii = 0;
            if (ItemsCredit != null)
                foreach (SaldoItem currentsaldos in ItemsCredit)
                {
                    SaldoAnaliticModel sa = new SaldoAnaliticModel();
                    sa.ACCFIELDKEY = currentsaldos.Fieldkey;
                    sa.ACCID = CreditAccount;
                    sa.DATA = DateTime.Now;
                    sa.LOOKUPFIELDKEY = currentsaldos.LiD;
                    sa.LOOKUPVAL = currentsaldos.Lookupval;
                    sa.TYPEACCKEY = typeAnaliticalKey;
                    sa.VALUEDATE = currentsaldos.ValueDate;
                    sa.VAL = currentsaldos.Value;
                    sa.KURS = currentsaldos.IsKol ? currentsaldos.OnePrice : currentsaldos.ValueKurs;
                    sa.VALVAL = currentsaldos.IsKol ? currentsaldos.ValueKol : currentsaldos.ValueVal;
                    sa.KURSM = currentsaldos.MainKurs;
                    sa.KURSD = currentsaldos.KursDif;
                    sa.VALUENUM = currentsaldos.ValueInt;
                    sa.VALUED = currentsaldos.ValueDebit;
                    sa.TYPE = 2;
                    sa.LOOKUPID = currentsaldos.Relookup;
                    sa.ID = currentsaldos.Id;
                    sa.CONTOID = CurrentWraperConto.CurrentConto.Id;
                    sa.SORTORDER = ii;
                    Context.SaveContoMovement(sa);
                    ii++;
                }
            ii = 0;
            if (ItemsDebit != null)
                foreach (SaldoItem currentsaldos in ItemsDebit)
                {
                    SaldoAnaliticModel sa = new SaldoAnaliticModel();
                    sa.ACCFIELDKEY = currentsaldos.Fieldkey;
                    sa.ACCID = DebitAccount;
                    sa.DATA = DateTime.Now;
                    sa.LOOKUPFIELDKEY = currentsaldos.LiD;
                    sa.LOOKUPVAL = currentsaldos.Lookupval;
                    sa.TYPEACCKEY = typeAnaliticalKey;
                    sa.VALUEDATE = currentsaldos.ValueDate;
                    sa.VAL = currentsaldos.Value;
                    sa.KURS = currentsaldos.IsKol ? currentsaldos.OnePrice : currentsaldos.ValueKurs;
                    sa.VALVAL = currentsaldos.IsKol ? currentsaldos.ValueKol : currentsaldos.ValueVal;
                    sa.VALUENUM = currentsaldos.ValueInt;
                    sa.VALUED = currentsaldos.ValueDebit;
                    sa.KURSM = currentsaldos.MainKurs;
                    sa.KURSD = currentsaldos.KursDif;
                    sa.TYPE = 1;
                    sa.LOOKUPID = currentsaldos.Relookup;
                    sa.ID = currentsaldos.Id;
                    sa.CONTOID = CurrentWraperConto.CurrentConto.Id;
                    sa.SORTORDER = ii;
                    Context.SaveContoMovement(sa);
                    ii++;
                }
            View();
            ContoRepository.Instance.Remove(string.Format("{0}-{1}",CurrentWraperConto.CurrentConto.Id,1));
            ContoRepository.Instance.Remove(string.Format("{0}-{1}", CurrentWraperConto.CurrentConto.Id,2));
            ContoRepository.Instance.Add(string.Format("{0}-{1}", CurrentWraperConto.CurrentConto.Id,1), ItemsDebit.ToList());
            ContoRepository.Instance.Add(string.Format("{0}-{1}", CurrentWraperConto.CurrentConto.Id,2), ItemsCredit.ToList());
            if (OborotChange)
            {
                OborotChange = false;
                UpdateRelatedDds();
            }
        }
        protected override void Add()
        {
            EnableGrid = false;
            WorkValuta = null;
            AddN();
            base.Add();
            OnSetFocusExecuted(string.IsNullOrWhiteSpace(Kd)
                        ? new SetFocusEventArg("DDS")
                        : new SetFocusEventArg("Ob"));
        }
        private bool SaveConto()
        {
            bool result1 = true;
            bool issumirakis = false;
            if (IsSales || IsPurchases)
            {
                var isdds = IsDds;
                var isSales = IsSales;
                var isPurc = IsPurchases;
                result1 = false;
                decimal dds = 0m;
                Conto newConto = CurrentWraperConto.CurrentConto.Clone();
                newConto.FirmId = ConfigTempoSinglenton.GetInstance().CurrentFirma.Id;
                Total++;
                CurrentWraperConto.CurrentConto.DocumentId = Total;
                CurrentWraperConto.CurrentConto.Nd = Total;
                CurrentWraperConto.CurrentConto.CartotecaCredit = Total;

                if (IsDdsInclude)
                {
                    decimal koefic = 1;
                    if (CurrItemDdsDnevSales != null)
                    {
                        koefic = Math.Round((CurrItemDdsDnevSales.DdsPercent + 100m)/100m, 2);
                    }
                    else
                    {
                         if (CurrItemDdsDnevPurchases != null)
                         {
                             koefic = Math.Round((CurrItemDdsDnevPurchases.DdsPercent + 100m) / 100m, 2);
                         }
                    }
                    decimal suma = CurrentWraperConto.CurrentConto.Oborot;
                    if (ContoAll > 0)
                    {
                        suma = ContoAll;
                    }
                    decimal oborotbezdds = Math.Round(suma/koefic, 2);
                    //newConto.Oborot = CurrentWraperConto.CurrentConto.Oborot - oborotbezdds;
                    dds = oborotbezdds;
                    CurrentWraperConto.CurrentConto.Oborot = oborotbezdds;
                }
                else
                {
                    //newConto.Oborot = Math.Round((CurrentWraperConto.CurrentConto.Oborot * CurrItemDdsDnev.DdsPercent) / 100, 2);
                    decimal suma = CurrentWraperConto.CurrentConto.Oborot;
                    if (ContoAll != 0)
                    {
                        suma = ContoAll;
                        issumirakis = true;
                    }
                    dds = suma;

                }
                
                //addsecond = true;
                if (!SaveMainConto())
                {
                    MessageBoxWrapper.Show("Възникна неочаквана грешка при запис!Някое от полета не е попълнено правилно! Моля обърнете се към администратора на програмата!");

                }
                if (isPurc)
                {
                    CodeDnev = 1;
                    newConto.Oborot = CalculateDds(dds,true,true,issumirakis);

                }
                if (isSales)
                {
                    CodeDnev = 2;
                    newConto.Oborot = CalculateDds(dds,true,true,issumirakis);
                }

                if (CanceledDds)
                {
                    UpdateConto();
                    CanceledDds = false;
                }

                CurrentWraperConto.CurrentConto.Oborot = newConto.Oborot;
                CurrentWraperConto.CurrentConto.OborotValutaD = 0;
                CurrentWraperConto.CurrentConto.OborotValutaK = 0;
                CurrentWraperConto.CurrentConto.OborotKolD = 0;
                CurrentWraperConto.CurrentConto.OborotKolK = 0;
                CurrentWraperConto.CurrentConto.DocumentId = Total+1;
                CurrentWraperConto.CurrentConto.Nd = Total+1;
                CurrentWraperConto.CurrentConto.CartotecaCredit = Total+1;
                NullDdsItems();
                if (isdds)
                {
                       
                        
                   
                    if (isSales)
                    {
                        var result = AllAccountsK.FirstOrDefault(e => e.Short==Entrence.DdsSmetkaK);
                        if (result != null)
                        {
                            CAccountsModel = result;
                            CurrentWraperConto.CurrentConto.CreditAccount = result.Id;
                        }
                    }
                    if (isPurc)
                    {
                        var result = AllAccountsK.FirstOrDefault(e => e.Short == Entrence.DdsSmetkaD);
                        if (result != null)
                        {

                            DAccountsModel = result;
                            CurrentWraperConto.CurrentConto.DebitAccount = result.Id;
                        }
                    }
                    base.Add();
                }

                else
                {
                    //CurrentTypeOperation = TypeOperation[0];
                    addsecond = true;
                    //Add();
                    //CurrentTypeOperation = TypeOperation[0];
                    return true;
                }
                //CurrentTypeOperation = TypeOperation[0];//clear dds settings 
                RefreshUI();
            }
            else
              {
                    Total++;
                    CurrentWraperConto.CurrentConto.DocumentId = Total;
                    CurrentWraperConto.CurrentConto.Nd = Total;
                    CurrentWraperConto.CurrentConto.CartotecaCredit = Total;
                    SaveMainConto();
               
                
                //hook for valutna razlika
                IsKursova = false;
                var iskurs = false;
                ObservableCollection<SaldoItem> prenos = new ObservableCollection<SaldoItem>();
                decimal kurssuma = 0;
                foreach (SaldoItem currentsaldos in ItemsDebit)
                {
                    
                    if (currentsaldos.KursDif != 0)
                    {
                        kurssuma = currentsaldos.KursDif;
                        IsKursova = true;
                        if (currentsaldos.KursDif < 0)
                        {
                            var result = AllAccountsK.FirstOrDefault(e => e.Short == "724");
                            if (result != null)
                            {
                                CAccountsModel = result;
                                CurrentWraperConto.CurrentConto.CreditAccount = result.Id;
                                CurrentWraperConto.CurrentConto.Oborot = currentsaldos.KursDif*(-1);
                                currentsaldos.MainKurs = currentsaldos.ValueKurs;
                                currentsaldos.KursDif = 0;
                                currentsaldos.ValueVal = 0;
                            }
                        }
                        else
                        {
                            var result = AllAccountsK.FirstOrDefault(e => e.Short == "624");
                            if (result != null)
                            {
                                iskurs = true;
                                CurrentWraperConto.CurrentConto.Oborot = currentsaldos.KursDif;
                                currentsaldos.MainKurs = currentsaldos.ValueKurs;
                                currentsaldos.KursDif = 0;
                                currentsaldos.ValueVal = 0;
                                prenos = new ObservableCollection<SaldoItem>(ItemsDebit);
                                CAccountsModel = DAccountsModel;
                                DAccountsModel = result;
                            }

                        }
                    }
                }
                if (iskurs)
                {
                    ItemsCredit = new ObservableCollection<SaldoItem>(prenos);
                }
                addsecond = true;
                
              }
            return result1;
        }

        private void NullDdsItems()
        {
            ShowContoAll = Visibility.Hidden;
            ContoAll = 0;
            CurrItemDdsDnevPurchases = null;
            CurrItemDdsDnevSales = null;
            if (CurrentWraperConto != null)
            {
                if (CurrentWraperConto.CurrentConto != null)
                {
                    CurrentWraperConto.CurrentConto.IsPurchases = 0;
                    CurrentWraperConto.CurrentConto.IsSales = 0;
                }
            }
            IsPurchases = false;
            IsDdsPurchases = false;
            IsDdsIncludePurchases = false;
            IsSales = false;
            IsDdsSales = false;
            IsDdsIncludeSales = false;

            VopSales = "";
            VopPurchases = "";
            
        }

        public decimal CalculateDds(decimal newConto, bool isloadvalue,bool isdds,bool issuma,bool isin=false)
        {
            isddsmode = true;
            DdsDnevnikModel ddsDnevnikModel = Context.LoadDenevnicItem(CurrentWraperConto.CurrentConto.Id, CodeDnev);
            ddsDnevnikModel.Title = CodeDnev == 1 ? "Дневник покупки" : "Дневник продажби";
            if (isin && ddsDnevnikModel.IsLinked)
            {
                var dialog1 = new DdsSellsView(ddsDnevnikModel, DddDelete, DdsDelayUpdate,DdsCancel);
                dialog1.ShowDialog();
                isddsmode = false;
                return dialog1.SumaDds;
            }
            //ddsDnevnikModel.DocId = CurrentWraperConto.CurrentConto.DocNum;
                ddsDnevnikModel.Date = CurrentWraperConto.CurrentConto.Data;
                ddsDnevnikModel.DataF = CurrentWraperConto.CurrentConto.Data;
                ddsDnevnikModel.KindActivity = CodeDnev;
                ddsDnevnikModel.KindDoc = 1;
                
                ddsDnevnikModel.CodeDoc = Kd;
                ddsDnevnikModel.Stoke = CurrentWraperConto.CurrentConto.Reason;
                ddsDnevnikModel.DdsIncluded = IsDdsInclude ? "ВКЛЮЧЕН ДДС" : "НЕВКЛЮЧЕН ДДС";
                ddsDnevnikModel.Total = CurrentWraperConto.Oborot.ToString(Vf.LevFormatUI);
            if (!ddsDnevnikModel.IsLinked)
                {
                    if (CodeDnev == 2)
                    {
                        if (ddsDnevnikModel.LookupID == 0)
                        {

                            foreach (SaldoItem saldoItem in ItemsCredit)
                            {
                                if (saldoItem.IsLookUp && saldoItem.Name.Contains("Контрагент"))
                                {
                                    ddsDnevnikModel.LookupID = saldoItem.Relookup;
                                    //ddsDnevnikModel.LookupElementID = saldoItem.LiD;
                                    ddsDnevnikModel.ClNum = saldoItem.Value;

                                }
                                if (saldoItem.Name.Contains("Дата на фактура"))
                                {
                                    ddsDnevnikModel.DataF = saldoItem.ValueDate;

                                }
                                if (saldoItem.Name.Contains("Номер фактура"))
                                {
                                    ddsDnevnikModel.DocId = saldoItem.Value;
                                }
                            }
                            foreach (SaldoItem saldoItem in ItemsDebit)
                            {
                                if (saldoItem.IsLookUp && saldoItem.Name.Contains("Контрагент"))
                                {
                                    ddsDnevnikModel.LookupID = saldoItem.Relookup;
                                    //ddsDnevnikModel.LookupElementID = saldoItem.LiD;
                                    ddsDnevnikModel.ClNum = saldoItem.Value;
                                }
                                if (saldoItem.Name.Contains("Дата на фактура"))
                                {
                                    ddsDnevnikModel.DataF = saldoItem.ValueDate;

                                }
                                if (saldoItem.Name.Contains("Номер фактура"))
                                {
                                    ddsDnevnikModel.DocId = saldoItem.Value;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (ddsDnevnikModel.LookupID == 0)
                        {

                            foreach (SaldoItem saldoItem in ItemsDebit)
                            {
                                if (saldoItem.IsLookUp && saldoItem.Name.Contains("Контрагент"))
                                {
                                    ddsDnevnikModel.LookupID = saldoItem.Relookup;
                                    //ddsDnevnikModel.LookupElementID = saldoItem.LiD;
                                    ddsDnevnikModel.ClNum = saldoItem.Value;

                                }
                                if (saldoItem.Name.Contains("Дата на фактура"))
                                {
                                    ddsDnevnikModel.DataF = saldoItem.ValueDate;

                                }
                                if (saldoItem.Name.Contains("Номер фактура"))
                                {
                                    ddsDnevnikModel.DocId = saldoItem.Value;
                                }
                            }
                            foreach (SaldoItem saldoItem in ItemsCredit)
                            {
                                if (saldoItem.IsLookUp && saldoItem.Name.Contains("Контрагент"))
                                {
                                    ddsDnevnikModel.LookupID = saldoItem.Relookup;
                                    //ddsDnevnikModel.LookupElementID = saldoItem.LiD;
                                    ddsDnevnikModel.ClNum = saldoItem.Value;
                                }
                                if (saldoItem.Name.Contains("Дата на фактура"))
                                {
                                    ddsDnevnikModel.DataF = saldoItem.ValueDate;

                                }
                                if (saldoItem.Name.Contains("Номер фактура"))
                                {
                                    ddsDnevnikModel.DocId = saldoItem.Value;
                                }
                            }
                        }
                    }
                }
            
            decimal ddspercent;
                ddspercent = CodeDnev == 1
                    ? CurrItemDdsDnevPurchases != null ? CurrItemDdsDnevPurchases.DdsPercent : 0
                    : CurrItemDdsDnevSales != null ? CurrItemDdsDnevSales.DdsPercent : 0;
                string name = CodeDnev == 1
                    ? CurrItemDdsDnevPurchases != null ? CurrItemDdsDnevPurchases.Name : ""
                    : CurrItemDdsDnevSales != null ? CurrItemDdsDnevSales.Name : "";
           
            DdsSellsView dialog;
                DdsViewModel.RefreshElement callback = null;
                if (isdds)
                {
                    callback = DdsDelayUpdate;
                }
                else
                {
                    callback = DdsSaved;
                }

                if (!ddsDnevnikModel.IsLinked)
                {
                    ddsDnevnikModel.IsSuma = issuma ? 1 : 0;
                dialog = new DdsSellsView(ddsDnevnikModel, new DdsDnevnicItem
                {
                    DdsPercent = ddspercent,
                    DdsSuma = newConto != 0 ? newConto : CurrentWraperConto.Oborot,
                    Name = name,
                    In = true
                    }, isdds ? (DdsViewModel.RefreshElement) null : DddDelete, callback,DdsCancel);

                }
                else
                {
                    if (ddsDnevnikModel.IsSuma == 0)
                    {
                        dialog = new DdsSellsView(ddsDnevnikModel, new DdsDnevnicItem
                        {
                            DdsPercent = ddspercent,
                            DdsSuma = CurrentWraperConto.Oborot,
                            Name = name,
                            In=true
                        }, isdds ? (DdsViewModel.RefreshElement) null : DddDelete, callback,DdsCancel);
                    }
                    else
                    {
                        ddsDnevnikModel.Total = ddsDnevnikModel.DetailItems.Sum(e => e.DdsSuma).ToString(Vf.LevFormatUI);
                        dialog = new DdsSellsView(ddsDnevnikModel, DddDelete, callback,DdsCancel);
                    }
                }
            
            dialog.ShowDialog();
            isddsmode = false;
            return dialog.SumaDds;
        }

        private void RefreshUI()
        {   OnPropertyChanged("Oborot");
            OnPropertyChanged("DocId");
            OnPropertyChanged("Note");
            OnPropertyChanged("Data");
            OnPropertyChanged("Folder");
            OnPropertyChanged("Reason");
            OnPropertyChanged("DataInvoise");
            OnPropertyChanged("NumberObject");
            OnPropertyChanged("Index");
            OnPropertyChanged("Month");
            OnPropertyChanged("Year");
            OnPropertyChanged("HasDnev");
            OnPropertyChanged("IsPurchases");
            OnPropertyChanged("IsSales");
            OnPropertyChanged("IsDds");
            OnPropertyChanged("IsDdsInclude");
            OnPropertyChanged("Total");
            OnPropertyChanged("Pr1");
            OnPropertyChanged("Pr2");
            OnPropertyChanged("Kd");
            OnPropertyChanged("Id");
        }

        private bool SaveMainConto()
        {
            CurrentWraperConto.CurrentConto.CDetails = "";
            CurrentWraperConto.CurrentConto.DDetails = "";
            foreach (SaldoItem currentsaldos in ItemsCredit)
            {
                if (currentsaldos.Fieldkey == 30)
                {
                    CurrentWraperConto.CurrentConto.OborotValutaK = currentsaldos.ValueVal;
                    CurrentWraperConto.CurrentConto.CDetails += string.Format("{0} - {1} {2}\n", currentsaldos.Name, currentsaldos.ValueVal, currentsaldos.Lookupval);
                    currentsaldos.Value = currentsaldos.ValueVal.ToString(Vf.LevFormatUI);
                }
                else
                {
                    if (currentsaldos.Fieldkey == 31)
                    {
                        CurrentWraperConto.CurrentConto.OborotKolK = currentsaldos.ValueKol;
                        CurrentWraperConto.CurrentConto.CDetails += string.Format("{0} - {1} {2}\n", currentsaldos.Name, currentsaldos.ValueKol, currentsaldos.Lookupval);
                        //currentsaldos.Value = currentsaldos.ValueVal.ToString(Vf.LevFormatUI);
                    }
                    else
                    {
                        CurrentWraperConto.CurrentConto.CDetails += string.Format("{0} - {1} {2}\n", currentsaldos.Name, currentsaldos.Value, currentsaldos.Lookupval);
                    }

                }
            }
            foreach (SaldoItem currentsaldos in ItemsDebit)
            {
                if (currentsaldos.Fieldkey == 30)
                {
                    CurrentWraperConto.CurrentConto.OborotValutaD = currentsaldos.ValueVal;
                    CurrentWraperConto.CurrentConto.DDetails += string.Format("{0} - {1} {2}\n", currentsaldos.Name, currentsaldos.ValueVal, currentsaldos.Lookupval);
                    currentsaldos.Value = currentsaldos.ValueVal.ToString(Vf.LevFormatUI);
                }
                else
                {
                    if (currentsaldos.Fieldkey == 31)
                    {
                        CurrentWraperConto.CurrentConto.OborotKolD = currentsaldos.ValueKol;
                        CurrentWraperConto.CurrentConto.DDetails += string.Format("{0} - {1} {2}\n", currentsaldos.Name, currentsaldos.ValueKol, currentsaldos.Lookupval);
                    }
                    else
                    {
                        CurrentWraperConto.CurrentConto.DDetails += string.Format("{0} - {1} {2}\n", currentsaldos.Name, currentsaldos.Value, currentsaldos.Lookupval);
                    }

                }

            }

            //if (CurrentWraperConto.CurrentConto.Id == 0) return;
            int ii = 0;
            List<SaldoAnaliticModel> debit = new List<SaldoAnaliticModel>();
            List<SaldoAnaliticModel> credit = new List<SaldoAnaliticModel>();
            if (ItemsCredit != null)
                foreach (SaldoItem currentsaldos in ItemsCredit)
                {
                    SaldoAnaliticModel sa = new SaldoAnaliticModel();
                    sa.ACCFIELDKEY = currentsaldos.Fieldkey;
                    sa.ACCID = CreditAccount;
                    sa.DATA = DateTime.Now;
                    //if (currentsaldos.SelectedLookupItem != null && !string.IsNullOrWhiteSpace(currentsaldos.SelectedLookupItem.Key))
                    //{
                    //    if (currentsaldos.SelectedLookupItem.Key != null)
                    //    {
                    //        int rez;
                    //        sa.LOOKUPFIELDKEY = int.TryParse(currentsaldos.SelectedLookupItem.Key,out rez)?rez:0;
                    //        sa.LOOKUPVAL = currentsaldos.SelectedLookupItem.Value;
                    //    }

                    //}
                    sa.LOOKUPFIELDKEY = currentsaldos.LiD;
                    sa.LOOKUPVAL = currentsaldos.Lookupval;
                    sa.TYPEACCKEY = typeAnaliticalKey;
                    sa.VALUEDATE = currentsaldos.ValueDate;
                    sa.VAL = currentsaldos.Value;
                    sa.VALUEMONEY = currentsaldos.Valuedecimal;
                    sa.VALUENUM = currentsaldos.ValueInt;
                    sa.VALUED = currentsaldos.Valuedecimald;
                    sa.KURS = currentsaldos.IsKol ? currentsaldos.OnePrice : currentsaldos.ValueKurs;
                    sa.VALVAL = currentsaldos.IsKol ? currentsaldos.ValueKol : currentsaldos.ValueVal;
                    sa.KURSM = currentsaldos.MainKurs;
                    sa.KURSD = currentsaldos.KursDif;
                    sa.KURSD = currentsaldos.KursDif;
                    sa.TYPE = 2;
                    sa.LOOKUPID = currentsaldos.Relookup;
                    sa.CONTOID = 0;
                    sa.SORTORDER = ii;
                    //Context.SaveContoMovement(sa);
                    debit.Add(sa);
                    ii++;
                }
            ii = 0;
            if (ItemsDebit != null)
                foreach (SaldoItem currentsaldos in ItemsDebit)
                {
                    SaldoAnaliticModel sa = new SaldoAnaliticModel();

                    sa.ACCFIELDKEY = currentsaldos.Fieldkey;
                    sa.ACCID = DebitAccount;
                    sa.DATA = DateTime.Now;
                    sa.LOOKUPFIELDKEY = currentsaldos.LiD;
                    sa.TYPEACCKEY = typeAnaliticalKey;
                    sa.VALUEDATE = currentsaldos.ValueDate;
                    sa.VAL = currentsaldos.Value;
                    sa.VALUEMONEY = currentsaldos.Valuedecimal;
                    sa.VALUENUM = currentsaldos.ValueInt;
                    sa.VALUED = currentsaldos.Valuedecimald;
                    sa.KURS = currentsaldos.IsKol ? currentsaldos.OnePrice : currentsaldos.ValueKurs;
                    sa.VALVAL = currentsaldos.IsKol ? currentsaldos.ValueKol : currentsaldos.ValueVal;
                    sa.KURSM = currentsaldos.MainKurs;
                    sa.KURSD = currentsaldos.KursDif;
                    sa.KURSD = currentsaldos.KursDif;
                    sa.TYPE = 1;
                    sa.LOOKUPID = currentsaldos.Relookup;
                    sa.LOOKUPVAL = currentsaldos.Lookupval;
                    sa.CONTOID = 0;
                    sa.SORTORDER = ii;
                    //Context.SaveContoMovement(sa);
                    credit.Add(sa);
                    ii++;
                }
            return Context.SaveConto(CurrentWraperConto.CurrentConto,debit,credit);
        }
        
        public  ObservableCollection<AccountsModel> AllAccountsK { get; set; }
        public ObservableCollection<LookUpSpecific> TypeOperation { get; set; }

        public string  LoadAnaliticDetailsCredit(string accname)
       {
           if (!accname.Contains("/"))
           {
               int num;
               if (int.TryParse(accname, out num))
               {
                   var model = AllAccountsK.FirstOrDefault(e => e.Num == num && e.SubNum == 0);
                    if (model == null)
                    {
                        RefreshAcc();
                        model = AllAccountsK.FirstOrDefault(e => e.Num == num && e.SubNum == 0);
                    }
                    if (model != null)
                   {
                       CAccountsModel = model;
                       return model.Short;
                   }
                   MainAcc mainAcc =
                       new MainAcc(
                           new AccountsModel
                               {
                                   FirmaId = ConfigTempoSinglenton.GetInstance().CurrentFirma.Id,
                                   TypeAccountEx = 1,
                                   Num = num
                               }, EditMode.Add, true,"",true);
                   if (mainAcc.NoAcc)
                   {
                       MessageBoxWrapper.Show("Нама такава сметка в Националния сметкоплан");
                       ItemsCredit = new ObservableCollection<SaldoItem>();
                       CAccountsModel = null;
                       return "";
                   }
                   mainAcc.ShowDialog();
                  
                   if (mainAcc.DialogResult.HasValue && mainAcc.DialogResult.Value)
                   {
                       AllAccountsK = new ObservableCollection<AccountsModel>(Context.GetAllAccounts(FirmaId));
                       model = AllAccountsK.FirstOrDefault(e => e.Num == num && e.SubNum == 0);
                       if (model != null)
                       {
                           CAccountsModel = model;
                           return model.Short;
                       }
                   }
                   ItemsCredit= new ObservableCollection<SaldoItem>();
                   CAccountsModel = null;
                   return "";
               }
           }
           else
           {
               int num, subnum;
               var ac = accname.Split('/');

               if (int.TryParse(ac[0], out num) && int.TryParse(ac[1], out subnum))
               {
                   var model = AllAccountsK.FirstOrDefault(e => e.Num == num && e.SubNum == subnum);
                   if (model == null)
                   {
                        RefreshAcc();
                        model = AllAccountsK.FirstOrDefault(e => e.Num == num && e.SubNum == subnum);
                   }
                    if (model != null)
                   {
                       CAccountsModel = model;
                       return model.Short;
                   }
                   MainAcc mainAcc =
                       new MainAcc(
                           new AccountsModel
                               {
                                   FirmaId = ConfigTempoSinglenton.GetInstance().CurrentFirma.Id,
                                   TypeAccountEx = 1,
                                   Num = num,
                                   SubNum = subnum
                               }, EditMode.Add, false,"",true);
                   mainAcc.ShowDialog();
                   if (mainAcc.DialogResult.HasValue && mainAcc.DialogResult.Value)
                   {
                       AllAccountsK = new ObservableCollection<AccountsModel>(Context.GetAllAccounts(FirmaId));
                       model = AllAccountsK.FirstOrDefault(e => e.Num == num && e.SubNum == subnum);
                       if (model != null)
                       {
                           CAccountsModel = model;
                           return model.Short;
                       }
                   }
                   CAccountsModel = null;
                   ItemsCredit = new ObservableCollection<SaldoItem>();
                   return "";
               }
           }
           CAccountsModel = null;
           ItemsCredit = new ObservableCollection<SaldoItem>();
           return "";
       }
        public string LoadAnaliticDetailsDebit(string accname)
       {
           if (!accname.Contains("/"))
           {
               int num;
               if (int.TryParse(accname, out num))
               {
                   var model = AllAccountsK.FirstOrDefault(e => e.Num == num && e.SubNum == 0);
                   if (model == null)
                   {
                       RefreshAcc();
                       model = AllAccountsK.FirstOrDefault(e => e.Num == num && e.SubNum == 0);
                   }
                   if (model != null)
                   {
                       DAccountsModel = model;
                       return model.Short;
                   }
                   else
                   {
                       
                       MainAcc mainAcc =
                           new MainAcc(
                               new AccountsModel
                                   {
                                       FirmaId = ConfigTempoSinglenton.GetInstance().CurrentFirma.Id,
                                       TypeAccountEx = 1,
                                       Num = num
                                   }, EditMode.Add, true,"",true);
                       if (mainAcc.NoAcc)
                       {
                           MessageBoxWrapper.Show("Нама такава сметка в Националния сметкоплан");
                           
                           ItemsDebit = new ObservableCollection<SaldoItem>();
                           DAccountsModel = null;
                           return "";
                       }
                       mainAcc.ShowDialog();
                       if (mainAcc.DialogResult.HasValue && mainAcc.DialogResult.Value)
                       {
                           AllAccountsK = new ObservableCollection<AccountsModel>(Context.GetAllAccounts(FirmaId));
                           model = AllAccountsK.FirstOrDefault(e => e.Num == num && e.SubNum == 0);
                           if (model != null)
                           {
                               DAccountsModel = model;
                               return model.Short;
                           }
                       }
                       ItemsDebit=new ObservableCollection<SaldoItem>();
                       DAccountsModel = null;
                       return "";
                       
                       
                   }
               }
               ItemsDebit = new ObservableCollection<SaldoItem>();
               DAccountsModel = null;
               return "";
           }
           else
           {
               int num, subnum;
               var ac = accname.Split('/');

               if (int.TryParse(ac[0], out num) && int.TryParse(ac[1], out subnum))
               {
                   var model = AllAccountsK.FirstOrDefault(e => e.Num == num && e.SubNum == subnum);
                   if (model == null)
                   {
                       RefreshAcc();
                       model = AllAccountsK.FirstOrDefault(e => e.Num == num && e.SubNum == subnum);
                   }
                   if (model != null)
                   {
                       DAccountsModel = model;
                       return model.Short;
                   }
                   MainAcc mainAcc =
                       new MainAcc(
                           new AccountsModel
                           {
                               FirmaId = ConfigTempoSinglenton.GetInstance().CurrentFirma.Id,
                               TypeAccountEx = 1,
                               Num = num,
                               SubNum = subnum
                           }, EditMode.Add, false,"",true);
                   mainAcc.ShowDialog();
                   if (mainAcc.DialogResult.HasValue && mainAcc.DialogResult.Value)
                   {
                       AllAccountsK = new ObservableCollection<AccountsModel>(Context.GetAllAccounts(FirmaId));
                       model = AllAccountsK.FirstOrDefault(e => e.Num == num && e.SubNum == subnum);
                       if (model != null)
                       {
                           DAccountsModel = model;
                           return model.Short;
                       }
                   }
                   ItemsDebit = new ObservableCollection<SaldoItem>();
                   DAccountsModel = null;
                   return "";
               }
           }
           ItemsDebit = new ObservableCollection<SaldoItem>();
           DAccountsModel = null;
           return "";
          
       }
        public IEnumerable<SaldoItem> LoadCreditAnaliticAtributes(IEnumerable<SaldoAnaliticModel> fields,int typecpnto)
       {
            List<SaldoItem> saldoItems =new List<SaldoItem>();
            int offset = 16;
            if (typecpnto == 2) offset = 60;
            int current = 0;
            foreach (SaldoAnaliticModel analiticalFields in fields.OrderBy(e => e.SORTORDER))
           {
               current++;
               //Titles.Add(analiticalFields.Name);
               SaldoItemTypes saldotype = SaldoItemTypes.String;
               if (analiticalFields.DBField == "integer")
               {
                   saldotype = SaldoItemTypes.Integer;

               }
               if (analiticalFields.DBField.Contains("DECIMAL"))
               {
                   saldotype = SaldoItemTypes.Currency;

               }
               if (analiticalFields.DBField == "Date")
               {
                   saldotype = SaldoItemTypes.Date;

               }
               
               SaldoItem saldoItem = new SaldoItem();
               saldoItem.ChangedKindCurrency+=saldoItem_ChangedKindCurrency;
                saldoItem.Type = saldotype;                   
                saldoItem.Name = analiticalFields.Name;
                saldoItem.Value = analiticalFields.VAL;
                saldoItem.Fieldkey = analiticalFields.ACCFIELDKEY;
                saldoItem.IsK=typecpnto==0;
                saldoItem.IsD=typecpnto==1;
                saldoItem.Id=analiticalFields.ID;
                saldoItem.KursDif = analiticalFields.KURSD;
                saldoItem.ValueKurs = analiticalFields.KURS;
                saldoItem.MainKurs = analiticalFields.KURSM;
                saldoItem.ValueVal = analiticalFields.VALVAL;
                saldoItem.ValueCredit = analiticalFields.VALUEMONEY;
                saldoItem.Lookupval = analiticalFields.LOOKUPVAL;
               saldoItem.TabIndex = offset + current;                         
               if (analiticalFields.ACCFIELDKEY == 29 || analiticalFields.ACCFIELDKEY == 30 ||
                   analiticalFields.ACCFIELDKEY == 31)
               {
                   saldoItem.IsDK = true;
                   if (analiticalFields.ACCFIELDKEY == 30)
                   {
                       //saldoItem.InfoTitle = "Валутен курс";
                       saldoItem.IsVal = true;
                       //if (typecpnto == 0)
                       //{
                       //    try
                       //    {
                       //        saldoItem.InfoValue = DAccountsModel.EndSaldoL/DAccountsModel.EndSaldoV;
                       //    }
                       //    catch (Exception)
                       //    {
                       //        saldoItem.InfoValue =0;
                       //    }
                           
                       //}
                       //if (typecpnto == 1)
                       //{
                       //    try
                       //    {
                       //        saldoItem.InfoValue = CAccountsModel.EndSaldoL / CAccountsModel.EndSaldoV;
                       //    }
                       //    catch (Exception)
                       //    {
                       //        saldoItem.InfoValue = 0;
                       //    }

                       //}
                   }
                   if (analiticalFields.ACCFIELDKEY == 31)
                   {
                       //    saldoItem.InfoTitle = "Единичнa цена";
                       saldoItem.IsKol = true;
                       saldoItem.ValueKol = analiticalFields.VALVAL;
                       saldoItem.OnePrice = analiticalFields.KURS;
                   }
                   //    if (typecpnto == 1)
                   //    {
                   //        try
                   //        {
                   //            saldoItem.InfoValue = (DAccountsModel.EndSaldoL / DAccountsModel.EndSaldoK);
                   //        }
                   //        catch (Exception)
                   //        {
                   //            saldoItem.InfoValue = 0;
                   //        }

                   //    }
                   //    if (typecpnto == 0)
                   //    {
                   //        try
                   //        {
                   //            saldoItem.InfoValue = CAccountsModel.EndSaldoL / CAccountsModel.EndSaldoK;
                   //        }
                   //        catch (Exception)
                   //        {
                   //            saldoItem.InfoValue = 0; ;
                   //        }

                   //    }
                   //}
               }
               if (analiticalFields.LOOKUPID != 0)
               {
                   //saldoItem.LiD = analiticalFields.LOOKUPFIELDKEY;
                  
                   saldoItem.Relookup = analiticalFields.LOOKUPID;
                   saldoItem.IsLookUp = true;
                   //LookupModel lm = Context.GetLookup(analiticalFields.LOOKUPID);
                   
                       
                   

                   //var list = Context.GetLookupDictionary(lm.LookUpMetaData.Tablename, ConfigTempoSinglenton.GetInstance().CurrentFirma.Id,""," FIRST 30 ");
                   //int k = 0;
                   //foreach (var enumerable in list)
                   //{
                   //    int i = 0;
                   //    SaldoItem saldoitem = new SaldoItem();
                   //    saldoitem.Name = saldoItem.Name;
                   //    saldoitem.Key = enumerable[lm.Fields[0].NameEng].ToString();
                   //    saldoitem.Value = enumerable[lm.Fields[1].NameEng].ToString();
                   //    saldoItem.LookUp.Add(saldoitem);
                   //    saldoItem.SelectedLookupItem =
                   //        saldoItem.LookUp.FirstOrDefault(e => e.Value == saldoItem.Value);
                   //}
                   //if (!string.IsNullOrWhiteSpace(analiticalFields.RTABLENAME))
                   //{
                   //    saldoItem.Key = analiticalFields.LOOKUPFIELDKEY.ToString();
                   //    saldoItem.IsLookUp = true;
                   //    var list1 = Context.GetLookup(analiticalFields.RTABLENAME, ConfigTempoSinglenton.GetInstance().CurrentFirma.Id);
                   //    k = 0;
                   //    foreach (IEnumerable<string> enumerable in list1)
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
               }
               saldoItems.Add(saldoItem);
           }
           return saldoItems;
       }

        private void saldoItem_ChangedKindCurrency(object sender, ChangeKindCurrencyArg e)
        {
            if (Mode == EditMode.Add)
            {
                WorkValuta = e.KindCurrency;
                if (!string.IsNullOrWhiteSpace(WorkValuta))
                {
                    UpdateValutenKurs(WorkValuta);
                }
            }
        }

        protected int GroupId { get; set; }

        public string Index
        {
            get { 
                if (CurrentWraperConto != null)
                {
                    SearchWord = CurrentWraperConto.Id;
                    return CurrentWraperConto.NomId.ToString();
        
                }
                return "";
            }
        }
       
        public int Total
        {
            get
            {
                return _total; 
            }
            set { _total = value;OnPropertyChanged("Total"); }
        }
        public string Month
        {
            get
            {
                if (CurrentWraperConto != null)
                {
                    SearchMonth = CurrentWraperConto.Month;
                    return CurrentWraperConto.Month.ToString();
                }
                return "";
            }
        }
        public string Year
        {
            get
            {
                if (CurrentWraperConto != null) return CurrentWraperConto.Year.ToString();
                return "";
            }
        }
        protected string Title
        {
            get; set;
        }

        protected bool IsEdit
        {
            get; set;
        }
        protected override void MoveFirst()
        {
            var search = new CSearchAcc();
            var d = ConfigTempoSinglenton.GetInstance().WorkDate;
            //search.Id = CurrentWraperConto.Id.ToString();
            search.UserId = IsCurrentUser ? Config.CurrentUser.Id.ToString() : null;
            search.FromDate = new DateTime(d.Year, d.Month, 1);
            search.ToDate = new DateTime(d.Year, d.Month, GetEndDayMonth(d.Month));
            var conto = new List<Conto>(Context.GetNextConto(FirmaId, search));
            if (conto != null && conto.Count > 0)
            {
                CurrentWraperConto = new WraperConto(conto.First());
            }
        }
        protected override void MoveLast()
        {
            var search = new CSearchAcc();
            var d = ConfigTempoSinglenton.GetInstance().WorkDate;
            //search.Id = CurrentWraperConto.Id.ToString();
            search.UserId = IsCurrentUser ? Config.CurrentUser.Id.ToString() : null;
            search.FromDate = new DateTime(d.Year, d.Month, 1);
            search.ToDate = new DateTime(d.Year, d.Month, GetEndDayMonth(d.Month));
            var conto = new List<Conto>(Context.GetPrevConto(FirmaId, search));
            if (conto != null && conto.Count > 0)
            {
                CurrentWraperConto = new WraperConto(conto.First());
       
            }
        }
        public void Next()
        {
            MoveNext();
        }
        public void Prev()
        {
            MovePrevius();
        }

        protected override void MoveNext()
        {
            //_currentItem++;
            //if (_currentItem >= AllWrapedConto.Count)
            //{
            //    _currentItem = 0;
            //    //if (CurrentWraperConto.Page<MaxPages)
            //    //{
            //    //    GetLast10Conto(CurrentWraperConto.Page - 1);
            //    //}
            //}
            //CurrentWraperConto = AllWrapedConto[_currentItem];
            ////CurrentWraperConto.CurrentConto = AllWrapedConto[_currentItem].CurrentWraperConto.CurrentConto;
            var search = new CSearchAcc();
            var d = ConfigTempoSinglenton.GetInstance().WorkDate;
            search.Id = CurrentWraperConto.Id.ToString();
            search.UserId = IsCurrentUser? Config.CurrentUser.Id.ToString():null;
            search.FromDate = new DateTime(d.Year, d.Month, 1);
            search.ToDate = new DateTime(d.Year, d.Month, GetEndDayMonth(d.Month));
            var conto = new List<Conto>(Context.GetNextConto(FirmaId, search));
            if (conto != null && conto.Count > 0)
            {
                CurrentWraperConto = new WraperConto(conto.First());
            }
        }
        protected override void MovePrevius()
        {
            //_currentItem--;
            //if (_currentItem < 0)
            //{
            //    _currentItem = AllWrapedConto.Count - 1;
            //    //if (CurrentWraperConto.Page > 0)
            //    //{
            //    //    GetLast10Conto(CurrentWraperConto.Page-1);
            //    //}
            //}
            ////CurrentWraperConto.CurrentConto = AllWrapedConto[_currentItem].CurrentWraperConto.CurrentConto;
            //CurrentWraperConto = AllWrapedConto[_currentItem];
            var search = new CSearchAcc();
            var d = ConfigTempoSinglenton.GetInstance().WorkDate;
            search.Id = CurrentWraperConto.Id.ToString();
            search.UserId = IsCurrentUser ? Config.CurrentUser.Id.ToString() : null;
            search.FromDate = new DateTime(d.Year, d.Month, 1);
            search.ToDate = new DateTime(d.Year, d.Month, GetEndDayMonth(d.Month));
            var conto = new List<Conto>(Context.GetPrevConto(FirmaId, search));
            if (conto != null && conto.Count>0)
            {
                CurrentWraperConto = new WraperConto(conto.Last());
            }
        }
        public void AddN()
        {
            ShowLastFive = false;
            var currentConto = new Conto{UserId=Config.CurrentUser.Id};
            currentConto.Data = ConfigTempoSinglenton.GetInstance().WorkDate;
            currentConto.DataInvoise = ConfigTempoSinglenton.GetInstance().WorkDate;
            currentConto.CartotecaCredit = Total + 1;
            currentConto.DocumentId= Total + 1;
            currentConto.Nd = Total + 1;
            if (CurrentWraperConto != null)
            {
                

                if (addsecond)
                {
                    currentConto.Data = CurrentWraperConto.CurrentConto.Data;
                    currentConto.DataInvoise = CurrentWraperConto.CurrentConto.DataInvoise;
                    currentConto.Folder = CurrentWraperConto.CurrentConto.Folder;
                    currentConto.NumberObject = CurrentWraperConto.CurrentConto.NumberObject;
                    int test;
                    if (int.TryParse(CurrentWraperConto.CurrentConto.DocNum,out test))
                    {
                        currentConto.DocNum = (test+1).ToString();
                    }
                    currentConto.DebitAccount = CurrentWraperConto.CurrentConto.DebitAccount;
                    currentConto.CreditAccount = CurrentWraperConto.CurrentConto.CreditAccount;
                    currentConto.Reason = CurrentWraperConto.CurrentConto.Reason;
                    currentConto.Note=CurrentWraperConto.CurrentConto.Note;
                    currentConto.KindDoc = CurrentWraperConto.CurrentConto.KindDoc;
                    currentConto.KD = CurrentWraperConto.CurrentConto.KD;
                }
                if (IsKursova)
                {
                    currentConto.Oborot = CurrentWraperConto.CurrentConto.Oborot;
                }
            }
            if (addsecond && !IsKursova)
            {
                foreach (SaldoItem item in ItemsCredit)
                {
                    if (item.Type == SaldoItemTypes.Date)
                    {
                        if (CurrentWraperConto != null) item.Value = CurrentWraperConto.Data;
                    }
                    else
                    {
                        if (item.Name != "Вид Валута")
                        {
                            item.Value = "";
                            item.Lookupval = "";
                        }
                        if (item.Name == "Сума валута")
                        {
                            item.ValueVal = 0;
                        }
                        item.OnePrice = 0;
                        item.ValueKol = 0;
                    }

                    //item.Lookupval = "";
                }
                foreach (SaldoItem item in ItemsDebit)
                {
                    if (item.Type == SaldoItemTypes.Date)
                    {
                        if (CurrentWraperConto != null) item.Value = CurrentWraperConto.Data;
                    }
                    else
                    {
                        if (item.Name != "Вид валута")
                        {
                            item.Value = "";
                            item.Lookupval = "";
                        }
                        if (item.Name == "Сума валута")
                        {
                            item.ValueVal = 0;
                        }
                        item.OnePrice = 0;
                        item.ValueKol = 0;
                    }
                }

            }
            else
            {
                if (!IsKursova)
                { 
                    ItemsCredit = new ObservableCollection<SaldoItem>();
                    ItemsDebit = new ObservableCollection<SaldoItem>();
                }
                
            }
            currentConto.FirmId = ConfigTempoSinglenton.GetInstance().CurrentFirma.Id;
            CurrentWraperConto = new WraperConto(currentConto);
            //AllWrapedConto.Add(CurrentWraperConto);
            //IsShowNavigation = false;
            //addsecond = false;
        }
        protected override void View()
        {
            addsecond = false;
            if (Mode == EditMode.Add)
            {
                //ShowLastFive = true;
                //LoadLastRecordsAsynk();
                MoveLast();

            }
            base.View();
            
        }
        public DelegateCommand SellsCommand { get; set; }

        public DelegateCommand PurchaseCommand { get; set; }

        public string Error
        {
            get
            {
                var dataErrorInfo = CurrentWraperConto.CurrentConto as IDataErrorInfo;
                if (dataErrorInfo != null)
                    return dataErrorInfo.Error;
                return null;
            }
        }

        public string this[string columnName]
        {
            get
            {
                string error = "";
                if (CurrentWraperConto != null && CurrentWraperConto.CurrentConto != null)
                {
                    error = GetValidationError(columnName);
                    // Dirty the commands registered with CommandManager,
                    // such as our Save command, so that they are queried
                    // to see if they can execute now.
                    CommandManager.InvalidateRequerySuggested();
                }
                return error;

            }

        }

        public static readonly string[] ValidatedProperties =
            {
                "CAccountsModel",
                "DAccountsModel",
                "DocId",
                "Folder",
                "ItemsDebit",
                "ItemsCredit",
                "Note",
                "Reason"
            };
        

        public string GetValidationError(string propertyName)
        {
            if (Array.IndexOf(ValidatedProperties, propertyName) < 0)
                return null;

            string error = null;

            switch (propertyName)
            {
                case "CAccountsModel":
                    error = this.CAccountsModel==null?"Не е въведена сметка в полето":ValidateAcc(this.CAccountsModel.Id);
                    break;
                case "DAccountsModel":
                    error = this.DAccountsModel == null ? "Не е въведена сметка в полето" : ValidateAcc(this.DAccountsModel.Id);
                    break;
                case "DocId":
                    error = String.IsNullOrWhiteSpace(DocId)||(DocId != null && DocId.Length>20) ? "Не е въведен номер на документа или е по дълъг от 20 символа" : null;
                    break;
                case "Folder":
                    error = String.IsNullOrWhiteSpace(Folder)||(Folder !=null && Folder.Length>10) ? "Не е въведена папка или е по-дълга от 10 символа" : null;
                    break;
                case "ItemsCredit":
                    error = ValidateItemsCredit();
                    break;
                case "ItemsDebit":
                    error = ValidateItemsDebit();
                    break;
                case "Note":
                    error = this.Note != null && this.Note.Length > 50 ? "Много дълга забележка! Моля намалете до 50 символа!" : null;
                    break;
                case "Reason":
                    error = this.Reason != null && this.Reason.Length > 50 ? "Много дълго основание! Моля намалете до 50 символа!" : null;
                    break;   
            }

            return error;
        }

        private string ValidateItemsDebit()
        {
            return (from saldoItem in ItemsDebit where string.IsNullOrWhiteSpace(saldoItem.Value) where !saldoItem.IsVal where !saldoItem.IsKol  select "Невалидна стойност на поле " + saldoItem.Name).FirstOrDefault();
        }

        private string ValidateItemsCredit()
        {
            return (from saldoItem in ItemsCredit where string.IsNullOrWhiteSpace(saldoItem.Value) where !saldoItem.IsVal where !saldoItem.IsKol select "Невалидна стойност на поле " + saldoItem.Name).FirstOrDefault();
        }

        private string ValidateAcc(int accid)
        {
            if (accid == 0 || accid < 0)
            {
                return "Не е въведена сметка";
            }
            return null;
        }

        protected override bool CanSave()
        {
            return IsValid;
        }
        
        public bool IsValid
        {
            get
            {
                foreach (string property in ValidatedProperties)
                    if (GetValidationError(property) != null)
                        return false;

                return true;
            }
        }

        public ObservableCollection<string> Osnovanie { get; set; }

        public ObservableCollection<string> Notes { get; set; }

        public List<List<string>> GetItems()
        {
            List<List<string>> result = new List<List<string>>();
            foreach (var conto in AllWrapedConto)
            {
                List<string> newitem = new List<string>();
                newitem.Add(conto.NomId.ToString());
                newitem.Add(conto.DocId);
                var firstOrDefault = AllAccountsK.FirstOrDefault(e => e.Id == conto.CurrentConto.DebitAccount);
                if (firstOrDefault != null)
                    newitem.Add(firstOrDefault.Short.Trim(' '));
                var accountsModel = AllAccountsK.FirstOrDefault(e => e.Id == conto.CurrentConto.CreditAccount);
                newitem.Add(conto.Data);
                if (accountsModel != null)
                    newitem.Add(accountsModel.Short.Trim(' '));
                newitem.Add(conto.Oborot.ToString(Vf.LevFormatUI));
                newitem.Add(conto.Val.ToString(Vf.LevFormatUI));
                newitem.Add(conto.ValK.ToString(Vf.LevFormatUI));
                newitem.Add(conto.Kol.ToString(Vf.LevFormatUI));
                newitem.Add(conto.KolK.ToString(Vf.LevFormatUI));
                newitem.Add(conto.Folder);
                newitem.Add(conto.Reason);
                newitem.Add(conto.Note);
                newitem.Add(conto.DataInvoise);
                result.Add(newitem);
            }
            return result;
        }

        public List<string> GetTitles()
        {
            return ReportItems.Select(reportItem => reportItem.Name).ToList();
        }

        public List<string> GetHeader()
        {

            var ret = new List<string>();
            ret.Add(String.Format("Дата на извлечението: {0}", DateTime.Now.ToShortDateString()));
            ret.Add(String.Format("За фирма            : {0}", ConfigTempoSinglenton.GetInstance().CurrentFirma.Name));
            ret.Add(String.Format("Съставил            : {0}", Entrence.UserName));
            return ret;
        }

        public List<string> GetFuther()
        {
            List<string> result = new List<string>();
            return result;
        }

        public string Filename
        {
            get { return "accreport"; }
           
        }

       

        public ICollection<object> GetRecordsBy(int StartingIndex, int NumberOfRecords, object FilterTag)
        {
            List<WraperConto> result = new List<WraperConto>();
            GetLast10Conto(StartingIndex,NumberOfRecords);
            foreach (WraperConto wraperConto in AllWrapedConto)
            {
                result.Add(wraperConto); 
            }

            return result.ToList<object>();
        }

        private void GetLast10Conto(int startingIndex, int numberOfRecords,WraperConto wraperConto=null,bool firsrow=false,bool getlast=true)
        {
            if (!ShowLastFive) return;
                var AllConto =
                new ObservableCollection<Conto>(IsCurrentUser? Context.GetAllConto(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id,CreateAcc(), startingIndex, numberOfRecords): Context.GetAllConto(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id, startingIndex, numberOfRecords));
            AllWrapedConto = new ObservableCollection<WraperConto>();
            RefreshConto(AllConto);
            OnPropertyChanged("AllWrapedConto");
            
        }
        private void ReloadLast(int startingIndex, int numberOfRecords)
        {
            if (ShowLastFive)
            {
                var AllConto =
                   new ObservableCollection<Conto>(Context.GetAllConto(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id, _startingIndex, _numberOfRecords));
                AllWrapedConto = new ObservableCollection<WraperConto>();
                RefreshConto(AllConto);
            }
        }

        private void ReloadLastFive(object sender, DoWorkEventArgs e)
        {
            Total = FetchCount();
            Count= FetchCountCurrUser();
            int i = Count > 0 ? 1 : 0;
            int pages = (Count - i) / _numberOfRecords;
            _currentPage = pages + 1;
            AllPages = pages + 1;
            FromToPages = string.Format("{0} от {1}", CurrentPage, AllPages);
            //FetchRange(0, _numberOfRecords);
            // AllWrapedContoAsync=new VirtualizingCollection<WraperConto>(this,_numberOfRecords);
            DontChange = true;
            if (Count > _numberOfRecords)
            {
                ReloadLast(Count - _numberOfRecords, _numberOfRecords);
            }
            else
            {
                ReloadLast(0, _numberOfRecords);
            }
            DontChange = false;
           
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            OnPropertyChanged("AllWrapedConto");
            if (!IsKursova)
            {
                Oborot = 0;
                foreach (SaldoItem item in ItemsCredit)
                {
                    if (item.Name == "Сума валута")
                    {
                        item.ValueVal = 0;
                    }
                    item.OnePrice = 0;
                    item.ValueKol = 0;
                }

                foreach (SaldoItem item in ItemsDebit)
                {

                    if (item.Name == "Сума валута")
                    {
                        item.ValueVal = 0;
                    }
                    item.OnePrice = 0;
                    item.ValueKol = 0;
                }

            }
        Visible = Visibility.Hidden;
            if (Mode == EditMode.View || Mode == EditMode.Edit)
            {
                EnableGrid = true;
            }
        }

        public ObservableCollection<WraperConto> AllWrapedConto { get; set; }
        //public VirtualizingCollection<WraperConto> AllWrapedContoAsync{get;set;}
        private WraperConto _currentWraperConto;
        public WraperConto CurrentWraperConto
        {
            get { return _currentWraperConto; }
            set
            {
                if (value != null && !DontChange)
                {
                    _currentWraperConto = value;
                    CurrentWraperConto.CurrentConto = value.CurrentConto;
                    
                    if (!String.IsNullOrWhiteSpace(CurrentWraperConto.VopPurchases))
                    {
                        CurrItemDdsDnevPurchases =
                            ItemsDdsDnevPurchases.FirstOrDefault(e => e.Code == CurrentWraperConto.VopPurchases);
                    }
                    else
                    {
                        CurrItemDdsDnevPurchases = null;
                    }
                    if (!String.IsNullOrWhiteSpace(CurrentWraperConto.VopSales))
                    {
                        CurrItemDdsDnevSales =
                            ItemsDdsDnevSales.FirstOrDefault(e => e.Code == CurrentWraperConto.VopSales);
                    }
                    else
                    {
                        CurrItemDdsDnevSales = null;
                    }
                    var daccountsModel =
                        AllAccountsK.FirstOrDefault(e => e.Id == CurrentWraperConto.CurrentConto.DebitAccount);
                    var caccountsModel =
                        AllAccountsK.FirstOrDefault(e => e.Id == CurrentWraperConto.CurrentConto.CreditAccount);

                    DAccountsModel = daccountsModel;
                    CAccountsModel = caccountsModel;
                    if (!addsecond)
                    {
                        ItemsCredit =
                            new ObservableCollection<SaldoItem>(LoadDetails(CurrentWraperConto.CurrentConto.Id, 2));
                        ItemsDebit =
                            new ObservableCollection<SaldoItem>(LoadDetails(CurrentWraperConto.CurrentConto.Id, 1));
                    }
                    OnPropertyChanged("CurrentWraperConto");
                   
                    //if (CurrentWraperConto.KindActivity=="1")
                    //{
                    //    IsPurchases=true;
                    //}
                    //if (CurrentWraperConto.KindActivity == "2")
                    //{
                    //    IsSales = true;
                    //}
                    RefreshUI();
                    KindDoc = KindDocLookup.FirstOrDefault(e => e.CodetId == Kd);
                    //OnSinkExecuted(new SinkEventArgs(value.Id));
                }
            }
        }
        
        protected override void Report()
        {
            ReportDialog report = new ReportDialog(this);
            report.ShowDialog();
        }

        public IEnumerable<ReportItem> ReportItems { get; set; }
        
        string IReportBuilder.Title
        {
            get { return "Движение по сметки"; }
        }

        private int FirmaId;

        public bool HasDnev
        {
            get { return (CurrentWraperConto.IsPurchases || CurrentWraperConto.IsSales); }
        }
        public string GetQuery()
        {
           return String.Format("SELECT a.\"Id\", a.\"Date\",c.NUM||'/'||c.\"SubNum\" AS debit,d.NUM||'/'||d.\"SubNum\" as credit,a.\"Oborot\",a.\"Reason\", a.\"Note\", a.\"DataInvoise\", a.\"NumberObject\"," +
                                "a.DOCNUM, a.OBOROTVALUTA, a.OBOROTKOL,a.OBOROTVALUTAK, a.OBOROTKOLK, a.FOLDER FROM \"conto\" a inner join \"accounts\" c on c.\"Id\"=a.\"DebitAccount\"" +
                                "inner join \"accounts\" d on d.\"Id\"=a.\"CreditAccount\" where \"FirmId\"={0} and YY={1} order by a.\"Id\"", ConfigTempoSinglenton.GetInstance().CurrentFirma.Id, ConfigTempoSinglenton.GetInstance().WorkDate.Year);
        }
        public List<string> Columns()
        {
            return new List<string> { "Номер", "Дата", "Док Номер", "Дебит", "Кредит", "Оборот", "Причина", "Забележка", "Дата фактура", "Номер обект", "Валута", "Количество", "Папка","14","15" };
        }
        protected override void Search()
        {
            SearcByNum();
        }
        
        public List<string> GetSubTitles()
        {
            return new List<string>();
        }
        
        public List<List<string>> GetTXTAntetka()
        {
            return null;
        }
        
        public long MaxPages { get; set; }
        
        public PaggingControl PaggingControl { get; set;}

        internal void UpdateOborot(decimal sumavaluta)
        {
            if (this.Mode == EditMode.Edit || this.Mode == EditMode.Add)
            {
                Oborot = Math.Round(sumavaluta,2);
            }
        }

        internal void UpdateValutenKurs(string vidvaluta)
        {
            if (notupdated) return;
            if (this.Mode == EditMode.Add)
            {
                WorkValuta = vidvaluta;
                decimal kursforaday = 0m;
                var loadKursForaDay = Context.LoadKursForaDay(Data, vidvaluta);
                if (loadKursForaDay != null)
                {
                    kursforaday = loadKursForaDay.Value;

                }
                else
                {
                    ValutaAdd valutaAdd = new ValutaAdd(Data, vidvaluta);
                    valutaAdd.ShowDialog();
                    if (valutaAdd.DialogResult.HasValue && valutaAdd.DialogResult.Value)
                    {
                        kursforaday = valutaAdd.Kurs;
                    }
                }
                foreach (var saldoItem in ItemsCredit)
                {
                    saldoItem.ValueKurs = kursforaday;
                    saldoItem.MainKurs = kursforaday;
                }
                foreach (var saldoItem in ItemsDebit)
                {
                    saldoItem.ValueKurs = kursforaday;
                    saldoItem.MainKurs = kursforaday;
                }
            }
        }

        public bool IsPurchases
        {
            get
            {
                if (CurrentWraperConto != null && CurrentWraperConto.CurrentConto != null)
                {
                    return CurrentWraperConto.CurrentConto.IsPurchases == 1;
                }
                return false;
            }
            set
            {
                if (CurrentWraperConto != null && CurrentWraperConto.CurrentConto != null)
                {
                    //if (CurrentWraperConto.CurrentConto.IsPurchases==1)
                    //{

                    //    if (MessageBoxWrapper.Show("Сигурен ли си ,че искаш да изтриеш запис в дневника?", "Предупреждение", MessageBoxWrapperButtons.YesNo) == DialogResult.Yes)
                    //    {
                    //        DdsDnevnikModel ddsDnevnikModel = Context.LoadDenevnicItem(CurrentWraperConto.CurrentConto.Id, 1);
                    //        Context.DeleteDdsDnevnicModel(ddsDnevnikModel.Id);
                    //        CurrentWraperConto.CurrentConto.IsPurchases = value ? 1 : 0;
                            
                    //        CurrItemDdsDnevPurchases = null;
                    //    IsDdsPurchases = false;
                    //    IsDdsIncludePurchases = false;
                    //    VopPurchases = "";
                    //    addnewddsitem = false;    
                    //    }
                    //}
                    //else
                    //{
                        CurrentWraperConto.CurrentConto.IsPurchases = value ? 1 : 0;
                        addnewddsitem = true;
                    //}
                    OnPropertyChanged("IsPurchases");
                    
                }
                
            }
        }
        public bool IsSales
        {
            get
            {
                if (CurrentWraperConto != null && CurrentWraperConto.CurrentConto != null) return CurrentWraperConto.CurrentConto.IsSales==1;
                return false;
            }
            set
            {
                //if (CurrentWraperConto != null && CurrentWraperConto.CurrentConto != null) CurrentWraperConto.CurrentConto.IsSales = value?1:0;
                //OnPropertyChanged("IsSales");
                //if (CurrentWraperConto.CurrentConto.IsSales == 1)
                //{

                //    if (MessageBoxWrapper.Show("Сигурен ли си ,че искаш да изтриеш запис в дневника?", "Предупреждение", MessageBoxWrapperButtons.YesNo) == DialogResult.Yes)
                //    {
                //        DdsDnevnikModel ddsDnevnikModel = Context.LoadDenevnicItem(CurrentWraperConto.CurrentConto.Id, 2);
                //        Context.DeleteDdsDnevnicModel(ddsDnevnikModel.Id);
                //        CurrentWraperConto.CurrentConto.IsSales = value ? 1 : 0;


                //        CurrItemDdsDnevSales = null;
                //        IsDdsSales = false;
                //        IsDdsIncludeSales = false;
                //        VopSales = "";
                //        addnewddsitem = false;
                //    }
               // }
                //else
               // {
                    CurrentWraperConto.CurrentConto.IsSales = value ? 1 : 0;
                    addnewddsitem = true;
                //}
                OnPropertyChanged("IsSales");
                
            }
        }

        public int FetchCount()
        {
            return Context.GetAllContoCount(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id, ConfigTempoSinglenton.GetInstance().WorkDate.Year, ConfigTempoSinglenton.GetInstance().WorkDate.Month);
        }
        public int FetchCountCurrUser()
        {
            return IsCurrentUser?Context.GetAllContoCount(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id, CreateAcc()):Total;
        }
        public IList<WraperConto> FetchRange(int startIndex, int count)
        {
            if (startIndex + count > Count)
            {
                count = Count - startIndex;
            }
            var rez=IsCurrentUser?Context.GetAllConto(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id,CreateAcc(),startIndex,count):Context.GetAllConto(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id, startIndex, count);
            RefreshConto(new ObservableCollection<Conto>(rez));

            if (CurrentWraperConto==null) 
            {
                if (AllWrapedConto != null && AllWrapedConto.Count>0) CurrentWraperConto = CurrentWraperConto ??AllWrapedConto.Last();
            }
            return AllWrapedConto.ToList();

        }

        private string _progress;
        private int Count;
        private int _id;

        public string Progress
        {
            get { return _progress; }
            set { _progress = value;OnPropertyChanged("Progress"); }
        }

        public System.Windows.Controls.DataGrid DataGrid { get; set; }

        public bool SearcByNum()
        {

            bool rez = false;
            CSearchAcc acc = CreateAcc();
            acc.Id = SearchWord.ToString();
            acc.UserId = null;
            var AllConto = new ObservableCollection<Conto>(Context.GetAllConto(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id, acc));
            if (AllConto.Count > 0)
            {
                CurrentWraperConto = new WraperConto(AllConto.First());
                rez = true;
            }
            return rez;

        }

        private CSearchAcc CreateAcc()
        {
            CSearchAcc acc = new CSearchAcc();
            var d = ConfigTempoSinglenton.GetInstance().WorkDate;
            acc.UserId = Config.CurrentUser.Id.ToString();
            acc.FromDate = new DateTime(d.Year, d.Month, 1);
            acc.ToDate = new DateTime(d.Year, d.Month, GetEndDayMonth(d.Month));
            return acc;
        }

        private bool FindInAllConto(long SearchWord, int SearchMonth)
        {
            var allconto = Context.GetAllConto(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id);
            var conto = allconto.FirstOrDefault(e => e.Data.Month == SearchMonth && e.Nd == SearchWord);
            if (conto!=null)
            {
                var count = allconto.Count(e => (e.Data.Month<SearchMonth)||(e.Data.Month==SearchMonth && e.Nd<conto.Nd));
                 if (count>_numberOfRecords)
                {
                    _currentPage = count/_numberOfRecords+1;
                    GetLast10Conto((_currentPage - 1) * _numberOfRecords, _numberOfRecords,new WraperConto(conto));
                    FromToPages = string.Format("{0} от {1}", _currentPage, AllPages);
                }
                else
                {
                    _currentPage = 1;
                    GetLast10Conto((_currentPage - 1) * _numberOfRecords, _numberOfRecords,new WraperConto(conto));
                    FromToPages = string.Format("{0} от {1}", _currentPage, AllPages);
                }
                return true;
            }
            return false;
        }

        private bool FindInAllConto1(string SearchDoc, string SearchFol)
        {
            var allconto = Context.GetAllConto(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id);
            var conto = allconto.FirstOrDefault(e => e.DocNum == SearchDoc && e.Folder == SearchFol);
            if (conto != null)
            {
                var count = allconto.Count(e =>  e.Nd < conto.Nd);
                if (count > _numberOfRecords)
                {
                    _currentPage = count / _numberOfRecords + 1;
                    GetLast10Conto((_currentPage - 1) * _numberOfRecords, _numberOfRecords, new WraperConto(conto));
                    FromToPages = string.Format("{0} от {1}", _currentPage, AllPages);
                }
                else
                {
                    _currentPage = 1;
                    GetLast10Conto((_currentPage - 1) * _numberOfRecords, _numberOfRecords, new WraperConto(conto));
                    FromToPages = string.Format("{0} от {1}", _currentPage, AllPages);
                }
                return true;
            }
            return false;
        }

        private long _searchWord;
        public long SearchWord
        {
            get { return _searchWord; }
            set { _searchWord = value; OnPropertyChanged("SearchWord");}
        }

        private int _searchMonth;
        private bool addnewddsitem;

        public int SearchMonth
        {
            get { return _searchMonth; }
            set { _searchMonth = value;OnPropertyChanged("SearchMonth"); }
        }

        public DelegateCommand MoveNextPageCommand { get; set; }

        public DelegateCommand MovePreviusPageCommand { get; set; }

        public DelegateCommand MoveLastPageCommand { get; set; }

        public DelegateCommand MoveFirstPageCommand { get; set; }

        private int _currentPage;
        private bool addsecond;
        public int CurrentPage
        {
            get { return _currentPage; }
            set
            {
                MoveDirection = _currentPage > value;
                _currentPage = value;
                var bw = new BackgroundWorker { WorkerReportsProgress = false, WorkerSupportsCancellation = true };
                bw.DoWork += new DoWorkEventHandler(ReloadPage);
                bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_ReloadPageCompleted);
                bw.RunWorkerAsync();
            }
        }

        private void bw_ReloadPageCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (MoveDirection)
            {
                if (AllWrapedConto != null && AllWrapedConto.Count > 0)
                {
                    CurrentWraperConto = AllWrapedConto.Last();
                }
            }
            else
            {
                if (AllWrapedConto != null && AllWrapedConto.Count > 0)
                {
                    CurrentWraperConto = AllWrapedConto.First();
                }
            }
            Visible = Visibility.Hidden;
            OnSinkExecuted(new SinkEventArgs(0, MoveDirection ? 1 : 2));
            if (Mode == EditMode.View || Mode == EditMode.Edit)
            {
                EnableGrid = true;
            }
        }

        private void ReloadPage(object sender, DoWorkEventArgs e)
        {
            Visible = Visibility.Visible;
            EnableGrid = false;
            GetLast10Conto((_currentPage - 1) * _numberOfRecords, _numberOfRecords, null, first);
            first = false;
            FromToPages = string.Format("{0} от {1}", _currentPage, AllPages);
        }

        public int AllPages { get; set; }

        public void DoFacturaInfoDebit()
        {
            string nomerf = "";
            string contr = "";
            foreach (SaldoItem saldoItem in ItemsDebit)
            {
                if (saldoItem.Name=="Контрагент")
                {
                    contr = saldoItem.Lookupval;
                }
                if (saldoItem.Name == "Номер фактура")
                {
                    nomerf = saldoItem.Value;
                }
            }
            if (!string.IsNullOrWhiteSpace(contr) && !string.IsNullOrWhiteSpace(nomerf))
            {
                FacturaContragentControl facturaContragentControl = new FacturaContragentControl(DAccountsModel, nomerf, contr);
                facturaContragentControl.ShowDialog();
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(contr))
                {
                    FacturaControl tetka = new FacturaControl(DAccountsModel,this,contr);
                    tetka.ShowDialog();
                   //SetInvoiseNum(tetka.SelectedItem, ItemsDebit);
                   // FacturaContragentControl facturaContragentControl = new FacturaContragentControl(DAccountsModel,contr);
                   // facturaContragentControl.ShowDialog();
                }
                else
                {
                    FacturaControl tetka = new FacturaControl(DAccountsModel,this);
                    tetka.ShowDialog();
                //    SetInvoiseNum(tetka.SelectedItem, ItemsDebit);
                }
            }
            
        }

        public void DoFacturaInfoCredit()
        {
            string nomerf = "";
            string contr = "";
            foreach (SaldoItem saldoItem in ItemsCredit)
            {
                if (saldoItem.Name == "Контрагент")
                {
                    contr = saldoItem.Lookupval;
                }
                if (saldoItem.Name == "Номер фактура")
                {
                    nomerf = saldoItem.Value;
                }
            }
            if (!string.IsNullOrWhiteSpace(contr) && !string.IsNullOrWhiteSpace(nomerf))
            {
                FacturaContragentControl facturaContragentControl = new FacturaContragentControl(CAccountsModel, nomerf, contr);
                facturaContragentControl.ShowDialog();
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(contr))
                {
                    FacturaControl tetka = new FacturaControl(CAccountsModel, this, contr);
                    tetka.ShowDialog();
                    //GoToLastRecord();
                }
                else
                {
                    FacturaControl tetka = new FacturaControl(CAccountsModel, this);
                    tetka.ShowDialog();
                    //GoToLastRecord();
                }

            }
            
        }

        private void SetInvoiseNum(AccItemSaldo selectedItem,ObservableCollection<SaldoItem> items)
        {
            if (selectedItem == null) return;
            foreach (SaldoItem saldoItem in items)
            {
                if (saldoItem.Name == "Номер фактура")
                {
                    saldoItem.Value=selectedItem.NInvoise;
                }
            }
        }

        


        public  bool SearcByNum1()
        {
            bool rez = false;
            //var s =
            //    AllWrapedConto.FirstOrDefault(
            //        e => e.DocId == SearchDoc && e.Folder == SearchFolder && e.Year == CurrentWraperConto.Year);
            //if (s != null)
            //{
            //    CurrentWraperConto = s;
            //    rez = true;
            //}
            //else
            //{
            //    if (FindInAllConto1(SearchDoc, SearchFolder))
            //    {
            //        rez = true;
            //    }
            //    else
            //    {
            //        MessageBoxWrapper.Show("Няма намерен резултат");
            //    }
            //}
            //var conto=Context.GE
            CSearchAcc acc = new CSearchAcc();
            acc.Folder = SearchFolder;
            acc.NumDoc = SearchDoc;
            var d = ConfigTempoSinglenton.GetInstance().WorkDate;
            acc.FromDate = new DateTime(d.Year, d.Month, 1);
            acc.ToDate= new DateTime(d.Year, d.Month, GetEndDayMonth(d.Month));
            var AllConto = new ObservableCollection<Conto>(Context.GetAllConto(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id, acc));
            if (AllConto.Count > 0)
            {
                CurrentWraperConto = new WraperConto(AllConto.First());
                rez = true;
            }
            return rez;
        }

        private int GetEndDayMonth(int toMonth)
        {
            int rez = 30;
            switch (toMonth)
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    {
                        rez = 31;
                    }
                    break;
                case 2:
                    rez = IsYearBig(ConfigTempoSinglenton.GetInstance().WorkDate.Year) ? 29 : 28;
                    break;
            }
            return rez;
        }

        protected virtual bool IsYearBig(int currentYear)
        {
            return currentYear % 4 == 0;
        }
        private string _searchDoc;
        public string SearchDoc
        {
            get { return _searchDoc; }
            set { _searchDoc = value; OnPropertyChanged("SearchDoc"); }
        }

        private string _searchFolder;
        private LookUpSpecific _kindDoc;
        private decimal _contoAll;
        private Visibility _showContoAll;
        private bool first;
        private bool _showLastFive;
        private int _total;
        private int _startingIndex;
        private bool MoveDirection;
        private Visibility _visible;
        private int _Id;
        public bool isddsmode;
        

        public string SearchFolder
        {
            get { return _searchFolder; }
            set { _searchFolder = value;OnPropertyChanged("SearchFolder"); }
        }

        public Visibility ShowContoAll
        {
            get { return _showContoAll; }
            set { _showContoAll = value; OnPropertyChanged("ShowContoAll"); }
        }

        public decimal ContoAll
        {
            get { return _contoAll; }
            set
            {
                _contoAll = value;
                OnPropertyChanged("ContoAll"); }
        }

        public DelegateCommand SumaDdsCommand { get; set; }

        internal void AddFromV()
        {
            base.Add();
        }

        internal void SaveF3()
        {
            if (CanSave())
            {
                if (CurrentWraperConto != null && CurrentWraperConto.CurrentConto != null)
                {
                    
                    if (SaveConto())
                    {
                        if (!IsKursova) Oborot = 0;
                        DontChange = true;
                        ReloadRecords();
                        DontChange = false;
                        RefreshUI();
                        Second();
                        base.Add();
                        OnSetFocusExecuted(new SetFocusEventArg("Ob"));
                    }
                }
            }
        }
        public void SaveF4()
        {
            if (CanSave())
            {
                if (CurrentWraperConto != null && CurrentWraperConto.CurrentConto != null)
                {

                    if (SaveConto())
                    {
                        
                    }
                }
            }
        }

        protected override void AddNewRepeat()
        {
            SaveF3();
        }

        protected override bool CanAddNewRepeat()
        {
            return IsValid;
        }

        internal void RefreshAcc()
        {
            AllAccountsK = new ObservableCollection<AccountsModel>(Context.GetAllAccounts(FirmaId));
        }

        internal void DoDetailsDebit(EditMode mode)
        {
            string filter = "";
            string contofilter = "";
            bool first = true;
            foreach (var saldoItem in ItemsDebit)
            {
                if (!String.IsNullOrWhiteSpace(saldoItem.Value) && !saldoItem.Name.Contains("Дата "))
                {
                    filter = string.Format("{0}|{1} ", filter, saldoItem.Value);
                    if (first && saldoItem.IsLookUp)
                    {
                        contofilter = string.Format("{0} - {1} {2} ", saldoItem.Name, saldoItem.Value, saldoItem.Lookupval);
                        first = false;
                    }
                }
                else
                {
                    break;
                }
            }
           DetailsUniverse sv = new DetailsUniverse(DAccountsModel, $"{filter}#{contofilter}",this,1,mode);
            sv.ShowDialog();
            if (sv.SelectedRow != null)
            {
                int i = 0;
                Oborot = decimal.Parse(sv.SelectedRow[sv.SelectedRow.Count - 1]);
                string[] stringSeparators = new string[] { "---" };
                foreach (var saldoItem in ItemsDebit)
                {
                    if (saldoItem.Name == "Количествo")
                    {
                        Oborot = decimal.Parse(sv.SelectedRow[sv.SelectedRow.Count - 1]);
                        saldoItem.ValueKol = decimal.Parse(sv.SelectedRow[sv.SelectedRow.Count - 5]);
                        saldoItem.Value = sv.SelectedRow[sv.SelectedRow.Count - 5];
                        i++;
                        continue;

                    }
                    if (saldoItem.Name == "Сума валута")
                    {
                        Oborot = decimal.Parse(sv.SelectedRow[sv.SelectedRow.Count - 1]);
                        saldoItem.ValueVal=decimal.Parse(sv.SelectedRow[sv.SelectedRow.Count - 5]);
                        saldoItem.Value = sv.SelectedRow[sv.SelectedRow.Count - 5];
                        i++;
                        continue;
                    }
                    var item = sv.SelectedRow[i].Trim();
                    if (item.Contains("---"))
                    {
                        var spliti = item.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                        saldoItem.Value = spliti[0];
                        saldoItem.Lookupval = spliti[1];
                    }
                    else
                    {
                        saldoItem.Value = item;
                    }
                    i++;
                }
            }
        }

        internal void DoDetailsCredit(EditMode mode)
        {
            string filter = "";
            string contofilter = "";
            bool first = true;
            foreach (var saldoItem in ItemsCredit)
            {
                if (!String.IsNullOrWhiteSpace(saldoItem.Value))
                {
                    filter = string.Format("{0}|{1} ", filter, saldoItem.Value);
                    if (first && saldoItem.IsLookUp)
                    {
                        contofilter = string.Format("{0} - {1} {2} ", saldoItem.Name, saldoItem.Value, saldoItem.Lookupval);
                        first = false;
                    }
                }
                else
                {
                    break;
                }
            }
            DetailsUniverse sv = new DetailsUniverse(CAccountsModel, $"{filter}#{contofilter}",this,2,mode);
            sv.ShowDialog();
            if (sv.SelectedRow != null)
            {
                string[] stringSeparators = new string[] { "---" };
                int i = 0;
                Oborot = decimal.Parse(sv.SelectedRow[sv.SelectedRow.Count - 1]);
                foreach (var saldoItem in ItemsCredit)
                {
                    if (saldoItem.Name == "Количествo")
                    {
                        Oborot = decimal.Parse(sv.SelectedRow[sv.SelectedRow.Count - 1]);
                        saldoItem.ValueKol = decimal.Parse(sv.SelectedRow[sv.SelectedRow.Count - 5]);
                        saldoItem.Value = sv.SelectedRow[sv.SelectedRow.Count - 5];
                        i++;
                        continue;

                    }
                    if (saldoItem.Name == "Сума валута")
                    {
                        Oborot = decimal.Parse(sv.SelectedRow[sv.SelectedRow.Count - 1]);
                        saldoItem.ValueVal = decimal.Parse(sv.SelectedRow[sv.SelectedRow.Count - 5]);
                        saldoItem.Value = sv.SelectedRow[sv.SelectedRow.Count - 5];
                        i++;
                        continue;
                    }
                    var item = sv.SelectedRow[i].Trim();
                    if (item.Contains("---"))
                    {
                        var spliti = item.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                        saldoItem.Value = spliti[0];
                        saldoItem.Lookupval = spliti[1];
                    }
                    else
                    {
                        saldoItem.Value = item;
                    }
                    i++;
                }
            }
        }

        internal void UpdateCol()
        {
            foreach (var saldoItem in ItemsCredit)
            {
                if (saldoItem.IsKol && saldoItem.ValueKol>0)
                {
                    saldoItem.OnePrice = Oborot/saldoItem.ValueKol;
                }
            }
            foreach (var saldoItem in ItemsDebit)
            {
                if (saldoItem.IsKol && saldoItem.ValueKol>0)
                {
                    saldoItem.OnePrice = Oborot/saldoItem.ValueKol;
                }
            }
        }

        public bool DontChange { get; set; }
        public bool ShowLastFive {
            get
            {
                return _showLastFive;
            }
            set
            {
                _showLastFive = value;
                if (_showLastFive)
                {
                    LoadLastRecordsAsynk();
                }
                OnPropertyChanged("ShowLastFive");
            }
        }

        internal void GetPrevVal(string s)
        {
            if (AllWrapedConto.Count > 0)
            {
                var wraperconto = AllWrapedConto[AllWrapedConto.Count - 1];
                if (s == "Folder")
                {
                    Folder = wraperconto.Folder;
                }
            }
        }

        internal void OnUpdateValuta()
        {
            UpdateValuta();
        }

        public Visibility Visible
        {
            get { return _visible; }
            set { _visible = value; OnPropertyChanged("Visible"); }
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

        public bool notupdated { get;  set; }
        public bool IsKursova { get; private set; }
    }
}
