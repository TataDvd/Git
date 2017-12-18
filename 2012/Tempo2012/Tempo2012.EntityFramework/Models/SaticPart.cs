using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tempo2012.EntityFramework.Models
{
    public class SaticPart
    {
        public virtual string Name { get; set; }
        public virtual string Value { get; set; }
        public virtual string Type { get; set; }
        public virtual int Code { get; set; }
    }
}
