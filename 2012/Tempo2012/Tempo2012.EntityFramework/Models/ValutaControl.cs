namespace Tempo2012.EntityFramework.Models
{
    public class ValutaControl
    {
        public string Id { get; set; }
        public string DocNum { get; set; }
        public string Data { get; set; }
        public string KindVal { get; set; }
        public string Accontant { get; set; }
        public string Object { get; set; }
        public decimal Oborot { get; set; }
        public decimal ValSum { get; set; }
        public decimal Kurs  { get; set; }
        public decimal MainKurs { get; set; }
        public decimal KursDif { get; set; }
        public string Reason { get; set; }
        public string Note { get; set; }

        public string Pr1 { get; set; }
        public string Pr2 { get; set; }
        public ValutaControl Clone()
        {
            return (ValutaControl)this.MemberwiseClone();
        }

        public bool IsDebit { get; set; }

        public string Folder { get; set; }

        public int DebitAccount { get; set; }

        public int CreditAccount { get; set; }

        public string User { get; set; }

        public string PorNom { get; set; }
        public string ClienCode { get; set; }
        public string NameClient { get;  set; }
        public string NInvoice { get;  set; }
        public string DInvoce { get;  set; }
        
    }
}