using System;

namespace Tempo2012.EntityFramework.Models
{
    public class AccItemSaldo
    {
        public string NInvoise { get; set; }
        public string NameContragent { get; set; }
        public string Code { get { return code; } set { code = value; int c; if (int.TryParse(value, out c)) { cod = c; } } }
        public int Cod
        {
            get { return cod; }

        }
        public decimal Nsd { get; set;}
        public decimal Nsc { get; set;}
        public decimal Od { get; set; }
        public decimal Oc { get; set; }

        private decimal ksd;
        public decimal Ksd
        {
            set
            {
                ksd = value;
            }
            get
            {
                if (Type==1)
                {
                    decimal rez = Nsd + Od - Nsc- Oc;
                    ksd=rez;
                   
                }
                return ksd;
            }

        }

        private decimal _ksc;
        private int cod;
        private string code;

        public decimal Ksc
        {
            get
            {
                if (Type == 2)
                {
                    decimal rez = Nsc + Oc - Nsd - Od;
                     _ksc=rez;
                    
                }
                return _ksc;
            }
            set
            {
                
                _ksc = value;
            }
        }
        public decimal Col { get; set;}
        public decimal EdC { get; set;}
        public int Type { get; set; }
        public bool IsDebit { get; set; }

        public DateTime Data { get; set;}
        public string Details { get; set; }
        public decimal Ns { get; set; }
        public decimal Ks { get; set; }
        public string Folder { get; set; }
        public string DocNumber { get; set; }
        public string Reason { get; set; }

        public AccItemSaldo Clone()
        {
            return (AccItemSaldo)this.MemberwiseClone();
        }
    }
}