using System;
using Tempo2012.UI.WPF.Models;

namespace Tempo2012.UI.WPF.ViewModels.ContoManagment
{
    public class FastLookupEventArgs:EventArgs
    {
        public FastLookupEventArgs(SaldoItem item)
        {
            SaldoItem = item;
        }
        public SaldoItem SaldoItem { get; set;}
    }
}