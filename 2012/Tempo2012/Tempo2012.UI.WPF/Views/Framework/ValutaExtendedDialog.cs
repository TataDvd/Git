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
    public class ValutaExtendedDialog : BaseViewModel, IReportBuilder
    {
        public Dictionary<int, List<string>> Rowfoother { get; set; }
        public ValutaExtendedDialog()
        {
            var reportItems = new List<ReportItem>();
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "запис", Width = 5 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "номер", Width = 5 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "дата", Width = 10 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "папка", Width = 5 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "сч.", Width = 3 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "вид валута", Width = 10 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "оборот лева", Width = 12, IsSuma = true });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "дебит сметка", Width = 12 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "кредит сметка", Width = 12 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "сума валута", Width = 12, IsSuma = true });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "търг. курс", Width = 12 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "oпoрeн. курс", Width = 12 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "курс. разл.", Width = 12 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "основание", Width = 30 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "забележка", Width = 20 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Признак 1", Width = 10 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Признак 2", Width = 10 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "код", Width = 10 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "клиент", Width = 40 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "номер фактура", Width = 20 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "дата фактура", Width = 20 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "потребител", Width = 12 });
            reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "транзакция", Width = 15 });
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
            Rowfoother = new Dictionary<int, List<string>>();
            List<List<string>> items = new List<List<string>>();
            List<ValutaControl> contos = new List<ValutaControl>(Context.GetAllContoValuta(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id, CurrenAcc.Id, FromDate, ToDate, VidVal, 1, CodeClient));
            List<ValutaControl> contos1 = new List<ValutaControl>(Context.GetAllContoValuta(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id, CurrenAcc.Id, FromDate, ToDate, VidVal, 2, CodeClient));
            List<ValutaControl> contos3 = new List<ValutaControl>(Context.GetAllContoValuta(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id, CurrenAcc.Id, FromDate, ToDate, VidVal, 1, CodeClient));
            List<ValutaControl> contos4 = new List<ValutaControl>(Context.GetAllContoValuta(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id, CurrenAcc.Id, FromDate, ToDate, VidVal, 2, CodeClient));
            List<ValutaControl> contosb = null;
            List<ValutaControl> contos1b = null;
            var rezil = Context.GetAllAnaliticSaldos(CurrenAcc.Id, CurrenAcc.FirmaId,VidVal);
            if (!string.IsNullOrWhiteSpace(CodeClient))
            {
                rezil = rezil.Where(e => e.Code == CodeClient).ToList();
            }
            var rezi= (from t in rezil
                       group t by new { t.Code }
                              into grp
                       select new SaldoFactura
                       {
                           Code = grp.Key.Code,
                           BeginSaldoCredit = grp.Sum(t => t.BeginSaldoCredit),
                           BeginSaldoCreditValuta = grp.Sum(t => t.BeginSaldoCreditValuta),
                           BeginSaldoDebit=grp.Sum(t=>t.BeginSaldoDebit),
                           BeginSaldoDebitValuta=grp.Sum(t=>t.BeginSaldoDebitValuta),
                           BeginSaldoDebitKol = grp.Sum(t => t.BeginSaldoDebitKol),
                           BeginSaldoCreditKol = grp.Sum(t => t.BeginSaldoDebitKol),
                       }).ToList();
            
            if (fromDate.Month > 1 && !string.IsNullOrWhiteSpace(CodeClient))
            {
                contosb = new List<ValutaControl>(Context.GetAllContoValuta(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id, CurrenAcc.Id, new DateTime(FromDate.Year, 1, 1), FromDate, VidVal, 1, CodeClient)); 
                contos1b = new List<ValutaControl>(Context.GetAllContoValuta(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id, CurrenAcc.Id, new DateTime(FromDate.Year, 1, 1), FromDate, VidVal,2, CodeClient)); 

                if (contosb != null)
                {
                    var query = (from t in contosb
                                 group t by new { t.ClienCode }
                              into grp
                                 select new ValutaControl
                                 {
                                     ClienCode = grp.Key.ClienCode,
                                     Oborot = grp.Sum(t => t.Oborot),
                                     ValSum = grp.Sum(t => t.ValSum)
                                 }).ToList();
                    foreach (var item in query)
                    {
                        if (rezi.FirstOrDefault(e => e.Code == item.ClienCode) != null)
                        {
                            rezi.FirstOrDefault(e => e.Code == item.ClienCode).BeginSaldoDebit += item.Oborot;
                            rezi.FirstOrDefault(e => e.Code == item.ClienCode).BeginSaldoDebitValuta += item.ValSum;
                        }
                        else
                        {
                            rezi.Add(new SaldoFactura { Code = item.ClienCode, BeginSaldoDebit = item.Oborot, BeginSaldoDebitValuta = item.ValSum });
                        }
                    }
                }
                if (contos1b != null)
                {
                    var query1 = (from t in contos1b
                                 group t by new { t.ClienCode }
                                 into grp
                                                select new ValutaControl
                                                {
                                                    ClienCode = grp.Key.ClienCode,
                                                    Oborot = grp.Sum(t => t.Oborot),
                                                    ValSum = grp.Sum(t => t.ValSum)
                                                }).ToList();
                    foreach (var item in query1)
                    {
                        if (rezi.FirstOrDefault(e => e.Code == item.ClienCode) != null)
                        {
                            rezi.FirstOrDefault(e => e.Code == item.ClienCode).BeginSaldoCredit += item.Oborot;
                            rezi.FirstOrDefault(e => e.Code == item.ClienCode).BeginSaldoCreditValuta += item.ValSum;
                        }
                        else
                        {
                            rezi.Add(new SaldoFactura { Code = item.ClienCode, BeginSaldoCredit = item.Oborot, BeginSaldoCreditValuta = item.ValSum });
                        }
                    }
                }

            }
            decimal sumad = 0, sumac = 0;
            decimal sumavald = 0, sumavalc = 0;
            decimal sumavalddf = 0, sumavalcdf = 0;
            decimal sumadm = 0;
            decimal sumavaldm = 0;
            decimal sumavalddfm = 0;

            sumad      = contos.Sum(e => e.Oborot);
            sumac      = contos1.Sum(e => e.Oborot);
            sumavald   = contos.Sum(e => e.ValSum);
            sumavalc   = contos1.Sum(e => e.ValSum);
            sumavalddf = contos.Sum(e => e.KursDif);
            sumavalcdf = contos1.Sum(e => e.KursDif);

            string currec = "", oldrec = "";
            string lastcode = "", lastname = "";
            contos3.AddRange(contos4);
            int currentrow = 0;
            foreach (
                var co in
                    contos3.OrderBy(e => e.ClienCode))
            {
                if (oldrec == "")
                {
                    oldrec = co.ClienCode;
                }
                currec = co.ClienCode;
                if (oldrec != currec)
                {
                    var lsumad1        = contos.Where(e=>e.ClienCode== oldrec).Sum(e => e.Oborot);
                    var lsumac1        =contos1.Where(e=>e.ClienCode== oldrec).Sum(e => e.Oborot);
                    var lsumavald1     = contos.Where(e=>e.ClienCode== oldrec).Sum(e => e.ValSum);
                    var lsumavalc1     =contos1.Where(e=>e.ClienCode== oldrec).Sum(e => e.ValSum);
                    var lsumavalddf1   = contos.Where(e=>e.ClienCode== oldrec).Sum(e => e.KursDif);
                    var lsumavalcdf1   =contos1.Where(e => e.ClienCode == oldrec).Sum(e => e.KursDif);
                    var saldo1 = rezi.FirstOrDefault(e => e.Code == oldrec);
                    if (saldo1 != null)
                    {
                        rezi.Remove(saldo1);
                    }
                    var lnsd1 = saldo1 != null ? saldo1.BeginSaldoDebit : 0;
                    var lnsc1 = saldo1 != null ? saldo1.BeginSaldoCredit : 0;
                    var lnsdv1 = saldo1 != null ? saldo1.BeginSaldoDebitValuta : 0;
                    var lnscv1 = saldo1 != null ? saldo1.BeginSaldoCreditValuta : 0;
                    if (CurrenAcc.TypeAccount == 1)
                    {
                        lnsd1 = lnsd1 - lnsc1;
                        lnsdv1 = lnsdv1 - lnscv1;
                        lnsc1 = 0;
                        lnscv1 = 0;
                    }
                    else
                    {
                        lnsc1 = lnsc1 - lnsd1;
                        lnscv1 = lnscv1 - lnsdv1;
                        lnsd1 = 0;
                        lnsdv1 = 0;
                    }
                    var lsbord1 = lnsd1 + lsumad1;
                    var lsborc1 = lnsc1 + lsumac1;
                    var lsbordv1 = lnsdv1 + lsumavald1;
                    var lsborcv1 = lnscv1 + lsumavalc1;
                    decimal lksd1 = 0;
                    decimal lksc1 = 0;
                    decimal lksdv1 = 0;
                    decimal lkscv1 = 0;
                    if (CurrenAcc.TypeAccount == 1)
                    {
                        lksd1 = (lsumad1 + lnsd1) - (lsumac1 + lnsc1);
                        lksdv1 = (lsumavald1 + lnsdv1) - (lsumavalc1 + lnsc1);
                    }
                    else
                    {
                        lksc1 = (lsumac1 + lnsc1) - (lsumad1 + lnsd1);
                        lkscv1 = (lsumavalc1 + lnscv1) - (lsumavald1 + lnsdv1);
                    }

                    var row2 = new List<string>();
                    row2.Add("----------------------------------------------------------------------------------");
                    row2.Add("|Сборно          |          л е в а              |           валута              |");
                    row2.Add("|                |    дебит      |    кредит     |    дебит      |    кредит     |");
                    row2.Add("----------------------------------------------------------------------------------");
                    row2.Add($"|Начални салда   |{lnsd1.ToString(Vf.LevFormatUI),15}|{lnsc1.ToString(Vf.LevFormatUI),15}|{lnsdv1.ToString(Vf.ValFormatUI),15}|{lnscv1.ToString(Vf.ValFormatUI),15}|");
                    row2.Add($"|Oбороти         |{lsumad1.ToString(Vf.LevFormatUI),15}|{lsumac1.ToString(Vf.LevFormatUI),15}|{lsumavald1.ToString(Vf.ValFormatUI),15}|{lsumavalc1.ToString(Vf.ValFormatUI),15}|");
                    row2.Add($"|Сборове         |{lsbord1.ToString(Vf.LevFormatUI),15}|{lsborc1.ToString(Vf.LevFormatUI),15}|{lsbordv1.ToString(Vf.ValFormatUI),15}|{lsborcv1.ToString(Vf.ValFormatUI),15}|");
                    row2.Add($"|Крайни салда    |{lksd1.ToString(Vf.LevFormatUI),15}|{lksc1.ToString(Vf.LevFormatUI),15}|{lksdv1.ToString(Vf.ValFormatUI),15}|{lkscv1.ToString(Vf.ValFormatUI),15}|");
                    row2.Add("----------------------------------------------------------------------------------");
                    
                    Rowfoother.Add(currentrow-1, row2);
                    oldrec = currec;
                }
                List<string> item2 = new List<string>();
                item2.Add(co.PorNom);
                item2.Add(co.DocNum);
                item2.Add(co.Data);
                item2.Add(co.Folder);
                item2.Add(co.User);
                item2.Add(co.KindVal);
                item2.Add(string.Format(Vf.LevFormat, co.Oborot));
                var dac = Allacc.FirstOrDefault(e => e.Id == co.DebitAccount);
                if (dac != null) item2.Add(dac.Short);
                dac = Allacc.FirstOrDefault(e => e.Id == co.CreditAccount);
                if (dac != null) item2.Add(dac.Short);
                item2.Add(string.Format(Vf.ValFormat, co.ValSum));
                item2.Add(string.Format(Vf.KursFormat, co.Kurs));
                item2.Add(string.Format(Vf.KursFormat, co.MainKurs));
                item2.Add(string.Format(Vf.LevFormat, co.KursDif));
                item2.Add(co.Reason);
                item2.Add(co.Note);
                item2.Add(co.Pr1);
                item2.Add(co.Pr2);
                item2.Add(co.ClienCode); lastcode = co.ClienCode;
                item2.Add(co.NameClient); lastname = co.NameClient;
                item2.Add(co.NInvoice); 
                item2.Add(co.DInvoce); 
                item2.Add(co.User);
                item2.Add(co.Id);
                items.Add(item2);
                sumadm += co.Oborot;
                sumavaldm += co.ValSum;
                sumavalddfm += co.KursDif;
                currentrow++;
            }

            var lsumad = contos.Where(e => e.ClienCode == currec).Sum(e => e.Oborot);
            var lsumac = contos1.Where(e => e.ClienCode == currec).Sum(e => e.Oborot);
            var lsumavald = contos.Where(e => e.ClienCode == currec).Sum(e => e.ValSum);
            var lsumavalc = contos1.Where(e => e.ClienCode == currec).Sum(e => e.ValSum);
            var lsumavalddf = contos.Where(e => e.ClienCode == currec).Sum(e => e.KursDif);
            var lsumavalcdf = contos1.Where(e => e.ClienCode == currec).Sum(e => e.KursDif);
            var saldo = rezi.FirstOrDefault(e => e.Code == currec);
            
            var lnsd = saldo !=null ? saldo.BeginSaldoDebit : 0;
                var lnsc = saldo != null ? saldo.BeginSaldoCredit : 0;
                var lnsdv = saldo != null ? saldo.BeginSaldoDebitValuta : 0;
                var lnscv = saldo != null ? saldo.BeginSaldoCreditValuta : 0;
            if (saldo != null)
            {
                rezi.Remove(saldo);
            }
                if (CurrenAcc.TypeAccount == 1)
                {
                    lnsd = lnsd - lnsc;
                    lnsdv = lnsdv - lnscv;
                    lnsc = 0;
                    lnscv = 0;
                }
                else
                {
                    lnsc =  lnsc - lnsd;
                    lnscv = lnscv - lnsdv;
                    lnsd = 0;
                    lnsdv = 0;
                }
                var lsbord = lnsd + lsumad;
                var lsborc = lnsc + lsumac;
                var lsbordv = lnsdv + lsumavald;
                var lsborcv = lnscv + lsumavalc;
                decimal lksd=0;
                decimal lksc=0;
                decimal lksdv=0;
                decimal lkscv=0;
                if (CurrenAcc.TypeAccount == 1)
                {
                    lksd = (lsumad + lnsd) - (lsumac + lnsc); 
                    lksdv = (lsumavald + lnsdv) - (lsumavalc + lnsc);
                }
                else
                {
                    lksc = (lsumac + lnsc)-(lsumad + lnsd);
                    lkscv = (lsumavalc + lnscv)-(lsumavald + lnsdv);
                }
            if (currentrow > 0)
            {
                var row1 = new List<string>();
                row1.Add("----------------------------------------------------------------------------------");
                row1.Add("|Сборно          |          л е в а              |           валута              |");
                row1.Add("|                |    дебит      |    кредит     |    дебит      |    кредит     |");
                row1.Add("----------------------------------------------------------------------------------");
                row1.Add($"|Начални салда   |{lnsd.ToString(Vf.LevFormatUI),15}|{lnsc.ToString(Vf.LevFormatUI),15}|{lnsdv.ToString(Vf.ValFormatUI),15}|{lnscv.ToString(Vf.ValFormatUI),15}|");
                row1.Add($"|Oбороти         |{lsumad.ToString(Vf.LevFormatUI),15}|{lsumac.ToString(Vf.LevFormatUI),15}|{lsumavald.ToString(Vf.ValFormatUI),15}|{lsumavalc.ToString(Vf.ValFormatUI),15}|");
                row1.Add($"|Сборове         |{lsbord.ToString(Vf.LevFormatUI),15}|{lsborc.ToString(Vf.LevFormatUI),15}|{lsbordv.ToString(Vf.ValFormatUI),15}|{lsborcv.ToString(Vf.ValFormatUI),15}|");
                row1.Add($"|Крайни салда    |{lksd.ToString(Vf.LevFormatUI),15}|{lksc.ToString(Vf.LevFormatUI),15}|{lksdv.ToString(Vf.ValFormatUI),15}|{lkscv.ToString(Vf.ValFormatUI),15}|");
                row1.Add("----------------------------------------------------------------------------------");
                Rowfoother.Add(currentrow - 1, row1);
            }
            
            foreach (var rez in rezi)
            {
                currentrow++;
                List<string> item2 = new List<string>();
                item2.Add("*****");
                item2.Add("");
                item2.Add("");
                item2.Add("");
                item2.Add("");
                item2.Add("");
                item2.Add("");
                item2.Add("");
                item2.Add("");
                item2.Add("");
                item2.Add("");
                item2.Add("");
                item2.Add("");
                item2.Add("");
                item2.Add("");
                item2.Add("");
                item2.Add("");
                item2.Add(rez.Code);
                item2.Add(rez.NameContragent);
                item2.Add(rez.NumInvoise);
                item2.Add(rez.Date.ToShortDateString());
                item2.Add("");
                item2.Add("");
                items.Add(item2);
                var row1 = new List<string>();
                row1.Add("----------------------------------------------------------------------------------");
                row1.Add("|Сборно          |          л е в а              |           валута              |");
                row1.Add("|                |    дебит      |    кредит     |    дебит      |    кредит     |");
                row1.Add("----------------------------------------------------------------------------------");
                row1.Add($"|Начални салда   |{rez.BeginSaldoDebit.ToString(Vf.LevFormatUI),15}|{rez.BeginSaldoCredit.ToString(Vf.LevFormatUI),15}|{rez.BeginSaldoDebitValuta.ToString(Vf.ValFormatUI),15}|{rez.BeginSaldoCreditValuta.ToString(Vf.ValFormatUI),15}|");
                row1.Add($"|Oбороти         |{0.ToString(Vf.LevFormatUI),15}|{0.ToString(Vf.LevFormatUI),15}|{0.ToString(Vf.ValFormatUI),15}|{0.ToString(Vf.ValFormatUI),15}|");
                row1.Add($"|Сборове         |{0.ToString(Vf.LevFormatUI),15}|{0.ToString(Vf.LevFormatUI),15}|{0.ToString(Vf.ValFormatUI),15}|{0.ToString(Vf.ValFormatUI),15}|");
                row1.Add($"|Крайни салда    |{rez.BeginSaldoDebit.ToString(Vf.LevFormatUI),15}|{rez.BeginSaldoCredit.ToString(Vf.LevFormatUI),15}|{rez.BeginSaldoDebitValuta.ToString(Vf.ValFormatUI),15}|{rez.BeginSaldoCreditValuta.ToString(Vf.ValFormatUI),15}|");
                row1.Add("----------------------------------------------------------------------------------");
                Rowfoother.Add(currentrow - 1, row1);
            }
            decimal nsd = 0;
            decimal nsc = 0;
            decimal nsdv = 0;
            decimal nscv = 0;
            if (FromDate.Month > 1)
            {
                var begcontosd =
                  new List<ValutaControl>(Context.GetAllContoValuta(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id, CurrenAcc.Id, new DateTime(FromDate.Year, 1, 1), FromDate.AddDays(-1), VidVal, 1, CodeClient));
                var begcontosc =
                  new List<ValutaControl>(Context.GetAllContoValuta(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id, CurrenAcc.Id, new DateTime(FromDate.Year, 1, 1), FromDate.AddDays(-1), VidVal, 2, CodeClient));
                nsd = begcontosd.Sum(e => e.Oborot);
                nsc = begcontosc.Sum(e => e.Oborot);
                nsdv = begcontosd.Sum(e => e.ValSum);
                nscv = begcontosc.Sum(e => e.ValSum);
            }
            if (CurrenAcc.TypeAccount == 1)
            {
                if (FromDate.Month == 1)
                {
                    BeginSaldoD = CurrenAcc.BeginSaldoL;
                    BeginValSd = CurrenAcc.BeginSaldoV;
                }
                else
                {
                    BeginSaldoD = (nsd + CurrenAcc.BeginSaldoL) - (nsc);
                    BeginValSd = (nsdv + CurrenAcc.BeginSaldoV) - (nscv);
                }

            }
            if (CurrenAcc.TypeAccount == 2)
            {
                if (FromDate.Month == 1)
                {
                    BeginSaldoK = CurrenAcc.BeginSaldoL;
                    BeginValSc = CurrenAcc.BeginSaldoV;
                }
                else
                {
                    BeginSaldoK = (nsc + CurrenAcc.BeginSaldoL) - (nsd);
                    BeginValSc = (nscv + CurrenAcc.BeginSaldoV) - (nsdv);
                }
            }
            OborotsDebit = string.Format(Vf.LevFormat, sumad);
            OborotsCredit = string.Format(Vf.LevFormat, sumac);
            Oborotsvald = string.Format(Vf.ValFormat, sumavald);
            Oborotsvalc = string.Format(Vf.ValFormat, sumavalc);
            Sumavald = string.Format(Vf.ValFormat, sumavald);
            Sumavalc = string.Format(Vf.ValFormat, sumavalc);
            KursDifd = string.Format(Vf.LevFormat, sumavalddf);
            KursDifc = string.Format(Vf.LevFormat, sumavalcdf);


            TotalD = CurrenAcc.TypeAccount == 1
                ? string.Format(Vf.LevFormat, sumad + BeginSaldoD)
                : string.Format(Vf.LevFormat, sumad);
            TotalC = CurrenAcc.TypeAccount == 2
                ? string.Format(Vf.LevFormat, sumac + BeginSaldoK)
                : string.Format(Vf.LevFormat, sumac);
            if (CurrenAcc.TypeAccount == 1)
            {
                KrainoSaldoD = (sumad + BeginSaldoD) - (sumac);
                KrainoSaldoDV = (sumavald + BeginValSd) - (sumavalc);
                KrainoSaldoKV = 0;
                Sad = string.Format(Vf.KursFormat, KrainoSaldoDV != 0 ? KrainoSaldoD / KrainoSaldoDV : 0);
                Sak = "";
            }
            if (CurrenAcc.TypeAccount == 2)
            {
                KrainoSaldoK = (sumac + BeginSaldoK) - (sumad);
                KrainoSaldoKV = (sumavalc + BeginValSc) - (sumavald);
                KrainoSaldoDV = 0;
                Sad = "";
                Sak = string.Format(Vf.KursFormat, KrainoSaldoKV != 0 ? KrainoSaldoK / KrainoSaldoKV : 0);
            }
            return items;
        }

        private static List<string> NewMethod()
        {
            List<string> item1 = new List<string>();
            item1.Add("---------------------------------------------");
            item1.Add("---------------------------------------------");
            item1.Add("---------------------------------------------");
            item1.Add("---------------------------------------------");
            item1.Add("---------------------------------------------");
            item1.Add("---------------------------------------------");
            item1.Add("---------------------------------------------");
            item1.Add("---------------------------------------------");
            item1.Add("---------------------------------------------");
            item1.Add("---------------------------------------------");
            item1.Add("---------------------------------------------");
            item1.Add("---------------------------------------------");
            item1.Add("---------------------------------------------");
            item1.Add("---------------------------------------------");
            item1.Add("---------------------------------------------");
            item1.Add("---------------------------------------------");
            item1.Add("---------------------------------------------");
            item1.Add("---------------------------------------------");
            item1.Add("---------------------------------------------");
            item1.Add("-------------------------------------------------------------------");
            item1.Add("---------------------------------------------");
            return item1;
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
            list.Add(string.Format("вид валута                : {0}", VidVal));
            if (!string.IsNullOrWhiteSpace(CodeClient))
                list.Add(string.Format("Клиент                    : {0} {1}", CodeClient, Client));
            return list;
        }

        public List<string> GetFuther()
        {
            if (!string.IsNullOrWhiteSpace(CodeClient)) return null;
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
            list.Add("| Валутни стойности           ||||||||||||||||||||||||||||||||||||||");
            list.Add("--------------------------------------------------------------------");
            list.Add(string.Format("| Валутни нач.салда           |{0,17}|{1,18}|", BeginValSd, BeginValSc));
            list.Add("--------------------------------------------------------------------");
            list.Add("| Валутни обороти             ||||||||||||||||||||||||||||||||||||||");
            list.Add("--------------------------------------------------------------------");
            list.Add(string.Format("| Валутна сума                |{0,17}|{1,18}|", Sumavald, Sumavalc));  //  -----това са валутните обороти
            list.Add(string.Format("| Курсови разлики             |{0,17}|{1,18}|", KursDifd, KursDifc));
            list.Add(string.Format("| Среднопретеглен курс        |{0,17}|{1,18}|", SrednoPretd, SrednoPretc));//    ----после ще го обсъдим
            list.Add(string.Format("| Средноаритм. курс           |{0,17}|{1,18}|", SrednoAritmd, SrednoAritmc));
            list.Add(string.Format("| Средноаритм. опорен курс    |{0,17}|{1,18}|", SrednoAritmOpd, SrednoAritmOpc));
            list.Add("--------------------------------------------------------------------");
            list.Add("| Валутни кр.салда            ||||||||||||||||||||||||||||||||||||||");
            list.Add("--------------------------------------------------------------------");
            list.Add(string.Format("| Валутнa сума                |{0,17}|{1,18}|", KrainoSaldoDV.ToString(Vf.KursFormatUI), KrainoSaldoKV.ToString(Vf.KursFormatUI)));
            list.Add(string.Format("| Средноаритметичен курс      |{0,17}|{1,18}|", Sad, Sak));
            list.Add("--------------------------------------------------------------------");
            return list;
        }

        public string Filename
        {
            get { return "analitics"; }
        }

        public string Title
        {
            get { return "AНАЛИТИЧЕН   РЕГИСТЪР - справка валута"; }
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
                OnPropertyChanged("FromDate");
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
        public string CodeClient { get; set; }
        public string Client { get; set; }
    }
}
