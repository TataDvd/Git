using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tempo2012.UI.WPF.Events
{
    public class ChangeValutaEventArgs:EventArgs
    {
        public ChangeValutaEventArgs(decimal valuta)
        {
            _valuta = valuta;
        }
        private decimal _valuta;

        public decimal Valuta
        {
            get { return _valuta; }
            set { _valuta = value; }
        }
    }
}
