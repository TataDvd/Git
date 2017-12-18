using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Tempo2012.EntityFramework;
using Tempo2012.UI.WPF.Views.Dnevnici;
using Tempo2012.UI.WPF.Views.ReportManager;
using Tempo2012.EntityFramework.Models;

namespace Tempo2012.UI.WPF.ViewModels.Dnevnici
{
    public class DnPurchasesViewModel:BaseViewModel
    {
        public DnPurchasesViewModel()
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
            DocGenerator.GenerateDdsPurchasesF(Context, Month, Year);
        }
        protected override void Add()
        {
            DocGenerator.GenerateDdsPurchases(Month,Year,false);
        }
    }
}
