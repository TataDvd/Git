using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            Data = DateTime.Now;
        }
        private List<AccountsModel> Allacc;
        private bool CanStartAuto()
        {
            return !string.IsNullOrWhiteSpace(DocId) && !string.IsNullOrWhiteSpace(Folder) && !string.IsNullOrWhiteSpace(Debit); 
        }

        private void StartAuto()
        {
            AccountsModel debi = LoadAcc(Debit);
            if (debi == null)
            {
                MessageBoxWrapper.Show("Невярна дебитна сметка");
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
            foreach (var item in rez)
            {
                AccountsModel am = LoadAcc(item[0]);
                if (am == null || am.Num==650) continue;
                c.Conto.Folder = Folder;
                c.Conto.Reason = "Автоматично приключване на сметка" + item[0];
                c.Conto.DocNum = DocId;
                c.Conto.Data = Data;
                c.Conto.DataInvoise = Data;
                c.Conto.CreditAccount = am.Id;
                c.Conto.DebitAccount = debi.Id;
                c.Conto.Oborot = decimal.Parse(item[6]);
                Context.SaveConto(c.Conto, new List<SaldoAnaliticModel>(), new List<SaldoAnaliticModel>());
            }
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
    }
}
