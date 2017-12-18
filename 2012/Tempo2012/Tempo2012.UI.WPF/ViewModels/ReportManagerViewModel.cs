using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using ReportBuilder;
using Tempo2012.UI.WPF.Views.ReportManager;
using Microsoft.Win32;
using Tempo2012.EntityFramework;

namespace Tempo2012.UI.WPF.ViewModels
{
    public class ReportManagerViewModel : BaseViewModel
    {
        private IReportBuilder builder;
        public ReportManagerViewModel(IReportBuilder builder)
        {

            this.builder = builder;
            _reportItems = new ObservableCollection<ReportItem>(builder.ReportItems);
            _visible = Visibility.Hidden;
            _enablerep = true;
        }
        protected override void Add()
        {
            Visible = Visibility.Visible;
            var bw = new BackgroundWorker();
            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;
            bw.DoWork += new DoWorkEventHandler(StartHtml);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
            bw.RunWorkerAsync();

        }

        private void StartHtml(object sender, DoWorkEventArgs e)
        {
            ReportBuilderGenerator.CreateWorkbookHtml(builder);
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Visible = Visibility.Hidden;
            EnableReport = true;
        }

        protected override void AddNew()
        {
            EnableReport = false;
            Visible = Visibility.Visible;
            var bw = new BackgroundWorker();
            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;
            bw.DoWork += new DoWorkEventHandler(StartTxt);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
            bw.RunWorkerAsync();
        }

        public void StartTxt(object sender, DoWorkEventArgs e)
        {
            ReportBuilderGenerator.CreateWorkbookTxt(builder);
        }


        protected override void Delete()
        {
            EnableReport = false;
            Visible = Visibility.Visible;
            var bw = new BackgroundWorker();
            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;
            bw.DoWork += new DoWorkEventHandler(StartCsv);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
            bw.RunWorkerAsync();
        }

        public void StartCsv(object sender, DoWorkEventArgs e)
        {
            ReportBuilderGenerator.CreateWorkbookCsv(builder);
        }

        protected override void Report()
        {
            EnableReport = false;
            Visible = Visibility.Visible;
            var bw = new BackgroundWorker();
            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;
            bw.DoWork += new DoWorkEventHandler(StartExcel);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
            bw.RunWorkerAsync();

        }

        public void StartExcel(object sender, DoWorkEventArgs e)
        {
            ReportBuilderGenerator.CreateWorkbook(builder);
        }

        private ObservableCollection<ReportItem> _reportItems;
        private Visibility _visible;
        private bool _enablerep;

        public ObservableCollection<ReportItem> ReportItems
        {
            get { return _reportItems; }
            set { _reportItems = value; OnPropertyChanged("ReportItems"); }
        }
        public Visibility Visible
        {
            get { return _visible; }
            set { _visible = value; OnPropertyChanged("Visible"); }
        }

        public bool EnableReport
        {
            get { return _enablerep; }
            set { _enablerep = value; OnPropertyChanged("EnableReport"); }
        }

        public DateTime FromDate
        {
            get
            {
                return builder.FromDate;
            }

            set
            {
                builder.FromDate = value;
                OnPropertyChanged("FromDate");
            }
        }

        public DateTime ToDate
        {
            get
            {
                return builder.ToDate;

            }

            set
            {
                builder.ToDate = value;
                OnPropertyChanged("ToDate");
            }

        }
        protected override void Update()
        {
            ReportMenuProviderView reportMenuProvider = new ReportMenuProviderView();
            reportMenuProvider.ShowDialog();
            if (reportMenuProvider.DialogResult.HasValue && reportMenuProvider.DialogResult.Value)
            {
                FromDate = reportMenuProvider.Vm.FromDate();
                ToDate = reportMenuProvider.Vm.ToDate();
            }
        }

        protected override void MoveNext()
        {
            // Create OpenFileDialog 
            SaveFileDialog dlg = new SaveFileDialog();



            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".conf";


            // Display OpenFileDialog by calling ShowDialog method 
            bool? result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                try
                {
                    builder.SaveSettings(dlg.FileName);
                }

                catch (Exception e)
                {
                    MessageBoxWrapper.Show(e.Message);
                }

            }
        }

       

        protected override void MovePrevius()
        {
            // Create OpenFileDialog 
            OpenFileDialog dlg = new OpenFileDialog();



            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".conf";


            // Display OpenFileDialog by calling ShowDialog method 
            bool? result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                try
                {
                    builder.LoadSettings(dlg.FileName);
                    _reportItems = new ObservableCollection<ReportItem>(builder.ReportItems);
                }
                catch (Exception e)
                {
                    MessageBoxWrapper.Show(e.Message);
                }
                OnPropertyChanged("ReportItems");
            }
        }
    }
}
