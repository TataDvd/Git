using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tempo2012.EntityFramework
{
    public class ValutaEntity
    {
        public ValutaEntity()
        {
            Date = DateTime.Now;
            CodeVal = "eur";
            Value = 1.99532m;
        }

        public DateTime Date { get; set; }
        public Decimal Value { get; set; }
        public String CodeVal { get; set; }
        public int State { get; set; }
    }
}
