using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Tempo2012.EntityFramework.Models;

namespace Tempo2012.UI.WPF.ViewModels.Saldos
{
    public class SaldosViewModel:BaseViewModel
    {
        public SaldosViewModel():base()
        {
          
        }

        public SaldosViewModel(AccountsModel accounts)
            : base()
        {
            this.Acc = accounts;
            Title = string.Format("Салда по сметка {0}", accounts);
        }




        public string Title { get; set; }
        public AccountsModel Acc { get; set; }

        public decimal SumaLvD
        {
            get { return Acc.SaldoDL; }
            set
            {
                 Acc.SaldoDL= value;
                 Acc.SaldoKL = 0;
                 OnPropertyChanged("SumaLvD");
                 OnPropertyChanged("SumaLvK");
            }
        }

        public decimal SumaLvK
        {
            get { return Acc.SaldoKL; }
            set
            {
                Acc.SaldoKL = value;
                Acc.SaldoDL=0;
                OnPropertyChanged("SumaLvK");
                OnPropertyChanged("SumaLvD");
            }
        }

        public decimal SumaVD
        {
            get { return Acc.SaldoDV; }
            set
            {
                 Acc.SaldoDV= value;
                 Acc.SaldoKV = 0;
                 OnPropertyChanged("SumaVD");
                 OnPropertyChanged("SumaVK");
            }
        }

        public decimal SumaVK
        {
            get { return Acc.SaldoKV; }
            set
            {
                Acc.SaldoKV = value;
                Acc.SaldoDV = 0;
                OnPropertyChanged("SumaVK");
                OnPropertyChanged("SumaVD");
            }
        }

        public decimal SumaKD
        {
            get { return Acc.SaldoDK; }
            set
            {
                Acc.SaldoDK = value;
                Acc.SaldoKK = 0;
                OnPropertyChanged("SumaKD");
                OnPropertyChanged("SumaKK");
            }
        }

        public decimal SumaKK
        {
            get { return Acc.SaldoKK; }
            set
            {
                Acc.SaldoKK = value;
                Acc.SaldoDK = 0;
                OnPropertyChanged("SumaKK");
                OnPropertyChanged("SumaKD");
            }
        }

        protected override void Save()
        {
            string message;
            Context.UpdateAccount(Acc, false, null, out message);
        }

       
    }
}
