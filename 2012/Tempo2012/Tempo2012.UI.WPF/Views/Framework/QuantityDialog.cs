﻿using System;
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
        public Dictionary<int, List<string>> Rowfoother { get; set; }
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
            reportItems.Add(new ReportItem {Height = 30, IsShow = true, Name = "Ед. цена", Width = 12});
            reportItems.Add(new ReportItem {Height = 30, IsShow = true, Name = "основание", Width = 30});
            reportItems.Add(new ReportItem {Height = 30, IsShow = true, Name = "забележка", Width = 20});
            reportItems.Add(new ReportItem {Height = 30, IsShow = true, Name = "код", Width = 10});
            reportItems.Add(new ReportItem {Height = 30, IsShow = true, Name = "материал", Width = 40});
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
            BeginSaldoD = 0;
            BeginValSd = 0;
            Rowfoother = new Dictionary<int, List<string>>();
            List<List<string>> items = new List<List<string>>();
            var rezi = Context.GetAllAnaliticSaldos(CurrenAcc.Id, CurrenAcc.FirmaId);
            List<QuantityModel> contosb = null;
            List<QuantityModel> contos1b = null;
            if (!string.IsNullOrWhiteSpace(KindStock))
            {
                rezi = rezi.Where(e => e.CodeMaterial == KindStock).ToList();
            }
            if (fromDate.Month > 1)
            {
                contosb = new List<QuantityModel>(Context.GetAllContoQuantity(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id, CurrenAcc.Id, new DateTime(FromDate.Year, 1, 1), FromDate.AddDays(-1), 1, KindStock));
                contos1b = new List<QuantityModel>(Context.GetAllContoQuantity(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id, CurrenAcc.Id, new DateTime(FromDate.Year, 1, 1), FromDate.AddDays(-1), 2, KindStock));

                if (contosb != null)
                {
                    var query = (from t in contosb
                                 group t by new { t.StockCode }
                              into grp
                                 select new QuantityModel
                                 {
                                     StockCode = grp.Key.StockCode,
                                     Stock=grp.First().Stock,
                                     Oborot=grp.Sum(t => t.Oborot),
                                     Quantity=grp.Sum(t=>t.Quantity)
                                 }).ToList();
                    foreach (var item in query)
                    {
                        if (rezi.FirstOrDefault(e => e.CodeMaterial == item.StockCode)!=null)
                        {
                            rezi.FirstOrDefault(e => e.CodeMaterial == item.StockCode).BeginSaldoDebit += item.Oborot;
                            rezi.FirstOrDefault(e => e.CodeMaterial == item.StockCode).BeginSaldoDebitKol += item.Quantity;
                        }
                        else
                        {
                            rezi.Add(new SaldoFactura { CodeMaterial = item.StockCode,NameMaterial=item.Stock, BeginSaldoDebit = item.Oborot,BeginSaldoDebitKol=item.Quantity });
                        }
                    }
                }
                if (contos1b != null)
                {
                    var query = (from t in contos1b
                                 group t by new { t.StockCode }
                              into grp
                                 select new QuantityModel
                                 {
                                     StockCode = grp.Key.StockCode,
                                     Stock = grp.First().Stock,
                                     Oborot = grp.Sum(t => t.Oborot),
                                     Quantity = grp.Sum(t => t.Quantity)
                                 }).ToList();
                    foreach (var item in query)
                    {
                        if (rezi.FirstOrDefault(e => e.CodeMaterial == item.StockCode) != null)
                        {
                            rezi.FirstOrDefault(e => e.CodeMaterial == item.StockCode).BeginSaldoCredit += item.Oborot;
                            rezi.FirstOrDefault(e => e.CodeMaterial == item.StockCode).BeginSaldoCreditKol += item.Quantity;
                        }
                        else
                        {
                            rezi.Add(new SaldoFactura { CodeMaterial = item.StockCode, NameMaterial = item.Stock, BeginSaldoCredit = item.Oborot,BeginSaldoCreditKol=item.Quantity});
                        }
                    }
                }

            }
            List<QuantityModel> contos =new List<QuantityModel>(Context.GetAllContoQuantity(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id,CurrenAcc.Id,FromDate,ToDate,1,KindStock));
            List<QuantityModel> contos1=new List<QuantityModel>(Context.GetAllContoQuantity(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id, CurrenAcc.Id, FromDate, ToDate, 2, KindStock));
            List<QuantityModel> contos3 = new List<QuantityModel>(Context.GetAllContoQuantity(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id, CurrenAcc.Id, FromDate, ToDate, 1, KindStock));
            List<QuantityModel> contos4 = new List<QuantityModel>(Context.GetAllContoQuantity(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id, CurrenAcc.Id, FromDate, ToDate, 2, KindStock));

            decimal sumad = 0,sumac = 0;
            decimal nsd = 0, nskold = 0;
            decimal nsc = 0, nskolc = 0;
            decimal ksd = 0, kskold = 0;
            decimal ksc = 0, kskolc = 0;
            decimal sumamd = 0, sumamc = 0;
            decimal sumacolmd = 0, sumacolmc = 0;

            sumad =contos.Sum(e=>e.Oborot);
            sumac =contos1.Sum(e=>e.Oborot);
            decimal sumaquantityd = contos.Sum(e=>e.Quantity);
            decimal sumaquantityc = contos1.Sum(e => e.Quantity);
            //decimal sumasinglepriced=contos1.Sum(e=>e.SinglePrice);
            //decimal sumasinglepricec = contos1.Sum(e => e.SinglePrice);
            string currec = "", oldrec = "";
            string lastcode="", lastname="";
            contos3.AddRange(contos4);
            var currentrow = 0;
            foreach (
                var co in
                    contos3.OrderBy(e=>e.StockCode))
            {

                if (oldrec == "")
                {
                    oldrec = co.StockCode;
                }
                currec = co.StockCode;
                if (oldrec != currec)
                {
                    var lsumad1 = contos.Where(e => e.StockCode == oldrec).Sum(e => e.Oborot);
                    var lsumac1 = contos1.Where(e => e.StockCode == oldrec).Sum(e => e.Oborot);
                    var lsumavald1 = contos.Where(e => e.StockCode == oldrec).Sum(e => e.Quantity);
                    var lsumavalc1 = contos1.Where(e => e.StockCode == oldrec).Sum(e => e.Quantity);
                    var saldo1 = rezi.FirstOrDefault(e => e.CodeMaterial == oldrec);
                    if (saldo1 != null)
                    {
                        rezi.Remove(saldo1);
                    }
                    var lnsd1 = saldo1 != null ? saldo1.BeginSaldoDebit : 0;
                    var lnsc1 = saldo1 != null ? saldo1.BeginSaldoCredit : 0;
                    var lnsdv1 = saldo1 != null ? saldo1.BeginSaldoDebitKol : 0;
                    var lnscv1 = saldo1 != null ? saldo1.BeginSaldoCreditKol : 0;
                    if (CurrenAcc.TypeAccount == 1)
                    {
                        lnsd1 = lnsd1 - lnsc1;
                        lnsdv1 = lnsdv1 - lnscv1;
                        lnsc1 = 0;
                        lnscv1 = 0;
                        nsd += lnsd1;
                        nskold += lnsdv1;
                    }
                    else
                    {
                        lnsc1 = lnsc1 - lnsd1;
                        lnscv1 = lnscv1 - lnsdv1;
                        lnsd1 = 0;
                        lnsdv1 = 0;
                        nsc += lnsc1;
                        nskolc += lnscv1;
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
                        ksd += lksd1;
                        kskold += lksdv1;
                    }
                    else
                    {
                        lksc1 = (lsumac1 + lnsc1) - (lsumad1 + lnsd1);
                        lkscv1 = (lsumavalc1 + lnscv1) - (lsumavald1 + lnsdv1);
                        ksc += lksc1;
                        kskolc += lkscv1;
                    }

                    var row2 = new List<string>();
                    row2.Add("----------------------------------------------------------------------------------");
                    row2.Add("|Сборно          |          л е в а              |           количество          |");
                    row2.Add("|                |    дебит      |    кредит     |    дебит      |    кредит     |");
                    row2.Add("----------------------------------------------------------------------------------");
                    row2.Add($"|Начални салда   |{lnsd1.ToString(Vf.LevFormatUI),15}|{lnsc1.ToString(Vf.LevFormatUI),15}|{lnsdv1.ToString(Vf.ValFormatUI),15}|{lnscv1.ToString(Vf.ValFormatUI),15}|");
                    row2.Add($"|Oбороти         |{lsumad1.ToString(Vf.LevFormatUI),15}|{lsumac1.ToString(Vf.LevFormatUI),15}|{lsumavald1.ToString(Vf.ValFormatUI),15}|{lsumavalc1.ToString(Vf.ValFormatUI),15}|");
                    row2.Add($"|Сборове         |{lsbord1.ToString(Vf.LevFormatUI),15}|{lsborc1.ToString(Vf.LevFormatUI),15}|{lsbordv1.ToString(Vf.ValFormatUI),15}|{lsborcv1.ToString(Vf.ValFormatUI),15}|");
                    row2.Add($"|Крайни салда    |{lksd1.ToString(Vf.LevFormatUI),15}|{lksc1.ToString(Vf.LevFormatUI),15}|{lksdv1.ToString(Vf.ValFormatUI),15}|{lkscv1.ToString(Vf.ValFormatUI),15}|");
                    row2.Add("----------------------------------------------------------------------------------");
                    Rowfoother.Add(currentrow - 1, row2);
                    oldrec = currec;
                    //var el1 = rezi.FirstOrDefault(e => e.CodeMaterial == oldrec);
                    //if (el1 != null)
                    //{
                    //    nsd = el1.BeginSaldoDebit - el1.BeginSaldoCredit;
                    //    BeginSaldoD += nsd;
                    //    nskold = el1.BeginSaldoDebitKol - el1.BeginSaldoCreditKol;
                    //    BeginValSd += nskold;
                    //    rezi.Remove(el1);
                    //}
                    //else
                    //{
                    //    nsd = 0;
                    //    nskold = 0;
                    //}
                    //var row = new List<string>();
                    //var sbor1 = nsd + sumamd;
                    //var sbork1 = nskold + sumacolmd;
                    //row.Add("----------------------------------------------------------------------------------");
                    //row.Add("|Сборно          |          л е в а              |           количества          |");
                    //row.Add("|                |    дебит      |    кредит     |    дебит      |    кредит     |");
                    //row.Add("----------------------------------------------------------------------------------");
                    //row.Add($"|Начални салда   |{nsd.ToString(Vf.LevFormatUI),15}|               |{nskold.ToString(Vf.KolFormatUI),15}|               |");
                    //row.Add($"|Oбороти         |{sumamd.ToString(Vf.LevFormatUI),15}|{sumamc.ToString(Vf.LevFormatUI),15}|{sumacolmd.ToString(Vf.KolFormatUI),15}|{sumacolmc.ToString(Vf.KolFormatUI),15}|");
                    //row.Add($"|Сборове         |{sbor1.ToString(Vf.LevFormatUI),15}|{sumamc.ToString(Vf.LevFormatUI),15}|{sbork1.ToString(Vf.KolFormatUI),15}|{sumacolmc.ToString(Vf.KolFormatUI),15}|");
                    //row.Add($"|Крайни салда    |{(sbor1 - sumamc).ToString(Vf.LevFormatUI),15}|               |{(sbork1 - sumacolmc).ToString(Vf.KolFormatUI),15}|               |");
                    ////row.Add($"|Средна цена     |{((sumamd + nsd) / (sumacolmd + nskold)).ToString(Vf.LevFormatUI),15}|               |               |               |");
                    //row.Add("----------------------------------------------------------------------------------");
                    //Rowfoother.Add(currentrow-1,row);
                    //sumamd = 0;
                    //sumacolmd = 0;
                    //sumamc = 0;
                    //sumacolmc = 0;
                    //oldrec = currec;
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
                currentrow++;
            }

            //var row1 = new List<string>();
            //var el = rezi.FirstOrDefault(e => e.CodeMaterial == currec);
            //if (el != null)
            //{
            //    nsd = el.BeginSaldoDebit-el.BeginSaldoCredit;
            //    BeginSaldoD += nsd;
            //    nskold = el.BeginSaldoDebitKol-el.BeginSaldoCreditKol;
            //    BeginValSd += nskold;
            //    rezi.Remove(el);
            //}
            //else
            //{
            //    nsd = 0;
            //    nskold = 0;
            //}
            //var sbor = nsd + sumamd;
            //var sbork = nskold + sumacolmd;
            //row1.Add("----------------------------------------------------------------------------------");
            //row1.Add("|Сборно          |          л е в а              |           количества          |");
            //row1.Add("|                |    дебит      |    кредит     |    дебит      |    кредит     |");
            //row1.Add("----------------------------------------------------------------------------------");
            //row1.Add($"|Начални салда   |{nsd.ToString(Vf.LevFormatUI),15}|               |{nskold.ToString(Vf.KolFormatUI),15}|               |");
            //row1.Add($"|Oбороти         |{sumamd.ToString(Vf.LevFormatUI),15}|{sumamc.ToString(Vf.LevFormatUI),15}|{sumacolmd.ToString(Vf.KolFormatUI),15}|{sumacolmc.ToString(Vf.KolFormatUI),15}|");
            //row1.Add($"|Сборове         |{sbor.ToString(Vf.LevFormatUI),15}|{sumamc.ToString(Vf.LevFormatUI),15}|{sbork.ToString(Vf.KolFormatUI),15}|{sumacolmc.ToString(Vf.KolFormatUI),15}|");
            //row1.Add($"|Крайни салда    |{(sbor - sumamc).ToString(Vf.LevFormatUI),15}|               |{(sbork - sumacolmc).ToString(Vf.KolFormatUI),15}|               |");
            ////row1.Add($"|Средна цена     |{((sumamd + nsd) / (sumacolmd + nskold)).ToString(Vf.LevFormatUI),15}|               |               |               |");
            //row1.Add("----------------------------------------------------------------------------------");
            //Rowfoother.Add(currentrow-1, row1);
            var lsumad = contos.Where(e => e.StockCode == currec).Sum(e => e.Oborot);
            var lsumac = contos1.Where(e => e.StockCode == currec).Sum(e => e.Oborot);
            var lsumavald = contos.Where(e => e.StockCode == currec).Sum(e => e.Quantity);
            var lsumavalc = contos1.Where(e => e.StockCode == currec).Sum(e => e.Quantity);
           
            var saldo = rezi.FirstOrDefault(e => e.CodeMaterial == currec);

            var lnsd = saldo != null ? saldo.BeginSaldoDebit : 0;
            var lnsc = saldo != null ? saldo.BeginSaldoCredit : 0;
            var lnsdv = saldo != null ? saldo.BeginSaldoDebitKol : 0;
            var lnscv = saldo != null ? saldo.BeginSaldoCreditKol : 0;
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
                nsd += lnsd;
                nskold += lnsdv;
            }
            else
            {
                lnsc = lnsc - lnsd;
                lnscv = lnscv - lnsdv;
                lnsd = 0;
                lnsdv = 0;
                nsc += lnsc;
                nskolc += lnscv;
            }
            var lsbord = lnsd + lsumad;
            var lsborc = lnsc + lsumac;
            var lsbordv = lnsdv + lsumavald;
            var lsborcv = lnscv + lsumavalc;
            decimal lksd = 0;
            decimal lksc = 0;
            decimal lksdv = 0;
            decimal lkscv = 0;
            if (CurrenAcc.TypeAccount == 1)
            {
                lksd = (lsumad + lnsd) - (lsumac + lnsc);
                lksdv = (lsumavald + lnsdv) - (lsumavalc + lnsc);
                ksd += lksd;
                kskold += lksdv;
            }
            else
            {
                lksc = (lsumac + lnsc) - (lsumad + lnsd);
                lkscv = (lsumavalc + lnscv) - (lsumavald + lnsdv);
                ksc += lksc;
                kskolc += lkscv;
            }
            if (currentrow > 0)
            {
                var row1 = new List<string>();
                row1.Add("----------------------------------------------------------------------------------");
                row1.Add("|Сборно          |          л е в а              |           количество          |");
                row1.Add("|                |    дебит      |    кредит     |    дебит      |    кредит     |");
                row1.Add("----------------------------------------------------------------------------------");
                row1.Add($"|Начални салда   |{lnsd.ToString(Vf.LevFormatUI),15}|{lnsc.ToString(Vf.LevFormatUI),15}|{lnsdv.ToString(Vf.ValFormatUI),15}|{lnscv.ToString(Vf.ValFormatUI),15}|");
                row1.Add($"|Oбороти         |{lsumad.ToString(Vf.LevFormatUI),15}|{lsumac.ToString(Vf.LevFormatUI),15}|{lsumavald.ToString(Vf.ValFormatUI),15}|{lsumavalc.ToString(Vf.ValFormatUI),15}|");
                row1.Add($"|Сборове         |{lsbord.ToString(Vf.LevFormatUI),15}|{lsborc.ToString(Vf.LevFormatUI),15}|{lsbordv.ToString(Vf.ValFormatUI),15}|{lsborcv.ToString(Vf.ValFormatUI),15}|");
                row1.Add($"|Крайни салда    |{lksd.ToString(Vf.LevFormatUI),15}|{lksc.ToString(Vf.LevFormatUI),15}|{lksdv.ToString(Vf.ValFormatUI),15}|{lkscv.ToString(Vf.ValFormatUI),15}|");
                if (!string.IsNullOrWhiteSpace(KindStock))
                {
                    if (CurrenAcc.TypeAccount == 1)
                    {
                        row1.Add($"|Средна цена     |{(lksd / (lksdv != 0 ? lksdv : 1)).ToString(Vf.LevFormatUI),15}|               |               |               |");
                    }
                    else
                    {
                        row1.Add($"|Средна цена     |{(lksc / (lkscv != 0 ? lkscv : 1)).ToString(Vf.LevFormatUI),15}|               |               |               |");
                    }
                }
                row1.Add("----------------------------------------------------------------------------------");
                Rowfoother.Add(currentrow - 1, row1);
            }
            if (string.IsNullOrWhiteSpace(KindStock))
            {
                foreach (var item in rezi)
                {
                    List<string> item2 = new List<string>();
                    item2.Add("");
                    item2.Add("");
                    item2.Add("Само салдо");
                    item2.Add("");
                    item2.Add("");
                    item2.Add(string.Format(Vf.LevFormat, item.BeginSaldoDebit-item.BeginSaldoCredit));
                    if (CurrenAcc.TypeAccount == 1)
                    {
                        nsd += item.BeginSaldoDebit - item.BeginSaldoCredit;
                        nskold += item.BeginSaldoDebitKol - item.BeginSaldoCreditKol;
                    }
                    else
                    {
                        nsc += item.BeginSaldoCredit- item.BeginSaldoDebit ;
                        nskolc +=item.BeginSaldoCreditKol - item.BeginSaldoDebitKol;
                    }
                    item2.Add("");
                    item2.Add("");
                    item2.Add(string.Format(Vf.ValFormat, item.BeginSaldoDebitKol-item.BeginSaldoCreditKol));
                    
                    item2.Add("");
                    item2.Add("");
                    item2.Add("");
                    item2.Add(item.CodeMaterial);
                    item2.Add(item.NameMaterial);
                    item2.Add("");
                    item2.Add("");
                    items.Add(item2);
                    sumamd += item.BeginSaldoDebit-item.BeginSaldoCredit;
                    sumacolmd += item.BeginSaldoDebitKol-item.BeginSaldoCreditKol;
                    currentrow++;
                    var row1 = new List<string>();
                    row1.Add("----------------------------------------------------------------------------------");
                    row1.Add("|Сборно          |          л е в а              |           количества          |");
                    row1.Add("|                |    дебит      |    кредит     |    дебит      |    кредит     |");
                    row1.Add("----------------------------------------------------------------------------------");
                    row1.Add($"|Начални салда   |{(item.BeginSaldoDebit - item.BeginSaldoCredit).ToString(Vf.LevFormatUI),15}|               |{(item.BeginSaldoDebitKol-item.BeginSaldoCreditKol).ToString(Vf.KolFormatUI),15}|               |");
                    row1.Add($"|Oбороти         |{0.ToString(Vf.LevFormatUI),15}|{0.ToString(Vf.LevFormatUI),15}|{0.ToString(Vf.KolFormatUI),15}|{0.ToString(Vf.KolFormatUI),15}|");
                    row1.Add($"|Сборове         |{0.ToString(Vf.LevFormatUI),15}|{0.ToString(Vf.LevFormatUI),15}|{0.ToString(Vf.KolFormatUI),15}|{0.ToString(Vf.KolFormatUI),15}|");
                    row1.Add($"|Крайни салда    |{(item.BeginSaldoDebit - item.BeginSaldoCredit).ToString(Vf.LevFormatUI),15}|               |{(item.BeginSaldoDebitKol - item.BeginSaldoCreditKol).ToString(Vf.KolFormatUI),15}|               |");
                    //row1.Add($"|Средна цена     |{(item.BeginSaldoDebit / (item.BeginSaldoDebitKol != 0 ? item.BeginSaldoDebitKol : 1)).ToString(Vf.LevFormatUI),15}|               |               |               |");
                    row1.Add("----------------------------------------------------------------------------------");
                    Rowfoother.Add(currentrow - 1, row1);
                }
            }
            else
            {
                var item = rezi.FirstOrDefault(e => e.CodeMaterial == KindStock);
                if (item != null)
                {
                    List<string> item2 = new List<string>();
                    item2.Add("Само салдо");
                    item2.Add("");
                    item2.Add("");
                    item2.Add("");
                    item2.Add("");
                    item2.Add(string.Format(Vf.LevFormat, item.BeginSaldoDebit-item.BeginSaldoCredit));
                    BeginSaldoD += item.BeginSaldoDebit - item.BeginSaldoCredit;
                    item2.Add("");
                    item2.Add("");
                    item2.Add(string.Format(Vf.ValFormat, item.BeginSaldoDebitKol-item.BeginSaldoCreditKol));
                    BeginValSd += item.BeginSaldoDebitKol - item.BeginSaldoCreditKol;
                    item2.Add("");
                    item2.Add("");
                    item2.Add("");
                    item2.Add(item.CodeMaterial);
                    item2.Add(item.NameMaterial);
                    item2.Add("");
                    item2.Add("");
                    items.Add(item2);
                    sumamd += item.BeginSaldoDebit;
                    sumacolmd += item.BeginSaldoDebitKol;
                    currentrow++;
                    var row1 = new List<string>();
                    row1.Add("----------------------------------------------------------------------------------");
                    row1.Add("|Сборно          |          л е в а              |           количества          |");
                    row1.Add("|                |    дебит      |    кредит     |    дебит      |    кредит     |");
                    row1.Add("----------------------------------------------------------------------------------");
                    row1.Add($"|Начални салда   |{(item.BeginSaldoDebit-item.BeginSaldoCredit).ToString(Vf.LevFormatUI),15}|               |{(item.BeginSaldoDebitKol-item.BeginSaldoCreditKol).ToString(Vf.KolFormatUI),15}|               |");
                    row1.Add($"|Oбороти         |{0.ToString(Vf.LevFormatUI),15}|{0.ToString(Vf.LevFormatUI),15}|{0.ToString(Vf.KolFormatUI),15}|{0.ToString(Vf.KolFormatUI),15}|");
                    row1.Add($"|Сборове         |{0.ToString(Vf.LevFormatUI),15}|{0.ToString(Vf.LevFormatUI),15}|{0.ToString(Vf.KolFormatUI),15}|{0.ToString(Vf.KolFormatUI),15}|");
                    row1.Add($"|Крайни салда    |{(item.BeginSaldoDebit-item.BeginSaldoCredit).ToString(Vf.LevFormatUI),15}|               |{(item.BeginSaldoDebitKol - item.BeginSaldoCreditKol).ToString(Vf.KolFormatUI),15}|               |");
                    row1.Add($"|Средна цена     |{(item.BeginSaldoDebit / (item.BeginSaldoDebitKol != 0 ? item.BeginSaldoDebitKol : 1)).ToString(Vf.LevFormatUI),15}|               |               |               |");
                    row1.Add("----------------------------------------------------------------------------------");
                    Rowfoother.Add(currentrow - 1, row1);
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
                ? string.Format(Vf.LevFormat, sumad + nsd)
                : string.Format(Vf.LevFormat, sumad);
            TotalC = CurrenAcc.TypeAccount == 2
                ? string.Format(Vf.LevFormat, sumac + nsc)
                : string.Format(Vf.LevFormat, sumac);
            if (CurrenAcc.TypeAccount == 1)
            {
                BeginSaldoD = nsd;
                BeginValSd = nskold;
                BeginValSc = 0;
                BeginSaldoK = 0;
                KrainoSaldoD = (sumad + nsd) - (sumac);
                TSumavald = string.Format(Vf.KolFormat, sumaquantityd + nskold);
                TSumavalc = string.Format(Vf.KolFormat, sumaquantityc + nskolc);
                KrainoSaldoDV = (sumaquantityd + nskold) - (sumaquantityc);
                KrainoSaldoKV = 0;
                //var test = (sumad + BeginSaldoD) / (sumaquantityd + BeginValSd);
                //Sad = string.Format(Vf.LevFormat,test );
                //Sak ="";
            }
            if (CurrenAcc.TypeAccount == 2)
            {
                BeginSaldoK = nsc;
                BeginValSc = nskolc;
                BeginSaldoD = 0;
                BeginValSd = 0;
                KrainoSaldoK = (sumac + nsc) - (sumad);
                KrainoSaldoKV = (sumaquantityc + nskolc) - (sumaquantityd);
                TSumavald = string.Format(Vf.KolFormat, sumaquantityd + nskold);
                TSumavalc = string.Format(Vf.KolFormat, sumaquantityc + nskolc);
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
            if (string.IsNullOrWhiteSpace(KindStock))
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
                list.Add(string.Format("| Начално  салдо              |{0,17}|{1,18}|", BeginValSd, BeginValSc));
                list.Add(string.Format("| Обороти                     |{0,17}|{1,18}|", Sumavald, Sumavalc));
                list.Add(string.Format("| Сборове                     |{0,17}|{1,18}|", TSumavald, TSumavalc));
                list.Add(string.Format("| Крайно салдо                |{0,17}|{1,18}|", KrainoSaldoDV.ToString(Vf.KolFormatUI), ""));
                //list.Add(string.Format("| Средна цена                 |{0,17}|{1,18}|", Sad, ""));
                list.Add("--------------------------------------------------------------------");
                return list;
            }
            return null;
        }

        public string Filename
        {
            get { return "analitics"; }
        }

        public string Title
        {
            get;set; //{ return "AНАЛИТИЧЕН   РЕГИСТЪР - справка стоки"; }
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

        public string TSumavalc { get; set; }

        public string TSumavald { get; set; }

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
