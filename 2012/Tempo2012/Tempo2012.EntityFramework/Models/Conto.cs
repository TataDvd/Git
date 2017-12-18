using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Tempo2012.EntityFramework.Models
{
    [Serializable]
    public class Conto
    {
        //base data
        public virtual DateTime Data { get; set;}
        public virtual int Id {get;set;}
        public virtual decimal Oborot { get; set; }
        public virtual decimal OborotValutaD { get; set; }
        public virtual decimal OborotKolD { get; set; }
        public virtual decimal OborotValutaK { get; set; }
        public virtual decimal OborotKolK { get; set; }
        public virtual string DocNum { get; set; }
        public virtual string Folder{get;set;}
        public virtual string Reason { get;set;}
        public virtual string Note { get; set; }
        public virtual DateTime DataInvoise{get;set;}
        public virtual int NumberObject{get;set;}
        //conect data
        public virtual int DebitAccount { get; set;}
        public virtual int CreditAccount{get;set;}
        public virtual int FirmId{get;set;}
        public virtual int DocumentId{ get; set;}
        public virtual int CartotekaDebit { get; set;}
        public virtual int CartotecaCredit { get; set;}
        public virtual long Nd { get; set; }
        public virtual long Page { get; set; }
        public Conto  Clone()
        {
 	        return (Conto)this.MemberwiseClone();
        }
        public virtual string CName{ get; set; }
        public virtual string DName{ get; set; }

        //public virtual string KindActivity { get; set; }

        public virtual int TotalCound { get; set; }

        public virtual string DDetails { get; set; }

        public virtual string CDetails { get; set; }

        public virtual int IsDdsPurchasesIncluded { get; set;}
        public virtual int IsDdsSalesIncluded { get; set;}
        public virtual int IsDdsPurchases { get; set;}
        public virtual int IsDdsSales { get; set;}
        public virtual string VopPurchases { get; set;}
        public virtual string VopSales { get; set; }

        public virtual int IsPurchases { get; set;}
        public virtual int IsSales { get; set;}
        public virtual int UserId { get; set; }

        public string Pr2 { get; set; }
        public string Pr1 { get; set; }

        public string KD { get; set; }
        
        public string Contragent { get; set;}
        public string Vat { get; set;}
        public string NomInvoise { get; set; }
        public string KindDoc { get; set; }
        public decimal Sum { get; set; }
        public decimal SumDds { get; set; }

        public string DataInvoiseDnev { get; set; }
        public string ClientNumDds { get; internal set; }
    }

    [Serializable]
    public class CartotecaDebit
    {
        public virtual int Id{get;set;}
        public virtual int ContoId{get;set;}
        public virtual string TitleValue{get;set;}
        public virtual string TypeValue{get;set;}
        public virtual string Value{get;set;}
    
        public CartotecaCredit  Clone()
        {
 	        return (CartotecaCredit) this.MemberwiseClone();
        }
    }

    [Serializable]
    public class CartotecaCredit
    {
        public virtual int Id { get; set; }
        public virtual int ContoId { get; set; }
        public virtual string TitleValue { get; set; }
        public virtual string TypeValue { get; set; }
        public virtual string Value { get; set; }

        public CartotecaCredit Clone()
        {
            return (CartotecaCredit)this.MemberwiseClone();
        }
    }
}
