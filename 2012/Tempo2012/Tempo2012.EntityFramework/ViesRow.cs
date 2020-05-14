using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tempo2012.EntityFramework
{
    public class ViesRow
    {
        public int PorNom { get; set; }
        public string Name { get; set; }

        public decimal K3 { get; set; }
        public decimal K4 { get; set; }
        public decimal K5 { get; set; }
    }

    public class ViesRowG
    {
        public int NomRow { get; set;}
        public int Period { get; set; }
        public string VIN { get; set; }
        public int KOD { get; set; }
        public string VINDest { get; set; }
        public string PeriodOP { get; set; }
    }
}
