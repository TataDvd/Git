using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tempo2012.EntityFramework.Models
{
    public class AccSaldo
    {
        public decimal Bsd { get; set; }
        public decimal Od { get; set; }
        public decimal Ksd { get; set; }
        public decimal Bsc { get; set; }
        public decimal Oc { get; set; }
        public decimal Ksc { get; set; }
    }
    public class SaldosModel
    {
        public int Id { get; set; }
        public int LookUpId { get; set; }
        public int ElementId { get; set; }
        public string Name { get; set; }
        public string Nom { get; set; }
        public decimal SaldoDebit { get; set; }
        public decimal SaldoCredit { get; set; }
        public decimal SaldoValutaDebit { get; set; }
        public decimal SaldoValutaCredit { get; set; }
    }
}
