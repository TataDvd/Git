﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tempo2012.EntityFramework.Models
{
    public class InvoiseControl
    {
        public string Id { get; set;}
        public decimal Oborot { get; set;}
        public decimal OborotValuta{ get; set; }
        
        public string VidValCode { get; set; }
        public string VidVal { get; set; }
        public decimal KolDebit { get; set; }
        public decimal KolCredit { get; set; }
        public string VidKol { get; set; }
        public DateTime DataInvoise { get; set;}
        public DateTime DataConto { get; set; }
        public int LOOKUPFIELDKEY { get; set; }
        public int LOOKUPID{get; set; }
        public int CKEY { get; set; }
        public int CID { get; set; }
        public string VALUE{get; set; }
        public string NameField{get; set; }
        public string VALUEDATE{get; set;}
        public string Details { get; set;}
        public string NInvoise { get; set;}
        public string NameContragent { get; set; }
        public string CodeContragent { get; set; }
        public InvoiseControl Clone()
        {
            return (InvoiseControl)this.MemberwiseClone();
        }

        public bool IsDebit { get; set; }
        public string Folder { get;  set; }
        public string DocNumber { get; set; }
        public string Reason { get; set; }

        public string Pr1 { get; set; }
        public string Pr2 { get; set; }
    }
}
