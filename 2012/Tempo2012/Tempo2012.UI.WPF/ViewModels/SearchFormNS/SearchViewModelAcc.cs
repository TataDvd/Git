using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Xml.Serialization;
using ReportBuilder;
using Tempo2012.EntityFramework;
using Tempo2012.EntityFramework.Interface;
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.PaggingControlProject;
using Tempo2012.UI.WPF.ViewModels.ContoManagment;
using Tempo2012.UI.WPF.ViewModels.SearchFormNS;
using Tempo2012.UI.WPF.Views.ReportManager;

namespace Tempo2012.UI.WPF.ViewModels.SearchFormNS
{
    [Serializable]
    public class SearchViewModelAcc : BaseViewModel, ISearchAcc, IReportBuilder
    {
        public SearchViewModelAcc()
        {
            AllAccountsK = new ObservableCollection<AccountsModel>(Context.GetAllAccounts(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id));
            AllWrapedConto = new ObservableCollection<WraperConto>();
            //AllConto =
                //new ObservableCollection<Conto>(context.GetAllConto(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id, 0, 30));
            //RefreshConto(AllConto);
            DataTypeForm = DateType.IsDateIn;
            var reportItems = new List<ReportItem>();
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Запис", Width = 5 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Номер Документ", Width = 5 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Обект", Width = 5 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Папка", Width = 5 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Вид Д.", Width = 5 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Дата осчетоводяване", Width = 10 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Дебит", Width = 12 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Кредит", Width = 12 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Оборот", Width = 12, Sborno = true });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Основание", Width = 30 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Дневник", Width = 10 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Сделка", Width = 10 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Забележка", Width = 20 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Оборот валута Кт", Width = 12, Sborno = true });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Оборот валута Дт", Width = 12, Sborno = true });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Оборот количество Кт", Width = 12, Sborno = true });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Оборот количество Дт", Width = 12, Sborno = true });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Дата на документ", Width = 10 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Признак 1", Width = 10 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Признак 2", Width = 10 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Ном. Факт.", Width = 10 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Дебит Детайли", Width = 100 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Кредит Детайли", Width = 100 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Потребител", Width = 12 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Транзакция", Width = 12 });

            ReportItems = reportItems;
            CopyInterface(Entrence.Mask,this);
            if (Entrence.Mask.DebitAcc != null) _debit = Entrence.Mask.DebitAcc.ToString();
            if (Entrence.Mask.CreditAcc != null) _credit= Entrence.Mask.CreditAcc.ToString();
            Hrono = "Хронологичен регистър";
        }
        public void LoadSettings(string Path)
        {
            ReportItems = SerializeUtil.DeserializeFromXML<List<ReportItem>>(Path);
        }

        public void SaveSettings(string Path)
        {
            SerializeUtil.SerializeToXML<List<ReportItem>>(Path, ReportItems);
        }
        public int Vid { get; set;}
        public List<List<string>> GetItems()
        {
            if (TypeRep == 2)
            {
                AllConto = new ObservableCollection<Conto>(Context.GetAllContoWithoutDds(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id, Entrence.Mask,1));
                RefreshConto(AllConto);
            }
            if (TypeRep == 1)
            {
                AllConto =
                    new ObservableCollection<Conto>(Context.GetAllConto(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id,Entrence.Mask));

                RefreshConto(AllConto);
            }
            List<string> newitem = null;
            List<List<string>> result = new List<List<string>>();
            foreach (var conto in AllWrapedConto)
            {
                newitem = new List<string>();
                newitem.Add(conto.NomId.ToString());
                newitem.Add(conto.DocId);
                newitem.Add(conto.NumberObject.ToString());
                newitem.Add(conto.Folder);
                newitem.Add(conto.Kd);
                newitem.Add(conto.Data);
                var firstOrDefault = AllAccountsK.FirstOrDefault(e => e.Id == conto.CurrentConto.DebitAccount);
                newitem.Add(firstOrDefault != null ? firstOrDefault.Short.Trim(' ') : "");
                var accountsModel = AllAccountsK.FirstOrDefault(e => e.Id == conto.CurrentConto.CreditAccount);
                newitem.Add(accountsModel != null ? accountsModel.Short.Trim(' ') : "");

                newitem.Add(conto.Oborot.ToString(Vf.LevFormatUI));
                newitem.Add(conto.Reason);
                string pok = "";
                if (conto.IsPurchases)
                {
                    pok = "Пок.";
                }
                if (conto.IsSales)
                {
                    pok = "Прод.";
                }
                if (conto.IsPurchases && conto.IsSales)
                {
                    pok = "Пок./Прод.";
                }
                newitem.Add(pok);
                pok = "";
                if (conto.IsPurchases)
                {
                    pok = conto.VopPurchases;
                }
                if (conto.IsSales)
                {
                    pok = conto.VopSales;
                }
                if (conto.IsPurchases&& conto.IsSales)
                {
                    pok = string.Format("{0}/{1}", conto.VopPurchases, conto.VopSales);
                }
                 
                newitem.Add(pok);
                newitem.Add(conto.Note);
                newitem.Add(conto.Val.ToString(Vf.LevFormatUI));
                newitem.Add(conto.ValK.ToString(Vf.LevFormatUI));
                newitem.Add(conto.Kol.ToString(Vf.LevFormatUI));
                newitem.Add(conto.KolK.ToString(Vf.LevFormatUI));
                newitem.Add(conto.DataInvoise);
                newitem.Add(conto.Pr1);
                newitem.Add(conto.Pr2);
                if (!string.IsNullOrWhiteSpace(conto.DDetails))
                {
                    string fn = "";
                    var sp = conto.DDetails.Split('\n');
                    foreach (string item in sp)
                    {
                        if (item.Contains("Номер фактура - "))
                        {
                            fn = item.Replace("Номер фактура - ", "");
                        }
                    }
                    newitem.Add(fn);
                }
                else
                {
                    newitem.Add("");
                }
                newitem.Add(conto.DDetails ?? "");
                newitem.Add(conto.CDetails ?? "");
                newitem.Add(conto.UserId.ToString());
                newitem.Add(conto.Id.ToString());
                result.Add(newitem);
                //reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Запис", Width = 20 });
                //reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Номер Документ", Width = 20 });
                //reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Дебит", Width = 20 });
                //reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Кредит", Width = 20 });
                //reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Дата осчетоводяване", Width = 20 });
                //reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Оборот", Width = 20 });
                //reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Оборот валута Кт", Width = 20 });
                //reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Оборот валута Дт", Width = 20 });
                //reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Оборот количество Кт", Width = 20 });
                //reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Оборот количество Дт", Width = 20 });
                //reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Папка", Width = 20 });
                //reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Основание", Width = 20 });
                //reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Забележка", Width = 20 });
                //reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Дата на документ", Width = 20 });
                //reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Дебит Детайли", Width = 100 });
                //reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Кредит Детайли", Width = 100 });
            }
            return result;
        }
        public ObservableCollection<AccountsModel> AllAccountsK { get; set;}

        public ObservableCollection<WraperConto> AllWrapedConto { get; set; }
        
        private void RefreshConto(ObservableCollection<Conto> AllConto)
        {
            AllWrapedConto.Clear();
            foreach (Conto conto in AllConto.OrderBy(e=>e.Data.Month).ThenBy(e=>e.Nd))
            {
                var firstOrDefault = AllAccountsK.FirstOrDefault(e => e.Id == conto.DebitAccount);
                if (firstOrDefault != null)
                    conto.DName = firstOrDefault.Short;
                var cfirstOrDefault = AllAccountsK.FirstOrDefault(e => e.Id == conto.CreditAccount);
                if (cfirstOrDefault != null)
                    conto.CName = cfirstOrDefault.Short;
                AllWrapedConto.Add(new WraperConto(conto));

            }
            //var rd = new ReportDialog(this);
            //rd.ShowDialog();
            
        }

        
        private DateType _dateType;
        private byte _typeDate;
        public DateType DataTypeForm
        {
            get { return _dateType; }
            set
            {
                _dateType = value;
                if (value == DateType.IsDateDoc) _typeDate = 2;
                if (value == DateType.IsDateIn) _typeDate = 1;
                OnPropertyChanged("DataTypeForm");
            }
        }




        public WraperConto CurrentWraperConto { get; set;}

        public DateTime FromDate
        {
            get
            {
                return Entrence.Mask.FromDate;
            }

            set
            {
                Entrence.Mask.FromDate = value;
                OnPropertyChanged("FromDate");
            }
        }

        public DateTime ToDate
        {
            get
            {
                return Entrence.Mask.ToDate;
            }

            set
            {
                Entrence.Mask.ToDate = value;
                OnPropertyChanged("ToDate");
            }
        }

        private int _month;
        public int Month
        {
            get { return _month; }
            set
            {
                _month = value==0?1:value;
                FromDate = new DateTime(ConfigTempoSinglenton.GetInstance().WorkDate.Year, _month, 1);
                ToDate = new DateTime(ConfigTempoSinglenton.GetInstance().WorkDate.Year, _month, 1).AddMonths(1); OnPropertyChanged("Month");
            }
        }

        private string _numDoc;
        public string NumDoc
        {
            get { return _numDoc; }
            set { _numDoc = value;OnPropertyChanged("NumDoc"); }
        }

        protected override void Add()
        {
            //AllConto =
            //    new ObservableCollection<Conto>(Context.GetAllConto(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id,this));
            
            //RefreshConto(AllConto);
            CSearchAcc ac = new CSearchAcc();
            CopyInterface(this,ac);
            Entrence.Mask = ac;
        }
        private void CopyInterface(ISearchAcc source, ISearchAcc dest)
        {

            foreach (PropertyInfo info in typeof(ISearchAcc)
                                              .GetProperties(BindingFlags.Instance
                                                              | BindingFlags.Public))
            {
                info.SetValue(dest, info.GetValue(source, null), null);
            }
        }
        protected override void AddNew()
        {

            //AllConto =
            //    new ObservableCollection<Conto>(Context.GetAllConto(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id,Entrence.Mask));

            //RefreshConto(AllConto);
            TypeRep = 1;
            var rd = new ReportDialog(this);
            rd.ShowDialog();

        }

        protected override void Update()
        {

           //AllConto = new ObservableCollection<Conto>(Context.GetAllContoWithDds(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id, Entrence.Mask,1));

           // RefreshConto(AllConto);


        }

        protected override void Delete()
        {
            //AllConto =
            //   new ObservableCollection<Conto>(Context.GetAllContoWithoutDds(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id, Entrence.Mask,1));

            //RefreshConto(AllConto);
            TypeRep = 2;
            var rd = new ReportDialog(this);
            rd.ShowDialog();
        }

        [XmlIgnore]
        public ObservableCollection<Conto> AllConto { get; set; }

        private string _debit;
        public string Debit
        {
            get { return _debit; }
            set { _debit = value;
                DebitAcc=SetValue(value);
                OnPropertyChanged("Debit");}
        }

        public IAccNum DebitAcc { get; set;}

        private string _credit;
        public string Credit
        {
            get { return _credit; }
            set
            {
                _credit = value;
                CreditAcc=SetValue(value);
                OnPropertyChanged("Credit");
            }
        }

        public IAccNum CreditAcc { get; set; }

        private AccNum SetValue(string value)
        {
            AccNum rez=new AccNum();
            string[] split = value.Split('/');
            if (split.Length > 1)
            {
                int num;
                if (int.TryParse(split[0],out num))
                {
                    rez.Num = num;
                }
                if(int.TryParse(split[1],out num)) 
                {
                    rez.SubNum = num;
                }
                
            }
            else
            {
                if (value.Contains("*"))
                {
                    string ts= value.Replace("*", "");
                    int num;
                    if (int.TryParse(ts,out num))
                    {
                        rez.Num = num;
                        rez.SubNum = -1;
                    } 
                }
                else
                {
                    int num;
                    if (int.TryParse(value, out num))
                    {
                        rez.Num = num;
                    }
                }
            }
            if (rez.Num == 0)
            {
                return null;
            }
            return rez;
        }


        public byte TypeDate
        {
            get
            {
                return _typeDate;
            }
            set { _typeDate = value; }
        }


        private string _reason;
        public string Reason
        {
            get { return _reason; }
            set { _reason = value; OnPropertyChanged("Reason"); }
        }

        private string _note;
        public string Note
        {
            get { return _note; }
            set { _note = value;OnPropertyChanged("Note"); }
        }

        public string CDetails { get; set; }

        public string DDetails { get; set; }

       
        public List<string> GetTitles()
        {
            return ReportItems.Select(reportItem => reportItem.Name).ToList();
        }

        public List<string> GetHeader()
        {

            var ret = new List<string>();
            ret.Add(String.Format("За период           : от {0} до {1}",Entrence.Mask.FromDate.ToShortDateString(),Entrence.Mask.ToDate.ToShortDateString()));

            ret.Add(String.Format("Дата на извлечението: {0}", DateTime.Now.ToShortDateString()));
            ret.Add(String.Format("За фирма            : {0}", ConfigTempoSinglenton.GetInstance().CurrentFirma.Name));
            ret.Add(String.Format("Съставил            : {0}", Entrence.UserName));
            ret.Add(Entrence.Mask.ToString());
            return ret;
        }

        public List<string> GetFuther()
        {
            List<string> result = new List<string>();
            return result;
        }

        
        

        public string Filename
        {
            get { return "contoreport"; }
        }

        public string Title
        {
            get { return Hrono; }
        }

        public IEnumerable<ReportItem> ReportItems { get; set;}

        public List<string> GetSubTitles()
        {
            return new List<string>();
        }

        public List<List<string>> GetTXTAntetka()
        {
            return new List<List<string>>();
        }

        public void FindDebitAcc()
        {
            AccountsModel acc=null;
            if (DebitAcc != null)
            {
                if (DebitAcc.Num > 0 && DebitAcc.SubNum > -1)
                {
                    acc = AllAccountsK.FirstOrDefault(e => e.Num == DebitAcc.Num && e.SubNum == DebitAcc.SubNum);
                }
                else
                {
                    acc = AllAccountsK.FirstOrDefault(e => e.Num == DebitAcc.Num);
                }
            }
            if (acc != null)
            {
                var list = Context.LoadAllAnaliticfields(acc.Id);
                DebitItems = new ObservableCollection<INameValuePair>();
                foreach (var saldoAnaliticModel in list)
                {
                    DebitItems.Add(new NameValuePair {Name = saldoAnaliticModel.Name});
                }
                OnPropertyChanged("DebitItems");
            }

        }
        public void FindCreditAcc()
        {
            AccountsModel acc=null;
            if (CreditAcc != null)
            {
                if (CreditAcc.Num > 0 && CreditAcc.SubNum > -1)
                {
                    acc = AllAccountsK.FirstOrDefault(e => e.Num == CreditAcc.Num && e.SubNum == CreditAcc.SubNum);
                }
                else
                {
                    acc = AllAccountsK.FirstOrDefault(e => e.Num == CreditAcc.Num);
                }
            }
            if (acc != null)
            {
                var list = Context.LoadAllAnaliticfields(acc.Id);
                CreditItems = new ObservableCollection<INameValuePair>();
                foreach (var saldoAnaliticModel in list)
                {
                    CreditItems.Add(new NameValuePair {Name = saldoAnaliticModel.Name});
                }
                OnPropertyChanged("CreditItems");
            }
        }

        public IList<INameValuePair> DebitItems { get; set;}
        public IList<INameValuePair> CreditItems { get; set; }

        private string _ob;
        public string Ob
        {
            get { return _ob; }
            set { _ob = value;OnPropertyChanged("Ob"); }
        }

        private string _folder;
        private string _pr1;
        private string _pr2;

        public string Folder
        {
            get { return _folder; }
            set { _folder = value;OnPropertyChanged("Folder"); }
        }

        public void PrCh(string name)
        {
            OnPropertyChanged(name);
        }


        public string Pr1
        {
            get { return _pr1; }
            set { _pr1 = value; OnPropertyChanged("Pr1");}
        }

        public string Pr2
        {
            get { return _pr2; }
            set { _pr2 = value; OnPropertyChanged("Pr2");}
        }

        public string Hrono { get; set; }

        public string PorNom
        {
            get; set;
        }
        string _id;
        private string _userId;

        public string Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged("Id"); }
        }

        public string UserId
        {
            get { return _userId; }
            set { _userId = value; OnPropertyChanged("UserId"); }
        }

        public int TypeRep { get; private set; }
    }
}
