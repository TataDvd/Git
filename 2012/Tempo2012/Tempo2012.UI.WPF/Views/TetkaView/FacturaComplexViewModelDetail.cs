using System;
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

namespace Tempo2012.UI.WPF.Views.TetkaView
{
    public class FacturaComplexViewModelDetail : BaseViewModel, IReportBuilder
    {
        public Dictionary<int, List<string>> Rowfoother { get; set; }
        private ContoViewModel cv;
        public FacturaComplexViewModelDetail(AccountsModel accountsModel, ContoViewModel contoView,bool withContragentSum,bool onlyContragent=false)
        {
            _movements = new List<AccItemSaldo>();
            this.accountsModel = accountsModel;
            cv = contoView;
            this.WithContragentSum = withContragentSum;
            this.OnlyContragent = onlyContragent;
            var _reportItems = new List<ReportItem>();
            _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "Код", Width = 10 });
            _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "Контрагент", Width = 50 });
            _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "Номер Фактура", Width = 10 });
            _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "Дата", Width = 10 });
            // _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "НС Дебит ", Width = 15, IsSuma = true });
            _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "НС", Width = 15, IsSuma = true });
            _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "Оборот Дебит ", Width = 15, IsSuma = true });
            _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "Оборот Кредит", Width = 15, IsSuma = true });
            _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "КС", Width = 15, IsSuma = true });
            _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "Папка", Width = 15, IsSuma = false });
            _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "Номер Документ", Width = 15, IsSuma = false });
            _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "Основание", Width = 30, IsSuma = false });
            _reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Признак 1", Width = 10 });
            _reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Признак 2", Width = 10 });
            ReportItems = _reportItems;
        }
        public FacturaComplexViewModelDetail(AccountsModel accountsModel,ContoViewModel contoView,bool WithContragentSum,string antetka,string contr = null, bool onlyContragent = false,string kindval=null)
        {
            _movements =new List<AccItemSaldo>();
            OnlyContragent = onlyContragent;
            cv = contoView;
            this.accountsModel = accountsModel;
            this.antetka = antetka;
            this.WithContragentSum = WithContragentSum;
            var _reportItems = new List<ReportItem>();
            _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "Код", Width = 10 });
            _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "Контрагент", Width = 50 });
            _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "Номер Фактура", Width = 10 });
            _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "Дата", Width = 10 });
            // _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "НС Дебит ", Width = 15, IsSuma = true });
            _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "НС", Width = 15, IsSuma = true });
            _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "Оборот Дебит ", Width = 15, IsSuma = true });
            _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "Оборот Кредит", Width = 15, IsSuma = true });
            _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "КС", Width = 15, IsSuma = true });
            _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "Папка", Width = 15, IsSuma = false });
            _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "Номер Документ", Width = 15, IsSuma = false });
            _reportItems.Add(new ReportItem { Height = 10, IsShow = true, Name = "Основание", Width = 30, IsSuma = false });
            _reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Признак 1", Width = 10 });
            _reportItems.Add(new ReportItem { Height = 30, IsShow = true, Name = "Признак 2", Width = 10 });
            ReportItems = _reportItems;
            filter=null;
            if (contr != null)
            {
                filter = string.Format("|{0} ", contr);
            }
            typerep = 1;

           
            
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
        public ObservableCollection<InvoiseControl> AllMovementDebit1 { get; set; }
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

        public List<List<string>> GetItems()
        {
            _movements = new List<AccItemSaldo>();
            _movements1 = new List<AccItemSaldo>();
            var items = new List<List<string>>();
            var rezi = Context.GetAllAnaliticSaldos(accountsModel.Id, accountsModel.FirmaId);
            if (accountsModel.TypeAccount == 1)
            {
                AllMovementDebit = new ObservableCollection<InvoiseControl>(Context.GetFullInvoiseContoDebit(accountsModel.Id).Where(e=>e.DataInvoise<FromDate));
                AllMovementCredit = new ObservableCollection<InvoiseControl>(Context.GetFullInvoiseContoCredit(accountsModel.Id,true).Where(e=>e.DataInvoise<FromDate));
                AllMovementDebit1 = new ObservableCollection<InvoiseControl>(Context.GetFullInvoiseContoDebit(accountsModel.Id).Where(e=>e.DataInvoise>=FromDate && e.DataInvoise<=ToDate));
                AllMovementCredit1 = new ObservableCollection<InvoiseControl>(Context.GetFullInvoiseContoCredit(accountsModel.Id,true).Where(e=>e.DataInvoise>=FromDate && e.DataInvoise<=ToDate));
            }
            else
            {
                AllMovementDebit = new ObservableCollection<InvoiseControl>(Context.GetFullInvoiseContoDebit(accountsModel.Id,true).Where(e=>e.DataInvoise<FromDate));
                AllMovementCredit = new ObservableCollection<InvoiseControl>(Context.GetFullInvoiseContoCredit(accountsModel.Id).Where(e=>e.DataInvoise<FromDate));
                AllMovementDebit1 = new ObservableCollection<InvoiseControl>(Context.GetFullInvoiseContoDebit(accountsModel.Id).Where(e=>e.DataInvoise>=FromDate && e.DataInvoise<=ToDate));
                AllMovementCredit1 = new ObservableCollection<InvoiseControl>(Context.GetFullInvoiseContoCredit(accountsModel.Id, true).Where(e=>e.DataInvoise>=FromDate && e.DataInvoise<=ToDate));
            }
            foreach (InvoiseControl invoiseControl in AllMovementDebit)
            {
                var item = new AccItemSaldo();
                item.NInvoise = invoiseControl.NInvoise;
                item.NameContragent = invoiseControl.NameContragent;
                item.Code = invoiseControl.CodeContragent;
                item.Od = invoiseControl.Oborot;
                item.Type = accountsModel.TypeAccount;
                item.Folder = invoiseControl.Folder;
                item.DocNumber = invoiseControl.DocNumber;
                item.Reason = invoiseControl.Reason;
                item.Pr1 = invoiseControl.Pr1;
                item.Pr2 = invoiseControl.Pr2;
                
                if (item.Type == 1) item.Data = invoiseControl.DataInvoise;
                var lc = AllMovementCredit.FirstOrDefault(
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
                item.Oc = invoiseControl.Oborot;
                item.Type = accountsModel.TypeAccount;
                item.Data = invoiseControl.DataInvoise;
                item.Folder = invoiseControl.Folder;
                item.DocNumber = invoiseControl.DocNumber;
                item.Reason = invoiseControl.Reason;
                item.Pr1 = invoiseControl.Pr1;
                item.Pr2 = invoiseControl.Pr2;
                _movements1.Add(item);

            }
            foreach (AccItemSaldo accItemSaldo in _movements1)
            {
                var saldo =
                    rezi.FirstOrDefault(
                        m => m.Code == accItemSaldo.Code && m.NumInvoise == accItemSaldo.NInvoise);
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
                item1.Data = item.Date;
                item1.Code = item.Code;
                item1.Od = 0;
                item1.Nsd = item.BeginSaldoDebit;
                item1.Nsc = item.BeginSaldoCredit;
                item1.Type = accountsModel.TypeAccount;
                _movements1.Add(item1);
            }

            foreach (InvoiseControl invoiseControl in AllMovementDebit1)
            {
                var item = new AccItemSaldo();
                item.NInvoise = invoiseControl.NInvoise;
                item.NameContragent = invoiseControl.NameContragent;
                item.Code = invoiseControl.CodeContragent;
                item.Od = invoiseControl.Oborot;
                item.Type = accountsModel.TypeAccount;
                item.Folder = invoiseControl.Folder;
                item.DocNumber = invoiseControl.DocNumber;
                item.Reason = invoiseControl.Reason;
                item.Pr1 = invoiseControl.Pr1;
                item.Pr2 = invoiseControl.Pr2;
                item.Data = invoiseControl.DataInvoise;
                //if (item.Type == 1) 
                //var lc = AllMovementCredit1.FirstOrDefault(
                //        w => w.CodeContragent == invoiseControl.CodeContragent && w.NInvoise == invoiseControl.NInvoise);
                //if (lc != null)
                //{

                //    item.Oc += lc.Oborot;
                //    if (item.Type == 2) item.Data = lc.DataInvoise;
                //    AllMovementCredit1.Remove(lc);
                //}
                _movements.Add(item);

            }
            foreach (InvoiseControl invoiseControl in AllMovementCredit1)
            {
                var item = new AccItemSaldo();
                item.NInvoise = invoiseControl.NInvoise;
                item.NameContragent = invoiseControl.NameContragent;
                item.Code = invoiseControl.CodeContragent;
                item.Oc = invoiseControl.Oborot;
                item.Type = accountsModel.TypeAccount;
                item.Data = invoiseControl.DataInvoise;
                item.Folder = invoiseControl.Folder;
                item.DocNumber = invoiseControl.DocNumber;
                item.Reason = invoiseControl.Reason;
                item.Pr1 = invoiseControl.Pr1;
                item.Pr2 = invoiseControl.Pr2;
                _movements.Add(item);

            }
            foreach (AccItemSaldo accItemSaldo in _movements)
            {
                var saldo =
                    _movements1.FirstOrDefault(
                        m => m.Code == accItemSaldo.Code && m.DocNumber == accItemSaldo.NInvoise);
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
                item1.Code = item.Code;
                item1.Od = 0;
                item1.Nsd = item.Ksd;
                item1.Nsc = item.Ksc;
                item1.Type = accountsModel.TypeAccount;
                item1.Data = item.Data;
                _movements.Add(item1);
            }
            //AccSaldo = Context.GetSaldoAcc(accountsModel.Id);
            //Period = string.Format("От дата {0} до дата {1}",
            //                       new DateTime(ConfigTempoSinglenton.GetInstance().WorkDate.Year,
            //                                    1, 1).ToShortDateString(),
            //                       new DateTime(ConfigTempoSinglenton.GetInstance().WorkDate.Year,
            //                                    1, 1).AddYears(1).AddDays(-1).ToShortDateString());
            //AccInfo = accountsModel.ShortName;
            //AllMovement = new ObservableCollection<AccItemSaldo>(_movements);
            //}
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
            string numinvoice = "";
            string folder ="";
            string reason = "";
            string docnum = "";
            string pr1 = "";
            string pr2 = "";
            bool first = true;

            decimal sumansc = 0;
            decimal sumaOc = 0;
            decimal sumansd = 0;
            decimal sumaOd = 0;
            decimal sumansct = 0;
            decimal sumaOct = 0;
            decimal sumansdt = 0;
            decimal sumaOdt = 0;
            foreach (AccItemSaldo itemSaldo in _movements.OrderBy(m => m.Cod).ThenBy(m=>m.NInvoise))
            {
                List<string> row = new List<string>();
                if (first)
                {
                    name = itemSaldo.NameContragent;
                    code = itemSaldo.Code;
                    numinvoice = itemSaldo.NInvoise;
                    first = false;
                    folder = itemSaldo.Folder;
                    reason = itemSaldo.Reason;
                    docnum = itemSaldo.DocNumber;
                    pr1 = itemSaldo.Pr1;
                    pr2 = itemSaldo.Pr2;
                }
                else
                {
                    if (code!= itemSaldo.Code && WithContragentSum)//|| numinvoice !=itemSaldo.NInvoise
                    {
                        if (!OnlyContragent) NewRow(items);
                        List<string> rowTotal = new List<string>();
                        if (!OnlyContragent)
                        {
                            rowTotal.Add("");
                            rowTotal.Add("");
                            //rowTotal.Add("");
                        }
                        else
                        {
                            rowTotal.Add(code);
                            rowTotal.Add(name);
                            rowTotal.Add(numinvoice);
                        }
                        rowTotal.Add("");
                        rowTotal.Add(" Общо :");
                        //row.Add(itemSaldo.Nsd.ToString(Vf.LevFormatUI));
                        if (accountsModel.TypeAccount != 1)
                        {
                            var ks = sumansc + sumaOc - sumansd - sumaOd;
                            var ns = sumansc - sumansd;
                            rowTotal.Add(ns.ToString(Vf.LevFormatUI));
                            rowTotal.Add(sumaOd.ToString(Vf.LevFormatUI));
                            rowTotal.Add(sumaOc.ToString(Vf.LevFormatUI));
                            rowTotal.Add(ks.ToString(Vf.LevFormatUI));
                           
                        }
                        else
                        {
                            var ks = sumansd + sumaOd - sumansc - sumaOc;
                            var ns = sumansd - sumansc;
                            rowTotal.Add(ns.ToString(Vf.LevFormatUI));
                            rowTotal.Add(sumaOd.ToString(Vf.LevFormatUI));
                            rowTotal.Add(sumaOc.ToString(Vf.LevFormatUI));
                            rowTotal.Add(ks.ToString(Vf.LevFormatUI));
                            
                        }
                        if (!OnlyContragent)
                        {
                            rowTotal.Add("");
                            rowTotal.Add("");
                            rowTotal.Add("");
                            rowTotal.Add("");
                            rowTotal.Add("");
                        }
                        else
                        {
                            rowTotal.Add(folder);
                            rowTotal.Add(docnum);
                            rowTotal.Add(reason);
                            rowTotal.Add(pr1);
                            rowTotal.Add(pr2);
                        }
                        items.Add(rowTotal);
                        if (!OnlyContragent)
                            NewRow(items);
                        sumansc = 0;
                        sumansd = 0;
                        sumaOc = 0;
                        sumaOd = 0;
                        name = itemSaldo.NameContragent;
                        code = itemSaldo.Code;
                        numinvoice = itemSaldo.NInvoise;
                        folder = itemSaldo.Folder;
                        reason = itemSaldo.Reason;
                        docnum = itemSaldo.DocNumber;
                        pr1 = itemSaldo.Pr1;
                        pr2 = itemSaldo.Pr2;
                    }
                }

                sumansc += itemSaldo.Nsc;
                sumansd += itemSaldo.Nsd;
                sumaOc += itemSaldo.Oc;
                sumaOd += itemSaldo.Od;
                sumansct += itemSaldo.Nsc;
                sumansdt += itemSaldo.Nsd;
                sumaOct += itemSaldo.Oc;
                sumaOdt += itemSaldo.Od;
                //row.Add(itemSaldo.Nsd.ToString(Vf.LevFormatUI));
                if (accountsModel.TypeAccount != 1)
                {
                    itemSaldo.Ks = itemSaldo.Nsc + itemSaldo.Oc - itemSaldo.Nsd - itemSaldo.Od;
                    itemSaldo.Ns = itemSaldo.Nsc - itemSaldo.Nsd;
                }
                else
                {
                    itemSaldo.Ks = itemSaldo.Nsd + itemSaldo.Od -itemSaldo.Nsc - itemSaldo.Oc;
                    itemSaldo.Ns = itemSaldo.Nsd - itemSaldo.Nsc;
                }
                if (!OnlyContragent)
                {
                    row.Add(itemSaldo.Code);
                    row.Add(itemSaldo.NameContragent);
                    row.Add(itemSaldo.NInvoise);
                    row.Add(string.Format("{0}.{1}.{2}",itemSaldo.Data.Day.ToZeroString(2), itemSaldo.Data.Month.ToZeroString(2), itemSaldo.Data.Year.ToZeroString(4)));
                    row.Add(itemSaldo.Ns.ToString(Vf.LevFormatUI));
                    row.Add(itemSaldo.Od.ToString(Vf.LevFormatUI));
                    row.Add(itemSaldo.Oc.ToString(Vf.LevFormatUI));
                    row.Add(itemSaldo.Ks.ToString(Vf.LevFormatUI));
                    row.Add(itemSaldo.Folder);
                    row.Add(itemSaldo.DocNumber);
                    row.Add(itemSaldo.Reason);
                    row.Add(itemSaldo.Pr1);
                    row.Add(itemSaldo.Pr2);
                    items.Add(row);
                }
                //row.Add(itemSaldo.Ksc.ToString(Vf.LevFormatUI));


            }
            if (WithContragentSum)
            {
                if (!OnlyContragent) NewRow(items);
                List<string> rowTotalLas = new List<string>();
                if (!OnlyContragent)
                {
                    rowTotalLas.Add("");
                    rowTotalLas.Add("");
                }
                else
                {
                    rowTotalLas.Add(code);
                    rowTotalLas.Add(name);
                    rowTotalLas.Add(numinvoice);
                }
                rowTotalLas.Add("");
                rowTotalLas.Add(" Общо :");
                //row.Add(itemSaldo.Nsd.ToString(Vf.LevFormatUI));
                if (accountsModel.TypeAccount != 1)
                {
                    var ks = sumansc + sumaOc - sumansd - sumaOd;
                    var ns = sumansc - sumansd;
                    rowTotalLas.Add(ns.ToString(Vf.LevFormatUI));
                    rowTotalLas.Add(sumaOd.ToString(Vf.LevFormatUI));
                    rowTotalLas.Add(sumaOc.ToString(Vf.LevFormatUI));
                    rowTotalLas.Add(ks.ToString(Vf.LevFormatUI));
                    rowTotalLas.Add(folder);
                    rowTotalLas.Add(docnum);
                    rowTotalLas.Add(reason);
                    rowTotalLas.Add(pr1);
                    rowTotalLas.Add(pr2);
                }
                else
                {
                    var ks = sumansd + sumaOd - sumansc - sumaOc;
                    var ns = sumansd - sumansc;
                    rowTotalLas.Add(ns.ToString(Vf.LevFormatUI));
                    rowTotalLas.Add(sumaOd.ToString(Vf.LevFormatUI));
                    rowTotalLas.Add(sumaOc.ToString(Vf.LevFormatUI));
                    rowTotalLas.Add(ks.ToString(Vf.LevFormatUI));
                    
                }
                if (!OnlyContragent)
                {
                    rowTotalLas.Add("");
                    rowTotalLas.Add("");
                    rowTotalLas.Add("");
                    rowTotalLas.Add("");
                    rowTotalLas.Add("");
                }
                else
                {
                    rowTotalLas.Add(folder);
                    rowTotalLas.Add(docnum);
                    rowTotalLas.Add(reason);
                    rowTotalLas.Add(pr1);
                    rowTotalLas.Add(pr2);
                }
                items.Add(rowTotalLas);
            }
            if (filter == null)
            {
                NewRow(items);
                List<string> rowTotalLast = new List<string>();
                rowTotalLast.Add("");
                rowTotalLast.Add("");
                rowTotalLast.Add("");
                rowTotalLast.Add(" Общо :");
                //row.Add(itemSaldo.Nsd.ToString(Vf.LevFormatUI));
                if (accountsModel.TypeAccount != 1)
                {
                    var ks = sumansct + sumaOct - (sumansdt + sumaOdt);
                    var ns = sumansct - (sumansdt);
                    rowTotalLast.Add(ns.ToString(Vf.LevFormatUI));
                    rowTotalLast.Add(sumaOdt.ToString(Vf.LevFormatUI));
                    rowTotalLast.Add(sumaOct.ToString(Vf.LevFormatUI));
                    rowTotalLast.Add(ks.ToString(Vf.LevFormatUI));

                }
                else
                {
                    var ks = sumansdt + sumaOdt - sumansct + sumaOct;
                    var ns = sumansdt - sumansct;
                    rowTotalLast.Add(ns.ToString(Vf.LevFormatUI));
                    rowTotalLast.Add(sumaOdt.ToString(Vf.LevFormatUI));
                    rowTotalLast.Add(sumaOct.ToString(Vf.LevFormatUI));
                    rowTotalLast.Add(ks.ToString(Vf.LevFormatUI));
                }
                items.Add(rowTotalLast);
            }
            return items;
        }

        private void NewRow(List<List<string>> items)
        {
            items.Add(new List<string> {
                            "-----------------------------------",
                            "---------------------------------------------------------------------------------------",
                            "---------------------------------------------------------------------------------------",
                            "-----------------------------------",
                            "-----------------------------------",
                            "-----------------------------------",
                            "-----------------------------------",
                            "-----------------------------------",
                            "-----------------------------------",
                            "-----------------------------------",
                            "-----------------------------------",
                            "-----------------------------------",
                            "-----------------------------------"
                             });
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
        string title;
        public string SubTitle
        {
            get
            {
                string a="разбити плащания за сметка " + this.accountsModel.ShortName;
                if (typerep == 1)
                {
                    return a + "  " + antetka;
                }
                return a;
            }
            set {
                title = value;
            }
        }
        public string Title
        {
            get; set;
        }
        public IEnumerable<ReportItem> ReportItems { get; set;}
        public bool WithContragentSum { get;  set; }
        public bool OnlyContragent { get; private set; }

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
