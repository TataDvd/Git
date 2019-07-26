using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ReportBuilder;
using Tempo2012.UI.WPF.Models;
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.ViewModels;
using System.Linq;
using Tempo2012.EntityFramework;
using Tempo2012.EntityFramework.Interface;
using Tempo2012.UI.WPF.ViewModels.SearchFormNS;
using Microsoft.Win32;

namespace Tempo2012.UI.WPF.Views.AccountRegisters
{
    internal class AnaliticRegisterViewModelContr : BaseViewModel, IReportBuilder
    {
        public Dictionary<int, List<string>> Rowfoother { get; set; }
        public AnaliticRegisterViewModelContr()
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
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "транзакция", Width = 12 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "фактура", Width = 12 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "ДДС Сума", Width = 15 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Обща сума", Width = 15 });
            ReportItems = reportItems;
            Allacc = new List<AccountsModel>(Context.GetAllAccounts(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id));

        }
        public void LoadSettings(string Path)
        {
            ReportItems = SerializeUtil.DeserializeFromXML<List<ReportItem>>(Path);
        }

        public void SaveSettings(string Path)
        {
            SerializeUtil.SerializeToXML<List<ReportItem>>(Path, ReportItems);
        }
        public AccountsModel CurrenAcc { get; set; }

        private List<AccountsModel> Allacc;
        public List<List<string>> GetItems()
        {

            List<List<string>> items = new List<List<string>>();
            List<Conto> contos0 = new List<Conto>(Context.GetAllContoWithDdsAndNot(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id, Entrence.Mask,ItemsDebit[0].Value,"17"));
            //Контрагент - 4 АНИ - ЕКС ООД
            var di = new List<INameValuePair>();
            foreach (var saldoAnaliticModel in ItemsDebit)
            {
                di.Add(new NameValuePair { Name = "Контрагент", Value=saldoAnaliticModel.Value});
            }
            var old = Entrence.Mask.DebitItems;
            Entrence.Mask.DebitItems = di;
            List<Conto> contos = new List<Conto>(Context.GetContosByContragent(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id, Entrence.Mask.FromDate, Entrence.Mask.ToDate, di[0].Value, "17"));
            Entrence.Mask.DebitItems = old;
            
            decimal sumad = 0, sumac = 0;
            var res = contos; 
            //if (ItemsDebit != null)
            //{
            //    foreach (SaldoItem item in ItemsDebit)
            //    {
            //        if (item.Name.Contains("Дата")) continue;
            //        if (!string.IsNullOrWhiteSpace(item.Value) && item.Value != "0" && item.Value != "0.00")
            //        {
            //            List<Conto> contoros = new List<Conto>(res);
            //            res = contoros.Where(e => e.DDetails.Contains(string.Format(" {0} {1} ", item.Value,item.Lookupval)) || e.CDetails.Contains(string.Format(" {0} {1} ", item.Value,item.Lookupval)) || e.ClientNumDds==item.Value).ToList();
            //        }
            //    }
            //}

            res = new List<Conto>(contos0.Union(res,new ContoComparer()));
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
                item.Add(string.Format(Vf.LevFormat, co.Oborot));
                var dac = Allacc.FirstOrDefault(e => e.Id == co.DebitAccount);
                if (dac != null) item.Add(dac.Short);
                dac = Allacc.FirstOrDefault(e => e.Id == co.CreditAccount);
                if (dac != null) item.Add(dac.Short);
                item.Add(co.Reason);
                item.Add(co.Note);
                string pok = "";
                if (co.IsPurchases == 1)
                {
                    pok = "Пок.";
                }
                if (co.IsSales == 1)
                {
                    pok = "Прод.";
                }
                if (co.IsPurchases == 1 && co.IsSales == 1)
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
                    pok = string.Format("{0}/{1}", co.VopPurchases, co.VopSales);
                }
                item.Add(pok);
                item.Add(co.Id.ToString());
                co.DName = Entrence.GetItemFromDetails(co.DDetails, co.CDetails, "Номер фактура", co.DName);
                item.Add(co.DName);
                item.Add(co.SumDds.ToString());
                item.Add(co.Sum.ToString());
                items.Add(item);
                if (co.DebitAccount == CurrenAcc.Id)
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
            Entrence.Mask.ToDate = oldDateTimeF;
            Entrence.Mask.FromDate = new DateTime(FromDate.Year, 1, 1);
            contos = new List<Conto>(Context.GetAllConto(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id, Entrence.Mask));
            Entrence.Mask.FromDate = oldDateTimeF;
            Entrence.Mask.ToDate = oldDateTimeT;
              
            OborotsDebit = string.Format(Vf.LevFormat, sumad);
            OborotsCredit = string.Format(Vf.LevFormat, sumac);
            if (CurrenAcc != null)
            {
                var rezi = Context.GetAllAnaliticSaldos(CurrenAcc.Id, ConfigTempoSinglenton.GetInstance().CurrentFirma.Id);
                BeginSaldoD = rezi.Where(e => e.Code == ItemsDebit[0].Value).Sum(e=>e.BeginSaldoDebit);
                BeginSaldoK = rezi.Where(e => e.Code == ItemsDebit[0].Value).Sum(e=>e.BeginSaldoCredit);
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
            List<string> list = new List<string>();
            list.Add(string.Format("за                        : {0}", Currentfilter));
            list.Add(string.Format("за период                 : {0} до {1}", FromDate.ToShortDateString(), ToDate.ToShortDateString()));
            list.Add(string.Format("Дата на извлечението      : {0}", DateTime.Now.ToShortDateString()));
            list.Add(string.Format("за фирма                  : {0}", ConfigTempoSinglenton.GetInstance().CurrentFirma.Name));
            list.Add(string.Format("Счетоводител              : {0}", Config.CurrentUser.Name));
            list.Add(string.Format("за папка                  : {0}", Folder));
            list.Add(string.Format("за обекта                 : {0}", ObjectName));
            return list;
        }

        public List<string> GetFuther()
        {
            List<string> list = new List<string>();
            list.Add("-------------------------------------------------------------------------");
            list.Add("| Параметри                   |    дебит           |        кредит      |");
            list.Add("-------------------------------------------------------------------------");
            list.Add(string.Format("| Началнo салдo  по сметка    |{0,20}|{1,20}|", CurrenAcc.TypeAccount == 1 ? (BeginSaldoD-BeginSaldoK).ToString() : "", CurrenAcc.TypeAccount == 2 ? (BeginSaldoK-BeginSaldoD).ToString() : ""));
            list.Add(string.Format("| {0} към {1}", CurrenAcc.ShortName,new DateTime(FromDate.Year,1,1).ToShortDateString()));
            list.Add(string.Format("| Oбороти                     |{0,20}|{1,20}|", OborotsDebit, OborotsCredit));
            //list.Add(string.Format("| Сборове                     |{0,15}|{1,15}|", TotalD, TotalC));
            //list.Add(string.Format("| Крайни салда                |{0,15}|{1,15}|", CurrenAcc.TypeAccount == 1 ? KrainoSaldoD.ToString(Vf.LevFormatUI) : "", CurrenAcc.TypeAccount == 2 ? KrainoSaldoK.ToString(Vf.LevFormatUI) : ""));
            list.Add("-------------------------------------------------------------------------");
            return list;
        }

        public string Filename
        {
            get
            {
                return "analitics";
            }
        }

        public string Title
        {
            get;set;// { return "AНАЛИТИЧЕН   РЕГИСТЪР"; }
        }
        public string SubTitle
        {
            get; set;
        }
        public IEnumerable<ReportItem> ReportItems { get; set; }

        public List<string> GetSubTitles()
        {
            return null;
        }

        public List<List<string>> GetTXTAntetka()
        {
            return null;
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



        public ObservableCollection<Models.SaldoItem> ItemsDebit { get; set; }
        public string Currentfilter
        {
            get
            {
                string str = "";
                if (ItemsDebit != null && ItemsDebit.Count > 0)
                {
                    foreach (SaldoItem item in ItemsDebit)
                    {
                        if (item.Name.Contains("Дата")) continue;
                        if (!string.IsNullOrWhiteSpace(item.Value) && item.Value != "0" && item.Value != "0.00")
                        {
                            str = string.Format("{1} {2} {3} {0}", str, item.Name, item.Value, item.Lookupval);
                        }
                    }
                }
                return str;
            }
        }

        
    }
}