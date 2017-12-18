using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using Tempo2012.EntityFramework;
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.ViewModels;

namespace Tempo2012.UI.WPF.Views.Dnevnici
{
    public class DdsAllModel:BaseViewModel
    {
        public DdsAllModel()
        {
            _year = ConfigTempoSinglenton.GetInstance().WorkDate.Year; 
            _month = ConfigTempoSinglenton.GetInstance().WorkDate.Month;
            _visible = Visibility.Hidden;
        }
        private int _year;
        public int Year
        {
            get { return _year; }
            set { _year = value; OnPropertyChanged("Year"); }
        }
        private int _month;
        private Visibility _visible;

        public int Month
        {
            get { return _month; }
            set { _month = value; OnPropertyChanged("Month"); }
        }

        internal void GenreteDocAsync(string kinddoc)
        {
            KindDoc = kinddoc;
            Visible = Visibility.Visible;
            var bw = new BackgroundWorker();
            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;
            bw.DoWork += new DoWorkEventHandler(GeneneDoc);
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
           
        }

        private void GeneneDoc(object sender, DoWorkEventArgs e)
        {
            GenreteDoc1(KindDoc);
        }
           
        internal void GenreteDoc1(string kinddoc)
        {
            Visible = Visibility.Visible;
            switch (kinddoc)
            {
                case "PURCHASES":
                    {
                        GeneratePurchases();
                    }
                    break;
                case "PURCHASESF":
                    {
                        GeneratePurchasesF();
                    }
                    break;
                case "SALES":
                    {
                        GenerateSales();
                    }
                    break;
                case "SALESF":
                    {
                        GenerateSalesF();
                    }
                    break;
                case "DECLAR":
                    {
                        GenerateDeclar();
                    }
                    break;
                case "DECLARF":
                    {
                        GenerateDeclarF();
                    }
                    break;
                case "VIES":
                    {
                        GenerateVies();
                    }
                    break;
                case "VIESF":
                    {
                        GenerateViesF();
                    }
                    break;
                case "ALL":
                    {
                        GenerateAll();
                    }
                    break;  
                case "ALLF":
                    {
                       GenerateAllF();
                    }
                    break;
            }
            Visible = Visibility.Hidden;
        }

        internal void GenreteDocSync()
        {
            GenerateSales();
            GeneratePurchases();
            GenerateDeclar();
            GenerateVies();
        }

        private void GenerateDeclar()
        {
            DocGenerator.GenrateDeclar(Context,Month,Year,ConfigTempoSinglenton.GetInstance().DeclarConfig);
        }

       
        private void GenerateVies()
        {
             DocGenerator.GenerateVies(Context,Month,Year,ConfigTempoSinglenton.GetInstance().DeclarConfig);
        }

        private void GenerateSalesF()
        {
           DocGenerator.GenerateDdsSalesF(Context,Month,Year);
        }

        private void GeneratePurchases()
        {
            DocGenerator.GenerateDdsPurchases(Month,Year,true);
   
        }

        private void GenerateAllF()
        {
            GenrateSalesF();
            GeneratePurchasesF();
            GenerateDeclarF();
            GenerateViesF();
            
        }

        private void GenerateViesF()
        {
            List<DeclarConfigModel> models;
            var model = DeclarConfigModel(out models);
            DocGenerator.GenerateViesF(Context,Month,Year,model);
        }

        private void GenerateDeclarF()
        {
            List<DeclarConfigModel> models;
            var model = DeclarConfigModel(out models);
            DocGenerator.GenrateDeclarF(Context, Month, Year, model);

        }

        private static DeclarConfigModel DeclarConfigModel(out List<DeclarConfigModel> models)
        {
            var model = ConfigTempoSinglenton.GetInstance().DeclarConfig;
            models = ConfigTempoSinglenton.GetInstance().DeclarConfigs;
            if (models != null)
            {
                var mod = models.FirstOrDefault(e => e.FirmaId == model.FirmaId);
                if (mod != null)
                {
                    model = mod;
                }
                else
                {
                    models.Add(model);
                }
            }
            else
            {
                models = new List<DeclarConfigModel>();
                models.Add(model);
                ConfigTempoSinglenton.GetInstance().DeclarConfigs = models;
            }
            return model;
        }

        private void GeneratePurchasesF()
        {
            DocGenerator.GenerateDdsPurchasesF(Context,Month,Year);
        }

        private void GenrateSalesF()
        {
            DocGenerator.GenerateDdsSalesF(Context,Month,Year);
        }

        private void GenerateAll()
        {
            GenerateSales();
            GeneratePurchases();
            GenerateDeclar();
            GenerateVies();
        }

        private void GenerateSales()
        {
           DocGenerator.GenerateDdsSales(Month,Year,true);
        }
        public Visibility Visible
        {
            get { return _visible; }
            set { _visible = value; OnPropertyChanged("Visible"); }
        }

        public string KindDoc { get; set; }
    }
}