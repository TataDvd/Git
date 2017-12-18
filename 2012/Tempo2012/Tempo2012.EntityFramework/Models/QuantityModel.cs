using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tempo2012.EntityFramework.Models
{
    public class QuantityModel
    {
        public string Id { get; set; }
        public string DocNum { get; set; }
        public string Data { get; set; }
        public string KindVal { get; set; }
        public string Accontant { get; set; }
        public string Object { get; set; }
        public decimal Oborot { get; set; }
        public decimal Quantity { get; set; }
        public decimal SinglePrice { get; set; }
        public string Reason { get; set; }
        public string Note { get; set; }
        public QuantityModel Clone()
        {
            return (QuantityModel)this.MemberwiseClone();
        }

        public bool IsDebit { get; set; }

        public string Folder { get; set; }

        public int DebitAccount { get; set; }

        public int CreditAccount { get; set; }

        public string User { get; set; }

        public string PorNom { get; set; }
        public string StockCode { get; set; }
        public string Stock { get; set; }
        public string CodeStorage { get;  set; }
        public string Storage { get; set; }
    }
}
