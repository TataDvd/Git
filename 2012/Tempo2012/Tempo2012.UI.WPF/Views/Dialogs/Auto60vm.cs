using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Tempo2012.EntityFramework;
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.ViewModels;

namespace Tempo2012.UI.WPF.Views.Dialogs
{
    public class Auto60vm : BaseViewModel
    {
        public ICommand StartAutoCommand { get; private set; }
        public Auto60vm()
           : base()
        {
            StartAutoCommand = new DelegateCommand((o) => StartAuto(), (o) => this.CanStartAuto());
            Allacc = new List<AccountsModel>(Context.GetAllAccounts(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id));
            Data = new DateTime(ConfigTempoSinglenton.GetInstance().WorkDate.Year,12,31);
            Visible = System.Windows.Visibility.Hidden;
            ExDebit = "650";
        }
        private List<AccountsModel> Allacc;
        private bool CanStartAuto()
        {
            return !string.IsNullOrWhiteSpace(DocId) && !string.IsNullOrWhiteSpace(Folder) && !string.IsNullOrWhiteSpace(Debit); 
        }
        public System.Windows.Visibility Visible
        {
            get { return _visible; }
            set { _visible = value; OnPropertyChanged("Visible"); }
        }

        private BackgroundWorker bw;

        private void StartAuto1()
        {
            AccountsModel debi = LoadAcc(Debit);
            AccountsModel exdi = LoadAcc(ExDebit);
            if (debi == null)
            {
                MessageBoxWrapper.Show("Невярна дебитна сметка");
                return;
            }
            if (exdi == null)
            {
                MessageBoxWrapper.Show("Невярна изключваща сметка");
                return;
            }
            var AllAccounts = new List<AccountsModel>(Context.GetAllAccounts(ConfigTempoSinglenton.GetInstance().ActiveFirma));
            ContoAll c = new ContoAll();
            c.Conto = new Conto
            {
                DocNum = DocId,
                FirmId = Entrence.CurrentFirma.Id,
                UserId = Entrence.UserId
            };
            List<List<string>> rez = Context.GetOborotnaVed(new DateTime(ConfigTempoSinglenton.GetInstance().WorkDate.Year, 1, 1), new DateTime(ConfigTempoSinglenton.GetInstance().WorkDate.Year, 12, 31), true);
            rez = rez.Where(e => e[0].StartsWith("6")).ToList();
            int i = 0;
            _rep = "";
            foreach (var item in rez)
            {
                AccountsModel am = LoadAcc(item[0]);
                if (am == null || am.Id == exdi.Id) continue;
                c.Conto.Folder = Folder;
                c.Conto.Reason = "Приключване на сметка" + item[0];
                c.Conto.DocNum = DocId;
                c.Conto.Data = Data;
                c.Conto.DataInvoise = Data;
                c.Conto.CreditAccount = am.Id;
                c.Conto.DebitAccount = debi.Id;
                c.Conto.Oborot = decimal.Parse(item[6]);
                if (c.Conto.Oborot == 0) continue;
                Context.SaveConto(c.Conto, new List<SaldoAnaliticModel>(), new List<SaldoAnaliticModel>());
                _rep = string.Format("{0}{1}  {2}\n",_rep,c.Conto.Reason,c.Conto.Oborot); 
                bw.ReportProgress(i++);
            }
        }
        private void StartAuto()
        {
            Visible = System.Windows.Visibility.Visible;
            bw = new BackgroundWorker();
            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;
            bw.DoWork += new DoWorkEventHandler(bw_dowork);
            bw.ProgressChanged += new ProgressChangedEventHandler(bw_progresschanged);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
            bw.RunWorkerAsync();
           
        }

        private void bw_progresschanged(object sender, ProgressChangedEventArgs e)
        {
            OnPropertyChanged("Rep");
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Visible = System.Windows.Visibility.Hidden;
            MessageBoxWrapper.Show("Приключването е готово");
        }

        private void bw_dowork(object sender, DoWorkEventArgs e)
        {
            StartAuto1();
        }

       

        private AccountsModel LoadAcc(string v)
        {
            AccountsModel CurrenAcc=null;
            if (!v.Contains("/"))
            {
                int num;
                if (int.TryParse(v, out num))
                {
                    CurrenAcc = Allacc.FirstOrDefault(e => e.Num == num && e.SubNum == 0);
                }

            }
            else
            {
                int num, subnum;
                var ac = v.Split('/');

                if (int.TryParse(ac[0], out num) && int.TryParse(ac[1], out subnum))
                {
                    CurrenAcc = Allacc.FirstOrDefault(e => e.Num == num && e.SubNum == subnum);
                }
            }
            return CurrenAcc;
        }
        string _debit;
        public string Debit
        {
            get
            {

                return _debit;
            }
            set
            {
                _debit= value;
                OnPropertyChanged("Debit");

            }
        }
        string _exdebit;
        public string ExDebit
        {
            get
            {

                return _exdebit;
            }
            set
            {
                _exdebit = value;
                OnPropertyChanged("ExDebit");

            }
        }
        string _docId;
        public string DocId
        {
            get
            {
                
                return _docId;
            }
            set
            {
                _docId = value;
                OnPropertyChanged("DocId");

            }
        }
        string _folder;
        private DateTime _data;
        private Visibility _visible;
        

        public string Folder
        {
            get
            {
                return _folder;
            }
            set
            {
                _folder = value;
                OnPropertyChanged("Folder");
    
            }
        }
        public DateTime Data {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
                OnPropertyChanged("Data");
            }
        }
        private string _rep;
        public string Rep {
            get
            {
                return _rep;
            }
            set
            {
                _rep = value;
                OnPropertyChanged("Rep");
            }
        }
    }
}
