using System;

namespace Tempo2012.EntityFramework.Models
{
    public class SaldoAnaliticModel
    { 
        public int ACCID{get;set;} 
        public int ACCFIELDKEY{get;set;}
        public int LOOKUPFIELDKEY{get;set;} 
        public DateTime DATA{get;set;}
        public string VAL{get;set;}
        public DateTime VALUEDATE{get;set;} 
        public decimal VALUEMONEY{get;set;}
        public decimal VALUENUM{get;set;}
        public long TYPEACCKEY {get; set;}
        private decimal vald;
        public decimal VALUED
        {
            get { return vald; }
            set
            {
                vald = value;
                if (value != 0)
                {
                    VALUEMONEY = 0;
                }
            }
        }
        public int GROUP { get; set;}
        public long ID { get; set;}
        public string Name { get; set;}
        public string DBField { get; set;}
        public string NAMEENG { get; set;}
        public int LENGTH { get; set;}
        public int ISNULL { get; set;}
        public int LOOKUPID { get; set;}
        public byte TYPE { get; set;}
        public int CONTOID { get; set; }
        public bool Required { get; set;}
        public decimal VALKOLK { get; set;}
        public decimal VALKOLD { get; set;}
        public decimal VALVALK { get; set; }
        public decimal VALVALD { get; set; }
        public string RFIELDNAME { get; set; }
        public string RTABLENAME { get; set; }
        public string RFIELDKEY { get; set; }
        public decimal KURS { get; set; }
        public decimal KURSD {get; set; }
        public decimal KURSM {get; set; }
        public decimal VALVAL {get; set; }
        public bool IsKol { get; set; }
        public string VALS { get; set;}
        public bool IsValutna { get; set; }
        public string LOOKUPVAL { get; set;}

        public int SORTORDER { get; set; }
    }
}