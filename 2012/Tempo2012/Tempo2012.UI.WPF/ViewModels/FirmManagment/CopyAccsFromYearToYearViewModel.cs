using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Tempo2012.EntityFramework;
using Tempo2012.EntityFramework.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Tempo2012.UI.WPF.ViewModels.FirmManagment;


namespace Tempo2012.UI.WPF.ViewModels
{
    
    public class CopyAccsFromYearToYearViewModel:BaseViewModel
    {
        public bool IsCalculateSaldo { get { return isCalculateSaldo; } set { isCalculateSaldo = value; OnPropertyChanged("IsCalculateSaldo"); }}
        public bool IsCopySmetkoplan { get { return isCopySmetkoplan; } set { isCopySmetkoplan = value; OnPropertyChanged("IsCopySmetkoplan"); } }
        public bool IsCalculateSaldoDetail { get { return isCalculateSaldoDetail; } set { isCalculateSaldoDetail = value; OnPropertyChanged("IsCalculateSaldoDetail"); } }

        public bool IsCalculateSaldoDetailZero { get { return isCalculateSaldoDetailZero; } set { isCalculateSaldoDetailZero = value; OnPropertyChanged("IsCalculateSaldoDetailZero"); } }
        public bool ExcludeTotalZero { get { return excludeTotalZero; } set { excludeTotalZero = value; OnPropertyChanged("ExcludeTotalZero"); } }

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
        public CopyAccsFromYearToYearViewModel()
            : base()
        {
            CopyCommand = new DelegateCommand((o) => this.CopyAccFromYtoY(), (o) => this.CanCopyAccFromYtoY());
            Visible = Visibility.Hidden;
            _fromYear = ConfigTempoSinglenton.GetInstance().WorkDate.Year;
            _toYear = _fromYear + 1;
            isCalculateSaldo = true;
            isCalculateSaldoDetail = true;
            isCopySmetkoplan = true;
            isCalculateSaldoDetailZero = false;
            excludeTotalZero = false;
        }

        private bool CanCopyAccFromYtoY()
        {
            return (ToYear ==FromYear+1 );
        }

        private void CopyAccFromYtoY()
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
        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            CurrentProgress = e.ProgressPercentage;
        }

        private void DoCopy(object sender, DoWorkEventArgs e)
        {
            Context.CopyAccFromYtoY(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id, FromYear, ToYear,IsCopySmetkoplan,IsCalculateSaldo,IsCalculateSaldoDetail,IsCalculateSaldoDetailZero, ExcludeTotalZero, bw);
        }

        

        public ICommand CopyCommand { get; private set;}

        private int _toYear;
        public int ToYear
        {
            get { return _toYear; }
            set { _toYear = value;OnPropertyChanged("ToYear"); }
        }

        private int _fromYear;
        private Visibility _visible;
        private bool isCalculateSaldo;
        private bool isCopySmetkoplan;
        private bool isCalculateSaldoDetail;
        private bool isCalculateSaldoDetailZero;
        private bool excludeTotalZero;
        private BackgroundWorker bw;
        private int currentProgress;

        public int FromYear
        {
            get { return _fromYear; }
            set { _fromYear = value;OnPropertyChanged("FromYear"); }
        }

        public Visibility Visible
        {
            get { return _visible; }
            set { _visible = value; OnPropertyChanged("Visible"); }
        }
    }
}
