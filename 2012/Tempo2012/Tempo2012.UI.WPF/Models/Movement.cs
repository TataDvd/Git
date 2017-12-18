using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tempo2012.UI.WPF.Models
{
    public class Movement
    {
        public virtual DateTime Data { get; set;}
        public virtual string Description { get; set;}
        public virtual decimal Slk { get; set;}
        public virtual decimal Sld { get; set;}
        public virtual decimal Slvk { get; set; }
        public virtual decimal Slvd { get; set; }
        public virtual decimal kold { get; set; }
        public virtual decimal kolk { get; set; }
    }
}
