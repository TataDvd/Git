using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tempo2012.UI.WPF.Models
{
    public class ChangeKindCurrencyArg:EventArgs
    {
        public ChangeKindCurrencyArg(string kc)
        {
            KindCurrency = kc;
        }
        public string KindCurrency { get; set;}
    }
}
