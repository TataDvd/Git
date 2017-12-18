using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tempo2012.EntityFramework.Models
{
    public class LookUpSpecific
    {
        public int Id{ get; set; }
        public virtual string Name { get; set;}
        public virtual string CodetId{get;set;}
        public virtual int TypeAcc { get; set;} 
        public override string ToString()
        {
            return string.Format("{0}-{1}", CodetId, Name);
        }
    }
}
