using System;

namespace Tempo2012.EntityFramework
{
    public class SaldoFactura
    {
        public string NumInvoise { get; set; }
        public string NameContragent { get; set; }
        public decimal BeginSaldoCredit { get; set; }
        public decimal BeginSaldoDebit { get; set; }
        public decimal BeginSaldoCreditValuta { get; set; }
        public decimal BeginSaldoDebitValuta { get; set; }
        public decimal BeginSaldoCreditKol { get; set; }
        public decimal BeginSaldoDebitKol { get; set; }
        public int LookupId { get; set; }
        public string Code { get; set; }
        public string Details { get; set; }
        public SaldoFactura Clone()
        {
            return (SaldoFactura)MemberwiseClone();
        }
        public DateTime Date { get; set;}
        public string NameMaterial { get; set; }
        public string CodeMaterial { get; set; }
    }
}