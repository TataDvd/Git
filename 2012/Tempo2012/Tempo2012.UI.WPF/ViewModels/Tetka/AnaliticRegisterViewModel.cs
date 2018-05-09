using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using ReportBuilder;
using Tempo2012.EntityFramework;
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.Models;
using Tempo2012.UI.WPF.ViewModels.SearchFormNS;
using Tempo2012.EntityFramework.Interface;
using Microsoft.Win32;

namespace Tempo2012.UI.WPF.ViewModels.Tetka
{
    public class AnaliticRegisterViewModel : BaseViewModel, IReportBuilder
    {
        public Dictionary<int, List<string>> Rowfoother { get; set; }
        public AnaliticRegisterViewModel()
        {
            var reportItems = new List<ReportItem>();
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "запис", Width = 5 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "номер", Width = 5 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "дата", Width = 10 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "папка", Width = 5 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "вид д.", Width = 5 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "сч.", Width = 3 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "обект", Width = 5 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "дата док.", Width = 10 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "оборот", Width = 12, IsSuma = true });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "дебит сметка", Width = 12 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "кредит сметка", Width = 12 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "основание", Width = 30 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "забележка", Width = 20 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "дневник", Width = 10 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "сделка", Width = 10 });
            reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "транзакция", Width = 10 });
            ReportItems = reportItems;
            Allacc = new List<AccountsModel>(Context.GetAllAccounts(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id));
            
        }

        public AccountsModel CurrenAcc { get; set;}
        public string Antetka { get {
                string a="";
                foreach (SaldoItem item in ItemsDebit)
                {
                    if (item.Name.Contains("Дата")) continue;
                    if (!string.IsNullOrWhiteSpace(item.Value) && item.Value != "0" && item.Value != "0.00")
                    {
                        a+= string.Format(" {0} - {1}:{2} ", item.Value, item.Lookupval,a);
                    }
                }
                return a;        
             } }
        private List<AccountsModel> Allacc;
        public List<List<string>> GetItems()
        {
            bool fullsaldo = true;
            string contr="";
            List<List<string>> items=new List<List<string>>();
            if (Entrence.Mask.DebitAcc != null)
            {
                Entrence.Mask.DebitAcc.Num = CurrenAcc.Id;
            }
            else
            {
                Entrence.Mask.DebitAcc = new AccNum();
                Entrence.Mask.DebitAcc.Num = CurrenAcc.Id;
            }
            List<Conto> contos=new List<Conto>(Context.GetAllContoOrfiltered(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id,Entrence.Mask));
            decimal sumad=0, sumac=0;
            string contain="";
            var res=contos.Where(
                e =>
                    (e.DebitAccount == CurrenAcc.Id || e.CreditAccount == CurrenAcc.Id));
            if (ItemsDebit != null)
            {
                foreach (SaldoItem item in ItemsDebit)
                {
                    if (item.Name.Contains("Дата")) continue;
                    if (!string.IsNullOrWhiteSpace(item.Value)&&item.Value!="0"&&item.Value!="0.00")
                    {
                        List<Conto> contoros=new List<Conto>(res);
                        res = contoros.Where(e => e.DDetails.Contains(string.Format("{0} - {1} ",item.Name, item.Value))||e.CDetails.Contains(string.Format("{0} - {1} ",item.Name, item.Value))).ToList();
                        if (item.Name=="Контрагент")
                        {
                            contr=item.Lookupval;
                            contain=string.Format("{0} - {1} ",item.Name, item.Value);
                        }
                        fullsaldo = false;
                    }
                }
            }
            foreach (var co in res.OrderBy(e => e.Data))
            {
                List<string> item = new List<string>();
                item.Add(co.Nd.ToString());
                item.Add(co.DocNum);
                item.Add(co.Data.ToShortDateString());
                item.Add(co.Folder);
                item.Add(co.KD);
                item.Add(co.UserId.ToString());
                item.Add(co.NumberObject.ToString());
                item.Add(co.DataInvoise.ToShortDateString());
                item.Add(string.Format(Vf.LevFormat,co.Oborot));
                var dac = Allacc.FirstOrDefault(e => e.Id == co.DebitAccount);
                if (dac != null) item.Add(dac.Short);
                dac = Allacc.FirstOrDefault(e => e.Id == co.CreditAccount);
                if (dac != null) item.Add(dac.Short);
                item.Add(co.Reason);
                item.Add(co.Note);
                string pok = "";
                if (co.IsPurchases==1)
                {
                    pok = "Пок.";
                }
                if (co.IsSales==1)
                {
                    pok = "Прод.";
                }
                if (co.IsPurchases==1 && co.IsSales==1)
                {
                    pok = "Пок./Прод.";
                }
                item.Add(pok);
                 pok = "";
                if (co.IsPurchases == 1)
                {
                    pok = co.VopPurchases;
                }
                if (co.IsSales == 1)
                {
                    pok = co.VopSales;
                }
                if (co.IsPurchases == 1 && co.IsSales == 1)
                {
                    pok = string.Format("{0}/{1}",co.VopPurchases,co.VopSales);
                }
                item.Add(pok);
                item.Add(co.Id.ToString()); 
                items.Add(item);
                if (co.DebitAccount==CurrenAcc.Id)
                {
                    sumad += co.Oborot;
                }
                else
                {
                    sumac += co.Oborot;
                }
                
            }
            DateTime oldDateTimeF = Entrence.Mask.FromDate;
            DateTime oldDateTimeT = Entrence.Mask.ToDate;
            decimal sumadb = 0, sumacb = 0;
            //if (FromDate.Month > 1)
            //{
                Entrence.Mask.ToDate = oldDateTimeF;
                Entrence.Mask.FromDate = new DateTime(FromDate.Year, 1, 1);
                contos = new List<Conto>(Context.GetAllContoOrfiltered(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id, Entrence.Mask));
                Entrence.Mask.FromDate = oldDateTimeF;
                Entrence.Mask.ToDate = oldDateTimeT;
               
                if (!fullsaldo)
                {
                    foreach (
                        var co in
                            contos.Where(
                                e =>
                                    (e.DebitAccount == CurrenAcc.Id || e.CreditAccount == CurrenAcc.Id) && e.Data < oldDateTimeF && (e.CDetails.Contains(contain) || e.DDetails.Contains(contain))))
                    {
                        if (co.DebitAccount == CurrenAcc.Id)
                        {
                            sumadb += co.Oborot;
                        }
                        else
                        {
                            sumacb += co.Oborot;
                        }
                    }


                }
                else
                {
                    foreach (
                           var co in
                               contos.Where(
                                   e =>
                                       (e.DebitAccount == CurrenAcc.Id || e.CreditAccount == CurrenAcc.Id) && e.Data < oldDateTimeF))
                    {
                        if (co.DebitAccount == CurrenAcc.Id)
                        {
                            sumadb += co.Oborot;
                        }
                        else
                        {
                            sumacb += co.Oborot;
                        }
                    }
                }
            //}
            if (!fullsaldo)
            {
                var rezi = Context.GetAllAnaliticSaldos(CurrenAcc.Id, CurrenAcc.FirmaId);
                var saldo = rezi.FirstOrDefault(e => e.NameContragent == contr);
                if (saldo != null)
                {
                    CurrenAcc.SaldoDL = saldo.BeginSaldoDebit;
                    CurrenAcc.SaldoKL= saldo.BeginSaldoCredit;
                }
                else
                {
                    CurrenAcc.SaldoDL = 0;
                    CurrenAcc.SaldoKL = 0;
                }

            }
            if (CurrenAcc.TypeAccount == 1)
            {
                BeginSaldoD = (sumadb + CurrenAcc.BeginSaldoL) - (sumacb);
            }
            if (CurrenAcc.TypeAccount == 2)
            {
                BeginSaldoK = (sumacb + CurrenAcc.BeginSaldoL) - (sumadb);
            } 
            OborotsDebit = string.Format(Vf.LevFormat, sumad);
            OborotsCredit = string.Format(Vf.LevFormat, sumac);
            TotalD = CurrenAcc.TypeAccount == 1 ? string.Format(Vf.LevFormat, sumad + BeginSaldoD) : string.Format(Vf.LevFormat, sumad);
            TotalC = CurrenAcc.TypeAccount == 2 ? string.Format(Vf.LevFormat, sumac + BeginSaldoK) : string.Format(Vf.LevFormat, sumac);
            if (CurrenAcc.TypeAccount == 1)
            {
                KrainoSaldoD = (sumad + BeginSaldoD) - (sumac);
            }
            if (CurrenAcc.TypeAccount == 2)
            {
                KrainoSaldoK = (sumac + BeginSaldoK) - (sumad);
            } 
            return items;
        }

        public decimal KrainoSaldoK { get; set; }

        public decimal KrainoSaldoD { get; set; }

        public decimal BeginSaldoD { get; set; }

        public decimal BeginSaldoK { get; set; }

        public string NsaldoDebit { get; set; }

        public string NsaldoCredit { get; set; }

        public List<string> GetTitles()
        {
            return ReportItems.Select(reportItem => reportItem.Name).ToList();
        }

        public List<string> GetHeader()
        {
            List<string> list=new List<string>();
            list.Add(string.Format("за сметка                 : {0} {1}",CurrenAcc,Antetka));
            list.Add(string.Format("за период                 : {0} до {1}",FromDate.ToShortDateString(),ToDate.ToShortDateString()));
            list.Add(string.Format("Дата на извлечението      : {0}",DateTime.Now.ToShortDateString()));
            list.Add(string.Format("за фирма                  : {0}",ConfigTempoSinglenton.GetInstance().CurrentFirma.Name));
            list.Add(string.Format("Счетоводител              : {0}",Config.CurrentUser.Name)); 
            list.Add(string.Format("за папка                  : {0}",Folder));
            list.Add(string.Format("за обекта                 : {0}",ObjectName));
            return list;
        }

        public List<string> GetFuther()
        {
            List<string> list = new List<string>();
                          list.Add("---------------------------------------------------------------");
                          list.Add("| Параметри на сметката       |    дебит      |   кредит      |");
                          list.Add("---------------------------------------------------------------");
                          list.Add(string.Format("| Начални салда               |{0,15}|{1,15}|", CurrenAcc.TypeAccount == 1 ? BeginSaldoD.ToString(Vf.LevFormatUI) : "", CurrenAcc.TypeAccount == 2 ? BeginSaldoK.ToString(Vf.LevFormatUI) : ""));
            list.Add(string.Format("| Oбороти                     |{0,15}|{1,15}|", OborotsDebit, OborotsCredit));
            list.Add(string.Format("| Сборове                     |{0,15}|{1,15}|", TotalD, TotalC));
            list.Add(string.Format("| Крайни салда                |{0,15}|{1,15}|", CurrenAcc.TypeAccount == 1 ? KrainoSaldoD.ToString(Vf.LevFormatUI) : "", CurrenAcc.TypeAccount == 2 ? KrainoSaldoK.ToString(Vf.LevFormatUI) : ""));
                          list.Add("---------------------------------------------------------------");
            return list;
        }

        public string Filename
        {
            get {
                return "analitics";
            }
        }

        public string Title
        {
            get { return "AНАЛИТИЧЕН   РЕГИСТЪР"; }
        }

        public IEnumerable<ReportItem> ReportItems { get ; set; }

        public List<string> GetSubTitles()
        {
            return null;
        }

        public List<List<string>> GetTXTAntetka()
        {
            return null;
        }

        public void LoadSettings(string Path)
        {
            ReportItems = SerializeUtil.DeserializeFromXML<List<ReportItem>>(Path);
        }

        public void SaveSettings(string Path)
        {
            SerializeUtil.SerializeToXML<List<ReportItem>>(Path, ReportItems);
        }

        public int Month { get; set; }

        public int Year { get; set; }

        public string FirmaName { get; set; }

        public string Folder { get; set; }

        public string ObjectName { get; set; }

        public string OborotsDebit { get; set; }

        public string OborotsCredit { get; set; }

        public string TotalD { get; set; }

        public string TotalC { get; set; }

        public string EndSaldoD { get; set; }

        public string EndSaldoC { get; set; }

        public string BeginSD { get; set; }

        public string BeginSK { get; set; }

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

        private string _accShortName;
        public string AccShortName
        {
            get { return _accShortName; }
            set
            {
                _accShortName = value;
                if (!value.Contains("/"))
                {
                    int num;
                    if (int.TryParse(value, out num))
                    {
                        CurrenAcc = Allacc.FirstOrDefault(e => e.Num == num && e.SubNum == 0);
                    }

                }
                else
                {
                    int num, subnum;
                    var ac = value.Split('/');

                    if (int.TryParse(ac[0], out num) && int.TryParse(ac[1], out subnum))
                    {
                        CurrenAcc = Allacc.FirstOrDefault(e => e.Num == num && e.SubNum == subnum);
                    }
                }
            }
        }

        public ObservableCollection<Models.SaldoItem> ItemsDebit { get; set; }
    }
}
