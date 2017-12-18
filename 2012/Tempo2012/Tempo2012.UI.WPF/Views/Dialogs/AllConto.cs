using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.Models;

namespace Tempo2012.UI.WPF.Views.Dialogs
{
    [Serializable]
    [XmlRoot("ContoAll")]
    [XmlInclude(typeof(Conto))]
    [XmlInclude(typeof(SaldoItem))]
    public class ContoAll
    {
        public Conto Conto { get; set; }

        [XmlArray("ItemsDebit")]
        [XmlArrayItem("SaldoItem")]
        public List<SaldoItem> ItemsDebit { get; set; }

        [XmlArray("ItemsCredit")]
        [XmlArrayItem("SaldoItem")]
        public List<SaldoItem> ItemsCredit { get; set; }



        public string KindDeal { get; set; }
        public string KindDds { get; set; }
        public string CountActions { get; set; }

        public bool IsDdsInclude { get; set; }
        public string NameClient { get; internal set; }
        public bool Sborno { get; internal set; }
    }
}
