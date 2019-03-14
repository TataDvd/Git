﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using ReportBuilder;
using Tempo2012.EntityFramework;
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.ViewModels;
using Tempo2012.UI.WPF.ViewModels.ContoManagment;
using System.IO;
using System.Diagnostics;

namespace Tempo2012.UI.WPF.Views.TetkaView
{
    public class FacturaComplexViewModel : BaseViewModel, IReportBuilder
    {
        public Dictionary<int, List<string>> Rowfoother { get; set; }
        private ContoViewModel cv;
        public FacturaComplexViewModel(AccountsModel accountsModel, ContoViewModel contoView,bool withContragentSum,bool onlyContragent=false,bool withletter=false,bool isval=false,string kindValuta=null)
        {
            _movements = new List<AccItemSaldo>();
            this.IsVal = isval;
            this.accountsModel = accountsModel;
            this.KindValuta = kindValuta;
            cv = contoView;
            this.WithContragentSum = withContragentSum;
            this.OnlyContragent = onlyContragent;
            var _reportItems = new List<ReportItem>();
            if (IsVal)
            {
                _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "Код", Width = 10 });
                _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "Контрагент", Width = 50 });
                _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "ЗДДС", Width = 15 });
                _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "НС Валута", Width = 15, IsSuma = true });
                _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "Оборот Дебит Валута", Width = 15, IsSuma = true });
                _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "Оборот Кредит Валута", Width = 15, IsSuma = true });
                _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "КС Валута", Width = 15, IsSuma = true });
            }
            else
            {
                _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "Код", Width = 10 });
                _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "Контрагент", Width = 50 });
                _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "ЗДДС", Width = 15 });
                _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "НС", Width = 15, IsSuma = true });
                _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "Оборот Дебит ", Width = 15, IsSuma = true });
                _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "Оборот Кредит", Width = 15, IsSuma = true });
                _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "КС", Width = 15, IsSuma = true });
            }
           // _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "КС Кредит", Width = 15, IsSuma = true });

            ReportItems = _reportItems;
            this.withletter = withletter;
        }
        public FacturaComplexViewModel(AccountsModel accountsModel,ContoViewModel contoView,bool WithContragentSum,string antetka,string contr = null, bool onlyContragent = false, bool withletter = false, bool isval = false,string kindvaluta=null)
        {
            _movements =new List<AccItemSaldo>();
            OnlyContragent = onlyContragent;
            cv = contoView;
            this.IsVal = isval;
            this.KindValuta = kindvaluta;
            this.accountsModel = accountsModel;
            this.antetka = antetka;
            this.WithContragentSum = WithContragentSum;
            var _reportItems = new List<ReportItem>();
            if (IsVal)
            {
                _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "Код", Width = 10 });
                _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "Контрагент", Width = 30 });
                _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "ЗДДС", Width = 15 });
                _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "Вид валута", Width = 15, IsSuma = true });
                _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "НС Валута", Width = 15, IsSuma = true });
                _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "Оборот Дебит Валута", Width = 15, IsSuma = true });
                _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "Оборот Кредит Валута", Width = 15, IsSuma = true });
                //_reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "КС Дебит ", Width = 15, IsSuma = true });
                _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "КС Валута", Width = 15, IsSuma = true });
            }
            else
            {
                _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "Код", Width = 10 });
                _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "Контрагент", Width = 30 });
                _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "ЗДДС", Width = 15 });
                //_reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "НС Дебит ", Width = 15, IsSuma = true });
                _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "НС", Width = 15, IsSuma = true });
                _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "Оборот Дебит ", Width = 15, IsSuma = true });
                _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "Оборот Кредит", Width = 15, IsSuma = true });
                //_reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "КС Дебит ", Width = 15, IsSuma = true });
                _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "КС", Width = 15, IsSuma = true });
            }
            ReportItems = _reportItems;
            filter=null;
            if (contr != null)
            {
                filter = string.Format("|{0} ", contr);
            }
            typerep = 1;
            this.withletter = withletter;
        }
        public void LoadSettings(string Path)
        {

            ReportItems = SerializeUtil.DeserializeFromXML<List<ReportItem>>(Path);

        }

        public void SaveSettings(string Path)
        {

            SerializeUtil.SerializeToXML<List<ReportItem>>(Path, ReportItems);

        }
        public ObservableCollection<InvoiseControl> AllMovementDebit { get; set;}
        public ObservableCollection<InvoiseControl> AllMovementCredit { get; set; }
        public ObservableCollection<InvoiseControl> AllMovementDebit1 { get; set;}
        public ObservableCollection<InvoiseControl> AllMovementCredit1 { get; set; }
        public AccSaldo AccSaldo { get; set;}
        public string AccInfo { get; set;}
        public string Period { get; set; }
        public ObservableCollection<AccItemSaldo> AllMovement { get; set; }
        private List<AccItemSaldo> _movements;
        private List<AccItemSaldo> _movements1;
        private AccItemSaldo _accItemSaldo;
        private decimal _suma;
        private Models.SaldoItem oldsaldo;
        private string p;
        private AccountsModel accountsModel;
        private string filter;
        private int typerep;
        private string antetka;
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
        private bool withletter;

        public AccItemSaldo AccItemSaldo
        {
            get { return _accItemSaldo; }
            set { _accItemSaldo = value; OnPropertyChanged("AccItemSaldo"); }
        }

        public decimal Suma
        {
            get { return _suma; }
            set { _suma = value; OnPropertyChanged("Suma");}
        }

        internal void All()
        {
           AllMovement=new ObservableCollection<AccItemSaldo>(_movements);
           OnPropertyChanged("AllMovement");
        }

        internal void Filter()
        {
            AllMovement = new ObservableCollection<AccItemSaldo>(_movements.Where(e => e.Ksd > 0));
            OnPropertyChanged("AllMovement");
        }

        internal void SaveConto(object item)
        {
            var it = item as AccItemSaldo;
            if (it == null) return;
            
            foreach (var saldoItem in cv.ItemsCredit)
            {
                if (saldoItem.Name.Contains("Дата на фактура"))
                {
                    saldoItem.ValueDate = it.Data;

                }
                if (saldoItem.Name.Contains("Номер фактура"))
                {
                    saldoItem.Value = it.NInvoise;
                }
                if (saldoItem.IsLookUp)
                {
                    oldsaldo = saldoItem;
                }
            }
            cv.CurrentWraperConto.Oborot = it.Ksd;
            cv.SaveF3();
            foreach (var saldoItem in cv.ItemsCredit)
            {
                
                if (saldoItem.IsLookUp)
                {
                    saldoItem.Bulstad = oldsaldo.Bulstad;
                    saldoItem.LookUp = oldsaldo.LookUp;
                    saldoItem.Value = oldsaldo.Value;
                    saldoItem.Lookupval = oldsaldo.Lookupval;
                    saldoItem.LiD = oldsaldo.LiD;
                }
            }
        }
        private static string CleanFileName(string fileName)
        {
            return Path.GetInvalidFileNameChars().Aggregate(fileName, (current, c) => current.Replace(c.ToString(), string.Empty));
        }
        public void savefile(string client, string content)
        {

            var path = Path.Combine(Entrence.CurrentFirmaPathTemplate, CleanFileName(client) + DateTime.Now.ToString("ddMMyyyy") + ".txt");
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.Write(content);
            }
        }
        public List<List<string>> GetItems()
        {
            if (this.IsVal)
            {
                return PokontragentValuta();
            }
            else
            {
                return Pokontragent();
            }
        }
        private List<List<string>> PokontragentValuta()
        {
            _movements = new List<AccItemSaldo>();
            _movements1 = new List<AccItemSaldo>();
            var items = new List<List<string>>();
            StringBuilder sb = new StringBuilder();

            string blanka = "";
            if (this.withletter)
            {
                blanka = File.ReadAllText(Path.Combine(Entrence.CurrentFirmaPathTemplate, "Pismo.txt"));
            }

            AllMovementDebit = new ObservableCollection<InvoiseControl>(Context.GetFullInvoiseContoDebit(accountsModel.Id).Where(e => e.DataInvoise < FromDate));
            AllMovementCredit = new ObservableCollection<InvoiseControl>(Context.GetFullInvoiseContoCredit(accountsModel.Id).Where(e => e.DataInvoise < FromDate));
            AllMovementDebit1 = new ObservableCollection<InvoiseControl>(Context.GetFullInvoiseContoDebit(accountsModel.Id).Where(e => e.DataInvoise >= FromDate && e.DataInvoise <= ToDate));
            AllMovementCredit1 = new ObservableCollection<InvoiseControl>(Context.GetFullInvoiseContoCredit(accountsModel.Id).Where(e => e.DataInvoise >= FromDate && e.DataInvoise <= ToDate));
            if (!string.IsNullOrWhiteSpace(KindValuta))
            {
                AllMovementDebit = new ObservableCollection<InvoiseControl>(AllMovementDebit.Where(e => e.VidValCode == KindValuta));
                AllMovementCredit = new ObservableCollection<InvoiseControl>(AllMovementCredit.Where(e => e.VidValCode == KindValuta));
                AllMovementDebit1 = new ObservableCollection<InvoiseControl>(AllMovementDebit1.Where(e => e.VidValCode == KindValuta));
                AllMovementCredit1 = new ObservableCollection<InvoiseControl>(AllMovementCredit1.Where(e => e.VidValCode == KindValuta));
            }
            if (!string.IsNullOrWhiteSpace(KindValuta))
            {
                AllMovementDebit = new ObservableCollection<InvoiseControl>(AllMovementDebit.Where(e => e.VidValCode == KindValuta));
                AllMovementCredit = new ObservableCollection<InvoiseControl>(AllMovementCredit.Where(e => e.VidValCode == KindValuta));
                AllMovementDebit1 = new ObservableCollection<InvoiseControl>(AllMovementDebit1.Where(e => e.VidValCode == KindValuta));
                AllMovementCredit1 = new ObservableCollection<InvoiseControl>(AllMovementCredit1.Where(e => e.VidValCode == KindValuta));
            }
            var rezi = Context.GetAllAnaliticSaldos(accountsModel.Id, accountsModel.FirmaId,KindValuta);
            if (typerep == 1 && filter != null)
            {
                string contr = "";
                var filt = filter.Split('|');
                if (filt.Length > 0)
                {
                    contr = filt[1].Trim();
                    AllMovementDebit = new ObservableCollection<InvoiseControl>(AllMovementDebit.Where(w => w.CodeContragent == contr));
                    AllMovementCredit = new ObservableCollection<InvoiseControl>(AllMovementCredit.Where(w => w.CodeContragent == contr));
                    AllMovementDebit1 = new ObservableCollection<InvoiseControl>(AllMovementDebit1.Where(w => w.CodeContragent == contr));
                    AllMovementCredit1 = new ObservableCollection<InvoiseControl>(AllMovementCredit1.Where(w => w.CodeContragent == contr));
                    rezi = new List<SaldoFactura>(rezi.Where(t => t.NameContragent == contr));
                }

            }
            int luki = 0;
            if (AllMovementDebit1.Count > 0)
            {
                luki = AllMovementDebit1.Max(e => e.CID);
            }
            else
            {
                if (AllMovementCredit1.Count > 0)
                {
                    luki = AllMovementCredit1.Max(e => e.CID);
                }
                else
                {
                    if (rezi.Count > 0)
                    {
                        luki = rezi.Max(e => e.LookupId);
                    }
                    else
                    {
                        if (AllMovementCredit.Count > 0)
                        {
                            luki = AllMovementCredit.Max(e => e.CID);
                        }
                        else
                        {
                            if (AllMovementDebit.Count > 0)
                            {
                                luki = AllMovementDebit.Max(e => e.CID);
                            }
                        }
                    }
                }
            }
            List<Dictionary<string, object>> lookup = null;
            if (luki > 0)
            {
                var look = Context.GetLookup(luki);
                lookup = Context.GetLookupDictionary(look.LookUpMetaData.Tablename, ConfigTempoSinglenton.GetInstance().CurrentFirma.Id).ToList();
            }
            foreach (InvoiseControl invoiseControl in AllMovementDebit)
            {
                var item = new AccItemSaldo();
                item.NInvoise = invoiseControl.NInvoise;
                item.NameContragent = invoiseControl.NameContragent;
                item.VidValCode = invoiseControl.VidValCode;
                item.Code = invoiseControl.CodeContragent;
                item.Odv = invoiseControl.OborotValuta;
                item.Type = accountsModel.TypeAccount;
                item.Data = invoiseControl.DataInvoise;
                if (item.Type == 1) item.Data = invoiseControl.DataInvoise;
                var lc =
                    AllMovementCredit.FirstOrDefault(
                        w => w.CodeContragent == invoiseControl.CodeContragent && w.NInvoise == invoiseControl.NInvoise);
                if (lc != null)
                {

                    item.Ocv += lc.OborotValuta;
                    if (item.Type == 2) item.Data = lc.DataInvoise;
                    AllMovementCredit.Remove(lc);
                }
                _movements1.Add(item);

            }
            foreach (InvoiseControl invoiseControl in AllMovementCredit)
            {
                var item = new AccItemSaldo();
                item.NInvoise = invoiseControl.NInvoise;
                item.NameContragent = invoiseControl.NameContragent;
                item.Code = invoiseControl.CodeContragent;
                item.VidValCode = invoiseControl.VidValCode;
                item.Ocv = invoiseControl.OborotValuta;
                item.Type = accountsModel.TypeAccount;
                item.Data = invoiseControl.DataInvoise;

                _movements1.Add(item);

            }
            foreach (AccItemSaldo accItemSaldo in _movements1)
            {
                var saldo =
                    rezi.FirstOrDefault(
                        m => m.Code == accItemSaldo.Code);
                if (saldo != null)
                {
                    accItemSaldo.Nsdv = saldo.BeginSaldoDebitValuta;
                    accItemSaldo.Nscv = saldo.BeginSaldoCreditValuta;
                    rezi.Remove(saldo);
                }
            }
            foreach (var item in rezi.OrderBy(e => e.Code))
            {
                var item1 = new AccItemSaldo();
                item1.NInvoise = item.NumInvoise;
                item1.NameContragent = item.NameContragent;
                item1.VidValCode = item.CodeValuta;
                item1.Code = item.Code;
                item1.Odv = 0;
                item1.Ocv = 0;
                item1.Data = item.Date;
                item1.Nsdv = item.BeginSaldoDebitValuta;
                item1.Nscv = item.BeginSaldoCreditValuta;
                item1.Type = accountsModel.TypeAccount;
                _movements1.Add(item1);
            }

           
            foreach (InvoiseControl invoiseControl in AllMovementDebit1)
            {
                var item = new AccItemSaldo();
                item.NInvoise = invoiseControl.NInvoise;
                item.NameContragent = invoiseControl.NameContragent;
                item.VidValCode = invoiseControl.VidValCode;
                item.Code = invoiseControl.CodeContragent;
                item.Odv = invoiseControl.OborotValuta;
                item.Type = accountsModel.TypeAccount;
                item.Data = invoiseControl.DataInvoise;
                if (item.Type == 1) item.Data = invoiseControl.DataInvoise;
                var lc =
                    AllMovementCredit1.FirstOrDefault(
                        w => w.CodeContragent == invoiseControl.CodeContragent && w.NInvoise == invoiseControl.NInvoise);
                if (lc != null)
                {

                    item.Ocv += lc.OborotValuta;
                    if (item.Type == 2) item.Data = lc.DataInvoise;
                    AllMovementCredit1.Remove(lc);
                }
                _movements.Add(item);

            }
            foreach (InvoiseControl invoiseControl in AllMovementCredit1)
            {
                var item = new AccItemSaldo();
                item.NInvoise = invoiseControl.NInvoise;
                item.NameContragent = invoiseControl.NameContragent;
                item.Code = invoiseControl.CodeContragent;
                item.VidValCode = invoiseControl.VidValCode;
                item.Ocv = invoiseControl.OborotValuta;
                item.Type = accountsModel.TypeAccount;
                item.Data = invoiseControl.DataInvoise;

                _movements.Add(item);

            }
            foreach (AccItemSaldo accItemSaldo in _movements)
            {
                var saldo =
                    _movements1.FirstOrDefault(
                        m => m.Code == accItemSaldo.Code);
                if (saldo != null)
                {
                    accItemSaldo.Nsdv = saldo.Ksdv;
                    accItemSaldo.Nscv = saldo.Kscv;
                    _movements1.Remove(saldo);
                }
            }
            foreach (var item in _movements1.OrderBy(e => e.Code))
            {
                var item1 = new AccItemSaldo();
                item1.NInvoise = item.NInvoise;
                item1.NameContragent = item.NameContragent;
                item1.VidValCode = item.VidValCode;
                item1.Code = item.Code;

                item1.Odv = 0;
                item1.Data = item.Data;
                item1.Nsdv = item.Ksdv;
                item1.Nscv = item.Kscv;
                item1.Type = accountsModel.TypeAccount;
                _movements.Add(item1);
            }

            if (typerep == 1 && filter!=null)
            {
                string contr = "";
                var filt = filter.Split('|');
                if (filt.Length > 0)
                {
                    contr = filt[1].Trim();
                    _movements = new List<AccItemSaldo>(_movements.Where(w => w.Code == contr));

                }

            }
            string name = "";
            string code = "";
            string zdds = "";
            string vidvaluta = "";
            bool first = true;

            decimal sumansc = 0;
            decimal sumaOc = 0;
            decimal sumansd = 0;
            decimal sumaOd = 0;

            decimal sumaOct = 0;

            decimal sumaOdt = 0;

            decimal totalns = 0;
            decimal totalks = 0;
            decimal totalod = 0;
            decimal totalok = 0;
            foreach (AccItemSaldo itemSaldo in _movements.OrderBy(m => m.Cod))
            {
                List<string> row = new List<string>();
                if (first)
                {
                    name = itemSaldo.NameContragent;
                    code = itemSaldo.Code;
                    vidvaluta= itemSaldo.VidValCode;
                    if (lookup != null)
                    {
                        var vat = lookup.FirstOrDefault(x => x.ContainsKey("VAT") && x["KONTRAGENT"].ToString() == itemSaldo.Code);
                        if (vat != null)
                        {
                            zdds = vat["VAT"].ToString();
                        }
                    }
                    else
                    {
                        zdds = "n/a";
                    }
                    first = false;

                }
                else
                {
                    if (code != itemSaldo.Code && WithContragentSum)
                    {

                        List<string> rowTotal = new List<string>();
                        if (!OnlyContragent)
                        {
                            rowTotal.Add("");
                            rowTotal.Add("");
                            rowTotal.Add("");
                            rowTotal.Add("");
                        }
                        else
                        {
                            rowTotal.Add(code);
                            rowTotal.Add(name);
                            rowTotal.Add(zdds);
                            rowTotal.Add(vidvaluta);
                        }
                        //rowTotal.Add("");
                        //rowTotal.Add(" Общо :");
                        //row.Add(itemSaldo.Nsd.ToString(Vf.LevFormatUI));
                        if (accountsModel.TypeAccount != 1)
                        {
                            var ks = (sumansc + sumaOc) - (sumansd + sumaOd);
                            var ns = sumansc - sumansd;
                            rowTotal.Add(ns.ToString(Vf.LevFormatUI));
                            rowTotal.Add(sumaOd.ToString(Vf.LevFormatUI));
                            rowTotal.Add(sumaOc.ToString(Vf.LevFormatUI));
                            rowTotal.Add(ks.ToString(Vf.LevFormatUI));
                            if (this.withletter && ks > 0)
                            {
                                savefile(name, ReplaceBlanka(blanka, name, rowTotal));
                            }
                        }
                        else
                        {
                            var ks = (sumansd + sumaOd) - (sumansc + sumaOc);
                            var ns = sumansd - sumansc;
                            rowTotal.Add(ns.ToString(Vf.LevFormatUI));
                            rowTotal.Add(sumaOd.ToString(Vf.LevFormatUI));
                            rowTotal.Add(sumaOc.ToString(Vf.LevFormatUI));
                            rowTotal.Add(ks.ToString(Vf.LevFormatUI));
                            if (this.withletter && ks > 0)
                            {
                                savefile(name, ReplaceBlanka(blanka, name, rowTotal));
                            }
                        }
                        items.Add(rowTotal);

                        sumansc = 0;
                        sumansd = 0;
                        sumaOc = 0;
                        sumaOd = 0;
                        name = itemSaldo.NameContragent;
                        code = itemSaldo.Code;
                        vidvaluta = itemSaldo.VidValCode;
                        if (lookup != null)
                        {
                            var vat = lookup.FirstOrDefault(x => x.ContainsKey("VAT") && x["KONTRAGENT"].ToString() == code);
                            if (vat != null)
                            {
                                zdds = vat["VAT"].ToString();
                            }
                        }
                        else
                        {
                            zdds = "n/a";
                        }
                    }
                }

                sumansc += itemSaldo.Nscv;
                sumansd += itemSaldo.Nsdv;
                sumaOc += itemSaldo.Ocv;
                sumaOd += itemSaldo.Odv;
                sumaOct += itemSaldo.Ocv;
                sumaOdt += itemSaldo.Odv;
                //row.Add(itemSaldo.Nsd.ToString(Vf.LevFormatUI));
                if (accountsModel.TypeAccount != 1)
                {
                    itemSaldo.Ksv = (itemSaldo.Nscv + itemSaldo.Ocv) - (itemSaldo.Nsdv + itemSaldo.Odv);
                    itemSaldo.Nsv = itemSaldo.Nscv - itemSaldo.Nsdv;
                }
                else
                {
                    itemSaldo.Ksv = (itemSaldo.Nsdv + itemSaldo.Odv) - (itemSaldo.Nscv + itemSaldo.Ocv);
                    itemSaldo.Nsv = itemSaldo.Nsdv - itemSaldo.Nscv;
                }

                totalns += itemSaldo.Nsv;
                totalks += itemSaldo.Ksv;
                totalod += itemSaldo.Odv;
                totalok += itemSaldo.Ocv;

            }
            if (WithContragentSum)
            {

                List<string> rowTotalLas = new List<string>();
                if (!OnlyContragent)
                {
                    rowTotalLas.Add("");
                    rowTotalLas.Add("");
                    rowTotalLas.Add("");
                    rowTotalLas.Add("");
                }
                else
                {
                    rowTotalLas.Add(code);
                    rowTotalLas.Add(name);
                    rowTotalLas.Add(zdds);
                    rowTotalLas.Add(vidvaluta);
                }
                //rowTotalLas.Add("");
                //rowTotalLas.Add(" Общо :");
                //row.Add(itemSaldo.Nsd.ToString(Vf.LevFormatUI));
                if (accountsModel.TypeAccount != 1)
                {
                    var ks = (sumansc + sumaOc) - (sumansd + sumaOd);
                    var ns = sumansc - sumansd;
                    rowTotalLas.Add(ns.ToString(Vf.LevFormatUI));
                    rowTotalLas.Add(sumaOd.ToString(Vf.LevFormatUI));
                    rowTotalLas.Add(sumaOc.ToString(Vf.LevFormatUI));
                    rowTotalLas.Add(ks.ToString(Vf.LevFormatUI));
                    if (this.withletter && ks > 0)
                    {
                        savefile(name, ReplaceBlanka(blanka, name, rowTotalLas));
                    }

                }
                else
                {
                    var ks = (sumansd + sumaOd) - (sumansc + sumaOc);
                    var ns = sumansd - sumansc;
                    rowTotalLas.Add(ns.ToString(Vf.LevFormatUI));
                    rowTotalLas.Add(sumaOd.ToString(Vf.LevFormatUI));
                    rowTotalLas.Add(sumaOc.ToString(Vf.LevFormatUI));
                    rowTotalLas.Add(ks.ToString(Vf.LevFormatUI));
                    if (this.withletter && ks > 0)
                    {
                        savefile(name, ReplaceBlanka(blanka, name, rowTotalLas));
                    }
                }
                items.Add(rowTotalLas);
            }
            items.Add(new List<string> {
                            "-----------------------------------",
                            "-----------------------------------",
                            "-----------------------------------",
                            "-----------------------------------",
                            "-----------------------------------",
                            "-----------------------------------",
                            "-----------------------------------",
                            "-----------------------------------"
                             });
            List<string> rowTotalLast = new List<string>();
            rowTotalLast.Add("");
            rowTotalLast.Add("");
            rowTotalLast.Add("");
            rowTotalLast.Add(" Общо :");
            rowTotalLast.Add(totalns.ToString(Vf.LevFormatUI));
            rowTotalLast.Add(totalod.ToString(Vf.LevFormatUI));
            rowTotalLast.Add(totalok.ToString(Vf.LevFormatUI));
            rowTotalLast.Add(totalks.ToString(Vf.LevFormatUI));
            items.Add(rowTotalLast);

            return items;
        }
        private List<List<string>> Pokontragent()
        {
            _movements = new List<AccItemSaldo>();
            _movements1 = new List<AccItemSaldo>();
            var items = new List<List<string>>();
            StringBuilder sb = new StringBuilder();

            string blanka = "";
            if (this.withletter)
            {
                blanka = File.ReadAllText(Path.Combine(Entrence.CurrentFirmaPathTemplate, "Pismo.txt"));
            }

            AllMovementDebit = new ObservableCollection<InvoiseControl>(Context.GetFullInvoiseContoDebit(accountsModel.Id, true).Where(e => e.DataInvoise < FromDate));
            AllMovementCredit = new ObservableCollection<InvoiseControl>(Context.GetFullInvoiseContoCredit(accountsModel.Id, true).Where(e => e.DataInvoise < FromDate));
            AllMovementDebit1 = new ObservableCollection<InvoiseControl>(Context.GetFullInvoiseContoDebit(accountsModel.Id, true).Where(e => e.DataInvoise >= FromDate && e.DataInvoise <= ToDate));
            AllMovementCredit1 = new ObservableCollection<InvoiseControl>(Context.GetFullInvoiseContoCredit(accountsModel.Id, true).Where(e => e.DataInvoise >= FromDate && e.DataInvoise <= ToDate));
            if (!string.IsNullOrWhiteSpace(KindValuta))
            {
                AllMovementDebit = new ObservableCollection<InvoiseControl>(AllMovementDebit.Where(e => e.VidValCode == KindValuta));
                AllMovementCredit = new ObservableCollection<InvoiseControl>(AllMovementCredit.Where(e => e.VidValCode == KindValuta));
                AllMovementDebit1 = new ObservableCollection<InvoiseControl>(AllMovementDebit1.Where(e => e.VidValCode == KindValuta));
                AllMovementCredit1 = new ObservableCollection<InvoiseControl>(AllMovementCredit1.Where(e => e.VidValCode == KindValuta));
            }
            var rezi = Context.GetAllAnaliticSaldos(accountsModel.Id, accountsModel.FirmaId,KindValuta);
            int luki = 0;
            if (AllMovementDebit1.Count > 0)
            {
                luki = AllMovementDebit1.Max(e => e.CID);
            }
            else
            {
                if (AllMovementCredit1.Count > 0)
                {
                    luki = AllMovementCredit1.Max(e => e.CID);
                }
                else
                {
                    if (rezi.Count > 0)
                    {
                        luki = rezi.Max(e => e.LookupId);
                    }
                    else
                    {
                        if (AllMovementCredit.Count > 0)
                        {
                            luki = AllMovementCredit.Max(e => e.CID);
                        }
                        else
                        {
                            if (AllMovementDebit.Count > 0)
                            {
                                luki = AllMovementDebit.Max(e => e.CID);
                            }
                        }
                    }
                }
            }
            List<Dictionary<string, object>> lookup = null;
            if (luki > 0)
            {
                var look = Context.GetLookup(luki);
                lookup = Context.GetLookupDictionary(look.LookUpMetaData.Tablename, ConfigTempoSinglenton.GetInstance().CurrentFirma.Id).ToList();
            }
            foreach (InvoiseControl invoiseControl in AllMovementDebit)
            {
                var item = new AccItemSaldo();
                item.NInvoise = invoiseControl.NInvoise;
                item.NameContragent = invoiseControl.NameContragent;
                item.VidValCode = invoiseControl.VidValCode;
                item.Code = invoiseControl.CodeContragent;
                item.Od = invoiseControl.Oborot;
                item.Type = accountsModel.TypeAccount;
                item.Data = invoiseControl.DataInvoise;
                if (item.Type == 1) item.Data = invoiseControl.DataInvoise;
                var lc =
                    AllMovementCredit.FirstOrDefault(
                        w => w.CodeContragent == invoiseControl.CodeContragent && w.NInvoise == invoiseControl.NInvoise);
                if (lc != null)
                {

                    item.Oc += lc.Oborot;
                    if (item.Type == 2) item.Data = lc.DataInvoise;
                    AllMovementCredit.Remove(lc);
                }
                _movements1.Add(item);

            }
            foreach (InvoiseControl invoiseControl in AllMovementCredit)
            {
                var item = new AccItemSaldo();
                item.NInvoise = invoiseControl.NInvoise;
                item.NameContragent = invoiseControl.NameContragent;
                item.Code = invoiseControl.CodeContragent;
                item.VidValCode = invoiseControl.VidValCode;
                item.Oc = invoiseControl.Oborot;
                item.Type = accountsModel.TypeAccount;
                item.Data = invoiseControl.DataInvoise;

                _movements1.Add(item);

            }
            foreach (AccItemSaldo accItemSaldo in _movements1)
            {
                var saldo =
                    rezi.FirstOrDefault(
                        m => m.Code == accItemSaldo.Code);
                if (saldo != null)
                {
                    accItemSaldo.Nsd = saldo.BeginSaldoDebit;
                    accItemSaldo.Nsc = saldo.BeginSaldoCredit;
                    rezi.Remove(saldo);
                }
            }
            foreach (var item in rezi.OrderBy(e => e.Code))
            {
                var item1 = new AccItemSaldo();
                item1.NInvoise = item.NumInvoise;
                item1.NameContragent = item.NameContragent;
                item1.VidValCode = item.CodeValuta;
                item1.Code = item.Code;
                item1.Od = 0;
                item1.Oc = 0;
                item1.Data = item.Date;
                item1.Nsd = item.BeginSaldoDebit;
                item1.Nsc = item.BeginSaldoCredit;
                item1.Type = accountsModel.TypeAccount;
                _movements1.Add(item1);
            }

            if (typerep == 1 && filter!=null)
            {
                string contr = "";
                var filt = filter.Split('|');
                if (filt.Length > 0)
                {
                    contr = filt[1].Trim();
                    _movements1 = new List<AccItemSaldo>(_movements1.Where(w => w.Code == contr));

                }

            }
            foreach (InvoiseControl invoiseControl in AllMovementDebit1)
            {
                var item = new AccItemSaldo();
                item.NInvoise = invoiseControl.NInvoise;
                item.NameContragent = invoiseControl.NameContragent;
                item.Code = invoiseControl.CodeContragent;
                item.VidValCode = invoiseControl.VidValCode;
                item.Od = invoiseControl.Oborot;
                item.Type = accountsModel.TypeAccount;
                item.Data = invoiseControl.DataInvoise;
                if (item.Type == 1) item.Data = invoiseControl.DataInvoise;
                var lc =
                    AllMovementCredit1.FirstOrDefault(
                        w => w.CodeContragent == invoiseControl.CodeContragent && w.NInvoise == invoiseControl.NInvoise);
                if (lc != null)
                {

                    item.Oc += lc.Oborot;
                    if (item.Type == 2) item.Data = lc.DataInvoise;
                    AllMovementCredit1.Remove(lc);
                }
                _movements.Add(item);

            }
            foreach (InvoiseControl invoiseControl in AllMovementCredit1)
            {
                var item = new AccItemSaldo();
                item.NInvoise = invoiseControl.NInvoise;
                item.NameContragent = invoiseControl.NameContragent;
                item.VidValCode = invoiseControl.VidValCode;
                item.Code = invoiseControl.CodeContragent;

                item.Oc = invoiseControl.Oborot;
                item.Type = accountsModel.TypeAccount;
                item.Data = invoiseControl.DataInvoise;

                _movements.Add(item);

            }
            foreach (AccItemSaldo accItemSaldo in _movements)
            {
                var saldo =
                    _movements1.FirstOrDefault(
                        m => m.Code == accItemSaldo.Code);
                if (saldo != null)
                {
                    accItemSaldo.Nsd = saldo.Ksd;
                    accItemSaldo.Nsc = saldo.Ksc;
                    _movements1.Remove(saldo);
                }
            }
            foreach (var item in _movements1.OrderBy(e => e.Code))
            {
                var item1 = new AccItemSaldo();
                item1.NInvoise = item.NInvoise;
                item1.NameContragent = item.NameContragent;
                item1.VidValCode = item.VidValCode;
                item1.Code = item.Code;

                item1.Od = 0;
                item1.Data = item.Data;
                item1.Nsd = item.Ksd;
                item1.Nsc = item.Ksc;
                item1.Type = accountsModel.TypeAccount;
                _movements.Add(item1);
            }

            if (typerep == 1 && filter != null) 
            {
                string contr = "";
                var filt = filter.Split('|');
                if (filt.Length > 0)
                {
                    contr = filt[1].Trim();
                    _movements = new List<AccItemSaldo>(_movements.Where(w => w.Code == contr));

                }

            }
            string name = "";
            string code = "";
            string zdds = "";
            string kindval = "";
            bool first = true;

            decimal sumansc = 0;
            decimal sumaOc = 0;
            decimal sumansd = 0;
            decimal sumaOd = 0;

            decimal sumaOct = 0;

            decimal sumaOdt = 0;

            decimal totalns = 0;
            decimal totalks = 0;
            decimal totalod = 0;
            decimal totalok = 0;
            foreach (AccItemSaldo itemSaldo in _movements.OrderBy(m => m.Cod))
            {
                List<string> row = new List<string>();
                if (first)
                {
                    name = itemSaldo.NameContragent;
                    code = itemSaldo.Code;
                    kindval = itemSaldo.VidValCode;
                    if (lookup != null)
                    {
                        var vat = lookup.FirstOrDefault(x => x.ContainsKey("VAT") && x["KONTRAGENT"].ToString() == itemSaldo.Code);
                        if (vat != null)
                        {
                            zdds = vat["VAT"].ToString();
                        }
                    }
                    else
                    {
                        zdds = "n/a";
                    }
                    first = false;

                }
                else
                {
                    if (code != itemSaldo.Code && WithContragentSum)
                    {

                        List<string> rowTotal = new List<string>();
                        if (!OnlyContragent)
                        {
                            rowTotal.Add("");
                            rowTotal.Add("");
                            rowTotal.Add("");
                            //rowTotal.Add("");
                        }
                        else
                        {
                            rowTotal.Add(code);
                            rowTotal.Add(name);
                            rowTotal.Add(zdds);
                            //rowTotal.Add(kindval);
                        }
                        //rowTotal.Add("");
                        //rowTotal.Add(" Общо :");
                        //row.Add(itemSaldo.Nsd.ToString(Vf.LevFormatUI));
                        if (accountsModel.TypeAccount != 1)
                        {
                            var ks = (sumansc + sumaOc) - (sumansd + sumaOd);
                            var ns = sumansc - sumansd;
                            rowTotal.Add(ns.ToString(Vf.LevFormatUI));
                            rowTotal.Add(sumaOd.ToString(Vf.LevFormatUI));
                            rowTotal.Add(sumaOc.ToString(Vf.LevFormatUI));
                            rowTotal.Add(ks.ToString(Vf.LevFormatUI));
                            if (this.withletter && ks > 0)
                            {
                                savefile(name, ReplaceBlanka(blanka, name, rowTotal));
                            }
                        }
                        else
                        {
                            var ks = (sumansd + sumaOd) - (sumansc + sumaOc);
                            var ns = sumansd - sumansc;
                            rowTotal.Add(ns.ToString(Vf.LevFormatUI));
                            rowTotal.Add(sumaOd.ToString(Vf.LevFormatUI));
                            rowTotal.Add(sumaOc.ToString(Vf.LevFormatUI));
                            rowTotal.Add(ks.ToString(Vf.LevFormatUI));
                            if (this.withletter && ks > 0)
                            {
                                savefile(name, ReplaceBlanka(blanka, name, rowTotal));
                            }
                        }
                        items.Add(rowTotal);

                        sumansc = 0;
                        sumansd = 0;
                        sumaOc = 0;
                        sumaOd = 0;
                        name = itemSaldo.NameContragent;
                        code = itemSaldo.Code;
                        kindval = itemSaldo.VidValCode;
                        if (lookup != null)
                        {
                            var vat = lookup.FirstOrDefault(x => x.ContainsKey("VAT") && x["KONTRAGENT"].ToString() == code);
                            if (vat != null)
                            {
                                zdds = vat["VAT"].ToString();
                            }
                        }
                        else
                        {
                            zdds = "n/a";
                        }
                    }
                }

                sumansc += itemSaldo.Nsc;
                sumansd += itemSaldo.Nsd;
                sumaOc += itemSaldo.Oc;
                sumaOd += itemSaldo.Od;
                sumaOct += itemSaldo.Oc;
                sumaOdt += itemSaldo.Od;
                //row.Add(itemSaldo.Nsd.ToString(Vf.LevFormatUI));
                if (accountsModel.TypeAccount != 1)
                {
                    itemSaldo.Ks = (itemSaldo.Nsc + itemSaldo.Oc) - (itemSaldo.Nsd + itemSaldo.Od);
                    itemSaldo.Ns = itemSaldo.Nsc - itemSaldo.Nsd;
                }
                else
                {
                    itemSaldo.Ks = (itemSaldo.Nsd + itemSaldo.Od) - (itemSaldo.Nsc + itemSaldo.Oc);
                    itemSaldo.Ns = itemSaldo.Nsd - itemSaldo.Nsc;
                }

                totalns += itemSaldo.Ns;
                totalks += itemSaldo.Ks;
                totalod += itemSaldo.Od;
                totalok += itemSaldo.Oc;

            }
            if (WithContragentSum)
            {

                List<string> rowTotalLas = new List<string>();
                if (!OnlyContragent)
                {
                    rowTotalLas.Add("");
                    rowTotalLas.Add("");
                    rowTotalLas.Add("");
                    //rowTotalLas.Add("");
                }
                else
                {

                    rowTotalLas.Add(code);
                    rowTotalLas.Add(name);
                    rowTotalLas.Add(zdds);
                    //rowTotalLas.Add(kindval);
                }
                //rowTotalLas.Add("");
                //rowTotalLas.Add(" Общо :");
                //row.Add(itemSaldo.Nsd.ToString(Vf.LevFormatUI));
                if (accountsModel.TypeAccount != 1)
                {
                    var ks = sumansc + sumaOc - sumansd - sumaOd;
                    var ns = sumansc - sumansd;
                    rowTotalLas.Add(ns.ToString(Vf.LevFormatUI));
                    rowTotalLas.Add(sumaOd.ToString(Vf.LevFormatUI));
                    rowTotalLas.Add(sumaOc.ToString(Vf.LevFormatUI));
                    rowTotalLas.Add(ks.ToString(Vf.LevFormatUI));
                    if (this.withletter && ks > 0)
                    {
                        savefile(name, ReplaceBlanka(blanka, name, rowTotalLas));
                    }

                }
                else
                {
                    var ks = sumansd + sumaOd - sumansc - sumaOc;
                    var ns = sumansd - sumansc;
                    rowTotalLas.Add(ns.ToString(Vf.LevFormatUI));
                    rowTotalLas.Add(sumaOd.ToString(Vf.LevFormatUI));
                    rowTotalLas.Add(sumaOc.ToString(Vf.LevFormatUI));
                    rowTotalLas.Add(ks.ToString(Vf.LevFormatUI));
                    if (this.withletter && ks > 0)
                    {
                        savefile(name, ReplaceBlanka(blanka, name, rowTotalLas));
                    }
                }
                items.Add(rowTotalLas);
            }
            items.Add(new List<string> {
                            "-----------------------------------",
                            "-----------------------------------",
                            "-----------------------------------",
                            "-----------------------------------",
                            "-----------------------------------",
                            "-----------------------------------",
                            "-----------------------------------"
                             });
            List<string> rowTotalLast = new List<string>();
            rowTotalLast.Add("");
            rowTotalLast.Add("");
            //rowTotalLast.Add("");
            rowTotalLast.Add(" Общо :");
            rowTotalLast.Add(totalns.ToString(Vf.LevFormatUI));
            rowTotalLast.Add(totalod.ToString(Vf.LevFormatUI));
            rowTotalLast.Add(totalok.ToString(Vf.LevFormatUI));
            rowTotalLast.Add(totalks.ToString(Vf.LevFormatUI));
            items.Add(rowTotalLast);

            return items;
        }

        private string ReplaceBlanka(string blanka, string name, List<string> rowTotalLas)
        {
            return blanka.Replace("@client", name)
                         .Replace("@ns", rowTotalLas[2])
                         .Replace("@do", rowTotalLas[3])
                         .Replace("@ko", rowTotalLas[4])
                         .Replace("@ks", rowTotalLas[5])
                         .Replace("@year", ToDate.Year.ToString()) 
                         .Replace("@firma",ConfigTempoSinglenton.GetInstance().CurrentFirma.Name)
                         .Replace("@currentdate", string.Format("{0}/{1}/{2}",ConfigTempoSinglenton.GetInstance().WorkDate.Day
                                                                             ,ConfigTempoSinglenton.GetInstance().WorkDate.Month
                                                                             ,ConfigTempoSinglenton.GetInstance().WorkDate.Year));

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
            ret.Add(string.Format("От дата {0} до дата {1}", FromDate.ToZeroString(), ToDate.ToZeroString()));
            return ret;
        }

        public List<string> GetFuther()
        {
            return null;
        }

        public string Filename
        {
            get { return "Invoises";}
        }

        public string Title
        {
            get
            {
                string b = "Справка Фактури обобщена за контрагент за сметка ";
                if (this.IsVal)  b = "Справка Фактури обобщена за контрагент и валута за сметка ";
                string a=b + this.accountsModel.ShortName;
                if (typerep == 1)
                {
                    if (KindValuta != null)
                    {
                        return a + "  " + antetka + " за тип валута " + KindValuta;
                    }
                    return a + "  " + antetka;
                }
                if (KindValuta != null)
                    {
                        return a +" за тип валута " + KindValuta;
                    }
                return a;
            }
        }

        public IEnumerable<ReportItem> ReportItems { get; set;}
        public bool WithContragentSum { get;  set; }
        public bool OnlyContragent { get; private set; }
        public bool IsVal { get; private set; }
        public string KindValuta { get; private set; }

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
