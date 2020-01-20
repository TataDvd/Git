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

namespace Tempo2012.UI.WPF.Views
{
    public class BackUpViewModel : BaseViewModel
    {



        private BackgroundWorker bw;
        private int currentProgress;
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
        public Visibility Visible { get; private set; }
        internal void Start()
        {

        }

        public BackUpViewModel()
            : base()
        {
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
            MessageBoxWrapper.Show(string.Format("Създаден е архив TEMPO2012_{0}_{1}_{2}_{3}_{4}.FDB", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,DateTime.Now.Hour,DateTime.Now.Minute));
        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            CurrentProgress = e.ProgressPercentage;
        }

        private void DoCopy(object sender, DoWorkEventArgs e)
        {
            
            FileSystem.FileCopy(Path.Combine(ConfigTempoSinglenton.GetInstance().BaseDbPath, "H" + ConfigTempoSinglenton.GetInstance().ActiveHolding.ToString(), "TEMPO2012.FDB"),
                                Path.Combine(ConfigTempoSinglenton.GetInstance().BaseDbPath, "H" + ConfigTempoSinglenton.GetInstance().ActiveHolding.ToString(), string.Format("TEMPO2012_{0}_{1}_{2}_{3}_{4}.FDB", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,DateTime.Now.Hour,DateTime.Now.Minute)));


            
        }
    }
} 
