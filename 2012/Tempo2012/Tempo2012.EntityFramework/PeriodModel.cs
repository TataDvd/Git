using System;

namespace Tempo2012.EntityFramework
{
    [Serializable]
    public class PeriodModel
    {
        public DateTime Fr { get; set;}
        public DateTime To { get; set; }

        public int Holding { get; set;}
        public int Firma { get; set; }
        public override string ToString()
        {
            return string.Format("От {0} До {1}", Fr.ToShortDateString(), To.ToShortDateString());
        }
    }
}