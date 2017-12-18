using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tempo2012.UI.WPF.Models
{
    public class AccountsModel
    {
        public virtual int Id { get; set;}
        public virtual int Num { get; set;}
        public virtual int SubNum { get; set; }
        public virtual int AnaliticalNum { get; set;}
        public virtual int PartidNum { get; set; }
        public virtual string NameMain { get; set;}
        public virtual string NameMainEng { get; set;}
        public virtual string NameSub { get; set; }
        public virtual string NameSubEng { get; set; }
        public virtual string NameSub { get; set; }
        public virtual string NameSubEng { get; set; }
    }
}
