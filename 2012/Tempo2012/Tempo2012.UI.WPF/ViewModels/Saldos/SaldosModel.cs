using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tempo2012.UI.WPF.ViewModels.Saldos
{
    public class SaldosModel
    {
        public int Id { get; set;}
        public int LookUpId { get; set;}
        public int ElementId { get; set;}
        public string Name { get; set;}
        public float Saldo { get; set;}
        public float SaldoV { get; set;}
    }
}
