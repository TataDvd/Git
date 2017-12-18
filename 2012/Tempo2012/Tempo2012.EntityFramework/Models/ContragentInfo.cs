using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tempo2012.EntityFramework.Models
{
    public class ContragentInfo
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Bulstad { get; set;}
        public virtual string Nzdds { get; set; }
        public virtual string ExtraInfo { get; set;}
    }
}
