using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using Tempo2012.EntityFramework.Models;
using Tempo2012.EntityFramework;
using System.IO;
using System.Diagnostics;
using System.Globalization;
using Tempo2012.UI.WPF.Views.Dnevnici;

namespace Tempo2012.UI.WPF.ViewModels.Deklar
{
    public class DeclarsViewModel : BaseViewModel, IDataErrorInfo
    {
        public DeclarsViewModel()
        {
            model = ConfigTempoSinglenton.GetInstance().DeclarConfig;
            model.FirmaId = ConfigTempoSinglenton.GetInstance().CurrentFirma.Id;
            var models = ConfigTempoSinglenton.GetInstance().DeclarConfigs;
            if (models != null)
            {
                var mod = models.FirstOrDefault(e => e.FirmaId == model.FirmaId);
                if (mod!=null)
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
                models=new List<DeclarConfigModel>();
                models.Add(model);
                ConfigTempoSinglenton.GetInstance().DeclarConfigs = models;
            }
            
            _year = ConfigTempoSinglenton.GetInstance().WorkDate.Year;
            _month = ConfigTempoSinglenton.GetInstance().WorkDate.Month;
            Visible = Visibility.Hidden;
        }
        private DeclarConfigModel model;
        private int _year;
        public int Year
        {
            get { return _year; }
            set { _year = value; OnPropertyChanged("Year");}
        }
        private int _month;
        private Visibility _visible;

        public int Month
        {
            get { return _month; }
            set { _month = value; OnPropertyChanged("Month");}
        }
        public Decimal Kl33 
        {
            get { return model.Kl33; }
            set { model.Kl33 = value; OnPropertyChanged("Kl33"); }
        }
        public Decimal Kl70
        {
            get { return model.Kl70; }
            set { model.Kl70 = value; OnPropertyChanged("Kl70");  }
        }
        public Decimal Kl71
        {
            get { return model.Kl71; }
            set { model.Kl71 = value; OnPropertyChanged("Kl71"); }
        }
        public Decimal Kl80
        {
            get { return model.Kl80; }
            set { model.Kl80 = value; OnPropertyChanged("Kl80"); }
        }
        public Decimal Kl81
        {
            get { return model.Kl81; }
            set { model.Kl81 = value; OnPropertyChanged("Kl81"); }
        }
        public Decimal Kl82
        {
            get { return model.Kl82; }
            set { model.Kl82 = value; OnPropertyChanged("Kl82"); }
        }
        protected override void Add()
        {
            Visible = Visibility.Visible;
            var bw = new BackgroundWorker {WorkerReportsProgress = true, WorkerSupportsCancellation = true};
            bw.DoWork += new DoWorkEventHandler(GenerateDeclarTxt);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
            bw.RunWorkerAsync();
        }

        private void GenerateDeclarTxt(object sender, DoWorkEventArgs e)
        {
            DocGenerator.GenrateDeclar(Context, Month, Year, model);
        }

        protected override void Save()
        {
            Visible = Visibility.Visible;
            var bw = new BackgroundWorker {WorkerReportsProgress = true, WorkerSupportsCancellation = true};
            bw.DoWork += new DoWorkEventHandler(SaveConfig);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
            bw.RunWorkerAsync();
            
            
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Visible = Visibility.Hidden;
        }

        private void SaveConfig(object sender, DoWorkEventArgs e1)
        {
            var models = ConfigTempoSinglenton.GetInstance().DeclarConfigs;
            if (models != null)
            {
                var mod = models.FirstOrDefault(e => e.FirmaId == model.FirmaId);
                if (mod == null)
                {
                    models.Add(model);
                }
                else
                {
                    models.Remove(mod);
                    models.Add(model);
                }
            }
            else
            {
                models = new List<DeclarConfigModel>();
                models.Add(model);
                ConfigTempoSinglenton.GetInstance().DeclarConfigs = models;
            }
            ConfigTempoSinglenton.GetInstance().SaveConfiguration();
        }

        protected override void AddNew()
        {
            Visible = Visibility.Visible;
            Visible = Visibility.Visible;
            var bw = new BackgroundWorker();
            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;
            bw.DoWork += new DoWorkEventHandler(GenDeclareFile);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
            bw.RunWorkerAsync();
         
        }

        private void GenDeclareFile(object sender, DoWorkEventArgs e)
        {
        
            DocGenerator.GenrateDeclarF(Context, Month, Year, model);
        }


        protected override void Update()
        {
            Visible = Visibility.Visible;
            DocGenerator.GenerateVies(Context, Month, Year,model);
            Visible = Visibility.Hidden;
        }
        protected override void Delete()
        { 
            Visible = Visibility.Visible;
            DocGenerator.GenerateViesF(Context, Month, Year, model);
            Visible = Visibility.Hidden;
        }

        public string Error
        {
            get { return "Грешни данни";}
        }

        public string this[string columnName]
        {
            get {
                switch (columnName)
                {
                    case "Kl33":
                        {
                            return Validate(Kl33);
                        }
                        break;
                }
                return null;
            }
        }

        private string Validate(decimal k)
        {
            if (k>=1)
            {
                return "Коефициента трябва да е по-малък от 1";
            }
            return null;
        }
        protected override bool CanSave()
        {
            return IsValid;
        }

        protected override bool CanAdd()
        {
            return IsValid;
        }

        protected override bool CanAddNew()
        {
            return IsValid;
        }

        public bool IsValid
        {
            get
            {
                if (Validate(Kl33) != null)
                        return false;

                return true;
            }
        }

        public Visibility Visible
        {
            get { return _visible; }
            set { _visible = value; OnPropertyChanged("Visible"); }
        }
    }
}
