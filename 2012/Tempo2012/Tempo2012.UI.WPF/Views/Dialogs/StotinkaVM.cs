using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Tempo2012.EntityFramework;
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.ViewModels;
using Tempo2012.UI.WPF.Extentions;
using System.Linq;
using Tempo2012.UI.WPF.Views.Dialogs;
using Tempo2012.UI.WPF.Models;
using Tempo2012.UI.WPF.ViewModels.Dnevnici;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.Collections.ObjectModel;

namespace Tempo2012.UI.WPF.Views
{
    public class StotinkaVM : BaseViewModel
    {
        private const double V = 0.05;
        private BackgroundWorker bw;
        private int currentProgress;
        private Visibility _visible;
        private AccountsModel accountsModel;
        private decimal _oborot = (decimal)V;

        public int CurrentProgress
        {
            get { return currentProgress; }
            private set
            {
                if (currentProgress != value)
                {
                    currentProgress = value;
                    OnPropertyChanged("CurrentProgress");
                }
            }
        }
        public ICommand StartImportCommand { get; private set; }
        public decimal Oborot
        {
            get { return _oborot; }
            set
            {
                _oborot = value;
                OnPropertyChanged("Oborot");
            }
        }
        public Visibility Visible
        {
            get { return _visible; }
            set { _visible = value; OnPropertyChanged("Visible"); }
        }
        internal void Start()
        {

        }

        public StotinkaVM()
            : base()
        {
            StartImportCommand = new DelegateCommand((o) => this.StartImport(), (o) => this.CanStartImport());
            Visible = Visibility.Hidden;

        }

        public StotinkaVM(AccountsModel accountsModel) : base()
        {
            this.accountsModel = accountsModel;
            StartImportCommand = new DelegateCommand((o) => this.StartImport(), (o) => this.CanStartImport());
            Visible = Visibility.Hidden;
        }

        private bool CanStartImport()
        {
            return true;
        }

        private void StartImport()
        {
            Visible = Visibility.Visible;
            bw = new BackgroundWorker();
            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;
            bw.DoWork += new DoWorkEventHandler(DoCopy);
            bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
            bw.RunWorkerAsync();
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            Visible = Visibility.Hidden;
            MessageBoxWrapper.Show(string.Format("Край"));
        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            CurrentProgress = e.ProgressPercentage;
        }

        private void DoCopy(object sender, DoWorkEventArgs e)
        {

            //int group = 0;
            
            var AllSaldoss = Context.GetAllMovementsDetailraz(accountsModel.Id);
            var Controlnosaldo = Context.GetAllMovementsDetailControl(accountsModel.Id);
            var worsaldos = new List<SaldoAnaliticModel>(AllSaldoss.Where(ee => ee.Name == "Сума лв." && ee.VALUED < this.Oborot && ee.VALUED > (-1) * this.Oborot ));
            var razbichenisalda = new List<SaldoAnaliticModel>();
            foreach (var item in worsaldos)
            {
                Context.DeleteMovement(accountsModel.Id, item.GROUP);
                razbichenisalda.AddRange(AllSaldoss.Where(ee=>ee.GROUP==item.GROUP));
            }
            var sortaz=new List<SaldoAnaliticModel>(razbichenisalda.Where(ee => ee.Name == "Контрагент").OrderBy(ee=>ee.VAL));

            int skok = razbichenisalda.Count / worsaldos.Count;
            string workkod = "-1";
            string oldkod = "-1";
            decimal sumadebit = 0;
            decimal sumacredit = 0;
            bool opa = false;
            var workforka = new List<SaldoAnaliticModel>();
            var onodeniforki = new List<SaldoAnaliticModel>();
            foreach (var item in sortaz)
            {
                if (workkod == "-1")
                {
                    workkod = item.VAL;
                    workforka = new List<SaldoAnaliticModel>(AllSaldoss.Where(ee => ee.GROUP == item.GROUP));
                }
                var forka=new List<SaldoAnaliticModel>(AllSaldoss.Where(ee => ee.GROUP == item.GROUP));
                foreach (var it in forka)
                {
                    if (it.Name == "Контрагент")
                    {
                        if (it.VAL != workkod)
                        {
                            opa = true;
                            oldkod = workkod;
                            workkod = it.VAL;
                        }
                    }
                }
                if (!opa)
                {
                    foreach (var it in forka)
                    {
                        if (it.Name == "Сума лв.")
                        {
                            sumadebit += it.VALUED;
                            sumacredit += it.VALUEMONEY;
                        }
                    }
                }
                if (opa)
                {
                    foreach (var it in workforka)
                    {
                        if (it.Name == "Номер фактура")
                        {
                            it.VAL = "0";
                        }
                        if (it.Name == "Сума лв.")
                        {
                            var nuleva = Controlnosaldo.FirstOrDefault(r => r.Code == oldkod && r.Invoise == "0");
                            if (nuleva != null)
                            {
                                if (nuleva.SaldoDebit < this.Oborot && nuleva.SaldoDebit > (-1) * this.Oborot)
                                {
                                    sumadebit += 0;
                                    sumacredit+= 0;
                                }
                                else
                                {
                                    sumadebit += nuleva.SaldoDebit;
                                    sumacredit += nuleva.SaldoCredit;
                                }
                                Context.DeleteMovement(accountsModel.Id, nuleva.Group);
                            }
                            it.VALUED = sumadebit;
                            it.VALUEMONEY = sumacredit;
                            
                        }
                    }
                    onodeniforki.AddRange(workforka);
                    sumadebit = 0;
                    sumacredit = 0;
                    foreach (var it in forka)
                    {
                        if (it.Name == "Сума лв.")
                        {
                            sumadebit += it.VALUED;
                            sumacredit += it.VALUEMONEY;
                        }
                    }
                    workforka = new List<SaldoAnaliticModel>(forka);
                    opa = false;
                }
            }
            foreach (var it in workforka)
            {
                if (it.Name == "Номер фактура")
                {
                    it.VAL = "0";
                }
                if (it.Name == "Сума лв.")
                {
                    it.VALUED = sumadebit;
                    it.VALUEMONEY = sumacredit;
                }
            }
            onodeniforki.AddRange(workforka);
            foreach (var item in onodeniforki)
            {
                Context.SaveMovement(item);
            }
        }
    }
} 
