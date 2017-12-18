using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tempo2012.UI.WPF.ViewModels;

namespace Tempo2012.UI.WPF.Views.Valuta
{
    public class ValutaAddViewMode:BaseViewModel
    {
        public ValutaAddViewMode(DateTime date, string CodeValuata)
        {
            this._date = date;
            this._codeValuta = CodeValuata;
        }

        private DateTime _date;
        public DateTime Date
        {
            get { return _date; }
            set { _date = value;OnPropertyChanged("Date"); }
        }

        private string _codeValuta;
        public string CodeValuta
        {
            get { return _codeValuta; }
            set { _codeValuta = value;OnPropertyChanged("CodeValuta"); }
        }

        private decimal _kurs;
        public decimal Kurs
        {
            get { return _kurs; }
            set { _kurs = value;OnPropertyChanged("Kurs"); }
        }

        protected override void Add()
        {
            Context.SaveKurs(Date,CodeValuta,Kurs);
        }
    }
}
