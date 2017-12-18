using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using ReportBuilder;
using Tempo2012.EntityFramework;
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.ViewModels;

namespace Tempo2012.UI.WPF.Views.Framework
{
    public class QuantityDialog : BaseViewModel, IReportBuilder
    {
        public QuantityDialog()
        {
            var reportItems = new List<ReportItem>();
            reportItems.Add(new ReportItem {Height = 30, IsShow = true, Name = "запис", Width = 5});
            reportItems.Add(new ReportItem {Height = 30, IsShow = true, Name = "номер", Width = 5});
            reportItems.Add(new ReportItem {Height = 30, IsShow = true, Name = "дата", Width = 10});
            reportItems.Add(new ReportItem {Height = 30, IsShow = true, Name = "папка", Width = 5});
            reportItems.Add(new ReportItem {Height = 30, IsShow = true, Name = "сч.", Width = 3});
            reportItems.Add(new ReportItem {Height = 30, IsShow = true, Name = "оборот лева", Width = 12, IsSuma = true});
            reportItems.Add(new ReportItem {Height = 30, IsShow = true, Name = "дебит сметка", Width = 12});
            reportItems.Add(new ReportItem {Height = 30, IsShow = true, Name = "кредит сметка", Width = 12});
            reportItems.Add(new ReportItem {Height = 30, IsShow = true, Name = "количество", Width = 12, IsSuma = true});
            reportItems.Add(new ReportItem {Height = 30, IsShow = true, Name = "средна цена", Width = 12});
            reportItems.Add(new ReportItem {Height = 30, IsShow = true, Name = "основание", Width = 30});
            reportItems.Add(new ReportItem {Height = 30, IsShow = true, Name = "забележка", Width = 20});
            reportItems.Add(new ReportItem {Height = 30, IsShow = true, Name = "код", Width = 10});
            reportItems.Add(new ReportItem {Height = 30, IsShow = true, Name = "клиент", Width = 40});
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "потребител", Width = 12 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "транзакция", Width = 15});
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
            List<QuantityModel> contos =new List<QuantityModel>(Context.GetAllContoQuantity(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id,CurrenAcc.Id,FromDate,ToDate,1,KindStock));
            List<QuantityModel> contos1=new List<QuantityModel>(Context.GetAllContoQuantity(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id, CurrenAcc.Id, FromDate, ToDate, 2, KindStock));

            decimal sumad = 0,sumac = 0;
            decimal sumacold = 0, sumacolc = 0;
            decimal sumamd = 0, sumamc = 0;
            decimal sumacolmd = 0, sumacolmc = 0;

            sumad =contos.Sum(e=>e.Oborot);
            sumac=contos1.Sum(e=>e.Oborot);
            decimal sumaquantityd = contos.Sum(e=>e.Quantity);
            decimal sumaquantityc = contos1.Sum(e => e.Quantity);
            //decimal sumasinglepriced=contos1.Sum(e=>e.SinglePrice);
            //decimal sumasinglepricec = contos1.Sum(e => e.SinglePrice);
            string currec = "", oldrec = "";
            string lastcode="", lastname="";
            contos.AddRange(contos1);

            foreach (
                var co in
                    contos.OrderBy(e=>e.StockCode))
            {
                if (oldrec == "")
                {
                    oldrec = co.StockCode;
                }
                currec = co.StockCode;
                if (oldrec != currec)
                {
                    NewRow(items);
                    var item1 = new List<string>();
                    item1.Add("");//запис
                    item1.Add("");//номер
                    item1.Add("ОД");//дата
                    item1.Add("");//папка
                    item1.Add("");//сч
                    item1.Add("ОК");//оборот
                    item1.Add("");//дебит сметка
                    item1.Add("");//кредит сметка
                    item1.Add("");//средна цена
                    item1.Add("");//количество
                    item1.Add("");//основание
                    item1.Add("");//забележка
                    item1.Add(lastcode);//код
                    item1.Add(lastname);//име
                    item1.Add("");//потребител
                    item1.Add("");//транзакция
                    items.Add(item1);

                    item1 = new List<string>();
                    item1.Add("");//запис
                    item1.Add("Сума");//номер
                    item1.Add(string.Format(Vf.LevFormat, sumamd));//дата
                    item1.Add("");//папка
                    item1.Add("");//сч
                    item1.Add(string.Format(Vf.LevFormat, sumamc));//оборот
                    item1.Add("");//дебит сметка
                    item1.Add("");//кредит сметка
                    item1.Add("");//средна цена
                    item1.Add("");//количество
                    item1.Add("");//основание
                    item1.Add("");//забележка
                    item1.Add(lastcode);//код
                    item1.Add(lastname);//име
                    item1.Add("");//потребител
                    item1.Add("");//транзакция
                    items.Add(item1);

                    item1 = new List<string>();
                    item1.Add("");//запис
                    item1.Add("Кол.");//номер
                    item1.Add(string.Format(Vf.LevFormat, sumacolmd));//дата
                    item1.Add("");//папка
                    item1.Add("");//сч
                    item1.Add(string.Format(Vf.LevFormat, sumacolmc));//оборот
                    item1.Add("");//дебит сметка
                    item1.Add("");//кредит сметка
                    item1.Add("");//средна цена
                    item1.Add("");//количество
                    item1.Add("");//основание
                    item1.Add("");//забележка
                    item1.Add(lastcode);//код
                    item1.Add(lastname);//име
                    item1.Add("");//потребител
                    item1.Add("");//транзакция
                    items.Add(item1);

                    NewRow(items);
                    sumamd = 0;
                    sumacolmd = 0;
                    sumamc = 0;
                    sumacolmc = 0;
                    oldrec = currec;
                }
                List<string> item2 = new List<string>();
                item2.Add(co.PorNom);
                item2.Add(co.DocNum);
                item2.Add(co.Data);
                item2.Add(co.Folder);
                item2.Add(co.User);
                item2.Add(string.Format(Vf.LevFormat, co.Oborot));
                var dac = Allacc.FirstOrDefault(e => e.Id == co.DebitAccount);
                if (dac != null) item2.Add(dac.Short);
                dac = Allacc.FirstOrDefault(e => e.Id == co.CreditAccount);
                if (dac != null) item2.Add(dac.Short);
                item2.Add(string.Format(Vf.ValFormat, co.Quantity));
                item2.Add(string.Format(Vf.KolFormat, co.SinglePrice));
                item2.Add(co.Reason);
                item2.Add(co.Note);
                item2.Add(co.StockCode);lastcode = co.StockCode;
                item2.Add(co.Stock);lastname = co.Stock;
                item2.Add(co.User);
                item2.Add(co.Id);
                items.Add(item2);
                if (co.IsDebit)
                {
                    sumamd += co.Oborot;
                    sumacolmd += co.Quantity;
                }
                else
                {
                    sumamc += co.Oborot;
                    sumacolmc += co.Quantity;
                }

            }
            NewRow(items);
            var item = new List<string>();
            item.Add("");//запис
            item.Add("");//номер
            item.Add("ОД");//дата
            item.Add("");//папка
            item.Add("");//сч
            item.Add("ОК");//оборот
            item.Add("");//дебит сметка
            item.Add("");//кредит сметка
            item.Add("");//средна цена
            item.Add("");//количество
            item.Add("");//основание
            item.Add("");//забележка
            item.Add("");//код
            item.Add("");//име
            item.Add("");//потребител
            item.Add("");//транзакция
            items.Add(item);

            item = new List<string>();
            item.Add("");//запис
            item.Add("Сума");//номер
            item.Add(string.Format(Vf.LevFormat, sumamd));//дата
            item.Add("");//папка
            item.Add("");//сч
            item.Add(string.Format(Vf.LevFormat, sumamc));//оборот
            item.Add("");//дебит сметка
            item.Add("");//кредит сметка
            item.Add("");//средна цена
            item.Add("");//количество
            item.Add("");//основание
            item.Add("");//забележка
            item.Add("");//код
            item.Add("");//име
            item.Add("");//потребител
            item.Add("");//транзакция
            items.Add(item);

            item = new List<string>();
            item.Add("");//запис
            item.Add("Кол.");//номер
            item.Add(string.Format(Vf.LevFormat, sumacolmd));//дата
            item.Add("");//папка
            item.Add("");//сч
            item.Add(string.Format(Vf.LevFormat, sumacolmc));//оборот
            item.Add("");//дебит сметка
            item.Add("");//кредит сметка
            item.Add("");//средна цена
            item.Add("");//количество
            item.Add("");//основание
            item.Add("");//забележка
            item.Add("");//код
            item.Add("");//име
            item.Add("");//потребител
            item.Add("");//транзакция
            items.Add(item);
            NewRow(items);
            decimal nsd = 0;
            decimal nsc = 0;
            decimal nsdv = 0;
            decimal nscv = 0;
            if (FromDate.Month > 1)
            {
                var begcontosd =
                  new List<QuantityModel>(Context.GetAllContoQuantity(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id, CurrenAcc.Id, new DateTime(FromDate.Year,1,1), FromDate,  1,KindStock));
                var begcontosc =
                  new List<QuantityModel>(Context.GetAllContoQuantity(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id, CurrenAcc.Id, new DateTime(FromDate.Year, 1, 1), FromDate, 2,KindStock));
                nsd = begcontosd.Sum(e => e.Oborot);
                nsc = begcontosc.Sum(e => e.Oborot);
                //nsdv = begcontosd.Sum(e => e.ValSum);
                //nscv = begcontosc.Sum(e => e.ValSum);
            }
            if (CurrenAcc.TypeAccount == 1)
            {
                if (FromDate.Month == 1)
                {
                    BeginSaldoD = CurrenAcc.BeginSaldoL;
                    BeginValSd = CurrenAcc.BeginSaldoK;
                }
                else
                {
                    BeginSaldoD = (nsd + CurrenAcc.BeginSaldoL) - (nsc);
                    BeginValSd = ((nsdv + CurrenAcc.BeginSaldoK) - (nscv));
                }
               
            }
            if (CurrenAcc.TypeAccount == 2)
            {
                if (FromDate.Month == 1)
                {
                    BeginSaldoK = CurrenAcc.BeginSaldoL;
                    BeginValSc = CurrenAcc.BeginSaldoK;
                }
                else
                { 
                    BeginSaldoK = (nsc + CurrenAcc.BeginSaldoL) - (nsd);
                    BeginValSc = ((nscv + CurrenAcc.BeginSaldoK) - (nsdv));
                }
            }
            OborotsDebit = string.Format(Vf.LevFormat, sumad);
            OborotsCredit = string.Format(Vf.LevFormat, sumac);
            Sumavald = string.Format(Vf.KolFormat, sumaquantityd);
            Sumavalc = string.Format(Vf.KolFormat, sumaquantityc);
            //Sumavald = string.Format(Vf.ValFormat, sumavald);
            //Sumavalc = string.Format(Vf.ValFormat, sumavalc);
            //KursDifd=string.Format(Vf.LevFormat, sumavalddf);
            //KursDifc=string.Format(Vf.LevFormat, sumavalcdf);
           

            TotalD = CurrenAcc.TypeAccount == 1
                ? string.Format(Vf.LevFormat, sumad + BeginSaldoD)
                : string.Format(Vf.LevFormat, sumad);
            TotalC = CurrenAcc.TypeAccount == 2
                ? string.Format(Vf.LevFormat, sumac + BeginSaldoK)
                : string.Format(Vf.LevFormat, sumac);
            if (CurrenAcc.TypeAccount == 1)
            {
                KrainoSaldoD = (sumad + BeginSaldoD) - (sumac);
                KrainoSaldoDV = (sumaquantityd + BeginSaldoD) - (sumaquantityc);
                KrainoSaldoKV = 0;
                //Sad = string.Format(Vf.KursFormat, KrainoSaldoDV != 0 ? KrainoSaldoD / KrainoSaldoDV : 0);
                //Sak ="";
            }
            if (CurrenAcc.TypeAccount == 2)
            {
                KrainoSaldoK = (sumac + BeginSaldoK) - (sumad);
                KrainoSaldoKV = (sumaquantityc + BeginValSc) - (sumaquantityd);
                KrainoSaldoDV = 0;
                //Sad = "";
                //Sak = string.Format(Vf.KursFormat, KrainoSaldoKV != 0 ? KrainoSaldoK / KrainoSaldoKV : 0);
            }
            return items;
        }

        private static void NewRow(List<List<string>> items)
        {
            var item = new List<string>();
            item.Add("------------");                                              //запис
            item.Add("------------");                                              //номер
            item.Add("------------");                                              //дата
            item.Add("------------");                                              //папка
            item.Add("------------");                                              //сч
            item.Add("------------");                                              //оборот
            item.Add("------------");                                              //дебит сметка
            item.Add("------------");                                              //кредит сметка
            item.Add("------------");                                              //средна цена
            item.Add("------------");                                              //количество
            item.Add("-----------------------------");                             //основание
            item.Add("-----------------------------");                             //забележка
            item.Add("------------");                                              //код
            item.Add("-------------------------------------------------------");   //име
            item.Add("------------");                                              //потребител
            item.Add("------------");                                              //транзакция
            items.Add(item);                                                        
        }

        public decimal KrainoSaldoK { get; set; }

        public decimal KrainoSaldoD { get; set; }

        public decimal KrainoSaldoKV { get; set; }

        public decimal KrainoSaldoDV { get; set; }

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
            list.Add(string.Format("за сметка                 : {0}", CurrenAcc));
            list.Add(string.Format("за период                 : {0} до {1}", FromDate.ToShortDateString(),
                ToDate.ToShortDateString()));
            list.Add(string.Format("Дата на извлечението      : {0}", DateTime.Now.ToShortDateString()));
            list.Add(string.Format("за фирма                  : {0}",
                ConfigTempoSinglenton.GetInstance().CurrentFirma.Name));
            list.Add(string.Format("Счетоводител              : {0}", Config.CurrentUser.Name));
            list.Add(string.Format("за папка                  : {0}", Folder));
            list.Add(string.Format("за обекта                 : {0}", ObjectName));
            //list.Add(string.Format("вид валута                : {0}", VidVal));
            if (!string.IsNullOrWhiteSpace(KindStock))
            list.Add(string.Format("                    : {0} {1}",KindStock,Stock));
            return list;
        }

        public List<string> GetFuther()
        {
            List<string> list = new List<string>();
            list.Add("--------------------------------------------------------------------");
            list.Add("| Параметри на сметката       |    дебит         |   кредит        |");
            list.Add("--------------------------------------------------------------------");
            list.Add(string.Format("| Начални салда               |{0,17}|{1,18}|",
                CurrenAcc.TypeAccount == 1 ? BeginSaldoD.ToString(Vf.LevFormatUI) : "",
                CurrenAcc.TypeAccount == 2 ? BeginSaldoK.ToString(Vf.LevFormatUI) : ""));
            list.Add(string.Format("| Oбороти                     |{0,17}|{1,18}|", OborotsDebit, OborotsCredit));
            list.Add(string.Format("| Сборове                     |{0,17}|{1,18}|", TotalD, TotalC));
            list.Add(string.Format("| Крайни салда                |{0,17}|{1,18}|",
                CurrenAcc.TypeAccount == 1 ? KrainoSaldoD.ToString(Vf.LevFormatUI) : "",
                CurrenAcc.TypeAccount == 2 ? KrainoSaldoK.ToString(Vf.LevFormatUI) : ""));
            list.Add("--------------------------------------------------------------------");
            list.Add("|Количество                   ||||||||||||||||||||||||||||||||||||||");
            list.Add("--------------------------------------------------------------------");
            list.Add(string.Format("| Начално  салдо              |{0,17}|{1,18}|",BeginValSd,BeginValSc));
            list.Add(string.Format("| Обороти                     |{0,17}|{1,18}|",Sumavald,Sumavalc)); 
            list.Add(string.Format("| Сборове                     |{0,17}|{1,18}|",Sumavald, Sumavalc)); 
            list.Add(string.Format("| Крайно салдо                |{0,17}|{1,18}|",KrainoSaldoDV.ToString(Vf.KursFormatUI), KrainoSaldoKV.ToString(Vf.KursFormatUI)));
            list.Add("--------------------------------------------------------------------");
            return list;
        }

        public string Filename
        {
            get { return "analitics"; }
        }

        public string Title
        {
            get { return "AНАЛИТИЧЕН   РЕГИСТЪР - справка стоки"; }
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

        public string VidVal { get; set; }

        public string Sumavalc { get; set; }
               
        public string Sumavald { get; set; }
             
        public string KursDifd { get; set; }
               
        public string KursDifc { get; set; }
               
        public string SrednoPretd { get; set; }
               
        public string SrednoAritmd { get; set; }
               
        public string SrednoAritmOpd { get; set; }
               
        public string SrednoPretc { get; set; }
               
        public string SrednoAritmc { get; set; }
               
        public string SrednoAritmOpc { get; set; }
               
        public decimal BeginValSd { get; set; }
               
        public decimal BeginValSc { get; set; }

        public string Oborotsvald { get; set; }

        public string Oborotsvalc { get; set; }
        
        public string Sad { get; set; }

        public string Sak { get; set; }
        public string KindStock { get; set; }
        public string Stock { get; set; }
    }
}
