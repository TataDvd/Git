using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tempo2012.EntityFramework.Models
{
    public class LookUpElement
    {
        public virtual string Name { get; set;}
        public virtual string Description { get; set;}
        public virtual int Code{get;set;}
    }
}
