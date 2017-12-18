using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tempo2012.UI.WPF.Models
{
    public class DdsDnevnikModel
    {
        public virtual string DocId { get; set;}
        public virtual DateTime Date { get; set; }
        public virtual int KindActivity { get; set;}
        public virtual int KindDoc { get; set;}
        public virtual int LookupID { get; set;}
        public virtual int LookupElementID { get; set;}
    }
}
