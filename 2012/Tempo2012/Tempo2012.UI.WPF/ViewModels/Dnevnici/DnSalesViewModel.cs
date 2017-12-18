using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tempo2012.EntityFramework.Models;
using System.Diagnostics;
using Tempo2012.EntityFramework;
using System.IO;
using Tempo2012.UI.WPF.Views.Dnevnici;
using Tempo2012.UI.WPF.Views.ReportManager;

namespace Tempo2012.UI.WPF.ViewModels.Dnevnici
{
    public class DnSalesViewModel:BaseViewModel
    {
        public DnSalesViewModel()
        {
            _year = DateTime.Now.Year;
            _month = DateTime.Now.Month;
        }
        private int _year;
        public int Year
        {
            get { return _year; }
            set { _year = value; OnPropertyChanged("Year"); }
        }
        private int _month;
        public int Month
        {
            get { return _month; }
            set { _month = value; OnPropertyChanged("Month"); }
        }
        protected override void AddNew()
        {
            DocGenerator.GenerateDdsSalesF(Context,Month,Year);
        }
        protected override void Save()
        {
            ConfigTempoSinglenton.GetInstance().SaveConfiguration();
        }
        protected override void Add()
        {
           DocGenerator.GenerateDdsSales(Month,Year,false);
        }
    }
}
