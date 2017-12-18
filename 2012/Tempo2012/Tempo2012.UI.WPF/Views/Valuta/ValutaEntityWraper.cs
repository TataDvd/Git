using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Tempo2012.EntityFramework;
using Tempo2012.UI.WPF.ViewModels;

namespace Tempo2012.UI.WPF.Views.Valuta
{
    public class ValutaEntityWraper:BaseViewModel
    {
        public ValutaEntityWraper(ValutaEntity v)
        {
            _codeVal = v.CodeVal;
            _date = v.Date;
            _value = v.Value;
            _state = v.State;
            Codes = new ObservableCollection<string>();
        }
        private string _codeVal;
        public string CodeVal
        {
            get { return _codeVal; }
            set { _codeVal = value;OnPropertyChanged("CodeVal"); }
        }

        private DateTime _date;
        public DateTime Date
        {
            get { return _date; }
            set { _date = value; OnPropertyChanged("Date");}
        }

        private decimal _value;
        public decimal Value
        {
            get { return _value; }
            set { _value = value;OnPropertyChanged("Value");}
        }

        private int _state;
        public int State
        {
            get { return _state; }
            set
            {
                _state = value;
                OnPropertyChanged("State");
            }
        }


        public ObservableCollection<string> Codes { get; set; }
    }
}
